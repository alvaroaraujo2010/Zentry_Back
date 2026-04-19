using System.Text.Json;
using Zentry.Application.Common;
using Zentry.Application.DTOs.Tenants;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Application.Interfaces.Security;
using Zentry.Domain.Entities;
using Zentry.Domain.Enums;

namespace Zentry.Application.Services;

public class TenantAppService : ITenantAppService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly ITenantRepository _tenants;
    private readonly ICurrentUserService _current;

    public TenantAppService(ITenantRepository tenants, ICurrentUserService current)
    {
        _tenants = tenants;
        _current = current;
    }

    public async Task<ApiResponse<List<TenantDto>>> ListAsync(CancellationToken cancellationToken = default)
    {
        var items = await _tenants.ListAsync(cancellationToken);

        return ApiResponse<List<TenantDto>>.Success(items.Select(x => new TenantDto
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            LegalName = x.LegalName,
            OwnerName = x.OwnerName,
            OwnerEmail = x.OwnerEmail,
            IsActive = x.IsActive
        }).ToList());
    }

    public async Task<ApiResponse<TenantDto>> CreateAsync(CreateTenantRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Code))
            return ApiResponse<TenantDto>.Fail("El código es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Name))
            return ApiResponse<TenantDto>.Fail("El nombre es obligatorio.");

        var existing = await _tenants.GetByCodeAsync(request.Code.Trim(), cancellationToken);
        if (existing is not null)
            return ApiResponse<TenantDto>.Fail("Ya existe un tenant con ese código.");

        var entity = new Tenant
        {
            Id = Guid.NewGuid(),
            Code = request.Code.Trim().ToUpperInvariant(),
            Name = request.Name.Trim(),
            LegalName = request.LegalName,
            OwnerName = request.OwnerName,
            OwnerEmail = request.OwnerEmail,
            BusinessType = BusinessType.BARBERSHOP,
            PlanCode = PlanCode.BASIC,
            Timezone = "America/Bogota",
            CurrencyCode = "COP",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _tenants.AddAsync(entity, cancellationToken);
        await _tenants.SaveChangesAsync(cancellationToken);

        return ApiResponse<TenantDto>.Success(new TenantDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            LegalName = entity.LegalName,
            OwnerName = entity.OwnerName,
            OwnerEmail = entity.OwnerEmail,
            IsActive = entity.IsActive
        }, "Tenant creado");
    }

    public async Task<ApiResponse<TenantBrandingDto>> GetCurrentBrandingAsync(CancellationToken cancellationToken = default)
    {
        if (!_current.TenantId.HasValue)
            return ApiResponse<TenantBrandingDto>.Fail("Tenant no encontrado.");

        var tenant = await _tenants.GetByIdAsync(_current.TenantId.Value, cancellationToken);
        if (tenant is null)
            return ApiResponse<TenantBrandingDto>.Fail("Tenant no encontrado.");

        return ApiResponse<TenantBrandingDto>.Success(MapBranding(tenant));
    }

    public async Task<ApiResponse<TenantBrandingDto>> UpdateCurrentBrandingAsync(UpdateTenantBrandingRequest request, CancellationToken cancellationToken = default)
    {
        if (!_current.TenantId.HasValue)
            return ApiResponse<TenantBrandingDto>.Fail("Tenant no encontrado.");

        var tenant = await _tenants.GetByIdAsync(_current.TenantId.Value, cancellationToken);
        if (tenant is null)
            return ApiResponse<TenantBrandingDto>.Fail("Tenant no encontrado.");

        if (!string.IsNullOrWhiteSpace(request.BusinessName))
        {
            tenant.Name = request.BusinessName.Trim();
        }

        var settings = ReadSettings(tenant.SettingsJson);
        settings.ThemePreset = Normalize(request.ThemePreset) ?? "ocean";
        settings.BusinessTagline = Normalize(request.BusinessTagline);
        settings.PublicHeroTitle = Normalize(request.PublicHeroTitle);
        settings.PublicHeroText = Normalize(request.PublicHeroText);
        settings.PublicHeroImageUrl = Normalize(request.PublicHeroImageUrl);
        settings.PublicServicesImageUrl = Normalize(request.PublicServicesImageUrl);
        settings.PublicContactImageUrl = Normalize(request.PublicContactImageUrl);
        settings.DashboardImageUrl = Normalize(request.DashboardImageUrl);
        settings.PublicWhatsapp = Normalize(request.PublicWhatsapp);
        settings.PublicAddress = Normalize(request.PublicAddress);
        settings.PublicHours = Normalize(request.PublicHours);
        settings.PublicEmail = Normalize(request.PublicEmail);

        tenant.SettingsJson = JsonSerializer.Serialize(settings, JsonOptions);
        tenant.UpdatedAt = DateTime.UtcNow;

        await _tenants.SaveChangesAsync(cancellationToken);
        return ApiResponse<TenantBrandingDto>.Success(MapBranding(tenant), "Branding actualizado");
    }

    public async Task<ApiResponse<TenantBrandingDto>> GetPublicBrandingAsync(string? code, CancellationToken cancellationToken = default)
    {
        Tenant? tenant = null;

        if (!string.IsNullOrWhiteSpace(code))
        {
            tenant = await _tenants.GetByCodeAsync(code.Trim().ToUpperInvariant(), cancellationToken);
        }

        tenant ??= await _tenants.GetFirstActiveAsync(cancellationToken);
        if (tenant is null)
            return ApiResponse<TenantBrandingDto>.Fail("No hay negocio disponible para mostrar.");

        return ApiResponse<TenantBrandingDto>.Success(MapBranding(tenant));
    }

    private static TenantBrandingDto MapBranding(Tenant tenant)
    {
        var settings = ReadSettings(tenant.SettingsJson);
        return new TenantBrandingDto
        {
            BusinessName = tenant.Name,
            ThemePreset = settings.ThemePreset ?? "ocean",
            BusinessTagline = settings.BusinessTagline,
            PublicHeroTitle = settings.PublicHeroTitle,
            PublicHeroText = settings.PublicHeroText,
            PublicHeroImageUrl = settings.PublicHeroImageUrl,
            PublicServicesImageUrl = settings.PublicServicesImageUrl,
            PublicContactImageUrl = settings.PublicContactImageUrl,
            DashboardImageUrl = settings.DashboardImageUrl,
            PublicWhatsapp = settings.PublicWhatsapp,
            PublicAddress = settings.PublicAddress,
            PublicHours = settings.PublicHours,
            PublicEmail = settings.PublicEmail
        };
    }

    private static TenantBrandingSettings ReadSettings(string? settingsJson)
    {
        if (string.IsNullOrWhiteSpace(settingsJson))
            return new TenantBrandingSettings();

        try
        {
            return JsonSerializer.Deserialize<TenantBrandingSettings>(settingsJson, JsonOptions) ?? new TenantBrandingSettings();
        }
        catch (JsonException)
        {
            return new TenantBrandingSettings();
        }
    }

    private static string? Normalize(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}

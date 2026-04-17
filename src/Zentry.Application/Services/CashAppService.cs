using Zentry.Application.Common;
using Zentry.Application.DTOs.Cash;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Application.Interfaces.Security;
using Zentry.Domain.Entities;

namespace Zentry.Application.Services;

public class CashAppService : ICashAppService
{
    private readonly ICashSessionRepository _cashSessions;
    private readonly ICurrentUserService _current;

    public CashAppService(ICashSessionRepository cashSessions, ICurrentUserService current)
    {
        _cashSessions = cashSessions;
        _current = current;
    }

    public async Task<ApiResponse<CashSessionDto?>> CurrentAsync(CancellationToken cancellationToken = default)
    {
        if (!_current.TenantId.HasValue || !_current.BranchId.HasValue)
            return ApiResponse<CashSessionDto?>.Success(null);

        var session = await _cashSessions.GetOpenSessionAsync(_current.TenantId.Value, _current.BranchId.Value, cancellationToken);
        return ApiResponse<CashSessionDto?>.Success(session is null ? null : MapDto(session));
    }

    public async Task<ApiResponse<List<CashSessionDto>>> ListAsync(CancellationToken cancellationToken = default)
    {
        if (!_current.TenantId.HasValue)
            return ApiResponse<List<CashSessionDto>>.Fail("Tenant no encontrado.");

        var items = await _cashSessions.ListByTenantAsync(_current.TenantId.Value, cancellationToken);
        return ApiResponse<List<CashSessionDto>>.Success(items.Select(MapDto).ToList());
    }

    public async Task<ApiResponse<CashSessionDto>> OpenAsync(CreateCashSessionRequest r, CancellationToken cancellationToken = default)
    {
        var tenantId = _current.TenantId ?? (r.TenantId == Guid.Empty ? null : r.TenantId);
        if (!tenantId.HasValue)
            return ApiResponse<CashSessionDto>.Fail("Tenant no encontrado.");
        if ((_current.UserId ?? Guid.Empty) == Guid.Empty)
            return ApiResponse<CashSessionDto>.Fail("Usuario no encontrado.");
        if (r.BranchId == Guid.Empty)
            return ApiResponse<CashSessionDto>.Fail("La sucursal es obligatoria.");
        if (r.OpeningAmount < 0)
            return ApiResponse<CashSessionDto>.Fail("El monto de apertura no puede ser negativo.");

        var existing = await _cashSessions.GetOpenSessionAsync(tenantId.Value, r.BranchId, cancellationToken);
        if (existing is not null)
            return ApiResponse<CashSessionDto>.Fail("Ya existe una caja abierta para esta sucursal.");

        var now = DateTime.UtcNow;
        var entity = new CashSession
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId.Value,
            BranchId = r.BranchId,
            OpenedByUserId = _current.UserId!.Value,
            OpenedAt = now,
            OpeningAmount = r.OpeningAmount,
            ExpectedAmount = r.OpeningAmount,
            DifferenceAmount = 0,
            Status = "OPEN",
            Notes = string.IsNullOrWhiteSpace(r.Notes) ? null : r.Notes.Trim(),
            CreatedAt = now,
            UpdatedAt = now
        };

        await _cashSessions.AddAsync(entity, cancellationToken);
        await _cashSessions.SaveChangesAsync(cancellationToken);

        return ApiResponse<CashSessionDto>.Success(MapDto(entity), "Caja abierta");
    }

    public async Task<ApiResponse<CashSessionDto>> CloseAsync(Guid id, CloseCashSessionRequest r, CancellationToken cancellationToken = default)
    {
        var entity = await _cashSessions.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return ApiResponse<CashSessionDto>.Fail("Sesion de caja no encontrada.");
        if (entity.Status != "OPEN")
            return ApiResponse<CashSessionDto>.Fail("La caja ya fue cerrada.");
        if (r.ClosingAmount < 0)
            return ApiResponse<CashSessionDto>.Fail("El monto de cierre no puede ser negativo.");

        var expectedAmount = entity.OpeningAmount;
        entity.ClosingAmount = r.ClosingAmount;
        entity.ExpectedAmount = expectedAmount;
        entity.DifferenceAmount = r.ClosingAmount - expectedAmount;
        entity.ClosedAt = DateTime.UtcNow;
        entity.ClosedByUserId = _current.UserId;
        entity.Status = "CLOSED";
        entity.Notes = string.IsNullOrWhiteSpace(r.Notes) ? entity.Notes : r.Notes.Trim();
        entity.UpdatedAt = DateTime.UtcNow;

        await _cashSessions.SaveChangesAsync(cancellationToken);

        return ApiResponse<CashSessionDto>.Success(MapDto(entity), "Caja cerrada");
    }

    private static CashSessionDto MapDto(CashSession entity)
    {
        return new CashSessionDto
        {
            Id = entity.Id,
            BranchId = entity.BranchId,
            OpeningAmount = entity.OpeningAmount,
            ClosingAmount = entity.ClosingAmount,
            ExpectedAmount = entity.ExpectedAmount,
            DifferenceAmount = entity.DifferenceAmount,
            Status = entity.Status,
            Notes = entity.Notes,
            OpenedAt = entity.OpenedAt,
            ClosedAt = entity.ClosedAt
        };
    }
}

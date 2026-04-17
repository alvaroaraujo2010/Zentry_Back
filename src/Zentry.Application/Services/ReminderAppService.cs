using Zentry.Application.Common;
using Zentry.Application.DTOs.Reminders;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Application.Interfaces.Security;
using Zentry.Domain.Entities;

namespace Zentry.Application.Services;

public class ReminderAppService : IReminderAppService
{
    private readonly IReminderRepository _reminders;
    private readonly ICurrentUserService _current;

    public ReminderAppService(IReminderRepository reminders, ICurrentUserService current)
    {
        _reminders = reminders;
        _current = current;
    }

    public async Task<ApiResponse<List<ReminderDto>>> ListAsync(CancellationToken cancellationToken = default)
    {
        if (!_current.TenantId.HasValue)
            return ApiResponse<List<ReminderDto>>.Fail("Tenant no encontrado.");

        var items = await _reminders.ListByTenantAsync(_current.TenantId.Value, cancellationToken);
        return ApiResponse<List<ReminderDto>>.Success(items.Select(MapDto).ToList());
    }

    public async Task<ApiResponse<ReminderDto>> CreateAsync(CreateReminderRequest r, CancellationToken cancellationToken = default)
    {
        var tenantId = _current.TenantId ?? (r.TenantId == Guid.Empty ? null : r.TenantId);
        if (!tenantId.HasValue)
            return ApiResponse<ReminderDto>.Fail("Tenant no encontrado.");
        if (r.AppointmentId == Guid.Empty)
            return ApiResponse<ReminderDto>.Fail("La cita es obligatoria.");
        if (r.CustomerId == Guid.Empty)
            return ApiResponse<ReminderDto>.Fail("El cliente es obligatorio.");
        if (string.IsNullOrWhiteSpace(r.Phone))
            return ApiResponse<ReminderDto>.Fail("El telefono es obligatorio.");
        if (string.IsNullOrWhiteSpace(r.Channel))
            return ApiResponse<ReminderDto>.Fail("El canal es obligatorio.");
        if (string.IsNullOrWhiteSpace(r.TemplateCode))
            return ApiResponse<ReminderDto>.Fail("La plantilla es obligatoria.");

        var scheduledFor = r.ScheduledFor == default ? DateTime.UtcNow : r.ScheduledFor;
        var entity = new ReminderQueue
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId.Value,
            AppointmentId = r.AppointmentId,
            CustomerId = r.CustomerId,
            Channel = r.Channel.Trim().ToUpperInvariant(),
            TemplateCode = r.TemplateCode.Trim(),
            Phone = r.Phone.Trim(),
            ScheduledFor = scheduledFor,
            Status = "PENDING",
            Attempts = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _reminders.AddAsync(entity, cancellationToken);
        await _reminders.SaveChangesAsync(cancellationToken);

        return ApiResponse<ReminderDto>.Success(MapDto(entity), "Recordatorio en cola");
    }

    private static ReminderDto MapDto(ReminderQueue entity)
    {
        return new ReminderDto
        {
            Id = entity.Id,
            AppointmentId = entity.AppointmentId,
            CustomerId = entity.CustomerId,
            Channel = entity.Channel,
            TemplateCode = entity.TemplateCode,
            Phone = entity.Phone,
            ScheduledFor = entity.ScheduledFor,
            Status = entity.Status,
            Attempts = entity.Attempts,
            LastError = entity.LastError,
            SentAt = entity.SentAt
        };
    }
}

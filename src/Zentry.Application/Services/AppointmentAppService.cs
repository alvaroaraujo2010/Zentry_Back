using Zentry.Application.Common;
using Zentry.Application.DTOs.Appointments;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Application.Interfaces.Security;
using Zentry.Domain.Entities;
using Zentry.Domain.Enums;

namespace Zentry.Application.Services;

public class AppointmentAppService : IAppointmentAppService
{
    private readonly IAppointmentRepository _appointments;
    private readonly ICurrentUserService _current;

    public AppointmentAppService(IAppointmentRepository appointments, ICurrentUserService current)
    {
        _appointments = appointments;
        _current = current;
    }

    public async Task<ApiResponse<List<AppointmentDto>>> ListAsync(CancellationToken cancellationToken = default)
    {
        var items = await _appointments.ListAsync(cancellationToken);
        return ApiResponse<List<AppointmentDto>>.Success(items.Select(x => new AppointmentDto { Id = x.Id, BranchId = x.BranchId, CustomerId = x.CustomerId, StaffUserId = x.StaffUserId, StartsAt = x.StartsAt, EndsAt = x.EndsAt, Notes = x.Notes, Status = x.Status.ToString(), Total = x.Total }).ToList());
    }

    public async Task<ApiResponse<AppointmentDto>> CreateAsync(CreateAppointmentRequest request, CancellationToken cancellationToken = default)
    {
        if (!_current.TenantId.HasValue) return ApiResponse<AppointmentDto>.Fail("Tenant no encontrado.");
        if (request.BranchId == Guid.Empty) return ApiResponse<AppointmentDto>.Fail("La sucursal es obligatoria.");
        if (request.CustomerId == Guid.Empty) return ApiResponse<AppointmentDto>.Fail("El cliente es obligatorio.");
        if (request.StartsAt >= request.EndsAt) return ApiResponse<AppointmentDto>.Fail("La fecha de inicio debe ser menor que la fecha fin.");
        var overlap = await _appointments.HasOverlapAsync(_current.TenantId.Value, request.StaffUserId, request.StartsAt, request.EndsAt, null, cancellationToken);
        if (overlap) return ApiResponse<AppointmentDto>.Fail("El staff ya tiene una cita en ese rango.");

        var entity = new Appointment { Id = Guid.NewGuid(), TenantId = _current.TenantId.Value, BranchId = request.BranchId, CustomerId = request.CustomerId, StaffUserId = request.StaffUserId, StartsAt = request.StartsAt, EndsAt = request.EndsAt, Status = AppointmentStatus.SCHEDULED, Source = "APP", Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim(), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        await _appointments.AddAsync(entity, cancellationToken);
        await _appointments.SaveChangesAsync(cancellationToken);
        return ApiResponse<AppointmentDto>.Success(new AppointmentDto { Id = entity.Id, BranchId = entity.BranchId, CustomerId = entity.CustomerId, StaffUserId = entity.StaffUserId, StartsAt = entity.StartsAt, EndsAt = entity.EndsAt, Notes = entity.Notes, Status = entity.Status.ToString(), Total = entity.Total }, "Cita creada");
    }

    public async Task<ApiResponse<AppointmentDto>> UpdateAsync(Guid id, UpdateAppointmentRequest request, CancellationToken cancellationToken = default)
    {
        if (!_current.TenantId.HasValue) return ApiResponse<AppointmentDto>.Fail("Tenant no encontrado.");
        var entity = await _appointments.GetByIdAsync(id, cancellationToken);
        if (entity is null) return ApiResponse<AppointmentDto>.Fail("Cita no encontrada.");
        if (request.BranchId == Guid.Empty) return ApiResponse<AppointmentDto>.Fail("La sucursal es obligatoria.");
        if (request.CustomerId == Guid.Empty) return ApiResponse<AppointmentDto>.Fail("El cliente es obligatorio.");
        if (request.StartsAt >= request.EndsAt) return ApiResponse<AppointmentDto>.Fail("La fecha de inicio debe ser menor que la fecha fin.");

        var overlap = await _appointments.HasOverlapAsync(_current.TenantId.Value, request.StaffUserId, request.StartsAt, request.EndsAt, id, cancellationToken);
        if (overlap) return ApiResponse<AppointmentDto>.Fail("El staff ya tiene una cita en ese rango.");

        entity.BranchId = request.BranchId;
        entity.CustomerId = request.CustomerId;
        entity.StaffUserId = request.StaffUserId;
        entity.StartsAt = request.StartsAt;
        entity.EndsAt = request.EndsAt;
        entity.Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim();
        entity.UpdatedAt = DateTime.UtcNow;

        await _appointments.SaveChangesAsync(cancellationToken);

        return ApiResponse<AppointmentDto>.Success(new AppointmentDto { Id = entity.Id, BranchId = entity.BranchId, CustomerId = entity.CustomerId, StaffUserId = entity.StaffUserId, StartsAt = entity.StartsAt, EndsAt = entity.EndsAt, Notes = entity.Notes, Status = entity.Status.ToString(), Total = entity.Total }, "Cita actualizada");
    }
}

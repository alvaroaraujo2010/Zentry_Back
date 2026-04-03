using Zentry.Application.Common;
using Zentry.Application.DTOs.Appointments;

namespace Zentry.Application.Services;
public interface IAppointmentAppService { Task<ApiResponse<List<AppointmentDto>>> ListAsync(CancellationToken cancellationToken = default); Task<ApiResponse<AppointmentDto>> CreateAsync(CreateAppointmentRequest request, CancellationToken cancellationToken = default); }

using Zentry.Application.Common;
using Zentry.Application.DTOs.Reminders;
namespace Zentry.Application.Services;
public interface IReminderAppService { Task<ApiResponse<List<ReminderDto>>> ListAsync(CancellationToken cancellationToken = default); Task<ApiResponse<ReminderDto>> CreateAsync(CreateReminderRequest r, CancellationToken cancellationToken = default); }

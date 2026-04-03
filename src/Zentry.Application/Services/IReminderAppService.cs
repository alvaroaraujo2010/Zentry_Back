using Zentry.Application.Common; using Zentry.Application.DTOs.Reminders;
namespace Zentry.Application.Services;
public interface IReminderAppService { Task<ApiResponse<string>> CreateAsync(CreateReminderRequest r); }
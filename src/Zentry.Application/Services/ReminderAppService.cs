using Zentry.Application.Common; using Zentry.Application.DTOs.Reminders;
namespace Zentry.Application.Services;
public class ReminderAppService:IReminderAppService{
 public Task<ApiResponse<string>> CreateAsync(CreateReminderRequest r){
  return Task.FromResult(ApiResponse<string>.Success("Reminder queued"));
 }}
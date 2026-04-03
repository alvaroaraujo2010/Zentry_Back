namespace Zentry.Domain.Entities;

public class AuditLog
{
    public long Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid? UserId { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public Guid? EntityId { get; set; }
    public string ActionCode { get; set; } = string.Empty;
    public string? OldValuesJson { get; set; }
    public string? NewValuesJson { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public DateTime CreatedAt { get; set; }
}

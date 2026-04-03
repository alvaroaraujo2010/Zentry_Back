using Zentry.Domain.Common;
using Zentry.Domain.Enums;

namespace Zentry.Domain.Entities;

public class OtpCode : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public Guid? UserId { get; set; }
    public string Phone { get; set; } = string.Empty;
    public OtpPurpose Purpose { get; set; }
    public string CodeHash { get; set; } = string.Empty;
    public int Attempts { get; set; }
    public int MaxAttempts { get; set; } = 5;
    public DateTime ExpiresAt { get; set; }
    public DateTime? UsedAt { get; set; }
}

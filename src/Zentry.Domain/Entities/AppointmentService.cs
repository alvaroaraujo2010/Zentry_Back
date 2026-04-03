using Zentry.Domain.Common;

namespace Zentry.Domain.Entities;

public class AppointmentService : BaseEntity, ITenantEntity
{
    public Guid AppointmentId { get; set; }
    public Guid TenantId { get; set; }
    public Guid ServiceId { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public int DurationMinutes { get; set; } = 30;
    public decimal LineTotal { get; set; }
}

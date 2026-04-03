using Zentry.Domain.Common;
using Zentry.Domain.Enums;

namespace Zentry.Domain.Entities;

public class Tenant : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? LegalName { get; set; }
    public string? TaxId { get; set; }
    public BusinessType BusinessType { get; set; } = BusinessType.BARBERSHOP;
    public PlanCode PlanCode { get; set; } = PlanCode.BASIC;
    public string? OwnerName { get; set; }
    public string? OwnerEmail { get; set; }
    public string? OwnerPhone { get; set; }
    public string Timezone { get; set; } = "America/Bogota";
    public string CurrencyCode { get; set; } = "COP";
    public bool IsActive { get; set; } = true;
    public string? SettingsJson { get; set; }
}

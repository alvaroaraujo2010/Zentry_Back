namespace Zentry.Application.DTOs.Cash;
public class CreateCashSessionRequest { public Guid TenantId { get; set; } public Guid BranchId { get; set; } public decimal OpeningAmount { get; set; } }
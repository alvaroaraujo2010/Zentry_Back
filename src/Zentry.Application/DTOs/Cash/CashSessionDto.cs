namespace Zentry.Application.DTOs.Cash;
public class CashSessionDto { public Guid Id { get; set; } public decimal OpeningAmount { get; set; } public string Status { get; set; } = "OPEN"; }
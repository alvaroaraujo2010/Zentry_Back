namespace Zentry.Application.DTOs.Payments;
public class PaymentDto { public Guid Id { get; set; } public decimal Amount { get; set; } public string Method { get; set; } = string.Empty; public string Status { get; set; } = string.Empty; public DateTime ReceivedAt { get; set; } }

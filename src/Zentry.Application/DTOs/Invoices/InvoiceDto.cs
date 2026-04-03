namespace Zentry.Application.DTOs.Invoices;
public class InvoiceDto { public Guid Id { get; set; } public decimal Total { get; set; } public string Status { get; set; } = ""; }
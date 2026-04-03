namespace Zentry.Application.DTOs.Customers;
public class CreateCustomerRequest { public Guid? BranchId { get; set; } public string FullName { get; set; } = string.Empty; public string? Phone { get; set; } public string? Email { get; set; } }

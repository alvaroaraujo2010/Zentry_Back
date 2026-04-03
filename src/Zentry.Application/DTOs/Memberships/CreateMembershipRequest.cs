namespace Zentry.Application.DTOs.Memberships;
public class CreateMembershipRequest { public Guid TenantId { get; set; } public string Name { get; set; } = ""; public decimal Price { get; set; } }
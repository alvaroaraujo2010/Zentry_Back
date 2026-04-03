namespace Zentry.Application.DTOs.Tenants;

public class CreateTenantRequest
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? LegalName { get; set; }
    public string? OwnerName { get; set; }
    public string? OwnerEmail { get; set; }
}
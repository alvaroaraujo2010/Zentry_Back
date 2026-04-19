namespace Zentry.Application.DTOs.Tenants;

public class UpdateTenantBrandingRequest
{
    public string? BusinessName { get; set; }
    public string? ThemePreset { get; set; }
    public string? BusinessTagline { get; set; }
    public string? PublicHeroTitle { get; set; }
    public string? PublicHeroText { get; set; }
    public string? PublicHeroImageUrl { get; set; }
    public string? PublicServicesImageUrl { get; set; }
    public string? PublicContactImageUrl { get; set; }
    public string? DashboardImageUrl { get; set; }
    public string? PublicWhatsapp { get; set; }
    public string? PublicAddress { get; set; }
    public string? PublicHours { get; set; }
    public string? PublicEmail { get; set; }
}

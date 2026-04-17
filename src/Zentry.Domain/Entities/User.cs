using Zentry.Domain.Common;
using Zentry.Domain.Enums;

namespace Zentry.Domain.Entities;

public class User : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public Guid? RoleId { get; set; }
    public Guid? BranchId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime? PhoneVerifiedAt { get; set; }
    public string? AvatarUrl { get; set; }
    public string Status { get; set; } = "ACTIVE";
    public DateTime? LastLoginAt { get; set; }
    public bool IsSupervisor { get; set; }

    public Role? RoleEntity { get; set; }
    // Agrega esta propiedad a la clase User para corregir el error CS1061
    public UserRole Role { get; set; }

}

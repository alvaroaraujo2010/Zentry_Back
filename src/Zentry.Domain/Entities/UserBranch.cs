namespace Zentry.Domain.Entities;

public class UserBranch
{
    public Guid UserId { get; set; }
    public Guid BranchId { get; set; }
    public DateTime CreatedAt { get; set; }
}

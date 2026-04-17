using Zentry.Application.Interfaces.Repositories;
using Zentry.Domain.Entities;
using Zentry.Infrastructure.Persistence;

namespace Zentry.Infrastructure.Repositories;

public class MembershipRepository : EfRepository<Membership>, IMembershipRepository
{
    public MembershipRepository(ZentryDbContext context) : base(context) { }
}

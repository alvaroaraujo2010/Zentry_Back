using Zentry.Application.Interfaces.Repositories;
using Zentry.Domain.Entities;
using Zentry.Infrastructure.Persistence;

namespace Zentry.Infrastructure.Repositories;

public class CustomerRepository : EfRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(ZentryDbContext context) : base(context) { }
}

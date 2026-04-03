using Zentry.Application.Interfaces.Repositories;
using Zentry.Domain.Entities;
using Zentry.Infrastructure.Persistence;

namespace Zentry.Infrastructure.Repositories;

public class PaymentRepository : EfRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(ZentryDbContext context) : base(context) { }
}

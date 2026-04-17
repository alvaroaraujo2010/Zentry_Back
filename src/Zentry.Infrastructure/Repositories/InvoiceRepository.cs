using Zentry.Application.Interfaces.Repositories;
using Zentry.Domain.Entities;
using Zentry.Infrastructure.Persistence;

namespace Zentry.Infrastructure.Repositories;

public class InvoiceRepository : EfRepository<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(ZentryDbContext context) : base(context) { }
}

using Microsoft.EntityFrameworkCore;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Domain.Entities;
using Zentry.Infrastructure.Persistence;

namespace Zentry.Infrastructure.Repositories;

public class AppointmentRepository : EfRepository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(ZentryDbContext context) : base(context) { }

    public async Task<bool> HasOverlapAsync(Guid tenantId, Guid? staffUserId, DateTime startAt, DateTime endAt, CancellationToken cancellationToken = default)
    {
        if (!staffUserId.HasValue) return false;
        return await Context.Appointments.AnyAsync(x => x.TenantId == tenantId && x.StaffUserId == staffUserId && x.Status != Domain.Enums.AppointmentStatus.CANCELLED && startAt < x.EndsAt && endAt > x.StartsAt, cancellationToken);
    }
}

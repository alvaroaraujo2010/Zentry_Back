using Zentry.Domain.Entities;

namespace Zentry.Application.Interfaces.Repositories;
public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<bool> HasOverlapAsync(Guid tenantId, Guid? staffUserId, DateTime startAt, DateTime endAt, Guid? ignoreAppointmentId = null, CancellationToken cancellationToken = default);
}

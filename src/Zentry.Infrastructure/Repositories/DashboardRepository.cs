using Microsoft.EntityFrameworkCore;
using Zentry.Application.DTOs.Dashboard;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Domain.Enums;
using Zentry.Infrastructure.Persistence;

namespace Zentry.Infrastructure.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly ZentryDbContext _context;

    public DashboardRepository(ZentryDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardSummaryDto?> GetSummaryAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        var tenant = await _context.Tenants
            .AsNoTracking()
            .Where(x => x.Id == tenantId)
            .Select(x => new { x.Name, x.Timezone })
            .FirstOrDefaultAsync(cancellationToken);

        if (tenant is null)
        {
            return null;
        }

        var timezone = ResolveTimezone(tenant.Timezone);
        var utcNow = DateTime.UtcNow;
        var localNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, timezone);
        var localDayStart = localNow.Date;
        var localDayEnd = localDayStart.AddDays(1);
        var dayStartUtc = TimeZoneInfo.ConvertTimeToUtc(localDayStart, timezone);
        var dayEndUtc = TimeZoneInfo.ConvertTimeToUtc(localDayEnd, timezone);

        var customersCount = await _context.Customers
            .AsNoTracking()
            .CountAsync(x => x.TenantId == tenantId, cancellationToken);

        var appointmentsToday = await _context.Appointments
            .AsNoTracking()
            .CountAsync(x =>
                x.TenantId == tenantId &&
                x.StartsAt >= dayStartUtc &&
                x.StartsAt < dayEndUtc &&
                x.Status != AppointmentStatus.CANCELLED,
                cancellationToken);

        var revenueToday = await _context.Payments
            .AsNoTracking()
            .Where(x =>
                x.TenantId == tenantId &&
                x.Status == "PAID" &&
                x.ReceivedAt >= dayStartUtc &&
                x.ReceivedAt < dayEndUtc)
            .Select(x => (decimal?)x.Amount)
            .SumAsync(cancellationToken) ?? 0m;

        var pendingInvoices = await _context.Invoices
            .AsNoTracking()
            .CountAsync(x => x.TenantId == tenantId && x.BalanceDue > 0, cancellationToken);

        var activeMemberships = await _context.Memberships
            .AsNoTracking()
            .CountAsync(x => x.TenantId == tenantId && x.IsActive, cancellationToken);

        var pendingReminders = await _context.ReminderQueue
            .AsNoTracking()
            .CountAsync(x => x.TenantId == tenantId && x.Status == "PENDING", cancellationToken);

        var openCashSessions = await _context.CashSessions
            .AsNoTracking()
            .CountAsync(x => x.TenantId == tenantId && x.Status == "OPEN", cancellationToken);

        var upcomingAppointments = await (
            from appointment in _context.Appointments.AsNoTracking()
            join customer in _context.Customers.AsNoTracking() on appointment.CustomerId equals customer.Id
            where appointment.TenantId == tenantId
                && appointment.StartsAt >= utcNow
                && appointment.Status != AppointmentStatus.CANCELLED
            orderby appointment.StartsAt
            select new DashboardUpcomingAppointmentDto
            {
                AppointmentId = appointment.Id,
                CustomerName = customer.FullName,
                StartsAt = appointment.StartsAt,
                Status = appointment.Status.ToString(),
                Total = appointment.Total
            })
            .Take(5)
            .ToListAsync(cancellationToken);

        return new DashboardSummaryDto
        {
            TenantName = tenant.Name,
            Timezone = timezone.Id,
            BusinessDate = localNow,
            CustomersCount = customersCount,
            AppointmentsToday = appointmentsToday,
            RevenueToday = revenueToday,
            PendingInvoices = pendingInvoices,
            ActiveMemberships = activeMemberships,
            PendingReminders = pendingReminders,
            OpenCashSessions = openCashSessions,
            UpcomingAppointments = upcomingAppointments
        };
    }

    private static TimeZoneInfo ResolveTimezone(string? timezoneId)
    {
        if (string.IsNullOrWhiteSpace(timezoneId))
        {
            return TimeZoneInfo.Utc;
        }

        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
        }
        catch (TimeZoneNotFoundException)
        {
            if (timezoneId.Equals("America/Bogota", StringComparison.OrdinalIgnoreCase))
            {
                return TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            }

            return TimeZoneInfo.Utc;
        }
    }
}

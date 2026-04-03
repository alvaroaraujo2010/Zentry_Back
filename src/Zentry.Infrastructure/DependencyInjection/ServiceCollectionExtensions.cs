using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Application.Interfaces.Security;
using Zentry.Application.Services;
using Zentry.Infrastructure.Persistence;
using Zentry.Infrastructure.Repositories;
using Zentry.Infrastructure.Security;

namespace Zentry.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddZentryInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? "server=localhost;port=3306;database=zentry;user=administrador;password=ingAlv4r0;";

        services.AddDbContext<ZentryDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IUserAppService, UserAppService>();
        services.AddScoped<IInvoiceAppService, InvoiceAppService>();
        services.AddScoped<IMembershipAppService, MembershipAppService>();
        services.AddScoped<ICashAppService, CashAppService>();
        services.AddScoped<IReminderAppService, ReminderAppService>();

        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICatalogServiceRepository, CatalogServiceRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        services.AddScoped<IAuthAppService, AuthAppService>();
        services.AddScoped<ICustomerAppService, CustomerAppService>();
        services.AddScoped<ICatalogServiceAppService, CatalogServiceAppService>();
        services.AddScoped<IAppointmentAppService, AppointmentAppService>();
        services.AddScoped<IPaymentAppService, PaymentAppService>();

        services.AddScoped<IStaffProfileRepository, StaffProfileRepository>();
        services.AddScoped<IStaffProfileAppService, StaffProfileAppService>();

        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<ITenantRepository, TenantRepository>();

        services.AddScoped<IRoleAppService, RoleAppService>();
        services.AddScoped<IBranchAppService, BranchAppService>();
        services.AddScoped<ITenantAppService, TenantAppService>();

        return services;
    }
}

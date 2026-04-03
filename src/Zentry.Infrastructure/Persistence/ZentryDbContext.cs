using Microsoft.EntityFrameworkCore;
using Zentry.Domain.Entities;

namespace Zentry.Infrastructure.Persistence;

public class ZentryDbContext : DbContext
{
    public ZentryDbContext(DbContextOptions<ZentryDbContext> options) : base(options) { }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserBranch> UserBranches => Set<UserBranch>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<OtpCode> OtpCodes => Set<OtpCode>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<StaffProfile> StaffProfiles => Set<StaffProfile>();
    public DbSet<CatalogService> CatalogServices => Set<CatalogService>();
    public DbSet<Membership> Memberships => Set<Membership>();
    public DbSet<CustomerMembership> CustomerMemberships => Set<CustomerMembership>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<AppointmentService> AppointmentServices => Set<AppointmentService>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<CashSession> CashSessions => Set<CashSession>();
    public DbSet<CashMovement> CashMovements => Set<CashMovement>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<ReminderQueue> ReminderQueue => Set<ReminderQueue>();
    public DbSet<WhatsappLog> WhatsappLogs => Set<WhatsappLog>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tenant>(e =>
        {
            e.ToTable("tenants");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Code).HasColumnName("code");
            e.Property(x => x.Name).HasColumnName("name");
            e.Property(x => x.LegalName).HasColumnName("legal_name");
            e.Property(x => x.TaxId).HasColumnName("tax_id");
            e.Property(x => x.BusinessType).HasColumnName("business_type").HasConversion<string>();
            e.Property(x => x.PlanCode).HasColumnName("plan_code").HasConversion<string>();
            e.Property(x => x.OwnerName).HasColumnName("owner_name");
            e.Property(x => x.OwnerEmail).HasColumnName("owner_email");
            e.Property(x => x.OwnerPhone).HasColumnName("owner_phone");
            e.Property(x => x.Timezone).HasColumnName("timezone");
            e.Property(x => x.CurrencyCode).HasColumnName("currency_code");
            e.Property(x => x.IsActive).HasColumnName("is_active");
            e.Property(x => x.SettingsJson).HasColumnName("settings");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Branch>(e =>
        {
            e.ToTable("branches");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.TenantId).HasColumnName("tenant_id");
            e.Property(x => x.Code).HasColumnName("code");
            e.Property(x => x.Name).HasColumnName("name");
            e.Property(x => x.Phone).HasColumnName("phone");
            e.Property(x => x.Email).HasColumnName("email");
            e.Property(x => x.Address).HasColumnName("address");
            e.Property(x => x.City).HasColumnName("city");
            e.Property(x => x.State).HasColumnName("state");
            e.Property(x => x.IsMain).HasColumnName("is_main");
            e.Property(x => x.OpensAt).HasColumnName("opens_at");
            e.Property(x => x.ClosesAt).HasColumnName("closes_at");
            e.Property(x => x.IsActive).HasColumnName("is_active");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Role>(e =>
        {
            e.ToTable("roles");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.TenantId).HasColumnName("tenant_id");
            e.Property(x => x.Code).HasColumnName("code");
            e.Property(x => x.Name).HasColumnName("name");
            e.Property(x => x.Description).HasColumnName("description");
            e.Property(x => x.PermissionsJson).HasColumnName("permissions");
            e.Property(x => x.IsSystem).HasColumnName("is_system");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("users");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.TenantId).HasColumnName("tenant_id");
            e.Property(x => x.RoleId).HasColumnName("role_id");
            e.Property(x => x.BranchId).HasColumnName("branch_id");
            e.Property(x => x.Email).HasColumnName("email");
            e.Property(x => x.PasswordHash).HasColumnName("password_hash");
            e.Property(x => x.FullName).HasColumnName("full_name");
            e.Property(x => x.Phone).HasColumnName("phone");
            e.Property(x => x.PhoneVerifiedAt).HasColumnName("phone_verified_at");
            e.Property(x => x.AvatarUrl).HasColumnName("avatar_url");
            e.Property(x => x.Status).HasColumnName("status");
            e.Property(x => x.LastLoginAt).HasColumnName("last_login_at");
            e.Property(x => x.IsSupervisor).HasColumnName("is_supervisor");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            e.HasOne(x => x.RoleEntity).WithMany().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<UserBranch>(e =>
        {
            e.ToTable("user_branches");
            e.HasKey(x => new { x.UserId, x.BranchId });
            e.Property(x => x.UserId).HasColumnName("user_id");
            e.Property(x => x.BranchId).HasColumnName("branch_id");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<RefreshToken>(e =>
        {
            e.ToTable("refresh_tokens");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.UserId).HasColumnName("user_id");
            e.Property(x => x.TokenHash).HasColumnName("token_hash");
            e.Property(x => x.UserAgent).HasColumnName("user_agent");
            e.Property(x => x.IpAddress).HasColumnName("ip_address");
            e.Property(x => x.ExpiresAt).HasColumnName("expires_at");
            e.Property(x => x.RevokedAt).HasColumnName("revoked_at");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<OtpCode>(e =>
        {
            e.ToTable("otp_codes");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.TenantId).HasColumnName("tenant_id");
            e.Property(x => x.UserId).HasColumnName("user_id");
            e.Property(x => x.Phone).HasColumnName("phone");
            e.Property(x => x.Purpose).HasColumnName("purpose").HasConversion<string>();
            e.Property(x => x.CodeHash).HasColumnName("code_hash");
            e.Property(x => x.Attempts).HasColumnName("attempts");
            e.Property(x => x.MaxAttempts).HasColumnName("max_attempts");
            e.Property(x => x.ExpiresAt).HasColumnName("expires_at");
            e.Property(x => x.UsedAt).HasColumnName("used_at");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<Customer>(e =>
        {
            e.ToTable("customers");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.TenantId).HasColumnName("tenant_id");
            e.Property(x => x.BranchId).HasColumnName("branch_id");
            e.Property(x => x.FullName).HasColumnName("full_name");
            e.Property(x => x.Phone).HasColumnName("phone");
            e.Property(x => x.Email).HasColumnName("email");
            e.Property(x => x.BirthDate).HasColumnName("birth_date");
            e.Property(x => x.Gender).HasColumnName("gender");
            e.Property(x => x.EmergencyContactName).HasColumnName("emergency_contact_name");
            e.Property(x => x.EmergencyContactPhone).HasColumnName("emergency_contact_phone");
            e.Property(x => x.Notes).HasColumnName("notes");
            e.Property(x => x.TagsJson).HasColumnName("tags");
            e.Property(x => x.LastVisitAt).HasColumnName("last_visit_at");
            e.Property(x => x.IsActive).HasColumnName("is_active");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<StaffProfile>(e =>
        {
            e.ToTable("staff_profiles");
            e.HasKey(x => x.UserId);
            e.Property(x => x.UserId).HasColumnName("user_id");
            e.Property(x => x.TenantId).HasColumnName("tenant_id");
            e.Property(x => x.BranchId).HasColumnName("branch_id");
            e.Property(x => x.Title).HasColumnName("title");
            e.Property(x => x.CommissionRate).HasColumnName("commission_rate");
            e.Property(x => x.CanTakeAppointments).HasColumnName("can_take_appointments");
            e.Property(x => x.WorkScheduleJson).HasColumnName("work_schedule");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<CatalogService>(e =>
        {
            e.ToTable("catalog_services");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.TenantId).HasColumnName("tenant_id");
            e.Property(x => x.BranchId).HasColumnName("branch_id");
            e.Property(x => x.Code).HasColumnName("code");
            e.Property(x => x.Name).HasColumnName("name");
            e.Property(x => x.Description).HasColumnName("description");
            e.Property(x => x.DurationMinutes).HasColumnName("duration_minutes");
            e.Property(x => x.Price).HasColumnName("price").HasPrecision(12,2);
            e.Property(x => x.Category).HasColumnName("category");
            e.Property(x => x.RequiresBooking).HasColumnName("requires_booking");
            e.Property(x => x.IsActive).HasColumnName("is_active");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Membership>(e =>
        {
            e.ToTable("memberships");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.TenantId).HasColumnName("tenant_id");
            e.Property(x => x.BranchId).HasColumnName("branch_id");
            e.Property(x => x.Code).HasColumnName("code");
            e.Property(x => x.Name).HasColumnName("name");
            e.Property(x => x.Description).HasColumnName("description");
            e.Property(x => x.Price).HasColumnName("price").HasPrecision(12,2);
            e.Property(x => x.BillingCycleDays).HasColumnName("billing_cycle_days");
            e.Property(x => x.VisitsPerCycle).HasColumnName("visits_per_cycle");
            e.Property(x => x.IsActive).HasColumnName("is_active");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<CustomerMembership>(e =>
        {
            e.ToTable("customer_memberships");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.TenantId).HasColumnName("tenant_id");
            e.Property(x => x.CustomerId).HasColumnName("customer_id");
            e.Property(x => x.MembershipId).HasColumnName("membership_id");
            e.Property(x => x.StartDate).HasColumnName("start_date");
            e.Property(x => x.EndDate).HasColumnName("end_date");
            e.Property(x => x.Status).HasColumnName("status");
            e.Property(x => x.RemainingVisits).HasColumnName("remaining_visits");
            e.Property(x => x.AutoRenew).HasColumnName("auto_renew");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Appointment>(e =>
        {
            e.ToTable("appointments");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.TenantId).HasColumnName("tenant_id");
            e.Property(x => x.BranchId).HasColumnName("branch_id");
            e.Property(x => x.CustomerId).HasColumnName("customer_id");
            e.Property(x => x.StaffUserId).HasColumnName("staff_user_id");
            e.Property(x => x.StartsAt).HasColumnName("starts_at");
            e.Property(x => x.EndsAt).HasColumnName("ends_at");
            e.Property(x => x.Status).HasColumnName("status").HasConversion<string>();
            e.Property(x => x.Source).HasColumnName("source");
            e.Property(x => x.Notes).HasColumnName("notes");
            e.Property(x => x.InternalNotes).HasColumnName("internal_notes");
            e.Property(x => x.Subtotal).HasColumnName("subtotal").HasPrecision(12,2);
            e.Property(x => x.Discount).HasColumnName("discount").HasPrecision(12,2);
            e.Property(x => x.Total).HasColumnName("total").HasPrecision(12,2);
            e.Property(x => x.BalanceDue).HasColumnName("balance_due").HasPrecision(12,2);
            e.Property(x => x.ConfirmedAt).HasColumnName("confirmed_at");
            e.Property(x => x.CancelledAt).HasColumnName("cancelled_at");
            e.Property(x => x.CompletedAt).HasColumnName("completed_at");
            e.Property(x => x.CreatedByUserId).HasColumnName("created_by_user_id");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<AppointmentService>(e =>
        {
            e.ToTable("appointment_services");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.AppointmentId).HasColumnName("appointment_id");
            e.Property(x => x.TenantId).HasColumnName("tenant_id");
            e.Property(x => x.ServiceId).HasColumnName("service_id");
            e.Property(x => x.Quantity).HasColumnName("quantity");
            e.Property(x => x.UnitPrice).HasColumnName("unit_price").HasPrecision(12,2);
            e.Property(x => x.DurationMinutes).HasColumnName("duration_minutes");
            e.Property(x => x.LineTotal).HasColumnName("line_total").HasPrecision(12,2);
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<Invoice>(e =>
        {
            e.ToTable("invoices");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.TenantId).HasColumnName("tenant_id");
            e.Property(x => x.BranchId).HasColumnName("branch_id");
            e.Property(x => x.AppointmentId).HasColumnName("appointment_id");
            e.Property(x => x.CustomerId).HasColumnName("customer_id");
            e.Property(x => x.InvoiceNumber).HasColumnName("invoice_number");
            e.Property(x => x.Subtotal).HasColumnName("subtotal").HasPrecision(12,2);
            e.Property(x => x.Tax).HasColumnName("tax").HasPrecision(12,2);
            e.Property(x => x.Discount).HasColumnName("discount").HasPrecision(12,2);
            e.Property(x => x.Total).HasColumnName("total").HasPrecision(12,2);
            e.Property(x => x.AmountPaid).HasColumnName("amount_paid").HasPrecision(12,2);
            e.Property(x => x.BalanceDue).HasColumnName("balance_due").HasPrecision(12,2);
            e.Property(x => x.PaymentStatus).HasColumnName("payment_status");
            e.Property(x => x.IssuedAt).HasColumnName("issued_at");
            e.Property(x => x.DueAt).HasColumnName("due_at");
            e.Property(x => x.Notes).HasColumnName("notes");
            e.Property(x => x.CreatedByUserId).HasColumnName("created_by_user_id");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Payment>(e =>
        {
            e.ToTable("payments");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.TenantId).HasColumnName("tenant_id");
            e.Property(x => x.BranchId).HasColumnName("branch_id");
            e.Property(x => x.InvoiceId).HasColumnName("invoice_id");
            e.Property(x => x.AppointmentId).HasColumnName("appointment_id");
            e.Property(x => x.CustomerId).HasColumnName("customer_id");
            e.Property(x => x.CashSessionId).HasColumnName("cash_session_id");
            e.Property(x => x.PaymentMethod).HasColumnName("payment_method").HasConversion<string>();
            e.Property(x => x.ReferenceCode).HasColumnName("reference_code");
            e.Property(x => x.Amount).HasColumnName("amount").HasPrecision(12,2);
            e.Property(x => x.ReceivedAt).HasColumnName("received_at");
            e.Property(x => x.Status).HasColumnName("status");
            e.Property(x => x.Notes).HasColumnName("notes");
            e.Property(x => x.CreatedByUserId).HasColumnName("created_by_user_id");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<CashSession>(e =>
        {
            e.ToTable("cash_sessions");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.TenantId).HasColumnName("tenant_id");
            e.Property(x => x.BranchId).HasColumnName("branch_id");
            e.Property(x => x.OpenedByUserId).HasColumnName("opened_by_user_id");
            e.Property(x => x.ClosedByUserId).HasColumnName("closed_by_user_id");
            e.Property(x => x.OpenedAt).HasColumnName("opened_at");
            e.Property(x => x.ClosedAt).HasColumnName("closed_at");
            e.Property(x => x.OpeningAmount).HasColumnName("opening_amount").HasPrecision(12,2);
            e.Property(x => x.ClosingAmount).HasColumnName("closing_amount").HasPrecision(12,2);
            e.Property(x => x.ExpectedAmount).HasColumnName("expected_amount").HasPrecision(12,2);
            e.Property(x => x.DifferenceAmount).HasColumnName("difference_amount").HasPrecision(12,2);
            e.Property(x => x.Status).HasColumnName("status");
            e.Property(x => x.Notes).HasColumnName("notes");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<CashMovement>(e =>
        {
            e.ToTable("cash_movements");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.CashSessionId).HasColumnName("cash_session_id");
            e.Property(x => x.TenantId).HasColumnName("tenant_id");
            e.Property(x => x.BranchId).HasColumnName("branch_id");
            e.Property(x => x.PaymentId).HasColumnName("payment_id");
            e.Property(x => x.MovementType).HasColumnName("movement_type");
            e.Property(x => x.Amount).HasColumnName("amount").HasPrecision(12,2);
            e.Property(x => x.Description).HasColumnName("description");
            e.Property(x => x.CreatedByUserId).HasColumnName("created_by_user_id");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<Subscription>(e =>
        {
            e.ToTable("subscriptions");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.TenantId).HasColumnName("tenant_id");
            e.Property(x => x.PlanCode).HasColumnName("plan_code");
            e.Property(x => x.Status).HasColumnName("status");
            e.Property(x => x.StartsAt).HasColumnName("starts_at");
            e.Property(x => x.EndsAt).HasColumnName("ends_at");
            e.Property(x => x.RenewsAt).HasColumnName("renews_at");
            e.Property(x => x.Amount).HasColumnName("amount").HasPrecision(12,2);
            e.Property(x => x.FeaturesJson).HasColumnName("features");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<ReminderQueue>(e =>
        {
            e.ToTable("reminder_queue");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.TenantId).HasColumnName("tenant_id");
            e.Property(x => x.AppointmentId).HasColumnName("appointment_id");
            e.Property(x => x.CustomerId).HasColumnName("customer_id");
            e.Property(x => x.Channel).HasColumnName("channel");
            e.Property(x => x.TemplateCode).HasColumnName("template_code");
            e.Property(x => x.Phone).HasColumnName("phone");
            e.Property(x => x.ScheduledFor).HasColumnName("scheduled_for");
            e.Property(x => x.Status).HasColumnName("status");
            e.Property(x => x.Attempts).HasColumnName("attempts");
            e.Property(x => x.LastError).HasColumnName("last_error");
            e.Property(x => x.SentAt).HasColumnName("sent_at");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<WhatsappLog>(e =>
        {
            e.ToTable("whatsapp_logs");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.TenantId).HasColumnName("tenant_id");
            e.Property(x => x.CustomerId).HasColumnName("customer_id");
            e.Property(x => x.AppointmentId).HasColumnName("appointment_id");
            e.Property(x => x.Phone).HasColumnName("phone");
            e.Property(x => x.Provider).HasColumnName("provider");
            e.Property(x => x.TemplateCode).HasColumnName("template_code");
            e.Property(x => x.PayloadJson).HasColumnName("payload");
            e.Property(x => x.ProviderMessageId).HasColumnName("provider_message_id");
            e.Property(x => x.Status).HasColumnName("status");
            e.Property(x => x.ErrorMessage).HasColumnName("error_message");
            e.Property(x => x.SentAt).HasColumnName("sent_at");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<AuditLog>(e =>
        {
            e.ToTable("audit_logs");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.TenantId).HasColumnName("tenant_id");
            e.Property(x => x.UserId).HasColumnName("user_id");
            e.Property(x => x.EntityName).HasColumnName("entity_name");
            e.Property(x => x.EntityId).HasColumnName("entity_id");
            e.Property(x => x.ActionCode).HasColumnName("action_code");
            e.Property(x => x.OldValuesJson).HasColumnName("old_values");
            e.Property(x => x.NewValuesJson).HasColumnName("new_values");
            e.Property(x => x.IpAddress).HasColumnName("ip_address");
            e.Property(x => x.UserAgent).HasColumnName("user_agent");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
        });
    }
}

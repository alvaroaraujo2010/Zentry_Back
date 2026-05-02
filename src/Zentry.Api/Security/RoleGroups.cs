namespace Zentry.Api.Security;

public static class RoleGroups
{
    public const string Owner = "OWNER";
    public const string Admin = "ADMIN";
    public const string Staff = "STAFF";
    public const string Cashier = "CASHIER";

    public const string Management = Owner + "," + Admin;
    public const string Finance = Owner + "," + Admin + "," + Cashier;
    public const string Operational = Owner + "," + Admin + "," + Staff;
    public const string TenantUsers = Owner + "," + Admin + "," + Staff + "," + Cashier;
    public const string AdminPortal = Finance;
}

using System.Collections.ObjectModel;

namespace Shared.Authorization;

public static class FshPermissions
{
    private static readonly FshPermission[] AllPermissions =
    [     
        //tenants
        new("View Tenants", FshActions.View, FshResources.Tenants, IsRoot: true),
        new("Create Tenants", FshActions.Create, FshResources.Tenants, IsRoot: true),
        new("Update Tenants", FshActions.Update, FshResources.Tenants, IsRoot: true),
        new("Upgrade Tenant Subscription", FshActions.UpgradeSubscription, FshResources.Tenants, IsRoot: true),

        //identity
        new("View Users", FshActions.View, FshResources.Users),
        new("Search Users", FshActions.Search, FshResources.Users),
        new("Create Users", FshActions.Create, FshResources.Users),
        new("Update Users", FshActions.Update, FshResources.Users),
        new("Delete Users", FshActions.Delete, FshResources.Users),
        new("Export Users", FshActions.Export, FshResources.Users),
        new("View UserRoles", FshActions.View, FshResources.UserRoles),
        new("Update UserRoles", FshActions.Update, FshResources.UserRoles),
        new("View Roles", FshActions.View, FshResources.Roles),
        new("Create Roles", FshActions.Create, FshResources.Roles),
        new("Update Roles", FshActions.Update, FshResources.Roles),
        new("Delete Roles", FshActions.Delete, FshResources.Roles),
        new("View RoleClaims", FshActions.View, FshResources.RoleClaims),
        new("Update RoleClaims", FshActions.Update, FshResources.RoleClaims),
        
        //products
        new("View Products", FshActions.View, FshResources.Products, IsBasic: true),
        new("Search Products", FshActions.Search, FshResources.Products, IsBasic: true),
        new("Create Products", FshActions.Create, FshResources.Products),
        new("Update Products", FshActions.Update, FshResources.Products),
        new("Delete Products", FshActions.Delete, FshResources.Products),
        new("Import Products", FshActions.Import, FshResources.Products),
        new("Export Products", FshActions.Export, FshResources.Products),

        //brands
        new("View Brands", FshActions.View, FshResources.Brands, IsBasic: true),
        new("Search Brands", FshActions.Search, FshResources.Brands, IsBasic: true),
        new("Create Brands", FshActions.Create, FshResources.Brands),
        new("Update Brands", FshActions.Update, FshResources.Brands),
        new("Delete Brands", FshActions.Delete, FshResources.Brands),
        new("Import Brands", FshActions.Import, FshResources.Brands),
        new("Export Brands", FshActions.Export, FshResources.Brands),

        //todos
        new("View Todos", FshActions.View, FshResources.Todos, IsBasic: true),
        new("Search Todos", FshActions.Search, FshResources.Todos, IsBasic: true),
        new("Create Todos", FshActions.Create, FshResources.Todos),
        new("Update Todos", FshActions.Update, FshResources.Todos),
        new("Delete Todos", FshActions.Delete, FshResources.Todos),
        new("Export Todos", FshActions.Export, FshResources.Todos),

         new("View Hangfire", FshActions.View, FshResources.Hangfire),
         new("View Dashboard", FshActions.View, FshResources.Dashboard),
         new("View Analytics", FshActions.View, FshResources.Analytics),

        //audit
        new("View Audit Trails", FshActions.View, FshResources.AuditTrails),
        
        //Accounting
        new("View Accounting", FshActions.View, FshResources.Accounting),
        new("Search Accounting", FshActions.Search, FshResources.Accounting),
        new("Create Accounting", FshActions.Create, FshResources.Accounting),
        new("Update Accounting", FshActions.Update, FshResources.Accounting),
        new("Delete Accounting", FshActions.Delete, FshResources.Accounting),
        new("Import Accounting", FshActions.Import, FshResources.Accounting),
        new("Export Accounting", FshActions.Export, FshResources.Accounting),
        new("Approve Accounting", FshActions.Approve, FshResources.Accounting),
        new("Reject Accounting", FshActions.Reject, FshResources.Accounting),
        new("Post Accounting", FshActions.Post, FshResources.Accounting),
        new("Void Accounting", FshActions.Void, FshResources.Accounting),
        new("Cancel Accounting", FshActions.Cancel, FshResources.Accounting),
        new("Send Accounting", FshActions.Send, FshResources.Accounting),
        new("Process Accounting", FshActions.Process, FshResources.Accounting),
        new("Complete Accounting", FshActions.Complete, FshResources.Accounting),

        //Store
        new("View Store", FshActions.View, FshResources.Store),
        new("Search Store", FshActions.Search, FshResources.Store),
        new("Create Store", FshActions.Create, FshResources.Store),
        new("Update Store", FshActions.Update, FshResources.Store),
        new("Delete Store", FshActions.Delete, FshResources.Store),
        new("Import Store", FshActions.Import, FshResources.Store),
        new("Export Store", FshActions.Export, FshResources.Store),
        
        //Warehouse
        new("View Warehouse", FshActions.View, FshResources.Warehouse),
        new("Search Warehouse", FshActions.Search, FshResources.Warehouse),
        new("Create Warehouse", FshActions.Create, FshResources.Warehouse),
        new("Update Warehouse", FshActions.Update, FshResources.Warehouse),
        new("Delete Warehouse", FshActions.Delete, FshResources.Warehouse),
        new("Import Warehouse", FshActions.Import, FshResources.Warehouse),
        new("Export Warehouse", FshActions.Export, FshResources.Warehouse),

        //Messaging
        new("View Messaging", FshActions.View, FshResources.Messaging, IsBasic: true),
        new("Search Messaging", FshActions.Search, FshResources.Messaging, IsBasic: true),
        new("Create Messaging", FshActions.Create, FshResources.Messaging, IsBasic: true),
        new("Update Messaging", FshActions.Update, FshResources.Messaging),
        new("Delete Messaging", FshActions.Delete, FshResources.Messaging),

        //Human Resources - Organization & Setup
        new("View Organization", FshActions.View, FshResources.Organization),
        new("Search Organization", FshActions.Search, FshResources.Organization),
        new("Create Organization", FshActions.Create, FshResources.Organization),
        new("Update Organization", FshActions.Update, FshResources.Organization),
        new("Delete Organization", FshActions.Delete, FshResources.Organization),
        new("Import Organization", FshActions.Import, FshResources.Organization),
        new("Export Organization", FshActions.Export, FshResources.Organization),

        //Human Resources - Employees
        new("View Employees", FshActions.View, FshResources.Employees),
        new("Search Employees", FshActions.Search, FshResources.Employees),
        new("Create Employees", FshActions.Create, FshResources.Employees),
        new("Update Employees", FshActions.Update, FshResources.Employees),
        new("Delete Employees", FshActions.Delete, FshResources.Employees),
        new("Import Employees", FshActions.Import, FshResources.Employees),
        new("Export Employees", FshActions.Export, FshResources.Employees),
        new("Manage Employees", FshActions.Manage, FshResources.Employees),
        new("Assign Employees", FshActions.Assign, FshResources.Employees),
        new("Submit Employees", FshActions.Submit, FshResources.Employees),
        new("Complete Employees", FshActions.Complete, FshResources.Employees),

        //Human Resources - Attendance
        new("View Attendance", FshActions.View, FshResources.Attendance),
        new("Search Attendance", FshActions.Search, FshResources.Attendance),
        new("Create Attendance", FshActions.Create, FshResources.Attendance),
        new("Update Attendance", FshActions.Update, FshResources.Attendance),
        new("Delete Attendance", FshActions.Delete, FshResources.Attendance),
        new("Import Attendance", FshActions.Import, FshResources.Attendance),
        new("Export Attendance", FshActions.Export, FshResources.Attendance),

        //Human Resources - Timesheets
        new("View Timesheets", FshActions.View, FshResources.Timesheets),
        new("Search Timesheets", FshActions.Search, FshResources.Timesheets),
        new("Create Timesheets", FshActions.Create, FshResources.Timesheets),
        new("Update Timesheets", FshActions.Update, FshResources.Timesheets),
        new("Delete Timesheets", FshActions.Delete, FshResources.Timesheets),
        new("Import Timesheets", FshActions.Import, FshResources.Timesheets),
        new("Export Timesheets", FshActions.Export, FshResources.Timesheets),

        //Human Resources - Leaves
        new("View Leaves", FshActions.View, FshResources.Leaves),
        new("Search Leaves", FshActions.Search, FshResources.Leaves),
        new("Create Leaves", FshActions.Create, FshResources.Leaves),
        new("Update Leaves", FshActions.Update, FshResources.Leaves),
        new("Delete Leaves", FshActions.Delete, FshResources.Leaves),
        new("Import Leaves", FshActions.Import, FshResources.Leaves),
        new("Export Leaves", FshActions.Export, FshResources.Leaves),
        new("Approve Leaves", FshActions.Approve, FshResources.Leaves),
        new("Reject Leaves", FshActions.Reject, FshResources.Leaves),
        new("Submit Leaves", FshActions.Submit, FshResources.Leaves),
        new("Cancel Leaves", FshActions.Cancel, FshResources.Leaves),

        //Human Resources - Payroll
        new("View Payroll", FshActions.View, FshResources.Payroll),
        new("Search Payroll", FshActions.Search, FshResources.Payroll),
        new("Create Payroll", FshActions.Create, FshResources.Payroll),
        new("Update Payroll", FshActions.Update, FshResources.Payroll),
        new("Delete Payroll", FshActions.Delete, FshResources.Payroll),
        new("Import Payroll", FshActions.Import, FshResources.Payroll),
        new("Export Payroll", FshActions.Export, FshResources.Payroll),
        new("Process Payroll", FshActions.Process, FshResources.Payroll),

        //Human Resources - Benefits
        new("View Benefits", FshActions.View, FshResources.Benefits),
        new("Search Benefits", FshActions.Search, FshResources.Benefits),
        new("Create Benefits", FshActions.Create, FshResources.Benefits),
        new("Update Benefits", FshActions.Update, FshResources.Benefits),
        new("Delete Benefits", FshActions.Delete, FshResources.Benefits),
        new("Import Benefits", FshActions.Import, FshResources.Benefits),
        new("Export Benefits", FshActions.Export, FshResources.Benefits),
        new("Approve Benefits", FshActions.Approve, FshResources.Benefits),
        new("Reject Benefits", FshActions.Reject, FshResources.Benefits),

        // Employees - special operations
        new("Regularize Employees", FshActions.Regularize, FshResources.Employees),
        new("Terminate Employees", FshActions.Terminate, FshResources.Employees),
        
        // Benefit Enrollments - special operation
        new("Terminate BenefitEnrollments", FshActions.Terminate, FshResources.Benefits),

        //Human Resources - Taxes
        new("View Taxes", FshActions.View, FshResources.Taxes),
        new("Search Taxes", FshActions.Search, FshResources.Taxes),
        new("Create Taxes", FshActions.Create, FshResources.Taxes),
        new("Update Taxes", FshActions.Update, FshResources.Taxes),
        new("Delete Taxes", FshActions.Delete, FshResources.Taxes),
        new("Import Taxes", FshActions.Import, FshResources.Taxes),
        new("Export Taxes", FshActions.Export, FshResources.Taxes),

        //MicroFinance
        new("View MicroFinance", FshActions.View, FshResources.MicroFinance),
        new("Search MicroFinance", FshActions.Search, FshResources.MicroFinance),
        new("Create MicroFinance", FshActions.Create, FshResources.MicroFinance),
        new("Update MicroFinance", FshActions.Update, FshResources.MicroFinance),
        new("Delete MicroFinance", FshActions.Delete, FshResources.MicroFinance),
        new("Import MicroFinance", FshActions.Import, FshResources.MicroFinance),
        new("Export MicroFinance", FshActions.Export, FshResources.MicroFinance),
        new("Approve MicroFinance", FshActions.Approve, FshResources.MicroFinance),
        new("Reject MicroFinance", FshActions.Reject, FshResources.MicroFinance),
        new("Process MicroFinance", FshActions.Process, FshResources.MicroFinance),
        new("Complete MicroFinance", FshActions.Complete, FshResources.MicroFinance),
        new("Cancel MicroFinance", FshActions.Cancel, FshResources.MicroFinance),
        new("Void MicroFinance", FshActions.Void, FshResources.MicroFinance),
        new("Post MicroFinance", FshActions.Post, FshResources.MicroFinance),
        new("Disburse MicroFinance", FshActions.Disburse, FshResources.MicroFinance),
        new("Close MicroFinance", FshActions.Close, FshResources.MicroFinance),
        new("Deposit MicroFinance", FshActions.Deposit, FshResources.MicroFinance),
        new("Withdraw MicroFinance", FshActions.Withdraw, FshResources.MicroFinance),
        new("Transfer MicroFinance", FshActions.Transfer, FshResources.MicroFinance),
        new("Freeze MicroFinance", FshActions.Freeze, FshResources.MicroFinance),
        new("Unfreeze MicroFinance", FshActions.Unfreeze, FshResources.MicroFinance),
        new("WriteOff MicroFinance", FshActions.WriteOff, FshResources.MicroFinance),
        new("Mature MicroFinance", FshActions.Mature, FshResources.MicroFinance),
        new("Renew MicroFinance", FshActions.Renew, FshResources.MicroFinance),
        new("Submit MicroFinance", FshActions.Submit, FshResources.MicroFinance),
    ];

    public static IReadOnlyList<FshPermission> All { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions);
    public static IReadOnlyList<FshPermission> Root { get; } = new ReadOnlyCollection<FshPermission>([.. AllPermissions.Where(p => p.IsRoot)]);
    public static IReadOnlyList<FshPermission> Admin { get; } = new ReadOnlyCollection<FshPermission>([.. AllPermissions.Where(p => !p.IsRoot)]);
    public static IReadOnlyList<FshPermission> Basic { get; } = new ReadOnlyCollection<FshPermission>([.. AllPermissions.Where(p => p.IsBasic)]);
}

public record FshPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource)
    {
        return $"Permissions.{resource}.{action}";
    }
}

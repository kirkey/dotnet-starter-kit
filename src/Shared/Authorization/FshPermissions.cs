using System.Collections.ObjectModel;

namespace Shared.Authorization;

public static class FshPermissions
{
    private static readonly FshPermission[] AllPermissions =
    [     
        //tenants
        new FshPermission("View Tenants", FshActions.View, FshResources.Tenants, IsRoot: true),
        new FshPermission("Create Tenants", FshActions.Create, FshResources.Tenants, IsRoot: true),
        new FshPermission("Update Tenants", FshActions.Update, FshResources.Tenants, IsRoot: true),
        new FshPermission("Upgrade Tenant Subscription", FshActions.UpgradeSubscription, FshResources.Tenants, IsRoot: true),

        //identity
        new FshPermission("View Users", FshActions.View, FshResources.Users),
        new FshPermission("Search Users", FshActions.Search, FshResources.Users),
        new FshPermission("Create Users", FshActions.Create, FshResources.Users),
        new FshPermission("Update Users", FshActions.Update, FshResources.Users),
        new FshPermission("Delete Users", FshActions.Delete, FshResources.Users),
        new FshPermission("Export Users", FshActions.Export, FshResources.Users),
        new FshPermission("View UserRoles", FshActions.View, FshResources.UserRoles),
        new FshPermission("Update UserRoles", FshActions.Update, FshResources.UserRoles),
        new FshPermission("View Roles", FshActions.View, FshResources.Roles),
        new FshPermission("Create Roles", FshActions.Create, FshResources.Roles),
        new FshPermission("Update Roles", FshActions.Update, FshResources.Roles),
        new FshPermission("Delete Roles", FshActions.Delete, FshResources.Roles),
        new FshPermission("View RoleClaims", FshActions.View, FshResources.RoleClaims),
        new FshPermission("Update RoleClaims", FshActions.Update, FshResources.RoleClaims),
        
        //products
        new FshPermission("View Products", FshActions.View, FshResources.Products, IsBasic: true),
        new FshPermission("Search Products", FshActions.Search, FshResources.Products, IsBasic: true),
        new FshPermission("Create Products", FshActions.Create, FshResources.Products),
        new FshPermission("Update Products", FshActions.Update, FshResources.Products),
        new FshPermission("Delete Products", FshActions.Delete, FshResources.Products),
        new FshPermission("Export Products", FshActions.Export, FshResources.Products),

        //brands
        new FshPermission("View Brands", FshActions.View, FshResources.Brands, IsBasic: true),
        new FshPermission("Search Brands", FshActions.Search, FshResources.Brands, IsBasic: true),
        new FshPermission("Create Brands", FshActions.Create, FshResources.Brands),
        new FshPermission("Update Brands", FshActions.Update, FshResources.Brands),
        new FshPermission("Delete Brands", FshActions.Delete, FshResources.Brands),
        new FshPermission("Export Brands", FshActions.Export, FshResources.Brands),

        //todos
        new FshPermission("View Todos", FshActions.View, FshResources.Todos, IsBasic: true),
        new FshPermission("Search Todos", FshActions.Search, FshResources.Todos, IsBasic: true),
        new FshPermission("Create Todos", FshActions.Create, FshResources.Todos),
        new FshPermission("Update Todos", FshActions.Update, FshResources.Todos),
        new FshPermission("Delete Todos", FshActions.Delete, FshResources.Todos),
        new FshPermission("Export Todos", FshActions.Export, FshResources.Todos),

         new FshPermission("View Hangfire", FshActions.View, FshResources.Hangfire),
         new FshPermission("View Dashboard", FshActions.View, FshResources.Dashboard),

        //audit
        new FshPermission("View Audit Trails", FshActions.View, FshResources.AuditTrails),
    ];

    public static IReadOnlyList<FshPermission> All { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions);
    public static IReadOnlyList<FshPermission> Root { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions.Where(p => p.IsRoot).ToArray());
    public static IReadOnlyList<FshPermission> Admin { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<FshPermission> Basic { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions.Where(p => p.IsBasic).ToArray());
}

public record FshPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource)
    {
        return $"Permissions.{resource}.{action}";
    }
}



using FSH.Starter.Blazor.Client.Models.NavigationMenu;

namespace FSH.Starter.Blazor.Client.Services.Navigation;

/// <summary>
/// Service that provides navigation menu structure and configuration.
/// Defines the complete menu hierarchy with sections, items, and their properties.
/// </summary>
public class MenuService : IMenuService
{
    private readonly List<MenuSectionModel> _features =
    [
        new MenuSectionModel
        {
            Title = "Start",
            SectionItems =
            [
                new MenuSectionItemModel { Title = "Home", Icon = Icons.Material.Filled.Home, Href = "/" },
                new MenuSectionItemModel { Title = "Counter", Icon = Icons.Material.Filled.Add, Href = "/counter" },
                new MenuSectionItemModel { Title = "Settings", Icon = Icons.Material.Filled.Settings, Href = "/app/settings" },
                new MenuSectionItemModel
                {
                    Title = "Hangfire",
                    Icon = Icons.Material.Filled.Engineering,
                    Href = "https://localhost:7000/jobs",
                    Target = "_blank",
                    Action = FshActions.View,
                    Resource = FshResources.Hangfire
                },

                new MenuSectionItemModel
                {
                    Title = "Audit Trail",
                    Icon = Icons.Material.Filled.WorkHistory,
                    Href = "/identity/audit-trail",
                    Action = FshActions.View,
                    Resource = FshResources.AuditTrails
                }
            ]
        },

        new MenuSectionModel
        {
            Title = "Modules",
            SectionItems =
            [
                new MenuSectionItemModel
                {
                    Title = "Application",
                    Icon = Icons.Material.Filled.AddBox,
                    IsParent = true,
                    MenuItems =
                        [new MenuSectionSubItemModel { Title = "Groups", Icon = Icons.Material.Filled.GroupWork, Href = "/app/groups" }]
                },

                new MenuSectionItemModel
                {
                    Title = "Catalog",
                    Icon = Icons.Material.Filled.AddBox,
                    IsParent = true,
                    MenuItems =
                    [
                        new MenuSectionSubItemModel
                        {
                            Title = "Products",
                            Icon = Icons.Material.Filled.ShoppingBag,
                            Href = "/catalog/products",
                            Action = FshActions.View,
                            Resource = FshResources.Products
                        },

                        new MenuSectionSubItemModel
                        {
                            Title = "Brands",
                            Icon = Icons.Material.Filled.Label,
                            Href = "/catalog/brands",
                            Action = FshActions.View,
                            Resource = FshResources.Brands
                        }
                    ]
                },

                new MenuSectionItemModel
                {
                    Title = "Accounting",
                    Icon = Icons.Material.Filled.AddBox,
                    IsParent = true,
                    MenuItems =
                    [
                        new MenuSectionSubItemModel { Title = "Chart Of Accounts", Icon = Icons.Material.Filled.List, Href = "/chart-of-accounts", PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Periods", Icon = Icons.Material.Filled.List, Href = "/accounting-periods", PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Accruals", Icon = Icons.Material.Filled.List, Href = "/accounting-accruals", PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Budgets", Icon = Icons.Material.Filled.List, Href = "/accounting-budgets", PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Projects", Icon = Icons.Material.Filled.List, Href = "/accounting-projects", PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Payees", Icon = Icons.Material.Filled.Groups, Href = "/accounting/payees", PageStatus = PageStatus.InProgress }
                    ]
                },
                
                new MenuSectionItemModel
                {
                    Title = "Store",
                    Icon = Icons.Material.Filled.AddBox,
                    IsParent = true,
                    MenuItems =
                    [
                        new MenuSectionSubItemModel { Title = "Dashboard", Icon = Icons.Material.Filled.Dashboard, Href = "/store/dashboard", Action = FshActions.View, Resource = FshResources.Store },
                        new MenuSectionSubItemModel { Title = "Categories", Icon = Icons.Material.Filled.Category, Href = "/store/categories", Action = FshActions.View, Resource = FshResources.Store },
                        new MenuSectionSubItemModel { Title = "Suppliers", Icon = Icons.Material.Filled.Groups, Href = "/store/suppliers", Action = FshActions.View, Resource = FshResources.Store },
                        new MenuSectionSubItemModel { Title = "Customers", Icon = Icons.Material.Filled.Groups, Href = "/store/customers", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Grocery Items", Icon = Icons.Material.Filled.Category, Href = "/store/grocery-items", Action = FshActions.View, Resource = FshResources.Store },
                        new MenuSectionSubItemModel { Title = "Inventory", Icon = Icons.Material.Filled.Inventory, Href = "/store/inventory", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Sales Reports", Icon = Icons.Material.Filled.Analytics, Href = "/store/sales-reports", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Point of Sale", Icon = Icons.Material.Filled.PointOfSale, Href = "/store/pos", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress }
                    ]
                },
                
                new MenuSectionItemModel
                {
                    Title = "Warehouse",
                    Icon = Icons.Material.Filled.AddBox,
                    IsParent = true,
                    MenuItems =
                    [
                        new MenuSectionSubItemModel { Title = "Warehouse", Icon = Icons.Material.Filled.Category, Href = "/warehouse/warehouses", Action = FshActions.View, Resource = FshResources.Warehouse },
                        new MenuSectionSubItemModel { Title = "Locations", Icon = Icons.Material.Filled.Category, Href = "/warehouse/locations", Action = FshActions.View, Resource = FshResources.Warehouse },
                        new MenuSectionSubItemModel { Title = "Stock Movements", Icon = Icons.Material.Filled.MoveUp, Href = "/warehouse/stock-movements", Action = FshActions.View, Resource = FshResources.Warehouse, PageStatus = PageStatus.InProgress }
                    ]
                },

                new MenuSectionItemModel
                {
                    Title = "Todos",
                    Icon = Icons.Material.Filled.Checklist,
                    Href = "/todos",
                    Action = FshActions.View,
                    Resource = FshResources.Todos
                },
                
                new MenuSectionItemModel
                {
                    Title = "Analytics",
                    Icon = Icons.Material.Filled.Analytics,
                    Href = "/analytics",
                    PageStatus = PageStatus.ComingSoon
                },
                
                new MenuSectionItemModel
                {
                    Title = "Reports",
                    Icon = Icons.Material.Filled.Assessment,
                    Href = "/reports",
                    PageStatus = PageStatus.InProgress
                }
            ]
        },

        new MenuSectionModel
        {
            Title = "Administration",
            SectionItems =
            [
                new MenuSectionItemModel
                {
                    Title = "Administration",
                    Icon = Icons.Material.Filled.ManageAccounts,
                    IsParent = true,
                    MenuItems =
                    [
                        new MenuSectionSubItemModel
                        {
                            Title = "Users",
                            Icon = Icons.Material.Filled.PeopleAlt,
                            Href = "/identity/users",
                            Action = FshActions.View,
                            Resource = FshResources.Users
                        },

                        new MenuSectionSubItemModel
                        {
                            Title = "Roles",
                            Icon = Icons.Material.Filled.EmojiPeople,
                            Href = "/identity/roles",
                            Action = FshActions.View,
                            Resource = FshResources.Roles
                        },

                        new MenuSectionSubItemModel
                        {
                            Title = "Tenants",
                            Icon = Icons.Material.Filled.GroupWork,
                            Href = "/tenants",
                            Action = FshActions.View,
                            Resource = FshResources.Tenants
                        }
                    ]
                }
            ]
        }
    ];

    /// <summary>
    /// Gets the complete navigation menu structure with all sections and items.
    /// </summary>
    public IEnumerable<MenuSectionModel> Features => _features;
}

using FSH.Starter.Blazor.Client.Models.NavigationMenu;

namespace FSH.Starter.Blazor.Client.Services.Navigation;

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
                        new MenuSectionSubItemModel { Title = "Chart Of Accounts", Icon = Icons.Material.Filled.List, Href = "/chart-of-accounts" },
                        new MenuSectionSubItemModel { Title = "Periods", Icon = Icons.Material.Filled.List, Href = "/accounting-periods" },
                        new MenuSectionSubItemModel { Title = "Accruals", Icon = Icons.Material.Filled.List, Href = "/accounting-accruals" },
                        new MenuSectionSubItemModel { Title = "Budgets", Icon = Icons.Material.Filled.List, Href = "/accounting-budgets" },
                        new MenuSectionSubItemModel { Title = "Payees", Icon = Icons.Material.Filled.Groups, Href = "/payees" }
                    ]
                },
                
                new MenuSectionItemModel
                {
                    Title = "Store",
                    Icon = Icons.Material.Filled.AddBox,
                    IsParent = true,
                    MenuItems =
                    [
                        new MenuSectionSubItemModel { Title = "Categories", Icon = Icons.Material.Filled.Category, Href = "/store/categories", Action = FshActions.View, Resource = FshResources.Store },
                        new MenuSectionSubItemModel { Title = "Suppliers", Icon = Icons.Material.Filled.Category, Href = "/store/suppliers", Action = FshActions.View, Resource = FshResources.Store },
                        new MenuSectionSubItemModel { Title = "Grocery Items", Icon = Icons.Material.Filled.Category, Href = "/store/grocery-items", Action = FshActions.View, Resource = FshResources.Store },
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
                    ]
                },

                new MenuSectionItemModel
                {
                    Title = "Todos",
                    Icon = Icons.Material.Filled.Checklist,
                    Href = "/todos",
                    Action = FshActions.View,
                    Resource = FshResources.Todos
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

    public IEnumerable<MenuSectionModel> Features => _features;
}

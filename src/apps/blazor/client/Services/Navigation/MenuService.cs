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
                        new MenuSectionSubItemModel { Title = "Chart Of Accounts", Icon = Icons.Material.Filled.List, Href = "/chart-of-accounts", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Journal Entries", Icon = Icons.Material.Filled.Receipt, Href = "/accounting/journal-entries", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Periods", Icon = Icons.Material.Filled.List, Href = "/accounting-periods", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Accruals", Icon = Icons.Material.Filled.List, Href = "/accounting-accruals", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Budgets", Icon = Icons.Material.Filled.List, Href = "/accounting-budgets", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Projects", Icon = Icons.Material.Filled.List, Href = "/accounting-projects", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Payees", Icon = Icons.Material.Filled.Groups, Href = "/accounting/payees", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Banks", Icon = Icons.Material.Filled.AccountBalance, Href = "/accounting/banks", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Checks", Icon = Icons.Material.Filled.Receipt, Href = "/accounting/checks", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Debit Memos", Icon = Icons.Material.Filled.AddCircleOutline, Href = "/accounting/debit-memos", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Credit Memos", Icon = Icons.Material.Filled.RemoveCircleOutline, Href = "/accounting/credit-memos", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.ComingSoon }
                    ]
                },
                
                new MenuSectionItemModel
                {
                    Title = "Store",
                    Icon = Icons.Material.Filled.AddBox,
                    IsParent = true,
                    MenuItems =
                    [
                        // Dashboard & Core Setup
                        new MenuSectionSubItemModel { Title = "Dashboard", Icon = Icons.Material.Filled.Dashboard, Href = "/store/dashboard", Action = FshActions.View, Resource = FshResources.Store },
                        new MenuSectionSubItemModel { Title = "Categories", Icon = Icons.Material.Filled.Category, Href = "/store/categories", Action = FshActions.View, Resource = FshResources.Store },
                        new MenuSectionSubItemModel { Title = "Items", Icon = Icons.Material.Filled.Inventory2, Href = "/store/items", Action = FshActions.View, Resource = FshResources.Store },
                        
                        // Suppliers & Customers
                        new MenuSectionSubItemModel { Title = "Suppliers", Icon = Icons.Material.Filled.LocalShipping, Href = "/store/suppliers", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Item Suppliers", Icon = Icons.Material.Filled.Link, Href = "/store/item-suppliers", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        
                        // Procurement
                        new MenuSectionSubItemModel { Title = "Purchase Orders", Icon = Icons.Material.Filled.ShoppingCart, Href = "/store/purchase-orders", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Goods Receipts", Icon = Icons.Material.Filled.MoveToInbox, Href = "/store/goods-receipts", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        
                        // Inventory Management
                        new MenuSectionSubItemModel { Title = "Stock Levels", Icon = Icons.Material.Filled.Inventory, Href = "/store/stock-levels", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Stock Adjustments", Icon = Icons.Material.Filled.Inventory, Href = "/store/stock-adjustments", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Inventory Transactions", Icon = Icons.Material.Filled.SwapHoriz, Href = "/store/inventory-transactions", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Inventory Transfers", Icon = Icons.Material.Filled.Transform, Href = "/store/inventory-transfers", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Inventory Reservations", Icon = Icons.Material.Filled.BookmarkAdded, Href = "/store/inventory-reservations", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        
                        // Tracking
                        new MenuSectionSubItemModel { Title = "Lot Numbers", Icon = Icons.Material.Filled.QrCode, Href = "/store/lot-numbers", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Serial Numbers", Icon = Icons.Material.Filled.Pin, Href = "/store/serial-numbers", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        
                        // Sales & Reporting
                        new MenuSectionSubItemModel { Title = "Point of Sale", Icon = Icons.Material.Filled.PointOfSale, Href = "/store/pos", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Sales Reports", Icon = Icons.Material.Filled.Analytics, Href = "/store/sales-reports", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.ComingSoon }
                    ]
                },
                
                new MenuSectionItemModel
                {
                    Title = "Warehouse",
                    Icon = Icons.Material.Filled.AddBox,
                    IsParent = true,
                    MenuItems =
                    [
                        // Warehouse Management
                        new MenuSectionSubItemModel { Title = "Warehouses", Icon = Icons.Material.Filled.Warehouse, Href = "/warehouses", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Locations", Icon = Icons.Material.Filled.LocationOn, Href = "/warehouse/locations", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Bins", Icon = Icons.Material.Filled.Inbox, Href = "/warehouse/bins", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Cycle Counts", Icon = Icons.Material.Filled.Checklist, Href = "/store/cycle-counts", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        
                        // Warehouse Operations
                        new MenuSectionSubItemModel { Title = "Pick Lists", Icon = Icons.Material.Filled.PlaylistAddCheck, Href = "/store/pick-lists", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Put Away Tasks", Icon = Icons.Material.Filled.AddToQueue, Href = "/store/put-away-tasks", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
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
                    Title = "Messaging",
                    Icon = Icons.Material.Filled.Chat,
                    Href = "/messaging",
                    Action = FshActions.View,
                    Resource = FshResources.Messaging
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

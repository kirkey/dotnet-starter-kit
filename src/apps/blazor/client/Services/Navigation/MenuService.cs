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
                        // ========== GENERAL LEDGER & CHART OF ACCOUNTS ==========
                        new MenuSectionSubItemModel { Title = "General Ledger", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Chart Of Accounts", Icon = Icons.Material.Filled.AccountTree, Href = "/accounting/chart-of-accounts", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "General Ledger", Icon = Icons.Material.Filled.Book, Href = "/accounting/general-ledger", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Journal Entries", Icon = Icons.Material.Filled.Receipt, Href = "/accounting/journal-entries", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        
                        // ========== ACCOUNTS RECEIVABLE ==========
                        new MenuSectionSubItemModel { Title = "Accounts Receivable", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Customers", Icon = Icons.Material.Filled.People, Href = "/accounting/customers", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Invoices", Icon = Icons.Material.Filled.Description, Href = "/accounting/invoices", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Credit Memos", Icon = Icons.Material.Filled.RemoveCircleOutline, Href = "/accounting/credit-memos", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.ComingSoon },
                        
                        // ========== ACCOUNTS PAYABLE ==========
                        new MenuSectionSubItemModel { Title = "Accounts Payable", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Vendors", Icon = Icons.Material.Filled.Business, Href = "/accounting/vendors", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Bills", Icon = Icons.Material.Filled.ReceiptLong, Href = "/accounting/bills", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Debit Memos", Icon = Icons.Material.Filled.AddCircleOutline, Href = "/accounting/debit-memos", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Payees", Icon = Icons.Material.Filled.Groups, Href = "/accounting/payees", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        
                        // ========== BANKING & PAYMENTS ==========
                        new MenuSectionSubItemModel { Title = "Banking & Cash", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Banks", Icon = Icons.Material.Filled.AccountBalance, Href = "/accounting/banks", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Bank Reconciliations", Icon = Icons.Material.Filled.AccountBalanceWallet, Href = "/accounting/bank-reconciliations", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Checks", Icon = Icons.Material.Filled.Payment, Href = "/accounting/checks", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        
                        // ========== BUDGETING & PROJECTS ==========
                        new MenuSectionSubItemModel { Title = "Planning & Tracking", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Budgets", Icon = Icons.Material.Filled.MonetizationOn, Href = "/accounting/budgets", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Projects", Icon = Icons.Material.Filled.Work, Href = "/accounting/projects", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        
                        // ========== PERIOD MANAGEMENT & REPORTING ==========
                        new MenuSectionSubItemModel { Title = "Period Close & Accruals", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Trial Balance", Icon = Icons.Material.Filled.Balance, Href = "/accounting/trial-balance", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Fiscal Period Close", Icon = Icons.Material.Filled.Lock, Href = "/accounting/fiscal-period-close", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Accounting Periods", Icon = Icons.Material.Filled.CalendarMonth, Href = "/accounting/periods", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Accruals", Icon = Icons.Material.Filled.Schedule, Href = "/accounting/accruals", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        
                        // ========== CONFIGURATION ==========
                        new MenuSectionSubItemModel { Title = "Configuration", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Tax Codes", Icon = Icons.Material.Filled.Percent, Href = "/accounting/tax-codes", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                    ]
                },
                
                new MenuSectionItemModel
                {
                    Title = "Store",
                    Icon = Icons.Material.Filled.AddBox,
                    IsParent = true,
                    MenuItems =
                    [
                        // ========== DASHBOARD & OVERVIEW ==========
                        new MenuSectionSubItemModel { Title = "Dashboard & Setup", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Dashboard", Icon = Icons.Material.Filled.Dashboard, Href = "/store/dashboard", Action = FshActions.View, Resource = FshResources.Store },
                        new MenuSectionSubItemModel { Title = "Categories", Icon = Icons.Material.Filled.Category, Href = "/store/categories", Action = FshActions.View, Resource = FshResources.Store },
                        new MenuSectionSubItemModel { Title = "Items", Icon = Icons.Material.Filled.Inventory2, Href = "/store/items", Action = FshActions.View, Resource = FshResources.Store },
                        
                        // ========== PROCUREMENT ==========
                        new MenuSectionSubItemModel { Title = "Procurement", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Suppliers", Icon = Icons.Material.Filled.LocalShipping, Href = "/store/suppliers", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Item Suppliers", Icon = Icons.Material.Filled.Link, Href = "/store/item-suppliers", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Purchase Orders", Icon = Icons.Material.Filled.ShoppingCart, Href = "/store/purchase-orders", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Goods Receipts", Icon = Icons.Material.Filled.MoveToInbox, Href = "/store/goods-receipts", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        
                        // ========== INVENTORY MANAGEMENT ==========
                        new MenuSectionSubItemModel { Title = "Inventory", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Stock Levels", Icon = Icons.Material.Filled.Inventory, Href = "/store/stock-levels", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Stock Adjustments", Icon = Icons.Material.Filled.TrendingDown, Href = "/store/stock-adjustments", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Inventory Transactions", Icon = Icons.Material.Filled.SwapHoriz, Href = "/store/inventory-transactions", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Inventory Transfers", Icon = Icons.Material.Filled.Transform, Href = "/store/inventory-transfers", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Inventory Reservations", Icon = Icons.Material.Filled.BookmarkAdded, Href = "/store/inventory-reservations", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        
                        // ========== TRACKING & TRACEABILITY ==========
                        new MenuSectionSubItemModel { Title = "Tracking", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Lot Numbers", Icon = Icons.Material.Filled.QrCode, Href = "/store/lot-numbers", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Serial Numbers", Icon = Icons.Material.Filled.Pin, Href = "/store/serial-numbers", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        
                        // ========== SALES & REPORTING ==========
                        new MenuSectionSubItemModel { Title = "Sales & Reports", IsGroupHeader = true },
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
                        // ========== WAREHOUSE SETUP ==========
                        new MenuSectionSubItemModel { Title = "Setup & Configuration", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Warehouses", Icon = Icons.Material.Filled.Warehouse, Href = "/warehouses", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Locations", Icon = Icons.Material.Filled.LocationOn, Href = "/warehouse/locations", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Bins", Icon = Icons.Material.Filled.Inbox, Href = "/warehouse/bins", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        
                        // ========== WAREHOUSE OPERATIONS ==========
                        new MenuSectionSubItemModel { Title = "Operations", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Pick Lists", Icon = Icons.Material.Filled.PlaylistAddCheck, Href = "/store/pick-lists", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Put Away Tasks", Icon = Icons.Material.Filled.AddToQueue, Href = "/store/put-away-tasks", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
                        
                        // ========== INVENTORY CONTROL ==========
                        new MenuSectionSubItemModel { Title = "Inventory Control", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Cycle Counts", Icon = Icons.Material.Filled.Checklist, Href = "/store/cycle-counts", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.InProgress },
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

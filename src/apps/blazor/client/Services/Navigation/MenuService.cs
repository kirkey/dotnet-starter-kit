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
                        new MenuSectionSubItemModel { Title = "Recurring Entries", Icon = Icons.Material.Filled.Repeat, Href = "/accounting/recurring-journal-entries", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        
                        // ========== ACCOUNTS RECEIVABLE ==========
                        new MenuSectionSubItemModel { Title = "Accounts Receivable", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "AR Accounts", Icon = Icons.Material.Filled.AccountBalanceWallet, Href = "/accounting/ar-accounts", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Customers", Icon = Icons.Material.Filled.People, Href = "/accounting/customers", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Invoices", Icon = Icons.Material.Filled.Description, Href = "/accounting/invoices", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Credit Memos", Icon = Icons.Material.Filled.RemoveCircleOutline, Href = "/accounting/credit-memos", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        
                        // ========== ACCOUNTS PAYABLE ==========
                        new MenuSectionSubItemModel { Title = "Accounts Payable", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "AP Accounts", Icon = Icons.Material.Filled.AccountBalanceWallet, Href = "/accounting/ap-accounts", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Vendors", Icon = Icons.Material.Filled.Business, Href = "/accounting/vendors", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Bills", Icon = Icons.Material.Filled.ReceiptLong, Href = "/accounting/bills", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Debit Memos", Icon = Icons.Material.Filled.AddCircleOutline, Href = "/accounting/debit-memos", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Payees", Icon = Icons.Material.Filled.Groups, Href = "/accounting/payees", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        
                        // ========== BANKING & PAYMENTS ==========
                        new MenuSectionSubItemModel { Title = "Banking & Cash", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Banks", Icon = Icons.Material.Filled.AccountBalance, Href = "/accounting/banks", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Bank Reconciliations", Icon = Icons.Material.Filled.AccountBalanceWallet, Href = "/accounting/bank-reconciliations", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Checks", Icon = Icons.Material.Filled.Payment, Href = "/accounting/checks", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        
                        // ========== PLANNING & TRACKING ==========
                        new MenuSectionSubItemModel { Title = "Planning & Tracking", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Budgets", Icon = Icons.Material.Filled.MonetizationOn, Href = "/accounting/budgets", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Projects", Icon = Icons.Material.Filled.Work, Href = "/accounting/projects", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Write-Offs", Icon = Icons.Material.Filled.MoneyOff, Href = "/accounting/write-offs", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Fixed Assets", Icon = Icons.Material.Filled.BusinessCenter, Href = "/accounting/fixed-assets", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        
                        // ========== PERIOD MANAGEMENT & REPORTING ==========
                        new MenuSectionSubItemModel { Title = "Period Close & Accruals", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Trial Balance", Icon = Icons.Material.Filled.Balance, Href = "/accounting/trial-balance", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Financial Statements", Icon = Icons.Material.Filled.Assessment, Href = "/accounting/financial-statements", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Fiscal Period Close", Icon = Icons.Material.Filled.Lock, Href = "/accounting/fiscal-period-close", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Retained Earnings", Icon = Icons.Material.Filled.TrendingUp, Href = "/accounting/retained-earnings", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Accounting Periods", Icon = Icons.Material.Filled.CalendarMonth, Href = "/accounting/periods", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Accruals", Icon = Icons.Material.Filled.Schedule, Href = "/accounting/accruals", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Deferred Revenue", Icon = Icons.Material.Filled.AccountBalance, Href = "/accounting/deferred-revenue", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Prepaid Expenses", Icon = Icons.Material.Filled.Payment, Href = "/accounting/prepaid-expenses", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        
                        // ========== CONFIGURATION ==========
                        new MenuSectionSubItemModel { Title = "Configuration", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Tax Codes", Icon = Icons.Material.Filled.Percent, Href = "/accounting/tax-codes", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Depreciation Methods", Icon = Icons.Material.Filled.Timeline, Href = "/accounting/depreciation-methods", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Inventory Items", Icon = Icons.Material.Filled.Inventory, Href = "/accounting/inventory-items", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.Completed },
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
                        new MenuSectionSubItemModel { Title = "Sales Imports", Icon = Icons.Material.Filled.Upload, Href = "/store/sales-imports", Action = FshActions.View, Resource = FshResources.Store },
                        
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
                        new MenuSectionSubItemModel { Title = "Warehouses", Icon = Icons.Material.Filled.Warehouse, Href = "/store/warehouses", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Locations", Icon = Icons.Material.Filled.LocationOn, Href = "/warehouse/locations", Action = FshActions.View, Resource = FshResources.Store, PageStatus = PageStatus.Completed },
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
                    Title = "Human Resource",
                    Icon = Icons.Material.Filled.People,
                    IsParent = true,
                    MenuItems =
                    [
                        // ========== ORGANIZATION & SETUP ==========
                        new MenuSectionSubItemModel { Title = "Organization & Setup", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Organizational Units", Icon = Icons.Material.Filled.AccountTree, Href = "/hr/organizational-units", Action = FshActions.View, Resource = FshResources.Organization, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Departments", Icon = Icons.Material.Filled.Business, Href = "/hr/departments", Action = FshActions.View, Resource = FshResources.Organization, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Designations", Icon = Icons.Material.Filled.WorkOutline, Href = "/hr/designations", Action = FshActions.View, Resource = FshResources.Organization, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Shifts", Icon = Icons.Material.Filled.Schedule, Href = "/hr/shifts", Action = FshActions.View, Resource = FshResources.Organization, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Holidays", Icon = Icons.Material.Filled.EventNote, Href = "/hr/holidays", Action = FshActions.View, Resource = FshResources.Organization, PageStatus = PageStatus.ComingSoon },
                        
                        // ========== EMPLOYEE MANAGEMENT ==========
                        new MenuSectionSubItemModel { Title = "Employee Management", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Employees", Icon = Icons.Material.Filled.Badge, Href = "/hr/employees", Action = FshActions.View, Resource = FshResources.Employees, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Employee Contacts", Icon = Icons.Material.Filled.Contacts, Href = "/hr/employee-contacts", Action = FshActions.View, Resource = FshResources.Employees, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Employee Dependents", Icon = Icons.Material.Filled.FamilyRestroom, Href = "/hr/employee-dependents", Action = FshActions.View, Resource = FshResources.Employees, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Employee Documents", Icon = Icons.Material.Filled.Description, Href = "/hr/employee-documents", Action = FshActions.View, Resource = FshResources.Employees, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Employee Education", Icon = Icons.Material.Filled.School, Href = "/hr/employee-educations", Action = FshActions.View, Resource = FshResources.Employees, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Performance Reviews", Icon = Icons.Material.Filled.Assessment, Href = "/hr/performance-reviews", Action = FshActions.View, Resource = FshResources.Employees, PageStatus = PageStatus.ComingSoon },
                        
                        // ========== TIME & ATTENDANCE ==========
                        new MenuSectionSubItemModel { Title = "Time & Attendance", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Attendance", Icon = Icons.Material.Filled.Fingerprint, Href = "/hr/attendances", Action = FshActions.View, Resource = FshResources.Attendance, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Timesheets", Icon = Icons.Material.Filled.AccessTime, Href = "/hr/timesheets", Action = FshActions.View, Resource = FshResources.Timesheets, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Shift Assignments", Icon = Icons.Material.Filled.AssignmentInd, Href = "/hr/shift-assignments", Action = FshActions.View, Resource = FshResources.Attendance, PageStatus = PageStatus.ComingSoon },
                        
                        // ========== LEAVE MANAGEMENT ==========
                        new MenuSectionSubItemModel { Title = "Leave Management", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Leave Types", Icon = Icons.Material.Filled.Category, Href = "/hr/leave-types", Action = FshActions.View, Resource = FshResources.Leaves, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Leave Requests", Icon = Icons.Material.Filled.EventAvailable, Href = "/hr/leave-requests", Action = FshActions.View, Resource = FshResources.Leaves, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Leave Balances", Icon = Icons.Material.Filled.AccountBalanceWallet, Href = "/hr/leave-balances", Action = FshActions.View, Resource = FshResources.Leaves, PageStatus = PageStatus.ComingSoon },
                        
                        // ========== PAYROLL ==========
                        new MenuSectionSubItemModel { Title = "Payroll", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Payroll Run", Icon = Icons.Material.Filled.Payments, Href = "/hr/payrolls", Action = FshActions.View, Resource = FshResources.Payroll, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Pay Components", Icon = Icons.Material.Filled.AttachMoney, Href = "/hr/pay-components", Action = FshActions.View, Resource = FshResources.Payroll, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Pay Component Rates", Icon = Icons.Material.Filled.TrendingUp, Href = "/hr/pay-component-rates", Action = FshActions.View, Resource = FshResources.Payroll, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Employee Pay Components", Icon = Icons.Material.Filled.PersonOutline, Href = "/hr/employee-pay-components", Action = FshActions.View, Resource = FshResources.Payroll, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Deductions", Icon = Icons.Material.Filled.RemoveCircleOutline, Href = "/hr/deductions", Action = FshActions.View, Resource = FshResources.Payroll, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Payroll Deductions", Icon = Icons.Material.Filled.MoneyOff, Href = "/hr/payroll-deductions", Action = FshActions.View, Resource = FshResources.Payroll, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Tax Brackets", Icon = Icons.Material.Filled.AccountBalance, Href = "/hr/tax-brackets", Action = FshActions.View, Resource = FshResources.Payroll, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Taxes", Icon = Icons.Material.Filled.Receipt, Href = "/hr/taxes", Action = FshActions.View, Resource = FshResources.Payroll, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Bank Accounts", Icon = Icons.Material.Filled.AccountBalance, Href = "/hr/bank-accounts", Action = FshActions.View, Resource = FshResources.Payroll, PageStatus = PageStatus.ComingSoon },
                        
                        // ========== BENEFITS ==========
                        new MenuSectionSubItemModel { Title = "Benefits & Enrollment", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Benefits", Icon = Icons.Material.Filled.CardGiftcard, Href = "/hr/benefits", Action = FshActions.View, Resource = FshResources.Benefits, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Benefit Enrollments", Icon = Icons.Material.Filled.HowToReg, Href = "/hr/benefit-enrollments", Action = FshActions.View, Resource = FshResources.Benefits, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Benefit Allocations", Icon = Icons.Material.Filled.LocalOffer, Href = "/hr/benefit-allocations", Action = FshActions.View, Resource = FshResources.Benefits, PageStatus = PageStatus.ComingSoon },
                        
                        // ========== DOCUMENTS & REPORTS ==========
                        new MenuSectionSubItemModel { Title = "Documents & Reports", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Document Templates", Icon = Icons.Material.Filled.Article, Href = "/hr/document-templates", Action = FshActions.View, Resource = FshResources.Employees, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Generated Documents", Icon = Icons.Material.Filled.FileCopy, Href = "/hr/generated-documents", Action = FshActions.View, Resource = FshResources.Employees, PageStatus = PageStatus.ComingSoon },
                        
                        // ========== ANALYTICS & INSIGHTS ==========
                        new MenuSectionSubItemModel { Title = "Analytics & Insights", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "HR Analytics", Icon = Icons.Material.Filled.Analytics, Href = "/hr/analytics", Action = FshActions.View, Resource = FshResources.Analytics, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Employee Dashboard", Icon = Icons.Material.Filled.Dashboard, Href = "/hr/employee-dashboard", Action = FshActions.View, Resource = FshResources.Dashboard, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Attendance Reports", Icon = Icons.Material.Filled.BarChart, Href = "/hr/attendance-reports", Action = FshActions.View, Resource = FshResources.Attendance, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Leave Reports", Icon = Icons.Material.Filled.EventNote, Href = "/hr/leave-reports", Action = FshActions.View, Resource = FshResources.Leaves, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Payroll Reports", Icon = Icons.Material.Filled.Receipt, Href = "/hr/payroll-reports", Action = FshActions.View, Resource = FshResources.Leaves, PageStatus = PageStatus.InProgress },
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

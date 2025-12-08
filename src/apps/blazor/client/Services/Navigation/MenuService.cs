using FSH.Starter.Blazor.Infrastructure.Navigation;
using FSH.Starter.Blazor.Infrastructure.Navigation.Models;

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
                },

                new MenuSectionItemModel
                {
                    Title = "Audit Trail",
                    Icon = Icons.Material.Filled.WorkHistory,
                    Href = "/identity/audit-trail",
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
                        new MenuSectionSubItemModel { Title = "Posting Batches", Icon = Icons.Material.Filled.BatchPrediction, Href = "/accounting/posting-batches", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Account Reconciliations", Icon = Icons.Material.Filled.CompareArrows, Href = "/accounting/account-reconciliations", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        
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
                        new MenuSectionSubItemModel { Title = "Payments", Icon = Icons.Material.Filled.Payments, Href = "/accounting/payments", Action = FshActions.View, Resource = FshResources.Accounting, PageStatus = PageStatus.InProgress },
                        
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
                
                // MicroFinance Module - Complete Entity Coverage (67 entities)
                new MenuSectionItemModel
                {
                    Title = "MicroFinance",
                    Icon = Icons.Material.Filled.AccountBalance,
                    IsParent = true,
                    MenuItems =
                    [
                        // ========== ORGANIZATION & SETUP ==========
                        new MenuSectionSubItemModel { Title = "Organization & Setup", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Branches", Icon = Icons.Material.Filled.Business, Href = "/microfinance/branches", Action = FshActions.View, Resource = FshResources.Branches, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Branch Targets", Icon = Icons.Material.Filled.TrackChanges, Href = "/microfinance/branch-targets", Action = FshActions.View, Resource = FshResources.BranchTargets, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "MFI Configurations", Icon = Icons.Material.Filled.Settings, Href = "/microfinance/mfi-configurations", Action = FshActions.View, Resource = FshResources.MfiConfigurations, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Staff", Icon = Icons.Material.Filled.Badge, Href = "/microfinance/staff", Action = FshActions.View, Resource = FshResources.Staff, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Staff Trainings", Icon = Icons.Material.Filled.School, Href = "/microfinance/staff-trainings", Action = FshActions.View, Resource = FshResources.StaffTrainings, PageStatus = PageStatus.InProgress },
                        
                        // ========== MEMBER MANAGEMENT ==========
                        new MenuSectionSubItemModel { Title = "Member Management", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Members", Icon = Icons.Material.Filled.People, Href = "/microfinance/members", Action = FshActions.View, Resource = FshResources.Members, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Member Groups", Icon = Icons.Material.Filled.Groups, Href = "/microfinance/member-groups", Action = FshActions.View, Resource = FshResources.MemberGroups, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Group Memberships", Icon = Icons.Material.Filled.GroupAdd, Href = "/microfinance/group-memberships", Action = FshActions.View, Resource = FshResources.GroupMemberships, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "KYC Documents", Icon = Icons.Material.Filled.VerifiedUser, Href = "/microfinance/kyc-documents", Action = FshActions.View, Resource = FshResources.KycDocuments, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Customer Segments", Icon = Icons.Material.Filled.Category, Href = "/microfinance/customer-segments", Action = FshActions.View, Resource = FshResources.CustomerSegments, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Customer Surveys", Icon = Icons.Material.Filled.Poll, Href = "/microfinance/customer-surveys", Action = FshActions.View, Resource = FshResources.CustomerSurveys, PageStatus = PageStatus.InProgress },
                        
                        // ========== PRODUCT CATALOG ==========
                        new MenuSectionSubItemModel { Title = "Product Catalog", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Loan Products", Icon = Icons.Material.Filled.CreditScore, Href = "/microfinance/loan-products", Action = FshActions.View, Resource = FshResources.LoanProducts, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Savings Products", Icon = Icons.Material.Filled.Savings, Href = "/microfinance/savings-products", Action = FshActions.View, Resource = FshResources.SavingsProducts, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Share Products", Icon = Icons.Material.Filled.Share, Href = "/microfinance/share-products", Action = FshActions.View, Resource = FshResources.ShareProducts, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Fee Definitions", Icon = Icons.Material.Filled.AttachMoney, Href = "/microfinance/fee-definitions", Action = FshActions.View, Resource = FshResources.FeeDefinitions, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Insurance Products", Icon = Icons.Material.Filled.HealthAndSafety, Href = "/microfinance/insurance-products", Action = FshActions.View, Resource = FshResources.InsuranceProducts, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Investment Products", Icon = Icons.Material.Filled.TrendingUp, Href = "/microfinance/investment-products", Action = FshActions.View, Resource = FshResources.InvestmentProducts, PageStatus = PageStatus.InProgress },
                        
                        // ========== ACCOUNTS ==========
                        new MenuSectionSubItemModel { Title = "Accounts", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Savings Accounts", Icon = Icons.Material.Filled.AccountBalance, Href = "/microfinance/savings-accounts", Action = FshActions.View, Resource = FshResources.SavingsAccounts, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Share Accounts", Icon = Icons.Material.Filled.Share, Href = "/microfinance/share-accounts", Action = FshActions.View, Resource = FshResources.ShareAccounts, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Fixed Deposits", Icon = Icons.Material.Filled.Lock, Href = "/microfinance/fixed-deposits", Action = FshActions.View, Resource = FshResources.FixedDeposits, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Investment Accounts", Icon = Icons.Material.Filled.ShowChart, Href = "/microfinance/investment-accounts", Action = FshActions.View, Resource = FshResources.InvestmentAccounts, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Insurance Policies", Icon = Icons.Material.Filled.Policy, Href = "/microfinance/insurance-policies", Action = FshActions.View, Resource = FshResources.InsurancePolicies, PageStatus = PageStatus.InProgress },
                        
                        // ========== LOAN OPERATIONS ==========
                        new MenuSectionSubItemModel { Title = "Loan Operations", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Loans", Icon = Icons.Material.Filled.MonetizationOn, Href = "/microfinance/loans", Action = FshActions.View, Resource = FshResources.Loans, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Loan Applications", Icon = Icons.Material.Filled.Assignment, Href = "/microfinance/loan-applications", Action = FshActions.View, Resource = FshResources.LoanApplications, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Loan Schedules", Icon = Icons.Material.Filled.CalendarMonth, Href = "/microfinance/loan-schedules", Action = FshActions.View, Resource = FshResources.LoanSchedules, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Loan Repayments", Icon = Icons.Material.Filled.Payment, Href = "/microfinance/loan-repayments", Action = FshActions.View, Resource = FshResources.LoanRepayments, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Loan Collaterals", Icon = Icons.Material.Filled.Security, Href = "/microfinance/loan-collaterals", Action = FshActions.View, Resource = FshResources.LoanCollaterals, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Loan Guarantors", Icon = Icons.Material.Filled.Handshake, Href = "/microfinance/loan-guarantors", Action = FshActions.View, Resource = FshResources.LoanGuarantors, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Loan Disbursement Tranches", Icon = Icons.Material.Filled.Paid, Href = "/microfinance/loan-disbursement-tranches", Action = FshActions.View, Resource = FshResources.LoanDisbursementTranches, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Loan Officer Assignments", Icon = Icons.Material.Filled.AssignmentInd, Href = "/microfinance/loan-officer-assignments", Action = FshActions.View, Resource = FshResources.LoanOfficerAssignments, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Loan Officer Targets", Icon = Icons.Material.Filled.TrackChanges, Href = "/microfinance/loan-officer-targets", Action = FshActions.View, Resource = FshResources.LoanOfficerTargets, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Loan Restructures", Icon = Icons.Material.Filled.Autorenew, Href = "/microfinance/loan-restructures", Action = FshActions.View, Resource = FshResources.LoanRestructures, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Loan Write-Offs", Icon = Icons.Material.Filled.MoneyOff, Href = "/microfinance/loan-write-offs", Action = FshActions.View, Resource = FshResources.LoanWriteOffs, PageStatus = PageStatus.InProgress },
                        
                        // ========== COLLATERAL MANAGEMENT ==========
                        new MenuSectionSubItemModel { Title = "Collateral Management", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Collateral Types", Icon = Icons.Material.Filled.Category, Href = "/microfinance/collateral-types", Action = FshActions.View, Resource = FshResources.CollateralTypes, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Collateral Valuations", Icon = Icons.Material.Filled.PriceCheck, Href = "/microfinance/collateral-valuations", Action = FshActions.View, Resource = FshResources.CollateralValuations, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Collateral Insurances", Icon = Icons.Material.Filled.HealthAndSafety, Href = "/microfinance/collateral-insurances", Action = FshActions.View, Resource = FshResources.CollateralInsurances, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Collateral Releases", Icon = Icons.Material.Filled.LockOpen, Href = "/microfinance/collateral-releases", Action = FshActions.View, Resource = FshResources.CollateralReleases, PageStatus = PageStatus.InProgress },
                        
                        // ========== COLLECTIONS & RECOVERY ==========
                        new MenuSectionSubItemModel { Title = "Collections & Recovery", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Collection Cases", Icon = Icons.Material.Filled.Gavel, Href = "/microfinance/collection-cases", Action = FshActions.View, Resource = FshResources.CollectionCases, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Collection Actions", Icon = Icons.Material.Filled.PlaylistAddCheck, Href = "/microfinance/collection-actions", Action = FshActions.View, Resource = FshResources.CollectionActions, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Collection Strategies", Icon = Icons.Material.Filled.Lightbulb, Href = "/microfinance/collection-strategies", Action = FshActions.View, Resource = FshResources.CollectionStrategies, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Promise to Pays", Icon = Icons.Material.Filled.Handshake, Href = "/microfinance/promise-to-pays", Action = FshActions.View, Resource = FshResources.PromiseToPays, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Debt Settlements", Icon = Icons.Material.Filled.Balance, Href = "/microfinance/debt-settlements", Action = FshActions.View, Resource = FshResources.DebtSettlements, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Legal Actions", Icon = Icons.Material.Filled.Gavel, Href = "/microfinance/legal-actions", Action = FshActions.View, Resource = FshResources.LegalActions, PageStatus = PageStatus.InProgress },
                        
                        // ========== TRANSACTIONS ==========
                        new MenuSectionSubItemModel { Title = "Transactions", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Savings Transactions", Icon = Icons.Material.Filled.Receipt, Href = "/microfinance/savings-transactions", Action = FshActions.View, Resource = FshResources.SavingsTransactions, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Share Transactions", Icon = Icons.Material.Filled.SwapHoriz, Href = "/microfinance/share-transactions", Action = FshActions.View, Resource = FshResources.ShareTransactions, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Fee Charges", Icon = Icons.Material.Filled.ReceiptLong, Href = "/microfinance/fee-charges", Action = FshActions.View, Resource = FshResources.FeeCharges, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Mobile Transactions", Icon = Icons.Material.Filled.PhoneIphone, Href = "/microfinance/mobile-transactions", Action = FshActions.View, Resource = FshResources.MobileTransactions, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Investment Transactions", Icon = Icons.Material.Filled.TrendingUp, Href = "/microfinance/investment-transactions", Action = FshActions.View, Resource = FshResources.InvestmentTransactions, PageStatus = PageStatus.InProgress },
                        
                        // ========== INSURANCE ==========
                        new MenuSectionSubItemModel { Title = "Insurance", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Insurance Claims", Icon = Icons.Material.Filled.MedicalServices, Href = "/microfinance/insurance-claims", Action = FshActions.View, Resource = FshResources.InsuranceClaims, PageStatus = PageStatus.InProgress },
                        
                        // ========== RISK & COMPLIANCE ==========
                        new MenuSectionSubItemModel { Title = "Risk & Compliance", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "AML Alerts", Icon = Icons.Material.Filled.Warning, Href = "/microfinance/aml-alerts", Action = FshActions.View, Resource = FshResources.AmlAlerts, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Credit Scores", Icon = Icons.Material.Filled.Score, Href = "/microfinance/credit-scores", Action = FshActions.View, Resource = FshResources.CreditScores, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Credit Bureau Inquiries", Icon = Icons.Material.Filled.Search, Href = "/microfinance/credit-bureau-inquiries", Action = FshActions.View, Resource = FshResources.CreditBureauInquiries, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Credit Bureau Reports", Icon = Icons.Material.Filled.Assessment, Href = "/microfinance/credit-bureau-reports", Action = FshActions.View, Resource = FshResources.CreditBureauReports, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Risk Alerts", Icon = Icons.Material.Filled.NotificationsActive, Href = "/microfinance/risk-alerts", Action = FshActions.View, Resource = FshResources.RiskAlerts, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Risk Categories", Icon = Icons.Material.Filled.Folder, Href = "/microfinance/risk-categories", Action = FshActions.View, Resource = FshResources.RiskCategories, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Risk Indicators", Icon = Icons.Material.Filled.Timeline, Href = "/microfinance/risk-indicators", Action = FshActions.View, Resource = FshResources.RiskIndicators, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Documents", Icon = Icons.Material.Filled.Description, Href = "/microfinance/documents", Action = FshActions.View, Resource = FshResources.Documents, PageStatus = PageStatus.InProgress },
                        
                        // ========== WORKFLOWS & APPROVALS ==========
                        new MenuSectionSubItemModel { Title = "Workflows & Approvals", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Approval Workflows", Icon = Icons.Material.Filled.Approval, Href = "/microfinance/approval-workflows", Action = FshActions.View, Resource = FshResources.ApprovalWorkflows, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Approval Requests", Icon = Icons.Material.Filled.ThumbUp, Href = "/microfinance/approval-requests", Action = FshActions.View, Resource = FshResources.ApprovalRequests, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Customer Cases", Icon = Icons.Material.Filled.SupportAgent, Href = "/microfinance/customer-cases", Action = FshActions.View, Resource = FshResources.CustomerCases, PageStatus = PageStatus.InProgress },
                        
                        // ========== COMMUNICATIONS ==========
                        new MenuSectionSubItemModel { Title = "Communications", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Communication Templates", Icon = Icons.Material.Filled.TextSnippet, Href = "/microfinance/communication-templates", Action = FshActions.View, Resource = FshResources.CommunicationTemplates, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Communication Logs", Icon = Icons.Material.Filled.Chat, Href = "/microfinance/communication-logs", Action = FshActions.View, Resource = FshResources.CommunicationLogs, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Marketing Campaigns", Icon = Icons.Material.Filled.Campaign, Href = "/microfinance/marketing-campaigns", Action = FshActions.View, Resource = FshResources.MarketingCampaigns, PageStatus = PageStatus.InProgress },
                        
                        // ========== DIGITAL CHANNELS ==========
                        new MenuSectionSubItemModel { Title = "Digital Channels", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Agent Bankings", Icon = Icons.Material.Filled.Store, Href = "/microfinance/agent-bankings", Action = FshActions.View, Resource = FshResources.AgentBankings, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Mobile Wallets", Icon = Icons.Material.Filled.PhoneAndroid, Href = "/microfinance/mobile-wallets", Action = FshActions.View, Resource = FshResources.MobileWallets, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Payment Gateways", Icon = Icons.Material.Filled.Payment, Href = "/microfinance/payment-gateways", Action = FshActions.View, Resource = FshResources.PaymentGateways, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "QR Payments", Icon = Icons.Material.Filled.QrCode, Href = "/microfinance/qr-payments", Action = FshActions.View, Resource = FshResources.QrPayments, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "USSD Sessions", Icon = Icons.Material.Filled.Dialpad, Href = "/microfinance/ussd-sessions", Action = FshActions.View, Resource = FshResources.UssdSessions, PageStatus = PageStatus.InProgress },
                        
                        // ========== CASH MANAGEMENT ==========
                        new MenuSectionSubItemModel { Title = "Cash Management", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Cash Vaults", Icon = Icons.Material.Filled.AccountBalance, Href = "/microfinance/cash-vaults", Action = FshActions.View, Resource = FshResources.CashVaults, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Teller Sessions", Icon = Icons.Material.Filled.PointOfSale, Href = "/microfinance/teller-sessions", Action = FshActions.View, Resource = FshResources.TellerSessions, PageStatus = PageStatus.InProgress },
                        
                        // ========== REPORTING ==========
                        new MenuSectionSubItemModel { Title = "Reporting", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Report Definitions", Icon = Icons.Material.Filled.ListAlt, Href = "/microfinance/report-definitions", Action = FshActions.View, Resource = FshResources.ReportDefinitions, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Report Generations", Icon = Icons.Material.Filled.Print, Href = "/microfinance/report-generations", Action = FshActions.View, Resource = FshResources.ReportGenerations, PageStatus = PageStatus.InProgress },
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
                        new MenuSectionSubItemModel { Title = "Organizational Units", Icon = Icons.Material.Filled.AccountTree, Href = "/hr/organizational-units", Action = FshActions.View, Resource = FshResources.Organization, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Departments", Icon = Icons.Material.Filled.Business, Href = "/hr/departments", Action = FshActions.View, Resource = FshResources.Organization, PageStatus = PageStatus.ComingSoon },
                        new MenuSectionSubItemModel { Title = "Designations", Icon = Icons.Material.Filled.WorkOutline, Href = "/hr/designations", Action = FshActions.View, Resource = FshResources.Employees, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Designation Assignments", Icon = Icons.Material.Filled.AssignmentTurnedIn, Href = "/hr/designation-assignments", Action = FshActions.View, Resource = FshResources.Employees, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Shifts", Icon = Icons.Material.Filled.Schedule, Href = "/hr/shifts", Action = FshActions.View, Resource = FshResources.Attendance, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Holidays", Icon = Icons.Material.Filled.EventNote, Href = "/hr/holidays", Action = FshActions.View, Resource = FshResources.Attendance, PageStatus = PageStatus.Completed },
                        
                        // ========== EMPLOYEE MANAGEMENT ==========
                        new MenuSectionSubItemModel { Title = "Employee Management", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Employees", Icon = Icons.Material.Filled.Badge, Href = "/hr/employees", Action = FshActions.View, Resource = FshResources.Employees, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Employee Contacts", Icon = Icons.Material.Filled.Contacts, Href = "/hr/employee-contacts", Action = FshActions.View, Resource = FshResources.Employees, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Employee Dependents", Icon = Icons.Material.Filled.FamilyRestroom, Href = "/hr/employee-dependents", Action = FshActions.View, Resource = FshResources.Employees, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Employee Documents", Icon = Icons.Material.Filled.Description, Href = "/hr/employee-documents", Action = FshActions.View, Resource = FshResources.Employees, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Employee Education", Icon = Icons.Material.Filled.School, Href = "/hr/employee-education", Action = FshActions.View, Resource = FshResources.Employees, PageStatus = PageStatus.InProgress },
                        
                        // ========== TIME & ATTENDANCE ==========
                        new MenuSectionSubItemModel { Title = "Time & Attendance", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Attendance", Icon = Icons.Material.Filled.Fingerprint, Href = "/hr/attendance", Action = FshActions.View, Resource = FshResources.Attendance, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Timesheets", Icon = Icons.Material.Filled.AccessTime, Href = "/hr/timesheets", Action = FshActions.View, Resource = FshResources.Timesheets, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Timesheet Lines", Icon = Icons.Material.Filled.ListAlt, Href = "/hr/timesheet-lines", Action = FshActions.View, Resource = FshResources.Timesheets, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Shift Assignments", Icon = Icons.Material.Filled.AssignmentInd, Href = "/hr/shift-assignments", Action = FshActions.View, Resource = FshResources.Attendance, PageStatus = PageStatus.InProgress },
                        
                        // ========== LEAVE MANAGEMENT ==========
                        new MenuSectionSubItemModel { Title = "Leave Management", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Leave Types", Icon = Icons.Material.Filled.Category, Href = "/hr/leave-types", Action = FshActions.View, Resource = FshResources.Leaves, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Leave Requests", Icon = Icons.Material.Filled.EventAvailable, Href = "/hr/leave-requests", Action = FshActions.View, Resource = FshResources.Leaves, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Leave Balances", Icon = Icons.Material.Filled.AccountBalanceWallet, Href = "/hr/leave-balances", Action = FshActions.View, Resource = FshResources.Leaves, PageStatus = PageStatus.InProgress },
                        
                        // ========== PAYROLL ==========
                        new MenuSectionSubItemModel { Title = "Payroll", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Payroll Run", Icon = Icons.Material.Filled.Payments, Href = "/hr/payrolls", Action = FshActions.View, Resource = FshResources.Payroll, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Payroll Lines", Icon = Icons.Material.Filled.ViewList, Href = "/hr/payroll-lines", Action = FshActions.View, Resource = FshResources.Payroll, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Employee Pays", Icon = Icons.Material.Filled.LocalAtm, Href = "/hr/employee-pays", Action = FshActions.View, Resource = FshResources.Payroll, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Pay Components", Icon = Icons.Material.Filled.AttachMoney, Href = "/hr/pay-components", Action = FshActions.View, Resource = FshResources.Payroll, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Pay Component Rates", Icon = Icons.Material.Filled.TrendingUp, Href = "/hr/pay-component-rates", Action = FshActions.View, Resource = FshResources.Payroll, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Employee Pay Components", Icon = Icons.Material.Filled.PersonOutline, Href = "/hr/employee-pay-components", Action = FshActions.View, Resource = FshResources.Payroll, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Deductions", Icon = Icons.Material.Filled.RemoveCircleOutline, Href = "/hr/deductions", Action = FshActions.View, Resource = FshResources.Payroll, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Payroll Deductions", Icon = Icons.Material.Filled.MoneyOff, Href = "/hr/payroll-deductions", Action = FshActions.View, Resource = FshResources.Payroll, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Tax Brackets", Icon = Icons.Material.Filled.AccountBalance, Href = "/hr/tax-brackets", Action = FshActions.View, Resource = FshResources.Taxes, PageStatus = PageStatus.Completed },
                        new MenuSectionSubItemModel { Title = "Taxes", Icon = Icons.Material.Filled.Receipt, Href = "/hr/taxes", Action = FshActions.View, Resource = FshResources.Taxes, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Bank Accounts", Icon = Icons.Material.Filled.AccountBalance, Href = "/hr/employee-bank-accounts", Action = FshActions.View, Resource = FshResources.Payroll, PageStatus = PageStatus.Completed },
                        
                        // ========== BENEFITS ==========
                        new MenuSectionSubItemModel { Title = "Benefits & Enrollment", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Benefits", Icon = Icons.Material.Filled.CardGiftcard, Href = "/hr/benefits", Action = FshActions.View, Resource = FshResources.Benefits, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Benefit Enrollments", Icon = Icons.Material.Filled.HowToReg, Href = "/hr/benefit-enrollments", Action = FshActions.View, Resource = FshResources.Benefits, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Benefit Allocations", Icon = Icons.Material.Filled.LocalOffer, Href = "/hr/benefit-allocations", Action = FshActions.View, Resource = FshResources.Benefits, PageStatus = PageStatus.InProgress },
                        
                        // ========== DOCUMENTS & REPORTS ==========
                        new MenuSectionSubItemModel { Title = "Documents & Reports", IsGroupHeader = true },
                        new MenuSectionSubItemModel { Title = "Document Templates", Icon = Icons.Material.Filled.Article, Href = "/hr/document-templates", Action = FshActions.View, Resource = FshResources.Employees, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Generated Documents", Icon = Icons.Material.Filled.FileCopy, Href = "/hr/generated-documents", Action = FshActions.View, Resource = FshResources.Employees, PageStatus = PageStatus.InProgress },
                        new MenuSectionSubItemModel { Title = "Performance Reviews", Icon = Icons.Material.Filled.Assessment, Href = "/hr/performance-reviews", Action = FshActions.View, Resource = FshResources.Employees, PageStatus = PageStatus.InProgress },
                        
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
                },
                
                new MenuSectionItemModel
                {
                    Title = "Messaging",
                    Icon = Icons.Material.Filled.Chat,
                    Href = "/messaging",
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


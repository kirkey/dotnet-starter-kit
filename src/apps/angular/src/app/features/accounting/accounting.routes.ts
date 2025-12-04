import { Routes } from '@angular/router';

export const ACCOUNTING_ROUTES: Routes = [
  {
    path: '',
    redirectTo: 'chart-of-accounts',
    pathMatch: 'full'
  },
  // Core Accounting
  {
    path: 'chart-of-accounts',
    loadComponent: () => import('./chart-of-accounts/chart-of-accounts.component').then(m => m.ChartOfAccountsComponent),
    title: 'Chart of Accounts'
  },
  {
    path: 'journal-entries',
    loadComponent: () => import('./journal-entries/journal-entries.component').then(m => m.JournalEntriesComponent),
    title: 'Journal Entries'
  },
  {
    path: 'general-ledger',
    loadComponent: () => import('./general-ledger/general-ledger.component').then(m => m.GeneralLedgerComponent),
    title: 'General Ledger'
  },
  {
    path: 'accounting-periods',
    loadComponent: () => import('./accounting-periods/accounting-periods.component').then(m => m.AccountingPeriodsComponent),
    title: 'Accounting Periods'
  },
  {
    path: 'fiscal-period-close',
    loadComponent: () => import('./fiscal-period-close/fiscal-period-close.component').then(m => m.FiscalPeriodCloseComponent),
    title: 'Fiscal Period Close'
  },
  {
    path: 'trial-balance',
    loadComponent: () => import('./trial-balance/trial-balance.component').then(m => m.TrialBalanceComponent),
    title: 'Trial Balance'
  },
  {
    path: 'financial-statements',
    loadComponent: () => import('./financial-statements/financial-statements.component').then(m => m.FinancialStatementsComponent),
    title: 'Financial Statements'
  },
  {
    path: 'retained-earnings',
    loadComponent: () => import('./retained-earnings/retained-earnings.component').then(m => m.RetainedEarningsComponent),
    title: 'Retained Earnings'
  },
  // Accounts Payable
  {
    path: 'vendors',
    loadComponent: () => import('./vendors/vendors.component').then(m => m.VendorsComponent),
    title: 'Vendors'
  },
  {
    path: 'bills',
    loadComponent: () => import('./bills/bills.component').then(m => m.BillsComponent),
    title: 'Bills'
  },
  {
    path: 'ap-accounts',
    loadComponent: () => import('./ap-accounts/ap-accounts.component').then(m => m.ApAccountsComponent),
    title: 'AP Accounts'
  },
  {
    path: 'payees',
    loadComponent: () => import('./payees/payees.component').then(m => m.PayeesComponent),
    title: 'Payees'
  },
  // Accounts Receivable
  {
    path: 'customers',
    loadComponent: () => import('./customers/customers.component').then(m => m.CustomersComponent),
    title: 'Customers'
  },
  {
    path: 'invoices',
    loadComponent: () => import('./invoices/invoices.component').then(m => m.InvoicesComponent),
    title: 'Invoices'
  },
  {
    path: 'ar-accounts',
    loadComponent: () => import('./ar-accounts/ar-accounts.component').then(m => m.ArAccountsComponent),
    title: 'AR Accounts'
  },
  {
    path: 'payments',
    loadComponent: () => import('./payments/payments.component').then(m => m.PaymentsComponent),
    title: 'Payments'
  },
  {
    path: 'credit-memos',
    loadComponent: () => import('./credit-memos/credit-memos.component').then(m => m.CreditMemosComponent),
    title: 'Credit Memos'
  },
  {
    path: 'debit-memos',
    loadComponent: () => import('./debit-memos/debit-memos.component').then(m => m.DebitMemosComponent),
    title: 'Debit Memos'
  },
  {
    path: 'write-offs',
    loadComponent: () => import('./write-offs/write-offs.component').then(m => m.WriteOffsComponent),
    title: 'Write Offs'
  },
  // Banking
  {
    path: 'banks',
    loadComponent: () => import('./banks/banks.component').then(m => m.BanksComponent),
    title: 'Banks'
  },
  {
    path: 'bank-reconciliations',
    loadComponent: () => import('./bank-reconciliations/bank-reconciliations.component').then(m => m.BankReconciliationsComponent),
    title: 'Bank Reconciliations'
  },
  {
    path: 'checks',
    loadComponent: () => import('./checks/checks.component').then(m => m.ChecksComponent),
    title: 'Checks'
  },
  {
    path: 'account-reconciliations',
    loadComponent: () => import('./account-reconciliations/account-reconciliations.component').then(m => m.AccountReconciliationsComponent),
    title: 'Account Reconciliations'
  },
  // Budgeting & Projects
  {
    path: 'budgets',
    loadComponent: () => import('./budgets/budgets.component').then(m => m.BudgetsComponent),
    title: 'Budgets'
  },
  {
    path: 'projects',
    loadComponent: () => import('./projects/projects.component').then(m => m.ProjectsComponent),
    title: 'Projects'
  },
  // Fixed Assets
  {
    path: 'fixed-assets',
    loadComponent: () => import('./fixed-assets/fixed-assets.component').then(m => m.FixedAssetsComponent),
    title: 'Fixed Assets'
  },
  {
    path: 'depreciation-methods',
    loadComponent: () => import('./depreciation-methods/depreciation-methods.component').then(m => m.DepreciationMethodsComponent),
    title: 'Depreciation Methods'
  },
  // Inventory
  {
    path: 'inventory',
    loadComponent: () => import('./inventory/inventory.component').then(m => m.InventoryComponent),
    title: 'Inventory'
  },
  // Period-End
  {
    path: 'accruals',
    loadComponent: () => import('./accruals/accruals.component').then(m => m.AccrualsComponent),
    title: 'Accruals'
  },
  {
    path: 'prepaid-expenses',
    loadComponent: () => import('./prepaid-expenses/prepaid-expenses.component').then(m => m.PrepaidExpensesComponent),
    title: 'Prepaid Expenses'
  },
  {
    path: 'deferred-revenue',
    loadComponent: () => import('./deferred-revenue/deferred-revenue.component').then(m => m.DeferredRevenueComponent),
    title: 'Deferred Revenue'
  },
  {
    path: 'recurring-entries',
    loadComponent: () => import('./recurring-entries/recurring-entries.component').then(m => m.RecurringEntriesComponent),
    title: 'Recurring Entries'
  },
  {
    path: 'posting-batches',
    loadComponent: () => import('./posting-batches/posting-batches.component').then(m => m.PostingBatchesComponent),
    title: 'Posting Batches'
  },
  // Reference Data
  {
    path: 'tax-codes',
    loadComponent: () => import('./tax-codes/tax-codes.component').then(m => m.TaxCodesComponent),
    title: 'Tax Codes'
  }
];

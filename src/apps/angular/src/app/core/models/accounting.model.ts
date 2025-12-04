// ============================================================================
// ACCOUNTING MODELS - Comprehensive interfaces for the accounting module
// ============================================================================

// ============================================================================
// ENUMS
// ============================================================================

export enum AccountType {
  Asset = 'Asset',
  Liability = 'Liability',
  Equity = 'Equity',
  Revenue = 'Revenue',
  Expense = 'Expense'
}

export enum AccountSubType {
  CurrentAsset = 'CurrentAsset',
  FixedAsset = 'FixedAsset',
  OtherAsset = 'OtherAsset',
  CurrentLiability = 'CurrentLiability',
  LongTermLiability = 'LongTermLiability',
  Equity = 'Equity',
  Revenue = 'Revenue',
  CostOfGoodsSold = 'CostOfGoodsSold',
  OperatingExpense = 'OperatingExpense',
  OtherExpense = 'OtherExpense'
}

export enum JournalEntryStatus {
  Draft = 'Draft',
  Pending = 'Pending',
  Approved = 'Approved',
  Posted = 'Posted',
  Rejected = 'Rejected',
  Reversed = 'Reversed'
}

export enum PeriodStatus {
  Open = 'Open',
  Closed = 'Closed',
  Locked = 'Locked'
}

export enum CloseType {
  Monthly = 'Monthly',
  Quarterly = 'Quarterly',
  Annual = 'Annual'
}

export enum ApprovalStatus {
  Draft = 'Draft',
  Pending = 'Pending',
  Approved = 'Approved',
  Rejected = 'Rejected'
}

export enum PaymentStatus {
  Unpaid = 'Unpaid',
  PartiallyPaid = 'PartiallyPaid',
  Paid = 'Paid',
  Overpaid = 'Overpaid'
}

export enum ReconciliationStatus {
  InProgress = 'InProgress',
  Completed = 'Completed',
  Approved = 'Approved'
}

export enum CheckStatus {
  Issued = 'Issued',
  Cleared = 'Cleared',
  Voided = 'Voided',
  StopPayment = 'StopPayment'
}

export enum DepreciationMethod {
  StraightLine = 'StraightLine',
  DecliningBalance = 'DecliningBalance',
  DoubleDecliningBalance = 'DoubleDecliningBalance',
  SumOfYearsDigits = 'SumOfYearsDigits',
  UnitsOfProduction = 'UnitsOfProduction'
}

export enum FrequencyType {
  Daily = 'Daily',
  Weekly = 'Weekly',
  BiWeekly = 'BiWeekly',
  Monthly = 'Monthly',
  Quarterly = 'Quarterly',
  SemiAnnually = 'SemiAnnually',
  Annually = 'Annually'
}

export enum TaxType {
  Sales = 'Sales',
  Purchase = 'Purchase',
  Both = 'Both'
}

export enum BudgetType {
  Operating = 'Operating',
  Capital = 'Capital',
  Cash = 'Cash',
  Project = 'Project'
}

export enum ProjectStatus {
  Planning = 'Planning',
  Active = 'Active',
  OnHold = 'OnHold',
  Completed = 'Completed',
  Cancelled = 'Cancelled'
}

export enum AccrualType {
  Expense = 'Expense',
  Revenue = 'Revenue'
}

// ============================================================================
// BASE INTERFACES
// ============================================================================

export interface BaseEntity {
  id: string;
  createdOn?: Date;
  createdBy?: string;
  lastModifiedOn?: Date;
  lastModifiedBy?: string;
}

export interface AuditableEntity extends BaseEntity {
  isActive: boolean;
  notes?: string;
}

// ============================================================================
// CHART OF ACCOUNTS
// ============================================================================

export interface ChartOfAccount extends AuditableEntity {
  accountCode: string;
  accountName: string;
  accountType: AccountType;
  accountSubType?: AccountSubType;
  parentAccountId?: string;
  parentAccountCode?: string;
  parentAccountName?: string;
  usoaCategory?: string;
  usoaCategoryCode?: string;
  description?: string;
  currentBalance: number;
  isHeader: boolean;
  isBankAccount: boolean;
  bankAccountNumber?: string;
  level: number;
  sortOrder: number;
  normalBalance: 'Debit' | 'Credit';
}

export interface CreateChartOfAccountRequest {
  accountCode: string;
  accountName: string;
  accountType: AccountType;
  accountSubType?: AccountSubType;
  parentAccountId?: string;
  usoaCategory?: string;
  description?: string;
  isHeader?: boolean;
  isBankAccount?: boolean;
  bankAccountNumber?: string;
}

export interface UpdateChartOfAccountRequest extends CreateChartOfAccountRequest {
  id: string;
  isActive: boolean;
}

// ============================================================================
// JOURNAL ENTRIES
// ============================================================================

export interface JournalEntry extends AuditableEntity {
  entryNumber: string;
  entryDate: Date;
  referenceNumber?: string;
  description: string;
  source: string;
  status: JournalEntryStatus;
  periodId: string;
  periodName?: string;
  totalDebits: number;
  totalCredits: number;
  isBalanced: boolean;
  isAutoReversing: boolean;
  reversalDate?: Date;
  reversedFromId?: string;
  reversedToId?: string;
  approvedBy?: string;
  approvedOn?: Date;
  postedBy?: string;
  postedOn?: Date;
  rejectedBy?: string;
  rejectedOn?: Date;
  rejectionReason?: string;
  lines: JournalEntryLine[];
}

export interface JournalEntryLine extends BaseEntity {
  journalEntryId: string;
  lineNumber: number;
  accountId: string;
  accountCode: string;
  accountName: string;
  description?: string;
  debitAmount: number;
  creditAmount: number;
  costCenterId?: string;
  costCenterName?: string;
  departmentId?: string;
  departmentName?: string;
  projectId?: string;
  projectName?: string;
}

export interface CreateJournalEntryRequest {
  entryDate: Date;
  referenceNumber?: string;
  description: string;
  source?: string;
  periodId: string;
  isAutoReversing?: boolean;
  reversalDate?: Date;
  lines: CreateJournalEntryLineRequest[];
}

export interface CreateJournalEntryLineRequest {
  accountId: string;
  description?: string;
  debitAmount: number;
  creditAmount: number;
  costCenterId?: string;
  departmentId?: string;
  projectId?: string;
}

export interface UpdateJournalEntryRequest extends CreateJournalEntryRequest {
  id: string;
}

export interface ApproveJournalEntryRequest {
  id: string;
  comments?: string;
}

export interface RejectJournalEntryRequest {
  id: string;
  reason: string;
}

export interface ReverseJournalEntryRequest {
  id: string;
  reversalDate: Date;
  description?: string;
}

// ============================================================================
// GENERAL LEDGER
// ============================================================================

export interface GeneralLedgerEntry extends BaseEntity {
  transactionDate: Date;
  accountId: string;
  accountCode: string;
  accountName: string;
  journalEntryId: string;
  journalEntryNumber: string;
  description: string;
  debitAmount: number;
  creditAmount: number;
  runningBalance: number;
  periodId: string;
  periodName: string;
  source: string;
  referenceNumber?: string;
  isPosted: boolean;
  costCenterId?: string;
  costCenterName?: string;
  departmentId?: string;
  departmentName?: string;
}

export interface GeneralLedgerSummary {
  accountId: string;
  accountCode: string;
  accountName: string;
  accountType: AccountType;
  beginningBalance: number;
  totalDebits: number;
  totalCredits: number;
  netChange: number;
  endingBalance: number;
}

// ============================================================================
// ACCOUNTING PERIODS
// ============================================================================

export interface AccountingPeriod extends AuditableEntity {
  periodNumber: number;
  periodName: string;
  fiscalYear: number;
  startDate: Date;
  endDate: Date;
  status: PeriodStatus;
  isCurrent: boolean;
  isAdjustmentPeriod: boolean;
  closedBy?: string;
  closedOn?: Date;
  reopenedBy?: string;
  reopenedOn?: Date;
}

export interface CreateAccountingPeriodRequest {
  periodName: string;
  fiscalYear: number;
  startDate: Date;
  endDate: Date;
  isAdjustmentPeriod?: boolean;
}

export interface ClosePeriodRequest {
  periodId: string;
  reason?: string;
}

export interface ReopenPeriodRequest {
  periodId: string;
  reason: string;
}

// ============================================================================
// FISCAL PERIOD CLOSE
// ============================================================================

export interface FiscalPeriodClose extends AuditableEntity {
  closeNumber: string;
  closeType: CloseType;
  periodId: string;
  periodName: string;
  fiscalYear: number;
  periodStartDate: Date;
  periodEndDate: Date;
  status: ApprovalStatus;
  checklistItems: FiscalCloseChecklistItem[];
  closedBy?: string;
  closedOn?: Date;
  approvedBy?: string;
  approvedOn?: Date;
}

export interface FiscalCloseChecklistItem {
  id: string;
  taskName: string;
  description: string;
  isCompleted: boolean;
  completedBy?: string;
  completedOn?: Date;
  sortOrder: number;
  isRequired: boolean;
}

export interface CreateFiscalPeriodCloseRequest {
  closeType: CloseType;
  periodId: string;
}

// ============================================================================
// TRIAL BALANCE
// ============================================================================

export interface TrialBalance extends AuditableEntity {
  trialBalanceNumber: string;
  periodId: string;
  periodName: string;
  fiscalYear: number;
  asOfDate: Date;
  status: ApprovalStatus;
  totalDebits: number;
  totalCredits: number;
  isBalanced: boolean;
  isFinalized: boolean;
  finalizedBy?: string;
  finalizedOn?: Date;
  lines: TrialBalanceLine[];
}

export interface TrialBalanceLine {
  id: string;
  trialBalanceId: string;
  accountId: string;
  accountCode: string;
  accountName: string;
  accountType: AccountType;
  beginningDebit: number;
  beginningCredit: number;
  periodDebit: number;
  periodCredit: number;
  endingDebit: number;
  endingCredit: number;
  sortOrder: number;
}

export interface GenerateTrialBalanceRequest {
  periodId: string;
  asOfDate: Date;
  includeZeroBalances?: boolean;
}

// ============================================================================
// FINANCIAL STATEMENTS
// ============================================================================

export interface FinancialStatement {
  statementType: 'BalanceSheet' | 'IncomeStatement' | 'CashFlow';
  periodId: string;
  periodName: string;
  asOfDate: Date;
  sections: FinancialStatementSection[];
  totals: FinancialStatementTotals;
}

export interface FinancialStatementSection {
  name: string;
  accountType: AccountType;
  lines: FinancialStatementLine[];
  total: number;
}

export interface FinancialStatementLine {
  accountId: string;
  accountCode: string;
  accountName: string;
  currentPeriod: number;
  priorPeriod?: number;
  variance?: number;
  variancePercentage?: number;
  level: number;
  isSubtotal: boolean;
}

export interface FinancialStatementTotals {
  totalAssets?: number;
  totalLiabilities?: number;
  totalEquity?: number;
  totalRevenue?: number;
  totalExpenses?: number;
  netIncome?: number;
  cashFromOperations?: number;
  cashFromInvesting?: number;
  cashFromFinancing?: number;
  netCashChange?: number;
}

// ============================================================================
// RETAINED EARNINGS
// ============================================================================

export interface RetainedEarnings extends AuditableEntity {
  fiscalYear: number;
  periodId: string;
  periodName: string;
  beginningBalance: number;
  netIncome: number;
  dividendsDeclared: number;
  otherAdjustments: number;
  endingBalance: number;
  distributions: RetainedEarningsDistribution[];
}

export interface RetainedEarningsDistribution {
  id: string;
  distributionDate: Date;
  distributionType: 'Dividend' | 'Distribution' | 'Other';
  amount: number;
  description: string;
  approvedBy?: string;
  approvedOn?: Date;
}

// ============================================================================
// VENDORS (ACCOUNTS PAYABLE)
// ============================================================================

export interface Vendor extends AuditableEntity {
  vendorCode: string;
  vendorName: string;
  legalName?: string;
  taxId?: string;
  vendorType?: string;
  contactName?: string;
  email?: string;
  phone?: string;
  fax?: string;
  website?: string;
  address1?: string;
  address2?: string;
  city?: string;
  state?: string;
  postalCode?: string;
  country?: string;
  paymentTerms?: string;
  paymentTermsDays?: number;
  creditLimit?: number;
  currentBalance: number;
  expenseAccountId?: string;
  expenseAccountCode?: string;
  defaultTaxCodeId?: string;
  is1099Vendor: boolean;
  vendor1099Type?: string;
}

export interface CreateVendorRequest {
  vendorCode: string;
  vendorName: string;
  legalName?: string;
  taxId?: string;
  vendorType?: string;
  contactName?: string;
  email?: string;
  phone?: string;
  address1?: string;
  address2?: string;
  city?: string;
  state?: string;
  postalCode?: string;
  country?: string;
  paymentTerms?: string;
  paymentTermsDays?: number;
  creditLimit?: number;
  expenseAccountId?: string;
  defaultTaxCodeId?: string;
  is1099Vendor?: boolean;
}

export interface UpdateVendorRequest extends CreateVendorRequest {
  id: string;
  isActive: boolean;
}

// ============================================================================
// BILLS (ACCOUNTS PAYABLE)
// ============================================================================

export interface Bill extends AuditableEntity {
  billNumber: string;
  vendorId: string;
  vendorCode: string;
  vendorName: string;
  billDate: Date;
  dueDate: Date;
  referenceNumber?: string;
  description?: string;
  status: ApprovalStatus;
  paymentStatus: PaymentStatus;
  subtotal: number;
  taxAmount: number;
  totalAmount: number;
  paidAmount: number;
  balanceDue: number;
  periodId: string;
  periodName?: string;
  isPosted: boolean;
  postedOn?: Date;
  postedBy?: string;
  lines: BillLine[];
}

export interface BillLine {
  id: string;
  billId: string;
  lineNumber: number;
  accountId: string;
  accountCode: string;
  accountName: string;
  description?: string;
  quantity: number;
  unitPrice: number;
  amount: number;
  taxCodeId?: string;
  taxAmount: number;
  projectId?: string;
  costCenterId?: string;
}

export interface CreateBillRequest {
  vendorId: string;
  billDate: Date;
  dueDate: Date;
  referenceNumber?: string;
  description?: string;
  periodId: string;
  lines: CreateBillLineRequest[];
}

export interface CreateBillLineRequest {
  accountId: string;
  description?: string;
  quantity: number;
  unitPrice: number;
  taxCodeId?: string;
  projectId?: string;
  costCenterId?: string;
}

// ============================================================================
// AP ACCOUNTS
// ============================================================================

export interface ApAccount extends AuditableEntity {
  accountNumber: string;
  accountName: string;
  vendorId: string;
  vendorCode: string;
  vendorName: string;
  generalLedgerAccountId: string;
  glAccountCode: string;
  glAccountName: string;
  creditLimit: number;
  currentBalance: number;
  lastActivityDate?: Date;
}

// ============================================================================
// CUSTOMERS (ACCOUNTS RECEIVABLE)
// ============================================================================

export interface Customer extends AuditableEntity {
  customerNumber: string;
  customerName: string;
  legalName?: string;
  taxId?: string;
  customerType?: string;
  contactName?: string;
  email?: string;
  phone?: string;
  fax?: string;
  website?: string;
  address1?: string;
  address2?: string;
  city?: string;
  state?: string;
  postalCode?: string;
  country?: string;
  paymentTerms?: string;
  paymentTermsDays?: number;
  creditLimit: number;
  currentBalance: number;
  revenueAccountId?: string;
  revenueAccountCode?: string;
  defaultTaxCodeId?: string;
  creditHold: boolean;
  creditHoldReason?: string;
}

export interface CreateCustomerRequest {
  customerNumber: string;
  customerName: string;
  legalName?: string;
  taxId?: string;
  customerType?: string;
  contactName?: string;
  email?: string;
  phone?: string;
  address1?: string;
  address2?: string;
  city?: string;
  state?: string;
  postalCode?: string;
  country?: string;
  paymentTerms?: string;
  paymentTermsDays?: number;
  creditLimit?: number;
  revenueAccountId?: string;
  defaultTaxCodeId?: string;
}

export interface UpdateCustomerRequest extends CreateCustomerRequest {
  id: string;
  isActive: boolean;
}

// ============================================================================
// INVOICES (ACCOUNTS RECEIVABLE)
// ============================================================================

export interface Invoice extends AuditableEntity {
  invoiceNumber: string;
  customerId: string;
  customerNumber: string;
  customerName: string;
  invoiceDate: Date;
  dueDate: Date;
  referenceNumber?: string;
  description?: string;
  status: ApprovalStatus;
  paymentStatus: PaymentStatus;
  subtotal: number;
  taxAmount: number;
  totalAmount: number;
  paidAmount: number;
  balanceDue: number;
  periodId: string;
  periodName?: string;
  isPosted: boolean;
  postedOn?: Date;
  postedBy?: string;
  lines: InvoiceLine[];
}

export interface InvoiceLine {
  id: string;
  invoiceId: string;
  lineNumber: number;
  accountId: string;
  accountCode: string;
  accountName: string;
  description?: string;
  quantity: number;
  unitPrice: number;
  amount: number;
  taxCodeId?: string;
  taxAmount: number;
  projectId?: string;
  costCenterId?: string;
}

export interface CreateInvoiceRequest {
  customerId: string;
  invoiceDate: Date;
  dueDate: Date;
  referenceNumber?: string;
  description?: string;
  periodId: string;
  lines: CreateInvoiceLineRequest[];
}

export interface CreateInvoiceLineRequest {
  accountId: string;
  description?: string;
  quantity: number;
  unitPrice: number;
  taxCodeId?: string;
  projectId?: string;
  costCenterId?: string;
}

// ============================================================================
// AR ACCOUNTS
// ============================================================================

export interface ArAccount extends AuditableEntity {
  accountNumber: string;
  accountName: string;
  customerId: string;
  customerNumber: string;
  customerName: string;
  generalLedgerAccountId: string;
  glAccountCode: string;
  glAccountName: string;
  creditLimit: number;
  currentBalance: number;
  lastActivityDate?: Date;
}

// ============================================================================
// PAYMENTS
// ============================================================================

export interface Payment extends AuditableEntity {
  paymentNumber: string;
  paymentDate: Date;
  customerId: string;
  customerNumber: string;
  customerName: string;
  paymentMethod: string;
  referenceNumber?: string;
  checkNumber?: string;
  bankAccountId?: string;
  amount: number;
  appliedAmount: number;
  unappliedAmount: number;
  status: ApprovalStatus;
  periodId: string;
  periodName?: string;
  isPosted: boolean;
  allocations: PaymentAllocation[];
}

export interface PaymentAllocation {
  id: string;
  paymentId: string;
  invoiceId: string;
  invoiceNumber: string;
  invoiceAmount: number;
  invoiceBalance: number;
  appliedAmount: number;
}

export interface CreatePaymentRequest {
  paymentDate: Date;
  customerId: string;
  paymentMethod: string;
  referenceNumber?: string;
  checkNumber?: string;
  bankAccountId?: string;
  amount: number;
  periodId: string;
  allocations?: CreatePaymentAllocationRequest[];
}

export interface CreatePaymentAllocationRequest {
  invoiceId: string;
  appliedAmount: number;
}

// ============================================================================
// CREDIT/DEBIT MEMOS
// ============================================================================

export interface CreditMemo extends AuditableEntity {
  memoNumber: string;
  customerId: string;
  customerNumber: string;
  customerName: string;
  memoDate: Date;
  reason: string;
  description?: string;
  amount: number;
  appliedAmount: number;
  unappliedAmount: number;
  status: ApprovalStatus;
  periodId: string;
  isPosted: boolean;
  relatedInvoiceId?: string;
  relatedInvoiceNumber?: string;
}

export interface DebitMemo extends AuditableEntity {
  memoNumber: string;
  customerId: string;
  customerNumber: string;
  customerName: string;
  memoDate: Date;
  reason: string;
  description?: string;
  amount: number;
  status: ApprovalStatus;
  periodId: string;
  isPosted: boolean;
}

// ============================================================================
// WRITE-OFFS
// ============================================================================

export interface WriteOff extends AuditableEntity {
  writeOffNumber: string;
  writeOffDate: Date;
  customerId: string;
  customerNumber: string;
  customerName: string;
  invoiceId?: string;
  invoiceNumber?: string;
  amount: number;
  reason: string;
  description?: string;
  status: ApprovalStatus;
  periodId: string;
  isPosted: boolean;
  recoveryAmount: number;
  hasRecovery: boolean;
}

// ============================================================================
// BANKS
// ============================================================================

export interface Bank extends AuditableEntity {
  bankCode: string;
  bankName: string;
  accountNumber: string;
  routingNumber?: string;
  swiftCode?: string;
  ibanNumber?: string;
  accountType: string;
  glAccountId: string;
  glAccountCode: string;
  glAccountName: string;
  currentBalance: number;
  lastReconciledDate?: Date;
  lastReconciledBalance?: number;
  contactName?: string;
  contactPhone?: string;
  address1?: string;
  address2?: string;
  city?: string;
  state?: string;
  postalCode?: string;
  country?: string;
  imageUrl?: string;
}

export interface CreateBankRequest {
  bankCode: string;
  bankName: string;
  accountNumber: string;
  routingNumber?: string;
  swiftCode?: string;
  ibanNumber?: string;
  accountType: string;
  glAccountId: string;
  contactName?: string;
  contactPhone?: string;
  address1?: string;
  city?: string;
  state?: string;
  postalCode?: string;
  country?: string;
}

// ============================================================================
// BANK RECONCILIATIONS
// ============================================================================

export interface BankReconciliation extends AuditableEntity {
  reconciliationNumber: string;
  bankId: string;
  bankCode: string;
  bankName: string;
  statementDate: Date;
  statementEndingBalance: number;
  bookBalance: number;
  adjustedBookBalance: number;
  adjustedBankBalance: number;
  difference: number;
  status: ReconciliationStatus;
  isReconciled: boolean;
  reconciledBy?: string;
  reconciledOn?: Date;
  approvedBy?: string;
  approvedOn?: Date;
  outstandingDeposits: number;
  outstandingChecks: number;
  bankAdjustments: number;
  bookAdjustments: number;
  items: BankReconciliationItem[];
}

export interface BankReconciliationItem {
  id: string;
  reconciliationId: string;
  transactionDate: Date;
  transactionType: string;
  referenceNumber?: string;
  description: string;
  amount: number;
  isCleared: boolean;
  clearedDate?: Date;
  clearedBy?: string;
}

export interface CreateBankReconciliationRequest {
  bankId: string;
  statementDate: Date;
  statementEndingBalance: number;
}

// ============================================================================
// CHECKS
// ============================================================================

export interface Check extends AuditableEntity {
  checkNumber: string;
  bankId: string;
  bankCode: string;
  bankName: string;
  checkDate: Date;
  payeeType: 'Vendor' | 'Employee' | 'Other';
  payeeId?: string;
  payeeName: string;
  payeeAddress?: string;
  amount: number;
  memo?: string;
  status: CheckStatus;
  clearedDate?: Date;
  voidedDate?: Date;
  voidedBy?: string;
  voidReason?: string;
  stopPaymentDate?: Date;
  stopPaymentReason?: string;
  billId?: string;
  billNumber?: string;
}

export interface CreateCheckRequest {
  bankId: string;
  checkDate: Date;
  payeeType: 'Vendor' | 'Employee' | 'Other';
  payeeId?: string;
  payeeName: string;
  payeeAddress?: string;
  amount: number;
  memo?: string;
  billId?: string;
}

// ============================================================================
// ACCOUNT RECONCILIATIONS
// ============================================================================

export interface AccountReconciliation extends AuditableEntity {
  reconciliationNumber: string;
  accountId: string;
  accountCode: string;
  accountName: string;
  periodId: string;
  periodName: string;
  reconciliationDate: Date;
  beginningBalance: number;
  endingBalance: number;
  reconciledBalance: number;
  unreconciledDifference: number;
  status: ReconciliationStatus;
  preparedBy?: string;
  preparedOn?: Date;
  reviewedBy?: string;
  reviewedOn?: Date;
  approvedBy?: string;
  approvedOn?: Date;
  supportingDocuments?: string[];
}

// ============================================================================
// BUDGETS
// ============================================================================

export interface Budget extends AuditableEntity {
  budgetName: string;
  budgetCode: string;
  budgetType: BudgetType;
  fiscalYear: number;
  periodId?: string;
  periodName?: string;
  startDate: Date;
  endDate: Date;
  status: ApprovalStatus;
  totalBudgetedAmount: number;
  totalActualAmount: number;
  variance: number;
  variancePercentage: number;
  lines: BudgetLine[];
  approvedBy?: string;
  approvedOn?: Date;
}

export interface BudgetLine {
  id: string;
  budgetId: string;
  accountId: string;
  accountCode: string;
  accountName: string;
  jan: number;
  feb: number;
  mar: number;
  apr: number;
  may: number;
  jun: number;
  jul: number;
  aug: number;
  sep: number;
  oct: number;
  nov: number;
  dec: number;
  totalBudgeted: number;
  totalActual: number;
  variance: number;
}

export interface CreateBudgetRequest {
  budgetName: string;
  budgetCode: string;
  budgetType: BudgetType;
  fiscalYear: number;
  startDate: Date;
  endDate: Date;
  lines: CreateBudgetLineRequest[];
}

export interface CreateBudgetLineRequest {
  accountId: string;
  jan?: number;
  feb?: number;
  mar?: number;
  apr?: number;
  may?: number;
  jun?: number;
  jul?: number;
  aug?: number;
  sep?: number;
  oct?: number;
  nov?: number;
  dec?: number;
}

// ============================================================================
// PROJECTS
// ============================================================================

export interface Project extends AuditableEntity {
  projectCode: string;
  projectName: string;
  description?: string;
  clientId?: string;
  clientName?: string;
  projectManagerId?: string;
  projectManagerName?: string;
  startDate: Date;
  endDate?: Date;
  estimatedEndDate?: Date;
  status: ProjectStatus;
  budgetedAmount: number;
  actualAmount: number;
  billedAmount: number;
  costToComplete: number;
  percentComplete: number;
  profitMargin: number;
  revenueAccountId?: string;
  expenseAccountId?: string;
}

export interface CreateProjectRequest {
  projectCode: string;
  projectName: string;
  description?: string;
  clientId?: string;
  projectManagerId?: string;
  startDate: Date;
  estimatedEndDate?: Date;
  budgetedAmount?: number;
  revenueAccountId?: string;
  expenseAccountId?: string;
}

// ============================================================================
// FIXED ASSETS
// ============================================================================

export interface FixedAsset extends AuditableEntity {
  assetCode: string;
  assetName: string;
  description?: string;
  assetType: string;
  assetCategory?: string;
  serialNumber?: string;
  location?: string;
  department?: string;
  purchaseDate: Date;
  purchasePrice: number;
  vendorId?: string;
  vendorName?: string;
  invoiceNumber?: string;
  usefulLifeYears: number;
  usefulLifeMonths: number;
  salvageValue: number;
  depreciationMethodId: string;
  depreciationMethodName: string;
  fixedAssetAccountId: string;
  accumulatedDepreciationAccountId: string;
  depreciationExpenseAccountId: string;
  currentBookValue: number;
  accumulatedDepreciation: number;
  lastDepreciationDate?: Date;
  disposalDate?: Date;
  disposalAmount?: number;
  disposalGainLoss?: number;
  isFullyDepreciated: boolean;
  isDisposed: boolean;
  imageUrl?: string;
}

export interface CreateFixedAssetRequest {
  assetCode: string;
  assetName: string;
  description?: string;
  assetType: string;
  assetCategory?: string;
  serialNumber?: string;
  location?: string;
  department?: string;
  purchaseDate: Date;
  purchasePrice: number;
  vendorId?: string;
  invoiceNumber?: string;
  usefulLifeYears: number;
  usefulLifeMonths?: number;
  salvageValue?: number;
  depreciationMethodId: string;
  fixedAssetAccountId: string;
  accumulatedDepreciationAccountId: string;
  depreciationExpenseAccountId: string;
}

export interface DisposeFixedAssetRequest {
  id: string;
  disposalDate: Date;
  disposalAmount: number;
  disposalReason?: string;
}

export interface DepreciateFixedAssetRequest {
  assetIds: string[];
  throughDate: Date;
}

// ============================================================================
// DEPRECIATION METHODS
// ============================================================================

export interface DepreciationMethodConfig extends AuditableEntity {
  methodCode: string;
  methodName: string;
  depreciationMethod: DepreciationMethod;
  description?: string;
  ratePercentage?: number;
  isDefault: boolean;
}

// ============================================================================
// INVENTORY
// ============================================================================

export interface InventoryItem extends AuditableEntity {
  sku: string;
  itemName: string;
  description?: string;
  category?: string;
  unitOfMeasure: string;
  quantityOnHand: number;
  quantityReserved: number;
  quantityAvailable: number;
  reorderPoint: number;
  reorderQuantity: number;
  unitCost: number;
  averageCost: number;
  lastCost: number;
  standardCost: number;
  sellingPrice: number;
  inventoryAccountId: string;
  cogsAccountId: string;
  revenueAccountId: string;
  defaultVendorId?: string;
  defaultVendorName?: string;
  location?: string;
  bin?: string;
  lastReceivedDate?: Date;
  lastSoldDate?: Date;
  imageUrl?: string;
}

export interface CreateInventoryItemRequest {
  sku: string;
  itemName: string;
  description?: string;
  category?: string;
  unitOfMeasure: string;
  quantityOnHand?: number;
  reorderPoint?: number;
  reorderQuantity?: number;
  unitCost?: number;
  sellingPrice?: number;
  inventoryAccountId: string;
  cogsAccountId: string;
  revenueAccountId: string;
  defaultVendorId?: string;
  location?: string;
  bin?: string;
}

export interface AdjustInventoryRequest {
  itemId: string;
  adjustmentType: 'Add' | 'Reduce';
  quantity: number;
  reason: string;
  unitCost?: number;
}

// ============================================================================
// ACCRUALS
// ============================================================================

export interface Accrual extends AuditableEntity {
  accrualNumber: string;
  accrualType: AccrualType;
  accrualDate: Date;
  periodId: string;
  periodName: string;
  accountId: string;
  accountCode: string;
  accountName: string;
  offsetAccountId: string;
  offsetAccountCode: string;
  offsetAccountName: string;
  amount: number;
  description: string;
  status: ApprovalStatus;
  isReversed: boolean;
  reversalDate?: Date;
  reversalPeriodId?: string;
  journalEntryId?: string;
  journalEntryNumber?: string;
}

export interface CreateAccrualRequest {
  accrualType: AccrualType;
  accrualDate: Date;
  periodId: string;
  accountId: string;
  offsetAccountId: string;
  amount: number;
  description: string;
  autoReverse?: boolean;
  reversalDate?: Date;
}

// ============================================================================
// PREPAID EXPENSES
// ============================================================================

export interface PrepaidExpense extends AuditableEntity {
  prepaidNumber: string;
  description: string;
  vendorId?: string;
  vendorName?: string;
  prepaidAccountId: string;
  prepaidAccountCode: string;
  prepaidAccountName: string;
  expenseAccountId: string;
  expenseAccountCode: string;
  expenseAccountName: string;
  originalAmount: number;
  amortizedAmount: number;
  remainingAmount: number;
  startDate: Date;
  endDate: Date;
  amortizationPeriods: number;
  amortizationPerPeriod: number;
  status: 'Active' | 'FullyAmortized' | 'Closed';
  amortizationSchedule: PrepaidAmortizationSchedule[];
}

export interface PrepaidAmortizationSchedule {
  id: string;
  prepaidExpenseId: string;
  periodId: string;
  periodName: string;
  scheduledDate: Date;
  amount: number;
  isAmortized: boolean;
  amortizedOn?: Date;
  journalEntryId?: string;
}

export interface CreatePrepaidExpenseRequest {
  description: string;
  vendorId?: string;
  prepaidAccountId: string;
  expenseAccountId: string;
  originalAmount: number;
  startDate: Date;
  endDate: Date;
  amortizationPeriods: number;
}

// ============================================================================
// DEFERRED REVENUE
// ============================================================================

export interface DeferredRevenue extends AuditableEntity {
  deferredNumber: string;
  description: string;
  customerId?: string;
  customerName?: string;
  deferredAccountId: string;
  deferredAccountCode: string;
  deferredAccountName: string;
  revenueAccountId: string;
  revenueAccountCode: string;
  revenueAccountName: string;
  originalAmount: number;
  recognizedAmount: number;
  remainingAmount: number;
  startDate: Date;
  endDate: Date;
  recognitionPeriods: number;
  recognitionPerPeriod: number;
  status: 'Active' | 'FullyRecognized' | 'Closed';
  recognitionSchedule: DeferredRevenueSchedule[];
}

export interface DeferredRevenueSchedule {
  id: string;
  deferredRevenueId: string;
  periodId: string;
  periodName: string;
  scheduledDate: Date;
  amount: number;
  isRecognized: boolean;
  recognizedOn?: Date;
  journalEntryId?: string;
}

export interface CreateDeferredRevenueRequest {
  description: string;
  customerId?: string;
  deferredAccountId: string;
  revenueAccountId: string;
  originalAmount: number;
  startDate: Date;
  endDate: Date;
  recognitionPeriods: number;
}

// ============================================================================
// RECURRING JOURNAL ENTRIES
// ============================================================================

export interface RecurringJournalEntry extends AuditableEntity {
  templateCode: string;
  templateName: string;
  description: string;
  frequency: FrequencyType;
  startDate: Date;
  endDate?: Date;
  nextRunDate: Date;
  lastRunDate?: Date;
  totalRuns: number;
  maxRuns?: number;
  amount: number;
  debitAccountId: string;
  debitAccountCode: string;
  debitAccountName: string;
  creditAccountId: string;
  creditAccountCode: string;
  creditAccountName: string;
  autoPost: boolean;
  requiresApproval: boolean;
  status: 'Active' | 'Paused' | 'Completed' | 'Cancelled';
}

export interface CreateRecurringJournalEntryRequest {
  templateCode: string;
  templateName: string;
  description: string;
  frequency: FrequencyType;
  startDate: Date;
  endDate?: Date;
  maxRuns?: number;
  amount: number;
  debitAccountId: string;
  creditAccountId: string;
  autoPost?: boolean;
  requiresApproval?: boolean;
}

// ============================================================================
// POSTING BATCHES
// ============================================================================

export interface PostingBatch extends AuditableEntity {
  batchNumber: string;
  batchName: string;
  batchDate: Date;
  periodId: string;
  periodName: string;
  source: string;
  entryCount: number;
  totalDebits: number;
  totalCredits: number;
  status: ApprovalStatus;
  isPosted: boolean;
  postedOn?: Date;
  postedBy?: string;
  entries: PostingBatchEntry[];
}

export interface PostingBatchEntry {
  id: string;
  batchId: string;
  journalEntryId: string;
  journalEntryNumber: string;
  entryDate: Date;
  description: string;
  debitAmount: number;
  creditAmount: number;
  isPosted: boolean;
}

// ============================================================================
// TAX CODES
// ============================================================================

export interface TaxCode extends AuditableEntity {
  taxCode: string;
  taxName: string;
  description?: string;
  taxType: TaxType;
  rate: number;
  effectiveDate: Date;
  expirationDate?: Date;
  jurisdiction?: string;
  taxAccountId: string;
  taxAccountCode: string;
  taxAccountName: string;
  isCompound: boolean;
  compoundOnTaxCodes?: string[];
  isDefault: boolean;
}

export interface CreateTaxCodeRequest {
  taxCode: string;
  taxName: string;
  description?: string;
  taxType: TaxType;
  rate: number;
  effectiveDate: Date;
  expirationDate?: Date;
  jurisdiction?: string;
  taxAccountId: string;
  isCompound?: boolean;
  compoundOnTaxCodes?: string[];
}

// ============================================================================
// PAYEES
// ============================================================================

export interface Payee extends AuditableEntity {
  payeeCode: string;
  payeeName: string;
  payeeType: 'Individual' | 'Company' | 'Government' | 'Other';
  taxId?: string;
  address1?: string;
  address2?: string;
  city?: string;
  state?: string;
  postalCode?: string;
  country?: string;
  phone?: string;
  email?: string;
  defaultExpenseAccountId?: string;
  defaultExpenseAccountCode?: string;
}

// ============================================================================
// COST CENTERS & DEPARTMENTS
// ============================================================================

export interface CostCenter extends AuditableEntity {
  costCenterCode: string;
  costCenterName: string;
  description?: string;
  managerId?: string;
  managerName?: string;
  parentCostCenterId?: string;
  parentCostCenterName?: string;
}

export interface Department extends AuditableEntity {
  departmentCode: string;
  departmentName: string;
  description?: string;
  managerId?: string;
  managerName?: string;
  parentDepartmentId?: string;
  parentDepartmentName?: string;
}

// ============================================================================
// SEARCH/FILTER REQUESTS
// ============================================================================

export interface AccountingSearchRequest {
  pageNumber: number;
  pageSize: number;
  searchTerm?: string;
  sortBy?: string;
  sortDescending?: boolean;
  periodId?: string;
  startDate?: Date;
  endDate?: Date;
  status?: string;
  accountId?: string;
  vendorId?: string;
  customerId?: string;
}

export interface PaginatedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

// ============================================================================
// USOA CATEGORIES
// ============================================================================

export interface UsoaCategory {
  code: string;
  name: string;
  description?: string;
  accountType: AccountType;
  sortOrder: number;
}

// ============================================================================
// DASHBOARD / SUMMARY MODELS
// ============================================================================

export interface AccountingSummary {
  totalAssets: number;
  totalLiabilities: number;
  totalEquity: number;
  totalRevenue: number;
  totalExpenses: number;
  netIncome: number;
  cashBalance: number;
  accountsReceivable: number;
  accountsPayable: number;
  currentRatio: number;
  quickRatio: number;
}

export interface AgingSummary {
  current: number;
  days1To30: number;
  days31To60: number;
  days61To90: number;
  over90Days: number;
  total: number;
}

export interface CashFlowSummary {
  operatingActivities: number;
  investingActivities: number;
  financingActivities: number;
  netChange: number;
  beginningCash: number;
  endingCash: number;
}

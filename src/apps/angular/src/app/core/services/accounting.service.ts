import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '@env/environment';
import {
  ChartOfAccount, CreateChartOfAccountRequest, UpdateChartOfAccountRequest,
  JournalEntry, CreateJournalEntryRequest, UpdateJournalEntryRequest,
  ApproveJournalEntryRequest, RejectJournalEntryRequest, ReverseJournalEntryRequest,
  GeneralLedgerEntry, GeneralLedgerSummary,
  AccountingPeriod, CreateAccountingPeriodRequest, ClosePeriodRequest, ReopenPeriodRequest,
  FiscalPeriodClose, CreateFiscalPeriodCloseRequest,
  TrialBalance, GenerateTrialBalanceRequest,
  FinancialStatement,
  RetainedEarnings,
  Vendor, CreateVendorRequest, UpdateVendorRequest,
  Bill, CreateBillRequest,
  ApAccount,
  Customer, CreateCustomerRequest, UpdateCustomerRequest,
  Invoice, CreateInvoiceRequest,
  ArAccount,
  Payment, CreatePaymentRequest,
  CreditMemo, DebitMemo, WriteOff,
  Bank, CreateBankRequest,
  BankReconciliation, CreateBankReconciliationRequest,
  Check, CreateCheckRequest,
  AccountReconciliation,
  Budget, CreateBudgetRequest,
  Project, CreateProjectRequest,
  FixedAsset, CreateFixedAssetRequest, DisposeFixedAssetRequest, DepreciateFixedAssetRequest,
  DepreciationMethodConfig,
  InventoryItem, CreateInventoryItemRequest, AdjustInventoryRequest,
  Accrual, CreateAccrualRequest,
  PrepaidExpense, CreatePrepaidExpenseRequest,
  DeferredRevenue, CreateDeferredRevenueRequest,
  RecurringJournalEntry, CreateRecurringJournalEntryRequest,
  PostingBatch,
  TaxCode, CreateTaxCodeRequest,
  Payee,
  CostCenter, Department, UsoaCategory,
  AccountingSearchRequest, PaginatedResult,
  AccountingSummary, AgingSummary, CashFlowSummary
} from '@core/models/accounting.model';

@Injectable({
  providedIn: 'root'
})
export class AccountingService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/v1/accounting`;

  // ============================================================================
  // CHART OF ACCOUNTS
  // ============================================================================

  getChartOfAccounts(request: AccountingSearchRequest): Observable<PaginatedResult<ChartOfAccount>> {
    return this.http.post<PaginatedResult<ChartOfAccount>>(`${this.baseUrl}/chart-of-accounts/search`, this.buildSearchRequest(request));
  }

  getChartOfAccountById(id: string): Observable<ChartOfAccount> {
    return this.http.get<ChartOfAccount>(`${this.baseUrl}/chart-of-accounts/${id}`);
  }

  createChartOfAccount(request: CreateChartOfAccountRequest): Observable<ChartOfAccount> {
    return this.http.post<ChartOfAccount>(`${this.baseUrl}/chart-of-accounts`, request);
  }

  updateChartOfAccount(request: UpdateChartOfAccountRequest): Observable<ChartOfAccount> {
    return this.http.put<ChartOfAccount>(`${this.baseUrl}/chart-of-accounts/${request.id}`, request);
  }

  deleteChartOfAccount(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/chart-of-accounts/${id}`);
  }

  getAccountsByType(accountType: string): Observable<ChartOfAccount[]> {
    return this.http.get<ChartOfAccount[]>(`${this.baseUrl}/chart-of-accounts/by-type/${accountType}`);
  }

  getUsoaCategories(): Observable<UsoaCategory[]> {
    return this.http.get<UsoaCategory[]>(`${this.baseUrl}/chart-of-accounts/usoa-categories`);
  }

  // ============================================================================
  // JOURNAL ENTRIES
  // ============================================================================

  getJournalEntries(request: AccountingSearchRequest): Observable<PaginatedResult<JournalEntry>> {
    return this.http.post<PaginatedResult<JournalEntry>>(`${this.baseUrl}/journal-entries/search`, this.buildSearchRequest(request));
  }

  getJournalEntryById(id: string): Observable<JournalEntry> {
    return this.http.get<JournalEntry>(`${this.baseUrl}/journal-entries/${id}`);
  }

  createJournalEntry(request: CreateJournalEntryRequest): Observable<JournalEntry> {
    return this.http.post<JournalEntry>(`${this.baseUrl}/journal-entries`, request);
  }

  updateJournalEntry(request: UpdateJournalEntryRequest): Observable<JournalEntry> {
    return this.http.put<JournalEntry>(`${this.baseUrl}/journal-entries/${request.id}`, request);
  }

  deleteJournalEntry(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/journal-entries/${id}`);
  }

  submitJournalEntryForApproval(id: string): Observable<JournalEntry> {
    return this.http.post<JournalEntry>(`${this.baseUrl}/journal-entries/${id}/submit`, {});
  }

  approveJournalEntry(request: ApproveJournalEntryRequest): Observable<JournalEntry> {
    return this.http.post<JournalEntry>(`${this.baseUrl}/journal-entries/${request.id}/approve`, request);
  }

  rejectJournalEntry(request: RejectJournalEntryRequest): Observable<JournalEntry> {
    return this.http.post<JournalEntry>(`${this.baseUrl}/journal-entries/${request.id}/reject`, request);
  }

  postJournalEntry(id: string): Observable<JournalEntry> {
    return this.http.post<JournalEntry>(`${this.baseUrl}/journal-entries/${id}/post`, {});
  }

  reverseJournalEntry(request: ReverseJournalEntryRequest): Observable<JournalEntry> {
    return this.http.post<JournalEntry>(`${this.baseUrl}/journal-entries/${request.id}/reverse`, request);
  }

  // ============================================================================
  // GENERAL LEDGER
  // ============================================================================

  getGeneralLedgerEntries(request: AccountingSearchRequest): Observable<PaginatedResult<GeneralLedgerEntry>> {
    return this.http.post<PaginatedResult<GeneralLedgerEntry>>(`${this.baseUrl}/general-ledger/search`, this.buildSearchRequest(request));
  }

  getGeneralLedgerByAccount(accountId: string, request: AccountingSearchRequest): Observable<PaginatedResult<GeneralLedgerEntry>> {
    const params = this.buildParams(request);
    return this.http.get<PaginatedResult<GeneralLedgerEntry>>(`${this.baseUrl}/general-ledger/account/${accountId}`, { params });
  }

  getGeneralLedgerSummary(periodId: string): Observable<GeneralLedgerSummary[]> {
    return this.http.get<GeneralLedgerSummary[]>(`${this.baseUrl}/general-ledger/summary/${periodId}`);
  }

  // ============================================================================
  // ACCOUNTING PERIODS
  // ============================================================================

  getAccountingPeriods(request: AccountingSearchRequest): Observable<PaginatedResult<AccountingPeriod>> {
    return this.http.post<PaginatedResult<AccountingPeriod>>(`${this.baseUrl}/accounting-periods/search`, this.buildSearchRequest(request));
  }

  getAccountingPeriodById(id: string): Observable<AccountingPeriod> {
    return this.http.get<AccountingPeriod>(`${this.baseUrl}/accounting-periods/${id}`);
  }

  getCurrentPeriod(): Observable<AccountingPeriod> {
    return this.http.get<AccountingPeriod>(`${this.baseUrl}/accounting-periods/current`);
  }

  getOpenPeriods(): Observable<AccountingPeriod[]> {
    return this.http.post<AccountingPeriod[]>(`${this.baseUrl}/accounting-periods/search`, { pageNumber: 1, pageSize: 100, status: 'Open' });
  }

  createAccountingPeriod(request: CreateAccountingPeriodRequest): Observable<AccountingPeriod> {
    return this.http.post<AccountingPeriod>(`${this.baseUrl}/accounting-periods`, request);
  }

  closePeriod(request: ClosePeriodRequest): Observable<AccountingPeriod> {
    return this.http.post<AccountingPeriod>(`${this.baseUrl}/accounting-periods/${request.periodId}/close`, request);
  }

  reopenPeriod(request: ReopenPeriodRequest): Observable<AccountingPeriod> {
    return this.http.post<AccountingPeriod>(`${this.baseUrl}/accounting-periods/${request.periodId}/reopen`, request);
  }

  // ============================================================================
  // FISCAL PERIOD CLOSE
  // ============================================================================

  getFiscalPeriodCloses(request: AccountingSearchRequest): Observable<PaginatedResult<FiscalPeriodClose>> {
    return this.http.post<PaginatedResult<FiscalPeriodClose>>(`${this.baseUrl}/fiscal-period-closes/search`, this.buildSearchRequest(request));
  }

  getFiscalPeriodCloseById(id: string): Observable<FiscalPeriodClose> {
    return this.http.get<FiscalPeriodClose>(`${this.baseUrl}/fiscal-period-closes/${id}`);
  }

  createFiscalPeriodClose(request: CreateFiscalPeriodCloseRequest): Observable<FiscalPeriodClose> {
    return this.http.post<FiscalPeriodClose>(`${this.baseUrl}/fiscal-period-closes`, request);
  }

  updateChecklistItem(closeId: string, itemId: string, isCompleted: boolean): Observable<FiscalPeriodClose> {
    return this.http.put<FiscalPeriodClose>(`${this.baseUrl}/fiscal-period-closes/${closeId}/checklist/${itemId}`, { isCompleted });
  }

  completeFiscalClose(id: string): Observable<FiscalPeriodClose> {
    return this.http.post<FiscalPeriodClose>(`${this.baseUrl}/fiscal-period-closes/${id}/complete`, {});
  }

  reopenFiscalClose(id: string, reason: string): Observable<FiscalPeriodClose> {
    return this.http.post<FiscalPeriodClose>(`${this.baseUrl}/fiscal-period-closes/${id}/reopen`, { reason });
  }

  // ============================================================================
  // TRIAL BALANCE
  // ============================================================================

  getTrialBalances(request: AccountingSearchRequest): Observable<PaginatedResult<TrialBalance>> {
    return this.http.post<PaginatedResult<TrialBalance>>(`${this.baseUrl}/trial-balance/search`, this.buildSearchRequest(request));
  }

  getTrialBalanceById(id: string): Observable<TrialBalance> {
    return this.http.get<TrialBalance>(`${this.baseUrl}/trial-balance/${id}`);
  }

  generateTrialBalance(request: GenerateTrialBalanceRequest): Observable<TrialBalance> {
    return this.http.post<TrialBalance>(`${this.baseUrl}/trial-balance/generate`, request);
  }

  finalizeTrialBalance(id: string): Observable<TrialBalance> {
    return this.http.post<TrialBalance>(`${this.baseUrl}/trial-balance/${id}/finalize`, {});
  }

  reopenTrialBalance(id: string, reason: string): Observable<TrialBalance> {
    return this.http.post<TrialBalance>(`${this.baseUrl}/trial-balance/${id}/reopen`, { reason });
  }

  exportTrialBalance(id: string, format: 'pdf' | 'excel'): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/trial-balance/${id}/export`, {
      params: { format },
      responseType: 'blob'
    });
  }

  // ============================================================================
  // FINANCIAL STATEMENTS
  // ============================================================================

  getBalanceSheet(periodId: string, asOfDate: Date): Observable<FinancialStatement> {
    return this.http.get<FinancialStatement>(`${this.baseUrl}/financial-statements/balance-sheet`, {
      params: { periodId, asOfDate: asOfDate.toISOString() }
    });
  }

  getIncomeStatement(periodId: string, startDate: Date, endDate: Date): Observable<FinancialStatement> {
    return this.http.get<FinancialStatement>(`${this.baseUrl}/financial-statements/income-statement`, {
      params: { periodId, startDate: startDate.toISOString(), endDate: endDate.toISOString() }
    });
  }

  getCashFlowStatement(periodId: string, startDate: Date, endDate: Date): Observable<FinancialStatement> {
    return this.http.get<FinancialStatement>(`${this.baseUrl}/financial-statements/cash-flow`, {
      params: { periodId, startDate: startDate.toISOString(), endDate: endDate.toISOString() }
    });
  }

  exportFinancialStatement(type: string, periodId: string, format: 'pdf' | 'excel'): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/financial-statements/${type}/export`, {
      params: { periodId, format },
      responseType: 'blob'
    });
  }

  // ============================================================================
  // RETAINED EARNINGS
  // ============================================================================

  getRetainedEarnings(request: AccountingSearchRequest): Observable<PaginatedResult<RetainedEarnings>> {
    return this.http.post<PaginatedResult<RetainedEarnings>>(`${this.baseUrl}/retained-earnings/search`, this.buildSearchRequest(request));
  }

  getRetainedEarningsById(id: string): Observable<RetainedEarnings> {
    return this.http.get<RetainedEarnings>(`${this.baseUrl}/retained-earnings/${id}`);
  }

  calculateRetainedEarnings(fiscalYear: number, periodId: string): Observable<RetainedEarnings> {
    return this.http.post<RetainedEarnings>(`${this.baseUrl}/retained-earnings/calculate`, { fiscalYear, periodId });
  }

  recordDistribution(id: string, distribution: any): Observable<RetainedEarnings> {
    return this.http.post<RetainedEarnings>(`${this.baseUrl}/retained-earnings/${id}/distribution`, distribution);
  }

  // ============================================================================
  // VENDORS
  // ============================================================================

  getVendors(request: AccountingSearchRequest): Observable<PaginatedResult<Vendor>> {
    return this.http.post<PaginatedResult<Vendor>>(`${this.baseUrl}/vendors/search`, this.buildSearchRequest(request));
  }

  getVendorById(id: string): Observable<Vendor> {
    return this.http.get<Vendor>(`${this.baseUrl}/vendors/${id}`);
  }

  createVendor(request: CreateVendorRequest): Observable<Vendor> {
    return this.http.post<Vendor>(`${this.baseUrl}/vendors`, request);
  }

  updateVendor(request: UpdateVendorRequest): Observable<Vendor> {
    return this.http.put<Vendor>(`${this.baseUrl}/vendors/${request.id}`, request);
  }

  deleteVendor(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/vendors/${id}`);
  }

  // ============================================================================
  // BILLS
  // ============================================================================

  getBills(request: AccountingSearchRequest): Observable<PaginatedResult<Bill>> {
    return this.http.post<PaginatedResult<Bill>>(`${this.baseUrl}/bills/search`, this.buildSearchRequest(request));
  }

  getBillById(id: string): Observable<Bill> {
    return this.http.get<Bill>(`${this.baseUrl}/bills/${id}`);
  }

  createBill(request: CreateBillRequest): Observable<Bill> {
    return this.http.post<Bill>(`${this.baseUrl}/bills`, request);
  }

  updateBill(id: string, request: CreateBillRequest): Observable<Bill> {
    return this.http.put<Bill>(`${this.baseUrl}/bills/${id}`, request);
  }

  deleteBill(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/bills/${id}`);
  }

  submitBillForApproval(id: string): Observable<Bill> {
    return this.http.post<Bill>(`${this.baseUrl}/bills/${id}/submit`, {});
  }

  approveBill(id: string): Observable<Bill> {
    return this.http.post<Bill>(`${this.baseUrl}/bills/${id}/approve`, {});
  }

  rejectBill(id: string, reason: string): Observable<Bill> {
    return this.http.post<Bill>(`${this.baseUrl}/bills/${id}/reject`, { reason });
  }

  postBill(id: string): Observable<Bill> {
    return this.http.post<Bill>(`${this.baseUrl}/bills/${id}/post`, {});
  }

  // ============================================================================
  // AP ACCOUNTS
  // ============================================================================

  getApAccounts(request: AccountingSearchRequest): Observable<PaginatedResult<ApAccount>> {
    return this.http.post<PaginatedResult<ApAccount>>(`${this.baseUrl}/accounts-payable/search`, this.buildSearchRequest(request));
  }

  getApAccountById(id: string): Observable<ApAccount> {
    return this.http.get<ApAccount>(`${this.baseUrl}/accounts-payable/${id}`);
  }

  // ============================================================================
  // CUSTOMERS
  // ============================================================================

  getCustomers(request: AccountingSearchRequest): Observable<PaginatedResult<Customer>> {
    return this.http.post<PaginatedResult<Customer>>(`${this.baseUrl}/customers/search`, this.buildSearchRequest(request));
  }

  getCustomerById(id: string): Observable<Customer> {
    return this.http.get<Customer>(`${this.baseUrl}/customers/${id}`);
  }

  createCustomer(request: CreateCustomerRequest): Observable<Customer> {
    return this.http.post<Customer>(`${this.baseUrl}/customers`, request);
  }

  updateCustomer(request: UpdateCustomerRequest): Observable<Customer> {
    return this.http.put<Customer>(`${this.baseUrl}/customers/${request.id}`, request);
  }

  deleteCustomer(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/customers/${id}`);
  }

  // ============================================================================
  // INVOICES
  // ============================================================================

  getInvoices(request: AccountingSearchRequest): Observable<PaginatedResult<Invoice>> {
    return this.http.post<PaginatedResult<Invoice>>(`${this.baseUrl}/invoices/search`, this.buildSearchRequest(request));
  }

  getInvoiceById(id: string): Observable<Invoice> {
    return this.http.get<Invoice>(`${this.baseUrl}/invoices/${id}`);
  }

  createInvoice(request: CreateInvoiceRequest): Observable<Invoice> {
    return this.http.post<Invoice>(`${this.baseUrl}/invoices`, request);
  }

  updateInvoice(id: string, request: CreateInvoiceRequest): Observable<Invoice> {
    return this.http.put<Invoice>(`${this.baseUrl}/invoices/${id}`, request);
  }

  deleteInvoice(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/invoices/${id}`);
  }

  submitInvoiceForApproval(id: string): Observable<Invoice> {
    return this.http.post<Invoice>(`${this.baseUrl}/invoices/${id}/submit`, {});
  }

  approveInvoice(id: string): Observable<Invoice> {
    return this.http.post<Invoice>(`${this.baseUrl}/invoices/${id}/approve`, {});
  }

  rejectInvoice(id: string, reason: string): Observable<Invoice> {
    return this.http.post<Invoice>(`${this.baseUrl}/invoices/${id}/reject`, { reason });
  }

  postInvoice(id: string): Observable<Invoice> {
    return this.http.post<Invoice>(`${this.baseUrl}/invoices/${id}/post`, {});
  }

  // ============================================================================
  // AR ACCOUNTS
  // ============================================================================

  getArAccounts(request: AccountingSearchRequest): Observable<PaginatedResult<ArAccount>> {
    return this.http.post<PaginatedResult<ArAccount>>(`${this.baseUrl}/accounts-receivable/search`, this.buildSearchRequest(request));
  }

  getArAccountById(id: string): Observable<ArAccount> {
    return this.http.get<ArAccount>(`${this.baseUrl}/accounts-receivable/${id}`);
  }

  // ============================================================================
  // PAYMENTS
  // ============================================================================

  getPayments(request: AccountingSearchRequest): Observable<PaginatedResult<Payment>> {
    return this.http.post<PaginatedResult<Payment>>(`${this.baseUrl}/payments/search`, this.buildSearchRequest(request));
  }

  getPaymentById(id: string): Observable<Payment> {
    return this.http.get<Payment>(`${this.baseUrl}/payments/${id}`);
  }

  createPayment(request: CreatePaymentRequest): Observable<Payment> {
    return this.http.post<Payment>(`${this.baseUrl}/payments`, request);
  }

  updatePayment(id: string, request: CreatePaymentRequest): Observable<Payment> {
    return this.http.put<Payment>(`${this.baseUrl}/payments/${id}`, request);
  }

  deletePayment(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/payments/${id}`);
  }

  allocatePayment(id: string, allocations: any[]): Observable<Payment> {
    return this.http.post<Payment>(`${this.baseUrl}/payments/${id}/allocate`, { allocations });
  }

  postPayment(id: string): Observable<Payment> {
    return this.http.post<Payment>(`${this.baseUrl}/payments/${id}/post`, {});
  }

  voidPayment(id: string, reason: string): Observable<Payment> {
    return this.http.post<Payment>(`${this.baseUrl}/payments/${id}/void`, { reason });
  }

  // ============================================================================
  // CREDIT MEMOS
  // ============================================================================

  getCreditMemos(request: AccountingSearchRequest): Observable<PaginatedResult<CreditMemo>> {
    return this.http.post<PaginatedResult<CreditMemo>>(`${this.baseUrl}/credit-memos/search`, this.buildSearchRequest(request));
  }

  getCreditMemoById(id: string): Observable<CreditMemo> {
    return this.http.get<CreditMemo>(`${this.baseUrl}/credit-memos/${id}`);
  }

  createCreditMemo(request: any): Observable<CreditMemo> {
    return this.http.post<CreditMemo>(`${this.baseUrl}/credit-memos`, request);
  }

  // ============================================================================
  // DEBIT MEMOS
  // ============================================================================

  getDebitMemos(request: AccountingSearchRequest): Observable<PaginatedResult<DebitMemo>> {
    return this.http.post<PaginatedResult<DebitMemo>>(`${this.baseUrl}/debit-memos/search`, this.buildSearchRequest(request));
  }

  getDebitMemoById(id: string): Observable<DebitMemo> {
    return this.http.get<DebitMemo>(`${this.baseUrl}/debit-memos/${id}`);
  }

  createDebitMemo(request: any): Observable<DebitMemo> {
    return this.http.post<DebitMemo>(`${this.baseUrl}/debit-memos`, request);
  }

  // ============================================================================
  // WRITE-OFFS
  // ============================================================================

  getWriteOffs(request: AccountingSearchRequest): Observable<PaginatedResult<WriteOff>> {
    return this.http.post<PaginatedResult<WriteOff>>(`${this.baseUrl}/write-offs/search`, this.buildSearchRequest(request));
  }

  getWriteOffById(id: string): Observable<WriteOff> {
    return this.http.get<WriteOff>(`${this.baseUrl}/write-offs/${id}`);
  }

  createWriteOff(request: any): Observable<WriteOff> {
    return this.http.post<WriteOff>(`${this.baseUrl}/write-offs`, request);
  }

  postWriteOff(id: string): Observable<WriteOff> {
    return this.http.post<WriteOff>(`${this.baseUrl}/write-offs/${id}/post`, {});
  }

  recordRecovery(id: string, amount: number): Observable<WriteOff> {
    return this.http.post<WriteOff>(`${this.baseUrl}/write-offs/${id}/recovery`, { amount });
  }

  // ============================================================================
  // BANKS
  // ============================================================================

  getBanks(request: AccountingSearchRequest): Observable<PaginatedResult<Bank>> {
    return this.http.post<PaginatedResult<Bank>>(`${this.baseUrl}/banks/search`, this.buildSearchRequest(request));
  }

  getBankById(id: string): Observable<Bank> {
    return this.http.get<Bank>(`${this.baseUrl}/banks/${id}`);
  }

  createBank(request: CreateBankRequest): Observable<Bank> {
    return this.http.post<Bank>(`${this.baseUrl}/banks`, request);
  }

  updateBank(id: string, request: CreateBankRequest): Observable<Bank> {
    return this.http.put<Bank>(`${this.baseUrl}/banks/${id}`, request);
  }

  deleteBank(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/banks/${id}`);
  }

  uploadBankImage(id: string, file: File): Observable<Bank> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<Bank>(`${this.baseUrl}/banks/${id}/image`, formData);
  }

  // ============================================================================
  // BANK RECONCILIATIONS
  // ============================================================================

  getBankReconciliations(request: AccountingSearchRequest): Observable<PaginatedResult<BankReconciliation>> {
    return this.http.post<PaginatedResult<BankReconciliation>>(`${this.baseUrl}/bank-reconciliations/search`, this.buildSearchRequest(request));
  }

  getBankReconciliationById(id: string): Observable<BankReconciliation> {
    return this.http.get<BankReconciliation>(`${this.baseUrl}/bank-reconciliations/${id}`);
  }

  createBankReconciliation(request: CreateBankReconciliationRequest): Observable<BankReconciliation> {
    return this.http.post<BankReconciliation>(`${this.baseUrl}/bank-reconciliations`, request);
  }

  clearItem(reconciliationId: string, itemId: string): Observable<BankReconciliation> {
    return this.http.post<BankReconciliation>(`${this.baseUrl}/bank-reconciliations/${reconciliationId}/items/${itemId}/clear`, {});
  }

  unclearItem(reconciliationId: string, itemId: string): Observable<BankReconciliation> {
    return this.http.post<BankReconciliation>(`${this.baseUrl}/bank-reconciliations/${reconciliationId}/items/${itemId}/unclear`, {});
  }

  addAdjustment(reconciliationId: string, adjustment: any): Observable<BankReconciliation> {
    return this.http.post<BankReconciliation>(`${this.baseUrl}/bank-reconciliations/${reconciliationId}/adjustments`, adjustment);
  }

  completeReconciliation(id: string): Observable<BankReconciliation> {
    return this.http.post<BankReconciliation>(`${this.baseUrl}/bank-reconciliations/${id}/complete`, {});
  }

  approveReconciliation(id: string): Observable<BankReconciliation> {
    return this.http.post<BankReconciliation>(`${this.baseUrl}/bank-reconciliations/${id}/approve`, {});
  }

  voidBankReconciliation(id: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/bank-reconciliations/${id}/void`, {});
  }

  // ============================================================================
  // CHECKS
  // ============================================================================

  getChecks(request: AccountingSearchRequest): Observable<PaginatedResult<Check>> {
    return this.http.post<PaginatedResult<Check>>(`${this.baseUrl}/checks/search`, this.buildSearchRequest(request));
  }

  getCheckById(id: string): Observable<Check> {
    return this.http.get<Check>(`${this.baseUrl}/checks/${id}`);
  }

  createCheck(request: CreateCheckRequest): Observable<Check> {
    return this.http.post<Check>(`${this.baseUrl}/checks`, request);
  }

  voidCheck(id: string, reason: string): Observable<Check> {
    return this.http.post<Check>(`${this.baseUrl}/checks/${id}/void`, { reason });
  }

  clearCheck(id: string, clearedDate: Date): Observable<Check> {
    return this.http.post<Check>(`${this.baseUrl}/checks/${id}/clear`, { clearedDate });
  }

  stopPayment(id: string, reason: string): Observable<Check> {
    return this.http.post<Check>(`${this.baseUrl}/checks/${id}/stop-payment`, { reason });
  }

  // ============================================================================
  // ACCOUNT RECONCILIATIONS
  // ============================================================================

  getAccountReconciliations(request: AccountingSearchRequest): Observable<PaginatedResult<AccountReconciliation>> {
    return this.http.post<PaginatedResult<AccountReconciliation>>(`${this.baseUrl}/account-reconciliations/search`, this.buildSearchRequest(request));
  }

  getAccountReconciliationById(id: string): Observable<AccountReconciliation> {
    return this.http.get<AccountReconciliation>(`${this.baseUrl}/account-reconciliations/${id}`);
  }

  createAccountReconciliation(request: any): Observable<AccountReconciliation> {
    return this.http.post<AccountReconciliation>(`${this.baseUrl}/account-reconciliations`, request);
  }

  approveAccountReconciliation(id: string): Observable<AccountReconciliation> {
    return this.http.post<AccountReconciliation>(`${this.baseUrl}/account-reconciliations/${id}/approve`, {});
  }

  // ============================================================================
  // BUDGETS
  // ============================================================================

  getBudgets(request: AccountingSearchRequest): Observable<PaginatedResult<Budget>> {
    return this.http.post<PaginatedResult<Budget>>(`${this.baseUrl}/budgets/search`, this.buildSearchRequest(request));
  }

  getBudgetById(id: string): Observable<Budget> {
    return this.http.get<Budget>(`${this.baseUrl}/budgets/${id}`);
  }

  createBudget(request: CreateBudgetRequest): Observable<Budget> {
    return this.http.post<Budget>(`${this.baseUrl}/budgets`, request);
  }

  updateBudget(id: string, request: CreateBudgetRequest): Observable<Budget> {
    return this.http.put<Budget>(`${this.baseUrl}/budgets/${id}`, request);
  }

  deleteBudget(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/budgets/${id}`);
  }

  submitBudgetForApproval(id: string): Observable<Budget> {
    return this.http.post<Budget>(`${this.baseUrl}/budgets/${id}/submit`, {});
  }

  approveBudget(id: string): Observable<Budget> {
    return this.http.post<Budget>(`${this.baseUrl}/budgets/${id}/approve`, {});
  }

  rejectBudget(id: string, reason: string): Observable<Budget> {
    return this.http.post<Budget>(`${this.baseUrl}/budgets/${id}/reject`, { reason });
  }

  activateBudget(id: string): Observable<Budget> {
    return this.http.post<Budget>(`${this.baseUrl}/budgets/${id}/activate`, {});
  }

  closeBudget(id: string): Observable<Budget> {
    return this.http.post<Budget>(`${this.baseUrl}/budgets/${id}/close`, {});
  }

  copyBudget(id: string, newFiscalYear: number): Observable<Budget> {
    return this.http.post<Budget>(`${this.baseUrl}/budgets/${id}/copy`, { newFiscalYear });
  }

  // ============================================================================
  // PROJECTS
  // ============================================================================

  getProjects(request: AccountingSearchRequest): Observable<PaginatedResult<Project>> {
    return this.http.post<PaginatedResult<Project>>(`${this.baseUrl}/projects/search`, this.buildSearchRequest(request));
  }

  getProjectById(id: string): Observable<Project> {
    return this.http.get<Project>(`${this.baseUrl}/projects/${id}`);
  }

  createProject(request: CreateProjectRequest): Observable<Project> {
    return this.http.post<Project>(`${this.baseUrl}/projects`, request);
  }

  updateProject(id: string, request: CreateProjectRequest): Observable<Project> {
    return this.http.put<Project>(`${this.baseUrl}/projects/${id}`, request);
  }

  deleteProject(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/projects/${id}`);
  }

  updateProjectStatus(id: string, status: string): Observable<Project> {
    return this.http.post<Project>(`${this.baseUrl}/projects/${id}/status`, { status });
  }

  // ============================================================================
  // FIXED ASSETS
  // ============================================================================

  getFixedAssets(request: AccountingSearchRequest): Observable<PaginatedResult<FixedAsset>> {
    return this.http.post<PaginatedResult<FixedAsset>>(`${this.baseUrl}/fixed-assets/search`, this.buildSearchRequest(request));
  }

  getFixedAssetById(id: string): Observable<FixedAsset> {
    return this.http.get<FixedAsset>(`${this.baseUrl}/fixed-assets/${id}`);
  }

  createFixedAsset(request: CreateFixedAssetRequest): Observable<FixedAsset> {
    return this.http.post<FixedAsset>(`${this.baseUrl}/fixed-assets`, request);
  }

  updateFixedAsset(id: string, request: CreateFixedAssetRequest): Observable<FixedAsset> {
    return this.http.put<FixedAsset>(`${this.baseUrl}/fixed-assets/${id}`, request);
  }

  deleteFixedAsset(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/fixed-assets/${id}`);
  }

  depreciateAssets(request: DepreciateFixedAssetRequest): Observable<FixedAsset[]> {
    return this.http.post<FixedAsset[]>(`${this.baseUrl}/fixed-assets/depreciate`, request);
  }

  disposeAsset(request: DisposeFixedAssetRequest): Observable<FixedAsset> {
    return this.http.post<FixedAsset>(`${this.baseUrl}/fixed-assets/${request.id}/dispose`, request);
  }

  transferAsset(id: string, newLocation: string, newDepartment?: string): Observable<FixedAsset> {
    return this.http.post<FixedAsset>(`${this.baseUrl}/fixed-assets/${id}/transfer`, { newLocation, newDepartment });
  }

  // ============================================================================
  // DEPRECIATION METHODS
  // ============================================================================

  getDepreciationMethods(): Observable<DepreciationMethodConfig[]> {
    return this.http.get<DepreciationMethodConfig[]>(`${this.baseUrl}/depreciation-methods`);
  }

  getDepreciationMethodById(id: string): Observable<DepreciationMethodConfig> {
    return this.http.get<DepreciationMethodConfig>(`${this.baseUrl}/depreciation-methods/${id}`);
  }

  createDepreciationMethod(request: any): Observable<DepreciationMethodConfig> {
    return this.http.post<DepreciationMethodConfig>(`${this.baseUrl}/depreciation-methods`, request);
  }

  updateDepreciationMethod(id: string, request: any): Observable<DepreciationMethodConfig> {
    return this.http.put<DepreciationMethodConfig>(`${this.baseUrl}/depreciation-methods/${id}`, request);
  }

  // ============================================================================
  // INVENTORY
  // ============================================================================

  getInventoryItems(request: AccountingSearchRequest): Observable<PaginatedResult<InventoryItem>> {
    return this.http.post<PaginatedResult<InventoryItem>>(`${this.baseUrl}/inventory-items/search`, this.buildSearchRequest(request));
  }

  getInventoryItemById(id: string): Observable<InventoryItem> {
    return this.http.get<InventoryItem>(`${this.baseUrl}/inventory-items/${id}`);
  }

  createInventoryItem(request: CreateInventoryItemRequest): Observable<InventoryItem> {
    return this.http.post<InventoryItem>(`${this.baseUrl}/inventory-items`, request);
  }

  updateInventoryItem(id: string, request: CreateInventoryItemRequest): Observable<InventoryItem> {
    return this.http.put<InventoryItem>(`${this.baseUrl}/inventory-items/${id}`, request);
  }

  deleteInventoryItem(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/inventory-items/${id}`);
  }

  adjustInventory(request: AdjustInventoryRequest): Observable<InventoryItem> {
    return this.http.post<InventoryItem>(`${this.baseUrl}/inventory-items/${request.itemId}/adjust`, request);
  }

  // ============================================================================
  // ACCRUALS
  // ============================================================================

  getAccruals(request: AccountingSearchRequest): Observable<PaginatedResult<Accrual>> {
    return this.http.post<PaginatedResult<Accrual>>(`${this.baseUrl}/accruals/search`, this.buildSearchRequest(request));
  }

  getAccrualById(id: string): Observable<Accrual> {
    return this.http.get<Accrual>(`${this.baseUrl}/accruals/${id}`);
  }

  createAccrual(request: CreateAccrualRequest): Observable<Accrual> {
    return this.http.post<Accrual>(`${this.baseUrl}/accruals`, request);
  }

  reverseAccrual(id: string): Observable<Accrual> {
    return this.http.post<Accrual>(`${this.baseUrl}/accruals/${id}/reverse`, {});
  }

  // ============================================================================
  // PREPAID EXPENSES
  // ============================================================================

  getPrepaidExpenses(request: AccountingSearchRequest): Observable<PaginatedResult<PrepaidExpense>> {
    return this.http.post<PaginatedResult<PrepaidExpense>>(`${this.baseUrl}/prepaid-expenses/search`, this.buildSearchRequest(request));
  }

  getPrepaidExpenseById(id: string): Observable<PrepaidExpense> {
    return this.http.get<PrepaidExpense>(`${this.baseUrl}/prepaid-expenses/${id}`);
  }

  createPrepaidExpense(request: CreatePrepaidExpenseRequest): Observable<PrepaidExpense> {
    return this.http.post<PrepaidExpense>(`${this.baseUrl}/prepaid-expenses`, request);
  }

  amortizePrepaidExpense(id: string, throughDate: Date): Observable<PrepaidExpense> {
    return this.http.post<PrepaidExpense>(`${this.baseUrl}/prepaid-expenses/${id}/amortize`, { throughDate });
  }

  closePrepaidExpense(id: string): Observable<PrepaidExpense> {
    return this.http.post<PrepaidExpense>(`${this.baseUrl}/prepaid-expenses/${id}/close`, {});
  }

  // ============================================================================
  // DEFERRED REVENUE
  // ============================================================================

  getDeferredRevenues(request: AccountingSearchRequest): Observable<PaginatedResult<DeferredRevenue>> {
    return this.http.post<PaginatedResult<DeferredRevenue>>(`${this.baseUrl}/deferred-revenues/search`, this.buildSearchRequest(request));
  }

  getDeferredRevenueById(id: string): Observable<DeferredRevenue> {
    return this.http.get<DeferredRevenue>(`${this.baseUrl}/deferred-revenues/${id}`);
  }

  createDeferredRevenue(request: CreateDeferredRevenueRequest): Observable<DeferredRevenue> {
    return this.http.post<DeferredRevenue>(`${this.baseUrl}/deferred-revenues`, request);
  }

  recognizeRevenue(id: string, throughDate: Date): Observable<DeferredRevenue> {
    return this.http.post<DeferredRevenue>(`${this.baseUrl}/deferred-revenues/${id}/recognize`, { throughDate });
  }

  closeDeferredRevenue(id: string): Observable<DeferredRevenue> {
    return this.http.post<DeferredRevenue>(`${this.baseUrl}/deferred-revenues/${id}/close`, {});
  }

  // ============================================================================
  // RECURRING JOURNAL ENTRIES
  // ============================================================================

  getRecurringEntries(request: AccountingSearchRequest): Observable<PaginatedResult<RecurringJournalEntry>> {
    return this.http.post<PaginatedResult<RecurringJournalEntry>>(`${this.baseUrl}/recurring-journal-entries/search`, this.buildSearchRequest(request));
  }

  getRecurringEntryById(id: string): Observable<RecurringJournalEntry> {
    return this.http.get<RecurringJournalEntry>(`${this.baseUrl}/recurring-journal-entries/${id}`);
  }

  createRecurringEntry(request: CreateRecurringJournalEntryRequest): Observable<RecurringJournalEntry> {
    return this.http.post<RecurringJournalEntry>(`${this.baseUrl}/recurring-journal-entries`, request);
  }

  updateRecurringEntry(id: string, request: CreateRecurringJournalEntryRequest): Observable<RecurringJournalEntry> {
    return this.http.put<RecurringJournalEntry>(`${this.baseUrl}/recurring-journal-entries/${id}`, request);
  }

  deleteRecurringEntry(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/recurring-journal-entries/${id}`);
  }

  runRecurringEntry(id: string): Observable<JournalEntry> {
    return this.http.post<JournalEntry>(`${this.baseUrl}/recurring-journal-entries/${id}/run`, {});
  }

  pauseRecurringEntry(id: string): Observable<RecurringJournalEntry> {
    return this.http.post<RecurringJournalEntry>(`${this.baseUrl}/recurring-journal-entries/${id}/pause`, {});
  }

  resumeRecurringEntry(id: string): Observable<RecurringJournalEntry> {
    return this.http.post<RecurringJournalEntry>(`${this.baseUrl}/recurring-journal-entries/${id}/resume`, {});
  }

  // ============================================================================
  // POSTING BATCHES
  // ============================================================================

  getPostingBatches(request: AccountingSearchRequest): Observable<PaginatedResult<PostingBatch>> {
    return this.http.post<PaginatedResult<PostingBatch>>(`${this.baseUrl}/posting-batch/search`, this.buildSearchRequest(request));
  }

  getPostingBatchById(id: string): Observable<PostingBatch> {
    return this.http.get<PostingBatch>(`${this.baseUrl}/posting-batch/${id}`);
  }

  createPostingBatch(request: any): Observable<PostingBatch> {
    return this.http.post<PostingBatch>(`${this.baseUrl}/posting-batch`, request);
  }

  postBatch(id: string): Observable<PostingBatch> {
    return this.http.post<PostingBatch>(`${this.baseUrl}/posting-batch/${id}/post`, {});
  }

  // ============================================================================
  // TAX CODES
  // ============================================================================

  getTaxCodes(request: AccountingSearchRequest): Observable<PaginatedResult<TaxCode>> {
    return this.http.post<PaginatedResult<TaxCode>>(`${this.baseUrl}/tax-codes/search`, this.buildSearchRequest(request));
  }

  getTaxCodeById(id: string): Observable<TaxCode> {
    return this.http.get<TaxCode>(`${this.baseUrl}/tax-codes/${id}`);
  }

  createTaxCode(request: CreateTaxCodeRequest): Observable<TaxCode> {
    return this.http.post<TaxCode>(`${this.baseUrl}/tax-codes`, request);
  }

  updateTaxCode(id: string, request: CreateTaxCodeRequest): Observable<TaxCode> {
    return this.http.put<TaxCode>(`${this.baseUrl}/tax-codes/${id}`, request);
  }

  deleteTaxCode(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/tax-codes/${id}`);
  }

  // ============================================================================
  // PAYEES
  // ============================================================================

  getPayees(request: AccountingSearchRequest): Observable<PaginatedResult<Payee>> {
    return this.http.post<PaginatedResult<Payee>>(`${this.baseUrl}/payees/search`, this.buildSearchRequest(request));
  }

  getPayeeById(id: string): Observable<Payee> {
    return this.http.get<Payee>(`${this.baseUrl}/payees/${id}`);
  }

  createPayee(request: any): Observable<Payee> {
    return this.http.post<Payee>(`${this.baseUrl}/payees`, request);
  }

  updatePayee(id: string, request: any): Observable<Payee> {
    return this.http.put<Payee>(`${this.baseUrl}/payees/${id}`, request);
  }

  deletePayee(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/payees/${id}`);
  }

  // ============================================================================
  // COST CENTERS & DEPARTMENTS
  // ============================================================================

  getCostCenters(): Observable<CostCenter[]> {
    return this.http.post<any>(`${this.baseUrl}/cost-centers/search`, { pageNumber: 1, pageSize: 1000 }).pipe(
      map((result: any) => result.items || result)
    );
  }

  getDepartments(): Observable<Department[]> {
    return this.http.get<Department[]>(`${this.baseUrl}/departments`);
  }

  // ============================================================================
  // DASHBOARD / SUMMARY
  // ============================================================================

  getAccountingSummary(periodId?: string): Observable<AccountingSummary> {
    let params = new HttpParams();
    if (periodId) {
      params = params.set('periodId', periodId);
    }
    return this.http.get<AccountingSummary>(`${this.baseUrl}/summary`, { params });
  }

  getArAgingSummary(): Observable<AgingSummary> {
    return this.http.get<AgingSummary>(`${this.baseUrl}/ar-aging`);
  }

  getApAgingSummary(): Observable<AgingSummary> {
    return this.http.get<AgingSummary>(`${this.baseUrl}/ap-aging`);
  }

  getCashFlowSummary(periodId: string): Observable<CashFlowSummary> {
    return this.http.get<CashFlowSummary>(`${this.baseUrl}/cash-flow-summary/${periodId}`);
  }

  // ============================================================================
  // HELPER METHODS
  // ============================================================================

  private buildSearchRequest(request: AccountingSearchRequest): any {
    const searchRequest: any = {
      pageNumber: request.pageNumber,
      pageSize: request.pageSize
    };

    if (request.searchTerm) {
      searchRequest.keyword = request.searchTerm;
    }
    if (request.sortBy) {
      searchRequest.orderBy = [request.sortDescending ? `${request.sortBy} desc` : request.sortBy];
    }
    if (request.periodId) {
      searchRequest.periodId = request.periodId;
    }
    if (request.startDate) {
      searchRequest.startDate = request.startDate.toISOString();
    }
    if (request.endDate) {
      searchRequest.endDate = request.endDate.toISOString();
    }
    if (request.status) {
      searchRequest.status = request.status;
    }
    if (request.accountId) {
      searchRequest.accountId = request.accountId;
    }
    if (request.vendorId) {
      searchRequest.vendorId = request.vendorId;
    }
    if (request.customerId) {
      searchRequest.customerId = request.customerId;
    }

    return searchRequest;
  }

  private buildParams(request: AccountingSearchRequest): HttpParams {
    let params = new HttpParams()
      .set('pageNumber', request.pageNumber.toString())
      .set('pageSize', request.pageSize.toString());

    if (request.searchTerm) {
      params = params.set('searchTerm', request.searchTerm);
    }
    if (request.sortBy) {
      params = params.set('sortBy', request.sortBy);
      params = params.set('sortDescending', (request.sortDescending ?? false).toString());
    }
    if (request.periodId) {
      params = params.set('periodId', request.periodId);
    }
    if (request.startDate) {
      params = params.set('startDate', request.startDate.toISOString());
    }
    if (request.endDate) {
      params = params.set('endDate', request.endDate.toISOString());
    }
    if (request.status) {
      params = params.set('status', request.status);
    }
    if (request.accountId) {
      params = params.set('accountId', request.accountId);
    }
    if (request.vendorId) {
      params = params.set('vendorId', request.vendorId);
    }
    if (request.customerId) {
      params = params.set('customerId', request.customerId);
    }

    return params;
  }
}

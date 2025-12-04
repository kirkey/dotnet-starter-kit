import { Component, OnInit, inject, signal, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSortModule, MatSort, Sort } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatTabsModule } from '@angular/material/tabs';
import { MatChipsModule } from '@angular/material/chips';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatDividerModule } from '@angular/material/divider';
import { MatTooltipModule } from '@angular/material/tooltip';

import { AccountingService } from '@core/services/accounting.service';
import {
  GeneralLedgerEntry,
  GeneralLedgerSummary,
  ChartOfAccount,
  AccountingPeriod,
  AccountingSearchRequest
} from '@core/models/accounting.model';

@Component({
  selector: 'app-general-ledger',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatExpansionModule,
    MatTabsModule,
    MatChipsModule,
    MatAutocompleteModule,
    MatDividerModule,
    MatTooltipModule
  ],
  template: `
    <div class="page-container">
      <mat-card>
        <mat-card-header>
          <mat-card-title>
            <div class="header-row">
              <span>General Ledger</span>
              <div class="header-actions">
                <button mat-stroked-button (click)="exportToExcel()" [disabled]="loading()">
                  <mat-icon>download</mat-icon>
                  Export
                </button>
                <button mat-stroked-button (click)="print()" [disabled]="loading()">
                  <mat-icon>print</mat-icon>
                  Print
                </button>
              </div>
            </div>
          </mat-card-title>
        </mat-card-header>

        <mat-card-content>
          <!-- Filters -->
          <mat-expansion-panel [expanded]="true" class="filters-panel">
            <mat-expansion-panel-header>
              <mat-panel-title>
                <mat-icon>filter_list</mat-icon>
                Filters
              </mat-panel-title>
            </mat-expansion-panel-header>

            <div class="filters-content">
              <div class="filters-row">
                <mat-form-field appearance="outline">
                  <mat-label>Account</mat-label>
                  <input matInput [(ngModel)]="selectedAccountId"
                         [matAutocomplete]="accountAuto"
                         placeholder="Select account...">
                  <mat-autocomplete #accountAuto="matAutocomplete" [displayWith]="displayAccount"
                                    (optionSelected)="onSearch()">
                    @for (account of accounts(); track account.id) {
                      <mat-option [value]="account.id">
                        {{ account.accountCode }} - {{ account.accountName }}
                      </mat-option>
                    }
                  </mat-autocomplete>
                  <button matSuffix mat-icon-button *ngIf="selectedAccountId" (click)="clearAccount()">
                    <mat-icon>close</mat-icon>
                  </button>
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Period</mat-label>
                  <mat-select [(ngModel)]="selectedPeriodId" (ngModelChange)="onSearch()">
                    <mat-option value="">All Periods</mat-option>
                    @for (period of periods(); track period.id) {
                      <mat-option [value]="period.id">{{ period.periodName }}</mat-option>
                    }
                  </mat-select>
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Start Date</mat-label>
                  <input matInput [matDatepicker]="startPicker"
                         [(ngModel)]="startDate" (ngModelChange)="onSearch()">
                  <mat-datepicker-toggle matIconSuffix [for]="startPicker"></mat-datepicker-toggle>
                  <mat-datepicker #startPicker></mat-datepicker>
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>End Date</mat-label>
                  <input matInput [matDatepicker]="endPicker"
                         [(ngModel)]="endDate" (ngModelChange)="onSearch()">
                  <mat-datepicker-toggle matIconSuffix [for]="endPicker"></mat-datepicker-toggle>
                  <mat-datepicker #endPicker></mat-datepicker>
                </mat-form-field>
              </div>

              <div class="filters-row">
                <mat-form-field appearance="outline" class="search-field">
                  <mat-label>Search</mat-label>
                  <input matInput [(ngModel)]="searchTerm" (ngModelChange)="onSearch()"
                         placeholder="Search by reference, description...">
                  <mat-icon matSuffix>search</mat-icon>
                </mat-form-field>

                <button mat-stroked-button (click)="clearFilters()">
                  <mat-icon>clear_all</mat-icon>
                  Clear Filters
                </button>
              </div>
            </div>
          </mat-expansion-panel>

          <!-- Tab Views -->
          <mat-tab-group [(selectedIndex)]="selectedTabIndex" (selectedTabChange)="onTabChange()">
            <!-- Detail View -->
            <mat-tab label="Detailed Transactions">
              @if (loading()) {
                <div class="loading-container">
                  <mat-spinner diameter="40"></mat-spinner>
                </div>
              }

              @if (!loading()) {
                <div class="table-container">
                  <table mat-table [dataSource]="dataSource" matSort (matSortChange)="onSort($event)">
                    <ng-container matColumnDef="transactionDate">
                      <th mat-header-cell *matHeaderCellDef mat-sort-header>Date</th>
                      <td mat-cell *matCellDef="let entry">
                        {{ entry.transactionDate | date:'shortDate' }}
                      </td>
                    </ng-container>

                    <ng-container matColumnDef="accountCode">
                      <th mat-header-cell *matHeaderCellDef mat-sort-header>Account</th>
                      <td mat-cell *matCellDef="let entry">
                        <div class="account-cell">
                          <span class="account-code">{{ entry.accountCode }}</span>
                          <span class="account-name">{{ entry.accountName }}</span>
                        </div>
                      </td>
                    </ng-container>

                    <ng-container matColumnDef="journalEntryNumber">
                      <th mat-header-cell *matHeaderCellDef mat-sort-header>JE #</th>
                      <td mat-cell *matCellDef="let entry">
                        <a class="link" (click)="viewJournalEntry(entry)">
                          {{ entry.journalEntryNumber }}
                        </a>
                      </td>
                    </ng-container>

                    <ng-container matColumnDef="description">
                      <th mat-header-cell *matHeaderCellDef mat-sort-header>Description</th>
                      <td mat-cell *matCellDef="let entry">{{ entry.description }}</td>
                    </ng-container>

                    <ng-container matColumnDef="debitAmount">
                      <th mat-header-cell *matHeaderCellDef class="text-right">Debit</th>
                      <td mat-cell *matCellDef="let entry" class="text-right amount-debit">
                        {{ entry.debitAmount > 0 ? (entry.debitAmount | currency) : '' }}
                      </td>
                    </ng-container>

                    <ng-container matColumnDef="creditAmount">
                      <th mat-header-cell *matHeaderCellDef class="text-right">Credit</th>
                      <td mat-cell *matCellDef="let entry" class="text-right amount-credit">
                        {{ entry.creditAmount > 0 ? (entry.creditAmount | currency) : '' }}
                      </td>
                    </ng-container>

                    <ng-container matColumnDef="runningBalance">
                      <th mat-header-cell *matHeaderCellDef class="text-right">Balance</th>
                      <td mat-cell *matCellDef="let entry" class="text-right">
                        {{ entry.runningBalance | currency }}
                      </td>
                    </ng-container>

                    <ng-container matColumnDef="reference">
                      <th mat-header-cell *matHeaderCellDef>Reference</th>
                      <td mat-cell *matCellDef="let entry">{{ entry.reference || '-' }}</td>
                    </ng-container>

                    <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
                    <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
                  </table>

                  <!-- Summary Row -->
                  <div class="summary-row" *ngIf="entries().length > 0">
                    <div class="summary-item">
                      <span class="summary-label">Total Debits:</span>
                      <span class="summary-value amount-debit">{{ totalDebits() | currency }}</span>
                    </div>
                    <div class="summary-item">
                      <span class="summary-label">Total Credits:</span>
                      <span class="summary-value amount-credit">{{ totalCredits() | currency }}</span>
                    </div>
                    <div class="summary-item">
                      <span class="summary-label">Net Change:</span>
                      <span class="summary-value" [class.positive]="netChange() >= 0" [class.negative]="netChange() < 0">
                        {{ netChange() | currency }}
                      </span>
                    </div>
                  </div>

                  <mat-paginator
                    [length]="totalRecords()"
                    [pageSize]="pageSize"
                    [pageSizeOptions]="[25, 50, 100, 250]"
                    (page)="onPageChange($event)"
                    showFirstLastButtons>
                  </mat-paginator>
                </div>
              }

              @if (!loading() && entries().length === 0) {
                <div class="empty-state">
                  <mat-icon>menu_book</mat-icon>
                  <h3>No ledger entries found</h3>
                  <p>Select an account or adjust filters to view ledger entries</p>
                </div>
              }
            </mat-tab>

            <!-- Summary View -->
            <mat-tab label="Account Summary">
              @if (loadingSummary()) {
                <div class="loading-container">
                  <mat-spinner diameter="40"></mat-spinner>
                </div>
              }

              @if (!loadingSummary()) {
                <div class="summary-grid">
                  @for (summary of summaries(); track summary.accountId) {
                    <mat-card class="summary-card" (click)="selectAccount(summary.accountId)">
                      <mat-card-header>
                        <mat-card-title>{{ summary.accountCode }}</mat-card-title>
                        <mat-card-subtitle>{{ summary.accountName }}</mat-card-subtitle>
                      </mat-card-header>
                      <mat-card-content>
                        <div class="summary-details">
                          <div class="detail-row">
                            <span class="detail-label">Opening Balance:</span>
                            <span class="detail-value">{{ summary.beginningBalance | currency }}</span>
                          </div>
                          <div class="detail-row">
                            <span class="detail-label">Total Debits:</span>
                            <span class="detail-value amount-debit">{{ summary.totalDebits | currency }}</span>
                          </div>
                          <div class="detail-row">
                            <span class="detail-label">Total Credits:</span>
                            <span class="detail-value amount-credit">{{ summary.totalCredits | currency }}</span>
                          </div>
                          <mat-divider></mat-divider>
                          <div class="detail-row closing">
                            <span class="detail-label">Closing Balance:</span>
                            <span class="detail-value" [class.positive]="summary.endingBalance >= 0"
                                  [class.negative]="summary.endingBalance < 0">
                              {{ summary.endingBalance | currency }}
                            </span>
                          </div>
                        </div>
                      </mat-card-content>
                    </mat-card>
                  }
                </div>

                @if (summaries().length === 0) {
                  <div class="empty-state">
                    <mat-icon>summarize</mat-icon>
                    <h3>No summary data available</h3>
                    <p>Select a period to view account summaries</p>
                  </div>
                }
              }
            </mat-tab>
          </mat-tab-group>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .page-container {
      padding: 24px;
    }

    .header-row {
      display: flex;
      justify-content: space-between;
      align-items: center;
      width: 100%;
    }

    .header-actions {
      display: flex;
      gap: 8px;
    }

    .filters-panel {
      margin-bottom: 16px;
    }

    .filters-content {
      padding-top: 16px;
    }

    .filters-row {
      display: flex;
      gap: 16px;
      flex-wrap: wrap;
      margin-bottom: 8px;
    }

    .filters-row mat-form-field {
      flex: 1;
      min-width: 200px;
    }

    .search-field {
      flex: 2 !important;
    }

    .table-container {
      overflow-x: auto;
      margin-top: 16px;
    }

    table {
      width: 100%;
    }

    .account-cell {
      display: flex;
      flex-direction: column;
    }

    .account-code {
      font-family: monospace;
      font-weight: 500;
    }

    .account-name {
      font-size: 12px;
      color: #666;
    }

    .text-right {
      text-align: right;
    }

    .amount-debit {
      color: #1976d2;
    }

    .amount-credit {
      color: #388e3c;
    }

    .positive {
      color: #388e3c;
    }

    .negative {
      color: #c62828;
    }

    .link {
      color: #1976d2;
      cursor: pointer;
      text-decoration: underline;
    }

    .summary-row {
      display: flex;
      justify-content: flex-end;
      gap: 32px;
      padding: 16px;
      background-color: #f5f5f5;
      border-radius: 4px;
      margin-top: 16px;
    }

    .summary-item {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .summary-label {
      font-weight: 500;
      color: #666;
    }

    .summary-value {
      font-size: 16px;
      font-weight: 600;
    }

    .loading-container {
      display: flex;
      justify-content: center;
      padding: 48px;
    }

    .empty-state {
      text-align: center;
      padding: 48px;
      color: #666;
    }

    .empty-state mat-icon {
      font-size: 64px;
      width: 64px;
      height: 64px;
      color: #ccc;
      margin-bottom: 16px;
    }

    .empty-state h3 {
      margin: 0 0 8px;
    }

    .empty-state p {
      margin: 0;
    }

    /* Summary Grid */
    .summary-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
      gap: 16px;
      padding: 16px 0;
    }

    .summary-card {
      cursor: pointer;
      transition: box-shadow 0.2s;
    }

    .summary-card:hover {
      box-shadow: 0 4px 12px rgba(0,0,0,0.15);
    }

    .summary-details {
      padding-top: 8px;
    }

    .detail-row {
      display: flex;
      justify-content: space-between;
      padding: 4px 0;
    }

    .detail-row.closing {
      padding-top: 12px;
      font-weight: 600;
    }

    .detail-label {
      color: #666;
    }

    .detail-value {
      font-weight: 500;
    }

    mat-divider {
      margin: 8px 0;
    }
  `]
})
export class GeneralLedgerComponent implements OnInit {
  private accountingService = inject(AccountingService);
  private snackBar = inject(MatSnackBar);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  entries = signal<GeneralLedgerEntry[]>([]);
  summaries = signal<GeneralLedgerSummary[]>([]);
  accounts = signal<ChartOfAccount[]>([]);
  periods = signal<AccountingPeriod[]>([]);
  loading = signal(false);
  loadingSummary = signal(false);
  totalRecords = signal(0);

  totalDebits = signal(0);
  totalCredits = signal(0);
  netChange = signal(0);

  dataSource = new MatTableDataSource<GeneralLedgerEntry>();

  displayedColumns = ['transactionDate', 'accountCode', 'journalEntryNumber', 'description', 'debitAmount', 'creditAmount', 'runningBalance', 'reference'];

  selectedTabIndex = 0;
  selectedAccountId = '';
  selectedPeriodId = '';
  startDate: Date | null = null;
  endDate: Date | null = null;
  searchTerm = '';
  pageSize = 50;
  pageNumber = 1;
  sortBy = 'transactionDate';
  sortDescending = false;

  ngOnInit(): void {
    this.loadAccounts();
    this.loadPeriods();
    this.loadEntries();
  }

  loadAccounts(): void {
    this.accountingService.getChartOfAccounts({ pageNumber: 1, pageSize: 1000 }).subscribe({
      next: (result) => {
        this.accounts.set(result.items.filter(a => a.isActive));
      }
    });
  }

  loadPeriods(): void {
    this.accountingService.getAccountingPeriods({ pageNumber: 1, pageSize: 100 }).subscribe({
      next: (result) => {
        this.periods.set(result.items);
      }
    });
  }

  loadEntries(): void {
    this.loading.set(true);
    const request: AccountingSearchRequest = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      sortDescending: this.sortDescending,
      accountId: this.selectedAccountId || undefined,
      periodId: this.selectedPeriodId || undefined,
      startDate: this.startDate || undefined,
      endDate: this.endDate || undefined
    };

    this.accountingService.getGeneralLedgerEntries(request).subscribe({
      next: (result) => {
        this.entries.set(result.items);
        this.totalRecords.set(result.totalCount);
        this.dataSource.data = result.items;
        this.calculateTotals(result.items);
        this.loading.set(false);
      },
      error: (error) => {
        console.error('Error loading ledger entries:', error);
        this.snackBar.open('Failed to load ledger entries', 'Close', { duration: 3000 });
        this.loading.set(false);
      }
    });
  }

  loadSummary(): void {
    if (!this.selectedPeriodId) {
      this.summaries.set([]);
      return;
    }

    this.loadingSummary.set(true);
    this.accountingService.getGeneralLedgerSummary(this.selectedPeriodId).subscribe({
      next: (summaries) => {
        this.summaries.set(summaries);
        this.loadingSummary.set(false);
      },
      error: (error) => {
        console.error('Error loading summary:', error);
        this.snackBar.open('Failed to load summary', 'Close', { duration: 3000 });
        this.loadingSummary.set(false);
      }
    });
  }

  calculateTotals(entries: GeneralLedgerEntry[]): void {
    let debits = 0;
    let credits = 0;
    entries.forEach(entry => {
      debits += entry.debitAmount || 0;
      credits += entry.creditAmount || 0;
    });
    this.totalDebits.set(debits);
    this.totalCredits.set(credits);
    this.netChange.set(debits - credits);
  }

  onSearch(): void {
    this.pageNumber = 1;
    if (this.selectedTabIndex === 0) {
      this.loadEntries();
    } else {
      this.loadSummary();
    }
  }

  onTabChange(): void {
    if (this.selectedTabIndex === 0) {
      this.loadEntries();
    } else {
      this.loadSummary();
    }
  }

  onSort(sort: Sort): void {
    this.sortBy = sort.active;
    this.sortDescending = sort.direction === 'desc';
    this.loadEntries();
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadEntries();
  }

  displayAccount = (accountId: string): string => {
    const account = this.accounts().find(a => a.id === accountId);
    return account ? `${account.accountCode} - ${account.accountName}` : '';
  };

  clearAccount(): void {
    this.selectedAccountId = '';
    this.onSearch();
  }

  clearFilters(): void {
    this.selectedAccountId = '';
    this.selectedPeriodId = '';
    this.startDate = null;
    this.endDate = null;
    this.searchTerm = '';
    this.onSearch();
  }

  selectAccount(accountId: string): void {
    this.selectedAccountId = accountId;
    this.selectedTabIndex = 0;
    this.loadEntries();
  }

  viewJournalEntry(entry: GeneralLedgerEntry): void {
    // TODO: Open journal entry dialog
    console.log('View journal entry:', entry.journalEntryId);
  }

  exportToExcel(): void {
    // TODO: Implement export functionality
    this.snackBar.open('Export functionality coming soon', 'Close', { duration: 3000 });
  }

  print(): void {
    window.print();
  }
}

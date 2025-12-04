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
import { MatMenuModule } from '@angular/material/menu';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDividerModule } from '@angular/material/divider';

import { AccountingService } from '@core/services/accounting.service';
import {
  TrialBalance,
  TrialBalanceLine,
  AccountingPeriod,
  AccountingSearchRequest
} from '@core/models/accounting.model';
import { GenerateTrialBalanceDialogComponent } from './generate-trial-balance-dialog.component';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-trial-balance',
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
    MatMenuModule,
    MatDialogModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatChipsModule,
    MatTooltipModule,
    MatDividerModule
  ],
  template: `
    <div class="page-container">
      <mat-card>
        <mat-card-header>
          <mat-card-title>
            <div class="header-row">
              <span>Trial Balance</span>
              <div class="header-actions">
                <button mat-raised-button color="primary" (click)="openGenerateDialog()">
                  <mat-icon>calculate</mat-icon>
                  Generate Trial Balance
                </button>
              </div>
            </div>
          </mat-card-title>
        </mat-card-header>

        <mat-card-content>
          <!-- Filters -->
          <div class="filters-row">
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
              <mat-label>Status</mat-label>
              <mat-select [(ngModel)]="selectedStatus" (ngModelChange)="onSearch()">
                <mat-option value="">All</mat-option>
                <mat-option value="Draft">Draft</mat-option>
                <mat-option value="Finalized">Finalized</mat-option>
              </mat-select>
            </mat-form-field>

            <mat-form-field appearance="outline" class="search-field">
              <mat-label>Search</mat-label>
              <input matInput [(ngModel)]="searchTerm" (ngModelChange)="onSearch()"
                     placeholder="Search...">
              <mat-icon matSuffix>search</mat-icon>
            </mat-form-field>
          </div>

          @if (loading()) {
            <div class="loading-container">
              <mat-spinner diameter="40"></mat-spinner>
            </div>
          }

          <!-- Trial Balance List -->
          @if (!loading() && !selectedTrialBalance()) {
            <div class="table-container">
              <table mat-table [dataSource]="dataSource" matSort (matSortChange)="onSort($event)">
                <ng-container matColumnDef="name">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Name</th>
                  <td mat-cell *matCellDef="let tb">{{ tb.trialBalanceNumber }}</td>
                </ng-container>

                <ng-container matColumnDef="periodName">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Period</th>
                  <td mat-cell *matCellDef="let tb">{{ tb.periodName }}</td>
                </ng-container>

                <ng-container matColumnDef="asOfDate">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>As Of Date</th>
                  <td mat-cell *matCellDef="let tb">{{ tb.asOfDate | date:'shortDate' }}</td>
                </ng-container>

                <ng-container matColumnDef="totalDebits">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Total Debits</th>
                  <td mat-cell *matCellDef="let tb" class="text-right">
                    {{ tb.totalDebits | currency }}
                  </td>
                </ng-container>

                <ng-container matColumnDef="totalCredits">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Total Credits</th>
                  <td mat-cell *matCellDef="let tb" class="text-right">
                    {{ tb.totalCredits | currency }}
                  </td>
                </ng-container>

                <ng-container matColumnDef="isBalanced">
                  <th mat-header-cell *matHeaderCellDef>Balanced</th>
                  <td mat-cell *matCellDef="let tb">
                    <mat-icon [class]="tb.isBalanced ? 'balanced' : 'unbalanced'">
                      {{ tb.isBalanced ? 'check_circle' : 'error' }}
                    </mat-icon>
                  </td>
                </ng-container>

                <ng-container matColumnDef="status">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Status</th>
                  <td mat-cell *matCellDef="let tb">
                    <mat-chip [class]="'status-chip ' + tb.status.toLowerCase()">
                      {{ tb.status }}
                    </mat-chip>
                  </td>
                </ng-container>

                <ng-container matColumnDef="actions">
                  <th mat-header-cell *matHeaderCellDef>Actions</th>
                  <td mat-cell *matCellDef="let tb">
                    <button mat-icon-button [matMenuTriggerFor]="menu" (click)="$event.stopPropagation()">
                      <mat-icon>more_vert</mat-icon>
                    </button>
                    <mat-menu #menu="matMenu">
                      <button mat-menu-item (click)="viewTrialBalance(tb)">
                        <mat-icon>visibility</mat-icon> View Details
                      </button>
                      @if (tb.status === 'Draft') {
                        <button mat-menu-item (click)="finalizeTrialBalance(tb)">
                          <mat-icon>lock</mat-icon> Finalize
                        </button>
                      }
                      @if (tb.status === 'Finalized') {
                        <button mat-menu-item (click)="reopenTrialBalance(tb)">
                          <mat-icon>lock_open</mat-icon> Reopen
                        </button>
                      }
                      <button mat-menu-item (click)="exportTrialBalance(tb, 'excel')">
                        <mat-icon>table_chart</mat-icon> Export to Excel
                      </button>
                      <button mat-menu-item (click)="exportTrialBalance(tb, 'pdf')">
                        <mat-icon>picture_as_pdf</mat-icon> Export to PDF
                      </button>
                      <button mat-menu-item (click)="printTrialBalance(tb)">
                        <mat-icon>print</mat-icon> Print
                      </button>
                    </mat-menu>
                  </td>
                </ng-container>

                <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
                <tr mat-row *matRowDef="let row; columns: displayedColumns;"
                    (click)="viewTrialBalance(row)"
                    class="clickable-row"></tr>
              </table>

              <mat-paginator
                [length]="totalRecords()"
                [pageSize]="pageSize"
                [pageSizeOptions]="[10, 25, 50]"
                (page)="onPageChange($event)"
                showFirstLastButtons>
              </mat-paginator>
            </div>

            @if (trialBalances().length === 0) {
              <div class="empty-state">
                <mat-icon>balance</mat-icon>
                <h3>No trial balances found</h3>
                <p>Generate a new trial balance to get started</p>
                <button mat-raised-button color="primary" (click)="openGenerateDialog()">
                  <mat-icon>calculate</mat-icon>
                  Generate Trial Balance
                </button>
              </div>
            }
          }

          <!-- Trial Balance Detail View -->
          @if (selectedTrialBalance()) {
            <div class="detail-view">
              <div class="detail-header">
                <button mat-icon-button (click)="clearSelection()">
                  <mat-icon>arrow_back</mat-icon>
                </button>
                <div class="detail-info">
                  <h2>{{ selectedTrialBalance()!.trialBalanceNumber }}</h2>
                  <span class="detail-subtitle">
                    {{ selectedTrialBalance()!.periodName }} | As of {{ selectedTrialBalance()!.asOfDate | date:'mediumDate' }}
                  </span>
                </div>
                <mat-chip [class]="'status-chip ' + selectedTrialBalance()!.status.toLowerCase()">
                  {{ selectedTrialBalance()!.status }}
                </mat-chip>
                <div class="detail-actions">
                  <button mat-stroked-button (click)="exportTrialBalance(selectedTrialBalance()!, 'excel')">
                    <mat-icon>table_chart</mat-icon> Excel
                  </button>
                  <button mat-stroked-button (click)="exportTrialBalance(selectedTrialBalance()!, 'pdf')">
                    <mat-icon>picture_as_pdf</mat-icon> PDF
                  </button>
                  <button mat-stroked-button (click)="printTrialBalance(selectedTrialBalance()!)">
                    <mat-icon>print</mat-icon> Print
                  </button>
                </div>
              </div>

              <table mat-table [dataSource]="lineDataSource" class="lines-table">
                <ng-container matColumnDef="accountCode">
                  <th mat-header-cell *matHeaderCellDef>Account Code</th>
                  <td mat-cell *matCellDef="let line">
                    <span class="account-code">{{ line.accountCode }}</span>
                  </td>
                </ng-container>

                <ng-container matColumnDef="accountName">
                  <th mat-header-cell *matHeaderCellDef>Account Name</th>
                  <td mat-cell *matCellDef="let line">{{ line.accountName }}</td>
                </ng-container>

                <ng-container matColumnDef="accountType">
                  <th mat-header-cell *matHeaderCellDef>Type</th>
                  <td mat-cell *matCellDef="let line">
                    <mat-chip [class]="'type-chip ' + line.accountType?.toLowerCase()">
                      {{ line.accountType }}
                    </mat-chip>
                  </td>
                </ng-container>

                <ng-container matColumnDef="debitBalance">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Debit</th>
                  <td mat-cell *matCellDef="let line" class="text-right amount-debit">
                    {{ line.debitBalance > 0 ? (line.debitBalance | currency) : '' }}
                  </td>
                </ng-container>

                <ng-container matColumnDef="creditBalance">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Credit</th>
                  <td mat-cell *matCellDef="let line" class="text-right amount-credit">
                    {{ line.creditBalance > 0 ? (line.creditBalance | currency) : '' }}
                  </td>
                </ng-container>

                <tr mat-header-row *matHeaderRowDef="lineColumns"></tr>
                <tr mat-row *matRowDef="let row; columns: lineColumns;"></tr>

                <!-- Totals Row -->
                <ng-container matColumnDef="totalLabel">
                  <td mat-footer-cell *matFooterCellDef colspan="3" class="totals-label">
                    <strong>TOTALS</strong>
                  </td>
                </ng-container>

                <ng-container matColumnDef="totalDebit">
                  <td mat-footer-cell *matFooterCellDef class="text-right totals-value amount-debit">
                    <strong>{{ selectedTrialBalance()!.totalDebits | currency }}</strong>
                  </td>
                </ng-container>

                <ng-container matColumnDef="totalCredit">
                  <td mat-footer-cell *matFooterCellDef class="text-right totals-value amount-credit">
                    <strong>{{ selectedTrialBalance()!.totalCredits | currency }}</strong>
                  </td>
                </ng-container>

                <tr mat-footer-row *matFooterRowDef="['totalLabel', 'totalDebit', 'totalCredit']"></tr>
              </table>

              <div class="balance-status" [class.balanced]="selectedTrialBalance()!.isBalanced"
                   [class.unbalanced]="!selectedTrialBalance()!.isBalanced">
                <mat-icon>{{ selectedTrialBalance()!.isBalanced ? 'check_circle' : 'error' }}</mat-icon>
                <span>
                  {{ selectedTrialBalance()!.isBalanced ? 'Trial balance is balanced' : 'Trial balance is NOT balanced' }}
                </span>
                @if (!selectedTrialBalance()!.isBalanced) {
                  <span class="difference">
                    Difference: {{ (selectedTrialBalance()!.totalDebits - selectedTrialBalance()!.totalCredits) | currency }}
                  </span>
                }
              </div>
            </div>
          }
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

    .filters-row {
      display: flex;
      gap: 16px;
      margin-bottom: 16px;
      flex-wrap: wrap;
    }

    .search-field {
      flex: 1;
      min-width: 250px;
    }

    .table-container {
      overflow-x: auto;
    }

    table {
      width: 100%;
    }

    .text-right {
      text-align: right;
    }

    .clickable-row {
      cursor: pointer;
      transition: background-color 0.2s;
    }

    .clickable-row:hover {
      background-color: rgba(0, 0, 0, 0.04);
    }

    .balanced {
      color: #388e3c;
    }

    .unbalanced {
      color: #c62828;
    }

    .status-chip {
      font-size: 12px;
    }

    .status-chip.draft {
      background-color: #fff3e0;
      color: #f57c00;
    }

    .status-chip.finalized {
      background-color: #e8f5e9;
      color: #388e3c;
    }

    .type-chip {
      font-size: 11px;
    }

    .type-chip.asset {
      background-color: #e3f2fd;
      color: #1976d2;
    }

    .type-chip.liability {
      background-color: #fce4ec;
      color: #c2185b;
    }

    .type-chip.equity {
      background-color: #e8f5e9;
      color: #388e3c;
    }

    .type-chip.revenue {
      background-color: #fff3e0;
      color: #f57c00;
    }

    .type-chip.expense {
      background-color: #f3e5f5;
      color: #7b1fa2;
    }

    .amount-debit {
      color: #1976d2;
    }

    .amount-credit {
      color: #388e3c;
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

    /* Detail View */
    .detail-view {
      margin-top: 16px;
    }

    .detail-header {
      display: flex;
      align-items: center;
      gap: 16px;
      margin-bottom: 24px;
      padding-bottom: 16px;
      border-bottom: 1px solid #e0e0e0;
    }

    .detail-info {
      flex: 1;
    }

    .detail-info h2 {
      margin: 0;
    }

    .detail-subtitle {
      color: #666;
    }

    .detail-actions {
      display: flex;
      gap: 8px;
    }

    .lines-table {
      margin-bottom: 16px;
    }

    .account-code {
      font-family: monospace;
      font-weight: 500;
    }

    .totals-label {
      font-weight: 600;
      background-color: #f5f5f5;
    }

    .totals-value {
      background-color: #f5f5f5;
    }

    .balance-status {
      display: flex;
      align-items: center;
      gap: 8px;
      padding: 16px;
      border-radius: 8px;
      font-weight: 500;
    }

    .balance-status.balanced {
      background-color: #e8f5e9;
      color: #388e3c;
    }

    .balance-status.unbalanced {
      background-color: #ffebee;
      color: #c62828;
    }

    .difference {
      margin-left: auto;
    }
  `]
})
export class TrialBalanceComponent implements OnInit {
  private accountingService = inject(AccountingService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  trialBalances = signal<TrialBalance[]>([]);
  periods = signal<AccountingPeriod[]>([]);
  loading = signal(false);
  totalRecords = signal(0);
  selectedTrialBalance = signal<TrialBalance | null>(null);

  dataSource = new MatTableDataSource<TrialBalance>();
  lineDataSource = new MatTableDataSource<TrialBalanceLine>();

  displayedColumns = ['name', 'periodName', 'asOfDate', 'totalDebits', 'totalCredits', 'isBalanced', 'status', 'actions'];
  lineColumns = ['accountCode', 'accountName', 'accountType', 'debitBalance', 'creditBalance'];

  selectedPeriodId = '';
  selectedStatus = '';
  searchTerm = '';
  pageSize = 25;
  pageNumber = 1;
  sortBy = 'asOfDate';
  sortDescending = true;

  ngOnInit(): void {
    this.loadPeriods();
    this.loadTrialBalances();
  }

  loadPeriods(): void {
    this.accountingService.getAccountingPeriods({ pageNumber: 1, pageSize: 100 }).subscribe({
      next: (result) => {
        this.periods.set(result.items);
      }
    });
  }

  loadTrialBalances(): void {
    this.loading.set(true);
    const request: AccountingSearchRequest = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      sortDescending: this.sortDescending,
      periodId: this.selectedPeriodId || undefined,
      status: this.selectedStatus || undefined
    };

    this.accountingService.getTrialBalances(request).subscribe({
      next: (result) => {
        this.trialBalances.set(result.items);
        this.totalRecords.set(result.totalCount);
        this.dataSource.data = result.items;
        this.loading.set(false);
      },
      error: (error) => {
        console.error('Error loading trial balances:', error);
        this.snackBar.open('Failed to load trial balances', 'Close', { duration: 3000 });
        this.loading.set(false);
      }
    });
  }

  onSearch(): void {
    this.pageNumber = 1;
    this.loadTrialBalances();
  }

  onSort(sort: Sort): void {
    this.sortBy = sort.active;
    this.sortDescending = sort.direction === 'desc';
    this.loadTrialBalances();
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadTrialBalances();
  }

  openGenerateDialog(): void {
    const dialogRef = this.dialog.open(GenerateTrialBalanceDialogComponent, {
      width: '500px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadTrialBalances();
      }
    });
  }

  viewTrialBalance(tb: TrialBalance): void {
    this.accountingService.getTrialBalanceById(tb.id).subscribe({
      next: (trialBalance) => {
        this.selectedTrialBalance.set(trialBalance);
        this.lineDataSource.data = trialBalance.lines || [];
      },
      error: (error) => {
        console.error('Error loading trial balance:', error);
        this.snackBar.open('Failed to load trial balance details', 'Close', { duration: 3000 });
      }
    });
  }

  clearSelection(): void {
    this.selectedTrialBalance.set(null);
  }

  finalizeTrialBalance(tb: TrialBalance): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Finalize Trial Balance',
        message: `Are you sure you want to finalize "${tb.trialBalanceNumber}"? This action will lock the trial balance.`,
        confirmText: 'Finalize',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.accountingService.finalizeTrialBalance(tb.id).subscribe({
          next: () => {
            this.snackBar.open('Trial balance finalized', 'Close', { duration: 3000 });
            this.loadTrialBalances();
          },
          error: (error) => {
            console.error('Error finalizing trial balance:', error);
            this.snackBar.open('Failed to finalize trial balance', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }

  reopenTrialBalance(tb: TrialBalance): void {
    // TODO: Show dialog to get reason
    const reason = 'Reopened for corrections';
    this.accountingService.reopenTrialBalance(tb.id, reason).subscribe({
      next: () => {
        this.snackBar.open('Trial balance reopened', 'Close', { duration: 3000 });
        this.loadTrialBalances();
      },
      error: (error) => {
        console.error('Error reopening trial balance:', error);
        this.snackBar.open('Failed to reopen trial balance', 'Close', { duration: 3000 });
      }
    });
  }

  exportTrialBalance(tb: TrialBalance, format: 'pdf' | 'excel'): void {
    this.accountingService.exportTrialBalance(tb.id, format).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `trial-balance-${tb.trialBalanceNumber}.${format === 'pdf' ? 'pdf' : 'xlsx'}`;
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: (error) => {
        console.error('Error exporting trial balance:', error);
        this.snackBar.open('Failed to export trial balance', 'Close', { duration: 3000 });
      }
    });
  }

  printTrialBalance(tb: TrialBalance): void {
    window.print();
  }
}

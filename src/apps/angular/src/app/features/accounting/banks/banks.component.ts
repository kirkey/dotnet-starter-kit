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
import { MatMenuModule } from '@angular/material/menu';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';

import { AccountingService } from '@core/services/accounting.service';
import { Bank, AccountingSearchRequest } from '@core/models/accounting.model';
import { BankDialogComponent } from './bank-dialog.component';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-banks',
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
    MatMenuModule,
    MatDialogModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatTooltipModule
  ],
  template: `
    <div class="page-container">
      <mat-card>
        <mat-card-header>
          <mat-card-title>
            <div class="header-row">
              <span>Bank Accounts</span>
              <button mat-raised-button color="primary" (click)="openDialog()">
                <mat-icon>add</mat-icon>
                Add Bank Account
              </button>
            </div>
          </mat-card-title>
        </mat-card-header>

        <mat-card-content>
          <!-- Search -->
          <div class="filters-row">
            <mat-form-field appearance="outline" class="search-field">
              <mat-label>Search Bank Accounts</mat-label>
              <input matInput [(ngModel)]="searchTerm" (ngModelChange)="onSearch()"
                     placeholder="Search by name, account number, bank...">
              <mat-icon matSuffix>search</mat-icon>
            </mat-form-field>
          </div>

          @if (loading()) {
            <div class="loading-container">
              <mat-spinner diameter="40"></mat-spinner>
            </div>
          }

          @if (!loading()) {
            <div class="table-container">
              <table mat-table [dataSource]="dataSource" matSort (matSortChange)="onSort($event)">
                <ng-container matColumnDef="bankName">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Account Name</th>
                  <td mat-cell *matCellDef="let bank">
                    <div class="bank-name">
                      <mat-icon class="bank-icon">account_balance</mat-icon>
                      {{ bank.bankName }}
                    </div>
                  </td>
                </ng-container>

                <ng-container matColumnDef="bankName">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Bank</th>
                  <td mat-cell *matCellDef="let bank">{{ bank.bankName }}</td>
                </ng-container>

                <ng-container matColumnDef="accountNumber">
                  <th mat-header-cell *matHeaderCellDef>Account Number</th>
                  <td mat-cell *matCellDef="let bank">
                    <span class="account-number">{{ maskAccountNumber(bank.accountNumber) }}</span>
                  </td>
                </ng-container>

                <ng-container matColumnDef="accountType">
                  <th mat-header-cell *matHeaderCellDef>Type</th>
                  <td mat-cell *matCellDef="let bank">{{ bank.accountType }}</td>
                </ng-container>

                <ng-container matColumnDef="currentBalance">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Current Balance</th>
                  <td mat-cell *matCellDef="let bank" class="text-right">
                    <span [class]="bank.currentBalance >= 0 ? 'positive' : 'negative'">
                      {{ bank.currentBalance | currency }}
                    </span>
                  </td>
                </ng-container>

                <ng-container matColumnDef="lastReconciledDate">
                  <th mat-header-cell *matHeaderCellDef>Last Reconciled</th>
                  <td mat-cell *matCellDef="let bank">
                    {{ bank.lastReconciledDate ? (bank.lastReconciledDate | date:'shortDate') : 'Never' }}
                  </td>
                </ng-container>

                <ng-container matColumnDef="isActive">
                  <th mat-header-cell *matHeaderCellDef>Status</th>
                  <td mat-cell *matCellDef="let bank">
                    <mat-chip [class]="bank.isActive ? 'status-active' : 'status-inactive'">
                      {{ bank.isActive ? 'Active' : 'Inactive' }}
                    </mat-chip>
                  </td>
                </ng-container>

                <ng-container matColumnDef="actions">
                  <th mat-header-cell *matHeaderCellDef>Actions</th>
                  <td mat-cell *matCellDef="let bank">
                    <button mat-icon-button [matMenuTriggerFor]="menu" (click)="$event.stopPropagation()">
                      <mat-icon>more_vert</mat-icon>
                    </button>
                    <mat-menu #menu="matMenu">
                      <button mat-menu-item (click)="openDialog(bank)">
                        <mat-icon>edit</mat-icon> Edit
                      </button>
                      <button mat-menu-item (click)="viewTransactions(bank)">
                        <mat-icon>list</mat-icon> View Transactions
                      </button>
                      <button mat-menu-item (click)="reconcile(bank)">
                        <mat-icon>check_circle</mat-icon> Reconcile
                      </button>
                      <button mat-menu-item (click)="importTransactions(bank)">
                        <mat-icon>upload</mat-icon> Import Transactions
                      </button>
                      <button mat-menu-item (click)="viewStatement(bank)">
                        <mat-icon>description</mat-icon> Bank Statement
                      </button>
                      <button mat-menu-item (click)="deleteBank(bank)" class="delete-item">
                        <mat-icon>delete</mat-icon> Delete
                      </button>
                    </mat-menu>
                  </td>
                </ng-container>

                <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
                <tr mat-row *matRowDef="let row; columns: displayedColumns;"
                    (click)="openDialog(row)"
                    class="clickable-row"></tr>
              </table>

              <mat-paginator
                [length]="totalRecords()"
                [pageSize]="pageSize"
                [pageSizeOptions]="[10, 25, 50, 100]"
                (page)="onPageChange($event)"
                showFirstLastButtons>
              </mat-paginator>
            </div>
          }

          @if (!loading() && banks().length === 0) {
            <div class="empty-state">
              <mat-icon>account_balance</mat-icon>
              <h3>No bank accounts found</h3>
              <p>Add your first bank account to start managing cash</p>
              <button mat-raised-button color="primary" (click)="openDialog()">
                <mat-icon>add</mat-icon>
                Add Bank Account
              </button>
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

    .filters-row {
      display: flex;
      gap: 16px;
      margin-bottom: 16px;
      flex-wrap: wrap;
    }

    .search-field {
      flex: 1;
      min-width: 250px;
      max-width: 400px;
    }

    .table-container {
      overflow-x: auto;
    }

    table {
      width: 100%;
    }

    .bank-name {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .bank-icon {
      color: #1976d2;
    }

    .account-number {
      font-family: monospace;
    }

    .text-right {
      text-align: right;
    }

    .positive {
      color: #388e3c;
      font-weight: 500;
    }

    .negative {
      color: #d32f2f;
      font-weight: 500;
    }

    .clickable-row {
      cursor: pointer;
      transition: background-color 0.2s;
    }

    .clickable-row:hover {
      background-color: rgba(0, 0, 0, 0.04);
    }

    .status-active {
      background-color: #e8f5e9;
      color: #388e3c;
    }

    .status-inactive {
      background-color: #fafafa;
      color: #9e9e9e;
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

    .delete-item {
      color: #f44336;
    }
  `]
})
export class BanksComponent implements OnInit {
  private accountingService = inject(AccountingService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  banks = signal<Bank[]>([]);
  loading = signal(false);
  totalRecords = signal(0);

  dataSource = new MatTableDataSource<Bank>();

  displayedColumns = ['bankName', 'bankCode', 'accountNumber', 'accountType', 'currentBalance', 'lastReconciledDate', 'isActive', 'actions'];

  searchTerm = '';
  pageSize = 25;
  pageNumber = 1;
  sortBy = 'bankName';
  sortDescending = false;

  ngOnInit(): void {
    this.loadBanks();
  }

  loadBanks(): void {
    this.loading.set(true);
    const request: AccountingSearchRequest = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      sortDescending: this.sortDescending
    };

    this.accountingService.getBanks(request).subscribe({
      next: (result) => {
        this.banks.set(result.items);
        this.totalRecords.set(result.totalCount);
        this.dataSource.data = result.items;
        this.loading.set(false);
      },
      error: (error) => {
        console.error('Error loading banks:', error);
        this.snackBar.open('Failed to load bank accounts', 'Close', { duration: 3000 });
        this.loading.set(false);
      }
    });
  }

  onSearch(): void {
    this.pageNumber = 1;
    this.loadBanks();
  }

  onSort(sort: Sort): void {
    this.sortBy = sort.active;
    this.sortDescending = sort.direction === 'desc';
    this.loadBanks();
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadBanks();
  }

  maskAccountNumber(accountNumber: string): string {
    if (!accountNumber || accountNumber.length < 4) return accountNumber;
    const lastFour = accountNumber.slice(-4);
    return '****' + lastFour;
  }

  openDialog(bank?: Bank): void {
    const dialogRef = this.dialog.open(BankDialogComponent, {
      width: '600px',
      maxHeight: '90vh',
      data: { bank }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadBanks();
      }
    });
  }

  viewTransactions(bank: Bank): void {
    // TODO: Navigate to bank transactions
  }

  reconcile(bank: Bank): void {
    // TODO: Navigate to bank reconciliation
  }

  importTransactions(bank: Bank): void {
    // TODO: Open import dialog
  }

  viewStatement(bank: Bank): void {
    // TODO: Generate bank statement
  }

  deleteBank(bank: Bank): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Delete Bank Account',
        message: `Are you sure you want to delete "${bank.bankName}"? This action cannot be undone.`,
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.accountingService.deleteBank(bank.id).subscribe({
          next: () => {
            this.snackBar.open('Bank account deleted successfully', 'Close', { duration: 3000 });
            this.loadBanks();
          },
          error: (error) => {
            console.error('Error deleting bank:', error);
            this.snackBar.open('Failed to delete bank account', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }
}

import { Component, OnInit, inject, signal, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSortModule, MatSort, Sort } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';

import { AccountingService } from '@core/services/accounting.service';
import { BankReconciliation, Bank, AccountingSearchRequest } from '@core/models/accounting.model';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-bank-reconciliations',
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
    MatSelectModule,
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
              <span>Bank Reconciliations</span>
              <button mat-raised-button color="primary" (click)="startReconciliation()">
                <mat-icon>add</mat-icon>
                Start Reconciliation
              </button>
            </div>
          </mat-card-title>
        </mat-card-header>

        <mat-card-content>
          <!-- Filters -->
          <div class="filters-row">
            <mat-form-field appearance="outline" class="search-field">
              <mat-label>Bank Account</mat-label>
              <mat-select [(ngModel)]="selectedBankId" (ngModelChange)="onSearch()">
                <mat-option value="">All Accounts</mat-option>
                @for (bank of banks(); track bank.id) {
                  <mat-option [value]="bank.id">{{ bank.bankName }}</mat-option>
                }
              </mat-select>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Status</mat-label>
              <mat-select [(ngModel)]="selectedStatus" (ngModelChange)="onSearch()">
                <mat-option value="">All</mat-option>
                <mat-option value="InProgress">In Progress</mat-option>
                <mat-option value="Completed">Completed</mat-option>
                <mat-option value="Voided">Voided</mat-option>
              </mat-select>
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
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Bank Account</th>
                  <td mat-cell *matCellDef="let rec">
                    <div class="bank-cell">
                      <mat-icon>account_balance</mat-icon>
                      {{ rec.bankName }}
                    </div>
                  </td>
                </ng-container>

                <ng-container matColumnDef="statementDate">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Statement Date</th>
                  <td mat-cell *matCellDef="let rec">{{ rec.statementDate | date:'shortDate' }}</td>
                </ng-container>

                <ng-container matColumnDef="statementEndingBalance">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Statement Balance</th>
                  <td mat-cell *matCellDef="let rec" class="text-right">
                    {{ rec.statementEndingBalance | currency }}
                  </td>
                </ng-container>

                <ng-container matColumnDef="bookBalance">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Book Balance</th>
                  <td mat-cell *matCellDef="let rec" class="text-right">
                    {{ rec.bookBalance | currency }}
                  </td>
                </ng-container>

                <ng-container matColumnDef="difference">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Difference</th>
                  <td mat-cell *matCellDef="let rec" class="text-right">
                    <span [class]="getDifferenceClass(rec)">
                      {{ rec.difference | currency }}
                    </span>
                  </td>
                </ng-container>

                <ng-container matColumnDef="status">
                  <th mat-header-cell *matHeaderCellDef>Status</th>
                  <td mat-cell *matCellDef="let rec">
                    <mat-chip [class]="getStatusClass(rec.status)">
                      {{ rec.status }}
                    </mat-chip>
                  </td>
                </ng-container>

                <ng-container matColumnDef="reconciledBy">
                  <th mat-header-cell *matHeaderCellDef>Reconciled By</th>
                  <td mat-cell *matCellDef="let rec">{{ rec.reconciledBy || '-' }}</td>
                </ng-container>

                <ng-container matColumnDef="actions">
                  <th mat-header-cell *matHeaderCellDef>Actions</th>
                  <td mat-cell *matCellDef="let rec">
                    <button mat-icon-button [matMenuTriggerFor]="menu" (click)="$event.stopPropagation()">
                      <mat-icon>more_vert</mat-icon>
                    </button>
                    <mat-menu #menu="matMenu">
                      @if (rec.status === 'InProgress') {
                        <button mat-menu-item (click)="continueReconciliation(rec)">
                          <mat-icon>play_arrow</mat-icon> Continue
                        </button>
                        <button mat-menu-item (click)="completeReconciliation(rec)">
                          <mat-icon>check_circle</mat-icon> Complete
                        </button>
                      }
                      @if (rec.status === 'Completed') {
                        <button mat-menu-item (click)="viewReconciliation(rec)">
                          <mat-icon>visibility</mat-icon> View Details
                        </button>
                        <button mat-menu-item (click)="printReconciliation(rec)">
                          <mat-icon>print</mat-icon> Print Report
                        </button>
                      }
                      @if (rec.status !== 'Voided') {
                        <button mat-menu-item (click)="voidReconciliation(rec)" class="delete-item">
                          <mat-icon>block</mat-icon> Void
                        </button>
                      }
                    </mat-menu>
                  </td>
                </ng-container>

                <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
                <tr mat-row *matRowDef="let row; columns: displayedColumns;"
                    (click)="viewReconciliation(row)"
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

          @if (!loading() && reconciliations().length === 0) {
            <div class="empty-state">
              <mat-icon>check_circle</mat-icon>
              <h3>No reconciliations found</h3>
              <p>Start a new bank reconciliation to match your records</p>
              <button mat-raised-button color="primary" (click)="startReconciliation()">
                <mat-icon>add</mat-icon>
                Start Reconciliation
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
      min-width: 250px;
    }

    .table-container {
      overflow-x: auto;
    }

    table {
      width: 100%;
    }

    .bank-cell {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .bank-cell mat-icon {
      color: #1976d2;
    }

    .text-right {
      text-align: right;
    }

    .balanced {
      color: #388e3c;
      font-weight: 500;
    }

    .unbalanced {
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

    .status-inprogress { background-color: #fff3e0; color: #f57c00; }
    .status-completed { background-color: #e8f5e9; color: #388e3c; }
    .status-voided { background-color: #fafafa; color: #9e9e9e; }

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
export class BankReconciliationsComponent implements OnInit {
  private accountingService = inject(AccountingService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  reconciliations = signal<BankReconciliation[]>([]);
  banks = signal<Bank[]>([]);
  loading = signal(false);
  totalRecords = signal(0);

  dataSource = new MatTableDataSource<BankReconciliation>();

  displayedColumns = ['bankName', 'statementDate', 'statementEndingBalance', 'bookBalance', 'difference', 'status', 'reconciledBy', 'actions'];

  selectedBankId = '';
  selectedStatus = '';
  pageSize = 25;
  pageNumber = 1;
  sortBy = 'statementDate';
  sortDescending = true;

  ngOnInit(): void {
    this.loadBanks();
    this.loadReconciliations();
  }

  private loadBanks(): void {
    this.accountingService.getBanks({ pageNumber: 1, pageSize: 100 }).subscribe({
      next: (result) => this.banks.set(result.items),
      error: (error) => console.error('Error loading banks:', error)
    });
  }

  loadReconciliations(): void {
    this.loading.set(true);
    const request: AccountingSearchRequest = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      sortBy: this.sortBy,
      sortDescending: this.sortDescending,
      status: this.selectedStatus || undefined
    };

    this.accountingService.getBankReconciliations(request).subscribe({
      next: (result) => {
        this.reconciliations.set(result.items);
        this.totalRecords.set(result.totalCount);
        this.dataSource.data = result.items;
        this.loading.set(false);
      },
      error: (error) => {
        console.error('Error loading reconciliations:', error);
        this.snackBar.open('Failed to load reconciliations', 'Close', { duration: 3000 });
        this.loading.set(false);
      }
    });
  }

  onSearch(): void {
    this.pageNumber = 1;
    this.loadReconciliations();
  }

  onSort(sort: Sort): void {
    this.sortBy = sort.active;
    this.sortDescending = sort.direction === 'desc';
    this.loadReconciliations();
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadReconciliations();
  }

  getDifferenceClass(rec: BankReconciliation): string {
    return Math.abs(rec.difference) < 0.01 ? 'balanced' : 'unbalanced';
  }

  getStatusClass(status: string): string {
    return `status-${status.toLowerCase()}`;
  }

  startReconciliation(): void {
    // TODO: Open start reconciliation dialog
    this.snackBar.open('Start reconciliation feature coming soon', 'Close', { duration: 3000 });
  }

  continueReconciliation(rec: BankReconciliation): void {
    // TODO: Navigate to reconciliation page
  }

  completeReconciliation(rec: BankReconciliation): void {
    // TODO: Complete reconciliation
  }

  viewReconciliation(rec: BankReconciliation): void {
    // TODO: View reconciliation details
  }

  printReconciliation(rec: BankReconciliation): void {
    // TODO: Print reconciliation report
  }

  voidReconciliation(rec: BankReconciliation): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Void Reconciliation',
        message: 'Are you sure you want to void this reconciliation? This action cannot be undone.',
        confirmText: 'Void',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.accountingService.voidBankReconciliation(rec.id).subscribe({
          next: () => {
            this.snackBar.open('Reconciliation voided successfully', 'Close', { duration: 3000 });
            this.loadReconciliations();
          },
          error: (error) => {
            console.error('Error voiding reconciliation:', error);
            this.snackBar.open('Failed to void reconciliation', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }
}

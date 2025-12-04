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
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';

import { AccountingService } from '@core/services/accounting.service';
import { Payment, AccountingSearchRequest } from '@core/models/accounting.model';
import { PaymentDialogComponent } from './payment-dialog.component';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-payments',
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
    MatDatepickerModule,
    MatNativeDateModule,
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
              <span>Payments</span>
              <button mat-raised-button color="primary" (click)="openDialog()">
                <mat-icon>add</mat-icon>
                Receive Payment
              </button>
            </div>
          </mat-card-title>
        </mat-card-header>

        <mat-card-content>
          <!-- Filters -->
          <div class="filters-row">
            <mat-form-field appearance="outline" class="search-field">
              <mat-label>Search</mat-label>
              <input matInput [(ngModel)]="searchTerm" (ngModelChange)="onSearch()"
                     placeholder="Payment number, customer, invoice...">
              <mat-icon matSuffix>search</mat-icon>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Payment Method</mat-label>
              <mat-select [(ngModel)]="selectedMethod" (ngModelChange)="onSearch()">
                <mat-option value="">All Methods</mat-option>
                <mat-option value="Cash">Cash</mat-option>
                <mat-option value="Check">Check</mat-option>
                <mat-option value="CreditCard">Credit Card</mat-option>
                <mat-option value="BankTransfer">Bank Transfer</mat-option>
                <mat-option value="ACH">ACH</mat-option>
                <mat-option value="Wire">Wire Transfer</mat-option>
                <mat-option value="Other">Other</mat-option>
              </mat-select>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>From Date</mat-label>
              <input matInput [matDatepicker]="fromPicker" [(ngModel)]="fromDate" (ngModelChange)="onSearch()">
              <mat-datepicker-toggle matIconSuffix [for]="fromPicker"></mat-datepicker-toggle>
              <mat-datepicker #fromPicker></mat-datepicker>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>To Date</mat-label>
              <input matInput [matDatepicker]="toPicker" [(ngModel)]="toDate" (ngModelChange)="onSearch()">
              <mat-datepicker-toggle matIconSuffix [for]="toPicker"></mat-datepicker-toggle>
              <mat-datepicker #toPicker></mat-datepicker>
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
                <ng-container matColumnDef="paymentNumber">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Payment #</th>
                  <td mat-cell *matCellDef="let payment">
                    <span class="payment-number">{{ payment.paymentNumber }}</span>
                  </td>
                </ng-container>

                <ng-container matColumnDef="customerName">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Customer</th>
                  <td mat-cell *matCellDef="let payment">{{ payment.customerName }}</td>
                </ng-container>

                <ng-container matColumnDef="paymentDate">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Date</th>
                  <td mat-cell *matCellDef="let payment">{{ payment.paymentDate | date:'shortDate' }}</td>
                </ng-container>

                <ng-container matColumnDef="paymentMethod">
                  <th mat-header-cell *matHeaderCellDef>Method</th>
                  <td mat-cell *matCellDef="let payment">
                    <div class="method-cell">
                      <mat-icon class="method-icon">{{ getMethodIcon(payment.paymentMethod) }}</mat-icon>
                      {{ payment.paymentMethod }}
                    </div>
                  </td>
                </ng-container>

                <ng-container matColumnDef="referenceNumber">
                  <th mat-header-cell *matHeaderCellDef>Reference</th>
                  <td mat-cell *matCellDef="let payment">{{ payment.referenceNumber || '-' }}</td>
                </ng-container>

                <ng-container matColumnDef="amount">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Amount</th>
                  <td mat-cell *matCellDef="let payment" class="text-right">
                    <span class="amount">{{ payment.amount | currency }}</span>
                  </td>
                </ng-container>

                <ng-container matColumnDef="appliedAmount">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Applied</th>
                  <td mat-cell *matCellDef="let payment" class="text-right">
                    {{ payment.appliedAmount | currency }}
                  </td>
                </ng-container>

                <ng-container matColumnDef="unappliedAmount">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Unapplied</th>
                  <td mat-cell *matCellDef="let payment" class="text-right">
                    <span [class.positive]="(payment.amount - payment.appliedAmount) > 0">
                      {{ (payment.amount - payment.appliedAmount) | currency }}
                    </span>
                  </td>
                </ng-container>

                <ng-container matColumnDef="status">
                  <th mat-header-cell *matHeaderCellDef>Status</th>
                  <td mat-cell *matCellDef="let payment">
                    <mat-chip [class]="getStatusClass(payment.status)">
                      {{ payment.status }}
                    </mat-chip>
                  </td>
                </ng-container>

                <ng-container matColumnDef="actions">
                  <th mat-header-cell *matHeaderCellDef>Actions</th>
                  <td mat-cell *matCellDef="let payment">
                    <button mat-icon-button [matMenuTriggerFor]="menu" (click)="$event.stopPropagation()">
                      <mat-icon>more_vert</mat-icon>
                    </button>
                    <mat-menu #menu="matMenu">
                      <button mat-menu-item (click)="openDialog(payment)">
                        <mat-icon>edit</mat-icon> Edit
                      </button>
                      <button mat-menu-item (click)="viewPayment(payment)">
                        <mat-icon>visibility</mat-icon> View Details
                      </button>
                      @if ((payment.amount - payment.appliedAmount) > 0) {
                        <button mat-menu-item (click)="applyToInvoices(payment)">
                          <mat-icon>assignment</mat-icon> Apply to Invoices
                        </button>
                      }
                      <button mat-menu-item (click)="printReceipt(payment)">
                        <mat-icon>print</mat-icon> Print Receipt
                      </button>
                      @if (payment.status !== 'Voided') {
                        <button mat-menu-item (click)="voidPayment(payment)" class="delete-item">
                          <mat-icon>block</mat-icon> Void
                        </button>
                      }
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

          @if (!loading() && payments().length === 0) {
            <div class="empty-state">
              <mat-icon>payments</mat-icon>
              <h3>No payments found</h3>
              <p>Record payments received from customers</p>
              <button mat-raised-button color="primary" (click)="openDialog()">
                <mat-icon>add</mat-icon>
                Receive Payment
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
      min-width: 200px;
    }

    .table-container {
      overflow-x: auto;
    }

    table {
      width: 100%;
    }

    .payment-number {
      font-family: monospace;
      font-weight: 500;
      color: #1976d2;
    }

    .method-cell {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .method-icon {
      font-size: 18px;
      width: 18px;
      height: 18px;
      color: #666;
    }

    .text-right {
      text-align: right;
    }

    .amount {
      font-weight: 500;
      color: #388e3c;
    }

    .positive {
      color: #f57c00;
    }

    .clickable-row {
      cursor: pointer;
      transition: background-color 0.2s;
    }

    .clickable-row:hover {
      background-color: rgba(0, 0, 0, 0.04);
    }

    .status-completed { background-color: #e8f5e9; color: #388e3c; }
    .status-pending { background-color: #fff3e0; color: #f57c00; }
    .status-voided { background-color: #fafafa; color: #9e9e9e; }
    .status-refunded { background-color: #ffebee; color: #d32f2f; }

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
export class PaymentsComponent implements OnInit {
  private accountingService = inject(AccountingService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  payments = signal<Payment[]>([]);
  loading = signal(false);
  totalRecords = signal(0);

  dataSource = new MatTableDataSource<Payment>();

  displayedColumns = ['paymentNumber', 'customerName', 'paymentDate', 'paymentMethod', 'referenceNumber', 'amount', 'appliedAmount', 'unappliedAmount', 'status', 'actions'];

  searchTerm = '';
  selectedMethod = '';
  fromDate: Date | null = null;
  toDate: Date | null = null;
  pageSize = 25;
  pageNumber = 1;
  sortBy = 'paymentDate';
  sortDescending = true;

  ngOnInit(): void {
    this.loadPayments();
  }

  loadPayments(): void {
    this.loading.set(true);
    const request: AccountingSearchRequest = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      sortDescending: this.sortDescending,
      startDate: this.fromDate || undefined,
      endDate: this.toDate || undefined
    };

    this.accountingService.getPayments(request).subscribe({
      next: (result) => {
        this.payments.set(result.items);
        this.totalRecords.set(result.totalCount);
        this.dataSource.data = result.items;
        this.loading.set(false);
      },
      error: (error) => {
        console.error('Error loading payments:', error);
        this.snackBar.open('Failed to load payments', 'Close', { duration: 3000 });
        this.loading.set(false);
      }
    });
  }

  onSearch(): void {
    this.pageNumber = 1;
    this.loadPayments();
  }

  onSort(sort: Sort): void {
    this.sortBy = sort.active;
    this.sortDescending = sort.direction === 'desc';
    this.loadPayments();
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadPayments();
  }

  getMethodIcon(method: string): string {
    const icons: Record<string, string> = {
      'Cash': 'payments',
      'Check': 'assignment',
      'CreditCard': 'credit_card',
      'BankTransfer': 'account_balance',
      'ACH': 'account_balance',
      'Wire': 'send',
      'Other': 'attach_money'
    };
    return icons[method] || 'payment';
  }

  getStatusClass(status: string): string {
    return `status-${status.toLowerCase()}`;
  }

  openDialog(payment?: Payment): void {
    const dialogRef = this.dialog.open(PaymentDialogComponent, {
      width: '700px',
      maxHeight: '90vh',
      data: { payment }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadPayments();
      }
    });
  }

  viewPayment(payment: Payment): void {
    // TODO: Navigate to payment detail view
  }

  applyToInvoices(payment: Payment): void {
    // TODO: Open apply to invoices dialog
  }

  printReceipt(payment: Payment): void {
    // TODO: Generate receipt PDF
  }

  voidPayment(payment: Payment): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Void Payment',
        message: `Are you sure you want to void payment ${payment.paymentNumber}? This will unapply the payment from all invoices.`,
        confirmText: 'Void',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.accountingService.voidPayment(payment.id, 'Payment voided by user').subscribe({
          next: () => {
            this.snackBar.open('Payment voided successfully', 'Close', { duration: 3000 });
            this.loadPayments();
          },
          error: (error) => {
            console.error('Error voiding payment:', error);
            this.snackBar.open('Failed to void payment', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }
}

import { Component, OnInit, inject, signal, ViewChild, computed } from '@angular/core';
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
import { Invoice, AccountingSearchRequest, ApprovalStatus, PaymentStatus } from '@core/models/accounting.model';
import { InvoiceDialogComponent } from './invoice-dialog.component';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-invoices',
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
              <span>Invoices</span>
              <button mat-raised-button color="primary" (click)="openDialog()">
                <mat-icon>add</mat-icon>
                Create Invoice
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
                     placeholder="Invoice number, customer...">
              <mat-icon matSuffix>search</mat-icon>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Status</mat-label>
              <mat-select [(ngModel)]="selectedStatus" (ngModelChange)="onSearch()">
                <mat-option value="">All Statuses</mat-option>
                <mat-option value="Draft">Draft</mat-option>
                <mat-option value="Pending">Pending</mat-option>
                <mat-option value="Sent">Sent</mat-option>
                <mat-option value="PartiallyPaid">Partially Paid</mat-option>
                <mat-option value="Paid">Paid</mat-option>
                <mat-option value="Overdue">Overdue</mat-option>
                <mat-option value="Cancelled">Cancelled</mat-option>
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

          <!-- Summary Cards -->
          <div class="summary-cards">
            <div class="summary-card">
              <div class="summary-label">Total Outstanding</div>
              <div class="summary-value">{{ summaryData().totalOutstanding | currency }}</div>
            </div>
            <div class="summary-card">
              <div class="summary-label">Overdue</div>
              <div class="summary-value overdue">{{ summaryData().totalOverdue | currency }}</div>
            </div>
            <div class="summary-card">
              <div class="summary-label">Draft</div>
              <div class="summary-value">{{ summaryData().totalDraft | currency }}</div>
            </div>
            <div class="summary-card">
              <div class="summary-label">Paid This Month</div>
              <div class="summary-value paid">{{ summaryData().paidThisMonth | currency }}</div>
            </div>
          </div>

          @if (loading()) {
            <div class="loading-container">
              <mat-spinner diameter="40"></mat-spinner>
            </div>
          }

          @if (!loading()) {
            <div class="table-container">
              <table mat-table [dataSource]="dataSource" matSort (matSortChange)="onSort($event)">
                <ng-container matColumnDef="invoiceNumber">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Invoice #</th>
                  <td mat-cell *matCellDef="let invoice">
                    <span class="invoice-number">{{ invoice.invoiceNumber }}</span>
                  </td>
                </ng-container>

                <ng-container matColumnDef="customerName">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Customer</th>
                  <td mat-cell *matCellDef="let invoice">{{ invoice.customerName }}</td>
                </ng-container>

                <ng-container matColumnDef="invoiceDate">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Date</th>
                  <td mat-cell *matCellDef="let invoice">{{ invoice.invoiceDate | date:'shortDate' }}</td>
                </ng-container>

                <ng-container matColumnDef="dueDate">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Due Date</th>
                  <td mat-cell *matCellDef="let invoice">
                    <span [class.overdue-text]="isOverdue(invoice)">
                      {{ invoice.dueDate | date:'shortDate' }}
                    </span>
                  </td>
                </ng-container>

                <ng-container matColumnDef="totalAmount">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Total</th>
                  <td mat-cell *matCellDef="let invoice" class="text-right">
                    {{ invoice.totalAmount | currency }}
                  </td>
                </ng-container>

                <ng-container matColumnDef="paidAmount">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Paid</th>
                  <td mat-cell *matCellDef="let invoice" class="text-right">
                    {{ invoice.paidAmount | currency }}
                  </td>
                </ng-container>

                <ng-container matColumnDef="balanceDue">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Balance</th>
                  <td mat-cell *matCellDef="let invoice" class="text-right">
                    <span [class.positive]="(invoice.totalAmount - invoice.paidAmount) > 0">
                      {{ (invoice.totalAmount - invoice.paidAmount) | currency }}
                    </span>
                  </td>
                </ng-container>

                <ng-container matColumnDef="status">
                  <th mat-header-cell *matHeaderCellDef>Status</th>
                  <td mat-cell *matCellDef="let invoice">
                    <mat-chip [class]="getStatusClass(invoice.status)">
                      {{ invoice.status }}
                    </mat-chip>
                  </td>
                </ng-container>

                <ng-container matColumnDef="actions">
                  <th mat-header-cell *matHeaderCellDef>Actions</th>
                  <td mat-cell *matCellDef="let invoice">
                    <button mat-icon-button [matMenuTriggerFor]="menu" (click)="$event.stopPropagation()">
                      <mat-icon>more_vert</mat-icon>
                    </button>
                    <mat-menu #menu="matMenu">
                      <button mat-menu-item (click)="openDialog(invoice)">
                        <mat-icon>edit</mat-icon> Edit
                      </button>
                      <button mat-menu-item (click)="viewInvoice(invoice)">
                        <mat-icon>visibility</mat-icon> View
                      </button>
                      @if (invoice.status === 'Draft') {
                        <button mat-menu-item (click)="sendInvoice(invoice)">
                          <mat-icon>send</mat-icon> Send
                        </button>
                      }
                      @if (invoice.status !== 'Paid' && invoice.status !== 'Cancelled') {
                        <button mat-menu-item (click)="recordPayment(invoice)">
                          <mat-icon>payments</mat-icon> Record Payment
                        </button>
                      }
                      <button mat-menu-item (click)="printInvoice(invoice)">
                        <mat-icon>print</mat-icon> Print
                      </button>
                      <button mat-menu-item (click)="duplicateInvoice(invoice)">
                        <mat-icon>content_copy</mat-icon> Duplicate
                      </button>
                      @if (invoice.status !== 'Cancelled') {
                        <button mat-menu-item (click)="createCreditMemo(invoice)">
                          <mat-icon>credit_card_off</mat-icon> Create Credit Memo
                        </button>
                      }
                      @if (invoice.status === 'Draft') {
                        <button mat-menu-item (click)="deleteInvoice(invoice)" class="delete-item">
                          <mat-icon>delete</mat-icon> Delete
                        </button>
                      }
                      @if (invoice.status !== 'Cancelled' && invoice.status !== 'Paid') {
                        <button mat-menu-item (click)="voidInvoice(invoice)" class="delete-item">
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

          @if (!loading() && invoices().length === 0) {
            <div class="empty-state">
              <mat-icon>receipt_long</mat-icon>
              <h3>No invoices found</h3>
              <p>Create your first invoice to start billing customers</p>
              <button mat-raised-button color="primary" (click)="openDialog()">
                <mat-icon>add</mat-icon>
                Create Invoice
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

    .summary-cards {
      display: flex;
      gap: 16px;
      margin-bottom: 24px;
      flex-wrap: wrap;
    }

    .summary-card {
      flex: 1;
      min-width: 150px;
      padding: 16px;
      background: #f5f5f5;
      border-radius: 8px;
    }

    .summary-label {
      font-size: 12px;
      color: #666;
      margin-bottom: 4px;
    }

    .summary-value {
      font-size: 20px;
      font-weight: 500;
    }

    .summary-value.overdue {
      color: #f44336;
    }

    .summary-value.paid {
      color: #388e3c;
    }

    .table-container {
      overflow-x: auto;
    }

    table {
      width: 100%;
    }

    .invoice-number {
      font-family: monospace;
      font-weight: 500;
      color: #1976d2;
    }

    .text-right {
      text-align: right;
    }

    .positive {
      color: #f57c00;
      font-weight: 500;
    }

    .overdue-text {
      color: #f44336;
    }

    .clickable-row {
      cursor: pointer;
      transition: background-color 0.2s;
    }

    .clickable-row:hover {
      background-color: rgba(0, 0, 0, 0.04);
    }

    .status-draft { background-color: #e3f2fd; color: #1976d2; }
    .status-pending { background-color: #fff3e0; color: #f57c00; }
    .status-sent { background-color: #e8f5e9; color: #388e3c; }
    .status-partiallypaid { background-color: #fff8e1; color: #ffa000; }
    .status-paid { background-color: #e8f5e9; color: #2e7d32; }
    .status-overdue { background-color: #ffebee; color: #d32f2f; }
    .status-cancelled { background-color: #fafafa; color: #9e9e9e; }

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
export class InvoicesComponent implements OnInit {
  private accountingService = inject(AccountingService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  invoices = signal<Invoice[]>([]);
  loading = signal(false);
  totalRecords = signal(0);

  summaryData = computed(() => {
    const list = this.invoices();
    const now = new Date();
    return {
      totalOutstanding: list.filter(i => i.paymentStatus !== PaymentStatus.Paid)
                           .reduce((sum, i) => sum + (i.totalAmount - i.paidAmount), 0),
      totalOverdue: list.filter(i => new Date(i.dueDate) < now && i.paymentStatus !== PaymentStatus.Paid)
                       .reduce((sum, i) => sum + (i.totalAmount - i.paidAmount), 0),
      totalDraft: list.filter(i => i.status === ApprovalStatus.Draft).reduce((sum, i) => sum + i.totalAmount, 0),
      paidThisMonth: list.filter(i => i.paymentStatus === PaymentStatus.Paid).reduce((sum, i) => sum + i.paidAmount, 0)
    };
  });

  dataSource = new MatTableDataSource<Invoice>();

  displayedColumns = ['invoiceNumber', 'customerName', 'invoiceDate', 'dueDate', 'totalAmount', 'paidAmount', 'balanceDue', 'status', 'actions'];

  searchTerm = '';
  selectedStatus = '';
  fromDate: Date | null = null;
  toDate: Date | null = null;
  pageSize = 25;
  pageNumber = 1;
  sortBy = 'invoiceDate';
  sortDescending = true;

  ngOnInit(): void {
    this.loadInvoices();
  }

  loadInvoices(): void {
    this.loading.set(true);
    const request: AccountingSearchRequest = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      sortDescending: this.sortDescending,
      status: this.selectedStatus || undefined,
      startDate: this.fromDate || undefined,
      endDate: this.toDate || undefined
    };

    this.accountingService.getInvoices(request).subscribe({
      next: (result) => {
        this.invoices.set(result.items);
        this.totalRecords.set(result.totalCount);
        this.dataSource.data = result.items;
        this.loading.set(false);
      },
      error: (error) => {
        console.error('Error loading invoices:', error);
        this.snackBar.open('Failed to load invoices', 'Close', { duration: 3000 });
        this.loading.set(false);
      }
    });
  }

  onSearch(): void {
    this.pageNumber = 1;
    this.loadInvoices();
  }

  onSort(sort: Sort): void {
    this.sortBy = sort.active;
    this.sortDescending = sort.direction === 'desc';
    this.loadInvoices();
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadInvoices();
  }

  isOverdue(invoice: Invoice): boolean {
    return new Date(invoice.dueDate) < new Date() && invoice.paymentStatus !== PaymentStatus.Paid;
  }

  getStatusClass(status: string): string {
    return `status-${status.toLowerCase()}`;
  }

  openDialog(invoice?: Invoice): void {
    const dialogRef = this.dialog.open(InvoiceDialogComponent, {
      width: '900px',
      maxHeight: '95vh',
      data: { invoice }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadInvoices();
      }
    });
  }

  viewInvoice(invoice: Invoice): void {
    // TODO: Navigate to invoice detail view
  }

  sendInvoice(invoice: Invoice): void {
    // TODO: Implement sendInvoice when service method is available
    this.snackBar.open('Send invoice feature coming soon', 'Close', { duration: 3000 });
  }

  recordPayment(invoice: Invoice): void {
    // TODO: Open payment dialog
  }

  printInvoice(invoice: Invoice): void {
    // TODO: Generate PDF
  }

  duplicateInvoice(invoice: Invoice): void {
    // TODO: Create duplicate invoice
  }

  createCreditMemo(invoice: Invoice): void {
    // TODO: Navigate to credit memo creation
  }

  deleteInvoice(invoice: Invoice): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Delete Invoice',
        message: `Are you sure you want to delete invoice ${invoice.invoiceNumber}?`,
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.accountingService.deleteInvoice(invoice.id).subscribe({
          next: () => {
            this.snackBar.open('Invoice deleted successfully', 'Close', { duration: 3000 });
            this.loadInvoices();
          },
          error: (error) => {
            console.error('Error deleting invoice:', error);
            this.snackBar.open('Failed to delete invoice', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }

  voidInvoice(invoice: Invoice): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Void Invoice',
        message: `Are you sure you want to void invoice ${invoice.invoiceNumber}? This action cannot be undone.`,
        confirmText: 'Void',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        // TODO: Implement voidInvoice when service method is available
        this.snackBar.open('Void invoice feature coming soon', 'Close', { duration: 3000 });
      }
    });
  }
}

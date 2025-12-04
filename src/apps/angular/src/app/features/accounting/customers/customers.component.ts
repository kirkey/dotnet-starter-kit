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
import { MatSelectModule } from '@angular/material/select';

import { AccountingService } from '@core/services/accounting.service';
import { Customer, AccountingSearchRequest } from '@core/models/accounting.model';
import { CustomerDialogComponent } from './customer-dialog.component';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-customers',
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
    MatSelectModule,
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
              <span>Customers</span>
              <button mat-raised-button color="primary" (click)="openDialog()">
                <mat-icon>add</mat-icon>
                Add Customer
              </button>
            </div>
          </mat-card-title>
        </mat-card-header>

        <mat-card-content>
          <!-- Search -->
          <div class="filters-row">
            <mat-form-field appearance="outline" class="search-field">
              <mat-label>Search Customers</mat-label>
              <input matInput [(ngModel)]="searchTerm" (ngModelChange)="onSearch()"
                     placeholder="Search by name, code, or contact...">
              <mat-icon matSuffix>search</mat-icon>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Status</mat-label>
              <mat-select [(ngModel)]="selectedStatus" (ngModelChange)="onSearch()">
                <mat-option value="">All</mat-option>
                <mat-option value="active">Active</mat-option>
                <mat-option value="inactive">Inactive</mat-option>
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
                <ng-container matColumnDef="customerCode">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Code</th>
                  <td mat-cell *matCellDef="let customer">
                    <span class="customer-code">{{ customer.customerCode }}</span>
                  </td>
                </ng-container>

                <ng-container matColumnDef="name">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Name</th>
                  <td mat-cell *matCellDef="let customer">{{ customer.customerName }}</td>
                </ng-container>

                <ng-container matColumnDef="contactName">
                  <th mat-header-cell *matHeaderCellDef>Contact</th>
                  <td mat-cell *matCellDef="let customer">{{ customer.contactName || '-' }}</td>
                </ng-container>

                <ng-container matColumnDef="email">
                  <th mat-header-cell *matHeaderCellDef>Email</th>
                  <td mat-cell *matCellDef="let customer">
                    @if (customer.email) {
                      <a href="mailto:{{ customer.email }}" class="link">{{ customer.email }}</a>
                    } @else {
                      -
                    }
                  </td>
                </ng-container>

                <ng-container matColumnDef="phone">
                  <th mat-header-cell *matHeaderCellDef>Phone</th>
                  <td mat-cell *matCellDef="let customer">{{ customer.phone || '-' }}</td>
                </ng-container>

                <ng-container matColumnDef="balance">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Balance</th>
                  <td mat-cell *matCellDef="let customer" class="text-right">
                    <span [class.positive]="customer.currentBalance > 0">
                      {{ customer.currentBalance | currency }}
                    </span>
                  </td>
                </ng-container>

                <ng-container matColumnDef="creditLimit">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Credit Limit</th>
                  <td mat-cell *matCellDef="let customer" class="text-right">
                    {{ customer.creditLimit | currency }}
                  </td>
                </ng-container>

                <ng-container matColumnDef="isActive">
                  <th mat-header-cell *matHeaderCellDef>Status</th>
                  <td mat-cell *matCellDef="let customer">
                    <mat-chip [class]="customer.isActive ? 'status-active' : 'status-inactive'">
                      {{ customer.isActive ? 'Active' : 'Inactive' }}
                    </mat-chip>
                  </td>
                </ng-container>

                <ng-container matColumnDef="actions">
                  <th mat-header-cell *matHeaderCellDef>Actions</th>
                  <td mat-cell *matCellDef="let customer">
                    <button mat-icon-button [matMenuTriggerFor]="menu" (click)="$event.stopPropagation()">
                      <mat-icon>more_vert</mat-icon>
                    </button>
                    <mat-menu #menu="matMenu">
                      <button mat-menu-item (click)="openDialog(customer)">
                        <mat-icon>edit</mat-icon> Edit
                      </button>
                      <button mat-menu-item (click)="viewInvoices(customer)">
                        <mat-icon>receipt</mat-icon> View Invoices
                      </button>
                      <button mat-menu-item (click)="viewPayments(customer)">
                        <mat-icon>payments</mat-icon> View Payments
                      </button>
                      <button mat-menu-item (click)="viewStatement(customer)">
                        <mat-icon>description</mat-icon> Customer Statement
                      </button>
                      <button mat-menu-item (click)="createInvoice(customer)">
                        <mat-icon>add_circle</mat-icon> Create Invoice
                      </button>
                      <button mat-menu-item (click)="deleteCustomer(customer)" class="delete-item">
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

          @if (!loading() && customers().length === 0) {
            <div class="empty-state">
              <mat-icon>people</mat-icon>
              <h3>No customers found</h3>
              <p>Add your first customer to start managing receivables</p>
              <button mat-raised-button color="primary" (click)="openDialog()">
                <mat-icon>add</mat-icon>
                Add Customer
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
    }

    .table-container {
      overflow-x: auto;
    }

    table {
      width: 100%;
    }

    .customer-code {
      font-family: monospace;
      font-weight: 500;
    }

    .text-right {
      text-align: right;
    }

    .positive {
      color: #388e3c;
    }

    .link {
      color: #1976d2;
      text-decoration: none;
    }

    .link:hover {
      text-decoration: underline;
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
export class CustomersComponent implements OnInit {
  private accountingService = inject(AccountingService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  customers = signal<Customer[]>([]);
  loading = signal(false);
  totalRecords = signal(0);

  dataSource = new MatTableDataSource<Customer>();

  displayedColumns = ['customerCode', 'name', 'contactName', 'email', 'phone', 'balance', 'creditLimit', 'isActive', 'actions'];

  searchTerm = '';
  selectedStatus = '';
  pageSize = 25;
  pageNumber = 1;
  sortBy = 'name';
  sortDescending = false;

  ngOnInit(): void {
    this.loadCustomers();
  }

  loadCustomers(): void {
    this.loading.set(true);
    const request: AccountingSearchRequest = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      sortDescending: this.sortDescending,
      status: this.selectedStatus || undefined
    };

    this.accountingService.getCustomers(request).subscribe({
      next: (result) => {
        this.customers.set(result.items);
        this.totalRecords.set(result.totalCount);
        this.dataSource.data = result.items;
        this.loading.set(false);
      },
      error: (error) => {
        console.error('Error loading customers:', error);
        this.snackBar.open('Failed to load customers', 'Close', { duration: 3000 });
        this.loading.set(false);
      }
    });
  }

  onSearch(): void {
    this.pageNumber = 1;
    this.loadCustomers();
  }

  onSort(sort: Sort): void {
    this.sortBy = sort.active;
    this.sortDescending = sort.direction === 'desc';
    this.loadCustomers();
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadCustomers();
  }

  openDialog(customer?: Customer): void {
    const dialogRef = this.dialog.open(CustomerDialogComponent, {
      width: '700px',
      maxHeight: '90vh',
      data: { customer }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadCustomers();
      }
    });
  }

  viewInvoices(customer: Customer): void {
    // TODO: Navigate to invoices filtered by customer
  }

  viewPayments(customer: Customer): void {
    // TODO: Navigate to payments filtered by customer
  }

  viewStatement(customer: Customer): void {
    // TODO: Generate customer statement
  }

  createInvoice(customer: Customer): void {
    // TODO: Navigate to create invoice for customer
  }

  deleteCustomer(customer: Customer): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Delete Customer',
        message: `Are you sure you want to delete customer "${customer.customerName}"? This action cannot be undone.`,
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.accountingService.deleteCustomer(customer.id).subscribe({
          next: () => {
            this.snackBar.open('Customer deleted successfully', 'Close', { duration: 3000 });
            this.loadCustomers();
          },
          error: (error) => {
            console.error('Error deleting customer:', error);
            this.snackBar.open('Failed to delete customer', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }
}

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
import { Vendor, AccountingSearchRequest } from '@core/models/accounting.model';
import { VendorDialogComponent } from './vendor-dialog.component';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-vendors',
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
    MatTooltipModule,
    MatSelectModule
  ],
  template: `
    <div class="page-container">
      <mat-card>
        <mat-card-header>
          <mat-card-title>
            <div class="header-row">
              <span>Vendors</span>
              <button mat-raised-button color="primary" (click)="openDialog()">
                <mat-icon>add</mat-icon>
                Add Vendor
              </button>
            </div>
          </mat-card-title>
        </mat-card-header>

        <mat-card-content>
          <!-- Search -->
          <div class="filters-row">
            <mat-form-field appearance="outline" class="search-field">
              <mat-label>Search Vendors</mat-label>
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
                <ng-container matColumnDef="vendorCode">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Code</th>
                  <td mat-cell *matCellDef="let vendor">
                    <span class="vendor-code">{{ vendor.vendorCode }}</span>
                  </td>
                </ng-container>

                <ng-container matColumnDef="name">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Name</th>
                  <td mat-cell *matCellDef="let vendor">{{ vendor.vendorName }}</td>
                </ng-container>

                <ng-container matColumnDef="contactName">
                  <th mat-header-cell *matHeaderCellDef>Contact</th>
                  <td mat-cell *matCellDef="let vendor">{{ vendor.contactName || '-' }}</td>
                </ng-container>

                <ng-container matColumnDef="email">
                  <th mat-header-cell *matHeaderCellDef>Email</th>
                  <td mat-cell *matCellDef="let vendor">
                    @if (vendor.email) {
                      <a href="mailto:{{ vendor.email }}" class="link">{{ vendor.email }}</a>
                    } @else {
                      -
                    }
                  </td>
                </ng-container>

                <ng-container matColumnDef="phone">
                  <th mat-header-cell *matHeaderCellDef>Phone</th>
                  <td mat-cell *matCellDef="let vendor">{{ vendor.phone || '-' }}</td>
                </ng-container>

                <ng-container matColumnDef="balance">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Balance</th>
                  <td mat-cell *matCellDef="let vendor" class="text-right">
                    <span [class.negative]="vendor.balance > 0">
                      {{ vendor.balance | currency }}
                    </span>
                  </td>
                </ng-container>

                <ng-container matColumnDef="isActive">
                  <th mat-header-cell *matHeaderCellDef>Status</th>
                  <td mat-cell *matCellDef="let vendor">
                    <mat-chip [class]="vendor.isActive ? 'status-active' : 'status-inactive'">
                      {{ vendor.isActive ? 'Active' : 'Inactive' }}
                    </mat-chip>
                  </td>
                </ng-container>

                <ng-container matColumnDef="actions">
                  <th mat-header-cell *matHeaderCellDef>Actions</th>
                  <td mat-cell *matCellDef="let vendor">
                    <button mat-icon-button [matMenuTriggerFor]="menu" (click)="$event.stopPropagation()">
                      <mat-icon>more_vert</mat-icon>
                    </button>
                    <mat-menu #menu="matMenu">
                      <button mat-menu-item (click)="openDialog(vendor)">
                        <mat-icon>edit</mat-icon> Edit
                      </button>
                      <button mat-menu-item (click)="viewBills(vendor)">
                        <mat-icon>receipt</mat-icon> View Bills
                      </button>
                      <button mat-menu-item (click)="viewPayments(vendor)">
                        <mat-icon>payments</mat-icon> View Payments
                      </button>
                      <button mat-menu-item (click)="viewStatement(vendor)">
                        <mat-icon>description</mat-icon> Vendor Statement
                      </button>
                      <button mat-menu-item (click)="deleteVendor(vendor)" class="delete-item">
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

          @if (!loading() && vendors().length === 0) {
            <div class="empty-state">
              <mat-icon>business</mat-icon>
              <h3>No vendors found</h3>
              <p>Add your first vendor to start managing payables</p>
              <button mat-raised-button color="primary" (click)="openDialog()">
                <mat-icon>add</mat-icon>
                Add Vendor
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

    .vendor-code {
      font-family: monospace;
      font-weight: 500;
    }

    .text-right {
      text-align: right;
    }

    .negative {
      color: #c62828;
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
export class VendorsComponent implements OnInit {
  private accountingService = inject(AccountingService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  vendors = signal<Vendor[]>([]);
  loading = signal(false);
  totalRecords = signal(0);

  dataSource = new MatTableDataSource<Vendor>();

  displayedColumns = ['vendorCode', 'name', 'contactName', 'email', 'phone', 'balance', 'isActive', 'actions'];

  searchTerm = '';
  selectedStatus = '';
  pageSize = 25;
  pageNumber = 1;
  sortBy = 'name';
  sortDescending = false;

  ngOnInit(): void {
    this.loadVendors();
  }

  loadVendors(): void {
    this.loading.set(true);
    const request: AccountingSearchRequest = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      sortDescending: this.sortDescending,
      status: this.selectedStatus || undefined
    };

    this.accountingService.getVendors(request).subscribe({
      next: (result) => {
        this.vendors.set(result.items);
        this.totalRecords.set(result.totalCount);
        this.dataSource.data = result.items;
        this.loading.set(false);
      },
      error: (error) => {
        console.error('Error loading vendors:', error);
        this.snackBar.open('Failed to load vendors', 'Close', { duration: 3000 });
        this.loading.set(false);
      }
    });
  }

  onSearch(): void {
    this.pageNumber = 1;
    this.loadVendors();
  }

  onSort(sort: Sort): void {
    this.sortBy = sort.active;
    this.sortDescending = sort.direction === 'desc';
    this.loadVendors();
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadVendors();
  }

  openDialog(vendor?: Vendor): void {
    const dialogRef = this.dialog.open(VendorDialogComponent, {
      width: '700px',
      maxHeight: '90vh',
      data: { vendor }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadVendors();
      }
    });
  }

  viewBills(vendor: Vendor): void {
    // TODO: Navigate to bills filtered by vendor
  }

  viewPayments(vendor: Vendor): void {
    // TODO: Navigate to payments filtered by vendor
  }

  viewStatement(vendor: Vendor): void {
    // TODO: Generate vendor statement
  }

  deleteVendor(vendor: Vendor): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Delete Vendor',
        message: `Are you sure you want to delete vendor "${vendor.vendorName}"? This action cannot be undone.`,
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.accountingService.deleteVendor(vendor.id).subscribe({
          next: () => {
            this.snackBar.open('Vendor deleted successfully', 'Close', { duration: 3000 });
            this.loadVendors();
          },
          error: (error) => {
            console.error('Error deleting vendor:', error);
            this.snackBar.open('Failed to delete vendor', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }
}

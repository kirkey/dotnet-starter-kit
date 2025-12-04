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
import { TaxCode, AccountingSearchRequest } from '@core/models/accounting.model';
import { TaxCodeDialogComponent } from './tax-code-dialog.component';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-tax-codes',
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
              <span>Tax Codes</span>
              <button mat-raised-button color="primary" (click)="openDialog()">
                <mat-icon>add</mat-icon>
                Add Tax Code
              </button>
            </div>
          </mat-card-title>
        </mat-card-header>

        <mat-card-content>
          <!-- Search -->
          <div class="filters-row">
            <mat-form-field appearance="outline" class="search-field">
              <mat-label>Search Tax Codes</mat-label>
              <input matInput [(ngModel)]="searchTerm" (ngModelChange)="onSearch()"
                     placeholder="Search by code or name...">
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
                <ng-container matColumnDef="code">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Code</th>
                  <td mat-cell *matCellDef="let taxCode">
                    <span class="tax-code">{{ taxCode.taxCode }}</span>
                  </td>
                </ng-container>

                <ng-container matColumnDef="name">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Name</th>
                  <td mat-cell *matCellDef="let taxCode">{{ taxCode.taxName }}</td>
                </ng-container>

                <ng-container matColumnDef="rate">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Rate</th>
                  <td mat-cell *matCellDef="let taxCode" class="text-right">
                    <span class="rate">{{ taxCode.rate }}%</span>
                  </td>
                </ng-container>

                <ng-container matColumnDef="taxType">
                  <th mat-header-cell *matHeaderCellDef>Type</th>
                  <td mat-cell *matCellDef="let taxCode">{{ taxCode.taxType }}</td>
                </ng-container>

                <ng-container matColumnDef="description">
                  <th mat-header-cell *matHeaderCellDef>Description</th>
                  <td mat-cell *matCellDef="let taxCode">{{ taxCode.description || '-' }}</td>
                </ng-container>

                <ng-container matColumnDef="isActive">
                  <th mat-header-cell *matHeaderCellDef>Status</th>
                  <td mat-cell *matCellDef="let taxCode">
                    <mat-chip [class]="taxCode.isDefault ? 'status-active' : 'status-inactive'">
                      {{ taxCode.isDefault ? 'Default' : 'Active' }}
                    </mat-chip>
                  </td>
                </ng-container>

                <ng-container matColumnDef="actions">
                  <th mat-header-cell *matHeaderCellDef>Actions</th>
                  <td mat-cell *matCellDef="let taxCode">
                    <button mat-icon-button [matMenuTriggerFor]="menu" (click)="$event.stopPropagation()">
                      <mat-icon>more_vert</mat-icon>
                    </button>
                    <mat-menu #menu="matMenu">
                      <button mat-menu-item (click)="openDialog(taxCode)">
                        <mat-icon>edit</mat-icon> Edit
                      </button>
                      <button mat-menu-item (click)="viewUsage(taxCode)">
                        <mat-icon>analytics</mat-icon> View Usage
                      </button>
                      <button mat-menu-item (click)="deleteTaxCode(taxCode)" class="delete-item">
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

          @if (!loading() && taxCodes().length === 0) {
            <div class="empty-state">
              <mat-icon>percent</mat-icon>
              <h3>No tax codes found</h3>
              <p>Configure tax codes for invoices and bills</p>
              <button mat-raised-button color="primary" (click)="openDialog()">
                <mat-icon>add</mat-icon>
                Add Tax Code
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
    }

    .search-field {
      flex: 1;
      max-width: 400px;
    }

    .table-container {
      overflow-x: auto;
    }

    table {
      width: 100%;
    }

    .tax-code {
      font-family: monospace;
      font-weight: 500;
      color: #1976d2;
    }

    .text-right {
      text-align: right;
    }

    .rate {
      font-weight: 500;
      color: #7b1fa2;
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
export class TaxCodesComponent implements OnInit {
  private accountingService = inject(AccountingService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  taxCodes = signal<TaxCode[]>([]);
  loading = signal(false);
  totalRecords = signal(0);

  dataSource = new MatTableDataSource<TaxCode>();

  displayedColumns = ['code', 'name', 'rate', 'taxType', 'description', 'isActive', 'actions'];

  searchTerm = '';
  pageSize = 25;
  pageNumber = 1;
  sortBy = 'code';
  sortDescending = false;

  ngOnInit(): void {
    this.loadTaxCodes();
  }

  loadTaxCodes(): void {
    this.loading.set(true);
    const request: AccountingSearchRequest = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      sortDescending: this.sortDescending
    };

    this.accountingService.getTaxCodes(request).subscribe({
      next: (result) => {
        this.taxCodes.set(result.items);
        this.totalRecords.set(result.totalCount);
        this.dataSource.data = result.items;
        this.loading.set(false);
      },
      error: (error) => {
        console.error('Error loading tax codes:', error);
        this.snackBar.open('Failed to load tax codes', 'Close', { duration: 3000 });
        this.loading.set(false);
      }
    });
  }

  onSearch(): void {
    this.pageNumber = 1;
    this.loadTaxCodes();
  }

  onSort(sort: Sort): void {
    this.sortBy = sort.active;
    this.sortDescending = sort.direction === 'desc';
    this.loadTaxCodes();
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadTaxCodes();
  }

  openDialog(taxCode?: TaxCode): void {
    const dialogRef = this.dialog.open(TaxCodeDialogComponent, {
      width: '500px',
      data: { taxCode }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadTaxCodes();
      }
    });
  }

  viewUsage(taxCode: TaxCode): void {
    // TODO: Show tax code usage report
  }

  deleteTaxCode(taxCode: TaxCode): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Delete Tax Code',
        message: `Are you sure you want to delete tax code "${taxCode.taxCode}"? This action cannot be undone.`,
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.accountingService.deleteTaxCode(taxCode.id).subscribe({
          next: () => {
            this.snackBar.open('Tax code deleted successfully', 'Close', { duration: 3000 });
            this.loadTaxCodes();
          },
          error: (error) => {
            console.error('Error deleting tax code:', error);
            this.snackBar.open('Failed to delete tax code', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }
}

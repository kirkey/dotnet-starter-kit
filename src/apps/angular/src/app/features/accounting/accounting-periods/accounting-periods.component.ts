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
import { MatSelectModule } from '@angular/material/select';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';

import { AccountingService } from '@core/services/accounting.service';
import { AccountingPeriod, PeriodStatus, AccountingSearchRequest } from '@core/models/accounting.model';
import { AccountingPeriodDialogComponent } from './accounting-period-dialog.component';
import { ClosePeriodDialogComponent } from './close-period-dialog.component';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-accounting-periods',
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
    MatSelectModule,
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
              <span>Accounting Periods</span>
              <button mat-raised-button color="primary" (click)="openDialog()">
                <mat-icon>add</mat-icon>
                Create Period
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
                     placeholder="Search by name...">
              <mat-icon matSuffix>search</mat-icon>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Fiscal Year</mat-label>
              <mat-select [(ngModel)]="selectedYear" (ngModelChange)="onSearch()">
                <mat-option value="">All Years</mat-option>
                @for (year of fiscalYears(); track year) {
                  <mat-option [value]="year">{{ year }}</mat-option>
                }
              </mat-select>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Status</mat-label>
              <mat-select [(ngModel)]="selectedStatus" (ngModelChange)="onSearch()">
                <mat-option value="">All Status</mat-option>
                @for (status of statuses; track status) {
                  <mat-option [value]="status">{{ status }}</mat-option>
                }
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
                <ng-container matColumnDef="name">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Period Name</th>
                  <td mat-cell *matCellDef="let period">
                    <div class="period-name">
                      {{ period.periodName }}
                      @if (period.isCurrent) {
                        <mat-chip class="current-chip">Current</mat-chip>
                      }
                    </div>
                  </td>
                </ng-container>

                <ng-container matColumnDef="fiscalYear">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Fiscal Year</th>
                  <td mat-cell *matCellDef="let period">{{ period.fiscalYear }}</td>
                </ng-container>

                <ng-container matColumnDef="periodNumber">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Period #</th>
                  <td mat-cell *matCellDef="let period">{{ period.periodNumber }}</td>
                </ng-container>

                <ng-container matColumnDef="startDate">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Start Date</th>
                  <td mat-cell *matCellDef="let period">{{ period.startDate | date:'mediumDate' }}</td>
                </ng-container>

                <ng-container matColumnDef="endDate">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>End Date</th>
                  <td mat-cell *matCellDef="let period">{{ period.endDate | date:'mediumDate' }}</td>
                </ng-container>

                <ng-container matColumnDef="status">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Status</th>
                  <td mat-cell *matCellDef="let period">
                    <mat-chip [class]="'status-chip ' + period.status.toLowerCase()">
                      {{ period.status }}
                    </mat-chip>
                  </td>
                </ng-container>

                <ng-container matColumnDef="closedDate">
                  <th mat-header-cell *matHeaderCellDef>Closed Date</th>
                  <td mat-cell *matCellDef="let period">
                    {{ period.closedDate ? (period.closedDate | date:'mediumDate') : '-' }}
                  </td>
                </ng-container>

                <ng-container matColumnDef="actions">
                  <th mat-header-cell *matHeaderCellDef>Actions</th>
                  <td mat-cell *matCellDef="let period">
                    <button mat-icon-button [matMenuTriggerFor]="menu" (click)="$event.stopPropagation()">
                      <mat-icon>more_vert</mat-icon>
                    </button>
                    <mat-menu #menu="matMenu">
                      <button mat-menu-item (click)="openDialog(period)">
                        <mat-icon>edit</mat-icon> Edit
                      </button>
                      @if (period.status === 'Open') {
                        <button mat-menu-item (click)="openCloseDialog(period)">
                          <mat-icon>lock</mat-icon> Close Period
                        </button>
                        <button mat-menu-item (click)="setAsCurrent(period)">
                          <mat-icon>star</mat-icon> Set as Current
                        </button>
                      }
                      @if (period.status === 'Closed') {
                        <button mat-menu-item (click)="reopenPeriod(period)">
                          <mat-icon>lock_open</mat-icon> Reopen Period
                        </button>
                      }
                      <button mat-menu-item (click)="viewTransactions(period)">
                        <mat-icon>receipt_long</mat-icon> View Transactions
                      </button>
                    </mat-menu>
                  </td>
                </ng-container>

                <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
                <tr mat-row *matRowDef="let row; columns: displayedColumns;"
                    [class.current-row]="row.isCurrent"></tr>
              </table>

              <mat-paginator
                [length]="totalRecords()"
                [pageSize]="pageSize"
                [pageSizeOptions]="[10, 25, 50]"
                (page)="onPageChange($event)"
                showFirstLastButtons>
              </mat-paginator>
            </div>
          }

          @if (!loading() && periods().length === 0) {
            <div class="empty-state">
              <mat-icon>date_range</mat-icon>
              <h3>No accounting periods found</h3>
              <p>Create your first accounting period to get started</p>
              <button mat-raised-button color="primary" (click)="openDialog()">
                <mat-icon>add</mat-icon>
                Create Period
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

    .period-name {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .current-chip {
      font-size: 10px;
      background-color: #e3f2fd;
      color: #1976d2;
    }

    .current-row {
      background-color: #f5f5f5;
    }

    .status-chip {
      font-size: 12px;
    }

    .status-chip.open {
      background-color: #e8f5e9;
      color: #388e3c;
    }

    .status-chip.closed {
      background-color: #e0e0e0;
      color: #616161;
    }

    .status-chip.future {
      background-color: #e3f2fd;
      color: #1976d2;
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
      margin: 0 0 24px;
    }
  `]
})
export class AccountingPeriodsComponent implements OnInit {
  private accountingService = inject(AccountingService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  periods = signal<AccountingPeriod[]>([]);
  fiscalYears = signal<number[]>([]);
  loading = signal(false);
  totalRecords = signal(0);

  dataSource = new MatTableDataSource<AccountingPeriod>();

  displayedColumns = ['name', 'fiscalYear', 'periodNumber', 'startDate', 'endDate', 'status', 'closedDate', 'actions'];
  statuses = Object.values(PeriodStatus);

  searchTerm = '';
  selectedYear = '';
  selectedStatus = '';
  pageSize = 25;
  pageNumber = 1;
  sortBy = 'startDate';
  sortDescending = true;

  ngOnInit(): void {
    this.loadPeriods();
    this.loadFiscalYears();
  }

  loadPeriods(): void {
    this.loading.set(true);
    const request: AccountingSearchRequest = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      sortDescending: this.sortDescending,
      status: this.selectedStatus || undefined
    };

    this.accountingService.getAccountingPeriods(request).subscribe({
      next: (result) => {
        let filteredItems = result.items;
        if (this.selectedYear) {
          filteredItems = filteredItems.filter(p => p.fiscalYear === parseInt(this.selectedYear));
        }
        this.periods.set(filteredItems);
        this.totalRecords.set(result.totalCount);
        this.dataSource.data = filteredItems;
        this.loading.set(false);
      },
      error: (error) => {
        console.error('Error loading periods:', error);
        this.snackBar.open('Failed to load accounting periods', 'Close', { duration: 3000 });
        this.loading.set(false);
      }
    });
  }

  loadFiscalYears(): void {
    // Generate fiscal years for filter
    const currentYear = new Date().getFullYear();
    const years = [];
    for (let i = currentYear - 5; i <= currentYear + 2; i++) {
      years.push(i);
    }
    this.fiscalYears.set(years);
  }

  onSearch(): void {
    this.pageNumber = 1;
    this.loadPeriods();
  }

  onSort(sort: Sort): void {
    this.sortBy = sort.active;
    this.sortDescending = sort.direction === 'desc';
    this.loadPeriods();
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadPeriods();
  }

  openDialog(period?: AccountingPeriod): void {
    const dialogRef = this.dialog.open(AccountingPeriodDialogComponent, {
      width: '500px',
      data: { period }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadPeriods();
      }
    });
  }

  openCloseDialog(period: AccountingPeriod): void {
    const dialogRef = this.dialog.open(ClosePeriodDialogComponent, {
      width: '500px',
      data: { period }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadPeriods();
      }
    });
  }

  setAsCurrent(period: AccountingPeriod): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Set as Current Period',
        message: `Are you sure you want to set "${period.periodName}" as the current period?`,
        confirmText: 'Set as Current',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        // TODO: Implement set as current API
        this.snackBar.open('Period set as current', 'Close', { duration: 3000 });
        this.loadPeriods();
      }
    });
  }

  reopenPeriod(period: AccountingPeriod): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Reopen Period',
        message: `Are you sure you want to reopen "${period.periodName}"? This will allow new transactions to be posted to this period.`,
        confirmText: 'Reopen',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.accountingService.reopenPeriod({
          periodId: period.id,
          reason: 'Reopened for additional entries'
        }).subscribe({
          next: () => {
            this.snackBar.open('Period reopened', 'Close', { duration: 3000 });
            this.loadPeriods();
          },
          error: (error) => {
            console.error('Error reopening period:', error);
            this.snackBar.open('Failed to reopen period', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }

  viewTransactions(period: AccountingPeriod): void {
    // TODO: Navigate to journal entries filtered by period
  }
}

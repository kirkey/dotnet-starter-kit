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
import { Budget, AccountingSearchRequest } from '@core/models/accounting.model';
import { BudgetDialogComponent } from './budget-dialog.component';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-budgets',
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
              <span>Budgets</span>
              <button mat-raised-button color="primary" (click)="openDialog()">
                <mat-icon>add</mat-icon>
                Create Budget
              </button>
            </div>
          </mat-card-title>
        </mat-card-header>

        <mat-card-content>
          <!-- Filters -->
          <div class="filters-row">
            <mat-form-field appearance="outline" class="search-field">
              <mat-label>Search Budgets</mat-label>
              <input matInput [(ngModel)]="searchTerm" (ngModelChange)="onSearch()"
                     placeholder="Search by name...">
              <mat-icon matSuffix>search</mat-icon>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Fiscal Year</mat-label>
              <mat-select [(ngModel)]="selectedYear" (ngModelChange)="onSearch()">
                <mat-option value="">All Years</mat-option>
                @for (year of fiscalYears; track year) {
                  <mat-option [value]="year">{{ year }}</mat-option>
                }
              </mat-select>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Status</mat-label>
              <mat-select [(ngModel)]="selectedStatus" (ngModelChange)="onSearch()">
                <mat-option value="">All</mat-option>
                <mat-option value="Draft">Draft</mat-option>
                <mat-option value="Active">Active</mat-option>
                <mat-option value="Closed">Closed</mat-option>
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
                <ng-container matColumnDef="budgetName">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Budget Name</th>
                  <td mat-cell *matCellDef="let budget">
                    <div class="budget-name">
                      <mat-icon class="budget-icon">assessment</mat-icon>
                      {{ budget.budgetName }}
                    </div>
                  </td>
                </ng-container>

                <ng-container matColumnDef="fiscalYear">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Fiscal Year</th>
                  <td mat-cell *matCellDef="let budget">{{ budget.fiscalYear }}</td>
                </ng-container>

                <ng-container matColumnDef="startDate">
                  <th mat-header-cell *matHeaderCellDef>Start Date</th>
                  <td mat-cell *matCellDef="let budget">{{ budget.startDate | date:'shortDate' }}</td>
                </ng-container>

                <ng-container matColumnDef="endDate">
                  <th mat-header-cell *matHeaderCellDef>End Date</th>
                  <td mat-cell *matCellDef="let budget">{{ budget.endDate | date:'shortDate' }}</td>
                </ng-container>

                <ng-container matColumnDef="totalBudgetedAmount">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Total Budget</th>
                  <td mat-cell *matCellDef="let budget" class="text-right">
                    {{ budget.totalBudgetedAmount | currency }}
                  </td>
                </ng-container>

                <ng-container matColumnDef="totalActualAmount">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Actual</th>
                  <td mat-cell *matCellDef="let budget" class="text-right">
                    {{ budget.totalActualAmount | currency }}
                  </td>
                </ng-container>

                <ng-container matColumnDef="variance">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Variance</th>
                  <td mat-cell *matCellDef="let budget" class="text-right">
                    <span [class]="getVarianceClass(budget)">
                      {{ budget.variance | currency }}
                    </span>
                  </td>
                </ng-container>

                <ng-container matColumnDef="status">
                  <th mat-header-cell *matHeaderCellDef>Status</th>
                  <td mat-cell *matCellDef="let budget">
                    <mat-chip [class]="getStatusClass(budget.status)">
                      {{ budget.status }}
                    </mat-chip>
                  </td>
                </ng-container>

                <ng-container matColumnDef="actions">
                  <th mat-header-cell *matHeaderCellDef>Actions</th>
                  <td mat-cell *matCellDef="let budget">
                    <button mat-icon-button [matMenuTriggerFor]="menu" (click)="$event.stopPropagation()">
                      <mat-icon>more_vert</mat-icon>
                    </button>
                    <mat-menu #menu="matMenu">
                      <button mat-menu-item (click)="openDialog(budget)">
                        <mat-icon>edit</mat-icon> Edit
                      </button>
                      <button mat-menu-item (click)="viewDetails(budget)">
                        <mat-icon>visibility</mat-icon> View Details
                      </button>
                      <button mat-menu-item (click)="viewVarianceReport(budget)">
                        <mat-icon>analytics</mat-icon> Variance Report
                      </button>
                      <button mat-menu-item (click)="copyBudget(budget)">
                        <mat-icon>content_copy</mat-icon> Copy Budget
                      </button>
                      @if (budget.status === 'Draft') {
                        <button mat-menu-item (click)="activateBudget(budget)">
                          <mat-icon>check_circle</mat-icon> Activate
                        </button>
                      }
                      @if (budget.status !== 'Closed') {
                        <button mat-menu-item (click)="closeBudget(budget)">
                          <mat-icon>lock</mat-icon> Close
                        </button>
                      }
                      <button mat-menu-item (click)="deleteBudget(budget)" class="delete-item">
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

          @if (!loading() && budgets().length === 0) {
            <div class="empty-state">
              <mat-icon>assessment</mat-icon>
              <h3>No budgets found</h3>
              <p>Create your first budget to start financial planning</p>
              <button mat-raised-button color="primary" (click)="openDialog()">
                <mat-icon>add</mat-icon>
                Create Budget
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

    .budget-name {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .budget-icon {
      color: #7b1fa2;
    }

    .text-right {
      text-align: right;
    }

    .positive {
      color: #388e3c;
    }

    .negative {
      color: #d32f2f;
    }

    .clickable-row {
      cursor: pointer;
      transition: background-color 0.2s;
    }

    .clickable-row:hover {
      background-color: rgba(0, 0, 0, 0.04);
    }

    .status-draft { background-color: #e3f2fd; color: #1976d2; }
    .status-active { background-color: #e8f5e9; color: #388e3c; }
    .status-closed { background-color: #fafafa; color: #9e9e9e; }

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
export class BudgetsComponent implements OnInit {
  private accountingService = inject(AccountingService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  budgets = signal<Budget[]>([]);
  loading = signal(false);
  totalRecords = signal(0);

  dataSource = new MatTableDataSource<Budget>();

  displayedColumns = ['budgetName', 'fiscalYear', 'startDate', 'endDate', 'totalBudgetedAmount', 'totalActualAmount', 'variance', 'status', 'actions'];

  searchTerm = '';
  selectedYear = '';
  selectedStatus = '';
  pageSize = 25;
  pageNumber = 1;
  sortBy = 'fiscalYear';
  sortDescending = true;

  fiscalYears: number[] = [];

  ngOnInit(): void {
    this.initFiscalYears();
    this.loadBudgets();
  }

  private initFiscalYears(): void {
    const currentYear = new Date().getFullYear();
    for (let i = currentYear + 1; i >= currentYear - 5; i--) {
      this.fiscalYears.push(i);
    }
  }

  loadBudgets(): void {
    this.loading.set(true);
    const request: AccountingSearchRequest = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      sortDescending: this.sortDescending,
      status: this.selectedStatus || undefined
    };

    this.accountingService.getBudgets(request).subscribe({
      next: (result) => {
        this.budgets.set(result.items);
        this.totalRecords.set(result.totalCount);
        this.dataSource.data = result.items;
        this.loading.set(false);
      },
      error: (error) => {
        console.error('Error loading budgets:', error);
        this.snackBar.open('Failed to load budgets', 'Close', { duration: 3000 });
        this.loading.set(false);
      }
    });
  }

  onSearch(): void {
    this.pageNumber = 1;
    this.loadBudgets();
  }

  onSort(sort: Sort): void {
    this.sortBy = sort.active;
    this.sortDescending = sort.direction === 'desc';
    this.loadBudgets();
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadBudgets();
  }

  getVarianceClass(budget: Budget): string {
    return budget.variance >= 0 ? 'positive' : 'negative';
  }

  getStatusClass(status: string): string {
    return `status-${status.toLowerCase()}`;
  }

  openDialog(budget?: Budget): void {
    const dialogRef = this.dialog.open(BudgetDialogComponent, {
      width: '900px',
      maxHeight: '90vh',
      data: { budget }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadBudgets();
      }
    });
  }

  viewDetails(budget: Budget): void {
    // TODO: Navigate to budget details
  }

  viewVarianceReport(budget: Budget): void {
    // TODO: Generate variance report
  }

  copyBudget(budget: Budget): void {
    // TODO: Copy budget
  }

  activateBudget(budget: Budget): void {
    this.accountingService.activateBudget(budget.id).subscribe({
      next: () => {
        this.snackBar.open('Budget activated successfully', 'Close', { duration: 3000 });
        this.loadBudgets();
      },
      error: (error) => {
        console.error('Error activating budget:', error);
        this.snackBar.open('Failed to activate budget', 'Close', { duration: 3000 });
      }
    });
  }

  closeBudget(budget: Budget): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Close Budget',
        message: `Are you sure you want to close "${budget.budgetName}"? This action cannot be undone.`,
        confirmText: 'Close Budget',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.accountingService.closeBudget(budget.id).subscribe({
          next: () => {
            this.snackBar.open('Budget closed successfully', 'Close', { duration: 3000 });
            this.loadBudgets();
          },
          error: (error) => {
            console.error('Error closing budget:', error);
            this.snackBar.open('Failed to close budget', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }

  deleteBudget(budget: Budget): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Delete Budget',
        message: `Are you sure you want to delete "${budget.budgetName}"? This action cannot be undone.`,
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.accountingService.deleteBudget(budget.id).subscribe({
          next: () => {
            this.snackBar.open('Budget deleted successfully', 'Close', { duration: 3000 });
            this.loadBudgets();
          },
          error: (error) => {
            console.error('Error deleting budget:', error);
            this.snackBar.open('Failed to delete budget', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }
}

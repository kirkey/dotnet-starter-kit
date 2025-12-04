import { Component, OnInit, inject, signal, computed, ViewChild } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSortModule, MatSort, Sort } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule, MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatMenuModule } from '@angular/material/menu';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { AccountingService } from '@core/services/accounting.service';
import { Payee, ChartOfAccount } from '@core/models/accounting.model';

@Component({
  selector: 'app-payees',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatCardModule,
    MatDialogModule,
    MatTooltipModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatSnackBarModule,
    MatMenuModule,
    MatDividerModule,
    MatProgressBarModule,
    DatePipe,
    CurrencyPipe
  ],
  template: `
    <div class="payees-container">
      <!-- Header -->
      <div class="page-header">
        <div class="header-content">
          <h1>Payees</h1>
          <p class="subtitle">Manage payee master data for payments and checks</p>
        </div>
        <div class="header-actions">
          <button mat-stroked-button (click)="toggleView()">
            <mat-icon>{{ isGridView() ? 'view_list' : 'grid_view' }}</mat-icon>
            {{ isGridView() ? 'List View' : 'Grid View' }}
          </button>
          <button mat-raised-button color="primary" (click)="openDialog()">
            <mat-icon>add</mat-icon>
            New Payee
          </button>
        </div>
      </div>

      <!-- Summary Cards -->
      <div class="summary-cards">
        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon individuals">
              <mat-icon>person</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-value">{{ individualCount() }}</span>
              <span class="summary-label">Individuals</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon companies">
              <mat-icon>business</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-value">{{ companyCount() }}</span>
              <span class="summary-label">Companies</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon government">
              <mat-icon>account_balance</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-value">{{ governmentCount() }}</span>
              <span class="summary-label">Government</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon total">
              <mat-icon>group</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-value">{{ totalCount() }}</span>
              <span class="summary-label">Total Payees</span>
            </div>
          </mat-card-content>
        </mat-card>
      </div>

      <!-- Filters -->
      <mat-card class="filter-card">
        <mat-card-content>
          <div class="filters">
            <mat-form-field appearance="outline" class="search-field">
              <mat-label>Search payees</mat-label>
              <mat-icon matPrefix>search</mat-icon>
              <input matInput [formControl]="searchControl" placeholder="Search by name, code, or email...">
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Payee Type</mat-label>
              <mat-select [formControl]="typeFilterControl">
                <mat-option value="">All Types</mat-option>
                <mat-option value="Individual">Individual</mat-option>
                <mat-option value="Company">Company</mat-option>
                <mat-option value="Government">Government</mat-option>
                <mat-option value="Other">Other</mat-option>
              </mat-select>
            </mat-form-field>

            <button mat-icon-button (click)="loadPayees()" matTooltip="Refresh">
              <mat-icon>refresh</mat-icon>
            </button>
          </div>
        </mat-card-content>
      </mat-card>

      <!-- Loading -->
      @if (isLoading()) {
        <div class="loading-container">
          <mat-spinner diameter="40"></mat-spinner>
          <p>Loading payees...</p>
        </div>
      }

      <!-- Grid View -->
      @if (!isLoading() && isGridView()) {
        <div class="payee-grid">
          @for (payee of payees(); track payee.id) {
            <mat-card class="payee-card" [class.inactive]="!payee.isActive">
              <mat-card-header>
                <div mat-card-avatar class="payee-avatar">
                  <mat-icon>{{ getPayeeTypeIcon(payee.payeeType) }}</mat-icon>
                </div>
                <mat-card-title>{{ payee.payeeName }}</mat-card-title>
                <mat-card-subtitle>{{ payee.payeeCode }}</mat-card-subtitle>
                <button mat-icon-button [matMenuTriggerFor]="payeeMenu" class="card-menu-btn">
                  <mat-icon>more_vert</mat-icon>
                </button>
                <mat-menu #payeeMenu="matMenu">
                  <button mat-menu-item (click)="openDialog(payee)">
                    <mat-icon>edit</mat-icon>
                    <span>Edit</span>
                  </button>
                  <button mat-menu-item (click)="viewPayee(payee)">
                    <mat-icon>visibility</mat-icon>
                    <span>View Details</span>
                  </button>
                  <mat-divider></mat-divider>
                  <button mat-menu-item (click)="deletePayee(payee)" class="delete-action">
                    <mat-icon>delete</mat-icon>
                    <span>Delete</span>
                  </button>
                </mat-menu>
              </mat-card-header>
              <mat-card-content>
                <div class="payee-details">
                  <div class="detail-row">
                    <mat-icon>category</mat-icon>
                    <span>{{ payee.payeeType }}</span>
                  </div>
                  @if (payee.email) {
                    <div class="detail-row">
                      <mat-icon>email</mat-icon>
                      <span>{{ payee.email }}</span>
                    </div>
                  }
                  @if (payee.phone) {
                    <div class="detail-row">
                      <mat-icon>phone</mat-icon>
                      <span>{{ payee.phone }}</span>
                    </div>
                  }
                  @if (payee.city && payee.state) {
                    <div class="detail-row">
                      <mat-icon>location_on</mat-icon>
                      <span>{{ payee.city }}, {{ payee.state }}</span>
                    </div>
                  }
                  @if (payee.defaultExpenseAccountCode) {
                    <div class="detail-row">
                      <mat-icon>account_balance_wallet</mat-icon>
                      <span>{{ payee.defaultExpenseAccountCode }}</span>
                    </div>
                  }
                </div>
                <div class="payee-status">
                  <mat-chip [class]="payee.isActive ? 'status-active' : 'status-inactive'">
                    {{ payee.isActive ? 'Active' : 'Inactive' }}
                  </mat-chip>
                </div>
              </mat-card-content>
            </mat-card>
          } @empty {
            <div class="empty-state">
              <mat-icon>group_off</mat-icon>
              <h3>No Payees Found</h3>
              <p>Create your first payee to get started</p>
              <button mat-raised-button color="primary" (click)="openDialog()">
                <mat-icon>add</mat-icon>
                Add Payee
              </button>
            </div>
          }
        </div>
      }

      <!-- Table View -->
      @if (!isLoading() && !isGridView()) {
        <mat-card class="table-card">
          <table mat-table [dataSource]="dataSource" matSort (matSortChange)="onSortChange($event)">
            <!-- Code Column -->
            <ng-container matColumnDef="payeeCode">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Code</th>
              <td mat-cell *matCellDef="let payee">
                <span class="code-badge">{{ payee.payeeCode }}</span>
              </td>
            </ng-container>

            <!-- Name Column -->
            <ng-container matColumnDef="payeeName">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Name</th>
              <td mat-cell *matCellDef="let payee">
                <div class="payee-name-cell">
                  <mat-icon class="type-icon">{{ getPayeeTypeIcon(payee.payeeType) }}</mat-icon>
                  <span>{{ payee.payeeName }}</span>
                </div>
              </td>
            </ng-container>

            <!-- Type Column -->
            <ng-container matColumnDef="payeeType">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Type</th>
              <td mat-cell *matCellDef="let payee">
                <mat-chip class="type-chip">{{ payee.payeeType }}</mat-chip>
              </td>
            </ng-container>

            <!-- Contact Column -->
            <ng-container matColumnDef="contact">
              <th mat-header-cell *matHeaderCellDef>Contact</th>
              <td mat-cell *matCellDef="let payee">
                <div class="contact-info">
                  @if (payee.email) {
                    <span class="email">{{ payee.email }}</span>
                  }
                  @if (payee.phone) {
                    <span class="phone">{{ payee.phone }}</span>
                  }
                </div>
              </td>
            </ng-container>

            <!-- Location Column -->
            <ng-container matColumnDef="location">
              <th mat-header-cell *matHeaderCellDef>Location</th>
              <td mat-cell *matCellDef="let payee">
                @if (payee.city || payee.state) {
                  <span>{{ payee.city ? payee.city + ', ' : '' }}{{ payee.state || '' }}</span>
                } @else {
                  <span class="no-data">—</span>
                }
              </td>
            </ng-container>

            <!-- Default Account Column -->
            <ng-container matColumnDef="defaultExpenseAccountCode">
              <th mat-header-cell *matHeaderCellDef>Default Account</th>
              <td mat-cell *matCellDef="let payee">
                @if (payee.defaultExpenseAccountCode) {
                  <span class="account-code">{{ payee.defaultExpenseAccountCode }}</span>
                } @else {
                  <span class="no-data">—</span>
                }
              </td>
            </ng-container>

            <!-- Status Column -->
            <ng-container matColumnDef="isActive">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Status</th>
              <td mat-cell *matCellDef="let payee">
                <mat-chip [class]="payee.isActive ? 'status-active' : 'status-inactive'">
                  {{ payee.isActive ? 'Active' : 'Inactive' }}
                </mat-chip>
              </td>
            </ng-container>

            <!-- Actions Column -->
            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef>Actions</th>
              <td mat-cell *matCellDef="let payee">
                <button mat-icon-button [matMenuTriggerFor]="actionMenu" matTooltip="Actions">
                  <mat-icon>more_vert</mat-icon>
                </button>
                <mat-menu #actionMenu="matMenu">
                  <button mat-menu-item (click)="openDialog(payee)">
                    <mat-icon>edit</mat-icon>
                    <span>Edit</span>
                  </button>
                  <button mat-menu-item (click)="viewPayee(payee)">
                    <mat-icon>visibility</mat-icon>
                    <span>View Details</span>
                  </button>
                  <mat-divider></mat-divider>
                  <button mat-menu-item (click)="deletePayee(payee)" class="delete-action">
                    <mat-icon>delete</mat-icon>
                    <span>Delete</span>
                  </button>
                </mat-menu>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns;"
                [class.inactive-row]="!row.isActive"></tr>
          </table>

          @if (payees().length === 0) {
            <div class="empty-table">
              <mat-icon>group_off</mat-icon>
              <p>No payees found</p>
            </div>
          }

          <mat-paginator
            [length]="totalRecords()"
            [pageSize]="pageSize()"
            [pageIndex]="pageIndex()"
            [pageSizeOptions]="[10, 25, 50, 100]"
            (page)="onPageChange($event)"
            showFirstLastButtons>
          </mat-paginator>
        </mat-card>
      }
    </div>
  `,
  styles: [`
    .payees-container {
      padding: 24px;
      max-width: 1400px;
      margin: 0 auto;
    }

    .page-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      margin-bottom: 24px;
    }

    .header-content h1 {
      margin: 0;
      font-size: 28px;
      font-weight: 500;
    }

    .subtitle {
      margin: 4px 0 0;
      color: rgba(0, 0, 0, 0.6);
    }

    .header-actions {
      display: flex;
      gap: 12px;
    }

    .summary-cards {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
      gap: 16px;
      margin-bottom: 24px;
    }

    .summary-card {
      mat-card-content {
        display: flex;
        align-items: center;
        gap: 16px;
        padding: 16px !important;
      }
    }

    .summary-icon {
      width: 48px;
      height: 48px;
      border-radius: 12px;
      display: flex;
      align-items: center;
      justify-content: center;

      mat-icon {
        color: white;
        font-size: 24px;
        width: 24px;
        height: 24px;
      }

      &.individuals { background: linear-gradient(135deg, #42a5f5 0%, #1e88e5 100%); }
      &.companies { background: linear-gradient(135deg, #66bb6a 0%, #43a047 100%); }
      &.government { background: linear-gradient(135deg, #ab47bc 0%, #8e24aa 100%); }
      &.total { background: linear-gradient(135deg, #78909c 0%, #546e7a 100%); }
    }

    .summary-info {
      display: flex;
      flex-direction: column;
    }

    .summary-value {
      font-size: 24px;
      font-weight: 600;
      line-height: 1.2;
    }

    .summary-label {
      font-size: 13px;
      color: rgba(0, 0, 0, 0.6);
    }

    .filter-card {
      margin-bottom: 24px;
    }

    .filters {
      display: flex;
      gap: 16px;
      align-items: center;
      flex-wrap: wrap;
    }

    .search-field {
      flex: 1;
      min-width: 280px;
    }

    .loading-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 48px;
      gap: 16px;

      p {
        color: rgba(0, 0, 0, 0.6);
      }
    }

    .payee-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
      gap: 20px;
    }

    .payee-card {
      transition: transform 0.2s, box-shadow 0.2s;

      &:hover {
        transform: translateY(-2px);
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.12);
      }

      &.inactive {
        opacity: 0.7;
      }

      mat-card-header {
        position: relative;
      }

      .card-menu-btn {
        position: absolute;
        top: -8px;
        right: -8px;
      }
    }

    .payee-avatar {
      background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
      display: flex;
      align-items: center;
      justify-content: center;

      mat-icon {
        color: white;
      }
    }

    .payee-details {
      margin-top: 12px;
    }

    .detail-row {
      display: flex;
      align-items: center;
      gap: 8px;
      padding: 6px 0;
      font-size: 13px;
      color: rgba(0, 0, 0, 0.7);

      mat-icon {
        font-size: 18px;
        width: 18px;
        height: 18px;
        color: rgba(0, 0, 0, 0.5);
      }
    }

    .payee-status {
      margin-top: 12px;
      display: flex;
      justify-content: flex-end;
    }

    .status-active {
      background-color: #e8f5e9 !important;
      color: #2e7d32 !important;
    }

    .status-inactive {
      background-color: #ffebee !important;
      color: #c62828 !important;
    }

    .empty-state {
      grid-column: 1 / -1;
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 64px 24px;
      text-align: center;
      background: #fafafa;
      border-radius: 12px;

      mat-icon {
        font-size: 64px;
        width: 64px;
        height: 64px;
        color: rgba(0, 0, 0, 0.2);
        margin-bottom: 16px;
      }

      h3 {
        margin: 0 0 8px;
        color: rgba(0, 0, 0, 0.7);
      }

      p {
        margin: 0 0 24px;
        color: rgba(0, 0, 0, 0.5);
      }
    }

    .table-card {
      overflow: hidden;
    }

    table {
      width: 100%;
    }

    .code-badge {
      font-family: 'Roboto Mono', monospace;
      font-size: 13px;
      padding: 4px 8px;
      background: #f5f5f5;
      border-radius: 4px;
    }

    .payee-name-cell {
      display: flex;
      align-items: center;
      gap: 8px;

      .type-icon {
        font-size: 18px;
        width: 18px;
        height: 18px;
        color: rgba(0, 0, 0, 0.5);
      }
    }

    .type-chip {
      font-size: 11px !important;
      min-height: 24px !important;
    }

    .contact-info {
      display: flex;
      flex-direction: column;
      gap: 2px;
      font-size: 13px;

      .email {
        color: #1976d2;
      }

      .phone {
        color: rgba(0, 0, 0, 0.6);
      }
    }

    .account-code {
      font-family: 'Roboto Mono', monospace;
      font-size: 12px;
    }

    .no-data {
      color: rgba(0, 0, 0, 0.3);
    }

    .inactive-row {
      opacity: 0.6;
    }

    .empty-table {
      display: flex;
      flex-direction: column;
      align-items: center;
      padding: 48px;
      color: rgba(0, 0, 0, 0.5);

      mat-icon {
        font-size: 48px;
        width: 48px;
        height: 48px;
        margin-bottom: 8px;
      }
    }

    .delete-action {
      color: #f44336;
    }
  `]
})
export class PayeesComponent implements OnInit {
  private accountingService = inject(AccountingService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);
  private fb = inject(FormBuilder);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  payees = signal<Payee[]>([]);
  isLoading = signal(false);
  isGridView = signal(true);
  totalRecords = signal(0);
  pageSize = signal(10);
  pageIndex = signal(0);

  searchControl = this.fb.control('');
  typeFilterControl = this.fb.control('');

  dataSource = new MatTableDataSource<Payee>();
  displayedColumns = ['payeeCode', 'payeeName', 'payeeType', 'contact', 'location', 'defaultExpenseAccountCode', 'isActive', 'actions'];

  individualCount = computed(() => this.payees().filter(p => p.payeeType === 'Individual').length);
  companyCount = computed(() => this.payees().filter(p => p.payeeType === 'Company').length);
  governmentCount = computed(() => this.payees().filter(p => p.payeeType === 'Government').length);
  totalCount = computed(() => this.payees().length);

  ngOnInit(): void {
    this.loadPayees();
    this.setupFilters();
  }

  private setupFilters(): void {
    this.searchControl.valueChanges.subscribe(() => this.loadPayees());
    this.typeFilterControl.valueChanges.subscribe(() => this.loadPayees());
  }

  loadPayees(): void {
    this.isLoading.set(true);
    const request = {
      pageNumber: this.pageIndex() + 1,
      pageSize: this.pageSize(),
      searchTerm: this.searchControl.value || undefined,
      status: this.typeFilterControl.value || undefined
    };

    this.accountingService.getPayees(request).subscribe({
      next: (result) => {
        this.payees.set(result.items);
        this.totalRecords.set(result.totalCount);
        this.dataSource.data = result.items;
        this.isLoading.set(false);
      },
      error: () => {
        this.snackBar.open('Error loading payees', 'Close', { duration: 3000 });
        this.isLoading.set(false);
      }
    });
  }

  toggleView(): void {
    this.isGridView.update(v => !v);
  }

  getPayeeTypeIcon(type: string): string {
    switch (type) {
      case 'Individual': return 'person';
      case 'Company': return 'business';
      case 'Government': return 'account_balance';
      default: return 'category';
    }
  }

  openDialog(payee?: Payee): void {
    const dialogRef = this.dialog.open(PayeeDialogComponent, {
      width: '600px',
      data: payee
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadPayees();
      }
    });
  }

  viewPayee(payee: Payee): void {
    this.dialog.open(PayeeViewDialogComponent, {
      width: '500px',
      data: payee
    });
  }

  deletePayee(payee: Payee): void {
    if (confirm(`Are you sure you want to delete payee "${payee.payeeName}"?`)) {
      this.accountingService.deletePayee(payee.id).subscribe({
        next: () => {
          this.snackBar.open('Payee deleted successfully', 'Close', { duration: 3000 });
          this.loadPayees();
        },
        error: () => {
          this.snackBar.open('Error deleting payee', 'Close', { duration: 3000 });
        }
      });
    }
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex.set(event.pageIndex);
    this.pageSize.set(event.pageSize);
    this.loadPayees();
  }

  onSortChange(sort: Sort): void {
    this.loadPayees();
  }
}

// Payee Dialog Component
@Component({
  selector: 'app-payee-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  template: `
    <h2 mat-dialog-title>{{ data ? 'Edit Payee' : 'New Payee' }}</h2>
    <mat-dialog-content>
      <form [formGroup]="form" class="payee-form">
        <div class="form-row">
          <mat-form-field appearance="outline">
            <mat-label>Payee Code</mat-label>
            <input matInput formControlName="payeeCode" placeholder="e.g., PAY001">
            @if (form.get('payeeCode')?.hasError('required')) {
              <mat-error>Code is required</mat-error>
            }
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Payee Type</mat-label>
            <mat-select formControlName="payeeType">
              <mat-option value="Individual">Individual</mat-option>
              <mat-option value="Company">Company</mat-option>
              <mat-option value="Government">Government</mat-option>
              <mat-option value="Other">Other</mat-option>
            </mat-select>
          </mat-form-field>
        </div>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Payee Name</mat-label>
          <input matInput formControlName="payeeName" placeholder="Enter payee name">
          @if (form.get('payeeName')?.hasError('required')) {
            <mat-error>Name is required</mat-error>
          }
        </mat-form-field>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Tax ID</mat-label>
          <input matInput formControlName="taxId" placeholder="Enter tax ID (optional)">
        </mat-form-field>

        <div class="section-title">Contact Information</div>

        <div class="form-row">
          <mat-form-field appearance="outline">
            <mat-label>Email</mat-label>
            <input matInput formControlName="email" type="email" placeholder="email@example.com">
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Phone</mat-label>
            <input matInput formControlName="phone" placeholder="(555) 123-4567">
          </mat-form-field>
        </div>

        <div class="section-title">Address</div>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Address Line 1</mat-label>
          <input matInput formControlName="address1" placeholder="Street address">
        </mat-form-field>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Address Line 2</mat-label>
          <input matInput formControlName="address2" placeholder="Apt, Suite, Unit, etc.">
        </mat-form-field>

        <div class="form-row">
          <mat-form-field appearance="outline">
            <mat-label>City</mat-label>
            <input matInput formControlName="city">
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>State</mat-label>
            <input matInput formControlName="state">
          </mat-form-field>
        </div>

        <div class="form-row">
          <mat-form-field appearance="outline">
            <mat-label>Postal Code</mat-label>
            <input matInput formControlName="postalCode">
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Country</mat-label>
            <input matInput formControlName="country" value="USA">
          </mat-form-field>
        </div>

        <div class="section-title">Defaults</div>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Default Expense Account</mat-label>
          <mat-select formControlName="defaultExpenseAccountId">
            <mat-option [value]="null">None</mat-option>
            @for (account of expenseAccounts(); track account.id) {
              <mat-option [value]="account.id">{{ account.accountCode }} - {{ account.accountName }}</mat-option>
            }
          </mat-select>
        </mat-form-field>
      </form>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-raised-button color="primary" (click)="save()" [disabled]="form.invalid || isSaving()">
        @if (isSaving()) {
          <mat-spinner diameter="20"></mat-spinner>
        } @else {
          {{ data ? 'Update' : 'Create' }}
        }
      </button>
    </mat-dialog-actions>
  `,
  styles: [`
    .payee-form {
      display: flex;
      flex-direction: column;
      gap: 8px;
      min-width: 500px;
    }

    .form-row {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 16px;
    }

    .full-width {
      width: 100%;
    }

    .section-title {
      font-size: 14px;
      font-weight: 500;
      color: rgba(0, 0, 0, 0.6);
      margin: 16px 0 8px;
      padding-bottom: 8px;
      border-bottom: 1px solid rgba(0, 0, 0, 0.1);
    }

    mat-dialog-content {
      max-height: 70vh;
    }
  `]
})
export class PayeeDialogComponent implements OnInit {
  private dialogRef = inject(MatDialogRef<PayeeDialogComponent>);
  private accountingService = inject(AccountingService);
  private snackBar = inject(MatSnackBar);
  private fb = inject(FormBuilder);
  public data = inject<Payee | null>(MAT_DIALOG_DATA);

  form!: FormGroup;
  isSaving = signal(false);
  expenseAccounts = signal<ChartOfAccount[]>([]);

  ngOnInit(): void {
    this.initForm();
    this.loadExpenseAccounts();
  }

  private initForm(): void {
    this.form = this.fb.group({
      payeeCode: [this.data?.payeeCode || '', Validators.required],
      payeeName: [this.data?.payeeName || '', Validators.required],
      payeeType: [this.data?.payeeType || 'Individual'],
      taxId: [this.data?.taxId || ''],
      email: [this.data?.email || ''],
      phone: [this.data?.phone || ''],
      address1: [this.data?.address1 || ''],
      address2: [this.data?.address2 || ''],
      city: [this.data?.city || ''],
      state: [this.data?.state || ''],
      postalCode: [this.data?.postalCode || ''],
      country: [this.data?.country || 'USA'],
      defaultExpenseAccountId: [this.data?.defaultExpenseAccountId || null]
    });
  }

  private loadExpenseAccounts(): void {
    this.accountingService.getAccountsByType('Expense').subscribe({
      next: (accounts) => this.expenseAccounts.set(accounts)
    });
  }

  save(): void {
    if (this.form.invalid) return;

    this.isSaving.set(true);
    const formValue = this.form.value;

    const operation = this.data
      ? this.accountingService.updatePayee(this.data.id, formValue)
      : this.accountingService.createPayee(formValue);

    operation.subscribe({
      next: () => {
        this.snackBar.open(`Payee ${this.data ? 'updated' : 'created'} successfully`, 'Close', { duration: 3000 });
        this.dialogRef.close(true);
      },
      error: () => {
        this.snackBar.open('Error saving payee', 'Close', { duration: 3000 });
        this.isSaving.set(false);
      }
    });
  }
}

// Payee View Dialog Component
@Component({
  selector: 'app-payee-view-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatDividerModule
  ],
  template: `
    <h2 mat-dialog-title>
      <div class="title-row">
        <mat-icon>{{ getPayeeTypeIcon(data.payeeType) }}</mat-icon>
        <span>{{ data.payeeName }}</span>
      </div>
    </h2>
    <mat-dialog-content>
      <div class="view-content">
        <div class="info-section">
          <div class="info-row">
            <span class="label">Code:</span>
            <span class="value code">{{ data.payeeCode }}</span>
          </div>
          <div class="info-row">
            <span class="label">Type:</span>
            <mat-chip>{{ data.payeeType }}</mat-chip>
          </div>
          <div class="info-row">
            <span class="label">Status:</span>
            <mat-chip [class]="data.isActive ? 'status-active' : 'status-inactive'">
              {{ data.isActive ? 'Active' : 'Inactive' }}
            </mat-chip>
          </div>
          @if (data.taxId) {
            <div class="info-row">
              <span class="label">Tax ID:</span>
              <span class="value">{{ data.taxId }}</span>
            </div>
          }
        </div>

        <mat-divider></mat-divider>

        <div class="info-section">
          <h4>Contact Information</h4>
          @if (data.email) {
            <div class="info-row">
              <mat-icon>email</mat-icon>
              <a [href]="'mailto:' + data.email">{{ data.email }}</a>
            </div>
          }
          @if (data.phone) {
            <div class="info-row">
              <mat-icon>phone</mat-icon>
              <span>{{ data.phone }}</span>
            </div>
          }
        </div>

        @if (data.address1 || data.city) {
          <mat-divider></mat-divider>
          <div class="info-section">
            <h4>Address</h4>
            <div class="address-block">
              @if (data.address1) {
                <div>{{ data.address1 }}</div>
              }
              @if (data.address2) {
                <div>{{ data.address2 }}</div>
              }
              @if (data.city || data.state || data.postalCode) {
                <div>{{ data.city }}{{ data.city && data.state ? ', ' : '' }}{{ data.state }} {{ data.postalCode }}</div>
              }
              @if (data.country) {
                <div>{{ data.country }}</div>
              }
            </div>
          </div>
        }

        @if (data.defaultExpenseAccountCode) {
          <mat-divider></mat-divider>
          <div class="info-section">
            <h4>Default Expense Account</h4>
            <div class="account-display">
              <mat-icon>account_balance_wallet</mat-icon>
              <span>{{ data.defaultExpenseAccountCode }}</span>
            </div>
          </div>
        }
      </div>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Close</button>
    </mat-dialog-actions>
  `,
  styles: [`
    .title-row {
      display: flex;
      align-items: center;
      gap: 12px;

      mat-icon {
        color: #667eea;
      }
    }

    .view-content {
      min-width: 400px;
    }

    .info-section {
      padding: 16px 0;

      h4 {
        margin: 0 0 12px;
        color: rgba(0, 0, 0, 0.6);
        font-size: 14px;
      }
    }

    .info-row {
      display: flex;
      align-items: center;
      gap: 12px;
      padding: 8px 0;

      .label {
        color: rgba(0, 0, 0, 0.6);
        min-width: 80px;
      }

      .value {
        font-weight: 500;

        &.code {
          font-family: 'Roboto Mono', monospace;
          background: #f5f5f5;
          padding: 4px 8px;
          border-radius: 4px;
        }
      }

      mat-icon {
        font-size: 18px;
        width: 18px;
        height: 18px;
        color: rgba(0, 0, 0, 0.5);
      }

      a {
        color: #1976d2;
        text-decoration: none;

        &:hover {
          text-decoration: underline;
        }
      }
    }

    .status-active {
      background-color: #e8f5e9 !important;
      color: #2e7d32 !important;
    }

    .status-inactive {
      background-color: #ffebee !important;
      color: #c62828 !important;
    }

    .address-block {
      padding-left: 24px;
      line-height: 1.6;
    }

    .account-display {
      display: flex;
      align-items: center;
      gap: 8px;
      font-family: 'Roboto Mono', monospace;

      mat-icon {
        color: #667eea;
      }
    }
  `]
})
export class PayeeViewDialogComponent {
  public data = inject<Payee>(MAT_DIALOG_DATA);

  getPayeeTypeIcon(type: string): string {
    switch (type) {
      case 'Individual': return 'person';
      case 'Company': return 'business';
      case 'Government': return 'account_balance';
      default: return 'category';
    }
  }
}

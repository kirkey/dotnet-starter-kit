import { Component, inject, signal, OnInit, computed } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { NotificationService } from '@core/services/notification.service';
import { AccountingService } from '@core/services/accounting.service';
import { AccountReconciliation, ReconciliationStatus, ChartOfAccount, AccountingPeriod } from '@core/models/accounting.model';

@Component({
  selector: 'app-account-reconciliations',
  standalone: true,
  imports: [
    CommonModule,
    CurrencyPipe,
    DatePipe,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDialogModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatMenuModule,
    MatTooltipModule,
    MatCardModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatDividerModule,
    MatExpansionModule,
    PageHeaderComponent
  ],
  template: `
    <div class="reconciliations-container">
      <app-page-header 
        title="Account Reconciliations" 
        subtitle="Reconcile general ledger accounts"
        icon="account_balance_wallet">
      </app-page-header>

      <!-- Toolbar -->
      <div class="toolbar">
        <div class="filters">
          <mat-form-field appearance="outline" class="search-field">
            <mat-label>Search reconciliations</mat-label>
            <input matInput 
                   [value]="searchQuery()" 
                   (input)="onSearch($event)"
                   placeholder="Search by account, number...">
            <mat-icon matPrefix>search</mat-icon>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Period</mat-label>
            <mat-select [value]="selectedPeriod()" (selectionChange)="onPeriodChange($event.value)">
              <mat-option value="">All Periods</mat-option>
              @for (period of periods(); track period.id) {
                <mat-option [value]="period.id">{{ period.periodName }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Status</mat-label>
            <mat-select [value]="selectedStatus()" (selectionChange)="onStatusChange($event.value)">
              <mat-option value="">All Statuses</mat-option>
              <mat-option value="InProgress">In Progress</mat-option>
              <mat-option value="Completed">Completed</mat-option>
              <mat-option value="Approved">Approved</mat-option>
            </mat-select>
          </mat-form-field>
        </div>

        <div class="actions">
          <button mat-raised-button color="primary" (click)="openNewReconciliation()">
            <mat-icon>add</mat-icon>
            New Reconciliation
          </button>
        </div>
      </div>

      <!-- Summary Cards -->
      <div class="summary-cards">
        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon in-progress">
              <mat-icon>hourglass_empty</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">In Progress</span>
              <span class="summary-value">{{ inProgressCount() }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon pending-review">
              <mat-icon>rate_review</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Pending Review</span>
              <span class="summary-value">{{ pendingReviewCount() }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon completed">
              <mat-icon>check_circle</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Completed This Period</span>
              <span class="summary-value">{{ completedCount() }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon variance">
              <mat-icon>warning</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Total Variances</span>
              <span class="summary-value">{{ totalVariance() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>
      </div>

      <!-- Loading -->
      @if (isLoading()) {
        <div class="loading-container">
          <mat-spinner diameter="48"></mat-spinner>
        </div>
      } @else {
        <!-- Table -->
        <div class="table-container">
          <table mat-table [dataSource]="filteredReconciliations()" matSort (matSortChange)="onSort($event)">
            <!-- Reconciliation Number Column -->
            <ng-container matColumnDef="reconciliationNumber">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Recon #</th>
              <td mat-cell *matCellDef="let recon">
                <span class="recon-number">{{ recon.reconciliationNumber }}</span>
              </td>
            </ng-container>

            <!-- Account Column -->
            <ng-container matColumnDef="account">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Account</th>
              <td mat-cell *matCellDef="let recon">
                <div class="account-info">
                  <span class="account-code">{{ recon.accountCode }}</span>
                  <span class="account-name">{{ recon.accountName }}</span>
                </div>
              </td>
            </ng-container>

            <!-- Period Column -->
            <ng-container matColumnDef="period">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Period</th>
              <td mat-cell *matCellDef="let recon">{{ recon.periodName }}</td>
            </ng-container>

            <!-- Reconciliation Date Column -->
            <ng-container matColumnDef="reconciliationDate">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Date</th>
              <td mat-cell *matCellDef="let recon">{{ recon.reconciliationDate | date:'mediumDate' }}</td>
            </ng-container>

            <!-- Beginning Balance Column -->
            <ng-container matColumnDef="beginningBalance">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Beginning</th>
              <td mat-cell *matCellDef="let recon">{{ recon.beginningBalance | currency }}</td>
            </ng-container>

            <!-- Ending Balance Column -->
            <ng-container matColumnDef="endingBalance">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Ending</th>
              <td mat-cell *matCellDef="let recon">{{ recon.endingBalance | currency }}</td>
            </ng-container>

            <!-- Difference Column -->
            <ng-container matColumnDef="difference">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Variance</th>
              <td mat-cell *matCellDef="let recon">
                <span [class.has-variance]="recon.unreconciledDifference !== 0">
                  {{ recon.unreconciledDifference | currency }}
                </span>
              </td>
            </ng-container>

            <!-- Status Column -->
            <ng-container matColumnDef="status">
              <th mat-header-cell *matHeaderCellDef>Status</th>
              <td mat-cell *matCellDef="let recon">
                <mat-chip [class]="getStatusClass(recon.status)">{{ getStatusLabel(recon.status) }}</mat-chip>
              </td>
            </ng-container>

            <!-- Prepared By Column -->
            <ng-container matColumnDef="preparedBy">
              <th mat-header-cell *matHeaderCellDef>Prepared By</th>
              <td mat-cell *matCellDef="let recon">{{ recon.preparedBy || '-' }}</td>
            </ng-container>

            <!-- Actions Column -->
            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef></th>
              <td mat-cell *matCellDef="let recon">
                <button mat-icon-button [matMenuTriggerFor]="menu">
                  <mat-icon>more_vert</mat-icon>
                </button>
                <mat-menu #menu="matMenu">
                  <button mat-menu-item (click)="viewReconciliation(recon)">
                    <mat-icon>visibility</mat-icon>
                    <span>View Details</span>
                  </button>
                  @if (recon.status === 'InProgress') {
                    <button mat-menu-item (click)="continueReconciliation(recon)">
                      <mat-icon>edit</mat-icon>
                      <span>Continue</span>
                    </button>
                    <button mat-menu-item (click)="completeReconciliation(recon)">
                      <mat-icon>check</mat-icon>
                      <span>Mark Complete</span>
                    </button>
                  }
                  @if (recon.status === 'Completed') {
                    <button mat-menu-item (click)="approveReconciliation(recon)">
                      <mat-icon>verified</mat-icon>
                      <span>Approve</span>
                    </button>
                    <button mat-menu-item (click)="reopenReconciliation(recon)">
                      <mat-icon>undo</mat-icon>
                      <span>Reopen</span>
                    </button>
                  }
                  <button mat-menu-item (click)="exportReconciliation(recon)">
                    <mat-icon>download</mat-icon>
                    <span>Export</span>
                  </button>
                  <button mat-menu-item (click)="uploadDocuments(recon)">
                    <mat-icon>attach_file</mat-icon>
                    <span>Attach Documents</span>
                  </button>
                </mat-menu>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
          </table>

          <mat-paginator 
            [length]="totalReconciliations()"
            [pageSize]="pageSize()"
            [pageSizeOptions]="[10, 25, 50, 100]"
            (page)="onPageChange($event)"
            showFirstLastButtons>
          </mat-paginator>
        </div>

        <!-- Empty State -->
        @if (filteredReconciliations().length === 0) {
          <div class="empty-state">
            <mat-icon>account_balance_wallet</mat-icon>
            <h3>No reconciliations found</h3>
            <p>Start reconciling accounts to ensure accuracy</p>
            <button mat-raised-button color="primary" (click)="openNewReconciliation()">
              <mat-icon>add</mat-icon>
              New Reconciliation
            </button>
          </div>
        }
      }
    </div>
  `,
  styles: [`
    .reconciliations-container {
      padding: 24px;
    }

    .toolbar {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      margin-bottom: 24px;
      gap: 16px;
      flex-wrap: wrap;
    }

    .filters {
      display: flex;
      gap: 12px;
      flex-wrap: wrap;
      flex: 1;
    }

    .search-field {
      min-width: 250px;
      flex: 1;
    }

    .filter-field {
      min-width: 150px;
    }

    .actions {
      display: flex;
      gap: 8px;
    }

    .summary-cards {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
      gap: 16px;
      margin-bottom: 24px;
    }

    .summary-card mat-card-content {
      display: flex;
      align-items: center;
      gap: 16px;
      padding: 16px !important;
    }

    .summary-icon {
      width: 48px;
      height: 48px;
      border-radius: 12px;
      display: flex;
      align-items: center;
      justify-content: center;
    }

    .summary-icon.in-progress {
      background: #fef3c7;
      color: #d97706;
    }

    .summary-icon.pending-review {
      background: #e0e7ff;
      color: #4f46e5;
    }

    .summary-icon.completed {
      background: #dcfce7;
      color: #16a34a;
    }

    .summary-icon.variance {
      background: #fee2e2;
      color: #dc2626;
    }

    .summary-info {
      display: flex;
      flex-direction: column;
    }

    .summary-label {
      font-size: 12px;
      color: var(--text-secondary);
    }

    .summary-value {
      font-size: 20px;
      font-weight: 600;
    }

    .loading-container {
      display: flex;
      justify-content: center;
      padding: 48px;
    }

    .table-container {
      background: var(--surface-color);
      border-radius: 8px;
      overflow: hidden;
      box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
    }

    table {
      width: 100%;
    }

    .recon-number {
      font-weight: 500;
      color: var(--primary-color);
    }

    .account-info {
      display: flex;
      flex-direction: column;
    }

    .account-code {
      font-weight: 500;
    }

    .account-name {
      font-size: 12px;
      color: var(--text-secondary);
    }

    .has-variance {
      color: #dc2626;
      font-weight: 500;
    }

    .status-inprogress { background: #fef3c7 !important; color: #92400e !important; }
    .status-completed { background: #e0e7ff !important; color: #3730a3 !important; }
    .status-approved { background: #dcfce7 !important; color: #166534 !important; }

    .empty-state {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 64px 24px;
      text-align: center;
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
      color: var(--text-secondary);
    }

    @media (max-width: 768px) {
      .filters {
        flex-direction: column;
      }

      .search-field,
      .filter-field {
        width: 100%;
        min-width: auto;
      }
    }
  `]
})
export class AccountReconciliationsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private notification = inject(NotificationService);
  private accountingService = inject(AccountingService);

  // State signals
  reconciliations = signal<AccountReconciliation[]>([]);
  accounts = signal<ChartOfAccount[]>([]);
  periods = signal<AccountingPeriod[]>([]);
  isLoading = signal(false);
  searchQuery = signal('');
  selectedPeriod = signal('');
  selectedStatus = signal('');

  // Pagination
  pageSize = signal(10);
  pageIndex = signal(0);
  totalReconciliations = signal(0);

  displayedColumns = ['reconciliationNumber', 'account', 'period', 'reconciliationDate', 'beginningBalance', 'endingBalance', 'difference', 'status', 'preparedBy', 'actions'];

  // Computed signals
  filteredReconciliations = computed(() => {
    let result = this.reconciliations();
    
    const query = this.searchQuery().toLowerCase();
    if (query) {
      result = result.filter(r => 
        r.reconciliationNumber.toLowerCase().includes(query) ||
        r.accountCode.toLowerCase().includes(query) ||
        r.accountName.toLowerCase().includes(query)
      );
    }
    
    const periodId = this.selectedPeriod();
    if (periodId) {
      result = result.filter(r => r.periodId === periodId);
    }

    const status = this.selectedStatus();
    if (status) {
      result = result.filter(r => r.status === status);
    }
    
    return result;
  });

  inProgressCount = computed(() => 
    this.reconciliations().filter(r => r.status === ReconciliationStatus.InProgress).length
  );

  pendingReviewCount = computed(() => 
    this.reconciliations().filter(r => r.status === ReconciliationStatus.Completed).length
  );

  completedCount = computed(() => 
    this.reconciliations().filter(r => r.status === ReconciliationStatus.Approved).length
  );

  totalVariance = computed(() => 
    this.reconciliations().reduce((sum, r) => sum + Math.abs(r.unreconciledDifference), 0)
  );

  ngOnInit(): void {
    this.loadData();
  }

  async loadData(): Promise<void> {
    this.isLoading.set(true);
    
    try {
      await Promise.all([
        this.loadReconciliations(),
        this.loadAccounts(),
        this.loadPeriods()
      ]);
    } finally {
      this.isLoading.set(false);
    }
  }

  private async loadReconciliations(): Promise<void> {
    const mockReconciliations: AccountReconciliation[] = [
      {
        id: '1',
        reconciliationNumber: 'RECON-001',
        accountId: '1',
        accountCode: '1100',
        accountName: 'Accounts Receivable',
        periodId: '1',
        periodName: 'January 2024',
        reconciliationDate: new Date('2024-01-31'),
        beginningBalance: 125000,
        endingBalance: 142500,
        reconciledBalance: 142500,
        unreconciledDifference: 0,
        status: ReconciliationStatus.Approved,
        preparedBy: 'John Smith',
        preparedOn: new Date('2024-02-01'),
        reviewedBy: 'Jane Doe',
        reviewedOn: new Date('2024-02-02'),
        approvedBy: 'Mike Johnson',
        approvedOn: new Date('2024-02-03'),
        isActive: true
      },
      {
        id: '2',
        reconciliationNumber: 'RECON-002',
        accountId: '2',
        accountCode: '2100',
        accountName: 'Accounts Payable',
        periodId: '1',
        periodName: 'January 2024',
        reconciliationDate: new Date('2024-01-31'),
        beginningBalance: 85000,
        endingBalance: 92300,
        reconciledBalance: 91500,
        unreconciledDifference: 800,
        status: ReconciliationStatus.Completed,
        preparedBy: 'John Smith',
        preparedOn: new Date('2024-02-01'),
        isActive: true
      },
      {
        id: '3',
        reconciliationNumber: 'RECON-003',
        accountId: '3',
        accountCode: '1400',
        accountName: 'Prepaid Expenses',
        periodId: '1',
        periodName: 'January 2024',
        reconciliationDate: new Date('2024-01-31'),
        beginningBalance: 15000,
        endingBalance: 12500,
        reconciledBalance: 10000,
        unreconciledDifference: 2500,
        status: ReconciliationStatus.InProgress,
        preparedBy: 'Sarah Wilson',
        preparedOn: new Date('2024-02-04'),
        isActive: true
      }
    ];
    
    await new Promise(resolve => setTimeout(resolve, 300));
    this.reconciliations.set(mockReconciliations);
    this.totalReconciliations.set(mockReconciliations.length);
  }

  private async loadAccounts(): Promise<void> {
    const mockAccounts: ChartOfAccount[] = [
      { id: '1', accountCode: '1100', accountName: 'Accounts Receivable', accountType: 'Asset' as any, currentBalance: 142500, isHeader: false, isBankAccount: false, level: 1, sortOrder: 1, normalBalance: 'Debit', isActive: true },
      { id: '2', accountCode: '2100', accountName: 'Accounts Payable', accountType: 'Liability' as any, currentBalance: 92300, isHeader: false, isBankAccount: false, level: 1, sortOrder: 2, normalBalance: 'Credit', isActive: true },
      { id: '3', accountCode: '1400', accountName: 'Prepaid Expenses', accountType: 'Asset' as any, currentBalance: 12500, isHeader: false, isBankAccount: false, level: 1, sortOrder: 3, normalBalance: 'Debit', isActive: true }
    ];
    this.accounts.set(mockAccounts);
  }

  private async loadPeriods(): Promise<void> {
    const mockPeriods: AccountingPeriod[] = [
      { id: '1', periodNumber: 1, periodName: 'January 2024', fiscalYear: 2024, startDate: new Date('2024-01-01'), endDate: new Date('2024-01-31'), status: 'Open' as any, isCurrent: true, isAdjustmentPeriod: false, isActive: true },
      { id: '2', periodNumber: 12, periodName: 'December 2023', fiscalYear: 2023, startDate: new Date('2023-12-01'), endDate: new Date('2023-12-31'), status: 'Closed' as any, isCurrent: false, isAdjustmentPeriod: false, isActive: true }
    ];
    this.periods.set(mockPeriods);
  }

  onSearch(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.searchQuery.set(value);
  }

  onPeriodChange(value: string): void {
    this.selectedPeriod.set(value);
  }

  onStatusChange(value: string): void {
    this.selectedStatus.set(value);
  }

  onSort(sort: Sort): void {
    // Implement sorting logic
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex.set(event.pageIndex);
    this.pageSize.set(event.pageSize);
  }

  getStatusClass(status: ReconciliationStatus): string {
    return `status-${status.toLowerCase()}`;
  }

  getStatusLabel(status: ReconciliationStatus): string {
    const labels: Record<string, string> = {
      'InProgress': 'In Progress',
      'Completed': 'Pending Review',
      'Approved': 'Approved'
    };
    return labels[status] || status;
  }

  openNewReconciliation(): void {
    // Open dialog for new reconciliation
    console.log('Open new reconciliation dialog');
  }

  viewReconciliation(recon: AccountReconciliation): void {
    console.log('View reconciliation:', recon);
  }

  continueReconciliation(recon: AccountReconciliation): void {
    console.log('Continue reconciliation:', recon);
  }

  async completeReconciliation(recon: AccountReconciliation): Promise<void> {
    try {
      await new Promise(resolve => setTimeout(resolve, 500));
      this.notification.success('Reconciliation marked as complete');
      this.loadReconciliations();
    } catch (error) {
      this.notification.error('Failed to complete reconciliation');
    }
  }

  async approveReconciliation(recon: AccountReconciliation): Promise<void> {
    try {
      await new Promise(resolve => setTimeout(resolve, 500));
      this.notification.success('Reconciliation approved');
      this.loadReconciliations();
    } catch (error) {
      this.notification.error('Failed to approve reconciliation');
    }
  }

  async reopenReconciliation(recon: AccountReconciliation): Promise<void> {
    try {
      await new Promise(resolve => setTimeout(resolve, 500));
      this.notification.success('Reconciliation reopened');
      this.loadReconciliations();
    } catch (error) {
      this.notification.error('Failed to reopen reconciliation');
    }
  }

  exportReconciliation(recon: AccountReconciliation): void {
    console.log('Export reconciliation:', recon);
  }

  uploadDocuments(recon: AccountReconciliation): void {
    console.log('Upload documents for:', recon);
  }
}

import { Component, inject, signal, OnInit, ViewChild, TemplateRef, computed } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
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
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { NotificationService } from '@core/services/notification.service';
import { Accrual, ApprovalStatus, ChartOfAccount, AccrualType, AccountType } from '@core/models/accounting.model';

@Component({
  selector: 'app-accruals',
  standalone: true,
  imports: [
    CommonModule, CurrencyPipe, DatePipe, ReactiveFormsModule,
    MatTableModule, MatPaginatorModule, MatSortModule, MatButtonModule, MatIconModule,
    MatFormFieldModule, MatInputModule, MatSelectModule, MatDialogModule,
    MatProgressSpinnerModule, MatChipsModule, MatMenuModule, MatTooltipModule,
    MatCardModule, MatDatepickerModule, MatNativeDateModule, PageHeaderComponent
  ],
  template: `
    <div class="accruals-container">
      <app-page-header 
        title="Accruals" 
        subtitle="Manage accrued expenses and accrued revenue entries"
        icon="schedule">
      </app-page-header>

      <div class="toolbar">
        <div class="filters">
          <mat-form-field appearance="outline" class="search-field">
            <mat-label>Search accruals</mat-label>
            <input matInput [value]="searchQuery()" (input)="onSearch($event)" placeholder="Search...">
            <mat-icon matPrefix>search</mat-icon>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Type</mat-label>
            <mat-select [value]="selectedType()" (selectionChange)="onTypeChange($event.value)">
              <mat-option value="">All</mat-option>
              <mat-option value="Expense">Expense Accrual</mat-option>
              <mat-option value="Revenue">Revenue Accrual</mat-option>
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Status</mat-label>
            <mat-select [value]="selectedStatus()" (selectionChange)="onStatusChange($event.value)">
              <mat-option value="">All</mat-option>
              <mat-option value="Draft">Draft</mat-option>
              <mat-option value="Pending">Pending</mat-option>
              <mat-option value="Approved">Approved</mat-option>
            </mat-select>
          </mat-form-field>
        </div>

        <button mat-raised-button color="primary" (click)="openAccrualDialog()">
          <mat-icon>add</mat-icon>
          New Accrual
        </button>
      </div>

      <div class="summary-cards">
        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon expense">
              <mat-icon>trending_down</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Accrued Expenses</span>
              <span class="summary-value">{{ totalExpenseAccruals() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon revenue">
              <mat-icon>trending_up</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Accrued Revenue</span>
              <span class="summary-value">{{ totalRevenueAccruals() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon pending">
              <mat-icon>hourglass_empty</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Pending Reversal</span>
              <span class="summary-value">{{ pendingReversalCount() }}</span>
            </div>
          </mat-card-content>
        </mat-card>
      </div>

      @if (isLoading()) {
        <div class="loading-container">
          <mat-spinner diameter="48"></mat-spinner>
        </div>
      } @else {
        <div class="table-container">
          <table mat-table [dataSource]="filteredAccruals()" matSort>
            <ng-container matColumnDef="accrualNumber">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Accrual #</th>
              <td mat-cell *matCellDef="let accrual">
                <span class="accrual-number">{{ accrual.accrualNumber }}</span>
              </td>
            </ng-container>

            <ng-container matColumnDef="type">
              <th mat-header-cell *matHeaderCellDef>Type</th>
              <td mat-cell *matCellDef="let accrual">
                <mat-chip [class]="'type-' + accrual.accrualType.toLowerCase()">{{ accrual.accrualType }}</mat-chip>
              </td>
            </ng-container>

            <ng-container matColumnDef="account">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Account</th>
              <td mat-cell *matCellDef="let accrual">{{ accrual.accountName }}</td>
            </ng-container>

            <ng-container matColumnDef="accrualDate">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Accrual Date</th>
              <td mat-cell *matCellDef="let accrual">{{ accrual.accrualDate | date:'mediumDate' }}</td>
            </ng-container>

            <ng-container matColumnDef="reversalDate">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Reversal Date</th>
              <td mat-cell *matCellDef="let accrual">{{ accrual.reversalDate | date:'mediumDate' }}</td>
            </ng-container>

            <ng-container matColumnDef="amount">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Amount</th>
              <td mat-cell *matCellDef="let accrual">{{ accrual.amount | currency }}</td>
            </ng-container>

            <ng-container matColumnDef="status">
              <th mat-header-cell *matHeaderCellDef>Status</th>
              <td mat-cell *matCellDef="let accrual">
                <mat-chip [class]="'status-' + accrual.status.toLowerCase()">{{ accrual.status }}</mat-chip>
              </td>
            </ng-container>

            <ng-container matColumnDef="reversed">
              <th mat-header-cell *matHeaderCellDef>Reversed</th>
              <td mat-cell *matCellDef="let accrual">
                @if (accrual.isReversed) {
                  <mat-icon class="reversed-icon">check_circle</mat-icon>
                } @else {
                  <mat-icon class="not-reversed-icon">pending</mat-icon>
                }
              </td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef></th>
              <td mat-cell *matCellDef="let accrual">
                <button mat-icon-button [matMenuTriggerFor]="menu">
                  <mat-icon>more_vert</mat-icon>
                </button>
                <mat-menu #menu="matMenu">
                  <button mat-menu-item (click)="viewAccrual(accrual)">
                    <mat-icon>visibility</mat-icon>
                    <span>View</span>
                  </button>
                  @if (accrual.status === 'Draft') {
                    <button mat-menu-item (click)="openAccrualDialog(accrual)">
                      <mat-icon>edit</mat-icon>
                      <span>Edit</span>
                    </button>
                    <button mat-menu-item (click)="submitForApproval(accrual)">
                      <mat-icon>send</mat-icon>
                      <span>Submit</span>
                    </button>
                  }
                  @if (accrual.status === 'Approved' && !accrual.isReversed) {
                    <button mat-menu-item (click)="reverseAccrual(accrual)">
                      <mat-icon>undo</mat-icon>
                      <span>Reverse</span>
                    </button>
                  }
                </mat-menu>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns;" [class.reversed]="row.isReversed"></tr>
          </table>

          <mat-paginator [length]="totalAccruals()" [pageSize]="pageSize()" [pageSizeOptions]="[10, 25, 50]" showFirstLastButtons></mat-paginator>
        </div>
      }
    </div>

    <ng-template #accrualDialog>
      <h2 mat-dialog-title>{{ editingAccrual() ? 'Edit Accrual' : 'New Accrual' }}</h2>
      <mat-dialog-content>
        <form [formGroup]="accrualForm" class="accrual-form">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Accrual Type</mat-label>
            <mat-select formControlName="accrualType" required>
              <mat-option value="Expense">Expense Accrual</mat-option>
              <mat-option value="Revenue">Revenue Accrual</mat-option>
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Account</mat-label>
            <mat-select formControlName="accountId" required>
              @for (account of accounts(); track account.id) {
                <mat-option [value]="account.id">{{ account.accountCode }} - {{ account.accountName }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Accrual Date</mat-label>
              <input matInput [matDatepicker]="accrualPicker" formControlName="accrualDate" required>
              <mat-datepicker-toggle matIconSuffix [for]="accrualPicker"></mat-datepicker-toggle>
              <mat-datepicker #accrualPicker></mat-datepicker>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Reversal Date</mat-label>
              <input matInput [matDatepicker]="reversalPicker" formControlName="reversalDate" required>
              <mat-datepicker-toggle matIconSuffix [for]="reversalPicker"></mat-datepicker-toggle>
              <mat-datepicker #reversalPicker></mat-datepicker>
            </mat-form-field>
          </div>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Amount</mat-label>
            <input matInput type="number" formControlName="amount" required>
            <span matPrefix>$&nbsp;</span>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Description</mat-label>
            <textarea matInput formControlName="description" rows="3" placeholder="Enter accrual description"></textarea>
          </mat-form-field>
        </form>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button mat-dialog-close>Cancel</button>
        <button mat-raised-button color="primary" [disabled]="accrualForm.invalid || isSaving()" (click)="saveAccrual()">
          @if (isSaving()) { <mat-spinner diameter="20"></mat-spinner> }
          Save
        </button>
      </mat-dialog-actions>
    </ng-template>
  `,
  styles: [`
    .accruals-container { padding: 24px; }
    .toolbar { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 24px; gap: 16px; flex-wrap: wrap; }
    .filters { display: flex; gap: 12px; flex-wrap: wrap; flex: 1; }
    .search-field { min-width: 250px; flex: 1; }
    .filter-field { min-width: 150px; }
    .summary-cards { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 16px; margin-bottom: 24px; }
    .summary-card mat-card-content { display: flex; align-items: center; gap: 16px; padding: 16px !important; }
    .summary-icon { width: 48px; height: 48px; border-radius: 12px; display: flex; align-items: center; justify-content: center; }
    .summary-icon.expense { background: #fee2e2; color: #dc2626; }
    .summary-icon.revenue { background: #dcfce7; color: #16a34a; }
    .summary-icon.pending { background: #fef3c7; color: #d97706; }
    .summary-info { display: flex; flex-direction: column; }
    .summary-label { font-size: 12px; color: var(--text-secondary); }
    .summary-value { font-size: 20px; font-weight: 600; }
    .loading-container { display: flex; justify-content: center; padding: 48px; }
    .table-container { background: var(--surface-color); border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1); }
    table { width: 100%; }
    .accrual-number { font-weight: 500; color: var(--primary-color); }
    .type-expense { background: #fee2e2 !important; color: #991b1b !important; }
    .type-revenue { background: #dcfce7 !important; color: #166534 !important; }
    .status-draft { background: #f3f4f6 !important; color: #374151 !important; }
    .status-pending { background: #fef3c7 !important; color: #92400e !important; }
    .status-approved { background: #dcfce7 !important; color: #166534 !important; }
    tr.reversed { background: #f9fafb; opacity: 0.7; }
    .reversed-icon { color: #16a34a; }
    .not-reversed-icon { color: #9ca3af; }
    .accrual-form { display: flex; flex-direction: column; gap: 16px; min-width: 450px; }
    .form-row { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
    .full-width { width: 100%; }
  `]
})
export class AccrualsComponent implements OnInit {
  @ViewChild('accrualDialog') accrualDialogTemplate!: TemplateRef<any>;

  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private notification = inject(NotificationService);

  accruals = signal<Accrual[]>([]);
  accounts = signal<ChartOfAccount[]>([]);
  isLoading = signal(false);
  isSaving = signal(false);
  searchQuery = signal('');
  selectedType = signal('');
  selectedStatus = signal('');
  editingAccrual = signal<Accrual | null>(null);

  pageSize = signal(10);
  totalAccruals = signal(0);

  displayedColumns = ['accrualNumber', 'type', 'account', 'accrualDate', 'reversalDate', 'amount', 'status', 'reversed', 'actions'];

  accrualForm: FormGroup = this.fb.group({
    accrualType: [AccrualType.Expense, Validators.required],
    accountId: ['', Validators.required],
    accrualDate: [new Date(), Validators.required],
    reversalDate: [new Date(), Validators.required],
    amount: [0, [Validators.required, Validators.min(0.01)]],
    description: ['']
  });

  filteredAccruals = computed(() => {
    let result = this.accruals();
    const query = this.searchQuery().toLowerCase();
    if (query) result = result.filter(a => a.accrualNumber.toLowerCase().includes(query) || a.accountName.toLowerCase().includes(query));
    if (this.selectedType()) result = result.filter(a => a.accrualType === this.selectedType());
    if (this.selectedStatus()) result = result.filter(a => a.status === this.selectedStatus());
    return result;
  });

  totalExpenseAccruals = computed(() => this.accruals().filter(a => a.accrualType === AccrualType.Expense && !a.isReversed).reduce((sum, a) => sum + a.amount, 0));
  totalRevenueAccruals = computed(() => this.accruals().filter(a => a.accrualType === AccrualType.Revenue && !a.isReversed).reduce((sum, a) => sum + a.amount, 0));
  pendingReversalCount = computed(() => this.accruals().filter(a => a.status === ApprovalStatus.Approved && !a.isReversed).length);

  ngOnInit(): void { this.loadData(); }

  async loadData(): Promise<void> {
    this.isLoading.set(true);
    try {
      const mockAccruals: Accrual[] = [
        { id: '1', accrualNumber: 'ACC-001', accrualType: AccrualType.Expense, accountId: '1', accountCode: '2100', accountName: 'Accrued Expenses', offsetAccountId: '1', offsetAccountCode: '6100', offsetAccountName: 'Utilities Expense', accrualDate: new Date('2024-01-31'), reversalDate: new Date('2024-02-01'), amount: 5000, description: 'January utilities accrual', status: ApprovalStatus.Approved, periodId: '1', periodName: 'January 2024', isReversed: false, isActive: true },
        { id: '2', accrualNumber: 'ACC-002', accrualType: AccrualType.Revenue, accountId: '2', accountCode: '1300', accountName: 'Accrued Revenue', offsetAccountId: '2', offsetAccountCode: '4100', offsetAccountName: 'Service Revenue', accrualDate: new Date('2024-01-31'), reversalDate: new Date('2024-02-01'), amount: 12500, description: 'January service revenue accrual', status: ApprovalStatus.Approved, periodId: '1', periodName: 'January 2024', isReversed: true, isActive: true },
        { id: '3', accrualNumber: 'ACC-003', accrualType: AccrualType.Expense, accountId: '1', accountCode: '2100', accountName: 'Accrued Expenses', offsetAccountId: '1', offsetAccountCode: '6200', offsetAccountName: 'Payroll Expense', accrualDate: new Date('2024-02-28'), reversalDate: new Date('2024-03-01'), amount: 3500, description: 'February payroll accrual', status: ApprovalStatus.Pending, periodId: '2', periodName: 'February 2024', isReversed: false, isActive: true }
      ];
      const mockAccounts: ChartOfAccount[] = [
        { id: '1', accountCode: '2100', accountName: 'Accrued Expenses', accountType: AccountType.Liability, normalBalance: 'Credit', isActive: true, currentBalance: 0, isHeader: false, isBankAccount: false, level: 0, sortOrder: 0 },
        { id: '2', accountCode: '1300', accountName: 'Accrued Revenue', accountType: AccountType.Asset, normalBalance: 'Debit', isActive: true, currentBalance: 0, isHeader: false, isBankAccount: false, level: 0, sortOrder: 0 }
      ];
      await new Promise(r => setTimeout(r, 300));
      this.accruals.set(mockAccruals);
      this.accounts.set(mockAccounts);
      this.totalAccruals.set(mockAccruals.length);
    } finally { this.isLoading.set(false); }
  }

  onSearch(event: Event): void { this.searchQuery.set((event.target as HTMLInputElement).value); }
  onTypeChange(value: string): void { this.selectedType.set(value); }
  onStatusChange(value: string): void { this.selectedStatus.set(value); }

  openAccrualDialog(accrual?: Accrual): void {
    this.editingAccrual.set(accrual || null);
    if (accrual) this.accrualForm.patchValue(accrual);
    else this.accrualForm.reset({ accrualType: AccrualType.Expense, accrualDate: new Date(), reversalDate: new Date(), amount: 0 });
    this.dialog.open(this.accrualDialogTemplate, { width: '500px' });
  }

  async saveAccrual(): Promise<void> {
    if (this.accrualForm.invalid) return;
    this.isSaving.set(true);
    try {
      await new Promise(r => setTimeout(r, 500));
      this.notification.success('Accrual saved');
      this.dialog.closeAll();
      this.loadData();
    } finally { this.isSaving.set(false); }
  }

  viewAccrual(accrual: Accrual): void {}
  async submitForApproval(accrual: Accrual): Promise<void> { this.notification.success('Submitted for approval'); }
  async reverseAccrual(accrual: Accrual): Promise<void> { this.notification.success('Accrual reversed'); this.loadData(); }
}

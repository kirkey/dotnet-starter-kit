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
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { NotificationService } from '@core/services/notification.service';
import { PrepaidExpense, ChartOfAccount, AccountType } from '@core/models/accounting.model';

@Component({
  selector: 'app-prepaid-expenses',
  standalone: true,
  imports: [
    CommonModule, CurrencyPipe, DatePipe, ReactiveFormsModule,
    MatTableModule, MatPaginatorModule, MatSortModule, MatButtonModule, MatIconModule,
    MatFormFieldModule, MatInputModule, MatSelectModule, MatDialogModule,
    MatProgressSpinnerModule, MatChipsModule, MatMenuModule, MatTooltipModule,
    MatCardModule, MatDatepickerModule, MatNativeDateModule, MatProgressBarModule, PageHeaderComponent
  ],
  template: `
    <div class="prepaid-container">
      <app-page-header 
        title="Prepaid Expenses" 
        subtitle="Manage prepaid expenses and their amortization schedules"
        icon="event_available">
      </app-page-header>

      <div class="toolbar">
        <div class="filters">
          <mat-form-field appearance="outline" class="search-field">
            <mat-label>Search prepaid expenses</mat-label>
            <input matInput [value]="searchQuery()" (input)="onSearch($event)" placeholder="Search...">
            <mat-icon matPrefix>search</mat-icon>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Status</mat-label>
            <mat-select [value]="selectedStatus()" (selectionChange)="onStatusChange($event.value)">
              <mat-option value="">All</mat-option>
              <mat-option value="active">Active</mat-option>
              <mat-option value="fullyAmortized">Fully Amortized</mat-option>
            </mat-select>
          </mat-form-field>
        </div>

        <button mat-raised-button color="primary" (click)="openPrepaidDialog()">
          <mat-icon>add</mat-icon>
          New Prepaid Expense
        </button>
      </div>

      <div class="summary-cards">
        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon total">
              <mat-icon>account_balance_wallet</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Total Prepaid Balance</span>
              <span class="summary-value">{{ totalBalance() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon amortized">
              <mat-icon>trending_down</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Amortized This Month</span>
              <span class="summary-value">{{ amortizedThisMonth() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon active">
              <mat-icon>event_repeat</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Active Schedules</span>
              <span class="summary-value">{{ activeCount() }}</span>
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
          <table mat-table [dataSource]="filteredPrepaids()" matSort>
            <ng-container matColumnDef="prepaidNumber">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Prepaid #</th>
              <td mat-cell *matCellDef="let prepaid">
                <span class="prepaid-number">{{ prepaid.prepaidNumber }}</span>
              </td>
            </ng-container>

            <ng-container matColumnDef="description">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Description</th>
              <td mat-cell *matCellDef="let prepaid">
                <div class="prepaid-desc">{{ prepaid.description }}</div>
                <div class="prepaid-vendor">{{ prepaid.vendorName }}</div>
              </td>
            </ng-container>

            <ng-container matColumnDef="originalAmount">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Original Amount</th>
              <td mat-cell *matCellDef="let prepaid">{{ prepaid.originalAmount | currency }}</td>
            </ng-container>

            <ng-container matColumnDef="amortized">
              <th mat-header-cell *matHeaderCellDef>Amortized</th>
              <td mat-cell *matCellDef="let prepaid">{{ prepaid.amortizedAmount | currency }}</td>
            </ng-container>

            <ng-container matColumnDef="balance">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Balance</th>
              <td mat-cell *matCellDef="let prepaid">{{ prepaid.remainingBalance | currency }}</td>
            </ng-container>

            <ng-container matColumnDef="progress">
              <th mat-header-cell *matHeaderCellDef>Progress</th>
              <td mat-cell *matCellDef="let prepaid">
                <div class="progress-cell">
                  <mat-progress-bar mode="determinate" [value]="getAmortizationProgress(prepaid)"></mat-progress-bar>
                  <span class="progress-text">{{ getAmortizationProgress(prepaid) | number:'1.0-0' }}%</span>
                </div>
              </td>
            </ng-container>

            <ng-container matColumnDef="dates">
              <th mat-header-cell *matHeaderCellDef>Period</th>
              <td mat-cell *matCellDef="let prepaid">
                <div class="date-range">{{ prepaid.startDate | date:'shortDate' }} - {{ prepaid.endDate | date:'shortDate' }}</div>
              </td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef></th>
              <td mat-cell *matCellDef="let prepaid">
                <button mat-icon-button [matMenuTriggerFor]="menu">
                  <mat-icon>more_vert</mat-icon>
                </button>
                <mat-menu #menu="matMenu">
                  <button mat-menu-item (click)="viewPrepaid(prepaid)">
                    <mat-icon>visibility</mat-icon>
                    <span>View Details</span>
                  </button>
                  <button mat-menu-item (click)="viewSchedule(prepaid)">
                    <mat-icon>schedule</mat-icon>
                    <span>View Schedule</span>
                  </button>
                  @if (prepaid.remainingBalance > 0) {
                    <button mat-menu-item (click)="openPrepaidDialog(prepaid)">
                      <mat-icon>edit</mat-icon>
                      <span>Edit</span>
                    </button>
                    <button mat-menu-item (click)="processAmortization(prepaid)">
                      <mat-icon>play_arrow</mat-icon>
                      <span>Process Amortization</span>
                    </button>
                  }
                </mat-menu>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns;" [class.fully-amortized]="row.remainingBalance === 0"></tr>
          </table>

          <mat-paginator [length]="totalPrepaids()" [pageSize]="pageSize()" [pageSizeOptions]="[10, 25, 50]" showFirstLastButtons></mat-paginator>
        </div>
      }
    </div>

    <ng-template #prepaidDialog>
      <h2 mat-dialog-title>{{ editingPrepaid() ? 'Edit Prepaid Expense' : 'New Prepaid Expense' }}</h2>
      <mat-dialog-content>
        <form [formGroup]="prepaidForm" class="prepaid-form">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Description</mat-label>
            <input matInput formControlName="description" required>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Prepaid Account</mat-label>
            <mat-select formControlName="prepaidAccountId" required>
              @for (account of prepaidAccounts(); track account.id) {
                <mat-option [value]="account.id">{{ account.accountCode }} - {{ account.accountName }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Expense Account</mat-label>
            <mat-select formControlName="expenseAccountId" required>
              @for (account of expenseAccounts(); track account.id) {
                <mat-option [value]="account.id">{{ account.accountCode }} - {{ account.accountName }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Original Amount</mat-label>
            <input matInput type="number" formControlName="originalAmount" required>
            <span matPrefix>$&nbsp;</span>
          </mat-form-field>

          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Start Date</mat-label>
              <input matInput [matDatepicker]="startPicker" formControlName="startDate" required>
              <mat-datepicker-toggle matIconSuffix [for]="startPicker"></mat-datepicker-toggle>
              <mat-datepicker #startPicker></mat-datepicker>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>End Date</mat-label>
              <input matInput [matDatepicker]="endPicker" formControlName="endDate" required>
              <mat-datepicker-toggle matIconSuffix [for]="endPicker"></mat-datepicker-toggle>
              <mat-datepicker #endPicker></mat-datepicker>
            </mat-form-field>
          </div>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Amortization Periods</mat-label>
            <input matInput type="number" formControlName="amortizationPeriods" required>
            <span matSuffix>months</span>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Notes</mat-label>
            <textarea matInput formControlName="notes" rows="2"></textarea>
          </mat-form-field>
        </form>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button mat-dialog-close>Cancel</button>
        <button mat-raised-button color="primary" [disabled]="prepaidForm.invalid || isSaving()" (click)="savePrepaid()">
          @if (isSaving()) { <mat-spinner diameter="20"></mat-spinner> }
          Save
        </button>
      </mat-dialog-actions>
    </ng-template>
  `,
  styles: [`
    .prepaid-container { padding: 24px; }
    .toolbar { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 24px; gap: 16px; flex-wrap: wrap; }
    .filters { display: flex; gap: 12px; flex-wrap: wrap; flex: 1; }
    .search-field { min-width: 250px; flex: 1; }
    .filter-field { min-width: 150px; }
    .summary-cards { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 16px; margin-bottom: 24px; }
    .summary-card mat-card-content { display: flex; align-items: center; gap: 16px; padding: 16px !important; }
    .summary-icon { width: 48px; height: 48px; border-radius: 12px; display: flex; align-items: center; justify-content: center; }
    .summary-icon.total { background: #dbeafe; color: #2563eb; }
    .summary-icon.amortized { background: #dcfce7; color: #16a34a; }
    .summary-icon.active { background: #e0e7ff; color: #4f46e5; }
    .summary-info { display: flex; flex-direction: column; }
    .summary-label { font-size: 12px; color: var(--text-secondary); }
    .summary-value { font-size: 20px; font-weight: 600; }
    .loading-container { display: flex; justify-content: center; padding: 48px; }
    .table-container { background: var(--surface-color); border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1); }
    table { width: 100%; }
    .prepaid-number { font-weight: 500; color: var(--primary-color); }
    .prepaid-desc { font-weight: 500; }
    .prepaid-vendor { font-size: 12px; color: var(--text-secondary); }
    .progress-cell { display: flex; align-items: center; gap: 8px; min-width: 120px; }
    .progress-text { font-size: 12px; min-width: 40px; }
    .date-range { font-size: 13px; }
    tr.fully-amortized { background: #f0fdf4; }
    .prepaid-form { display: flex; flex-direction: column; gap: 16px; min-width: 450px; }
    .form-row { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
    .full-width { width: 100%; }
  `]
})
export class PrepaidExpensesComponent implements OnInit {
  @ViewChild('prepaidDialog') prepaidDialogTemplate!: TemplateRef<any>;

  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private notification = inject(NotificationService);

  prepaids = signal<PrepaidExpense[]>([]);
  prepaidAccounts = signal<ChartOfAccount[]>([]);
  expenseAccounts = signal<ChartOfAccount[]>([]);
  isLoading = signal(false);
  isSaving = signal(false);
  searchQuery = signal('');
  selectedStatus = signal('');
  editingPrepaid = signal<PrepaidExpense | null>(null);

  pageSize = signal(10);
  totalPrepaids = signal(0);

  displayedColumns = ['prepaidNumber', 'description', 'originalAmount', 'amortized', 'balance', 'progress', 'dates', 'actions'];

  prepaidForm: FormGroup = this.fb.group({
    description: ['', Validators.required],
    prepaidAccountId: ['', Validators.required],
    expenseAccountId: ['', Validators.required],
    originalAmount: [0, [Validators.required, Validators.min(0.01)]],
    startDate: [new Date(), Validators.required],
    endDate: [new Date(), Validators.required],
    amortizationPeriods: [12, [Validators.required, Validators.min(1)]],
    notes: ['']
  });

  filteredPrepaids = computed(() => {
    let result = this.prepaids();
    const query = this.searchQuery().toLowerCase();
    if (query) result = result.filter(p => p.prepaidNumber.toLowerCase().includes(query) || p.description.toLowerCase().includes(query));
    if (this.selectedStatus() === 'active') result = result.filter(p => p.remainingAmount > 0);
    if (this.selectedStatus() === 'fullyAmortized') result = result.filter(p => p.remainingAmount === 0);
    return result;
  });

  totalBalance = computed(() => this.prepaids().reduce((sum, p) => sum + p.remainingAmount, 0));
  amortizedThisMonth = computed(() => this.prepaids().reduce((sum, p) => sum + p.amortizationPerPeriod, 0));
  activeCount = computed(() => this.prepaids().filter(p => p.remainingAmount > 0).length);

  ngOnInit(): void { this.loadData(); }

  getAmortizationProgress(prepaid: PrepaidExpense): number {
    return (prepaid.amortizedAmount / prepaid.originalAmount) * 100;
  }

  async loadData(): Promise<void> {
    this.isLoading.set(true);
    try {
      const mockPrepaids: PrepaidExpense[] = [
        { id: '1', prepaidNumber: 'PP-001', description: 'Annual Insurance Premium', vendorName: 'ABC Insurance', prepaidAccountId: '1', prepaidAccountCode: '1400', prepaidAccountName: 'Prepaid Expenses', expenseAccountId: '2', expenseAccountCode: '6100', expenseAccountName: 'Insurance Expense', originalAmount: 24000, amortizedAmount: 8000, remainingAmount: 16000, amortizationPerPeriod: 2000, startDate: new Date('2024-01-01'), endDate: new Date('2024-12-31'), amortizationPeriods: 12, status: 'Active', isActive: true, amortizationSchedule: [] },
        { id: '2', prepaidNumber: 'PP-002', description: 'Software License (3-year)', vendorName: 'Tech Vendor', prepaidAccountId: '1', prepaidAccountCode: '1400', prepaidAccountName: 'Prepaid Expenses', expenseAccountId: '3', expenseAccountCode: '6200', expenseAccountName: 'Software Expense', originalAmount: 36000, amortizedAmount: 12000, remainingAmount: 24000, amortizationPerPeriod: 1000, startDate: new Date('2023-07-01'), endDate: new Date('2026-06-30'), amortizationPeriods: 36, status: 'Active', isActive: true, amortizationSchedule: [] },
        { id: '3', prepaidNumber: 'PP-003', description: 'Rent Deposit', vendorName: 'Property Mgmt', prepaidAccountId: '1', prepaidAccountCode: '1400', prepaidAccountName: 'Prepaid Expenses', expenseAccountId: '4', expenseAccountCode: '6300', expenseAccountName: 'Rent Expense', originalAmount: 15000, amortizedAmount: 15000, remainingAmount: 0, amortizationPerPeriod: 0, startDate: new Date('2023-01-01'), endDate: new Date('2023-12-31'), amortizationPeriods: 12, status: 'FullyAmortized', isActive: true, amortizationSchedule: [] }
      ];
      const mockPrepaidAccounts: ChartOfAccount[] = [
        { id: '1', accountCode: '1400', accountName: 'Prepaid Expenses', accountType: AccountType.Asset, normalBalance: 'Debit', isActive: true, currentBalance: 0, isHeader: false, isBankAccount: false, level: 1, sortOrder: 1 }
      ];
      const mockExpenseAccounts: ChartOfAccount[] = [
        { id: '2', accountCode: '6100', accountName: 'Insurance Expense', accountType: AccountType.Expense, normalBalance: 'Debit', isActive: true, currentBalance: 0, isHeader: false, isBankAccount: false, level: 1, sortOrder: 1 },
        { id: '3', accountCode: '6200', accountName: 'Software Expense', accountType: AccountType.Expense, normalBalance: 'Debit', isActive: true, currentBalance: 0, isHeader: false, isBankAccount: false, level: 1, sortOrder: 1 },
        { id: '4', accountCode: '6300', accountName: 'Rent Expense', accountType: AccountType.Expense, normalBalance: 'Debit', isActive: true, currentBalance: 0, isHeader: false, isBankAccount: false, level: 1, sortOrder: 1 }
      ];
      await new Promise(r => setTimeout(r, 300));
      this.prepaids.set(mockPrepaids);
      this.prepaidAccounts.set(mockPrepaidAccounts);
      this.expenseAccounts.set(mockExpenseAccounts);
      this.totalPrepaids.set(mockPrepaids.length);
    } finally { this.isLoading.set(false); }
  }

  onSearch(event: Event): void { this.searchQuery.set((event.target as HTMLInputElement).value); }
  onStatusChange(value: string): void { this.selectedStatus.set(value); }

  openPrepaidDialog(prepaid?: PrepaidExpense): void {
    this.editingPrepaid.set(prepaid || null);
    if (prepaid) this.prepaidForm.patchValue(prepaid);
    else this.prepaidForm.reset({ startDate: new Date(), endDate: new Date(), originalAmount: 0, amortizationPeriods: 12 });
    this.dialog.open(this.prepaidDialogTemplate, { width: '500px' });
  }

  async savePrepaid(): Promise<void> {
    if (this.prepaidForm.invalid) return;
    this.isSaving.set(true);
    try {
      await new Promise(r => setTimeout(r, 500));
      this.notification.success('Prepaid expense saved');
      this.dialog.closeAll();
      this.loadData();
    } finally { this.isSaving.set(false); }
  }

  viewPrepaid(prepaid: PrepaidExpense): void {}
  viewSchedule(prepaid: PrepaidExpense): void {}
  async processAmortization(prepaid: PrepaidExpense): Promise<void> { this.notification.success('Amortization processed'); this.loadData(); }
}

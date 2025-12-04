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
import { DeferredRevenue, ChartOfAccount, Customer, AccountType } from '@core/models/accounting.model';

@Component({
  selector: 'app-deferred-revenue',
  standalone: true,
  imports: [
    CommonModule, CurrencyPipe, DatePipe, ReactiveFormsModule,
    MatTableModule, MatPaginatorModule, MatSortModule, MatButtonModule, MatIconModule,
    MatFormFieldModule, MatInputModule, MatSelectModule, MatDialogModule,
    MatProgressSpinnerModule, MatChipsModule, MatMenuModule, MatTooltipModule,
    MatCardModule, MatDatepickerModule, MatNativeDateModule, MatProgressBarModule, PageHeaderComponent
  ],
  template: `
    <div class="deferred-container">
      <app-page-header 
        title="Deferred Revenue" 
        subtitle="Manage deferred revenue and revenue recognition schedules"
        icon="event_busy">
      </app-page-header>

      <div class="toolbar">
        <div class="filters">
          <mat-form-field appearance="outline" class="search-field">
            <mat-label>Search deferred revenue</mat-label>
            <input matInput [value]="searchQuery()" (input)="onSearch($event)" placeholder="Search...">
            <mat-icon matPrefix>search</mat-icon>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Customer</mat-label>
            <mat-select [value]="selectedCustomer()" (selectionChange)="onCustomerChange($event.value)">
              <mat-option value="">All Customers</mat-option>
              @for (customer of customers(); track customer.id) {
                <mat-option [value]="customer.id">{{ customer.customerName }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Status</mat-label>
            <mat-select [value]="selectedStatus()" (selectionChange)="onStatusChange($event.value)">
              <mat-option value="">All</mat-option>
              <mat-option value="active">Active</mat-option>
              <mat-option value="fullyRecognized">Fully Recognized</mat-option>
            </mat-select>
          </mat-form-field>
        </div>

        <button mat-raised-button color="primary" (click)="openDeferredDialog()">
          <mat-icon>add</mat-icon>
          New Deferred Revenue
        </button>
      </div>

      <div class="summary-cards">
        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon total">
              <mat-icon>account_balance</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Total Deferred</span>
              <span class="summary-value">{{ totalDeferred() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon recognized">
              <mat-icon>trending_up</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Recognized This Month</span>
              <span class="summary-value">{{ recognizedThisMonth() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon active">
              <mat-icon>schedule</mat-icon>
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
          <table mat-table [dataSource]="filteredDeferred()" matSort>
            <ng-container matColumnDef="deferredNumber">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Deferred #</th>
              <td mat-cell *matCellDef="let deferred">
                <span class="deferred-number">{{ deferred.deferredNumber }}</span>
              </td>
            </ng-container>

            <ng-container matColumnDef="customer">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Customer</th>
              <td mat-cell *matCellDef="let deferred">
                <div class="customer-name">{{ deferred.customerName }}</div>
                <div class="contract-ref">{{ deferred.contractReference }}</div>
              </td>
            </ng-container>

            <ng-container matColumnDef="description">
              <th mat-header-cell *matHeaderCellDef>Description</th>
              <td mat-cell *matCellDef="let deferred">{{ deferred.description }}</td>
            </ng-container>

            <ng-container matColumnDef="originalAmount">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Original Amount</th>
              <td mat-cell *matCellDef="let deferred">{{ deferred.originalAmount | currency }}</td>
            </ng-container>

            <ng-container matColumnDef="recognized">
              <th mat-header-cell *matHeaderCellDef>Recognized</th>
              <td mat-cell *matCellDef="let deferred">{{ deferred.recognizedAmount | currency }}</td>
            </ng-container>

            <ng-container matColumnDef="balance">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Balance</th>
              <td mat-cell *matCellDef="let deferred">{{ deferred.remainingAmount | currency }}</td>
            </ng-container>

            <ng-container matColumnDef="progress">
              <th mat-header-cell *matHeaderCellDef>Progress</th>
              <td mat-cell *matCellDef="let deferred">
                <div class="progress-cell">
                  <mat-progress-bar mode="determinate" [value]="getRecognitionProgress(deferred)"></mat-progress-bar>
                  <span class="progress-text">{{ getRecognitionProgress(deferred) | number:'1.0-0' }}%</span>
                </div>
              </td>
            </ng-container>

            <ng-container matColumnDef="dates">
              <th mat-header-cell *matHeaderCellDef>Period</th>
              <td mat-cell *matCellDef="let deferred">
                <div class="date-range">{{ deferred.startDate | date:'shortDate' }} - {{ deferred.endDate | date:'shortDate' }}</div>
              </td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef></th>
              <td mat-cell *matCellDef="let deferred">
                <button mat-icon-button [matMenuTriggerFor]="menu">
                  <mat-icon>more_vert</mat-icon>
                </button>
                <mat-menu #menu="matMenu">
                  <button mat-menu-item (click)="viewDeferred(deferred)">
                    <mat-icon>visibility</mat-icon>
                    <span>View Details</span>
                  </button>
                  <button mat-menu-item (click)="viewSchedule(deferred)">
                    <mat-icon>schedule</mat-icon>
                    <span>View Schedule</span>
                  </button>
                  @if (deferred.remainingAmount > 0) {
                    <button mat-menu-item (click)="openDeferredDialog(deferred)">
                      <mat-icon>edit</mat-icon>
                      <span>Edit</span>
                    </button>
                    <button mat-menu-item (click)="processRecognition(deferred)">
                      <mat-icon>play_arrow</mat-icon>
                      <span>Process Recognition</span>
                    </button>
                  }
                </mat-menu>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns;" [class.fully-recognized]="row.remainingAmount === 0"></tr>
          </table>

          <mat-paginator [length]="totalDeferred()" [pageSize]="pageSize()" [pageSizeOptions]="[10, 25, 50]" showFirstLastButtons></mat-paginator>
        </div>
      }
    </div>

    <ng-template #deferredDialog>
      <h2 mat-dialog-title>{{ editingDeferred() ? 'Edit Deferred Revenue' : 'New Deferred Revenue' }}</h2>
      <mat-dialog-content>
        <form [formGroup]="deferredForm" class="deferred-form">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Customer</mat-label>
            <mat-select formControlName="customerId" required>
              @for (customer of customers(); track customer.id) {
                <mat-option [value]="customer.id">{{ customer.customerName }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Contract Reference</mat-label>
            <input matInput formControlName="contractReference">
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Description</mat-label>
            <input matInput formControlName="description" required>
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
            <mat-label>Recognition Periods</mat-label>
            <input matInput type="number" formControlName="recognitionPeriods" required>
            <span matSuffix>months</span>
          </mat-form-field>

          <div class="form-row">
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Deferred Revenue Account</mat-label>
              <mat-select formControlName="deferredAccountId" required>
                @for (account of deferredAccounts(); track account.id) {
                  <mat-option [value]="account.id">{{ account.accountCode }} - {{ account.accountName }}</mat-option>
                }
              </mat-select>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Revenue Account</mat-label>
              <mat-select formControlName="revenueAccountId" required>
                @for (account of revenueAccounts(); track account.id) {
                  <mat-option [value]="account.id">{{ account.accountCode }} - {{ account.accountName }}</mat-option>
                }
              </mat-select>
            </mat-form-field>
          </div>
        </form>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button mat-dialog-close>Cancel</button>
        <button mat-raised-button color="primary" [disabled]="deferredForm.invalid || isSaving()" (click)="saveDeferred()">
          @if (isSaving()) { <mat-spinner diameter="20"></mat-spinner> }
          Save
        </button>
      </mat-dialog-actions>
    </ng-template>
  `,
  styles: [`
    .deferred-container { padding: 24px; }
    .toolbar { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 24px; gap: 16px; flex-wrap: wrap; }
    .filters { display: flex; gap: 12px; flex-wrap: wrap; flex: 1; }
    .search-field { min-width: 250px; flex: 1; }
    .filter-field { min-width: 150px; }
    .summary-cards { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 16px; margin-bottom: 24px; }
    .summary-card mat-card-content { display: flex; align-items: center; gap: 16px; padding: 16px !important; }
    .summary-icon { width: 48px; height: 48px; border-radius: 12px; display: flex; align-items: center; justify-content: center; }
    .summary-icon.total { background: #e0e7ff; color: #4f46e5; }
    .summary-icon.recognized { background: #dcfce7; color: #16a34a; }
    .summary-icon.active { background: #dbeafe; color: #2563eb; }
    .summary-info { display: flex; flex-direction: column; }
    .summary-label { font-size: 12px; color: var(--text-secondary); }
    .summary-value { font-size: 20px; font-weight: 600; }
    .loading-container { display: flex; justify-content: center; padding: 48px; }
    .table-container { background: var(--surface-color); border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1); }
    table { width: 100%; }
    .deferred-number { font-weight: 500; color: var(--primary-color); }
    .customer-name { font-weight: 500; }
    .contract-ref { font-size: 12px; color: var(--text-secondary); }
    .progress-cell { display: flex; align-items: center; gap: 8px; min-width: 120px; }
    .progress-text { font-size: 12px; min-width: 40px; }
    .date-range { font-size: 13px; }
    tr.fully-recognized { background: #f0fdf4; }
    .deferred-form { display: flex; flex-direction: column; gap: 16px; min-width: 500px; }
    .form-row { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
    .full-width { width: 100%; }
  `]
})
export class DeferredRevenueComponent implements OnInit {
  @ViewChild('deferredDialog') deferredDialogTemplate!: TemplateRef<any>;

  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private notification = inject(NotificationService);

  deferredRevenues = signal<DeferredRevenue[]>([]);
  customers = signal<Customer[]>([]);
  deferredAccounts = signal<ChartOfAccount[]>([]);
  revenueAccounts = signal<ChartOfAccount[]>([]);
  isLoading = signal(false);
  isSaving = signal(false);
  searchQuery = signal('');
  selectedCustomer = signal('');
  selectedStatus = signal('');
  editingDeferred = signal<DeferredRevenue | null>(null);

  pageSize = signal(10);
  totalDeferredCount = signal(0);

  displayedColumns = ['deferredNumber', 'customer', 'description', 'originalAmount', 'recognized', 'balance', 'progress', 'dates', 'actions'];

  deferredForm: FormGroup = this.fb.group({
    customerId: ['', Validators.required],
    contractReference: [''],
    description: ['', Validators.required],
    originalAmount: [0, [Validators.required, Validators.min(0.01)]],
    startDate: [new Date(), Validators.required],
    endDate: [new Date(), Validators.required],
    recognitionPeriods: [12, [Validators.required, Validators.min(1)]],
    deferredAccountId: ['', Validators.required],
    revenueAccountId: ['', Validators.required]
  });

  filteredDeferred = computed(() => {
    let result = this.deferredRevenues();
    const query = this.searchQuery().toLowerCase();
    if (query) result = result.filter(d => d.deferredNumber.toLowerCase().includes(query) || (d.customerName && d.customerName.toLowerCase().includes(query)));
    if (this.selectedCustomer()) result = result.filter(d => d.customerId === this.selectedCustomer());
    if (this.selectedStatus() === 'active') result = result.filter(d => d.remainingAmount > 0);
    if (this.selectedStatus() === 'fullyRecognized') result = result.filter(d => d.remainingAmount === 0);
    return result;
  });

  totalDeferred = computed(() => this.deferredRevenues().reduce((sum, d) => sum + d.remainingAmount, 0));
  recognizedThisMonth = computed(() => this.deferredRevenues().reduce((sum, d) => sum + d.recognitionPerPeriod, 0));
  activeCount = computed(() => this.deferredRevenues().filter(d => d.remainingAmount > 0).length);

  ngOnInit(): void { this.loadData(); }

  getRecognitionProgress(deferred: DeferredRevenue): number {
    return (deferred.recognizedAmount / deferred.originalAmount) * 100;
  }

  async loadData(): Promise<void> {
    this.isLoading.set(true);
    try {
      const mockDeferred: DeferredRevenue[] = [
        { id: '1', deferredNumber: 'DR-001', customerId: '1', customerName: 'Acme Corp', description: 'Annual Support Contract', deferredAccountId: '1', deferredAccountCode: '2400', deferredAccountName: 'Deferred Revenue', revenueAccountId: '2', revenueAccountCode: '4100', revenueAccountName: 'Service Revenue', originalAmount: 60000, recognizedAmount: 25000, remainingAmount: 35000, recognitionPerPeriod: 5000, startDate: new Date('2024-01-01'), endDate: new Date('2024-12-31'), recognitionPeriods: 12, status: 'Active', isActive: true, recognitionSchedule: [] },
        { id: '2', deferredNumber: 'DR-002', customerId: '2', customerName: 'Tech Solutions', description: 'SaaS Subscription', deferredAccountId: '1', deferredAccountCode: '2400', deferredAccountName: 'Deferred Revenue', revenueAccountId: '2', revenueAccountCode: '4100', revenueAccountName: 'Service Revenue', originalAmount: 24000, recognizedAmount: 8000, remainingAmount: 16000, recognitionPerPeriod: 2000, startDate: new Date('2024-01-01'), endDate: new Date('2024-12-31'), recognitionPeriods: 12, status: 'Active', isActive: true, recognitionSchedule: [] }
      ];
      const mockCustomers: Customer[] = [
        { id: '1', customerNumber: 'C001', customerName: 'Acme Corp', creditLimit: 100000, currentBalance: 45000, creditHold: false, isActive: true },
        { id: '2', customerNumber: 'C002', customerName: 'Tech Solutions', creditLimit: 50000, currentBalance: 25000, creditHold: false, isActive: true }
      ];
      const mockDeferredAccounts: ChartOfAccount[] = [
        { id: '1', accountCode: '2400', accountName: 'Deferred Revenue', accountType: AccountType.Liability, normalBalance: 'Credit', isActive: true, currentBalance: 0, isHeader: false, isBankAccount: false, level: 1, sortOrder: 1 }
      ];
      const mockRevenueAccounts: ChartOfAccount[] = [
        { id: '2', accountCode: '4100', accountName: 'Service Revenue', accountType: AccountType.Revenue, normalBalance: 'Credit', isActive: true, currentBalance: 0, isHeader: false, isBankAccount: false, level: 1, sortOrder: 1 }
      ];
      await new Promise(r => setTimeout(r, 300));
      this.deferredRevenues.set(mockDeferred);
      this.customers.set(mockCustomers);
      this.deferredAccounts.set(mockDeferredAccounts);
      this.revenueAccounts.set(mockRevenueAccounts);
      this.totalDeferredCount.set(mockDeferred.length);
    } finally { this.isLoading.set(false); }
  }

  onSearch(event: Event): void { this.searchQuery.set((event.target as HTMLInputElement).value); }
  onCustomerChange(value: string): void { this.selectedCustomer.set(value); }
  onStatusChange(value: string): void { this.selectedStatus.set(value); }

  openDeferredDialog(deferred?: DeferredRevenue): void {
    this.editingDeferred.set(deferred || null);
    if (deferred) this.deferredForm.patchValue(deferred);
    else this.deferredForm.reset({ startDate: new Date(), endDate: new Date(), originalAmount: 0, recognitionPeriods: 12 });
    this.dialog.open(this.deferredDialogTemplate, { width: '600px' });
  }

  async saveDeferred(): Promise<void> {
    if (this.deferredForm.invalid) return;
    this.isSaving.set(true);
    try {
      await new Promise(r => setTimeout(r, 500));
      this.notification.success('Deferred revenue saved');
      this.dialog.closeAll();
      this.loadData();
    } finally { this.isSaving.set(false); }
  }

  viewDeferred(deferred: DeferredRevenue): void {}
  viewSchedule(deferred: DeferredRevenue): void {}
  async processRecognition(deferred: DeferredRevenue): Promise<void> { this.notification.success('Recognition processed'); this.loadData(); }
}

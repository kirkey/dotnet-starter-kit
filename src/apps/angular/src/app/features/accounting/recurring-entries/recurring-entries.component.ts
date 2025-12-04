import { Component, inject, signal, OnInit, ViewChild, TemplateRef, computed } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
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
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { NotificationService } from '@core/services/notification.service';
import { RecurringJournalEntry, FrequencyType, ChartOfAccount, AccountType } from '@core/models/accounting.model';

@Component({
  selector: 'app-recurring-entries',
  standalone: true,
  imports: [
    CommonModule, CurrencyPipe, DatePipe, ReactiveFormsModule,
    MatTableModule, MatPaginatorModule, MatSortModule, MatButtonModule, MatIconModule,
    MatFormFieldModule, MatInputModule, MatSelectModule, MatDialogModule,
    MatProgressSpinnerModule, MatChipsModule, MatMenuModule, MatTooltipModule,
    MatCardModule, MatSlideToggleModule, PageHeaderComponent
  ],
  template: `
    <div class="recurring-container">
      <app-page-header 
        title="Recurring Entries" 
        subtitle="Manage recurring journal entries and automated postings"
        icon="repeat">
      </app-page-header>

      <div class="toolbar">
        <div class="filters">
          <mat-form-field appearance="outline" class="search-field">
            <mat-label>Search recurring entries</mat-label>
            <input matInput [value]="searchQuery()" (input)="onSearch($event)" placeholder="Search...">
            <mat-icon matPrefix>search</mat-icon>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Frequency</mat-label>
            <mat-select [value]="selectedFrequency()" (selectionChange)="onFrequencyChange($event.value)">
              <mat-option value="">All</mat-option>
              <mat-option value="Daily">Daily</mat-option>
              <mat-option value="Weekly">Weekly</mat-option>
              <mat-option value="Monthly">Monthly</mat-option>
              <mat-option value="Quarterly">Quarterly</mat-option>
              <mat-option value="Yearly">Yearly</mat-option>
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Status</mat-label>
            <mat-select [value]="selectedStatus()" (selectionChange)="onStatusChange($event.value)">
              <mat-option value="">All</mat-option>
              <mat-option value="active">Active</mat-option>
              <mat-option value="paused">Paused</mat-option>
            </mat-select>
          </mat-form-field>
        </div>

        <button mat-raised-button color="primary" (click)="openEntryDialog()">
          <mat-icon>add</mat-icon>
          New Recurring Entry
        </button>
      </div>

      <div class="summary-cards">
        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon active">
              <mat-icon>play_circle</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Active Entries</span>
              <span class="summary-value">{{ activeCount() }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon due">
              <mat-icon>event</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Due This Month</span>
              <span class="summary-value">{{ dueThisMonth() }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon total">
              <mat-icon>attach_money</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Monthly Amount</span>
              <span class="summary-value">{{ monthlyTotal() | currency }}</span>
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
          <table mat-table [dataSource]="filteredEntries()" matSort>
            <ng-container matColumnDef="templateName">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Name</th>
              <td mat-cell *matCellDef="let entry">
                <span class="entry-name">{{ entry.templateName }}</span>
              </td>
            </ng-container>

            <ng-container matColumnDef="description">
              <th mat-header-cell *matHeaderCellDef>Description</th>
              <td mat-cell *matCellDef="let entry">{{ entry.description }}</td>
            </ng-container>

            <ng-container matColumnDef="frequency">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Frequency</th>
              <td mat-cell *matCellDef="let entry">
                <mat-chip [class]="'freq-' + entry.frequency.toLowerCase()">{{ entry.frequency }}</mat-chip>
              </td>
            </ng-container>

            <ng-container matColumnDef="amount">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Amount</th>
              <td mat-cell *matCellDef="let entry">{{ entry.amount | currency }}</td>
            </ng-container>

            <ng-container matColumnDef="lastRun">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Last Run</th>
              <td mat-cell *matCellDef="let entry">
                @if (entry.lastRunDate) {
                  {{ entry.lastRunDate | date:'mediumDate' }}
                } @else {
                  <span class="never-run">Never</span>
                }
              </td>
            </ng-container>

            <ng-container matColumnDef="nextRun">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Next Run</th>
              <td mat-cell *matCellDef="let entry">
                <span [class.upcoming]="isUpcoming(entry.nextRunDate)">{{ entry.nextRunDate | date:'mediumDate' }}</span>
              </td>
            </ng-container>

            <ng-container matColumnDef="status">
              <th mat-header-cell *matHeaderCellDef>Status</th>
              <td mat-cell *matCellDef="let entry">
                @if (entry.isActive) {
                  <mat-chip class="status-active">Active</mat-chip>
                } @else {
                  <mat-chip class="status-paused">Paused</mat-chip>
                }
              </td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef></th>
              <td mat-cell *matCellDef="let entry">
                <button mat-icon-button [matMenuTriggerFor]="menu">
                  <mat-icon>more_vert</mat-icon>
                </button>
                <mat-menu #menu="matMenu">
                  <button mat-menu-item (click)="viewEntry(entry)">
                    <mat-icon>visibility</mat-icon>
                    <span>View</span>
                  </button>
                  <button mat-menu-item (click)="openEntryDialog(entry)">
                    <mat-icon>edit</mat-icon>
                    <span>Edit</span>
                  </button>
                  @if (entry.isActive) {
                    <button mat-menu-item (click)="runNow(entry)">
                      <mat-icon>play_arrow</mat-icon>
                      <span>Run Now</span>
                    </button>
                    <button mat-menu-item (click)="pauseEntry(entry)">
                      <mat-icon>pause</mat-icon>
                      <span>Pause</span>
                    </button>
                  } @else {
                    <button mat-menu-item (click)="resumeEntry(entry)">
                      <mat-icon>play_arrow</mat-icon>
                      <span>Resume</span>
                    </button>
                  }
                  <button mat-menu-item (click)="viewHistory(entry)">
                    <mat-icon>history</mat-icon>
                    <span>View History</span>
                  </button>
                  <button mat-menu-item (click)="deleteEntry(entry)" class="delete-action">
                    <mat-icon color="warn">delete</mat-icon>
                    <span>Delete</span>
                  </button>
                </mat-menu>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns;" [class.paused]="!row.isActive"></tr>
          </table>

          <mat-paginator [length]="totalEntries()" [pageSize]="pageSize()" [pageSizeOptions]="[10, 25, 50]" showFirstLastButtons></mat-paginator>
        </div>
      }
    </div>

    <ng-template #entryDialog>
      <h2 mat-dialog-title>{{ editingEntry() ? 'Edit Recurring Entry' : 'New Recurring Entry' }}</h2>
      <mat-dialog-content>
        <form [formGroup]="entryForm" class="entry-form">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Entry Name</mat-label>
            <input matInput formControlName="templateName" required>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Description</mat-label>
            <textarea matInput formControlName="description" rows="2"></textarea>
          </mat-form-field>

          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Frequency</mat-label>
              <mat-select formControlName="frequency" required>
                <mat-option value="Daily">Daily</mat-option>
                <mat-option value="Weekly">Weekly</mat-option>
                <mat-option value="Monthly">Monthly</mat-option>
                <mat-option value="Quarterly">Quarterly</mat-option>
                <mat-option value="Yearly">Yearly</mat-option>
              </mat-select>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Amount</mat-label>
              <input matInput type="number" formControlName="amount" required>
              <span matPrefix>$&nbsp;</span>
            </mat-form-field>
          </div>

          <div class="lines-section">
            <div class="lines-header">
              <h3>Entry Lines</h3>
              <button mat-stroked-button type="button" (click)="addLine()">
                <mat-icon>add</mat-icon> Add Line
              </button>
            </div>

            @for (line of lines.controls; track $index; let i = $index) {
              <div class="line-row" [formGroupName]="i">
                <mat-form-field appearance="outline" class="account-field">
                  <mat-label>Account</mat-label>
                  <mat-select formControlName="accountId" required>
                    @for (account of accounts(); track account.id) {
                      <mat-option [value]="account.id">{{ account.accountCode }} - {{ account.accountName }}</mat-option>
                    }
                  </mat-select>
                </mat-form-field>

                <mat-form-field appearance="outline" class="amount-field">
                  <mat-label>Debit</mat-label>
                  <input matInput type="number" formControlName="debitAmount">
                </mat-form-field>

                <mat-form-field appearance="outline" class="amount-field">
                  <mat-label>Credit</mat-label>
                  <input matInput type="number" formControlName="creditAmount">
                </mat-form-field>

                <button mat-icon-button type="button" (click)="removeLine(i)" [disabled]="lines.length <= 2">
                  <mat-icon>close</mat-icon>
                </button>
              </div>
            }
          </div>

          <mat-slide-toggle formControlName="isActive">Active</mat-slide-toggle>
        </form>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button mat-dialog-close>Cancel</button>
        <button mat-raised-button color="primary" [disabled]="entryForm.invalid || isSaving()" (click)="saveEntry()">
          @if (isSaving()) { <mat-spinner diameter="20"></mat-spinner> }
          Save
        </button>
      </mat-dialog-actions>
    </ng-template>
  `,
  styles: [`
    .recurring-container { padding: 24px; }
    .toolbar { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 24px; gap: 16px; flex-wrap: wrap; }
    .filters { display: flex; gap: 12px; flex-wrap: wrap; flex: 1; }
    .search-field { min-width: 250px; flex: 1; }
    .filter-field { min-width: 150px; }
    .summary-cards { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 16px; margin-bottom: 24px; }
    .summary-card mat-card-content { display: flex; align-items: center; gap: 16px; padding: 16px !important; }
    .summary-icon { width: 48px; height: 48px; border-radius: 12px; display: flex; align-items: center; justify-content: center; }
    .summary-icon.active { background: #dcfce7; color: #16a34a; }
    .summary-icon.due { background: #dbeafe; color: #2563eb; }
    .summary-icon.total { background: #e0e7ff; color: #4f46e5; }
    .summary-info { display: flex; flex-direction: column; }
    .summary-label { font-size: 12px; color: var(--text-secondary); }
    .summary-value { font-size: 20px; font-weight: 600; }
    .loading-container { display: flex; justify-content: center; padding: 48px; }
    .table-container { background: var(--surface-color); border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1); }
    table { width: 100%; }
    .entry-name { font-weight: 500; color: var(--primary-color); }
    .never-run { color: #9ca3af; font-style: italic; }
    .upcoming { color: #2563eb; font-weight: 500; }
    .freq-daily { background: #fef3c7 !important; color: #92400e !important; }
    .freq-weekly { background: #dbeafe !important; color: #1d4ed8 !important; }
    .freq-monthly { background: #dcfce7 !important; color: #166534 !important; }
    .freq-quarterly { background: #e0e7ff !important; color: #4338ca !important; }
    .freq-yearly { background: #fce7f3 !important; color: #be185d !important; }
    .status-active { background: #dcfce7 !important; color: #166534 !important; }
    .status-paused { background: #f3f4f6 !important; color: #374151 !important; }
    tr.paused { opacity: 0.6; }
    .entry-form { display: flex; flex-direction: column; gap: 16px; min-width: 600px; }
    .form-row { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
    .full-width { width: 100%; }
    .lines-section { border: 1px solid #e5e7eb; border-radius: 8px; padding: 16px; }
    .lines-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 12px; }
    .lines-header h3 { margin: 0; font-size: 14px; }
    .line-row { display: grid; grid-template-columns: 2fr 1fr 1fr auto; gap: 8px; align-items: flex-start; margin-bottom: 8px; }
    .account-field { min-width: 200px; }
    .amount-field { min-width: 100px; }
    .delete-action { color: #dc2626; }
  `]
})
export class RecurringEntriesComponent implements OnInit {
  @ViewChild('entryDialog') entryDialogTemplate!: TemplateRef<any>;

  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private notification = inject(NotificationService);

  entries = signal<RecurringJournalEntry[]>([]);
  accounts = signal<ChartOfAccount[]>([]);
  isLoading = signal(false);
  isSaving = signal(false);
  searchQuery = signal('');
  selectedFrequency = signal('');
  selectedStatus = signal('');
  editingEntry = signal<RecurringJournalEntry | null>(null);

  pageSize = signal(10);
  totalEntries = signal(0);

  displayedColumns = ['templateName', 'description', 'frequency', 'amount', 'lastRun', 'nextRun', 'status', 'actions'];

  entryForm: FormGroup = this.fb.group({
    templateName: ['', Validators.required],
    description: [''],
    frequency: ['Monthly', Validators.required],
    amount: [0, [Validators.required, Validators.min(0.01)]],
    lines: this.fb.array([]),
    isActive: [true]
  });

  get lines(): FormArray { return this.entryForm.get('lines') as FormArray; }

  filteredEntries = computed(() => {
    let result = this.entries();
    const query = this.searchQuery().toLowerCase();
    if (query) result = result.filter(e => e.templateName.toLowerCase().includes(query));
    if (this.selectedFrequency()) result = result.filter(e => e.frequency === this.selectedFrequency());
    if (this.selectedStatus() === 'active') result = result.filter(e => e.isActive);
    if (this.selectedStatus() === 'paused') result = result.filter(e => !e.isActive);
    return result;
  });

  activeCount = computed(() => this.entries().filter(e => e.isActive).length);
  dueThisMonth = computed(() => this.entries().filter(e => e.isActive && this.isDueThisMonth(e.nextRunDate)).length);
  monthlyTotal = computed(() => this.entries().filter(e => e.isActive && e.frequency === FrequencyType.Monthly).reduce((sum, e) => sum + e.amount, 0));

  ngOnInit(): void { this.loadData(); this.initializeLines(); }

  initializeLines(): void {
    this.lines.clear();
    this.addLine();
    this.addLine();
  }

  addLine(): void {
    this.lines.push(this.fb.group({
      accountId: ['', Validators.required],
      debitAmount: [0],
      creditAmount: [0]
    }));
  }

  removeLine(index: number): void {
    if (this.lines.length > 2) this.lines.removeAt(index);
  }

  isUpcoming(date: Date): boolean {
    const diff = new Date(date).getTime() - new Date().getTime();
    return diff > 0 && diff < 7 * 24 * 60 * 60 * 1000;
  }

  isDueThisMonth(date: Date): boolean {
    const now = new Date();
    const d = new Date(date);
    return d.getMonth() === now.getMonth() && d.getFullYear() === now.getFullYear();
  }

  async loadData(): Promise<void> {
    this.isLoading.set(true);
    try {
      const mockEntries: RecurringJournalEntry[] = [
        { id: '1', templateCode: 'DEP-001', templateName: 'Monthly Depreciation', description: 'Record monthly depreciation expense', frequency: FrequencyType.Monthly, amount: 5000, debitAccountId: '2', debitAccountCode: '6100', debitAccountName: 'Depreciation Expense', creditAccountId: '3', creditAccountCode: '1500', creditAccountName: 'Accumulated Depreciation', startDate: new Date('2024-01-01'), nextRunDate: new Date('2024-02-29'), lastRunDate: new Date('2024-01-31'), totalRuns: 1, autoPost: true, requiresApproval: false, status: 'Active', isActive: true },
        { id: '2', templateCode: 'INT-001', templateName: 'Quarterly Interest Accrual', description: 'Accrue interest on loans', frequency: FrequencyType.Quarterly, amount: 12500, debitAccountId: '2', debitAccountCode: '6100', debitAccountName: 'Interest Expense', creditAccountId: '1', creditAccountCode: '1100', creditAccountName: 'Cash', startDate: new Date('2023-01-01'), nextRunDate: new Date('2024-03-31'), lastRunDate: new Date('2023-12-31'), totalRuns: 4, autoPost: true, requiresApproval: false, status: 'Active', isActive: true },
        { id: '3', templateCode: 'PAY-001', templateName: 'Weekly Payroll Accrual', description: 'Accrue weekly payroll', frequency: FrequencyType.Weekly, amount: 25000, debitAccountId: '2', debitAccountCode: '6200', debitAccountName: 'Payroll Expense', creditAccountId: '1', creditAccountCode: '1100', creditAccountName: 'Cash', startDate: new Date('2024-01-01'), nextRunDate: new Date('2024-02-09'), lastRunDate: new Date('2024-02-02'), totalRuns: 5, autoPost: false, requiresApproval: true, status: 'Paused', isActive: false }
      ];
      const mockAccounts: ChartOfAccount[] = [
        { id: '1', accountCode: '1100', accountName: 'Cash', accountType: AccountType.Asset, normalBalance: 'Debit', isActive: true, currentBalance: 0, isHeader: false, isBankAccount: false, level: 1, sortOrder: 1 },
        { id: '2', accountCode: '6100', accountName: 'Depreciation Expense', accountType: AccountType.Expense, normalBalance: 'Debit', isActive: true, currentBalance: 0, isHeader: false, isBankAccount: false, level: 1, sortOrder: 1 },
        { id: '3', accountCode: '1500', accountName: 'Accumulated Depreciation', accountType: AccountType.Asset, normalBalance: 'Credit', isActive: true, currentBalance: 0, isHeader: false, isBankAccount: false, level: 1, sortOrder: 1 }
      ];
      await new Promise(r => setTimeout(r, 300));
      this.entries.set(mockEntries);
      this.accounts.set(mockAccounts);
      this.totalEntries.set(mockEntries.length);
    } finally { this.isLoading.set(false); }
  }

  onSearch(event: Event): void { this.searchQuery.set((event.target as HTMLInputElement).value); }
  onFrequencyChange(value: string): void { this.selectedFrequency.set(value); }
  onStatusChange(value: string): void { this.selectedStatus.set(value); }

  openEntryDialog(entry?: RecurringJournalEntry): void {
    this.editingEntry.set(entry || null);
    if (entry) {
      this.entryForm.patchValue(entry);
    } else {
      this.entryForm.reset({ frequency: 'Monthly', amount: 0, isActive: true });
      this.initializeLines();
    }
    this.dialog.open(this.entryDialogTemplate, { width: '700px' });
  }

  async saveEntry(): Promise<void> {
    if (this.entryForm.invalid) return;
    this.isSaving.set(true);
    try {
      await new Promise(r => setTimeout(r, 500));
      this.notification.success('Recurring entry saved');
      this.dialog.closeAll();
      this.loadData();
    } finally { this.isSaving.set(false); }
  }

  viewEntry(entry: RecurringJournalEntry): void {}
  viewHistory(entry: RecurringJournalEntry): void {}
  async runNow(entry: RecurringJournalEntry): Promise<void> { this.notification.success('Entry executed'); this.loadData(); }
  async pauseEntry(entry: RecurringJournalEntry): Promise<void> { this.notification.success('Entry paused'); this.loadData(); }
  async resumeEntry(entry: RecurringJournalEntry): Promise<void> { this.notification.success('Entry resumed'); this.loadData(); }
  async deleteEntry(entry: RecurringJournalEntry): Promise<void> { this.notification.success('Entry deleted'); this.loadData(); }
}

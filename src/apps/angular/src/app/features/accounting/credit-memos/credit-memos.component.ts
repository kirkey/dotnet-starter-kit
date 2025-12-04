import { Component, inject, signal, OnInit, ViewChild, TemplateRef, computed } from '@angular/core';
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
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';
import { NotificationService } from '@core/services/notification.service';
import { CreditMemo, Customer, ApprovalStatus } from '@core/models/accounting.model';

@Component({
  selector: 'app-credit-memos',
  standalone: true,
  imports: [
    CommonModule, CurrencyPipe, DatePipe, ReactiveFormsModule,
    MatTableModule, MatPaginatorModule, MatSortModule, MatButtonModule, MatIconModule,
    MatFormFieldModule, MatInputModule, MatSelectModule, MatDialogModule,
    MatProgressSpinnerModule, MatChipsModule, MatMenuModule, MatTooltipModule,
    MatCardModule, MatDatepickerModule, MatNativeDateModule, PageHeaderComponent
  ],
  template: `
    <div class="credit-memos-container">
      <app-page-header 
        title="Credit Memos" 
        subtitle="Manage customer credit memos"
        icon="credit_card_off">
      </app-page-header>

      <div class="toolbar">
        <div class="filters">
          <mat-form-field appearance="outline" class="search-field">
            <mat-label>Search credit memos</mat-label>
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
              <mat-option value="Draft">Draft</mat-option>
              <mat-option value="Pending">Pending</mat-option>
              <mat-option value="Approved">Approved</mat-option>
            </mat-select>
          </mat-form-field>
        </div>

        <button mat-raised-button color="primary" (click)="openMemoDialog()">
          <mat-icon>add</mat-icon>
          New Credit Memo
        </button>
      </div>

      <div class="summary-cards">
        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon total">
              <mat-icon>credit_card_off</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Total Outstanding</span>
              <span class="summary-value">{{ totalOutstanding() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon applied">
              <mat-icon>check_circle</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Applied This Month</span>
              <span class="summary-value">{{ appliedThisMonth() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon pending">
              <mat-icon>hourglass_empty</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Pending Approval</span>
              <span class="summary-value">{{ pendingCount() }}</span>
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
          <table mat-table [dataSource]="filteredMemos()" matSort>
            <ng-container matColumnDef="memoNumber">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Memo #</th>
              <td mat-cell *matCellDef="let memo">
                <span class="memo-number">{{ memo.memoNumber }}</span>
              </td>
            </ng-container>

            <ng-container matColumnDef="customer">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Customer</th>
              <td mat-cell *matCellDef="let memo">{{ memo.customerName }}</td>
            </ng-container>

            <ng-container matColumnDef="memoDate">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Date</th>
              <td mat-cell *matCellDef="let memo">{{ memo.memoDate | date:'mediumDate' }}</td>
            </ng-container>

            <ng-container matColumnDef="reason">
              <th mat-header-cell *matHeaderCellDef>Reason</th>
              <td mat-cell *matCellDef="let memo">{{ memo.reason }}</td>
            </ng-container>

            <ng-container matColumnDef="amount">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Amount</th>
              <td mat-cell *matCellDef="let memo">{{ memo.amount | currency }}</td>
            </ng-container>

            <ng-container matColumnDef="unapplied">
              <th mat-header-cell *matHeaderCellDef>Unapplied</th>
              <td mat-cell *matCellDef="let memo">{{ memo.unappliedAmount | currency }}</td>
            </ng-container>

            <ng-container matColumnDef="status">
              <th mat-header-cell *matHeaderCellDef>Status</th>
              <td mat-cell *matCellDef="let memo">
                <mat-chip [class]="'status-' + memo.status.toLowerCase()">{{ memo.status }}</mat-chip>
              </td>
            </ng-container>

            <ng-container matColumnDef="invoice">
              <th mat-header-cell *matHeaderCellDef>Related Invoice</th>
              <td mat-cell *matCellDef="let memo">{{ memo.relatedInvoiceNumber || '-' }}</td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef></th>
              <td mat-cell *matCellDef="let memo">
                <button mat-icon-button [matMenuTriggerFor]="menu">
                  <mat-icon>more_vert</mat-icon>
                </button>
                <mat-menu #menu="matMenu">
                  <button mat-menu-item (click)="viewMemo(memo)">
                    <mat-icon>visibility</mat-icon>
                    <span>View</span>
                  </button>
                  @if (memo.status === 'Draft') {
                    <button mat-menu-item (click)="openMemoDialog(memo)">
                      <mat-icon>edit</mat-icon>
                      <span>Edit</span>
                    </button>
                    <button mat-menu-item (click)="submitForApproval(memo)">
                      <mat-icon>send</mat-icon>
                      <span>Submit</span>
                    </button>
                  }
                  @if (memo.unappliedAmount > 0 && memo.isPosted) {
                    <button mat-menu-item (click)="applyToInvoice(memo)">
                      <mat-icon>link</mat-icon>
                      <span>Apply to Invoice</span>
                    </button>
                  }
                </mat-menu>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
          </table>

          <mat-paginator [length]="totalMemos()" [pageSize]="pageSize()" [pageSizeOptions]="[10, 25, 50]" showFirstLastButtons></mat-paginator>
        </div>

        @if (filteredMemos().length === 0) {
          <div class="empty-state">
            <mat-icon>credit_card_off</mat-icon>
            <h3>No credit memos found</h3>
            <p>Create a credit memo when you need to adjust a customer's balance</p>
            <button mat-raised-button color="primary" (click)="openMemoDialog()">
              <mat-icon>add</mat-icon>
              New Credit Memo
            </button>
          </div>
        }
      }
    </div>

    <ng-template #memoDialog>
      <h2 mat-dialog-title>{{ editingMemo() ? 'Edit Credit Memo' : 'New Credit Memo' }}</h2>
      <mat-dialog-content>
        <form [formGroup]="memoForm" class="memo-form">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Customer</mat-label>
            <mat-select formControlName="customerId" required>
              @for (customer of customers(); track customer.id) {
                <mat-option [value]="customer.id">{{ customer.customerName }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Date</mat-label>
              <input matInput [matDatepicker]="datePicker" formControlName="memoDate" required>
              <mat-datepicker-toggle matIconSuffix [for]="datePicker"></mat-datepicker-toggle>
              <mat-datepicker #datePicker></mat-datepicker>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Amount</mat-label>
              <input matInput type="number" formControlName="amount" required>
              <span matPrefix>$&nbsp;</span>
            </mat-form-field>
          </div>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Related Invoice (Optional)</mat-label>
            <mat-select formControlName="relatedInvoiceId">
              <mat-option value="">None</mat-option>
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Reason</mat-label>
            <mat-select formControlName="reason" required>
              <mat-option value="Return">Product Return</mat-option>
              <mat-option value="Pricing">Pricing Adjustment</mat-option>
              <mat-option value="Damage">Damaged Goods</mat-option>
              <mat-option value="Billing">Billing Error</mat-option>
              <mat-option value="Other">Other</mat-option>
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Description</mat-label>
            <textarea matInput formControlName="description" rows="3"></textarea>
          </mat-form-field>
        </form>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button mat-dialog-close>Cancel</button>
        <button mat-raised-button color="primary" [disabled]="memoForm.invalid || isSaving()" (click)="saveMemo()">
          @if (isSaving()) { <mat-spinner diameter="20"></mat-spinner> }
          Save
        </button>
      </mat-dialog-actions>
    </ng-template>
  `,
  styles: [`
    .credit-memos-container { padding: 24px; }
    .toolbar { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 24px; gap: 16px; flex-wrap: wrap; }
    .filters { display: flex; gap: 12px; flex-wrap: wrap; flex: 1; }
    .search-field { min-width: 250px; flex: 1; }
    .filter-field { min-width: 150px; }
    .summary-cards { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 16px; margin-bottom: 24px; }
    .summary-card mat-card-content { display: flex; align-items: center; gap: 16px; padding: 16px !important; }
    .summary-icon { width: 48px; height: 48px; border-radius: 12px; display: flex; align-items: center; justify-content: center; }
    .summary-icon.total { background: #e0e7ff; color: #4f46e5; }
    .summary-icon.applied { background: #dcfce7; color: #16a34a; }
    .summary-icon.pending { background: #fef3c7; color: #d97706; }
    .summary-info { display: flex; flex-direction: column; }
    .summary-label { font-size: 12px; color: var(--text-secondary); }
    .summary-value { font-size: 20px; font-weight: 600; }
    .loading-container { display: flex; justify-content: center; padding: 48px; }
    .table-container { background: var(--surface-color); border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1); }
    table { width: 100%; }
    .memo-number { font-weight: 500; color: var(--primary-color); }
    .status-draft { background: #f3f4f6 !important; color: #374151 !important; }
    .status-pending { background: #fef3c7 !important; color: #92400e !important; }
    .status-approved { background: #dcfce7 !important; color: #166534 !important; }
    .empty-state { display: flex; flex-direction: column; align-items: center; padding: 64px 24px; text-align: center; }
    .empty-state mat-icon { font-size: 64px; width: 64px; height: 64px; color: #ccc; margin-bottom: 16px; }
    .memo-form { display: flex; flex-direction: column; gap: 16px; min-width: 400px; }
    .form-row { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
    .full-width { width: 100%; }
  `]
})
export class CreditMemosComponent implements OnInit {
  @ViewChild('memoDialog') memoDialogTemplate!: TemplateRef<any>;

  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private notification = inject(NotificationService);

  memos = signal<CreditMemo[]>([]);
  customers = signal<Customer[]>([]);
  isLoading = signal(false);
  isSaving = signal(false);
  searchQuery = signal('');
  selectedCustomer = signal('');
  selectedStatus = signal('');
  editingMemo = signal<CreditMemo | null>(null);

  pageSize = signal(10);
  totalMemos = signal(0);

  displayedColumns = ['memoNumber', 'customer', 'memoDate', 'reason', 'amount', 'unapplied', 'status', 'invoice', 'actions'];

  memoForm: FormGroup = this.fb.group({
    customerId: ['', Validators.required],
    memoDate: [new Date(), Validators.required],
    amount: [0, [Validators.required, Validators.min(0.01)]],
    relatedInvoiceId: [''],
    reason: ['', Validators.required],
    description: ['']
  });

  filteredMemos = computed(() => {
    let result = this.memos();
    const query = this.searchQuery().toLowerCase();
    if (query) result = result.filter(m => m.memoNumber.toLowerCase().includes(query) || m.customerName.toLowerCase().includes(query));
    if (this.selectedCustomer()) result = result.filter(m => m.customerId === this.selectedCustomer());
    if (this.selectedStatus()) result = result.filter(m => m.status === this.selectedStatus());
    return result;
  });

  totalOutstanding = computed(() => this.memos().reduce((sum, m) => sum + m.unappliedAmount, 0));
  appliedThisMonth = computed(() => this.memos().reduce((sum, m) => sum + m.appliedAmount, 0));
  pendingCount = computed(() => this.memos().filter(m => m.status === ApprovalStatus.Pending).length);

  ngOnInit(): void { this.loadData(); }

  async loadData(): Promise<void> {
    this.isLoading.set(true);
    try {
      const mockMemos: CreditMemo[] = [
        { id: '1', memoNumber: 'CM-001', customerId: '1', customerNumber: 'C001', customerName: 'Acme Corp', memoDate: new Date('2024-01-15'), reason: 'Product Return', description: 'Returned defective items', amount: 500, appliedAmount: 200, unappliedAmount: 300, status: ApprovalStatus.Approved, periodId: '1', isPosted: true, isActive: true },
        { id: '2', memoNumber: 'CM-002', customerId: '2', customerNumber: 'C002', customerName: 'Tech Solutions', memoDate: new Date('2024-01-20'), reason: 'Pricing Adjustment', amount: 150, appliedAmount: 0, unappliedAmount: 150, status: ApprovalStatus.Pending, periodId: '1', isPosted: false, isActive: true }
      ];
      const mockCustomers: Customer[] = [
        { id: '1', customerNumber: 'C001', customerName: 'Acme Corp', creditLimit: 100000, currentBalance: 45000, creditHold: false, isActive: true },
        { id: '2', customerNumber: 'C002', customerName: 'Tech Solutions', creditLimit: 50000, currentBalance: 25000, creditHold: false, isActive: true }
      ];
      await new Promise(r => setTimeout(r, 300));
      this.memos.set(mockMemos);
      this.customers.set(mockCustomers);
      this.totalMemos.set(mockMemos.length);
    } finally { this.isLoading.set(false); }
  }

  onSearch(event: Event): void { this.searchQuery.set((event.target as HTMLInputElement).value); }
  onCustomerChange(value: string): void { this.selectedCustomer.set(value); }
  onStatusChange(value: string): void { this.selectedStatus.set(value); }

  openMemoDialog(memo?: CreditMemo): void {
    this.editingMemo.set(memo || null);
    if (memo) this.memoForm.patchValue(memo);
    else this.memoForm.reset({ memoDate: new Date(), amount: 0 });
    this.dialog.open(this.memoDialogTemplate, { width: '500px' });
  }

  async saveMemo(): Promise<void> {
    if (this.memoForm.invalid) return;
    this.isSaving.set(true);
    try {
      await new Promise(r => setTimeout(r, 500));
      this.notification.success('Credit memo saved');
      this.dialog.closeAll();
      this.loadData();
    } finally { this.isSaving.set(false); }
  }

  viewMemo(memo: CreditMemo): void {}
  async submitForApproval(memo: CreditMemo): Promise<void> { this.notification.success('Submitted for approval'); }
  applyToInvoice(memo: CreditMemo): void {}
}

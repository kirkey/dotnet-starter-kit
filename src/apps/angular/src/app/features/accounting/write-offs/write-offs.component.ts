import { Component, inject, signal, OnInit, ViewChild, TemplateRef, computed } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
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
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { NotificationService } from '@core/services/notification.service';
import { WriteOff, Customer, Invoice, ApprovalStatus, PaymentStatus } from '@core/models/accounting.model';

@Component({
  selector: 'app-write-offs',
  standalone: true,
  imports: [
    CommonModule, CurrencyPipe, DatePipe, ReactiveFormsModule,
    MatTableModule, MatPaginatorModule, MatSortModule, MatButtonModule, MatIconModule,
    MatFormFieldModule, MatInputModule, MatSelectModule, MatDialogModule,
    MatProgressSpinnerModule, MatChipsModule, MatMenuModule, MatTooltipModule,
    MatCardModule, MatDatepickerModule, MatNativeDateModule, MatSlideToggleModule, PageHeaderComponent
  ],
  template: `
    <div class="write-offs-container">
      <app-page-header 
        title="Write-Offs" 
        subtitle="Manage bad debt write-offs and uncollectible accounts"
        icon="money_off">
      </app-page-header>

      <div class="toolbar">
        <div class="filters">
          <mat-form-field appearance="outline" class="search-field">
            <mat-label>Search write-offs</mat-label>
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

        <button mat-raised-button color="primary" (click)="openWriteOffDialog()">
          <mat-icon>add</mat-icon>
          New Write-Off
        </button>
      </div>

      <div class="summary-cards">
        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon total">
              <mat-icon>money_off</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Total Written Off (YTD)</span>
              <span class="summary-value">{{ totalWrittenOff() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon pending">
              <mat-icon>pending</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Pending Approval</span>
              <span class="summary-value">{{ pendingAmount() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon recovered">
              <mat-icon>account_balance_wallet</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Recovered Amount</span>
              <span class="summary-value">{{ recoveredAmount() | currency }}</span>
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
          <table mat-table [dataSource]="filteredWriteOffs()" matSort>
            <ng-container matColumnDef="writeOffNumber">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Write-Off #</th>
              <td mat-cell *matCellDef="let wo">
                <span class="wo-number">{{ wo.writeOffNumber }}</span>
              </td>
            </ng-container>

            <ng-container matColumnDef="customer">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Customer</th>
              <td mat-cell *matCellDef="let wo">{{ wo.customerName }}</td>
            </ng-container>

            <ng-container matColumnDef="invoice">
              <th mat-header-cell *matHeaderCellDef>Invoice</th>
              <td mat-cell *matCellDef="let wo">{{ wo.invoiceNumber }}</td>
            </ng-container>

            <ng-container matColumnDef="writeOffDate">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Date</th>
              <td mat-cell *matCellDef="let wo">{{ wo.writeOffDate | date:'mediumDate' }}</td>
            </ng-container>

            <ng-container matColumnDef="reason">
              <th mat-header-cell *matHeaderCellDef>Reason</th>
              <td mat-cell *matCellDef="let wo">{{ wo.reason }}</td>
            </ng-container>

            <ng-container matColumnDef="amount">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Amount</th>
              <td mat-cell *matCellDef="let wo" class="amount-cell">{{ wo.amount | currency }}</td>
            </ng-container>

            <ng-container matColumnDef="status">
              <th mat-header-cell *matHeaderCellDef>Status</th>
              <td mat-cell *matCellDef="let wo">
                <mat-chip [class]="'status-' + wo.status.toLowerCase()">{{ wo.status }}</mat-chip>
              </td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef></th>
              <td mat-cell *matCellDef="let wo">
                <button mat-icon-button [matMenuTriggerFor]="menu">
                  <mat-icon>more_vert</mat-icon>
                </button>
                <mat-menu #menu="matMenu">
                  <button mat-menu-item (click)="viewWriteOff(wo)">
                    <mat-icon>visibility</mat-icon>
                    <span>View</span>
                  </button>
                  @if (wo.status === 'Draft') {
                    <button mat-menu-item (click)="openWriteOffDialog(wo)">
                      <mat-icon>edit</mat-icon>
                      <span>Edit</span>
                    </button>
                    <button mat-menu-item (click)="submitForApproval(wo)">
                      <mat-icon>send</mat-icon>
                      <span>Submit</span>
                    </button>
                    <button mat-menu-item (click)="deleteWriteOff(wo)" class="delete-action">
                      <mat-icon color="warn">delete</mat-icon>
                      <span>Delete</span>
                    </button>
                  }
                  @if (wo.isPosted && !wo.hasRecovery) {
                    <button mat-menu-item (click)="recordRecovery(wo)">
                      <mat-icon>payments</mat-icon>
                      <span>Record Recovery</span>
                    </button>
                  }
                </mat-menu>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns;" [class.recovered]="row.hasRecovery"></tr>
          </table>

          <mat-paginator [length]="totalWriteOffs()" [pageSize]="pageSize()" [pageSizeOptions]="[10, 25, 50]" showFirstLastButtons></mat-paginator>
        </div>
      }
    </div>

    <ng-template #writeOffDialog>
      <h2 mat-dialog-title>{{ editingWriteOff() ? 'Edit Write-Off' : 'New Write-Off' }}</h2>
      <mat-dialog-content>
        <form [formGroup]="writeOffForm" class="wo-form">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Customer</mat-label>
            <mat-select formControlName="customerId" required (selectionChange)="onCustomerSelect($event.value)">
              @for (customer of customers(); track customer.id) {
                <mat-option [value]="customer.id">{{ customer.customerName }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          @if (writeOffForm.get('customerId')?.value) {
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Invoice</mat-label>
              <mat-select formControlName="invoiceId" required>
                @for (invoice of customerInvoices(); track invoice.id) {
                  <mat-option [value]="invoice.id">{{ invoice.invoiceNumber }} - {{ invoice.balanceDue | currency }}</mat-option>
                }
              </mat-select>
            </mat-form-field>
          }

          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Date</mat-label>
              <input matInput [matDatepicker]="datePicker" formControlName="writeOffDate" required>
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
            <mat-label>Reason</mat-label>
            <mat-select formControlName="reason" required>
              <mat-option value="Bankruptcy">Bankruptcy</mat-option>
              <mat-option value="Uncollectible">Uncollectible</mat-option>
              <mat-option value="Disputed">Disputed</mat-option>
              <mat-option value="CustomerClosed">Customer Closed</mat-option>
              <mat-option value="Other">Other</mat-option>
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Notes</mat-label>
            <textarea matInput formControlName="notes" rows="3" placeholder="Document the reason and circumstances"></textarea>
          </mat-form-field>
        </form>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button mat-dialog-close>Cancel</button>
        <button mat-raised-button color="primary" [disabled]="writeOffForm.invalid || isSaving()" (click)="saveWriteOff()">
          @if (isSaving()) { <mat-spinner diameter="20"></mat-spinner> }
          Save
        </button>
      </mat-dialog-actions>
    </ng-template>
  `,
  styles: [`
    .write-offs-container { padding: 24px; }
    .toolbar { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 24px; gap: 16px; flex-wrap: wrap; }
    .filters { display: flex; gap: 12px; flex-wrap: wrap; flex: 1; }
    .search-field { min-width: 250px; flex: 1; }
    .filter-field { min-width: 150px; }
    .summary-cards { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 16px; margin-bottom: 24px; }
    .summary-card mat-card-content { display: flex; align-items: center; gap: 16px; padding: 16px !important; }
    .summary-icon { width: 48px; height: 48px; border-radius: 12px; display: flex; align-items: center; justify-content: center; }
    .summary-icon.total { background: #fee2e2; color: #dc2626; }
    .summary-icon.pending { background: #fef3c7; color: #d97706; }
    .summary-icon.recovered { background: #dcfce7; color: #16a34a; }
    .summary-info { display: flex; flex-direction: column; }
    .summary-label { font-size: 12px; color: var(--text-secondary); }
    .summary-value { font-size: 20px; font-weight: 600; }
    .loading-container { display: flex; justify-content: center; padding: 48px; }
    .table-container { background: var(--surface-color); border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1); }
    table { width: 100%; }
    .wo-number { font-weight: 500; color: var(--primary-color); }
    .amount-cell { color: #dc2626; font-weight: 500; }
    .status-draft { background: #f3f4f6 !important; color: #374151 !important; }
    .status-pending { background: #fef3c7 !important; color: #92400e !important; }
    .status-approved { background: #dcfce7 !important; color: #166534 !important; }
    tr.recovered { background: #f0fdf4; }
    .wo-form { display: flex; flex-direction: column; gap: 16px; min-width: 400px; }
    .form-row { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
    .full-width { width: 100%; }
    .delete-action { color: #dc2626; }
  `]
})
export class WriteOffsComponent implements OnInit {
  @ViewChild('writeOffDialog') writeOffDialogTemplate!: TemplateRef<any>;

  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private notification = inject(NotificationService);

  writeOffs = signal<WriteOff[]>([]);
  customers = signal<Customer[]>([]);
  customerInvoices = signal<Invoice[]>([]);
  isLoading = signal(false);
  isSaving = signal(false);
  searchQuery = signal('');
  selectedCustomer = signal('');
  selectedStatus = signal('');
  editingWriteOff = signal<WriteOff | null>(null);

  pageSize = signal(10);
  totalWriteOffs = signal(0);

  displayedColumns = ['writeOffNumber', 'customer', 'invoice', 'writeOffDate', 'reason', 'amount', 'status', 'actions'];

  writeOffForm: FormGroup = this.fb.group({
    customerId: ['', Validators.required],
    invoiceId: ['', Validators.required],
    writeOffDate: [new Date(), Validators.required],
    amount: [0, [Validators.required, Validators.min(0.01)]],
    reason: ['', Validators.required],
    notes: ['']
  });

  filteredWriteOffs = computed(() => {
    let result = this.writeOffs();
    const query = this.searchQuery().toLowerCase();
    if (query) result = result.filter(wo => wo.writeOffNumber.toLowerCase().includes(query) || wo.customerName.toLowerCase().includes(query));
    if (this.selectedCustomer()) result = result.filter(wo => wo.customerId === this.selectedCustomer());
    if (this.selectedStatus()) result = result.filter(wo => wo.status === this.selectedStatus());
    return result;
  });

  totalWrittenOff = computed(() => this.writeOffs().filter(wo => wo.isPosted).reduce((sum, wo) => sum + wo.amount, 0));
  pendingAmount = computed(() => this.writeOffs().filter(wo => wo.status === ApprovalStatus.Pending).reduce((sum, wo) => sum + wo.amount, 0));
  recoveredAmount = computed(() => this.writeOffs().filter(wo => wo.hasRecovery).reduce((sum, wo) => sum + (wo.recoveryAmount || 0), 0));

  ngOnInit(): void { this.loadData(); }

  async loadData(): Promise<void> {
    this.isLoading.set(true);
    try {
      const mockWriteOffs: WriteOff[] = [
        { id: '1', writeOffNumber: 'WO-001', customerId: '1', customerNumber: 'C001', customerName: 'Acme Corp', invoiceId: '101', invoiceNumber: 'INV-1001', writeOffDate: new Date('2024-01-18'), reason: 'Bankruptcy', description: 'Customer filed Chapter 11', amount: 2500, status: ApprovalStatus.Approved, periodId: '1', isPosted: true, hasRecovery: false, recoveryAmount: 0, isActive: true },
        { id: '2', writeOffNumber: 'WO-002', customerId: '2', customerNumber: 'C002', customerName: 'Tech Solutions', invoiceId: '102', invoiceNumber: 'INV-1022', writeOffDate: new Date('2024-01-22'), reason: 'Uncollectible', amount: 1200, status: ApprovalStatus.Pending, periodId: '1', isPosted: false, hasRecovery: false, recoveryAmount: 0, isActive: true }
      ];
      const mockCustomers: Customer[] = [
        { id: '1', customerNumber: 'C001', customerName: 'Acme Corp', creditLimit: 100000, currentBalance: 45000, creditHold: false, isActive: true },
        { id: '2', customerNumber: 'C002', customerName: 'Tech Solutions', creditLimit: 50000, currentBalance: 25000, creditHold: false, isActive: true }
      ];
      await new Promise(r => setTimeout(r, 300));
      this.writeOffs.set(mockWriteOffs);
      this.customers.set(mockCustomers);
      this.totalWriteOffs.set(mockWriteOffs.length);
    } finally { this.isLoading.set(false); }
  }

  onSearch(event: Event): void { this.searchQuery.set((event.target as HTMLInputElement).value); }
  onCustomerChange(value: string): void { this.selectedCustomer.set(value); }
  onStatusChange(value: string): void { this.selectedStatus.set(value); }
  
  async onCustomerSelect(customerId: string): Promise<void> {
    const mockInvoices: Invoice[] = [
      { id: '101', invoiceNumber: 'INV-1001', customerId, customerNumber: 'C001', customerName: 'Customer', invoiceDate: new Date(), dueDate: new Date(), subtotal: 2500, taxAmount: 0, totalAmount: 2500, paidAmount: 0, balanceDue: 2500, status: ApprovalStatus.Approved, paymentStatus: PaymentStatus.Unpaid, periodId: '1', isPosted: true, lines: [], isActive: true },
      { id: '102', invoiceNumber: 'INV-1002', customerId, customerNumber: 'C002', customerName: 'Customer', invoiceDate: new Date(), dueDate: new Date(), subtotal: 1200, taxAmount: 0, totalAmount: 1200, paidAmount: 0, balanceDue: 1200, status: ApprovalStatus.Approved, paymentStatus: PaymentStatus.Unpaid, periodId: '1', isPosted: true, lines: [], isActive: true }
    ];
    this.customerInvoices.set(mockInvoices);
  }

  openWriteOffDialog(writeOff?: WriteOff): void {
    this.editingWriteOff.set(writeOff || null);
    if (writeOff) {
      this.writeOffForm.patchValue(writeOff);
      this.onCustomerSelect(writeOff.customerId);
    } else {
      this.writeOffForm.reset({ writeOffDate: new Date(), amount: 0 });
      this.customerInvoices.set([]);
    }
    this.dialog.open(this.writeOffDialogTemplate, { width: '500px' });
  }

  async saveWriteOff(): Promise<void> {
    if (this.writeOffForm.invalid) return;
    this.isSaving.set(true);
    try {
      await new Promise(r => setTimeout(r, 500));
      this.notification.success('Write-off saved');
      this.dialog.closeAll();
      this.loadData();
    } finally { this.isSaving.set(false); }
  }

  viewWriteOff(wo: WriteOff): void {}
  async submitForApproval(wo: WriteOff): Promise<void> { this.notification.success('Submitted for approval'); }
  async deleteWriteOff(wo: WriteOff): Promise<void> { this.notification.success('Write-off deleted'); this.loadData(); }
  recordRecovery(wo: WriteOff): void {}
}

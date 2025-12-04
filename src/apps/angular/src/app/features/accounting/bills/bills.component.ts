import { Component, inject, signal, OnInit, ViewChild, TemplateRef, computed } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
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
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';
import { NotificationService } from '@core/services/notification.service';
import { AccountingService } from '@core/services/accounting.service';
import { Bill, Vendor, ChartOfAccount, AccountingPeriod, ApprovalStatus, PaymentStatus } from '@core/models/accounting.model';

@Component({
  selector: 'app-bills',
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
    PageHeaderComponent
  ],
  template: `
    <div class="bills-container">
      <app-page-header 
        title="Bills" 
        subtitle="Manage vendor bills and payables"
        icon="receipt_long">
      </app-page-header>

      <!-- Toolbar -->
      <div class="toolbar">
        <div class="filters">
          <mat-form-field appearance="outline" class="search-field">
            <mat-label>Search bills</mat-label>
            <input matInput 
                   [value]="searchQuery()" 
                   (input)="onSearch($event)"
                   placeholder="Search by bill number, vendor...">
            <mat-icon matPrefix>search</mat-icon>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Vendor</mat-label>
            <mat-select [value]="selectedVendor()" (selectionChange)="onVendorChange($event.value)">
              <mat-option value="">All Vendors</mat-option>
              @for (vendor of vendors(); track vendor.id) {
                <mat-option [value]="vendor.id">{{ vendor.vendorName }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Status</mat-label>
            <mat-select [value]="selectedStatus()" (selectionChange)="onStatusChange($event.value)">
              <mat-option value="">All Statuses</mat-option>
              <mat-option value="Draft">Draft</mat-option>
              <mat-option value="Pending">Pending</mat-option>
              <mat-option value="Approved">Approved</mat-option>
              <mat-option value="Rejected">Rejected</mat-option>
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Payment Status</mat-label>
            <mat-select [value]="selectedPaymentStatus()" (selectionChange)="onPaymentStatusChange($event.value)">
              <mat-option value="">All</mat-option>
              <mat-option value="Unpaid">Unpaid</mat-option>
              <mat-option value="PartiallyPaid">Partially Paid</mat-option>
              <mat-option value="Paid">Paid</mat-option>
            </mat-select>
          </mat-form-field>
        </div>

        <div class="actions">
          <button mat-raised-button color="primary" (click)="openBillDialog()">
            <mat-icon>add</mat-icon>
            New Bill
          </button>
        </div>
      </div>

      <!-- Summary Cards -->
      <div class="summary-cards">
        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon unpaid">
              <mat-icon>pending_actions</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Total Unpaid</span>
              <span class="summary-value">{{ totalUnpaid() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon overdue">
              <mat-icon>warning</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Overdue</span>
              <span class="summary-value">{{ totalOverdue() | currency }}</span>
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

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon paid">
              <mat-icon>check_circle</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Paid This Month</span>
              <span class="summary-value">{{ paidThisMonth() | currency }}</span>
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
          <table mat-table [dataSource]="filteredBills()" matSort (matSortChange)="onSort($event)">
            <!-- Bill Number Column -->
            <ng-container matColumnDef="billNumber">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Bill #</th>
              <td mat-cell *matCellDef="let bill">
                <span class="bill-number">{{ bill.billNumber }}</span>
              </td>
            </ng-container>

            <!-- Vendor Column -->
            <ng-container matColumnDef="vendor">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Vendor</th>
              <td mat-cell *matCellDef="let bill">
                <div class="vendor-info">
                  <span class="vendor-name">{{ bill.vendorName }}</span>
                  <span class="vendor-code">{{ bill.vendorCode }}</span>
                </div>
              </td>
            </ng-container>

            <!-- Bill Date Column -->
            <ng-container matColumnDef="billDate">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Bill Date</th>
              <td mat-cell *matCellDef="let bill">{{ bill.billDate | date:'mediumDate' }}</td>
            </ng-container>

            <!-- Due Date Column -->
            <ng-container matColumnDef="dueDate">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Due Date</th>
              <td mat-cell *matCellDef="let bill">
                <span [class.overdue]="isOverdue(bill)">{{ bill.dueDate | date:'mediumDate' }}</span>
              </td>
            </ng-container>

            <!-- Amount Column -->
            <ng-container matColumnDef="amount">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Amount</th>
              <td mat-cell *matCellDef="let bill">{{ bill.totalAmount | currency }}</td>
            </ng-container>

            <!-- Balance Column -->
            <ng-container matColumnDef="balance">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Balance</th>
              <td mat-cell *matCellDef="let bill">
                <span [class.has-balance]="bill.balanceDue > 0">{{ bill.balanceDue | currency }}</span>
              </td>
            </ng-container>

            <!-- Status Column -->
            <ng-container matColumnDef="status">
              <th mat-header-cell *matHeaderCellDef>Status</th>
              <td mat-cell *matCellDef="let bill">
                <mat-chip [class]="getStatusClass(bill.status)">{{ bill.status }}</mat-chip>
              </td>
            </ng-container>

            <!-- Payment Status Column -->
            <ng-container matColumnDef="paymentStatus">
              <th mat-header-cell *matHeaderCellDef>Payment</th>
              <td mat-cell *matCellDef="let bill">
                <mat-chip [class]="getPaymentStatusClass(bill.paymentStatus)">{{ bill.paymentStatus }}</mat-chip>
              </td>
            </ng-container>

            <!-- Actions Column -->
            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef></th>
              <td mat-cell *matCellDef="let bill">
                <button mat-icon-button [matMenuTriggerFor]="menu">
                  <mat-icon>more_vert</mat-icon>
                </button>
                <mat-menu #menu="matMenu">
                  <button mat-menu-item (click)="openBillDialog(bill)">
                    <mat-icon>edit</mat-icon>
                    <span>Edit</span>
                  </button>
                  <button mat-menu-item (click)="viewBill(bill)">
                    <mat-icon>visibility</mat-icon>
                    <span>View Details</span>
                  </button>
                  @if (bill.status === 'Draft') {
                    <button mat-menu-item (click)="submitForApproval(bill)">
                      <mat-icon>send</mat-icon>
                      <span>Submit for Approval</span>
                    </button>
                  }
                  @if (bill.status === 'Pending') {
                    <button mat-menu-item (click)="approveBill(bill)">
                      <mat-icon>check</mat-icon>
                      <span>Approve</span>
                    </button>
                    <button mat-menu-item (click)="rejectBill(bill)">
                      <mat-icon>close</mat-icon>
                      <span>Reject</span>
                    </button>
                  }
                  @if (bill.status === 'Approved' && !bill.isPosted) {
                    <button mat-menu-item (click)="postBill(bill)">
                      <mat-icon>publish</mat-icon>
                      <span>Post</span>
                    </button>
                  }
                  @if (bill.balanceDue > 0 && bill.isPosted) {
                    <button mat-menu-item (click)="makePayment(bill)">
                      <mat-icon>payment</mat-icon>
                      <span>Make Payment</span>
                    </button>
                  }
                  <mat-divider></mat-divider>
                  <button mat-menu-item (click)="duplicateBill(bill)">
                    <mat-icon>content_copy</mat-icon>
                    <span>Duplicate</span>
                  </button>
                  @if (bill.status === 'Draft') {
                    <button mat-menu-item class="delete-action" (click)="deleteBill(bill)">
                      <mat-icon>delete</mat-icon>
                      <span>Delete</span>
                    </button>
                  }
                </mat-menu>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
          </table>

          <mat-paginator 
            [length]="totalBills()"
            [pageSize]="pageSize()"
            [pageSizeOptions]="[10, 25, 50, 100]"
            (page)="onPageChange($event)"
            showFirstLastButtons>
          </mat-paginator>
        </div>

        <!-- Empty State -->
        @if (filteredBills().length === 0) {
          <div class="empty-state">
            <mat-icon>receipt_long</mat-icon>
            <h3>No bills found</h3>
            <p>Create your first bill to track vendor payables</p>
            <button mat-raised-button color="primary" (click)="openBillDialog()">
              <mat-icon>add</mat-icon>
              New Bill
            </button>
          </div>
        }
      }
    </div>

    <!-- Bill Dialog -->
    <ng-template #billDialog>
      <h2 mat-dialog-title>{{ editingBill() ? 'Edit Bill' : 'New Bill' }}</h2>
      <mat-dialog-content>
        <form [formGroup]="billForm" class="bill-form">
          <!-- Header Section -->
          <div class="form-section">
            <h4>Bill Information</h4>
            <div class="form-row">
              <mat-form-field appearance="outline">
                <mat-label>Vendor</mat-label>
                <mat-select formControlName="vendorId" required>
                  @for (vendor of vendors(); track vendor.id) {
                    <mat-option [value]="vendor.id">{{ vendor.vendorName }}</mat-option>
                  }
                </mat-select>
                @if (billForm.get('vendorId')?.hasError('required')) {
                  <mat-error>Vendor is required</mat-error>
                }
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Period</mat-label>
                <mat-select formControlName="periodId" required>
                  @for (period of periods(); track period.id) {
                    <mat-option [value]="period.id">{{ period.periodName }}</mat-option>
                  }
                </mat-select>
              </mat-form-field>
            </div>

            <div class="form-row">
              <mat-form-field appearance="outline">
                <mat-label>Bill Date</mat-label>
                <input matInput [matDatepicker]="billDatePicker" formControlName="billDate" required>
                <mat-datepicker-toggle matIconSuffix [for]="billDatePicker"></mat-datepicker-toggle>
                <mat-datepicker #billDatePicker></mat-datepicker>
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Due Date</mat-label>
                <input matInput [matDatepicker]="dueDatePicker" formControlName="dueDate" required>
                <mat-datepicker-toggle matIconSuffix [for]="dueDatePicker"></mat-datepicker-toggle>
                <mat-datepicker #dueDatePicker></mat-datepicker>
              </mat-form-field>
            </div>

            <div class="form-row">
              <mat-form-field appearance="outline">
                <mat-label>Reference Number</mat-label>
                <input matInput formControlName="referenceNumber" placeholder="Vendor invoice #">
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Description</mat-label>
                <input matInput formControlName="description">
              </mat-form-field>
            </div>
          </div>

          <!-- Line Items Section -->
          <div class="form-section">
            <div class="section-header">
              <h4>Line Items</h4>
              <button mat-stroked-button color="primary" type="button" (click)="addLine()">
                <mat-icon>add</mat-icon>
                Add Line
              </button>
            </div>

            <div class="lines-container" formArrayName="lines">
              @for (line of linesFormArray.controls; track $index; let i = $index) {
                <div class="line-row" [formGroupName]="i">
                  <mat-form-field appearance="outline" class="account-field">
                    <mat-label>Account</mat-label>
                    <mat-select formControlName="accountId" required>
                      @for (account of expenseAccounts(); track account.id) {
                        <mat-option [value]="account.id">{{ account.accountCode }} - {{ account.accountName }}</mat-option>
                      }
                    </mat-select>
                  </mat-form-field>

                  <mat-form-field appearance="outline" class="description-field">
                    <mat-label>Description</mat-label>
                    <input matInput formControlName="description">
                  </mat-form-field>

                  <mat-form-field appearance="outline" class="quantity-field">
                    <mat-label>Qty</mat-label>
                    <input matInput type="number" formControlName="quantity" (input)="calculateLineAmount(i)">
                  </mat-form-field>

                  <mat-form-field appearance="outline" class="price-field">
                    <mat-label>Unit Price</mat-label>
                    <input matInput type="number" formControlName="unitPrice" (input)="calculateLineAmount(i)">
                    <span matPrefix>$&nbsp;</span>
                  </mat-form-field>

                  <mat-form-field appearance="outline" class="amount-field">
                    <mat-label>Amount</mat-label>
                    <input matInput type="number" formControlName="amount" readonly>
                    <span matPrefix>$&nbsp;</span>
                  </mat-form-field>

                  <button mat-icon-button color="warn" type="button" (click)="removeLine(i)" [disabled]="linesFormArray.length === 1">
                    <mat-icon>delete</mat-icon>
                  </button>
                </div>
              }
            </div>

            <!-- Totals -->
            <div class="totals-section">
              <div class="total-row">
                <span class="total-label">Subtotal:</span>
                <span class="total-value">{{ calculateSubtotal() | currency }}</span>
              </div>
              <div class="total-row">
                <span class="total-label">Tax:</span>
                <span class="total-value">{{ billForm.get('taxAmount')?.value | currency }}</span>
              </div>
              <mat-divider></mat-divider>
              <div class="total-row grand-total">
                <span class="total-label">Total:</span>
                <span class="total-value">{{ calculateTotal() | currency }}</span>
              </div>
            </div>
          </div>
        </form>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button mat-dialog-close>Cancel</button>
        <button mat-stroked-button color="primary" (click)="saveAsDraft()" [disabled]="isSaving()">
          Save as Draft
        </button>
        <button mat-raised-button 
                color="primary" 
                [disabled]="billForm.invalid || isSaving()"
                (click)="saveBill()">
          @if (isSaving()) {
            <mat-spinner diameter="20"></mat-spinner>
          }
          {{ editingBill() ? 'Update' : 'Save & Submit' }}
        </button>
      </mat-dialog-actions>
    </ng-template>
  `,
  styles: [`
    .bills-container {
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

    .summary-icon.unpaid {
      background: #fef3c7;
      color: #d97706;
    }

    .summary-icon.overdue {
      background: #fee2e2;
      color: #dc2626;
    }

    .summary-icon.pending {
      background: #e0e7ff;
      color: #4f46e5;
    }

    .summary-icon.paid {
      background: #dcfce7;
      color: #16a34a;
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

    .bill-number {
      font-weight: 500;
      color: var(--primary-color);
    }

    .vendor-info {
      display: flex;
      flex-direction: column;
    }

    .vendor-name {
      font-weight: 500;
    }

    .vendor-code {
      font-size: 12px;
      color: var(--text-secondary);
    }

    .overdue {
      color: #dc2626;
      font-weight: 500;
    }

    .has-balance {
      color: #d97706;
      font-weight: 500;
    }

    .status-draft { background: #f3f4f6 !important; color: #374151 !important; }
    .status-pending { background: #fef3c7 !important; color: #92400e !important; }
    .status-approved { background: #dcfce7 !important; color: #166534 !important; }
    .status-rejected { background: #fee2e2 !important; color: #991b1b !important; }

    .payment-unpaid { background: #fef3c7 !important; color: #92400e !important; }
    .payment-partiallypaid { background: #e0e7ff !important; color: #3730a3 !important; }
    .payment-paid { background: #dcfce7 !important; color: #166534 !important; }

    .delete-action {
      color: #dc2626;
    }

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

    /* Dialog Styles */
    .bill-form {
      display: flex;
      flex-direction: column;
      gap: 24px;
      min-width: 700px;
      max-width: 900px;
    }

    .form-section {
      background: var(--surface-color);
      padding: 16px;
      border-radius: 8px;
      border: 1px solid var(--border-color);
    }

    .form-section h4 {
      margin: 0 0 16px;
      color: var(--text-primary);
      font-weight: 500;
    }

    .section-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 16px;
    }

    .section-header h4 {
      margin: 0;
    }

    .form-row {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 16px;
    }

    .lines-container {
      display: flex;
      flex-direction: column;
      gap: 8px;
    }

    .line-row {
      display: flex;
      gap: 8px;
      align-items: flex-start;
    }

    .account-field {
      flex: 2;
    }

    .description-field {
      flex: 2;
    }

    .quantity-field {
      width: 80px;
    }

    .price-field {
      width: 120px;
    }

    .amount-field {
      width: 120px;
    }

    .totals-section {
      margin-top: 16px;
      padding-top: 16px;
      border-top: 1px solid var(--border-color);
    }

    .total-row {
      display: flex;
      justify-content: flex-end;
      gap: 24px;
      padding: 8px 0;
    }

    .total-label {
      font-weight: 500;
      min-width: 80px;
      text-align: right;
    }

    .total-value {
      min-width: 120px;
      text-align: right;
    }

    .grand-total {
      font-size: 18px;
      font-weight: 600;
      color: var(--primary-color);
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

      .bill-form {
        min-width: auto;
      }

      .form-row {
        grid-template-columns: 1fr;
      }

      .line-row {
        flex-wrap: wrap;
      }
    }
  `]
})
export class BillsComponent implements OnInit {
  @ViewChild('billDialog') billDialogTemplate!: TemplateRef<any>;

  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private notification = inject(NotificationService);
  private accountingService = inject(AccountingService);

  // State signals
  bills = signal<Bill[]>([]);
  vendors = signal<Vendor[]>([]);
  periods = signal<AccountingPeriod[]>([]);
  expenseAccounts = signal<ChartOfAccount[]>([]);
  isLoading = signal(false);
  isSaving = signal(false);
  searchQuery = signal('');
  selectedVendor = signal('');
  selectedStatus = signal('');
  selectedPaymentStatus = signal('');
  editingBill = signal<Bill | null>(null);

  // Pagination
  pageSize = signal(10);
  pageIndex = signal(0);
  totalBills = signal(0);

  displayedColumns = ['billNumber', 'vendor', 'billDate', 'dueDate', 'amount', 'balance', 'status', 'paymentStatus', 'actions'];

  billForm: FormGroup = this.fb.group({
    vendorId: ['', Validators.required],
    periodId: ['', Validators.required],
    billDate: [new Date(), Validators.required],
    dueDate: [new Date(), Validators.required],
    referenceNumber: [''],
    description: [''],
    taxAmount: [0],
    lines: this.fb.array([])
  });

  get linesFormArray(): FormArray {
    return this.billForm.get('lines') as FormArray;
  }

  // Computed signals
  filteredBills = computed(() => {
    let result = this.bills();
    
    const query = this.searchQuery().toLowerCase();
    if (query) {
      result = result.filter(b => 
        b.billNumber.toLowerCase().includes(query) ||
        b.vendorName.toLowerCase().includes(query)
      );
    }
    
    const vendorId = this.selectedVendor();
    if (vendorId) {
      result = result.filter(b => b.vendorId === vendorId);
    }

    const status = this.selectedStatus();
    if (status) {
      result = result.filter(b => b.status === status);
    }

    const paymentStatus = this.selectedPaymentStatus();
    if (paymentStatus) {
      result = result.filter(b => b.paymentStatus === paymentStatus);
    }
    
    return result;
  });

  totalUnpaid = computed(() => 
    this.bills().filter(b => b.paymentStatus !== 'Paid').reduce((sum, b) => sum + b.balanceDue, 0)
  );

  totalOverdue = computed(() => 
    this.bills().filter(b => this.isOverdue(b)).reduce((sum, b) => sum + b.balanceDue, 0)
  );

  pendingCount = computed(() => 
    this.bills().filter(b => b.status === 'Pending').length
  );

  paidThisMonth = computed(() => {
    const now = new Date();
    const startOfMonth = new Date(now.getFullYear(), now.getMonth(), 1);
    return this.bills()
      .filter(b => b.paymentStatus === 'Paid' && new Date(b.billDate) >= startOfMonth)
      .reduce((sum, b) => sum + b.paidAmount, 0);
  });

  ngOnInit(): void {
    this.loadData();
  }

  async loadData(): Promise<void> {
    this.isLoading.set(true);
    
    try {
      await Promise.all([
        this.loadBills(),
        this.loadVendors(),
        this.loadPeriods(),
        this.loadExpenseAccounts()
      ]);
    } finally {
      this.isLoading.set(false);
    }
  }

  private async loadBills(): Promise<void> {
    // Mock data - replace with actual API call
    const mockBills: Bill[] = [
      {
        id: '1',
        billNumber: 'BILL-001',
        vendorId: '1',
        vendorCode: 'V001',
        vendorName: 'ABC Supplies',
        billDate: new Date('2024-01-15'),
        dueDate: new Date('2024-02-15'),
        referenceNumber: 'INV-12345',
        description: 'Office supplies',
        status: ApprovalStatus.Approved,
        paymentStatus: PaymentStatus.Unpaid,
        subtotal: 1500,
        taxAmount: 120,
        totalAmount: 1620,
        paidAmount: 0,
        balanceDue: 1620,
        periodId: '1',
        periodName: 'January 2024',
        isPosted: true,
        isActive: true,
        lines: []
      },
      {
        id: '2',
        billNumber: 'BILL-002',
        vendorId: '2',
        vendorCode: 'V002',
        vendorName: 'XYZ Services',
        billDate: new Date('2024-01-20'),
        dueDate: new Date('2024-01-25'),
        referenceNumber: 'SVC-789',
        description: 'Consulting services',
        status: ApprovalStatus.Approved,
        paymentStatus: PaymentStatus.PartiallyPaid,
        subtotal: 5000,
        taxAmount: 0,
        totalAmount: 5000,
        paidAmount: 2000,
        balanceDue: 3000,
        periodId: '1',
        periodName: 'January 2024',
        isPosted: true,
        isActive: true,
        lines: []
      },
      {
        id: '3',
        billNumber: 'BILL-003',
        vendorId: '1',
        vendorCode: 'V001',
        vendorName: 'ABC Supplies',
        billDate: new Date('2024-01-22'),
        dueDate: new Date('2024-02-22'),
        status: ApprovalStatus.Pending,
        paymentStatus: PaymentStatus.Unpaid,
        subtotal: 750,
        taxAmount: 60,
        totalAmount: 810,
        paidAmount: 0,
        balanceDue: 810,
        periodId: '1',
        periodName: 'January 2024',
        isPosted: false,
        isActive: true,
        lines: []
      }
    ];
    
    await new Promise(resolve => setTimeout(resolve, 300));
    this.bills.set(mockBills);
    this.totalBills.set(mockBills.length);
  }

  private async loadVendors(): Promise<void> {
    const mockVendors: Vendor[] = [
      { id: '1', vendorCode: 'V001', vendorName: 'ABC Supplies', isActive: true, currentBalance: 1620, is1099Vendor: false },
      { id: '2', vendorCode: 'V002', vendorName: 'XYZ Services', isActive: true, currentBalance: 3000, is1099Vendor: true },
      { id: '3', vendorCode: 'V003', vendorName: 'Tech Solutions', isActive: true, currentBalance: 0, is1099Vendor: false }
    ];
    this.vendors.set(mockVendors);
  }

  private async loadPeriods(): Promise<void> {
    const mockPeriods: AccountingPeriod[] = [
      { id: '1', periodNumber: 1, periodName: 'January 2024', fiscalYear: 2024, startDate: new Date('2024-01-01'), endDate: new Date('2024-01-31'), status: 'Open' as any, isCurrent: true, isAdjustmentPeriod: false, isActive: true }
    ];
    this.periods.set(mockPeriods);
  }

  private async loadExpenseAccounts(): Promise<void> {
    const mockAccounts: ChartOfAccount[] = [
      { id: '1', accountCode: '5000', accountName: 'Cost of Goods Sold', accountType: 'Expense' as any, currentBalance: 0, isHeader: false, isBankAccount: false, level: 1, sortOrder: 1, normalBalance: 'Debit', isActive: true },
      { id: '2', accountCode: '5100', accountName: 'Supplies Expense', accountType: 'Expense' as any, currentBalance: 0, isHeader: false, isBankAccount: false, level: 1, sortOrder: 2, normalBalance: 'Debit', isActive: true },
      { id: '3', accountCode: '5200', accountName: 'Professional Services', accountType: 'Expense' as any, currentBalance: 0, isHeader: false, isBankAccount: false, level: 1, sortOrder: 3, normalBalance: 'Debit', isActive: true }
    ];
    this.expenseAccounts.set(mockAccounts);
  }

  onSearch(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.searchQuery.set(value);
  }

  onVendorChange(value: string): void {
    this.selectedVendor.set(value);
  }

  onStatusChange(value: string): void {
    this.selectedStatus.set(value);
  }

  onPaymentStatusChange(value: string): void {
    this.selectedPaymentStatus.set(value);
  }

  onSort(sort: Sort): void {
    // Implement sorting logic
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex.set(event.pageIndex);
    this.pageSize.set(event.pageSize);
  }

  isOverdue(bill: Bill): boolean {
    return new Date(bill.dueDate) < new Date() && bill.balanceDue > 0;
  }

  getStatusClass(status: ApprovalStatus): string {
    return `status-${status.toLowerCase()}`;
  }

  getPaymentStatusClass(status: PaymentStatus): string {
    return `payment-${status.toLowerCase()}`;
  }

  openBillDialog(bill?: Bill): void {
    this.editingBill.set(bill || null);
    
    // Reset form
    this.linesFormArray.clear();
    
    if (bill) {
      this.billForm.patchValue({
        vendorId: bill.vendorId,
        periodId: bill.periodId,
        billDate: bill.billDate,
        dueDate: bill.dueDate,
        referenceNumber: bill.referenceNumber,
        description: bill.description,
        taxAmount: bill.taxAmount
      });
      
      bill.lines.forEach(line => {
        this.addLine(line);
      });
    } else {
      this.billForm.reset({
        billDate: new Date(),
        dueDate: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000),
        taxAmount: 0
      });
      this.addLine();
    }

    this.dialog.open(this.billDialogTemplate, {
      width: '900px',
      maxHeight: '90vh'
    });
  }

  addLine(line?: any): void {
    const lineForm = this.fb.group({
      accountId: [line?.accountId || '', Validators.required],
      description: [line?.description || ''],
      quantity: [line?.quantity || 1, [Validators.required, Validators.min(0)]],
      unitPrice: [line?.unitPrice || 0, [Validators.required, Validators.min(0)]],
      amount: [{ value: line?.amount || 0, disabled: false }],
      taxCodeId: [line?.taxCodeId || '']
    });
    
    this.linesFormArray.push(lineForm);
  }

  removeLine(index: number): void {
    this.linesFormArray.removeAt(index);
  }

  calculateLineAmount(index: number): void {
    const line = this.linesFormArray.at(index);
    const quantity = line.get('quantity')?.value || 0;
    const unitPrice = line.get('unitPrice')?.value || 0;
    line.get('amount')?.setValue(quantity * unitPrice);
  }

  calculateSubtotal(): number {
    return this.linesFormArray.controls.reduce((sum, line) => {
      return sum + (line.get('amount')?.value || 0);
    }, 0);
  }

  calculateTotal(): number {
    return this.calculateSubtotal() + (this.billForm.get('taxAmount')?.value || 0);
  }

  async saveAsDraft(): Promise<void> {
    this.isSaving.set(true);
    try {
      await new Promise(resolve => setTimeout(resolve, 500));
      this.notification.success('Bill saved as draft');
      this.dialog.closeAll();
      this.loadBills();
    } catch (error) {
      this.notification.error('Failed to save bill');
    } finally {
      this.isSaving.set(false);
    }
  }

  async saveBill(): Promise<void> {
    if (this.billForm.invalid) return;

    this.isSaving.set(true);
    
    try {
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      if (this.editingBill()) {
        this.notification.success('Bill updated successfully');
      } else {
        this.notification.success('Bill created and submitted for approval');
      }
      
      this.dialog.closeAll();
      this.loadBills();
    } catch (error) {
      this.notification.error('Failed to save bill');
    } finally {
      this.isSaving.set(false);
    }
  }

  viewBill(bill: Bill): void {
    console.log('View bill:', bill);
  }

  async submitForApproval(bill: Bill): Promise<void> {
    try {
      await new Promise(resolve => setTimeout(resolve, 500));
      this.notification.success('Bill submitted for approval');
      this.loadBills();
    } catch (error) {
      this.notification.error('Failed to submit bill');
    }
  }

  async approveBill(bill: Bill): Promise<void> {
    try {
      await new Promise(resolve => setTimeout(resolve, 500));
      this.notification.success('Bill approved');
      this.loadBills();
    } catch (error) {
      this.notification.error('Failed to approve bill');
    }
  }

  async rejectBill(bill: Bill): Promise<void> {
    try {
      await new Promise(resolve => setTimeout(resolve, 500));
      this.notification.success('Bill rejected');
      this.loadBills();
    } catch (error) {
      this.notification.error('Failed to reject bill');
    }
  }

  async postBill(bill: Bill): Promise<void> {
    try {
      await new Promise(resolve => setTimeout(resolve, 500));
      this.notification.success('Bill posted successfully');
      this.loadBills();
    } catch (error) {
      this.notification.error('Failed to post bill');
    }
  }

  makePayment(bill: Bill): void {
    // Navigate to payment screen or open payment dialog
    console.log('Make payment for:', bill);
  }

  duplicateBill(bill: Bill): void {
    const duplicate = { ...bill, id: '', billNumber: '' };
    this.openBillDialog(duplicate);
  }

  async deleteBill(bill: Bill): Promise<void> {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete Bill',
        message: `Are you sure you want to delete bill "${bill.billNumber}"?`,
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(async result => {
      if (result) {
        try {
          await new Promise(resolve => setTimeout(resolve, 500));
          this.bills.update(bills => bills.filter(b => b.id !== bill.id));
          this.notification.success('Bill deleted successfully');
        } catch (error) {
          this.notification.error('Failed to delete bill');
        }
      }
    });
  }
}

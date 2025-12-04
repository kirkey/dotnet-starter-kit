import { Component, inject, signal, OnInit, ViewChild, TemplateRef, computed } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
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
import { MatRadioModule } from '@angular/material/radio';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';
import { NotificationService } from '@core/services/notification.service';
import { AccountingService } from '@core/services/accounting.service';
import { Check, CheckStatus, Bank, Vendor } from '@core/models/accounting.model';

@Component({
  selector: 'app-checks',
  standalone: true,
  imports: [
    CommonModule,
    CurrencyPipe,
    DatePipe,
    FormsModule,
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
    MatRadioModule,
    PageHeaderComponent
  ],
  template: `
    <div class="checks-container">
      <app-page-header 
        title="Checks" 
        subtitle="Manage check payments"
        icon="payments">
      </app-page-header>

      <!-- Toolbar -->
      <div class="toolbar">
        <div class="filters">
          <mat-form-field appearance="outline" class="search-field">
            <mat-label>Search checks</mat-label>
            <input matInput 
                   [value]="searchQuery()" 
                   (input)="onSearch($event)"
                   placeholder="Search by check number, payee...">
            <mat-icon matPrefix>search</mat-icon>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Bank Account</mat-label>
            <mat-select [value]="selectedBank()" (selectionChange)="onBankChange($event.value)">
              <mat-option value="">All Accounts</mat-option>
              @for (bank of banks(); track bank.id) {
                <mat-option [value]="bank.id">{{ bank.bankName }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Status</mat-label>
            <mat-select [value]="selectedStatus()" (selectionChange)="onStatusChange($event.value)">
              <mat-option value="">All Statuses</mat-option>
              <mat-option value="Issued">Issued</mat-option>
              <mat-option value="Cleared">Cleared</mat-option>
              <mat-option value="Voided">Voided</mat-option>
              <mat-option value="StopPayment">Stop Payment</mat-option>
            </mat-select>
          </mat-form-field>
        </div>

        <div class="actions">
          <button mat-raised-button color="primary" (click)="openCheckDialog()">
            <mat-icon>add</mat-icon>
            Write Check
          </button>
        </div>
      </div>

      <!-- Summary Cards -->
      <div class="summary-cards">
        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon issued">
              <mat-icon>send</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Outstanding</span>
              <span class="summary-value">{{ outstandingAmount() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon cleared">
              <mat-icon>check_circle</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Cleared This Month</span>
              <span class="summary-value">{{ clearedThisMonth() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon count">
              <mat-icon>receipt</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Checks Written</span>
              <span class="summary-value">{{ checksThisMonth() }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon voided">
              <mat-icon>block</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Voided</span>
              <span class="summary-value">{{ voidedCount() }}</span>
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
          <table mat-table [dataSource]="filteredChecks()" matSort (matSortChange)="onSort($event)">
            <!-- Check Number Column -->
            <ng-container matColumnDef="checkNumber">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Check #</th>
              <td mat-cell *matCellDef="let check">
                <span class="check-number">{{ check.checkNumber }}</span>
              </td>
            </ng-container>

            <!-- Bank Column -->
            <ng-container matColumnDef="bank">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Bank Account</th>
              <td mat-cell *matCellDef="let check">{{ check.bankName }}</td>
            </ng-container>

            <!-- Check Date Column -->
            <ng-container matColumnDef="checkDate">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Date</th>
              <td mat-cell *matCellDef="let check">{{ check.checkDate | date:'mediumDate' }}</td>
            </ng-container>

            <!-- Payee Column -->
            <ng-container matColumnDef="payee">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Payee</th>
              <td mat-cell *matCellDef="let check">
                <div class="payee-info">
                  <span class="payee-name">{{ check.payeeName }}</span>
                  <span class="payee-type">{{ check.payeeType }}</span>
                </div>
              </td>
            </ng-container>

            <!-- Amount Column -->
            <ng-container matColumnDef="amount">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Amount</th>
              <td mat-cell *matCellDef="let check">{{ check.amount | currency }}</td>
            </ng-container>

            <!-- Memo Column -->
            <ng-container matColumnDef="memo">
              <th mat-header-cell *matHeaderCellDef>Memo</th>
              <td mat-cell *matCellDef="let check">{{ check.memo || '-' }}</td>
            </ng-container>

            <!-- Status Column -->
            <ng-container matColumnDef="status">
              <th mat-header-cell *matHeaderCellDef>Status</th>
              <td mat-cell *matCellDef="let check">
                <mat-chip [class]="getStatusClass(check.status)">{{ getStatusLabel(check.status) }}</mat-chip>
              </td>
            </ng-container>

            <!-- Cleared Date Column -->
            <ng-container matColumnDef="clearedDate">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Cleared</th>
              <td mat-cell *matCellDef="let check">
                {{ check.clearedDate ? (check.clearedDate | date:'mediumDate') : '-' }}
              </td>
            </ng-container>

            <!-- Actions Column -->
            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef></th>
              <td mat-cell *matCellDef="let check">
                <button mat-icon-button [matMenuTriggerFor]="menu">
                  <mat-icon>more_vert</mat-icon>
                </button>
                <mat-menu #menu="matMenu">
                  <button mat-menu-item (click)="viewCheck(check)">
                    <mat-icon>visibility</mat-icon>
                    <span>View Details</span>
                  </button>
                  <button mat-menu-item (click)="printCheck(check)">
                    <mat-icon>print</mat-icon>
                    <span>Print</span>
                  </button>
                  @if (check.status === 'Issued') {
                    <button mat-menu-item (click)="clearCheck(check)">
                      <mat-icon>check_circle</mat-icon>
                      <span>Mark as Cleared</span>
                    </button>
                    <button mat-menu-item (click)="voidCheck(check)">
                      <mat-icon>block</mat-icon>
                      <span>Void Check</span>
                    </button>
                    <button mat-menu-item (click)="stopPayment(check)">
                      <mat-icon>cancel</mat-icon>
                      <span>Stop Payment</span>
                    </button>
                  }
                  @if (check.status === 'Cleared') {
                    <button mat-menu-item (click)="unclearCheck(check)">
                      <mat-icon>undo</mat-icon>
                      <span>Unclear</span>
                    </button>
                  }
                </mat-menu>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns;" [class.voided]="row.status === 'Voided'"></tr>
          </table>

          <mat-paginator 
            [length]="totalChecks()"
            [pageSize]="pageSize()"
            [pageSizeOptions]="[10, 25, 50, 100]"
            (page)="onPageChange($event)"
            showFirstLastButtons>
          </mat-paginator>
        </div>

        <!-- Empty State -->
        @if (filteredChecks().length === 0) {
          <div class="empty-state">
            <mat-icon>payments</mat-icon>
            <h3>No checks found</h3>
            <p>Write your first check to start tracking payments</p>
            <button mat-raised-button color="primary" (click)="openCheckDialog()">
              <mat-icon>add</mat-icon>
              Write Check
            </button>
          </div>
        }
      }
    </div>

    <!-- Check Dialog -->
    <ng-template #checkDialog>
      <h2 mat-dialog-title>Write Check</h2>
      <mat-dialog-content>
        <form [formGroup]="checkForm" class="check-form">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Bank Account</mat-label>
            <mat-select formControlName="bankId" required>
              @for (bank of banks(); track bank.id) {
                <mat-option [value]="bank.id">
                  {{ bank.bankName }} ({{ bank.accountNumber }}) - {{ bank.currentBalance | currency }}
                </mat-option>
              }
            </mat-select>
            @if (checkForm.get('bankId')?.hasError('required')) {
              <mat-error>Bank account is required</mat-error>
            }
          </mat-form-field>

          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Check Date</mat-label>
              <input matInput [matDatepicker]="checkDatePicker" formControlName="checkDate" required>
              <mat-datepicker-toggle matIconSuffix [for]="checkDatePicker"></mat-datepicker-toggle>
              <mat-datepicker #checkDatePicker></mat-datepicker>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Check Number</mat-label>
              <input matInput formControlName="checkNumber" placeholder="Auto-generated if blank">
            </mat-form-field>
          </div>

          <div class="payee-type-section">
            <label>Payee Type</label>
            <mat-radio-group formControlName="payeeType" class="payee-type-group">
              <mat-radio-button value="Vendor">Vendor</mat-radio-button>
              <mat-radio-button value="Employee">Employee</mat-radio-button>
              <mat-radio-button value="Other">Other</mat-radio-button>
            </mat-radio-group>
          </div>

          @if (checkForm.get('payeeType')?.value === 'Vendor') {
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Select Vendor</mat-label>
              <mat-select formControlName="payeeId" (selectionChange)="onVendorSelect($event.value)">
                @for (vendor of vendors(); track vendor.id) {
                  <mat-option [value]="vendor.id">{{ vendor.vendorName }}</mat-option>
                }
              </mat-select>
            </mat-form-field>
          }

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Pay to the Order of</mat-label>
            <input matInput formControlName="payeeName" required>
            @if (checkForm.get('payeeName')?.hasError('required')) {
              <mat-error>Payee name is required</mat-error>
            }
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Address</mat-label>
            <textarea matInput formControlName="payeeAddress" rows="2"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Amount</mat-label>
            <input matInput type="number" formControlName="amount" required>
            <span matPrefix>$&nbsp;</span>
            @if (checkForm.get('amount')?.hasError('required')) {
              <mat-error>Amount is required</mat-error>
            }
            @if (checkForm.get('amount')?.hasError('min')) {
              <mat-error>Amount must be greater than 0</mat-error>
            }
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Memo</mat-label>
            <input matInput formControlName="memo" placeholder="Payment description">
          </mat-form-field>

          <div class="amount-in-words">
            <label>Amount in Words:</label>
            <span class="words">{{ getAmountInWords() }}</span>
          </div>
        </form>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button mat-dialog-close>Cancel</button>
        <button mat-stroked-button color="primary" (click)="printAndSave()" [disabled]="checkForm.invalid || isSaving()">
          <mat-icon>print</mat-icon>
          Print Check
        </button>
        <button mat-raised-button 
                color="primary" 
                [disabled]="checkForm.invalid || isSaving()"
                (click)="saveCheck()">
          @if (isSaving()) {
            <mat-spinner diameter="20"></mat-spinner>
          }
          Save Check
        </button>
      </mat-dialog-actions>
    </ng-template>

    <!-- Void Check Dialog -->
    <ng-template #voidDialog>
      <h2 mat-dialog-title>Void Check</h2>
      <mat-dialog-content>
        <p>Are you sure you want to void check #{{ checkToVoid()?.checkNumber }}?</p>
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Reason for Voiding</mat-label>
          <textarea matInput [(ngModel)]="voidReason" rows="3" required></textarea>
        </mat-form-field>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button mat-dialog-close>Cancel</button>
        <button mat-raised-button color="warn" [disabled]="!voidReason" (click)="confirmVoid()">
          Void Check
        </button>
      </mat-dialog-actions>
    </ng-template>
  `,
  styles: [`
    .checks-container {
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

    .summary-icon.issued {
      background: #fef3c7;
      color: #d97706;
    }

    .summary-icon.cleared {
      background: #dcfce7;
      color: #16a34a;
    }

    .summary-icon.count {
      background: #e0e7ff;
      color: #4f46e5;
    }

    .summary-icon.voided {
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

    .check-number {
      font-weight: 500;
      font-family: monospace;
    }

    .payee-info {
      display: flex;
      flex-direction: column;
    }

    .payee-name {
      font-weight: 500;
    }

    .payee-type {
      font-size: 12px;
      color: var(--text-secondary);
    }

    tr.voided {
      opacity: 0.6;
      text-decoration: line-through;
    }

    .status-issued { background: #fef3c7 !important; color: #92400e !important; }
    .status-cleared { background: #dcfce7 !important; color: #166534 !important; }
    .status-voided { background: #fee2e2 !important; color: #991b1b !important; }
    .status-stoppayment { background: #fce7f3 !important; color: #9d174d !important; }

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
    .check-form {
      display: flex;
      flex-direction: column;
      gap: 16px;
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

    .payee-type-section {
      margin: 8px 0;
    }

    .payee-type-section label {
      display: block;
      font-size: 12px;
      color: var(--text-secondary);
      margin-bottom: 8px;
    }

    .payee-type-group {
      display: flex;
      gap: 24px;
    }

    .amount-in-words {
      padding: 12px;
      background: var(--surface-color);
      border: 1px solid var(--border-color);
      border-radius: 8px;
    }

    .amount-in-words label {
      font-size: 12px;
      color: var(--text-secondary);
      display: block;
      margin-bottom: 4px;
    }

    .amount-in-words .words {
      font-style: italic;
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

      .check-form {
        min-width: auto;
      }

      .form-row {
        grid-template-columns: 1fr;
      }

      .payee-type-group {
        flex-direction: column;
        gap: 8px;
      }
    }
  `]
})
export class ChecksComponent implements OnInit {
  @ViewChild('checkDialog') checkDialogTemplate!: TemplateRef<any>;
  @ViewChild('voidDialog') voidDialogTemplate!: TemplateRef<any>;

  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private notification = inject(NotificationService);
  private accountingService = inject(AccountingService);

  // State signals
  checks = signal<Check[]>([]);
  banks = signal<Bank[]>([]);
  vendors = signal<Vendor[]>([]);
  isLoading = signal(false);
  isSaving = signal(false);
  searchQuery = signal('');
  selectedBank = signal('');
  selectedStatus = signal('');
  checkToVoid = signal<Check | null>(null);
  voidReason = '';

  // Pagination
  pageSize = signal(10);
  pageIndex = signal(0);
  totalChecks = signal(0);

  displayedColumns = ['checkNumber', 'bank', 'checkDate', 'payee', 'amount', 'memo', 'status', 'clearedDate', 'actions'];

  checkForm: FormGroup = this.fb.group({
    bankId: ['', Validators.required],
    checkDate: [new Date(), Validators.required],
    checkNumber: [''],
    payeeType: ['Vendor', Validators.required],
    payeeId: [''],
    payeeName: ['', Validators.required],
    payeeAddress: [''],
    amount: [0, [Validators.required, Validators.min(0.01)]],
    memo: ['']
  });

  // Computed signals
  filteredChecks = computed(() => {
    let result = this.checks();
    
    const query = this.searchQuery().toLowerCase();
    if (query) {
      result = result.filter(c => 
        c.checkNumber.toLowerCase().includes(query) ||
        c.payeeName.toLowerCase().includes(query)
      );
    }
    
    const bankId = this.selectedBank();
    if (bankId) {
      result = result.filter(c => c.bankId === bankId);
    }

    const status = this.selectedStatus();
    if (status) {
      result = result.filter(c => c.status === status);
    }
    
    return result;
  });

  outstandingAmount = computed(() => 
    this.checks().filter(c => c.status === CheckStatus.Issued).reduce((sum, c) => sum + c.amount, 0)
  );

  clearedThisMonth = computed(() => {
    const now = new Date();
    const startOfMonth = new Date(now.getFullYear(), now.getMonth(), 1);
    return this.checks()
      .filter(c => c.status === CheckStatus.Cleared && c.clearedDate && new Date(c.clearedDate) >= startOfMonth)
      .reduce((sum, c) => sum + c.amount, 0);
  });

  checksThisMonth = computed(() => {
    const now = new Date();
    const startOfMonth = new Date(now.getFullYear(), now.getMonth(), 1);
    return this.checks().filter(c => new Date(c.checkDate) >= startOfMonth).length;
  });

  voidedCount = computed(() => 
    this.checks().filter(c => c.status === CheckStatus.Voided).length
  );

  ngOnInit(): void {
    this.loadData();
  }

  async loadData(): Promise<void> {
    this.isLoading.set(true);
    
    try {
      await Promise.all([
        this.loadChecks(),
        this.loadBanks(),
        this.loadVendors()
      ]);
    } finally {
      this.isLoading.set(false);
    }
  }

  private async loadChecks(): Promise<void> {
    const mockChecks: Check[] = [
      {
        id: '1',
        checkNumber: '1001',
        bankId: '1',
        bankCode: 'BOA-001',
        bankName: 'Bank of America - Operating',
        checkDate: new Date('2024-01-15'),
        payeeType: 'Vendor',
        payeeId: '1',
        payeeName: 'ABC Supplies',
        payeeAddress: '123 Main St, Anytown, USA',
        amount: 1500,
        memo: 'Office supplies',
        status: CheckStatus.Issued,
        isActive: true
      },
      {
        id: '2',
        checkNumber: '1002',
        bankId: '1',
        bankCode: 'BOA-001',
        bankName: 'Bank of America - Operating',
        checkDate: new Date('2024-01-10'),
        payeeType: 'Vendor',
        payeeId: '2',
        payeeName: 'XYZ Services',
        amount: 2500,
        memo: 'Consulting services',
        status: CheckStatus.Cleared,
        clearedDate: new Date('2024-01-18'),
        isActive: true
      },
      {
        id: '3',
        checkNumber: '1003',
        bankId: '1',
        bankCode: 'BOA-001',
        bankName: 'Bank of America - Operating',
        checkDate: new Date('2024-01-05'),
        payeeType: 'Other',
        payeeName: 'Utilities Company',
        amount: 350,
        memo: 'Monthly utilities',
        status: CheckStatus.Voided,
        voidedDate: new Date('2024-01-06'),
        voidReason: 'Incorrect amount',
        isActive: true
      }
    ];
    
    await new Promise(resolve => setTimeout(resolve, 300));
    this.checks.set(mockChecks);
    this.totalChecks.set(mockChecks.length);
  }

  private async loadBanks(): Promise<void> {
    const mockBanks: Bank[] = [
      { id: '1', bankCode: 'BOA-001', bankName: 'Bank of America - Operating', accountNumber: '****1234', accountType: 'Checking', glAccountId: '1', glAccountCode: '1000', glAccountName: 'Cash', currentBalance: 50000, isActive: true },
      { id: '2', bankCode: 'CHASE-001', bankName: 'Chase - Payroll', accountNumber: '****5678', accountType: 'Checking', glAccountId: '2', glAccountCode: '1010', glAccountName: 'Payroll Cash', currentBalance: 25000, isActive: true }
    ];
    this.banks.set(mockBanks);
  }

  private async loadVendors(): Promise<void> {
    const mockVendors: Vendor[] = [
      { id: '1', vendorCode: 'V001', vendorName: 'ABC Supplies', address1: '123 Main St', city: 'Anytown', state: 'CA', postalCode: '12345', isActive: true, currentBalance: 0, is1099Vendor: false },
      { id: '2', vendorCode: 'V002', vendorName: 'XYZ Services', address1: '456 Oak Ave', city: 'Somewhere', state: 'NY', postalCode: '67890', isActive: true, currentBalance: 0, is1099Vendor: true }
    ];
    this.vendors.set(mockVendors);
  }

  onSearch(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.searchQuery.set(value);
  }

  onBankChange(value: string): void {
    this.selectedBank.set(value);
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

  getStatusClass(status: CheckStatus): string {
    return `status-${status.toLowerCase()}`;
  }

  getStatusLabel(status: CheckStatus): string {
    const labels: Record<string, string> = {
      'Issued': 'Outstanding',
      'Cleared': 'Cleared',
      'Voided': 'Voided',
      'StopPayment': 'Stop Payment'
    };
    return labels[status] || status;
  }

  onVendorSelect(vendorId: string): void {
    const vendor = this.vendors().find(v => v.id === vendorId);
    if (vendor) {
      this.checkForm.patchValue({
        payeeName: vendor.vendorName,
        payeeAddress: [vendor.address1, vendor.city, vendor.state, vendor.postalCode].filter(Boolean).join(', ')
      });
    }
  }

  getAmountInWords(): string {
    const amount = this.checkForm.get('amount')?.value || 0;
    if (amount <= 0) return '---';
    
    const dollars = Math.floor(amount);
    const cents = Math.round((amount - dollars) * 100);
    
    // Simple number to words (for demo purposes)
    const ones = ['', 'One', 'Two', 'Three', 'Four', 'Five', 'Six', 'Seven', 'Eight', 'Nine', 'Ten',
                  'Eleven', 'Twelve', 'Thirteen', 'Fourteen', 'Fifteen', 'Sixteen', 'Seventeen', 'Eighteen', 'Nineteen'];
    const tens = ['', '', 'Twenty', 'Thirty', 'Forty', 'Fifty', 'Sixty', 'Seventy', 'Eighty', 'Ninety'];
    
    const numToWords = (n: number): string => {
      if (n < 20) return ones[n];
      if (n < 100) return tens[Math.floor(n / 10)] + (n % 10 ? '-' + ones[n % 10] : '');
      if (n < 1000) return ones[Math.floor(n / 100)] + ' Hundred' + (n % 100 ? ' ' + numToWords(n % 100) : '');
      if (n < 1000000) return numToWords(Math.floor(n / 1000)) + ' Thousand' + (n % 1000 ? ' ' + numToWords(n % 1000) : '');
      return 'Amount too large';
    };
    
    return `${numToWords(dollars)} and ${cents.toString().padStart(2, '0')}/100 Dollars`;
  }

  openCheckDialog(): void {
    this.checkForm.reset({
      checkDate: new Date(),
      payeeType: 'Vendor',
      amount: 0
    });

    this.dialog.open(this.checkDialogTemplate, {
      width: '600px',
      maxHeight: '90vh'
    });
  }

  async saveCheck(): Promise<void> {
    if (this.checkForm.invalid) return;

    this.isSaving.set(true);
    
    try {
      await new Promise(resolve => setTimeout(resolve, 1000));
      this.notification.success('Check created successfully');
      this.dialog.closeAll();
      this.loadChecks();
    } catch (error) {
      this.notification.error('Failed to save check');
    } finally {
      this.isSaving.set(false);
    }
  }

  async printAndSave(): Promise<void> {
    await this.saveCheck();
    // Trigger print dialog
    window.print();
  }

  viewCheck(check: Check): void {
    console.log('View check:', check);
  }

  printCheck(check: Check): void {
    console.log('Print check:', check);
    window.print();
  }

  async clearCheck(check: Check): Promise<void> {
    try {
      await new Promise(resolve => setTimeout(resolve, 500));
      this.notification.success('Check marked as cleared');
      this.loadChecks();
    } catch (error) {
      this.notification.error('Failed to clear check');
    }
  }

  async unclearCheck(check: Check): Promise<void> {
    try {
      await new Promise(resolve => setTimeout(resolve, 500));
      this.notification.success('Check marked as outstanding');
      this.loadChecks();
    } catch (error) {
      this.notification.error('Failed to unclear check');
    }
  }

  voidCheck(check: Check): void {
    this.checkToVoid.set(check);
    this.voidReason = '';
    this.dialog.open(this.voidDialogTemplate, {
      width: '400px'
    });
  }

  async confirmVoid(): Promise<void> {
    try {
      await new Promise(resolve => setTimeout(resolve, 500));
      this.notification.success('Check voided successfully');
      this.dialog.closeAll();
      this.loadChecks();
    } catch (error) {
      this.notification.error('Failed to void check');
    }
  }

  async stopPayment(check: Check): Promise<void> {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Stop Payment',
        message: `Are you sure you want to stop payment on check #${check.checkNumber}? This action may incur bank fees.`,
        confirmText: 'Stop Payment',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(async result => {
      if (result) {
        try {
          await new Promise(resolve => setTimeout(resolve, 500));
          this.notification.success('Stop payment issued');
          this.loadChecks();
        } catch (error) {
          this.notification.error('Failed to stop payment');
        }
      }
    });
  }
}

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
import { RetainedEarnings, AccountingPeriod, ChartOfAccount } from '@core/models/accounting.model';

@Component({
  selector: 'app-retained-earnings',
  standalone: true,
  imports: [
    CommonModule, CurrencyPipe, DatePipe, ReactiveFormsModule,
    MatTableModule, MatPaginatorModule, MatSortModule, MatButtonModule, MatIconModule,
    MatFormFieldModule, MatInputModule, MatSelectModule, MatDialogModule,
    MatProgressSpinnerModule, MatChipsModule, MatMenuModule, MatTooltipModule,
    MatCardModule, MatDatepickerModule, MatNativeDateModule, PageHeaderComponent
  ],
  template: `
    <div class="retained-container">
      <app-page-header 
        title="Retained Earnings" 
        subtitle="Manage retained earnings and dividend distributions"
        icon="savings">
      </app-page-header>

      <div class="summary-cards">
        <mat-card class="summary-card highlight">
          <mat-card-content>
            <div class="summary-icon balance">
              <mat-icon>account_balance</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Current Retained Earnings</span>
              <span class="summary-value">{{ currentBalance() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon income">
              <mat-icon>trending_up</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">YTD Net Income</span>
              <span class="summary-value">{{ ytdNetIncome() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon dividends">
              <mat-icon>payments</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">YTD Dividends</span>
              <span class="summary-value">{{ ytdDividends() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon adjustments">
              <mat-icon>tune</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Prior Period Adjustments</span>
              <span class="summary-value">{{ totalAdjustments() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>
      </div>

      <div class="toolbar">
        <div class="filters">
          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Fiscal Year</mat-label>
            <mat-select [value]="selectedYear()" (selectionChange)="onYearChange($event.value)">
              @for (year of fiscalYears(); track year) {
                <mat-option [value]="year">{{ year }}</mat-option>
              }
            </mat-select>
          </mat-form-field>
        </div>

        <div class="toolbar-actions">
          <button mat-stroked-button (click)="recordDividend()">
            <mat-icon>payments</mat-icon>
            Record Dividend
          </button>
          <button mat-stroked-button (click)="recordAdjustment()">
            <mat-icon>tune</mat-icon>
            Prior Period Adjustment
          </button>
          <button mat-raised-button color="primary" (click)="closeYear()">
            <mat-icon>lock</mat-icon>
            Close Year
          </button>
        </div>
      </div>

      @if (isLoading()) {
        <div class="loading-container">
          <mat-spinner diameter="48"></mat-spinner>
        </div>
      } @else {
        <div class="table-container">
          <table mat-table [dataSource]="retainedEarnings()" matSort>
            <ng-container matColumnDef="period">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Period</th>
              <td mat-cell *matCellDef="let re">
                <span class="period-name">{{ re.periodName }}</span>
              </td>
            </ng-container>

            <ng-container matColumnDef="beginningBalance">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Beginning Balance</th>
              <td mat-cell *matCellDef="let re">{{ re.beginningBalance | currency }}</td>
            </ng-container>

            <ng-container matColumnDef="netIncome">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Net Income</th>
              <td mat-cell *matCellDef="let re" [class.positive]="re.netIncome > 0" [class.negative]="re.netIncome < 0">
                {{ re.netIncome | currency }}
              </td>
            </ng-container>

            <ng-container matColumnDef="dividends">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Dividends</th>
              <td mat-cell *matCellDef="let re" class="negative">
                @if (re.dividendsDeclared > 0) {
                  ({{ re.dividendsDeclared | currency }})
                } @else {
                  -
                }
              </td>
            </ng-container>

            <ng-container matColumnDef="adjustments">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Adjustments</th>
              <td mat-cell *matCellDef="let re" [class.positive]="re.otherAdjustments > 0" [class.negative]="re.otherAdjustments < 0">
                @if (re.otherAdjustments !== 0) {
                  {{ re.otherAdjustments | currency }}
                } @else {
                  -
                }
              </td>
            </ng-container>

            <ng-container matColumnDef="endingBalance">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Ending Balance</th>
              <td mat-cell *matCellDef="let re" class="ending-balance">{{ re.endingBalance | currency }}</td>
            </ng-container>

            <ng-container matColumnDef="status">
              <th mat-header-cell *matHeaderCellDef>Status</th>
              <td mat-cell *matCellDef="let re">
                @if (re.distributions.length > 0) {
                  <mat-chip class="status-closed">Distributed</mat-chip>
                } @else {
                  <mat-chip class="status-open">Open</mat-chip>
                }
              </td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef></th>
              <td mat-cell *matCellDef="let re">
                <button mat-icon-button [matMenuTriggerFor]="menu">
                  <mat-icon>more_vert</mat-icon>
                </button>
                <mat-menu #menu="matMenu">
                  <button mat-menu-item (click)="viewDetails(re)">
                    <mat-icon>visibility</mat-icon>
                    <span>View Details</span>
                  </button>
                  <button mat-menu-item (click)="viewTransactions(re)">
                    <mat-icon>receipt_long</mat-icon>
                    <span>View Transactions</span>
                  </button>
                  <button mat-menu-item (click)="closePeriod(re)">
                    <mat-icon>lock</mat-icon>
                    <span>Close Period</span>
                  </button>
                </mat-menu>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns;" [class.closed-row]="row.distributions.length > 0"></tr>
          </table>
        </div>

        <mat-card class="reconciliation-card">
          <mat-card-header>
            <mat-card-title>Retained Earnings Reconciliation</mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <div class="reconciliation-grid">
              <div class="recon-item">
                <span class="recon-label">Beginning Retained Earnings</span>
                <span class="recon-value">{{ beginningRetainedEarnings() | currency }}</span>
              </div>
              <div class="recon-item add">
                <span class="recon-label">Add: Net Income (YTD)</span>
                <span class="recon-value">{{ ytdNetIncome() | currency }}</span>
              </div>
              <div class="recon-item subtract">
                <span class="recon-label">Less: Dividends</span>
                <span class="recon-value">({{ ytdDividends() | currency }})</span>
              </div>
              <div class="recon-item">
                <span class="recon-label">Prior Period Adjustments</span>
                <span class="recon-value">{{ totalAdjustments() | currency }}</span>
              </div>
              <div class="recon-item total">
                <span class="recon-label">Ending Retained Earnings</span>
                <span class="recon-value">{{ currentBalance() | currency }}</span>
              </div>
            </div>
          </mat-card-content>
        </mat-card>
      }
    </div>

    <ng-template #dividendDialog>
      <h2 mat-dialog-title>Record Dividend</h2>
      <mat-dialog-content>
        <form [formGroup]="dividendForm" class="dividend-form">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Date</mat-label>
            <input matInput [matDatepicker]="picker" formControlName="date" required>
            <mat-datepicker-toggle matIconSuffix [for]="picker"></mat-datepicker-toggle>
            <mat-datepicker #picker></mat-datepicker>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Amount</mat-label>
            <input matInput type="number" formControlName="amount" required>
            <span matPrefix>$&nbsp;</span>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Description</mat-label>
            <textarea matInput formControlName="description" rows="2" required></textarea>
          </mat-form-field>
        </form>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button mat-dialog-close>Cancel</button>
        <button mat-raised-button color="primary" [disabled]="dividendForm.invalid || isSaving()" (click)="saveDividend()">
          @if (isSaving()) { <mat-spinner diameter="20"></mat-spinner> }
          Record
        </button>
      </mat-dialog-actions>
    </ng-template>
  `,
  styles: [`
    .retained-container { padding: 24px; }
    .summary-cards { display: grid; grid-template-columns: repeat(auto-fit, minmax(220px, 1fr)); gap: 16px; margin-bottom: 24px; }
    .summary-card mat-card-content { display: flex; align-items: center; gap: 16px; padding: 16px !important; }
    .summary-card.highlight { background: linear-gradient(135deg, #4f46e5 0%, #6366f1 100%); color: white; }
    .summary-card.highlight .summary-label { color: rgba(255,255,255,0.8); }
    .summary-icon { width: 48px; height: 48px; border-radius: 12px; display: flex; align-items: center; justify-content: center; }
    .summary-icon.balance { background: rgba(255,255,255,0.2); }
    .summary-icon.income { background: #dcfce7; color: #16a34a; }
    .summary-icon.dividends { background: #fee2e2; color: #dc2626; }
    .summary-icon.adjustments { background: #fef3c7; color: #d97706; }
    .summary-info { display: flex; flex-direction: column; }
    .summary-label { font-size: 12px; color: var(--text-secondary); }
    .summary-value { font-size: 20px; font-weight: 600; }
    .toolbar { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 24px; gap: 16px; flex-wrap: wrap; }
    .filters { display: flex; gap: 12px; }
    .filter-field { min-width: 150px; }
    .toolbar-actions { display: flex; gap: 8px; }
    .loading-container { display: flex; justify-content: center; padding: 48px; }
    .table-container { background: var(--surface-color); border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1); margin-bottom: 24px; }
    table { width: 100%; }
    .period-name { font-weight: 500; }
    .positive { color: #16a34a; }
    .negative { color: #dc2626; }
    .ending-balance { font-weight: 600; }
    .status-open { background: #dbeafe !important; color: #1d4ed8 !important; }
    .status-closed { background: #f3f4f6 !important; color: #374151 !important; }
    tr.closed-row { background: #f9fafb; }
    .reconciliation-card { max-width: 500px; }
    .reconciliation-grid { display: flex; flex-direction: column; gap: 12px; }
    .recon-item { display: flex; justify-content: space-between; padding: 8px 0; }
    .recon-item.add .recon-value { color: #16a34a; }
    .recon-item.subtract .recon-value { color: #dc2626; }
    .recon-item.total { border-top: 2px solid #e5e7eb; padding-top: 12px; font-weight: 600; }
    .recon-label { color: var(--text-secondary); }
    .recon-value { font-weight: 500; }
    .dividend-form { display: flex; flex-direction: column; gap: 16px; min-width: 400px; }
    .full-width { width: 100%; }
  `]
})
export class RetainedEarningsComponent implements OnInit {
  @ViewChild('dividendDialog') dividendDialogTemplate!: TemplateRef<any>;

  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private notification = inject(NotificationService);

  retainedEarnings = signal<RetainedEarnings[]>([]);
  fiscalYears = signal<number[]>([2024, 2023, 2022]);
  isLoading = signal(false);
  isSaving = signal(false);
  selectedYear = signal(2024);

  displayedColumns = ['period', 'beginningBalance', 'netIncome', 'dividends', 'adjustments', 'endingBalance', 'status', 'actions'];

  dividendForm: FormGroup = this.fb.group({
    date: [new Date(), Validators.required],
    amount: [0, [Validators.required, Validators.min(0.01)]],
    description: ['', Validators.required]
  });

  currentBalance = computed(() => {
    const earnings = this.retainedEarnings();
    return earnings.length > 0 ? earnings[earnings.length - 1].endingBalance : 0;
  });

  beginningRetainedEarnings = computed(() => {
    const earnings = this.retainedEarnings();
    return earnings.length > 0 ? earnings[0].beginningBalance : 0;
  });

  ytdNetIncome = computed(() => this.retainedEarnings().reduce((sum, re) => sum + re.netIncome, 0));
  ytdDividends = computed(() => this.retainedEarnings().reduce((sum, re) => sum + re.dividendsDeclared, 0));
  totalAdjustments = computed(() => this.retainedEarnings().reduce((sum, re) => sum + re.otherAdjustments, 0));

  ngOnInit(): void { this.loadData(); }

  async loadData(): Promise<void> {
    this.isLoading.set(true);
    try {
      const mockData: RetainedEarnings[] = [
        { id: '1', periodId: '1', fiscalYear: 2024, periodName: 'January 2024', beginningBalance: 500000, netIncome: 45000, dividendsDeclared: 0, otherAdjustments: 0, endingBalance: 545000, distributions: [], isActive: true },
        { id: '2', periodId: '2', fiscalYear: 2024, periodName: 'February 2024', beginningBalance: 545000, netIncome: 52000, dividendsDeclared: 25000, otherAdjustments: 0, endingBalance: 572000, distributions: [], isActive: true },
        { id: '3', periodId: '3', fiscalYear: 2024, periodName: 'March 2024', beginningBalance: 572000, netIncome: 38000, dividendsDeclared: 0, otherAdjustments: -5000, endingBalance: 605000, distributions: [], isActive: true }
      ];
      await new Promise(r => setTimeout(r, 300));
      this.retainedEarnings.set(mockData);
    } finally { this.isLoading.set(false); }
  }

  onYearChange(year: number): void {
    this.selectedYear.set(year);
    this.loadData();
  }

  recordDividend(): void {
    this.dividendForm.reset({ date: new Date(), amount: 0 });
    this.dialog.open(this.dividendDialogTemplate, { width: '450px' });
  }

  async saveDividend(): Promise<void> {
    if (this.dividendForm.invalid) return;
    this.isSaving.set(true);
    try {
      await new Promise(r => setTimeout(r, 500));
      this.notification.success('Dividend recorded');
      this.dialog.closeAll();
      this.loadData();
    } finally { this.isSaving.set(false); }
  }

  recordAdjustment(): void { this.notification.info('Prior period adjustment feature'); }
  closeYear(): void { this.notification.info('Year-end close process'); }
  viewDetails(re: RetainedEarnings): void {}
  viewTransactions(re: RetainedEarnings): void {}
  async closePeriod(re: RetainedEarnings): Promise<void> { this.notification.success('Period closed'); this.loadData(); }
}

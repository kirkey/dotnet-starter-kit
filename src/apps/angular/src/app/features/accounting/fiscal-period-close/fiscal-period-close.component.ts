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
import { MatStepperModule } from '@angular/material/stepper';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatListModule } from '@angular/material/list';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { NotificationService } from '@core/services/notification.service';
import { FiscalPeriodClose, AccountingPeriod, ApprovalStatus, CloseType } from '@core/models/accounting.model';

// Extended interface for UI display purposes - adds computed/UI-specific properties
interface FiscalPeriodCloseView extends FiscalPeriodClose {
  // Computed from checklistItems
  completionPercent?: number;
  allEntriesPosted?: boolean;
  reconciliationsComplete?: boolean;
  adjustmentsComplete?: boolean;
  reportsReviewed?: boolean;
}

@Component({
  selector: 'app-fiscal-period-close',
  standalone: true,
  imports: [
    CommonModule, CurrencyPipe, DatePipe, ReactiveFormsModule,
    MatTableModule, MatPaginatorModule, MatSortModule, MatButtonModule, MatIconModule,
    MatFormFieldModule, MatInputModule, MatSelectModule, MatDialogModule,
    MatProgressSpinnerModule, MatChipsModule, MatMenuModule, MatTooltipModule,
    MatCardModule, MatStepperModule, MatCheckboxModule, MatListModule, MatProgressBarModule, PageHeaderComponent
  ],
  template: `
    <div class="close-container">
      <app-page-header 
        title="Fiscal Period Close" 
        subtitle="Manage period-end closing procedures"
        icon="event_available">
      </app-page-header>

      <div class="summary-cards">
        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon current">
              <mat-icon>calendar_today</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Current Period</span>
              <span class="summary-value">{{ currentPeriod()?.periodName || 'N/A' }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon open">
              <mat-icon>lock_open</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Open Periods</span>
              <span class="summary-value">{{ openPeriodCount() }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon closed">
              <mat-icon>lock</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Closed (YTD)</span>
              <span class="summary-value">{{ closedPeriodCount() }}</span>
            </div>
          </mat-card-content>
        </mat-card>
      </div>

      @if (isLoading()) {
        <div class="loading-container">
          <mat-spinner diameter="48"></mat-spinner>
        </div>
      } @else {
        <div class="periods-grid">
          @for (period of periods(); track period.id) {
            <mat-card class="period-card" [class.closed]="period.status === ApprovalStatus.Approved" [class.in-progress]="period.status === ApprovalStatus.Pending">
              <mat-card-header>
                <mat-card-title>{{ period.periodName }}</mat-card-title>
                <mat-chip [class]="'status-' + period.status.toLowerCase()">{{ period.status }}</mat-chip>
              </mat-card-header>
              <mat-card-content>
                <div class="period-dates">
                  <span>{{ period.periodStartDate | date:'mediumDate' }} - {{ period.periodEndDate | date:'mediumDate' }}</span>
                </div>

                @if (period.status === ApprovalStatus.Pending) {
                  <div class="close-progress">
                    <mat-progress-bar mode="determinate" [value]="period.completionPercent || 0"></mat-progress-bar>
                    <span class="progress-text">{{ period.completionPercent || 0 }}% Complete</span>
                  </div>
                }

                <div class="period-checklist">
                  <div class="checklist-item" [class.complete]="period.allEntriesPosted">
                    <mat-icon>{{ period.allEntriesPosted ? 'check_circle' : 'radio_button_unchecked' }}</mat-icon>
                    <span>All entries posted</span>
                  </div>
                  <div class="checklist-item" [class.complete]="period.reconciliationsComplete">
                    <mat-icon>{{ period.reconciliationsComplete ? 'check_circle' : 'radio_button_unchecked' }}</mat-icon>
                    <span>Reconciliations complete</span>
                  </div>
                  <div class="checklist-item" [class.complete]="period.adjustmentsComplete">
                    <mat-icon>{{ period.adjustmentsComplete ? 'check_circle' : 'radio_button_unchecked' }}</mat-icon>
                    <span>Adjustments complete</span>
                  </div>
                  <div class="checklist-item" [class.complete]="period.reportsReviewed">
                    <mat-icon>{{ period.reportsReviewed ? 'check_circle' : 'radio_button_unchecked' }}</mat-icon>
                    <span>Reports reviewed</span>
                  </div>
                </div>
              </mat-card-content>
              <mat-card-actions>
                @if (period.status === ApprovalStatus.Draft) {
                  <button mat-stroked-button (click)="startClose(period)">
                    <mat-icon>play_arrow</mat-icon>
                    Start Close
                  </button>
                }
                @if (period.status === ApprovalStatus.Pending) {
                  <button mat-stroked-button (click)="continueClose(period)">
                    <mat-icon>arrow_forward</mat-icon>
                    Continue
                  </button>
                  <button mat-raised-button color="primary" [disabled]="!canFinalize(period)" (click)="finalizeClose(period)">
                    <mat-icon>lock</mat-icon>
                    Finalize
                  </button>
                }
                @if (period.status === ApprovalStatus.Approved) {
                  <button mat-stroked-button (click)="viewDetails(period)">
                    <mat-icon>visibility</mat-icon>
                    View Details
                  </button>
                  <button mat-stroked-button (click)="reopenPeriod(period)" [disabled]="!canReopen(period)">
                    <mat-icon>lock_open</mat-icon>
                    Reopen
                  </button>
                }
              </mat-card-actions>
            </mat-card>
          }
        </div>
      }
    </div>

    <ng-template #closeDialog>
      <h2 mat-dialog-title>Period Close: {{ closingPeriod()?.periodName }}</h2>
      <mat-dialog-content>
        <mat-stepper #stepper linear>
          <mat-step label="Verify Entries">
            <div class="step-content">
              <p>Ensure all journal entries have been posted for this period.</p>
              <mat-list>
                <mat-list-item>
                  <mat-icon matListItemIcon>check_circle</mat-icon>
                  <span matListItemTitle>Unposted Entries: 0</span>
                </mat-list-item>
                <mat-list-item>
                  <mat-icon matListItemIcon>check_circle</mat-icon>
                  <span matListItemTitle>Pending Approvals: 0</span>
                </mat-list-item>
              </mat-list>
              <button mat-raised-button color="primary" matStepperNext>Next</button>
            </div>
          </mat-step>

          <mat-step label="Reconciliations">
            <div class="step-content">
              <p>Complete all account reconciliations.</p>
              <mat-list>
                <mat-list-item>
                  <mat-icon matListItemIcon>check_circle</mat-icon>
                  <span matListItemTitle>Bank Reconciliations: Complete</span>
                </mat-list-item>
                <mat-list-item>
                  <mat-icon matListItemIcon>warning</mat-icon>
                  <span matListItemTitle>AR Reconciliation: Pending</span>
                </mat-list-item>
              </mat-list>
              <button mat-button matStepperPrevious>Back</button>
              <button mat-raised-button color="primary" matStepperNext>Next</button>
            </div>
          </mat-step>

          <mat-step label="Adjusting Entries">
            <div class="step-content">
              <p>Record all period-end adjustments.</p>
              <mat-list>
                <mat-list-item>
                  <mat-icon matListItemIcon>check_circle</mat-icon>
                  <span matListItemTitle>Accruals Posted</span>
                </mat-list-item>
                <mat-list-item>
                  <mat-icon matListItemIcon>check_circle</mat-icon>
                  <span matListItemTitle>Depreciation Posted</span>
                </mat-list-item>
              </mat-list>
              <button mat-button matStepperPrevious>Back</button>
              <button mat-raised-button color="primary" matStepperNext>Next</button>
            </div>
          </mat-step>

          <mat-step label="Review Reports">
            <div class="step-content">
              <p>Review and approve financial reports.</p>
              <mat-checkbox>Trial Balance reviewed</mat-checkbox><br>
              <mat-checkbox>Income Statement reviewed</mat-checkbox><br>
              <mat-checkbox>Balance Sheet reviewed</mat-checkbox>
              <div class="step-actions">
                <button mat-button matStepperPrevious>Back</button>
                <button mat-raised-button color="primary" matStepperNext>Next</button>
              </div>
            </div>
          </mat-step>

          <mat-step label="Finalize">
            <div class="step-content">
              <p>Ready to close the period. This action will:</p>
              <ul>
                <li>Lock all transactions in this period</li>
                <li>Post closing entries</li>
                <li>Reset temporary accounts</li>
              </ul>
              <button mat-button matStepperPrevious>Back</button>
              <button mat-raised-button color="warn" (click)="confirmClose()">
                <mat-icon>lock</mat-icon>
                Close Period
              </button>
            </div>
          </mat-step>
        </mat-stepper>
      </mat-dialog-content>
    </ng-template>
  `,
  styles: [`
    .close-container { padding: 24px; }
    .summary-cards { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 16px; margin-bottom: 24px; }
    .summary-card mat-card-content { display: flex; align-items: center; gap: 16px; padding: 16px !important; }
    .summary-icon { width: 48px; height: 48px; border-radius: 12px; display: flex; align-items: center; justify-content: center; }
    .summary-icon.current { background: #dbeafe; color: #2563eb; }
    .summary-icon.open { background: #dcfce7; color: #16a34a; }
    .summary-icon.closed { background: #f3f4f6; color: #374151; }
    .summary-info { display: flex; flex-direction: column; }
    .summary-label { font-size: 12px; color: var(--text-secondary); }
    .summary-value { font-size: 20px; font-weight: 600; }
    .loading-container { display: flex; justify-content: center; padding: 48px; }
    .periods-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(320px, 1fr)); gap: 16px; }
    .period-card { transition: all 0.2s ease; }
    .period-card:hover { transform: translateY(-2px); box-shadow: 0 4px 12px rgba(0,0,0,0.15); }
    .period-card.closed { opacity: 0.7; }
    .period-card.in-progress { border-left: 4px solid #3b82f6; }
    .period-card mat-card-header { display: flex; justify-content: space-between; align-items: center; }
    .period-dates { font-size: 13px; color: var(--text-secondary); margin-bottom: 16px; }
    .close-progress { margin-bottom: 16px; }
    .progress-text { font-size: 12px; color: var(--text-secondary); }
    .period-checklist { display: flex; flex-direction: column; gap: 8px; }
    .checklist-item { display: flex; align-items: center; gap: 8px; font-size: 13px; color: var(--text-secondary); }
    .checklist-item mat-icon { font-size: 18px; width: 18px; height: 18px; }
    .checklist-item.complete { color: #16a34a; }
    .checklist-item.complete mat-icon { color: #16a34a; }
    .status-open { background: #dcfce7 !important; color: #166534 !important; }
    .status-inprogress { background: #dbeafe !important; color: #1d4ed8 !important; }
    .status-closed { background: #f3f4f6 !important; color: #374151 !important; }
    mat-card-actions { display: flex; gap: 8px; padding: 16px !important; }
    .step-content { padding: 16px 0; }
    .step-content p { margin-bottom: 16px; }
    .step-content ul { margin: 16px 0; }
    .step-actions { margin-top: 16px; display: flex; gap: 8px; }
  `]
})
export class FiscalPeriodCloseComponent implements OnInit {
  @ViewChild('closeDialog') closeDialogTemplate!: TemplateRef<any>;

  private dialog = inject(MatDialog);
  private notification = inject(NotificationService);

  // Expose ApprovalStatus to template
  readonly ApprovalStatus = ApprovalStatus;

  periods = signal<FiscalPeriodCloseView[]>([]);
  isLoading = signal(false);
  closingPeriod = signal<FiscalPeriodCloseView | null>(null);

  currentPeriod = computed(() => this.periods().find(p => p.status === ApprovalStatus.Draft || p.status === ApprovalStatus.Pending));
  openPeriodCount = computed(() => this.periods().filter(p => p.status === ApprovalStatus.Draft).length);
  closedPeriodCount = computed(() => this.periods().filter(p => p.status === ApprovalStatus.Approved).length);

  ngOnInit(): void { this.loadData(); }

  canFinalize(period: FiscalPeriodCloseView): boolean {
    return !!(period.allEntriesPosted && period.reconciliationsComplete && period.adjustmentsComplete && period.reportsReviewed);
  }

  canReopen(period: FiscalPeriodCloseView): boolean {
    const periods = this.periods();
    const index = periods.findIndex(p => p.id === period.id);
    return index === periods.length - 1 || periods.slice(index + 1).every(p => p.status !== ApprovalStatus.Approved);
  }

  async loadData(): Promise<void> {
    this.isLoading.set(true);
    try {
      const mockPeriods: FiscalPeriodCloseView[] = [
        { id: '1', closeNumber: 'FC-2024-01', closeType: CloseType.Monthly, periodId: '1', periodName: 'January 2024', fiscalYear: 2024, periodStartDate: new Date('2024-01-01'), periodEndDate: new Date('2024-01-31'), status: ApprovalStatus.Approved, checklistItems: [], closedBy: 'admin', closedOn: new Date('2024-02-05'), isActive: true, completionPercent: 100, allEntriesPosted: true, reconciliationsComplete: true, adjustmentsComplete: true, reportsReviewed: true },
        { id: '2', closeNumber: 'FC-2024-02', closeType: CloseType.Monthly, periodId: '2', periodName: 'February 2024', fiscalYear: 2024, periodStartDate: new Date('2024-02-01'), periodEndDate: new Date('2024-02-29'), status: ApprovalStatus.Pending, checklistItems: [], isActive: true, completionPercent: 75, allEntriesPosted: true, reconciliationsComplete: true, adjustmentsComplete: true, reportsReviewed: false },
        { id: '3', closeNumber: 'FC-2024-03', closeType: CloseType.Monthly, periodId: '3', periodName: 'March 2024', fiscalYear: 2024, periodStartDate: new Date('2024-03-01'), periodEndDate: new Date('2024-03-31'), status: ApprovalStatus.Draft, checklistItems: [], isActive: true, completionPercent: 0, allEntriesPosted: false, reconciliationsComplete: false, adjustmentsComplete: false, reportsReviewed: false }
      ];
      await new Promise(r => setTimeout(r, 300));
      this.periods.set(mockPeriods);
    } finally { this.isLoading.set(false); }
  }

  startClose(period: FiscalPeriodCloseView): void {
    this.closingPeriod.set(period);
    this.dialog.open(this.closeDialogTemplate, { width: '600px', disableClose: true });
  }

  continueClose(period: FiscalPeriodCloseView): void {
    this.closingPeriod.set(period);
    this.dialog.open(this.closeDialogTemplate, { width: '600px', disableClose: true });
  }

  async finalizeClose(period: FiscalPeriodCloseView): Promise<void> {
    this.notification.success('Period closed successfully');
    this.loadData();
  }

  confirmClose(): void {
    this.notification.success('Period closed successfully');
    this.dialog.closeAll();
    this.loadData();
  }

  viewDetails(period: FiscalPeriodCloseView): void {}
  
  async reopenPeriod(period: FiscalPeriodCloseView): Promise<void> {
    this.notification.success('Period reopened');
    this.loadData();
  }
}

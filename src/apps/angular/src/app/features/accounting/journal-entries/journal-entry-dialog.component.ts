import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatTabsModule } from '@angular/material/tabs';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDividerModule } from '@angular/material/divider';

import { AccountingService } from '@core/services/accounting.service';
import {
  JournalEntry,
  JournalEntryLine,
  ChartOfAccount,
  AccountingPeriod,
  CreateJournalEntryRequest,
  UpdateJournalEntryRequest,
  CostCenter,
  Department
} from '@core/models/accounting.model';

@Component({
  selector: 'app-journal-entry-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTableModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatAutocompleteModule,
    MatTabsModule,
    MatChipsModule,
    MatTooltipModule,
    MatDividerModule
  ],
  template: `
    <h2 mat-dialog-title>
      <div class="dialog-title">
        <span>{{ isEditMode ? 'Edit Journal Entry' : isViewMode ? 'View Journal Entry' : 'Create Journal Entry' }}</span>
        @if (entry?.status) {
          <mat-chip [class]="'status-chip ' + entry!.status.toLowerCase()">
            {{ entry!.status }}
          </mat-chip>
        }
      </div>
    </h2>

    <mat-dialog-content>
      <form [formGroup]="form">
        <mat-tab-group>
          <!-- Header Tab -->
          <mat-tab label="Header">
            <div class="tab-content">
              <div class="form-row">
                <mat-form-field appearance="outline" class="third-width">
                  <mat-label>Entry Number</mat-label>
                  <input matInput formControlName="entryNumber" placeholder="Auto-generated" readonly>
                </mat-form-field>

                <mat-form-field appearance="outline" class="third-width">
                  <mat-label>Entry Date</mat-label>
                  <input matInput [matDatepicker]="datePicker" formControlName="entryDate">
                  <mat-datepicker-toggle matIconSuffix [for]="datePicker"></mat-datepicker-toggle>
                  <mat-datepicker #datePicker></mat-datepicker>
                  @if (form.get('entryDate')?.hasError('required')) {
                    <mat-error>Entry date is required</mat-error>
                  }
                </mat-form-field>

                <mat-form-field appearance="outline" class="third-width">
                  <mat-label>Accounting Period</mat-label>
                  <mat-select formControlName="periodId">
                    @for (period of periods(); track period.id) {
                      <mat-option [value]="period.id">{{ period.periodName }}</mat-option>
                    }
                  </mat-select>
                  @if (form.get('periodId')?.hasError('required')) {
                    <mat-error>Period is required</mat-error>
                  }
                </mat-form-field>
              </div>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Description</mat-label>
                <textarea matInput formControlName="description" rows="2"
                          placeholder="Enter journal entry description"></textarea>
                @if (form.get('description')?.hasError('required')) {
                  <mat-error>Description is required</mat-error>
                }
              </mat-form-field>

              <div class="form-row">
                <mat-form-field appearance="outline" class="half-width">
                  <mat-label>Reference Number</mat-label>
                  <input matInput formControlName="referenceNumber" placeholder="Optional reference">
                </mat-form-field>

                <mat-form-field appearance="outline" class="half-width">
                  <mat-label>Source</mat-label>
                  <input matInput formControlName="source" placeholder="Entry source">
                </mat-form-field>
              </div>
            </div>
          </mat-tab>

          <!-- Lines Tab -->
          <mat-tab label="Journal Lines">
            <div class="tab-content">
              <div class="lines-header">
                <span class="lines-title">Journal Entry Lines</span>
                @if (!isViewMode) {
                  <button mat-raised-button color="primary" (click)="addLine()">
                    <mat-icon>add</mat-icon>
                    Add Line
                  </button>
                }
              </div>

              <div class="lines-table">
                <table mat-table [dataSource]="lines.controls">
                  <ng-container matColumnDef="lineNumber">
                    <th mat-header-cell *matHeaderCellDef>#</th>
                    <td mat-cell *matCellDef="let line; let i = index">{{ i + 1 }}</td>
                  </ng-container>

                  <ng-container matColumnDef="accountId">
                    <th mat-header-cell *matHeaderCellDef>Account</th>
                    <td mat-cell *matCellDef="let line; let i = index">
                      <mat-form-field appearance="outline" class="table-field">
                        <input matInput [formControl]="line.get('accountId')"
                               [matAutocomplete]="accountAuto"
                               placeholder="Search account...">
                        <mat-autocomplete #accountAuto="matAutocomplete" [displayWith]="displayAccount">
                          @for (account of filteredAccounts(); track account.id) {
                            <mat-option [value]="account.id">
                              {{ account.accountCode }} - {{ account.accountName }}
                            </mat-option>
                          }
                        </mat-autocomplete>
                      </mat-form-field>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="description">
                    <th mat-header-cell *matHeaderCellDef>Description</th>
                    <td mat-cell *matCellDef="let line">
                      <mat-form-field appearance="outline" class="table-field">
                        <input matInput [formControl]="line.get('description')" placeholder="Line description">
                      </mat-form-field>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="debitAmount">
                    <th mat-header-cell *matHeaderCellDef class="text-right">Debit</th>
                    <td mat-cell *matCellDef="let line">
                      <mat-form-field appearance="outline" class="table-field amount-field">
                        <input matInput type="number" [formControl]="line.get('debitAmount')"
                               placeholder="0.00" (blur)="onDebitChange(line)">
                        <span matTextPrefix>$</span>
                      </mat-form-field>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="creditAmount">
                    <th mat-header-cell *matHeaderCellDef class="text-right">Credit</th>
                    <td mat-cell *matCellDef="let line">
                      <mat-form-field appearance="outline" class="table-field amount-field">
                        <input matInput type="number" [formControl]="line.get('creditAmount')"
                               placeholder="0.00" (blur)="onCreditChange(line)">
                        <span matTextPrefix>$</span>
                      </mat-form-field>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="costCenterId">
                    <th mat-header-cell *matHeaderCellDef>Cost Center</th>
                    <td mat-cell *matCellDef="let line">
                      <mat-form-field appearance="outline" class="table-field">
                        <mat-select [formControl]="line.get('costCenterId')">
                          <mat-option value="">None</mat-option>
                          @for (cc of costCenters(); track cc.id) {
                            <mat-option [value]="cc.id">{{ cc.costCenterName }}</mat-option>
                          }
                        </mat-select>
                      </mat-form-field>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="actions">
                    <th mat-header-cell *matHeaderCellDef></th>
                    <td mat-cell *matCellDef="let line; let i = index">
                      @if (!isViewMode && lines.length > 2) {
                        <button mat-icon-button color="warn" (click)="removeLine(i)"
                                matTooltip="Remove line">
                          <mat-icon>delete</mat-icon>
                        </button>
                      }
                    </td>
                  </ng-container>

                  <tr mat-header-row *matHeaderRowDef="lineColumns"></tr>
                  <tr mat-row *matRowDef="let row; columns: lineColumns;"></tr>
                </table>
              </div>

              <mat-divider></mat-divider>

              <div class="totals-row">
                <div class="total-item">
                  <span class="total-label">Total Debits:</span>
                  <span class="total-value">{{ totalDebit() | currency }}</span>
                </div>
                <div class="total-item">
                  <span class="total-label">Total Credits:</span>
                  <span class="total-value">{{ totalCredit() | currency }}</span>
                </div>
                <div class="total-item" [class.balanced]="isBalanced()" [class.unbalanced]="!isBalanced()">
                  <span class="total-label">Difference:</span>
                  <span class="total-value">{{ difference() | currency }}</span>
                </div>
              </div>

              @if (!isBalanced()) {
                <div class="balance-warning">
                  <mat-icon>warning</mat-icon>
                  <span>Journal entry is not balanced. Debits must equal credits.</span>
                </div>
              }
            </div>
          </mat-tab>

          <!-- Attachments Tab -->
          <mat-tab label="Attachments">
            <div class="tab-content">
              <div class="attachments-area">
                <p class="attachments-placeholder">
                  <mat-icon>attach_file</mat-icon>
                  Drag and drop files here or click to upload
                </p>
                <input type="file" #fileInput (change)="onFileSelected($event)" multiple hidden>
                <button mat-stroked-button (click)="fileInput.click()">
                  <mat-icon>upload</mat-icon>
                  Upload Files
                </button>
              </div>
            </div>
          </mat-tab>

          <!-- Audit Tab (View Mode Only) -->
          @if (isViewMode && entry) {
            <mat-tab label="Audit Trail">
              <div class="tab-content">
                <div class="audit-info">
                  <div class="audit-item">
                    <span class="audit-label">Created By:</span>
                    <span class="audit-value">{{ entry.createdBy }}</span>
                  </div>
                  <div class="audit-item">
                    <span class="audit-label">Created Date:</span>
                    <span class="audit-value">{{ entry.createdOn | date:'medium' }}</span>
                  </div>
                  @if (entry.approvedBy) {
                    <div class="audit-item">
                      <span class="audit-label">Approved By:</span>
                      <span class="audit-value">{{ entry.approvedBy }}</span>
                    </div>
                    <div class="audit-item">
                      <span class="audit-label">Approved Date:</span>
                      <span class="audit-value">{{ entry.approvedOn | date:'medium' }}</span>
                    </div>
                  }
                  @if (entry.postedBy) {
                    <div class="audit-item">
                      <span class="audit-label">Posted By:</span>
                      <span class="audit-value">{{ entry.postedBy }}</span>
                    </div>
                    <div class="audit-item">
                      <span class="audit-label">Posted Date:</span>
                      <span class="audit-value">{{ entry.postedOn | date:'medium' }}</span>
                    </div>
                  }
                  @if (entry.lastModifiedBy) {
                    <div class="audit-item">
                      <span class="audit-label">Last Modified By:</span>
                      <span class="audit-value">{{ entry.lastModifiedBy }}</span>
                    </div>
                    <div class="audit-item">
                      <span class="audit-label">Last Modified:</span>
                      <span class="audit-value">{{ entry.lastModifiedOn | date:'medium' }}</span>
                    </div>
                  }
                </div>
              </div>
            </mat-tab>
          }
        </mat-tab-group>
      </form>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>{{ isViewMode ? 'Close' : 'Cancel' }}</button>
      @if (!isViewMode) {
        <button mat-raised-button color="primary"
                [disabled]="form.invalid || !isBalanced() || saving()"
                (click)="save()">
          @if (saving()) {
            <mat-spinner diameter="20"></mat-spinner>
          } @else {
            {{ isEditMode ? 'Update' : 'Create' }}
          }
        </button>
      }
    </mat-dialog-actions>
  `,
  styles: [`
    .dialog-title {
      display: flex;
      align-items: center;
      gap: 16px;
    }

    mat-dialog-content {
      min-width: 800px;
      max-height: 70vh;
    }

    .tab-content {
      padding: 24px 0;
    }

    .form-row {
      display: flex;
      gap: 16px;
    }

    .third-width {
      flex: 1;
    }

    .half-width {
      flex: 1;
    }

    .full-width {
      width: 100%;
    }

    .lines-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 16px;
    }

    .lines-title {
      font-weight: 500;
      font-size: 16px;
    }

    .lines-table {
      overflow-x: auto;
    }

    .lines-table table {
      width: 100%;
    }

    .table-field {
      width: 100%;
      font-size: 14px;
    }

    ::ng-deep .table-field .mat-mdc-form-field-subscript-wrapper {
      display: none;
    }

    .amount-field {
      width: 120px;
    }

    .text-right {
      text-align: right;
    }

    .totals-row {
      display: flex;
      justify-content: flex-end;
      gap: 32px;
      padding: 16px 0;
    }

    .total-item {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .total-label {
      font-weight: 500;
    }

    .total-value {
      font-size: 16px;
      font-weight: 600;
    }

    .balanced {
      color: #388e3c;
    }

    .unbalanced {
      color: #c62828;
    }

    .balance-warning {
      display: flex;
      align-items: center;
      gap: 8px;
      padding: 12px;
      background-color: #fff3e0;
      color: #f57c00;
      border-radius: 4px;
      margin-top: 16px;
    }

    .attachments-area {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 48px;
      border: 2px dashed #ccc;
      border-radius: 8px;
      text-align: center;
      color: #666;
    }

    .attachments-placeholder {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 8px;
      margin-bottom: 16px;
    }

    .attachments-placeholder mat-icon {
      font-size: 48px;
      width: 48px;
      height: 48px;
      color: #ccc;
    }

    .audit-info {
      display: grid;
      grid-template-columns: repeat(2, 1fr);
      gap: 16px;
    }

    .audit-item {
      display: flex;
      flex-direction: column;
      gap: 4px;
    }

    .audit-label {
      font-size: 12px;
      color: #666;
      text-transform: uppercase;
    }

    .audit-value {
      font-weight: 500;
    }

    .status-chip {
      font-size: 12px;
    }

    .status-chip.draft {
      background-color: #e0e0e0;
    }

    .status-chip.pendingapproval {
      background-color: #fff3e0;
      color: #f57c00;
    }

    .status-chip.approved {
      background-color: #e3f2fd;
      color: #1976d2;
    }

    .status-chip.posted {
      background-color: #e8f5e9;
      color: #388e3c;
    }

    mat-dialog-actions {
      padding: 16px 24px;
    }
  `]
})
export class JournalEntryDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<JournalEntryDialogComponent>);
  private data = inject<{ entry?: JournalEntry }>(MAT_DIALOG_DATA);
  private accountingService = inject(AccountingService);
  private snackBar = inject(MatSnackBar);

  form!: FormGroup;
  saving = signal(false);
  accounts = signal<ChartOfAccount[]>([]);
  filteredAccounts = signal<ChartOfAccount[]>([]);
  periods = signal<AccountingPeriod[]>([]);
  costCenters = signal<CostCenter[]>([]);
  departments = signal<Department[]>([]);

  lineColumns = ['lineNumber', 'accountId', 'description', 'debitAmount', 'creditAmount', 'costCenterId', 'actions'];

  get entry(): JournalEntry | undefined {
    return this.data?.entry;
  }

  get isEditMode(): boolean {
    return !!this.entry?.id && this.entry.status === 'Draft';
  }

  get isViewMode(): boolean {
    return !!this.entry?.id && this.entry.status !== 'Draft';
  }

  get lines(): FormArray {
    return this.form.get('lines') as FormArray;
  }

  totalDebit = signal(0);
  totalCredit = signal(0);
  difference = signal(0);

  isBalanced(): boolean {
    return Math.abs(this.difference()) < 0.01;
  }

  ngOnInit(): void {
    this.initForm();
    this.loadAccounts();
    this.loadPeriods();
    this.loadCostCenters();
    this.calculateTotals();
  }

  initForm(): void {
    const entry = this.entry;
    this.form = this.fb.group({
      entryNumber: [{ value: entry?.entryNumber || 'Auto-generated', disabled: true }],
      entryDate: [entry?.entryDate ? new Date(entry.entryDate) : new Date(), Validators.required],
      periodId: [entry?.periodId || '', Validators.required],
      description: [entry?.description || '', Validators.required],
      referenceNumber: [entry?.referenceNumber || ''],
      source: [entry?.source || ''],
      lines: this.fb.array([])
    });

    // Add existing lines or default lines
    if (entry?.lines && entry.lines.length > 0) {
      entry.lines.forEach(line => this.addLine(line));
    } else {
      // Add two default lines
      this.addLine();
      this.addLine();
    }

    // Disable form if view mode
    if (this.isViewMode) {
      this.form.disable();
    }
  }

  loadAccounts(): void {
    this.accountingService.getChartOfAccounts({ pageNumber: 1, pageSize: 1000 }).subscribe({
      next: (result) => {
        const activeAccounts = result.items.filter(a => a.isActive && !a.isHeader);
        this.accounts.set(activeAccounts);
        this.filteredAccounts.set(activeAccounts);
      }
    });
  }

  loadPeriods(): void {
    this.accountingService.getOpenPeriods().subscribe({
      next: (periods) => {
        this.periods.set(periods);
        // Set current period as default if not editing
        if (!this.entry && periods.length > 0) {
          const currentPeriod = periods.find(p => p.isCurrent) || periods[0];
          this.form.patchValue({ periodId: currentPeriod.id });
        }
      }
    });
  }

  loadCostCenters(): void {
    this.accountingService.getCostCenters().subscribe({
      next: (costCenters) => {
        this.costCenters.set(costCenters);
      }
    });
  }

  addLine(line?: JournalEntryLine): void {
    const lineGroup = this.fb.group({
      accountId: [line?.accountId || '', Validators.required],
      description: [line?.description || ''],
      debitAmount: [line?.debitAmount || 0],
      creditAmount: [line?.creditAmount || 0],
      costCenterId: [line?.costCenterId || ''],
      departmentId: [line?.departmentId || '']
    });

    // Subscribe to changes for recalculation
    lineGroup.valueChanges.subscribe(() => this.calculateTotals());

    this.lines.push(lineGroup);
    this.calculateTotals();
  }

  removeLine(index: number): void {
    this.lines.removeAt(index);
    this.calculateTotals();
  }

  onDebitChange(line: FormGroup): void {
    const debit = line.get('debitAmount')?.value || 0;
    if (debit > 0) {
      line.patchValue({ creditAmount: 0 }, { emitEvent: false });
    }
    this.calculateTotals();
  }

  onCreditChange(line: FormGroup): void {
    const credit = line.get('creditAmount')?.value || 0;
    if (credit > 0) {
      line.patchValue({ debitAmount: 0 }, { emitEvent: false });
    }
    this.calculateTotals();
  }

  calculateTotals(): void {
    let debit = 0;
    let credit = 0;

    this.lines.controls.forEach(line => {
      debit += parseFloat(line.get('debitAmount')?.value) || 0;
      credit += parseFloat(line.get('creditAmount')?.value) || 0;
    });

    this.totalDebit.set(debit);
    this.totalCredit.set(credit);
    this.difference.set(debit - credit);
  }

  displayAccount = (accountId: string): string => {
    const account = this.accounts().find(a => a.id === accountId);
    return account ? `${account.accountCode} - ${account.accountName}` : '';
  };

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files) {
      // TODO: Handle file upload
      console.log('Files selected:', input.files);
    }
  }

  save(): void {
    if (this.form.invalid || !this.isBalanced()) return;

    this.saving.set(true);
    const formValue = this.form.getRawValue();
    const lines = formValue.lines.map((line: any, index: number) => ({
      ...line,
      lineNumber: index + 1
    }));

    if (this.isEditMode) {
      const request: UpdateJournalEntryRequest = {
        id: this.entry!.id,
        entryDate: formValue.entryDate,
        periodId: formValue.periodId,
        description: formValue.description,
        referenceNumber: formValue.referenceNumber,
        source: formValue.source,
        lines
      };

      this.accountingService.updateJournalEntry(request).subscribe({
        next: () => {
          this.snackBar.open('Journal entry updated successfully', 'Close', { duration: 3000 });
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error('Error updating entry:', error);
          this.snackBar.open('Failed to update journal entry', 'Close', { duration: 3000 });
          this.saving.set(false);
        }
      });
    } else {
      const request: CreateJournalEntryRequest = {
        entryDate: formValue.entryDate,
        periodId: formValue.periodId,
        description: formValue.description,
        referenceNumber: formValue.referenceNumber,
        source: formValue.source,
        lines
      };

      this.accountingService.createJournalEntry(request).subscribe({
        next: () => {
          this.snackBar.open('Journal entry created successfully', 'Close', { duration: 3000 });
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error('Error creating entry:', error);
          this.snackBar.open('Failed to create journal entry', 'Close', { duration: 3000 });
          this.saving.set(false);
        }
      });
    }
  }
}

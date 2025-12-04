import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';

import { AccountingService } from '@core/services/accounting.service';
import { Budget, BudgetLine, ChartOfAccount } from '@core/models/accounting.model';

@Component({
  selector: 'app-budget-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatIconModule,
    MatTabsModule,
    MatTableModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatTooltipModule
  ],
  template: `
    <h2 mat-dialog-title>{{ data.budget ? 'Edit Budget' : 'Create Budget' }}</h2>

    <mat-dialog-content>
      <form [formGroup]="form">
        <mat-tab-group>
          <!-- Budget Header -->
          <mat-tab label="Budget Details">
            <div class="tab-content">
              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Budget Name</mat-label>
                <input matInput formControlName="budgetName" required>
                @if (form.get('budgetName')?.hasError('required')) {
                  <mat-error>Budget name is required</mat-error>
                }
              </mat-form-field>

              <div class="form-row">
                <mat-form-field appearance="outline">
                  <mat-label>Budget Code</mat-label>
                  <input matInput formControlName="budgetCode" required>
                  @if (form.get('budgetCode')?.hasError('required')) {
                    <mat-error>Budget code is required</mat-error>
                  }
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Fiscal Year</mat-label>
                  <mat-select formControlName="fiscalYear" required>
                    @for (year of fiscalYears; track year) {
                      <mat-option [value]="year">{{ year }}</mat-option>
                    }
                  </mat-select>
                  @if (form.get('fiscalYear')?.hasError('required')) {
                    <mat-error>Fiscal year is required</mat-error>
                  }
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Budget Type</mat-label>
                  <mat-select formControlName="budgetType" required>
                    <mat-option value="Operating">Operating</mat-option>
                    <mat-option value="Capital">Capital</mat-option>
                    <mat-option value="Cash">Cash</mat-option>
                    <mat-option value="Project">Project</mat-option>
                  </mat-select>
                </mat-form-field>
              </div>

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
            </div>
          </mat-tab>

          <!-- Budget Lines -->
          <mat-tab label="Budget Lines">
            <div class="tab-content">
              <div class="lines-header">
                <span>Budget Line Items</span>
                <button mat-raised-button color="primary" (click)="addLine()" type="button">
                  <mat-icon>add</mat-icon>
                  Add Line
                </button>
              </div>

              <div class="lines-table">
                <table mat-table [dataSource]="linesArray.controls">
                  <ng-container matColumnDef="account">
                    <th mat-header-cell *matHeaderCellDef>Account</th>
                    <td mat-cell *matCellDef="let line; let i = index" [formGroupName]="i">
                      <mat-form-field appearance="outline" class="table-field">
                        <mat-select formControlName="accountId" required>
                          @for (account of expenseAccounts(); track account.id) {
                            <mat-option [value]="account.id">
                              {{ account.accountCode }} - {{ account.accountName }}
                            </mat-option>
                          }
                        </mat-select>
                      </mat-form-field>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="jan">
                    <th mat-header-cell *matHeaderCellDef class="text-right">Jan</th>
                    <td mat-cell *matCellDef="let line; let i = index" [formGroupName]="i">
                      <mat-form-field appearance="outline" class="table-field month">
                        <input matInput type="number" formControlName="jan" min="0" step="0.01">
                      </mat-form-field>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="feb">
                    <th mat-header-cell *matHeaderCellDef class="text-right">Feb</th>
                    <td mat-cell *matCellDef="let line; let i = index" [formGroupName]="i">
                      <mat-form-field appearance="outline" class="table-field month">
                        <input matInput type="number" formControlName="feb" min="0" step="0.01">
                      </mat-form-field>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="mar">
                    <th mat-header-cell *matHeaderCellDef class="text-right">Mar</th>
                    <td mat-cell *matCellDef="let line; let i = index" [formGroupName]="i">
                      <mat-form-field appearance="outline" class="table-field month">
                        <input matInput type="number" formControlName="mar" min="0" step="0.01">
                      </mat-form-field>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="total">
                    <th mat-header-cell *matHeaderCellDef class="text-right">Total</th>
                    <td mat-cell *matCellDef="let line; let i = index" class="text-right">
                      {{ getLineTotal(i) | currency }}
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="actions">
                    <th mat-header-cell *matHeaderCellDef></th>
                    <td mat-cell *matCellDef="let line; let i = index">
                      <button mat-icon-button color="warn" (click)="removeLine(i)" type="button">
                        <mat-icon>delete</mat-icon>
                      </button>
                    </td>
                  </ng-container>

                  <tr mat-header-row *matHeaderRowDef="lineColumns"></tr>
                  <tr mat-row *matRowDef="let row; columns: lineColumns;"></tr>
                </table>
              </div>

              @if (linesArray.length === 0) {
                <div class="empty-lines">
                  <p>No budget lines added. Click "Add Line" to add budget items.</p>
                </div>
              }

              <div class="budget-total">
                <span>Total Budget:</span>
                <span class="total-amount">{{ getTotalBudget() | currency }}</span>
              </div>
            </div>
          </mat-tab>
        </mat-tab-group>
      </form>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-button (click)="cancel()">Cancel</button>
      <button mat-raised-button (click)="saveAsDraft()" [disabled]="saving">
        Save as Draft
      </button>
      <button mat-raised-button color="primary" (click)="save()" [disabled]="saving || !form.valid">
        @if (saving) {
          <mat-spinner diameter="20"></mat-spinner>
        } @else {
          {{ data.budget ? 'Update' : 'Create' }}
        }
      </button>
    </mat-dialog-actions>
  `,
  styles: [`
    mat-dialog-content {
      min-width: 800px;
    }

    .tab-content {
      padding: 16px 0;
    }

    .form-row {
      display: flex;
      gap: 16px;
    }

    .form-row mat-form-field {
      flex: 1;
    }

    .full-width {
      width: 100%;
    }

    mat-form-field {
      margin-bottom: 8px;
    }

    .lines-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 16px;
      font-weight: 500;
    }

    .lines-table {
      overflow-x: auto;
    }

    .lines-table table {
      width: 100%;
    }

    .table-field {
      margin: 4px 0;
    }

    .table-field.description {
      min-width: 150px;
    }

    .table-field.month {
      max-width: 80px;
    }

    .text-right {
      text-align: right;
    }

    .empty-lines {
      text-align: center;
      padding: 32px;
      color: #666;
      background: #f5f5f5;
      border-radius: 4px;
    }

    .budget-total {
      display: flex;
      justify-content: flex-end;
      align-items: center;
      gap: 16px;
      margin-top: 24px;
      padding: 16px;
      background: #f5f5f5;
      border-radius: 4px;
      font-weight: 500;
    }

    .total-amount {
      font-size: 20px;
      color: #1976d2;
    }

    mat-spinner {
      display: inline-block;
    }
  `]
})
export class BudgetDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private accountingService = inject(AccountingService);
  private dialogRef = inject(MatDialogRef<BudgetDialogComponent>);
  private snackBar = inject(MatSnackBar);
  public data: { budget?: Budget } = inject(MAT_DIALOG_DATA);

  form!: FormGroup;
  saving = false;

  expenseAccounts = signal<ChartOfAccount[]>([]);

  fiscalYears: number[] = [];
  lineColumns = ['account', 'jan', 'feb', 'mar', 'total', 'actions'];

  get linesArray(): FormArray {
    return this.form.get('lines') as FormArray;
  }

  ngOnInit(): void {
    this.initFiscalYears();
    this.initForm();
    this.loadAccounts();
  }

  private initFiscalYears(): void {
    const currentYear = new Date().getFullYear();
    for (let i = currentYear + 1; i >= currentYear - 2; i--) {
      this.fiscalYears.push(i);
    }
  }

  private initForm(): void {
    const budget = this.data.budget;
    const currentYear = new Date().getFullYear();
    
    this.form = this.fb.group({
      budgetName: [budget?.budgetName || '', Validators.required],
      budgetCode: [budget?.budgetCode || '', Validators.required],
      fiscalYear: [budget?.fiscalYear || currentYear, Validators.required],
      budgetType: [budget?.budgetType || 'Operating', Validators.required],
      startDate: [budget?.startDate ? new Date(budget.startDate) : new Date(currentYear, 0, 1), Validators.required],
      endDate: [budget?.endDate ? new Date(budget.endDate) : new Date(currentYear, 11, 31), Validators.required],
      lines: this.fb.array([])
    });

    // Load existing lines
    if (budget?.lines) {
      budget.lines.forEach(line => this.addLine(line));
    }
  }

  private loadAccounts(): void {
    this.accountingService.getChartOfAccounts({ pageNumber: 1, pageSize: 1000 }).subscribe({
      next: (result) => this.expenseAccounts.set(result.items.filter(a => a.accountType === 'Expense')),
      error: (error) => console.error('Error loading accounts:', error)
    });
  }

  addLine(line?: BudgetLine): void {
    const lineForm = this.fb.group({
      id: [line?.id || null],
      accountId: [line?.accountId || '', Validators.required],
      jan: [line?.jan || 0],
      feb: [line?.feb || 0],
      mar: [line?.mar || 0]
      // Add more months as needed
    });
    this.linesArray.push(lineForm);
  }

  removeLine(index: number): void {
    this.linesArray.removeAt(index);
  }

  getLineTotal(index: number): number {
    const line = this.linesArray.at(index);
    return (line.get('jan')?.value || 0) +
           (line.get('feb')?.value || 0) +
           (line.get('mar')?.value || 0);
  }

  getTotalBudget(): number {
    let total = 0;
    for (let i = 0; i < this.linesArray.length; i++) {
      total += this.getLineTotal(i);
    }
    return total;
  }

  saveAsDraft(): void {
    this.saveBudget('Draft');
  }

  save(): void {
    this.saveBudget('Active');
  }

  private saveBudget(status: string): void {
    if (this.form.invalid && status !== 'Draft') return;

    this.saving = true;
    const budgetData = {
      ...this.form.value,
      status,
      totalBudget: this.getTotalBudget()
    };

    const operation = this.data.budget
      ? this.accountingService.updateBudget(this.data.budget.id, budgetData)
      : this.accountingService.createBudget(budgetData);

    operation.subscribe({
      next: () => {
        this.snackBar.open(
          `Budget ${this.data.budget ? 'updated' : 'created'} successfully`,
          'Close',
          { duration: 3000 }
        );
        this.dialogRef.close(true);
      },
      error: (error) => {
        console.error('Error saving budget:', error);
        this.snackBar.open('Failed to save budget', 'Close', { duration: 3000 });
        this.saving = false;
      }
    });
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}

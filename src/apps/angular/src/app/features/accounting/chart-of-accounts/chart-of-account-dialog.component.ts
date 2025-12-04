import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatTabsModule } from '@angular/material/tabs';

import { AccountingService } from '@core/services/accounting.service';
import {
  ChartOfAccount,
  AccountType,
  AccountSubType,
  CreateChartOfAccountRequest,
  UpdateChartOfAccountRequest,
  UsoaCategory
} from '@core/models/accounting.model';

@Component({
  selector: 'app-chart-of-account-dialog',
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
    MatCheckboxModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatAutocompleteModule,
    MatTabsModule
  ],
  template: `
    <h2 mat-dialog-title>
      {{ isEditMode ? 'Edit Account' : 'Create Account' }}
    </h2>

    <mat-dialog-content>
      <form [formGroup]="form">
        <mat-tab-group>
          <!-- General Tab -->
          <mat-tab label="General">
            <div class="tab-content">
              <div class="form-row">
                <mat-form-field appearance="outline" class="half-width">
                  <mat-label>Account Code</mat-label>
                  <input matInput formControlName="accountCode" placeholder="e.g., 1000">
                  @if (form.get('accountCode')?.hasError('required')) {
                    <mat-error>Account code is required</mat-error>
                  }
                </mat-form-field>

                <mat-form-field appearance="outline" class="half-width">
                  <mat-label>Account Name</mat-label>
                  <input matInput formControlName="accountName" placeholder="e.g., Cash">
                  @if (form.get('accountName')?.hasError('required')) {
                    <mat-error>Account name is required</mat-error>
                  }
                </mat-form-field>
              </div>

              <div class="form-row">
                <mat-form-field appearance="outline" class="half-width">
                  <mat-label>Account Type</mat-label>
                  <mat-select formControlName="accountType" (selectionChange)="onTypeChange()">
                    @for (type of accountTypes; track type) {
                      <mat-option [value]="type">{{ type }}</mat-option>
                    }
                  </mat-select>
                  @if (form.get('accountType')?.hasError('required')) {
                    <mat-error>Account type is required</mat-error>
                  }
                </mat-form-field>

                <mat-form-field appearance="outline" class="half-width">
                  <mat-label>Account Sub-Type</mat-label>
                  <mat-select formControlName="accountSubType">
                    <mat-option value="">None</mat-option>
                    @for (subType of filteredSubTypes(); track subType) {
                      <mat-option [value]="subType">{{ subType }}</mat-option>
                    }
                  </mat-select>
                </mat-form-field>
              </div>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Description</mat-label>
                <textarea matInput formControlName="description" rows="3"
                          placeholder="Optional description of the account"></textarea>
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Parent Account</mat-label>
                <input matInput formControlName="parentAccountId"
                       [matAutocomplete]="parentAuto"
                       placeholder="Search for parent account...">
                <mat-autocomplete #parentAuto="matAutocomplete" [displayWith]="displayParent">
                  @for (account of parentAccounts(); track account.id) {
                    <mat-option [value]="account.id">
                      {{ account.accountCode }} - {{ account.accountName }}
                    </mat-option>
                  }
                </mat-autocomplete>
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>USOA Category</mat-label>
                <mat-select formControlName="usoaCategoryId">
                  <mat-option value="">None</mat-option>
                  @for (category of usoaCategories(); track category.code) {
                    <mat-option [value]="category.code">
                      {{ category.code }} - {{ category.name }}
                    </mat-option>
                  }
                </mat-select>
              </mat-form-field>
            </div>
          </mat-tab>

          <!-- Settings Tab -->
          <mat-tab label="Settings">
            <div class="tab-content">
              <div class="checkbox-group">
                <mat-checkbox formControlName="isActive">Active</mat-checkbox>
                <mat-checkbox formControlName="isHeader">Header Account (Summary Only)</mat-checkbox>
                <mat-checkbox formControlName="isCashFlowAccount">Cash Flow Account</mat-checkbox>
                <mat-checkbox formControlName="isBankAccount">Bank Account</mat-checkbox>
                <mat-checkbox formControlName="allowManualEntry">Allow Manual Journal Entry</mat-checkbox>
              </div>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Normal Balance</mat-label>
                <mat-select formControlName="normalBalance">
                  <mat-option value="Debit">Debit</mat-option>
                  <mat-option value="Credit">Credit</mat-option>
                </mat-select>
              </mat-form-field>

              <div class="form-row">
                <mat-form-field appearance="outline" class="half-width">
                  <mat-label>Opening Balance</mat-label>
                  <input matInput type="number" formControlName="openingBalance" placeholder="0.00">
                  <span matTextPrefix>$&nbsp;</span>
                </mat-form-field>

                <mat-form-field appearance="outline" class="half-width">
                  <mat-label>Opening Balance Date</mat-label>
                  <input matInput type="date" formControlName="openingBalanceDate">
                </mat-form-field>
              </div>
            </div>
          </mat-tab>

          <!-- Tax & Reporting Tab -->
          <mat-tab label="Tax & Reporting">
            <div class="tab-content">
              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Tax Code</mat-label>
                <input matInput formControlName="taxCode" placeholder="Optional tax code">
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Reporting Code</mat-label>
                <input matInput formControlName="reportingCode" placeholder="Optional reporting code">
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Notes</mat-label>
                <textarea matInput formControlName="notes" rows="4"
                          placeholder="Additional notes for reporting..."></textarea>
              </mat-form-field>
            </div>
          </mat-tab>
        </mat-tab-group>
      </form>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-raised-button color="primary"
              [disabled]="form.invalid || saving()"
              (click)="save()">
        @if (saving()) {
          <mat-spinner diameter="20"></mat-spinner>
        } @else {
          {{ isEditMode ? 'Update' : 'Create' }}
        }
      </button>
    </mat-dialog-actions>
  `,
  styles: [`
    mat-dialog-content {
      min-width: 500px;
    }

    .tab-content {
      padding: 24px 0;
    }

    .form-row {
      display: flex;
      gap: 16px;
    }

    .half-width {
      flex: 1;
    }

    .full-width {
      width: 100%;
    }

    .checkbox-group {
      display: flex;
      flex-direction: column;
      gap: 12px;
      margin-bottom: 24px;
    }

    mat-dialog-actions {
      padding: 16px 24px;
    }

    mat-spinner {
      display: inline-block;
    }
  `]
})
export class ChartOfAccountDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<ChartOfAccountDialogComponent>);
  private data = inject<{ account?: ChartOfAccount; parentAccountId?: string }>(MAT_DIALOG_DATA);
  private accountingService = inject(AccountingService);
  private snackBar = inject(MatSnackBar);

  form!: FormGroup;
  saving = signal(false);
  parentAccounts = signal<ChartOfAccount[]>([]);
  usoaCategories = signal<UsoaCategory[]>([]);
  filteredSubTypes = signal<string[]>([]);

  accountTypes = Object.values(AccountType);
  accountSubTypes = Object.values(AccountSubType);

  get isEditMode(): boolean {
    return !!this.data?.account;
  }

  ngOnInit(): void {
    this.initForm();
    this.loadParentAccounts();
    this.loadUsoaCategories();

    if (this.data?.parentAccountId) {
      this.form.patchValue({ parentAccountId: this.data.parentAccountId });
    }
  }

  initForm(): void {
    const account = this.data?.account;
    this.form = this.fb.group({
      accountCode: [account?.accountCode || '', Validators.required],
      accountName: [account?.accountName || '', Validators.required],
      description: [account?.description || ''],
      accountType: [account?.accountType || '', Validators.required],
      accountSubType: [account?.accountSubType || ''],
      parentAccountId: [account?.parentAccountId || ''],
      usoaCategoryId: [account?.usoaCategory || ''],
      isActive: [account?.isActive ?? true],
      isHeader: [account?.isHeader ?? false],
      isCashFlowAccount: [false],
      isBankAccount: [account?.isBankAccount ?? false],
      allowManualEntry: [true],
      normalBalance: [account?.normalBalance || 'Debit'],
      openingBalance: [0],
      openingBalanceDate: [''],
      taxCode: [''],
      reportingCode: [''],
      notes: [account?.notes || '']
    });

    if (account?.accountType) {
      this.updateSubTypes(account.accountType);
    }
  }

  loadParentAccounts(): void {
    this.accountingService.getChartOfAccounts({ pageNumber: 1, pageSize: 1000 }).subscribe({
      next: (result) => {
        // Filter out current account and its children if editing
        let accounts = result.items;
        if (this.isEditMode) {
          accounts = accounts.filter(a => a.id !== this.data.account!.id);
        }
        this.parentAccounts.set(accounts);
      }
    });
  }

  loadUsoaCategories(): void {
    this.accountingService.getUsoaCategories().subscribe({
      next: (categories) => {
        this.usoaCategories.set(categories);
      }
    });
  }

  onTypeChange(): void {
    const accountType = this.form.get('accountType')?.value;
    this.updateSubTypes(accountType);

    // Set default normal balance based on account type
    if (accountType === AccountType.Asset || accountType === AccountType.Expense) {
      this.form.patchValue({ normalBalance: 'Debit' });
    } else {
      this.form.patchValue({ normalBalance: 'Credit' });
    }
  }

  updateSubTypes(accountType: AccountType): void {
    const subTypeMap: Record<string, string[]> = {
      [AccountType.Asset]: [
        AccountSubType.CurrentAsset,
        AccountSubType.FixedAsset,
        AccountSubType.OtherAsset
      ],
      [AccountType.Liability]: [
        AccountSubType.CurrentLiability,
        AccountSubType.LongTermLiability
      ],
      [AccountType.Equity]: [
        AccountSubType.Equity
      ],
      [AccountType.Revenue]: [
        AccountSubType.Revenue
      ],
      [AccountType.Expense]: [
        AccountSubType.CostOfGoodsSold,
        AccountSubType.OperatingExpense,
        AccountSubType.OtherExpense
      ]
    };

    this.filteredSubTypes.set(subTypeMap[accountType] || []);
  }

  displayParent = (accountId: string): string => {
    const account = this.parentAccounts().find(a => a.id === accountId);
    return account ? `${account.accountCode} - ${account.accountName}` : '';
  };

  save(): void {
    if (this.form.invalid) return;

    this.saving.set(true);
    const formValue = this.form.value;

    if (this.isEditMode) {
      const request: UpdateChartOfAccountRequest = {
        id: this.data.account!.id,
        ...formValue
      };

      this.accountingService.updateChartOfAccount(request).subscribe({
        next: () => {
          this.snackBar.open('Account updated successfully', 'Close', { duration: 3000 });
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error('Error updating account:', error);
          this.snackBar.open('Failed to update account', 'Close', { duration: 3000 });
          this.saving.set(false);
        }
      });
    } else {
      const request: CreateChartOfAccountRequest = formValue;

      this.accountingService.createChartOfAccount(request).subscribe({
        next: () => {
          this.snackBar.open('Account created successfully', 'Close', { duration: 3000 });
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error('Error creating account:', error);
          this.snackBar.open('Failed to create account', 'Close', { duration: 3000 });
          this.saving.set(false);
        }
      });
    }
  }
}

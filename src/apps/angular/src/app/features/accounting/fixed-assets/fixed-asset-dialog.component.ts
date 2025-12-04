import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { AccountingService } from '@core/services/accounting.service';
import { FixedAsset, ChartOfAccount, DepreciationMethodConfig, AccountType, AccountSubType } from '@core/models/accounting.model';

@Component({
  selector: 'app-fixed-asset-dialog',
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
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  template: `
    <h2 mat-dialog-title>{{ data.asset ? 'Edit Asset' : 'Add Fixed Asset' }}</h2>

    <mat-dialog-content>
      <form [formGroup]="form">
        <mat-tab-group>
          <!-- General Tab -->
          <mat-tab label="General">
            <div class="tab-content">
              <div class="form-row">
                <mat-form-field appearance="outline">
                  <mat-label>Asset Code</mat-label>
                  <input matInput formControlName="assetCode" required>
                  @if (form.get('assetCode')?.hasError('required')) {
                    <mat-error>Asset code is required</mat-error>
                  }
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Serial Number</mat-label>
                  <input matInput formControlName="serialNumber">
                </mat-form-field>
              </div>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Asset Name</mat-label>
                <input matInput formControlName="assetName" required>
                @if (form.get('assetName')?.hasError('required')) {
                  <mat-error>Asset name is required</mat-error>
                }
              </mat-form-field>

              <div class="form-row">
                <mat-form-field appearance="outline">
                  <mat-label>Category</mat-label>
                  <mat-select formControlName="assetCategory" required>
                    <mat-option value="Buildings">Buildings</mat-option>
                    <mat-option value="Vehicles">Vehicles</mat-option>
                    <mat-option value="Equipment">Equipment</mat-option>
                    <mat-option value="Furniture">Furniture</mat-option>
                    <mat-option value="ComputerEquipment">Computer Equipment</mat-option>
                    <mat-option value="LeaseholdImprovements">Leasehold Improvements</mat-option>
                    <mat-option value="Land">Land</mat-option>
                    <mat-option value="Other">Other</mat-option>
                  </mat-select>
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Location</mat-label>
                  <input matInput formControlName="location">
                </mat-form-field>
              </div>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Description</mat-label>
                <textarea matInput formControlName="description" rows="2"></textarea>
              </mat-form-field>
            </div>
          </mat-tab>

          <!-- Acquisition Tab -->
          <mat-tab label="Acquisition">
            <div class="tab-content">
              <div class="form-row">
                <mat-form-field appearance="outline">
                  <mat-label>Purchase Date</mat-label>
                  <input matInput [matDatepicker]="acqPicker" formControlName="purchaseDate" required>
                  <mat-datepicker-toggle matIconSuffix [for]="acqPicker"></mat-datepicker-toggle>
                  <mat-datepicker #acqPicker></mat-datepicker>
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Purchase Price</mat-label>
                  <input matInput type="number" formControlName="purchasePrice" required min="0" step="0.01">
                  <span matPrefix>$&nbsp;</span>
                </mat-form-field>
              </div>

              <div class="form-row">
                <mat-form-field appearance="outline">
                  <mat-label>Vendor</mat-label>
                  <input matInput formControlName="vendorName">
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Invoice Number</mat-label>
                  <input matInput formControlName="invoiceNumber">
                </mat-form-field>
              </div>

              <div class="form-row">
                <mat-form-field appearance="outline">
                  <mat-label>Asset Type</mat-label>
                  <input matInput formControlName="assetType">
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Department</mat-label>
                  <input matInput formControlName="department">
                </mat-form-field>
              </div>
            </div>
          </mat-tab>

          <!-- Depreciation Tab -->
          <mat-tab label="Depreciation">
            <div class="tab-content">
              <div class="form-row">
                <mat-form-field appearance="outline">
                  <mat-label>Depreciation Method</mat-label>
                  <mat-select formControlName="depreciationMethodId" required>
                    @for (method of depreciationMethods(); track method.id) {
                      <mat-option [value]="method.id">{{ method.methodName }}</mat-option>
                    }
                  </mat-select>
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Useful Life (Years)</mat-label>
                  <input matInput type="number" formControlName="usefulLifeYears" required min="1">
                </mat-form-field>
              </div>

              <div class="form-row">
                <mat-form-field appearance="outline">
                  <mat-label>Salvage Value</mat-label>
                  <input matInput type="number" formControlName="salvageValue" min="0" step="0.01">
                  <span matPrefix>$&nbsp;</span>
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Useful Life (Months)</mat-label>
                  <input matInput type="number" formControlName="usefulLifeMonths" min="0">
                </mat-form-field>
              </div>

              @if (data.asset) {
                <div class="depreciation-summary">
                  <div class="summary-item">
                    <span class="label">Accumulated Depreciation:</span>
                    <span class="value">{{ data.asset.accumulatedDepreciation | currency }}</span>
                  </div>
                  <div class="summary-item">
                    <span class="label">Book Value:</span>
                    <span class="value book-value">
                      {{ data.asset.currentBookValue | currency }}
                    </span>
                  </div>
                </div>
              }
            </div>
          </mat-tab>

          <!-- GL Accounts Tab -->
          <mat-tab label="GL Accounts">
            <div class="tab-content">
              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Fixed Asset Account</mat-label>
                <mat-select formControlName="fixedAssetAccountId">
                  @for (account of assetAccounts(); track account.id) {
                    <mat-option [value]="account.id">
                      {{ account.accountCode }} - {{ account.accountName }}
                    </mat-option>
                  }
                </mat-select>
                <mat-hint>GL account for the asset</mat-hint>
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Accumulated Depreciation Account</mat-label>
                <mat-select formControlName="accumulatedDepreciationAccountId">
                  @for (account of accDepAccounts(); track account.id) {
                    <mat-option [value]="account.id">
                      {{ account.accountCode }} - {{ account.accountName }}
                    </mat-option>
                  }
                </mat-select>
                <mat-hint>Contra asset account for accumulated depreciation</mat-hint>
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Depreciation Expense Account</mat-label>
                <mat-select formControlName="depreciationExpenseAccountId">
                  @for (account of expenseAccounts(); track account.id) {
                    <mat-option [value]="account.id">
                      {{ account.accountCode }} - {{ account.accountName }}
                    </mat-option>
                  }
                </mat-select>
                <mat-hint>Expense account for depreciation</mat-hint>
              </mat-form-field>
            </div>
          </mat-tab>
        </mat-tab-group>
      </form>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-button (click)="cancel()">Cancel</button>
      <button mat-raised-button color="primary" (click)="save()" [disabled]="saving || !form.valid">
        @if (saving) {
          <mat-spinner diameter="20"></mat-spinner>
        } @else {
          {{ data.asset ? 'Update' : 'Create' }}
        }
      </button>
    </mat-dialog-actions>
  `,
  styles: [`
    mat-dialog-content {
      min-width: 600px;
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

    .depreciation-summary {
      margin-top: 24px;
      padding: 16px;
      background: #f5f5f5;
      border-radius: 8px;
    }

    .summary-item {
      display: flex;
      justify-content: space-between;
      padding: 8px 0;
    }

    .summary-item .label {
      color: #666;
    }

    .summary-item .value {
      font-weight: 500;
    }

    .summary-item .book-value {
      color: #388e3c;
      font-size: 18px;
    }

    mat-spinner {
      display: inline-block;
    }
  `]
})
export class FixedAssetDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private accountingService = inject(AccountingService);
  private dialogRef = inject(MatDialogRef<FixedAssetDialogComponent>);
  private snackBar = inject(MatSnackBar);
  public data: { asset?: FixedAsset } = inject(MAT_DIALOG_DATA);

  form!: FormGroup;
  saving = false;

  depreciationMethods = signal<DepreciationMethodConfig[]>([]);
  assetAccounts = signal<ChartOfAccount[]>([]);
  accDepAccounts = signal<ChartOfAccount[]>([]);
  expenseAccounts = signal<ChartOfAccount[]>([]);

  ngOnInit(): void {
    this.initForm();
    this.loadLookups();
  }

  private initForm(): void {
    const asset = this.data.asset;
    this.form = this.fb.group({
      assetCode: [asset?.assetCode || '', Validators.required],
      serialNumber: [asset?.serialNumber || ''],
      assetName: [asset?.assetName || '', Validators.required],
      assetCategory: [asset?.assetCategory || 'Equipment', Validators.required],
      assetType: [asset?.assetType || ''],
      location: [asset?.location || ''],
      department: [asset?.department || ''],
      description: [asset?.description || ''],
      // Acquisition
      purchaseDate: [asset?.purchaseDate ? new Date(asset.purchaseDate) : new Date(), Validators.required],
      purchasePrice: [asset?.purchasePrice || 0, [Validators.required, Validators.min(0)]],
      vendorName: [asset?.vendorName || ''],
      invoiceNumber: [asset?.invoiceNumber || ''],
      // Depreciation
      depreciationMethodId: [asset?.depreciationMethodId || '', Validators.required],
      usefulLifeYears: [asset?.usefulLifeYears || 5, [Validators.required, Validators.min(1)]],
      usefulLifeMonths: [asset?.usefulLifeMonths || 0],
      salvageValue: [asset?.salvageValue || 0],
      // GL Accounts
      fixedAssetAccountId: [asset?.fixedAssetAccountId || ''],
      accumulatedDepreciationAccountId: [asset?.accumulatedDepreciationAccountId || ''],
      depreciationExpenseAccountId: [asset?.depreciationExpenseAccountId || '']
    });
  }

  private loadLookups(): void {
    // Load depreciation methods
    this.accountingService.getDepreciationMethods().subscribe({
      next: (result) => this.depreciationMethods.set(result),
      error: (error) => console.error('Error loading depreciation methods:', error)
    });

    // Load asset accounts
    this.accountingService.getChartOfAccounts({ pageNumber: 1, pageSize: 1000 }).subscribe({
      next: (result) => {
        this.assetAccounts.set(result.items.filter(a => a.accountSubType === AccountSubType.FixedAsset));
        this.accDepAccounts.set(result.items.filter(a => a.accountType === AccountType.Asset && a.accountName?.toLowerCase().includes('accumulated')));
      },
      error: (error) => console.error('Error loading accounts:', error)
    });

    // Load expense accounts
    this.accountingService.getChartOfAccounts({ pageNumber: 1, pageSize: 1000 }).subscribe({
      next: (result) => this.expenseAccounts.set(result.items.filter(a => a.accountType === AccountType.Expense)),
      error: (error) => console.error('Error loading expense accounts:', error)
    });
  }

  save(): void {
    if (this.form.invalid) return;

    this.saving = true;
    const assetData = this.form.value;

    const operation = this.data.asset
      ? this.accountingService.updateFixedAsset(this.data.asset.id, assetData)
      : this.accountingService.createFixedAsset(assetData);

    operation.subscribe({
      next: () => {
        this.snackBar.open(
          `Asset ${this.data.asset ? 'updated' : 'created'} successfully`,
          'Close',
          { duration: 3000 }
        );
        this.dialogRef.close(true);
      },
      error: (error) => {
        console.error('Error saving asset:', error);
        this.snackBar.open('Failed to save asset', 'Close', { duration: 3000 });
        this.saving = false;
      }
    });
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}

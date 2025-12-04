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
import { MatTabsModule } from '@angular/material/tabs';
import { MatAutocompleteModule } from '@angular/material/autocomplete';

import { AccountingService } from '@core/services/accounting.service';
import { Vendor, ChartOfAccount, CreateVendorRequest, UpdateVendorRequest, AccountSubType } from '@core/models/accounting.model';

@Component({
  selector: 'app-vendor-dialog',
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
    MatTabsModule,
    MatAutocompleteModule
  ],
  template: `
    <h2 mat-dialog-title>
      {{ isEditMode ? 'Edit Vendor' : 'Create Vendor' }}
    </h2>

    <mat-dialog-content>
      <form [formGroup]="form">
        <mat-tab-group>
          <!-- General Tab -->
          <mat-tab label="General">
            <div class="tab-content">
              <div class="form-row">
                <mat-form-field appearance="outline" class="third-width">
                  <mat-label>Vendor Code</mat-label>
                  <input matInput formControlName="vendorCode" placeholder="e.g., V001">
                  @if (form.get('vendorCode')?.hasError('required')) {
                    <mat-error>Vendor code is required</mat-error>
                  }
                </mat-form-field>

                <mat-form-field appearance="outline" class="two-thirds-width">
                  <mat-label>Vendor Name</mat-label>
                  <input matInput formControlName="vendorName" placeholder="Company name">
                  @if (form.get('vendorName')?.hasError('required')) {
                    <mat-error>Vendor name is required</mat-error>
                  }
                </mat-form-field>
              </div>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Legal Name</mat-label>
                <input matInput formControlName="legalName" placeholder="Legal entity name (if different)">
              </mat-form-field>

              <div class="form-row">
                <mat-form-field appearance="outline" class="half-width">
                  <mat-label>Tax ID / EIN</mat-label>
                  <input matInput formControlName="taxId" placeholder="XX-XXXXXXX">
                </mat-form-field>

                <mat-form-field appearance="outline" class="half-width">
                  <mat-label>Vendor Type</mat-label>
                  <mat-select formControlName="vendorType">
                    <mat-option value="Supplier">Supplier</mat-option>
                    <mat-option value="Contractor">Contractor</mat-option>
                    <mat-option value="Service">Service Provider</mat-option>
                    <mat-option value="Utility">Utility</mat-option>
                    <mat-option value="Other">Other</mat-option>
                  </mat-select>
                </mat-form-field>
              </div>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Expense Account</mat-label>
                <mat-select formControlName="expenseAccountId">
                  @for (account of apAccounts(); track account.id) {
                    <mat-option [value]="account.id">
                      {{ account.accountCode }} - {{ account.accountName }}
                    </mat-option>
                  }
                </mat-select>
              </mat-form-field>

              <mat-checkbox formControlName="is1099Vendor">1099 Vendor</mat-checkbox>
            </div>
          </mat-tab>

          <!-- Contact Tab -->
          <mat-tab label="Contact">
            <div class="tab-content">
              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Contact Name</mat-label>
                <input matInput formControlName="contactName" placeholder="Primary contact">
              </mat-form-field>

              <div class="form-row">
                <mat-form-field appearance="outline" class="half-width">
                  <mat-label>Email</mat-label>
                  <input matInput formControlName="email" type="email" placeholder="email@example.com">
                  @if (form.get('email')?.hasError('email')) {
                    <mat-error>Invalid email format</mat-error>
                  }
                </mat-form-field>

                <mat-form-field appearance="outline" class="half-width">
                  <mat-label>Phone</mat-label>
                  <input matInput formControlName="phone" placeholder="(XXX) XXX-XXXX">
                </mat-form-field>
              </div>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Fax</mat-label>
                <input matInput formControlName="fax" placeholder="(XXX) XXX-XXXX">
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Website</mat-label>
                <input matInput formControlName="website" placeholder="https://example.com">
              </mat-form-field>
            </div>
          </mat-tab>

          <!-- Address Tab -->
          <mat-tab label="Address">
            <div class="tab-content">
              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Address Line 1</mat-label>
                <input matInput formControlName="address1" placeholder="Street address">
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Address Line 2</mat-label>
                <input matInput formControlName="address2" placeholder="Suite, unit, etc.">
              </mat-form-field>

              <div class="form-row">
                <mat-form-field appearance="outline" class="half-width">
                  <mat-label>City</mat-label>
                  <input matInput formControlName="city">
                </mat-form-field>

                <mat-form-field appearance="outline" class="quarter-width">
                  <mat-label>State</mat-label>
                  <input matInput formControlName="state">
                </mat-form-field>

                <mat-form-field appearance="outline" class="quarter-width">
                  <mat-label>ZIP Code</mat-label>
                  <input matInput formControlName="postalCode">
                </mat-form-field>
              </div>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Country</mat-label>
                <input matInput formControlName="country" placeholder="USA">
              </mat-form-field>
            </div>
          </mat-tab>

          <!-- Payment Tab -->
          <mat-tab label="Payment">
            <div class="tab-content">
              <div class="form-row">
                <mat-form-field appearance="outline" class="half-width">
                  <mat-label>Payment Terms</mat-label>
                  <mat-select formControlName="paymentTerms">
                    <mat-option value="Net30">Net 30</mat-option>
                    <mat-option value="Net45">Net 45</mat-option>
                    <mat-option value="Net60">Net 60</mat-option>
                    <mat-option value="DueOnReceipt">Due on Receipt</mat-option>
                    <mat-option value="2_10_Net30">2% 10 Net 30</mat-option>
                  </mat-select>
                </mat-form-field>

                <mat-form-field appearance="outline" class="half-width">
                  <mat-label>Payment Terms Days</mat-label>
                  <input matInput type="number" formControlName="paymentTermsDays" placeholder="30">
                </mat-form-field>
              </div>

              <div class="form-row">
                <mat-form-field appearance="outline" class="half-width">
                  <mat-label>Credit Limit</mat-label>
                  <input matInput type="number" formControlName="creditLimit" placeholder="0.00">
                  <span matTextPrefix>$&nbsp;</span>
                </mat-form-field>

                <mat-form-field appearance="outline" class="half-width">
                  <mat-label>Default Tax Code</mat-label>
                  <input matInput formControlName="defaultTaxCodeId">
                </mat-form-field>
              </div>
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
      min-width: 600px;
    }

    .tab-content {
      padding: 24px 0;
    }

    .form-row {
      display: flex;
      gap: 16px;
    }

    .full-width {
      width: 100%;
    }

    .half-width {
      flex: 1;
    }

    .third-width {
      flex: 0 0 30%;
    }

    .two-thirds-width {
      flex: 0 0 calc(70% - 16px);
    }

    .quarter-width {
      flex: 0 0 calc(25% - 12px);
    }

    h4 {
      margin: 24px 0 16px;
      color: #666;
    }

    mat-dialog-actions {
      padding: 16px 24px;
    }
  `]
})
export class VendorDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<VendorDialogComponent>);
  private data = inject<{ vendor?: Vendor }>(MAT_DIALOG_DATA);
  private accountingService = inject(AccountingService);
  private snackBar = inject(MatSnackBar);

  form!: FormGroup;
  saving = signal(false);
  apAccounts = signal<ChartOfAccount[]>([]);

  get isEditMode(): boolean {
    return !!this.data?.vendor;
  }

  ngOnInit(): void {
    this.initForm();
    this.loadApAccounts();
  }

  initForm(): void {
    const vendor = this.data?.vendor;
    this.form = this.fb.group({
      vendorCode: [vendor?.vendorCode || '', Validators.required],
      vendorName: [vendor?.vendorName || '', Validators.required],
      legalName: [vendor?.legalName || ''],
      taxId: [vendor?.taxId || ''],
      vendorType: [vendor?.vendorType || 'Supplier'],
      expenseAccountId: [vendor?.expenseAccountId || ''],
      is1099Vendor: [vendor?.is1099Vendor ?? false],
      // Contact
      contactName: [vendor?.contactName || ''],
      email: [vendor?.email || '', Validators.email],
      phone: [vendor?.phone || ''],
      fax: [vendor?.fax || ''],
      website: [vendor?.website || ''],
      // Address
      address1: [vendor?.address1 || ''],
      address2: [vendor?.address2 || ''],
      city: [vendor?.city || ''],
      state: [vendor?.state || ''],
      postalCode: [vendor?.postalCode || ''],
      country: [vendor?.country || 'USA'],
      // Payment
      paymentTerms: [vendor?.paymentTerms || 'Net30'],
      paymentTermsDays: [vendor?.paymentTermsDays || 30],
      creditLimit: [vendor?.creditLimit || 0],
      defaultTaxCodeId: [vendor?.defaultTaxCodeId || '']
    });
  }

  loadApAccounts(): void {
    this.accountingService.getAccountsByType('Liability').subscribe({
      next: (accounts) => {
        this.apAccounts.set(accounts.filter(a => a.accountSubType === AccountSubType.CurrentLiability || a.accountName.toLowerCase().includes('payable')));
      }
    });
  }

  save(): void {
    if (this.form.invalid) return;

    this.saving.set(true);
    const formValue = this.form.value;

    if (this.isEditMode) {
      const request: UpdateVendorRequest = {
        id: this.data.vendor!.id,
        ...formValue
      };

      this.accountingService.updateVendor(request).subscribe({
        next: () => {
          this.snackBar.open('Vendor updated successfully', 'Close', { duration: 3000 });
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error('Error updating vendor:', error);
          this.snackBar.open('Failed to update vendor', 'Close', { duration: 3000 });
          this.saving.set(false);
        }
      });
    } else {
      const request: CreateVendorRequest = formValue;

      this.accountingService.createVendor(request).subscribe({
        next: () => {
          this.snackBar.open('Vendor created successfully', 'Close', { duration: 3000 });
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error('Error creating vendor:', error);
          this.snackBar.open('Failed to create vendor', 'Close', { duration: 3000 });
          this.saving.set(false);
        }
      });
    }
  }
}

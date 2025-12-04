import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { AccountingService } from '@core/services/accounting.service';
import { Customer } from '@core/models/accounting.model';

@Component({
  selector: 'app-customer-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatCheckboxModule,
    MatIconModule,
    MatTabsModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  template: `
    <h2 mat-dialog-title>{{ data.customer ? 'Edit Customer' : 'Add Customer' }}</h2>

    <mat-dialog-content>
      <form [formGroup]="form">
        <mat-tab-group>
          <!-- General Tab -->
          <mat-tab label="General">
            <div class="tab-content">
              <div class="form-row">
                <mat-form-field appearance="outline">
                  <mat-label>Customer Number</mat-label>
                  <input matInput formControlName="customerNumber" placeholder="AUTO-GENERATE if blank">
                  <mat-hint>Leave blank to auto-generate</mat-hint>
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Customer Name</mat-label>
                  <input matInput formControlName="customerName" required>
                  @if (form.get('customerName')?.hasError('required')) {
                    <mat-error>Name is required</mat-error>
                  }
                </mat-form-field>
              </div>

              <div class="form-row">
                <mat-form-field appearance="outline">
                  <mat-label>Contact Name</mat-label>
                  <input matInput formControlName="contactName">
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Customer Type</mat-label>
                  <input matInput formControlName="customerType">
                </mat-form-field>
              </div>

              <div class="form-row">
                <mat-form-field appearance="outline">
                  <mat-label>Email</mat-label>
                  <input matInput formControlName="email" type="email">
                  @if (form.get('email')?.hasError('email')) {
                    <mat-error>Invalid email format</mat-error>
                  }
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Phone</mat-label>
                  <input matInput formControlName="phone">
                </mat-form-field>
              </div>

              <mat-checkbox formControlName="isActive">Active</mat-checkbox>
            </div>
          </mat-tab>

          <!-- Address Tab -->
          <mat-tab label="Address">
            <div class="tab-content">
              <h4>Address</h4>
              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Address Line 1</mat-label>
                <input matInput formControlName="address1">
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Address Line 2</mat-label>
                <input matInput formControlName="address2">
              </mat-form-field>

              <div class="form-row">
                <mat-form-field appearance="outline">
                  <mat-label>City</mat-label>
                  <input matInput formControlName="city">
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>State/Province</mat-label>
                  <input matInput formControlName="state">
                </mat-form-field>
              </div>

              <div class="form-row">
                <mat-form-field appearance="outline">
                  <mat-label>Postal Code</mat-label>
                  <input matInput formControlName="postalCode">
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Country</mat-label>
                  <input matInput formControlName="country">
                </mat-form-field>
              </div>
            </div>
          </mat-tab>

          <!-- Financial Tab -->
          <mat-tab label="Financial">
            <div class="tab-content">
              <div class="form-row">
                <mat-form-field appearance="outline">
                  <mat-label>Payment Terms</mat-label>
                  <input matInput formControlName="paymentTerms" placeholder="e.g. Net 30">
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Payment Terms Days</mat-label>
                  <input matInput formControlName="paymentTermsDays" type="number" min="0">
                </mat-form-field>
              </div>

              <div class="form-row">
                <mat-form-field appearance="outline">
                  <mat-label>Credit Limit</mat-label>
                  <input matInput formControlName="creditLimit" type="number" min="0" step="0.01">
                  <span matPrefix>$&nbsp;</span>
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Tax ID</mat-label>
                  <input matInput formControlName="taxId">
                </mat-form-field>
              </div>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Website</mat-label>
                <input matInput formControlName="website" placeholder="https://">
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Notes</mat-label>
                <textarea matInput formControlName="notes" rows="3"></textarea>
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
          {{ data.customer ? 'Update' : 'Create' }}
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

    h4 {
      margin: 16px 0 8px;
      color: #666;
    }

    mat-checkbox {
      margin-bottom: 16px;
    }

    mat-spinner {
      display: inline-block;
    }
  `]
})
export class CustomerDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private accountingService = inject(AccountingService);
  private dialogRef = inject(MatDialogRef<CustomerDialogComponent>);
  private snackBar = inject(MatSnackBar);
  public data: { customer?: Customer } = inject(MAT_DIALOG_DATA);

  form!: FormGroup;
  saving = false;

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    const customer = this.data.customer;
    this.form = this.fb.group({
      customerNumber: [customer?.customerNumber || ''],
      customerName: [customer?.customerName || '', Validators.required],
      contactName: [customer?.contactName || ''],
      customerType: [customer?.customerType || ''],
      email: [customer?.email || '', Validators.email],
      phone: [customer?.phone || ''],
      isActive: [customer?.isActive ?? true],
      // Address
      address1: [customer?.address1 || ''],
      address2: [customer?.address2 || ''],
      city: [customer?.city || ''],
      state: [customer?.state || ''],
      postalCode: [customer?.postalCode || ''],
      country: [customer?.country || ''],
      // Financial
      paymentTerms: [customer?.paymentTerms || ''],
      paymentTermsDays: [customer?.paymentTermsDays || null],
      creditLimit: [customer?.creditLimit || 0],
      taxId: [customer?.taxId || ''],
      website: [customer?.website || ''],
      notes: [customer?.notes || '']
    });
  }

  save(): void {
    if (this.form.invalid) return;

    this.saving = true;
    const customerData = this.form.value;

    const operation = this.data.customer
      ? this.accountingService.updateCustomer({ ...customerData, id: this.data.customer.id })
      : this.accountingService.createCustomer(customerData);

    operation.subscribe({
      next: () => {
        this.snackBar.open(
          `Customer ${this.data.customer ? 'updated' : 'created'} successfully`,
          'Close',
          { duration: 3000 }
        );
        this.dialogRef.close(true);
      },
      error: (error) => {
        console.error('Error saving customer:', error);
        this.snackBar.open('Failed to save customer', 'Close', { duration: 3000 });
        this.saving = false;
      }
    });
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}

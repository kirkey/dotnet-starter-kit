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
import { MatTableModule } from '@angular/material/table';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { AccountingService } from '@core/services/accounting.service';
import { Payment, Customer, Invoice, Bank } from '@core/models/accounting.model';

@Component({
  selector: 'app-payment-dialog',
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
    MatTableModule,
    MatCheckboxModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  template: `
    <h2 mat-dialog-title>{{ data.payment ? 'Edit Payment' : 'Receive Payment' }}</h2>

    <mat-dialog-content>
      <form [formGroup]="form">
        <!-- Payment Header -->
        <div class="form-section">
          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Payment Number</mat-label>
              <input matInput formControlName="paymentNumber" placeholder="AUTO-GENERATE if blank">
              <mat-hint>Leave blank to auto-generate</mat-hint>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Customer</mat-label>
              <mat-select formControlName="customerId" required (selectionChange)="onCustomerChange()">
                @for (customer of customers(); track customer.id) {
                  <mat-option [value]="customer.id">{{ customer.customerName }}</mat-option>
                }
              </mat-select>
              @if (form.get('customerId')?.hasError('required')) {
                <mat-error>Customer is required</mat-error>
              }
            </mat-form-field>
          </div>

          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Payment Date</mat-label>
              <input matInput [matDatepicker]="datePicker" formControlName="paymentDate" required>
              <mat-datepicker-toggle matIconSuffix [for]="datePicker"></mat-datepicker-toggle>
              <mat-datepicker #datePicker></mat-datepicker>
              @if (form.get('paymentDate')?.hasError('required')) {
                <mat-error>Payment date is required</mat-error>
              }
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Amount</mat-label>
              <input matInput type="number" formControlName="amount" required min="0" step="0.01">
              <span matPrefix>$&nbsp;</span>
              @if (form.get('amount')?.hasError('required')) {
                <mat-error>Amount is required</mat-error>
              }
            </mat-form-field>
          </div>

          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Payment Method</mat-label>
              <mat-select formControlName="paymentMethod" required>
                <mat-option value="Cash">Cash</mat-option>
                <mat-option value="Check">Check</mat-option>
                <mat-option value="CreditCard">Credit Card</mat-option>
                <mat-option value="BankTransfer">Bank Transfer</mat-option>
                <mat-option value="ACH">ACH</mat-option>
                <mat-option value="Wire">Wire Transfer</mat-option>
                <mat-option value="Other">Other</mat-option>
              </mat-select>
              @if (form.get('paymentMethod')?.hasError('required')) {
                <mat-error>Payment method is required</mat-error>
              }
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Reference Number</mat-label>
              <input matInput formControlName="referenceNumber" placeholder="Check #, Transaction ID, etc.">
            </mat-form-field>
          </div>

          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Deposit To Account</mat-label>
              <mat-select formControlName="bankAccountId">
                @for (bank of banks(); track bank.id) {
                  <mat-option [value]="bank.id">{{ bank.accountNumber }} - {{ bank.bankName }}</mat-option>
                }
              </mat-select>
            </mat-form-field>
          </div>
        </div>

        <!-- Outstanding Invoices -->
        @if (outstandingInvoices().length > 0) {
          <div class="invoices-section">
            <h4>Apply to Outstanding Invoices</h4>
            <div class="invoices-table">
              <table mat-table [dataSource]="outstandingInvoices()">
                <ng-container matColumnDef="select">
                  <th mat-header-cell *matHeaderCellDef></th>
                  <td mat-cell *matCellDef="let invoice; let i = index">
                    <mat-checkbox [checked]="isInvoiceSelected(invoice)"
                                  (change)="toggleInvoice(invoice, $event.checked)">
                    </mat-checkbox>
                  </td>
                </ng-container>

                <ng-container matColumnDef="invoiceNumber">
                  <th mat-header-cell *matHeaderCellDef>Invoice #</th>
                  <td mat-cell *matCellDef="let invoice">{{ invoice.invoiceNumber }}</td>
                </ng-container>

                <ng-container matColumnDef="invoiceDate">
                  <th mat-header-cell *matHeaderCellDef>Date</th>
                  <td mat-cell *matCellDef="let invoice">{{ invoice.invoiceDate | date:'shortDate' }}</td>
                </ng-container>

                <ng-container matColumnDef="dueDate">
                  <th mat-header-cell *matHeaderCellDef>Due Date</th>
                  <td mat-cell *matCellDef="let invoice">{{ invoice.dueDate | date:'shortDate' }}</td>
                </ng-container>

                <ng-container matColumnDef="totalAmount">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Total</th>
                  <td mat-cell *matCellDef="let invoice" class="text-right">{{ invoice.totalAmount | currency }}</td>
                </ng-container>

                <ng-container matColumnDef="balanceDue">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Balance</th>
                  <td mat-cell *matCellDef="let invoice" class="text-right">
                    {{ (invoice.totalAmount - invoice.paidAmount) | currency }}
                  </td>
                </ng-container>

                <ng-container matColumnDef="applyAmount">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Apply</th>
                  <td mat-cell *matCellDef="let invoice">
                    <mat-form-field appearance="outline" class="apply-field">
                      <input matInput type="number" min="0" step="0.01"
                             [value]="getAppliedAmount(invoice)"
                             (input)="updateAppliedAmount(invoice, $event)"
                             [disabled]="!isInvoiceSelected(invoice)">
                    </mat-form-field>
                  </td>
                </ng-container>

                <tr mat-header-row *matHeaderRowDef="invoiceColumns"></tr>
                <tr mat-row *matRowDef="let row; columns: invoiceColumns;"></tr>
              </table>
            </div>

            <div class="apply-summary">
              <div class="summary-row">
                <span>Payment Amount:</span>
                <span>{{ form.get('amount')?.value | currency }}</span>
              </div>
              <div class="summary-row">
                <span>Applied:</span>
                <span>{{ totalApplied | currency }}</span>
              </div>
              <div class="summary-row">
                <span>Unapplied:</span>
                <span [class.warning]="unapplied < 0">{{ unapplied | currency }}</span>
              </div>
            </div>
          </div>
        }

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Notes</mat-label>
          <textarea matInput formControlName="notes" rows="2"></textarea>
        </mat-form-field>
      </form>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-button (click)="cancel()">Cancel</button>
      <button mat-raised-button color="primary" (click)="save()" [disabled]="saving || !form.valid">
        @if (saving) {
          <mat-spinner diameter="20"></mat-spinner>
        } @else {
          {{ data.payment ? 'Update' : 'Save' }}
        }
      </button>
    </mat-dialog-actions>
  `,
  styles: [`
    mat-dialog-content {
      min-width: 600px;
    }

    .form-section {
      margin-bottom: 24px;
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

    .invoices-section {
      margin: 24px 0;
      padding: 16px;
      background: #f5f5f5;
      border-radius: 8px;
    }

    .invoices-section h4 {
      margin: 0 0 16px;
      color: #333;
    }

    .invoices-table {
      overflow-x: auto;
      background: white;
      border-radius: 4px;
    }

    .invoices-table table {
      width: 100%;
    }

    .apply-field {
      width: 100px;
      margin: 0;
    }

    .text-right {
      text-align: right;
    }

    .apply-summary {
      margin-top: 16px;
      display: flex;
      flex-direction: column;
      align-items: flex-end;
    }

    .summary-row {
      display: flex;
      justify-content: space-between;
      width: 200px;
      padding: 4px 0;
    }

    .warning {
      color: #f44336;
    }

    mat-spinner {
      display: inline-block;
    }
  `]
})
export class PaymentDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private accountingService = inject(AccountingService);
  private dialogRef = inject(MatDialogRef<PaymentDialogComponent>);
  private snackBar = inject(MatSnackBar);
  public data: { payment?: Payment } = inject(MAT_DIALOG_DATA);

  form!: FormGroup;
  saving = false;

  customers = signal<Customer[]>([]);
  banks = signal<Bank[]>([]);
  outstandingInvoices = signal<Invoice[]>([]);

  invoiceColumns = ['select', 'invoiceNumber', 'invoiceDate', 'dueDate', 'totalAmount', 'balanceDue', 'applyAmount'];

  selectedInvoices: Map<string, number> = new Map();

  get totalApplied(): number {
    let total = 0;
    this.selectedInvoices.forEach((amount) => {
      total += amount;
    });
    return total;
  }

  get unapplied(): number {
    return (this.form.get('amount')?.value || 0) - this.totalApplied;
  }

  ngOnInit(): void {
    this.initForm();
    this.loadLookups();
  }

  private initForm(): void {
    const payment = this.data.payment;
    this.form = this.fb.group({
      paymentNumber: [payment?.paymentNumber || ''],
      customerId: [payment?.customerId || '', Validators.required],
      paymentDate: [payment?.paymentDate ? new Date(payment.paymentDate) : new Date(), Validators.required],
      amount: [payment?.amount || 0, [Validators.required, Validators.min(0.01)]],
      paymentMethod: [payment?.paymentMethod || 'Check', Validators.required],
      referenceNumber: [payment?.referenceNumber || ''],
      bankAccountId: [payment?.bankAccountId || ''],
      notes: [payment?.notes || '']
    });

    // Load existing allocations
    if (payment?.allocations) {
      payment.allocations.forEach((alloc: any) => {
        this.selectedInvoices.set(alloc.invoiceId, alloc.appliedAmount);
      });
    }
  }

  private loadLookups(): void {
    // Load customers
    this.accountingService.getCustomers({ pageNumber: 1, pageSize: 1000 }).subscribe({
      next: (result) => {
        this.customers.set(result.items);
        if (this.data.payment?.customerId) {
          this.loadOutstandingInvoices(this.data.payment.customerId);
        }
      },
      error: (error) => console.error('Error loading customers:', error)
    });

    // Load banks
    this.accountingService.getBanks({ pageNumber: 1, pageSize: 100 }).subscribe({
      next: (result) => this.banks.set(result.items),
      error: (error) => console.error('Error loading banks:', error)
    });
  }

  onCustomerChange(): void {
    const customerId = this.form.get('customerId')?.value;
    if (customerId) {
      this.loadOutstandingInvoices(customerId);
    } else {
      this.outstandingInvoices.set([]);
    }
    this.selectedInvoices.clear();
  }

  private loadOutstandingInvoices(customerId: string): void {
    this.accountingService.getInvoices({
      pageNumber: 1,
      pageSize: 100,
      customerId
    }).subscribe({
      next: (result) => this.outstandingInvoices.set(result.items.filter(i => (i.totalAmount - i.paidAmount) > 0)),
      error: (error) => console.error('Error loading invoices:', error)
    });
  }

  isInvoiceSelected(invoice: Invoice): boolean {
    return this.selectedInvoices.has(invoice.id);
  }

  getAppliedAmount(invoice: Invoice): number {
    return this.selectedInvoices.get(invoice.id) || 0;
  }

  toggleInvoice(invoice: Invoice, selected: boolean): void {
    if (selected) {
      const balance = invoice.totalAmount - invoice.paidAmount;
      const available = this.unapplied;
      const applyAmount = Math.min(balance, available > 0 ? available : balance);
      this.selectedInvoices.set(invoice.id, applyAmount);
    } else {
      this.selectedInvoices.delete(invoice.id);
    }
  }

  updateAppliedAmount(invoice: Invoice, event: Event): void {
    const input = event.target as HTMLInputElement;
    const amount = parseFloat(input.value) || 0;
    const balance = invoice.totalAmount - invoice.paidAmount;
    const validAmount = Math.min(Math.max(0, amount), balance);
    
    if (validAmount > 0) {
      this.selectedInvoices.set(invoice.id, validAmount);
    } else {
      this.selectedInvoices.delete(invoice.id);
    }
  }

  save(): void {
    if (this.form.invalid) return;

    this.saving = true;
    const paymentData = {
      ...this.form.value,
      applications: Array.from(this.selectedInvoices.entries()).map(([invoiceId, amount]) => ({
        invoiceId,
        amount
      }))
    };

    const operation = this.data.payment
      ? this.accountingService.updatePayment(this.data.payment.id, paymentData)
      : this.accountingService.createPayment(paymentData);

    operation.subscribe({
      next: () => {
        this.snackBar.open(
          `Payment ${this.data.payment ? 'updated' : 'recorded'} successfully`,
          'Close',
          { duration: 3000 }
        );
        this.dialogRef.close(true);
      },
      error: (error) => {
        console.error('Error saving payment:', error);
        this.snackBar.open('Failed to save payment', 'Close', { duration: 3000 });
        this.saving = false;
      }
    });
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}

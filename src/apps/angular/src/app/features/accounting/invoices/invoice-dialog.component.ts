import { Component, inject, OnInit, signal, computed } from '@angular/core';
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
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatTooltipModule } from '@angular/material/tooltip';

import { AccountingService } from '@core/services/accounting.service';
import { Invoice, InvoiceLine, Customer, ChartOfAccount, TaxCode, AccountType, ApprovalStatus } from '@core/models/accounting.model';

@Component({
  selector: 'app-invoice-dialog',
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
    MatAutocompleteModule,
    MatTooltipModule
  ],
  template: `
    <h2 mat-dialog-title>{{ data.invoice ? 'Edit Invoice' : 'Create Invoice' }}</h2>

    <mat-dialog-content>
      <form [formGroup]="form">
        <mat-tab-group>
          <!-- Header Tab -->
          <mat-tab label="Invoice Details">
            <div class="tab-content">
              <div class="form-row">
                <mat-form-field appearance="outline">
                  <mat-label>Invoice Number</mat-label>
                  <input matInput formControlName="invoiceNumber" placeholder="AUTO-GENERATE if blank">
                  <mat-hint>Leave blank to auto-generate</mat-hint>
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Customer</mat-label>
                  <mat-select formControlName="customerId" required>
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
                  <mat-label>Invoice Date</mat-label>
                  <input matInput [matDatepicker]="invoicePicker" formControlName="invoiceDate" required>
                  <mat-datepicker-toggle matIconSuffix [for]="invoicePicker"></mat-datepicker-toggle>
                  <mat-datepicker #invoicePicker></mat-datepicker>
                  @if (form.get('invoiceDate')?.hasError('required')) {
                    <mat-error>Invoice date is required</mat-error>
                  }
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Due Date</mat-label>
                  <input matInput [matDatepicker]="duePicker" formControlName="dueDate" required>
                  <mat-datepicker-toggle matIconSuffix [for]="duePicker"></mat-datepicker-toggle>
                  <mat-datepicker #duePicker></mat-datepicker>
                  @if (form.get('dueDate')?.hasError('required')) {
                    <mat-error>Due date is required</mat-error>
                  }
                </mat-form-field>
              </div>

              <div class="form-row">
                <mat-form-field appearance="outline">
                  <mat-label>Reference Number</mat-label>
                  <input matInput formControlName="referenceNumber">
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Description</mat-label>
                  <input matInput formControlName="description">
                </mat-form-field>
              </div>
            </div>
          </mat-tab>

          <!-- Line Items Tab -->
          <mat-tab label="Line Items">
            <div class="tab-content">
              <div class="lines-header">
                <span>Invoice Lines</span>
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
                          @for (account of revenueAccounts(); track account.id) {
                            <mat-option [value]="account.id">
                              {{ account.accountCode }} - {{ account.accountName }}
                            </mat-option>
                          }
                        </mat-select>
                      </mat-form-field>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="description">
                    <th mat-header-cell *matHeaderCellDef>Description</th>
                    <td mat-cell *matCellDef="let line; let i = index" [formGroupName]="i">
                      <mat-form-field appearance="outline" class="table-field description">
                        <input matInput formControlName="description">
                      </mat-form-field>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="quantity">
                    <th mat-header-cell *matHeaderCellDef class="text-right">Qty</th>
                    <td mat-cell *matCellDef="let line; let i = index" [formGroupName]="i">
                      <mat-form-field appearance="outline" class="table-field small">
                        <input matInput type="number" formControlName="quantity" min="0" step="1"
                               (input)="calculateLineTotal(i)">
                      </mat-form-field>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="unitPrice">
                    <th mat-header-cell *matHeaderCellDef class="text-right">Unit Price</th>
                    <td mat-cell *matCellDef="let line; let i = index" [formGroupName]="i">
                      <mat-form-field appearance="outline" class="table-field small">
                        <input matInput type="number" formControlName="unitPrice" min="0" step="0.01"
                               (input)="calculateLineTotal(i)">
                      </mat-form-field>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="taxCode">
                    <th mat-header-cell *matHeaderCellDef>Tax</th>
                    <td mat-cell *matCellDef="let line; let i = index" [formGroupName]="i">
                      <mat-form-field appearance="outline" class="table-field small">
                        <mat-select formControlName="taxCodeId" (selectionChange)="calculateLineTotal(i)">
                          <mat-option [value]="null">None</mat-option>
                          @for (tax of taxCodes(); track tax.id) {
                            <mat-option [value]="tax.id">{{ tax.taxCode }} ({{ tax.rate }}%)</mat-option>
                          }
                        </mat-select>
                      </mat-form-field>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="amount">
                    <th mat-header-cell *matHeaderCellDef class="text-right">Amount</th>
                    <td mat-cell *matCellDef="let line; let i = index" [formGroupName]="i" class="text-right">
                      {{ getLineTotal(i) | currency }}
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="actions">
                    <th mat-header-cell *matHeaderCellDef></th>
                    <td mat-cell *matCellDef="let line; let i = index">
                      <button mat-icon-button color="warn" (click)="removeLine(i)" type="button"
                              matTooltip="Remove line">
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
                  <p>No line items added. Click "Add Line" to add invoice items.</p>
                </div>
              }

              <!-- Totals -->
              <div class="totals-section">
                <div class="totals-row">
                  <span>Subtotal:</span>
                  <span>{{ subtotal() | currency }}</span>
                </div>
                <div class="totals-row">
                  <span>Tax:</span>
                  <span>{{ totalTax() | currency }}</span>
                </div>
                <div class="totals-row grand-total">
                  <span>Total:</span>
                  <span>{{ grandTotal() | currency }}</span>
                </div>
              </div>
            </div>
          </mat-tab>

          <!-- Notes Tab -->
          <mat-tab label="Notes">
            <div class="tab-content">
              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Notes</mat-label>
                <textarea matInput formControlName="notes" rows="4"
                          placeholder="Notes for this invoice"></textarea>
              </mat-form-field>
            </div>
          </mat-tab>
        </mat-tab-group>
      </form>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-button (click)="cancel()">Cancel</button>
      @if (!data.invoice || data.invoice.status === ApprovalStatus.Draft) {
        <button mat-raised-button (click)="saveAsDraft()" [disabled]="saving">
          Save as Draft
        </button>
      }
      <button mat-raised-button color="primary" (click)="save()" [disabled]="saving || !form.valid">
        @if (saving) {
          <mat-spinner diameter="20"></mat-spinner>
        } @else {
          {{ data.invoice ? 'Update' : 'Create' }}
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
      min-width: 200px;
    }

    .table-field.small {
      max-width: 100px;
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

    .totals-section {
      margin-top: 24px;
      display: flex;
      flex-direction: column;
      align-items: flex-end;
    }

    .totals-row {
      display: flex;
      justify-content: space-between;
      width: 250px;
      padding: 8px 0;
      border-bottom: 1px solid #eee;
    }

    .totals-row.grand-total {
      font-weight: bold;
      font-size: 18px;
      border-top: 2px solid #333;
      border-bottom: 2px solid #333;
      margin-top: 8px;
      padding: 12px 0;
    }

    mat-spinner {
      display: inline-block;
    }
  `]
})
export class InvoiceDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private accountingService = inject(AccountingService);
  private dialogRef = inject(MatDialogRef<InvoiceDialogComponent>);
  private snackBar = inject(MatSnackBar);
  public data: { invoice?: Invoice } = inject(MAT_DIALOG_DATA);

  // Expose to template
  readonly ApprovalStatus = ApprovalStatus;

  form!: FormGroup;
  saving = false;

  customers = signal<Customer[]>([]);
  revenueAccounts = signal<ChartOfAccount[]>([]);
  taxCodes = signal<TaxCode[]>([]);

  lineColumns = ['account', 'description', 'quantity', 'unitPrice', 'taxCode', 'amount', 'actions'];

  subtotal = computed(() => {
    return this.linesArray.controls.reduce((sum, line) => {
      const qty = line.get('quantity')?.value || 0;
      const price = line.get('unitPrice')?.value || 0;
      return sum + (qty * price);
    }, 0);
  });

  totalTax = computed(() => {
    return this.linesArray.controls.reduce((sum, line) => {
      const qty = line.get('quantity')?.value || 0;
      const price = line.get('unitPrice')?.value || 0;
      const taxCodeId = line.get('taxCodeId')?.value;
      const taxCode = this.taxCodes().find(t => t.id === taxCodeId);
      const taxRate = taxCode?.rate || 0;
      return sum + ((qty * price) * (taxRate / 100));
    }, 0);
  });

  grandTotal = computed(() => this.subtotal() + this.totalTax());

  get linesArray(): FormArray {
    return this.form.get('lines') as FormArray;
  }

  ngOnInit(): void {
    this.initForm();
    this.loadLookups();
  }

  private initForm(): void {
    const invoice = this.data.invoice;
    this.form = this.fb.group({
      invoiceNumber: [invoice?.invoiceNumber || ''],
      customerId: [invoice?.customerId || '', Validators.required],
      invoiceDate: [invoice?.invoiceDate ? new Date(invoice.invoiceDate) : new Date(), Validators.required],
      dueDate: [invoice?.dueDate ? new Date(invoice.dueDate) : this.getDefaultDueDate(), Validators.required],
      referenceNumber: [invoice?.referenceNumber || ''],
      description: [invoice?.description || ''],
      notes: [invoice?.notes || ''],
      lines: this.fb.array([])
    });

    // Load existing lines
    if (invoice?.lines) {
      invoice.lines.forEach(line => this.addLine(line));
    } else {
      this.addLine(); // Add empty line for new invoice
    }
  }

  private getDefaultDueDate(): Date {
    const date = new Date();
    date.setDate(date.getDate() + 30); // Default to Net 30
    return date;
  }

  private loadLookups(): void {
    // Load customers
    this.accountingService.getCustomers({ pageNumber: 1, pageSize: 1000 }).subscribe({
      next: (result) => this.customers.set(result.items),
      error: (error) => console.error('Error loading customers:', error)
    });

    // Load revenue accounts
    this.accountingService.getChartOfAccounts({ pageNumber: 1, pageSize: 1000 }).subscribe({
      next: (result) => this.revenueAccounts.set(result.items.filter(a => a.accountType === AccountType.Revenue)),
      error: (error) => console.error('Error loading accounts:', error)
    });

    // Load tax codes
    this.accountingService.getTaxCodes({ pageNumber: 1, pageSize: 100 }).subscribe({
      next: (result) => this.taxCodes.set(result.items),
      error: (error) => console.error('Error loading tax codes:', error)
    });
  }

  addLine(line?: InvoiceLine): void {
    const lineForm = this.fb.group({
      id: [line?.id || null],
      accountId: [line?.accountId || '', Validators.required],
      description: [line?.description || ''],
      quantity: [line?.quantity || 1, [Validators.required, Validators.min(0)]],
      unitPrice: [line?.unitPrice || 0, [Validators.required, Validators.min(0)]],
      taxCodeId: [line?.taxCodeId || null]
    });
    this.linesArray.push(lineForm);
  }

  removeLine(index: number): void {
    this.linesArray.removeAt(index);
  }

  calculateLineTotal(index: number): void {
    // Trigger recomputation of totals
    this.form.updateValueAndValidity();
  }

  getLineTotal(index: number): number {
    const line = this.linesArray.at(index);
    const qty = line.get('quantity')?.value || 0;
    const price = line.get('unitPrice')?.value || 0;
    const taxCodeId = line.get('taxCodeId')?.value;
    const taxCode = this.taxCodes().find(t => t.id === taxCodeId);
    const taxRate = taxCode?.rate || 0;
    const subtotal = qty * price;
    return subtotal + (subtotal * (taxRate / 100));
  }

  saveAsDraft(): void {
    this.saveInvoice('Draft');
  }

  save(): void {
    this.saveInvoice('Pending');
  }

  private saveInvoice(status: string): void {
    if (this.form.invalid && status !== 'Draft') return;

    this.saving = true;
    const invoiceData = {
      ...this.form.value,
      status,
      subtotal: this.subtotal(),
      taxAmount: this.totalTax(),
      totalAmount: this.grandTotal()
    };

    const operation = this.data.invoice
      ? this.accountingService.updateInvoice(this.data.invoice.id, invoiceData)
      : this.accountingService.createInvoice(invoiceData);

    operation.subscribe({
      next: () => {
        this.snackBar.open(
          `Invoice ${this.data.invoice ? 'updated' : 'created'} successfully`,
          'Close',
          { duration: 3000 }
        );
        this.dialogRef.close(true);
      },
      error: (error) => {
        console.error('Error saving invoice:', error);
        this.snackBar.open('Failed to save invoice', 'Close', { duration: 3000 });
        this.saving = false;
      }
    });
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}

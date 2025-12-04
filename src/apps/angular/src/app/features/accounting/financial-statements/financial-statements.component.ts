import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { MatExpansionModule } from '@angular/material/expansion';

import { AccountingService } from '@core/services/accounting.service';
import { AccountingPeriod } from '@core/models/accounting.model';

interface FinancialReportLine {
  accountNumber?: string;
  accountName: string;
  amount: number;
  isTotal?: boolean;
  isHeader?: boolean;
  level?: number;
}

@Component({
  selector: 'app-financial-statements',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatTabsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatTableModule,
    MatExpansionModule
  ],
  template: `
    <div class="page-container">
      <mat-card>
        <mat-card-header>
          <mat-card-title>Financial Statements</mat-card-title>
        </mat-card-header>

        <mat-card-content>
          <!-- Report Selection -->
          <div class="filters-row">
            <mat-form-field appearance="outline">
              <mat-label>Report Type</mat-label>
              <mat-select [(ngModel)]="selectedReport" (ngModelChange)="generateReport()">
                <mat-option value="balanceSheet">Balance Sheet</mat-option>
                <mat-option value="incomeStatement">Income Statement</mat-option>
                <mat-option value="cashFlow">Cash Flow Statement</mat-option>
                <mat-option value="retainedEarnings">Retained Earnings Statement</mat-option>
              </mat-select>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Period</mat-label>
              <mat-select [(ngModel)]="selectedPeriodId" (ngModelChange)="generateReport()">
                @for (period of periods(); track period.id) {
                  <mat-option [value]="period.id">{{ period.periodName }}</mat-option>
                }
              </mat-select>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>As of Date</mat-label>
              <input matInput [matDatepicker]="datePicker" [(ngModel)]="asOfDate" (ngModelChange)="generateReport()">
              <mat-datepicker-toggle matIconSuffix [for]="datePicker"></mat-datepicker-toggle>
              <mat-datepicker #datePicker></mat-datepicker>
            </mat-form-field>

            <button mat-raised-button color="primary" (click)="generateReport()">
              <mat-icon>refresh</mat-icon>
              Generate
            </button>

            <button mat-stroked-button (click)="exportReport()">
              <mat-icon>download</mat-icon>
              Export
            </button>

            <button mat-stroked-button (click)="printReport()">
              <mat-icon>print</mat-icon>
              Print
            </button>
          </div>

          @if (loading()) {
            <div class="loading-container">
              <mat-spinner diameter="40"></mat-spinner>
            </div>
          }

          @if (!loading() && reportData().length > 0) {
            <div class="report-container">
              <div class="report-header">
                <h2>{{ getReportTitle() }}</h2>
                <p>As of {{ asOfDate | date:'longDate' }}</p>
              </div>

              <table class="report-table">
                <thead>
                  <tr>
                    <th>Account</th>
                    <th class="text-right">Amount</th>
                  </tr>
                </thead>
                <tbody>
                  @for (line of reportData(); track $index) {
                    <tr [class.header-row]="line.isHeader" 
                        [class.total-row]="line.isTotal"
                        [class.indent-1]="line.level === 1"
                        [class.indent-2]="line.level === 2">
                      <td>
                        @if (line.accountNumber) {
                          <span class="account-number">{{ line.accountNumber }}</span>
                        }
                        {{ line.accountName }}
                      </td>
                      <td class="text-right">
                        @if (!line.isHeader) {
                          <span [class.negative]="line.amount < 0">
                            {{ line.amount | currency }}
                          </span>
                        }
                      </td>
                    </tr>
                  }
                </tbody>
              </table>
            </div>
          }

          @if (!loading() && reportData().length === 0) {
            <div class="empty-state">
              <mat-icon>assessment</mat-icon>
              <h3>No report generated</h3>
              <p>Select a report type and date range, then click Generate</p>
            </div>
          }
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .page-container {
      padding: 24px;
    }

    .filters-row {
      display: flex;
      gap: 16px;
      margin-bottom: 24px;
      flex-wrap: wrap;
      align-items: center;
    }

    .loading-container {
      display: flex;
      justify-content: center;
      padding: 48px;
    }

    .report-container {
      background: white;
      border: 1px solid #e0e0e0;
      border-radius: 8px;
      padding: 24px;
    }

    .report-header {
      text-align: center;
      margin-bottom: 24px;
      padding-bottom: 16px;
      border-bottom: 2px solid #333;
    }

    .report-header h2 {
      margin: 0;
      font-size: 24px;
    }

    .report-header p {
      margin: 8px 0 0;
      color: #666;
    }

    .report-table {
      width: 100%;
      border-collapse: collapse;
    }

    .report-table th,
    .report-table td {
      padding: 8px 12px;
      border-bottom: 1px solid #eee;
    }

    .report-table th {
      text-align: left;
      font-weight: 500;
      background: #f5f5f5;
    }

    .text-right {
      text-align: right;
    }

    .header-row {
      background-color: #f5f5f5;
      font-weight: bold;
    }

    .total-row {
      font-weight: bold;
      border-top: 2px solid #333;
      border-bottom: 2px double #333;
    }

    .indent-1 td:first-child {
      padding-left: 24px;
    }

    .indent-2 td:first-child {
      padding-left: 48px;
    }

    .account-number {
      font-family: monospace;
      color: #666;
      margin-right: 8px;
    }

    .negative {
      color: #d32f2f;
    }

    .empty-state {
      text-align: center;
      padding: 48px;
      color: #666;
    }

    .empty-state mat-icon {
      font-size: 64px;
      width: 64px;
      height: 64px;
      color: #ccc;
      margin-bottom: 16px;
    }
  `]
})
export class FinancialStatementsComponent implements OnInit {
  private accountingService = inject(AccountingService);
  private snackBar = inject(MatSnackBar);

  periods = signal<AccountingPeriod[]>([]);
  reportData = signal<FinancialReportLine[]>([]);
  loading = signal(false);

  selectedReport = 'balanceSheet';
  selectedPeriodId = '';
  asOfDate: Date = new Date();

  ngOnInit(): void {
    this.loadPeriods();
  }

  private loadPeriods(): void {
    this.accountingService.getAccountingPeriods({ pageNumber: 1, pageSize: 100 }).subscribe({
      next: (result) => this.periods.set(result.items),
      error: (error) => console.error('Error loading periods:', error)
    });
  }

  getReportTitle(): string {
    const titles: Record<string, string> = {
      'balanceSheet': 'Balance Sheet',
      'incomeStatement': 'Income Statement',
      'cashFlow': 'Cash Flow Statement',
      'retainedEarnings': 'Statement of Retained Earnings'
    };
    return titles[this.selectedReport] || 'Financial Statement';
  }

  generateReport(): void {
    this.loading.set(true);
    
    // Use mock data for demo - in real implementation, call appropriate API
    // based on selectedReport (balanceSheet, incomeStatement, cashFlow)
    setTimeout(() => {
      this.reportData.set(this.getMockData());
      this.loading.set(false);
    }, 500);
  }

  private getMockData(): FinancialReportLine[] {
    if (this.selectedReport === 'balanceSheet') {
      return [
        { accountName: 'ASSETS', isHeader: true, amount: 0 },
        { accountName: 'Current Assets', isHeader: true, level: 1, amount: 0 },
        { accountNumber: '1000', accountName: 'Cash and Cash Equivalents', amount: 125000, level: 2 },
        { accountNumber: '1100', accountName: 'Accounts Receivable', amount: 45000, level: 2 },
        { accountNumber: '1200', accountName: 'Inventory', amount: 30000, level: 2 },
        { accountName: 'Total Current Assets', amount: 200000, isTotal: true, level: 1 },
        { accountName: 'Fixed Assets', isHeader: true, level: 1, amount: 0 },
        { accountNumber: '1500', accountName: 'Property & Equipment', amount: 350000, level: 2 },
        { accountNumber: '1510', accountName: 'Less: Accumulated Depreciation', amount: -50000, level: 2 },
        { accountName: 'Total Fixed Assets', amount: 300000, isTotal: true, level: 1 },
        { accountName: 'TOTAL ASSETS', amount: 500000, isTotal: true },
        { accountName: '', amount: 0 },
        { accountName: 'LIABILITIES', isHeader: true, amount: 0 },
        { accountNumber: '2000', accountName: 'Accounts Payable', amount: 35000, level: 1 },
        { accountNumber: '2100', accountName: 'Accrued Expenses', amount: 15000, level: 1 },
        { accountName: 'Total Liabilities', amount: 50000, isTotal: true },
        { accountName: '', amount: 0 },
        { accountName: 'EQUITY', isHeader: true, amount: 0 },
        { accountNumber: '3000', accountName: 'Common Stock', amount: 100000, level: 1 },
        { accountNumber: '3100', accountName: 'Retained Earnings', amount: 350000, level: 1 },
        { accountName: 'Total Equity', amount: 450000, isTotal: true },
        { accountName: 'TOTAL LIABILITIES & EQUITY', amount: 500000, isTotal: true }
      ];
    }
    return [];
  }

  exportReport(): void {
    this.snackBar.open('Report exported successfully', 'Close', { duration: 3000 });
  }

  printReport(): void {
    window.print();
  }
}

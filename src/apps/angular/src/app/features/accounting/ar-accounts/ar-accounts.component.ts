import { Component, inject, signal, OnInit, computed } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCardModule } from '@angular/material/card';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { NotificationService } from '@core/services/notification.service';
import { AccountingService } from '@core/services/accounting.service';
import { ArAccount } from '@core/models/accounting.model';

@Component({
  selector: 'app-ar-accounts',
  standalone: true,
  imports: [
    CommonModule,
    CurrencyPipe,
    DatePipe,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatMenuModule,
    MatTooltipModule,
    MatCardModule,
    PageHeaderComponent
  ],
  template: `
    <div class="ar-accounts-container">
      <app-page-header 
        title="AR Accounts" 
        subtitle="View accounts receivable balances by customer"
        icon="account_balance">
      </app-page-header>

      <!-- Toolbar -->
      <div class="toolbar">
        <div class="filters">
          <mat-form-field appearance="outline" class="search-field">
            <mat-label>Search accounts</mat-label>
            <input matInput 
                   [value]="searchQuery()" 
                   (input)="onSearch($event)"
                   placeholder="Search by customer, account...">
            <mat-icon matPrefix>search</mat-icon>
          </mat-form-field>
        </div>

        <div class="actions">
          <button mat-stroked-button (click)="exportReport()">
            <mat-icon>download</mat-icon>
            Export
          </button>
        </div>
      </div>

      <!-- Summary Cards -->
      <div class="summary-cards">
        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon total">
              <mat-icon>account_balance</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Total AR Balance</span>
              <span class="summary-value">{{ totalBalance() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon customers">
              <mat-icon>people</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Active Customers</span>
              <span class="summary-value">{{ activeCustomerCount() }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon overdue">
              <mat-icon>warning</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Over Credit Limit</span>
              <span class="summary-value">{{ overCreditCount() }}</span>
            </div>
          </mat-card-content>
        </mat-card>
      </div>

      <!-- Loading -->
      @if (isLoading()) {
        <div class="loading-container">
          <mat-spinner diameter="48"></mat-spinner>
        </div>
      } @else {
        <!-- Table -->
        <div class="table-container">
          <table mat-table [dataSource]="filteredAccounts()" matSort (matSortChange)="onSort($event)">
            <!-- Account Number Column -->
            <ng-container matColumnDef="accountNumber">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Account #</th>
              <td mat-cell *matCellDef="let account">{{ account.accountNumber }}</td>
            </ng-container>

            <!-- Account Name Column -->
            <ng-container matColumnDef="accountName">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Account Name</th>
              <td mat-cell *matCellDef="let account">{{ account.accountName }}</td>
            </ng-container>

            <!-- Customer Column -->
            <ng-container matColumnDef="customer">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Customer</th>
              <td mat-cell *matCellDef="let account">
                <div class="customer-info">
                  <span class="customer-name">{{ account.customerName }}</span>
                  <span class="customer-number">{{ account.customerNumber }}</span>
                </div>
              </td>
            </ng-container>

            <!-- GL Account Column -->
            <ng-container matColumnDef="glAccount">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>GL Account</th>
              <td mat-cell *matCellDef="let account">
                <div class="gl-info">
                  <span class="gl-code">{{ account.glAccountCode }}</span>
                  <span class="gl-name">{{ account.glAccountName }}</span>
                </div>
              </td>
            </ng-container>

            <!-- Credit Limit Column -->
            <ng-container matColumnDef="creditLimit">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Credit Limit</th>
              <td mat-cell *matCellDef="let account">{{ account.creditLimit | currency }}</td>
            </ng-container>

            <!-- Current Balance Column -->
            <ng-container matColumnDef="currentBalance">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Current Balance</th>
              <td mat-cell *matCellDef="let account">
                <span [class.over-limit]="account.currentBalance > account.creditLimit">
                  {{ account.currentBalance | currency }}
                </span>
              </td>
            </ng-container>

            <!-- Available Credit Column -->
            <ng-container matColumnDef="availableCredit">
              <th mat-header-cell *matHeaderCellDef>Available</th>
              <td mat-cell *matCellDef="let account">
                @if (account.creditLimit - account.currentBalance >= 0) {
                  {{ (account.creditLimit - account.currentBalance) | currency }}
                } @else {
                  <span class="over-limit">{{ (account.creditLimit - account.currentBalance) | currency }}</span>
                }
              </td>
            </ng-container>

            <!-- Last Activity Column -->
            <ng-container matColumnDef="lastActivity">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Last Activity</th>
              <td mat-cell *matCellDef="let account">
                {{ account.lastActivityDate ? (account.lastActivityDate | date:'mediumDate') : '-' }}
              </td>
            </ng-container>

            <!-- Actions Column -->
            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef></th>
              <td mat-cell *matCellDef="let account">
                <button mat-icon-button [matMenuTriggerFor]="menu">
                  <mat-icon>more_vert</mat-icon>
                </button>
                <mat-menu #menu="matMenu">
                  <button mat-menu-item (click)="viewDetails(account)">
                    <mat-icon>visibility</mat-icon>
                    <span>View Details</span>
                  </button>
                  <button mat-menu-item (click)="viewTransactions(account)">
                    <mat-icon>list</mat-icon>
                    <span>View Transactions</span>
                  </button>
                  <button mat-menu-item (click)="viewCustomer(account)">
                    <mat-icon>person</mat-icon>
                    <span>View Customer</span>
                  </button>
                  <button mat-menu-item (click)="sendStatement(account)">
                    <mat-icon>email</mat-icon>
                    <span>Send Statement</span>
                  </button>
                </mat-menu>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
          </table>

          <mat-paginator 
            [length]="totalAccounts()"
            [pageSize]="pageSize()"
            [pageSizeOptions]="[10, 25, 50, 100]"
            (page)="onPageChange($event)"
            showFirstLastButtons>
          </mat-paginator>
        </div>
      }
    </div>
  `,
  styles: [`
    .ar-accounts-container {
      padding: 24px;
    }

    .toolbar {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      margin-bottom: 24px;
      gap: 16px;
    }

    .filters {
      display: flex;
      gap: 12px;
      flex: 1;
    }

    .search-field {
      min-width: 300px;
    }

    .summary-cards {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
      gap: 16px;
      margin-bottom: 24px;
    }

    .summary-card mat-card-content {
      display: flex;
      align-items: center;
      gap: 16px;
      padding: 16px !important;
    }

    .summary-icon {
      width: 48px;
      height: 48px;
      border-radius: 12px;
      display: flex;
      align-items: center;
      justify-content: center;
    }

    .summary-icon.total {
      background: #dcfce7;
      color: #16a34a;
    }

    .summary-icon.customers {
      background: #e0e7ff;
      color: #4f46e5;
    }

    .summary-icon.overdue {
      background: #fee2e2;
      color: #dc2626;
    }

    .summary-info {
      display: flex;
      flex-direction: column;
    }

    .summary-label {
      font-size: 12px;
      color: var(--text-secondary);
    }

    .summary-value {
      font-size: 20px;
      font-weight: 600;
    }

    .loading-container {
      display: flex;
      justify-content: center;
      padding: 48px;
    }

    .table-container {
      background: var(--surface-color);
      border-radius: 8px;
      overflow: hidden;
      box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
    }

    table {
      width: 100%;
    }

    .customer-info, .gl-info {
      display: flex;
      flex-direction: column;
    }

    .customer-name, .gl-code {
      font-weight: 500;
    }

    .customer-number, .gl-name {
      font-size: 12px;
      color: var(--text-secondary);
    }

    .over-limit {
      color: #dc2626;
      font-weight: 500;
    }
  `]
})
export class ArAccountsComponent implements OnInit {
  private notification = inject(NotificationService);
  private accountingService = inject(AccountingService);

  accounts = signal<ArAccount[]>([]);
  isLoading = signal(false);
  searchQuery = signal('');

  pageSize = signal(10);
  pageIndex = signal(0);
  totalAccounts = signal(0);

  displayedColumns = ['accountNumber', 'accountName', 'customer', 'glAccount', 'creditLimit', 'currentBalance', 'availableCredit', 'lastActivity', 'actions'];

  filteredAccounts = computed(() => {
    let result = this.accounts();
    const query = this.searchQuery().toLowerCase();
    if (query) {
      result = result.filter(a => 
        a.accountNumber.toLowerCase().includes(query) ||
        a.customerName.toLowerCase().includes(query)
      );
    }
    return result;
  });

  totalBalance = computed(() => 
    this.accounts().reduce((sum, a) => sum + a.currentBalance, 0)
  );

  activeCustomerCount = computed(() => 
    new Set(this.accounts().map(a => a.customerId)).size
  );

  overCreditCount = computed(() => 
    this.accounts().filter(a => a.currentBalance > a.creditLimit).length
  );

  ngOnInit(): void {
    this.loadAccounts();
  }

  async loadAccounts(): Promise<void> {
    this.isLoading.set(true);
    try {
      const mockAccounts: ArAccount[] = [
        { id: '1', accountNumber: 'AR-001', accountName: 'Acme Corp AR', customerId: '1', customerNumber: 'C001', customerName: 'Acme Corporation', generalLedgerAccountId: '1', glAccountCode: '1200', glAccountName: 'Accounts Receivable', creditLimit: 100000, currentBalance: 45000, lastActivityDate: new Date('2024-01-18'), isActive: true },
        { id: '2', accountNumber: 'AR-002', accountName: 'Tech Solutions AR', customerId: '2', customerNumber: 'C002', customerName: 'Tech Solutions Inc', generalLedgerAccountId: '1', glAccountCode: '1200', glAccountName: 'Accounts Receivable', creditLimit: 50000, currentBalance: 55000, lastActivityDate: new Date('2024-01-22'), isActive: true }
      ];
      await new Promise(resolve => setTimeout(resolve, 300));
      this.accounts.set(mockAccounts);
      this.totalAccounts.set(mockAccounts.length);
    } finally {
      this.isLoading.set(false);
    }
  }

  onSearch(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.searchQuery.set(value);
  }

  onSort(sort: Sort): void {}
  onPageChange(event: PageEvent): void {
    this.pageIndex.set(event.pageIndex);
    this.pageSize.set(event.pageSize);
  }

  exportReport(): void {
    this.notification.success('Report exported');
  }

  viewDetails(account: ArAccount): void {}
  viewTransactions(account: ArAccount): void {}
  viewCustomer(account: ArAccount): void {}
  sendStatement(account: ArAccount): void {
    this.notification.success('Statement sent to customer');
  }
}

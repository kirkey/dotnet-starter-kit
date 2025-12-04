import { Component, OnInit, inject, signal, computed, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSortModule, MatSort, Sort } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSelectModule } from '@angular/material/select';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTreeModule, MatTreeNestedDataSource } from '@angular/material/tree';
import { NestedTreeControl } from '@angular/cdk/tree';

import { AccountingService } from '@core/services/accounting.service';
import { ChartOfAccount, AccountType, AccountSubType, AccountingSearchRequest } from '@core/models/accounting.model';
import { ChartOfAccountDialogComponent } from './chart-of-account-dialog.component';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';

interface AccountTreeNode {
  account: ChartOfAccount;
  children: AccountTreeNode[];
}

@Component({
  selector: 'app-chart-of-accounts',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatDialogModule,
    MatTooltipModule,
    MatSelectModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatTreeModule
  ],
  template: `
    <div class="page-container">
      <mat-card>
        <mat-card-header>
          <mat-card-title>
            <div class="header-row">
              <span>Chart of Accounts</span>
              <div class="header-actions">
                <mat-slide-toggle
                  [checked]="viewMode() === 'tree'"
                  (change)="toggleViewMode()"
                  matTooltip="Toggle tree/table view">
                  Tree View
                </mat-slide-toggle>
                <button mat-raised-button color="primary" (click)="openDialog()">
                  <mat-icon>add</mat-icon>
                  Add Account
                </button>
              </div>
            </div>
          </mat-card-title>
        </mat-card-header>

        <mat-card-content>
          <!-- Search & Filters -->
          <div class="filters-row">
            <mat-form-field appearance="outline" class="search-field">
              <mat-label>Search Accounts</mat-label>
              <input matInput
                     [(ngModel)]="searchTerm"
                     (ngModelChange)="onSearch()"
                     placeholder="Search by code, name...">
              <mat-icon matSuffix>search</mat-icon>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Account Type</mat-label>
              <mat-select [(ngModel)]="selectedType" (ngModelChange)="onSearch()">
                <mat-option value="">All Types</mat-option>
                @for (type of accountTypes; track type) {
                  <mat-option [value]="type">{{ type }}</mat-option>
                }
              </mat-select>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Status</mat-label>
              <mat-select [(ngModel)]="selectedStatus" (ngModelChange)="onSearch()">
                <mat-option value="">All</mat-option>
                <mat-option value="active">Active</mat-option>
                <mat-option value="inactive">Inactive</mat-option>
              </mat-select>
            </mat-form-field>
          </div>

          @if (loading()) {
            <div class="loading-container">
              <mat-spinner diameter="40"></mat-spinner>
            </div>
          }

          <!-- Tree View -->
          @if (viewMode() === 'tree' && !loading()) {
            <mat-tree [dataSource]="treeDataSource" [treeControl]="treeControl" class="account-tree">
              <mat-nested-tree-node *matTreeNodeDef="let node">
                <div class="mat-tree-node">
                  <button mat-icon-button disabled></button>
                  <div class="tree-node-content" (click)="openDialog(node.account)">
                    <span class="account-code">{{ node.account.accountCode }}</span>
                    <span class="account-name">{{ node.account.accountName }}</span>
                    <mat-chip [class]="'type-chip ' + node.account.accountType.toLowerCase()">
                      {{ node.account.accountType }}
                    </mat-chip>
                    <span class="account-balance">{{ node.account.currentBalance | currency }}</span>
                  </div>
                  <button mat-icon-button [matMenuTriggerFor]="nodeMenu">
                    <mat-icon>more_vert</mat-icon>
                  </button>
                  <mat-menu #nodeMenu="matMenu">
                    <button mat-menu-item (click)="openDialog(node.account)">
                      <mat-icon>edit</mat-icon> Edit
                    </button>
                    <button mat-menu-item (click)="openDialog(undefined, node.account.id)">
                      <mat-icon>add</mat-icon> Add Sub-Account
                    </button>
                    <button mat-menu-item (click)="deleteAccount(node.account)" class="delete-item">
                      <mat-icon>delete</mat-icon> Delete
                    </button>
                  </mat-menu>
                </div>
                @if (node.children.length > 0) {
                  <div class="tree-children">
                    <ng-container matTreeNodeOutlet></ng-container>
                  </div>
                }
              </mat-nested-tree-node>

              <mat-nested-tree-node *matTreeNodeDef="let node; when: hasChild">
                <div class="mat-tree-node">
                  <button mat-icon-button matTreeNodeToggle>
                    <mat-icon>
                      {{ treeControl.isExpanded(node) ? 'expand_more' : 'chevron_right' }}
                    </mat-icon>
                  </button>
                  <div class="tree-node-content" (click)="openDialog(node.account)">
                    <span class="account-code">{{ node.account.accountCode }}</span>
                    <span class="account-name">{{ node.account.accountName }}</span>
                    <mat-chip [class]="'type-chip ' + node.account.accountType.toLowerCase()">
                      {{ node.account.accountType }}
                    </mat-chip>
                    <span class="account-balance">{{ node.account.currentBalance | currency }}</span>
                  </div>
                  <button mat-icon-button [matMenuTriggerFor]="nodeMenu">
                    <mat-icon>more_vert</mat-icon>
                  </button>
                  <mat-menu #nodeMenu="matMenu">
                    <button mat-menu-item (click)="openDialog(node.account)">
                      <mat-icon>edit</mat-icon> Edit
                    </button>
                    <button mat-menu-item (click)="openDialog(undefined, node.account.id)">
                      <mat-icon>add</mat-icon> Add Sub-Account
                    </button>
                    <button mat-menu-item (click)="deleteAccount(node.account)" class="delete-item">
                      <mat-icon>delete</mat-icon> Delete
                    </button>
                  </mat-menu>
                </div>
                @if (treeControl.isExpanded(node)) {
                  <div class="tree-children">
                    <ng-container matTreeNodeOutlet></ng-container>
                  </div>
                }
              </mat-nested-tree-node>
            </mat-tree>
          }

          <!-- Table View -->
          @if (viewMode() === 'table' && !loading()) {
            <div class="table-container">
              <table mat-table [dataSource]="dataSource" matSort (matSortChange)="onSort($event)">
                <ng-container matColumnDef="accountCode">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Code</th>
                  <td mat-cell *matCellDef="let account">{{ account.accountCode }}</td>
                </ng-container>

                <ng-container matColumnDef="accountName">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Name</th>
                  <td mat-cell *matCellDef="let account">
                    <span [style.padding-left.px]="(account.level || 0) * 20">
                      {{ account.accountName }}
                    </span>
                  </td>
                </ng-container>

                <ng-container matColumnDef="accountType">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Type</th>
                  <td mat-cell *matCellDef="let account">
                    <mat-chip [class]="'type-chip ' + account.accountType.toLowerCase()">
                      {{ account.accountType }}
                    </mat-chip>
                  </td>
                </ng-container>

                <ng-container matColumnDef="accountSubType">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Sub Type</th>
                  <td mat-cell *matCellDef="let account">{{ account.accountSubType || '-' }}</td>
                </ng-container>

                <ng-container matColumnDef="currentBalance">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header class="text-right">Balance</th>
                  <td mat-cell *matCellDef="let account" class="text-right">
                    {{ account.currentBalance | currency }}
                  </td>
                </ng-container>

                <ng-container matColumnDef="isActive">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Status</th>
                  <td mat-cell *matCellDef="let account">
                    <mat-chip [class]="account.isActive ? 'status-active' : 'status-inactive'">
                      {{ account.isActive ? 'Active' : 'Inactive' }}
                    </mat-chip>
                  </td>
                </ng-container>

                <ng-container matColumnDef="actions">
                  <th mat-header-cell *matHeaderCellDef>Actions</th>
                  <td mat-cell *matCellDef="let account">
                    <button mat-icon-button [matMenuTriggerFor]="menu">
                      <mat-icon>more_vert</mat-icon>
                    </button>
                    <mat-menu #menu="matMenu">
                      <button mat-menu-item (click)="openDialog(account)">
                        <mat-icon>edit</mat-icon> Edit
                      </button>
                      <button mat-menu-item (click)="openDialog(undefined, account.id)">
                        <mat-icon>add</mat-icon> Add Sub-Account
                      </button>
                      <button mat-menu-item (click)="viewTransactions(account)">
                        <mat-icon>receipt_long</mat-icon> View Transactions
                      </button>
                      <button mat-menu-item (click)="deleteAccount(account)" class="delete-item">
                        <mat-icon>delete</mat-icon> Delete
                      </button>
                    </mat-menu>
                  </td>
                </ng-container>

                <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
                <tr mat-row *matRowDef="let row; columns: displayedColumns;"
                    (click)="openDialog(row)"
                    class="clickable-row"></tr>
              </table>

              <mat-paginator
                [length]="totalRecords()"
                [pageSize]="pageSize"
                [pageSizeOptions]="[10, 25, 50, 100]"
                (page)="onPageChange($event)"
                showFirstLastButtons>
              </mat-paginator>
            </div>
          }

          @if (!loading() && accounts().length === 0) {
            <div class="empty-state">
              <mat-icon>account_balance</mat-icon>
              <h3>No accounts found</h3>
              <p>Get started by creating your first chart of account</p>
              <button mat-raised-button color="primary" (click)="openDialog()">
                <mat-icon>add</mat-icon>
                Create Account
              </button>
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

    .header-row {
      display: flex;
      justify-content: space-between;
      align-items: center;
      width: 100%;
    }

    .header-actions {
      display: flex;
      gap: 16px;
      align-items: center;
    }

    .filters-row {
      display: flex;
      gap: 16px;
      margin-bottom: 16px;
      flex-wrap: wrap;
    }

    .search-field {
      flex: 1;
      min-width: 250px;
    }

    .table-container {
      overflow-x: auto;
    }

    table {
      width: 100%;
    }

    .text-right {
      text-align: right;
    }

    .clickable-row {
      cursor: pointer;
      transition: background-color 0.2s;
    }

    .clickable-row:hover {
      background-color: rgba(0, 0, 0, 0.04);
    }

    .type-chip {
      font-size: 12px;
    }

    .type-chip.asset {
      background-color: #e3f2fd;
      color: #1976d2;
    }

    .type-chip.liability {
      background-color: #fce4ec;
      color: #c2185b;
    }

    .type-chip.equity {
      background-color: #e8f5e9;
      color: #388e3c;
    }

    .type-chip.revenue {
      background-color: #fff3e0;
      color: #f57c00;
    }

    .type-chip.expense {
      background-color: #f3e5f5;
      color: #7b1fa2;
    }

    .status-active {
      background-color: #e8f5e9;
      color: #388e3c;
    }

    .status-inactive {
      background-color: #fafafa;
      color: #9e9e9e;
    }

    .loading-container {
      display: flex;
      justify-content: center;
      padding: 48px;
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

    .empty-state h3 {
      margin: 0 0 8px;
    }

    .empty-state p {
      margin: 0 0 24px;
    }

    .delete-item {
      color: #f44336;
    }

    /* Tree styles */
    .account-tree {
      margin-top: 16px;
    }

    .mat-tree-node {
      display: flex;
      align-items: center;
      padding: 8px 0;
      border-bottom: 1px solid #eee;
    }

    .tree-node-content {
      flex: 1;
      display: flex;
      align-items: center;
      gap: 16px;
      cursor: pointer;
      padding: 8px;
      border-radius: 4px;
      transition: background-color 0.2s;
    }

    .tree-node-content:hover {
      background-color: rgba(0, 0, 0, 0.04);
    }

    .account-code {
      font-family: monospace;
      font-weight: 500;
      color: #666;
      min-width: 80px;
    }

    .account-name {
      flex: 1;
    }

    .account-balance {
      font-weight: 500;
      min-width: 120px;
      text-align: right;
    }

    .tree-children {
      padding-left: 32px;
    }
  `]
})
export class ChartOfAccountsComponent implements OnInit {
  private accountingService = inject(AccountingService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  accounts = signal<ChartOfAccount[]>([]);
  loading = signal(false);
  totalRecords = signal(0);
  viewMode = signal<'table' | 'tree'>('table');

  dataSource = new MatTableDataSource<ChartOfAccount>();
  treeDataSource = new MatTreeNestedDataSource<AccountTreeNode>();
  treeControl = new NestedTreeControl<AccountTreeNode>(node => node.children);

  displayedColumns = ['accountCode', 'accountName', 'accountType', 'accountSubType', 'currentBalance', 'isActive', 'actions'];
  accountTypes = Object.values(AccountType);

  searchTerm = '';
  selectedType = '';
  selectedStatus = '';
  pageSize = 25;
  pageNumber = 1;
  sortBy = 'accountCode';
  sortDescending = false;

  hasChild = (_: number, node: AccountTreeNode) => node.children && node.children.length > 0;

  ngOnInit(): void {
    this.loadAccounts();
  }

  loadAccounts(): void {
    this.loading.set(true);
    const request: AccountingSearchRequest = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      sortDescending: this.sortDescending,
      status: this.selectedStatus
    };

    this.accountingService.getChartOfAccounts(request).subscribe({
      next: (result) => {
        this.accounts.set(result.items);
        this.totalRecords.set(result.totalCount);
        this.dataSource.data = result.items;
        this.buildTree(result.items);
        this.loading.set(false);
      },
      error: (error) => {
        console.error('Error loading accounts:', error);
        this.snackBar.open('Failed to load accounts', 'Close', { duration: 3000 });
        this.loading.set(false);
      }
    });
  }

  buildTree(accounts: ChartOfAccount[]): void {
    const nodeMap = new Map<string, AccountTreeNode>();
    const rootNodes: AccountTreeNode[] = [];

    // Create nodes for all accounts
    accounts.forEach(account => {
      nodeMap.set(account.id, { account, children: [] });
    });

    // Build hierarchy
    accounts.forEach(account => {
      const node = nodeMap.get(account.id)!;
      if (account.parentAccountId && nodeMap.has(account.parentAccountId)) {
        nodeMap.get(account.parentAccountId)!.children.push(node);
      } else {
        rootNodes.push(node);
      }
    });

    // Sort by account code
    const sortNodes = (nodes: AccountTreeNode[]) => {
      nodes.sort((a, b) => a.account.accountCode.localeCompare(b.account.accountCode));
      nodes.forEach(node => sortNodes(node.children));
    };
    sortNodes(rootNodes);

    this.treeDataSource.data = rootNodes;
  }

  onSearch(): void {
    this.pageNumber = 1;
    this.loadAccounts();
  }

  onSort(sort: Sort): void {
    this.sortBy = sort.active;
    this.sortDescending = sort.direction === 'desc';
    this.loadAccounts();
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadAccounts();
  }

  toggleViewMode(): void {
    this.viewMode.set(this.viewMode() === 'table' ? 'tree' : 'table');
  }

  openDialog(account?: ChartOfAccount, parentAccountId?: string): void {
    const dialogRef = this.dialog.open(ChartOfAccountDialogComponent, {
      width: '600px',
      data: { account, parentAccountId }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadAccounts();
      }
    });
  }

  viewTransactions(account: ChartOfAccount): void {
    // Navigate to general ledger filtered by this account
    // TODO: Implement navigation
  }

  deleteAccount(account: ChartOfAccount): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Delete Account',
        message: `Are you sure you want to delete account "${account.accountCode} - ${account.accountName}"? This action cannot be undone.`,
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.accountingService.deleteChartOfAccount(account.id).subscribe({
          next: () => {
            this.snackBar.open('Account deleted successfully', 'Close', { duration: 3000 });
            this.loadAccounts();
          },
          error: (error) => {
            console.error('Error deleting account:', error);
            this.snackBar.open('Failed to delete account', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }
}

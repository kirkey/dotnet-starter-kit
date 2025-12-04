import { Component, OnInit, inject, signal, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
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
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTabsModule } from '@angular/material/tabs';

import { AccountingService } from '@core/services/accounting.service';
import {
  JournalEntry,
  JournalEntryStatus,
  AccountingSearchRequest,
  AccountingPeriod
} from '@core/models/accounting.model';
import { JournalEntryDialogComponent } from './journal-entry-dialog.component';
import { ApproveRejectDialogComponent } from './approve-reject-dialog.component';
import { ReverseJournalEntryDialogComponent } from './reverse-journal-entry-dialog.component';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-journal-entries',
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
    MatDatepickerModule,
    MatNativeDateModule,
    MatSnackBarModule,
    MatTabsModule
  ],
  template: `
    <div class="page-container">
      <mat-card>
        <mat-card-header>
          <mat-card-title>
            <div class="header-row">
              <span>Journal Entries</span>
              <div class="header-actions">
                <button mat-raised-button color="primary" (click)="openDialog()">
                  <mat-icon>add</mat-icon>
                  New Journal Entry
                </button>
              </div>
            </div>
          </mat-card-title>
        </mat-card-header>

        <mat-card-content>
          <!-- Tab Navigation for Status Filter -->
          <mat-tab-group [(selectedIndex)]="selectedTabIndex" (selectedTabChange)="onTabChange()">
            <mat-tab label="All"></mat-tab>
            <mat-tab label="Draft"></mat-tab>
            <mat-tab label="Pending Approval"></mat-tab>
            <mat-tab label="Approved"></mat-tab>
            <mat-tab label="Posted"></mat-tab>
            <mat-tab label="Rejected"></mat-tab>
            <mat-tab label="Reversed"></mat-tab>
          </mat-tab-group>

          <!-- Search & Filters -->
          <div class="filters-row">
            <mat-form-field appearance="outline" class="search-field">
              <mat-label>Search</mat-label>
              <input matInput
                     [(ngModel)]="searchTerm"
                     (ngModelChange)="onSearch()"
                     placeholder="Search by entry number, description...">
              <mat-icon matSuffix>search</mat-icon>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Period</mat-label>
              <mat-select [(ngModel)]="selectedPeriodId" (ngModelChange)="onSearch()">
                <mat-option value="">All Periods</mat-option>
                @for (period of periods(); track period.id) {
                  <mat-option [value]="period.id">{{ period.periodName }}</mat-option>
                }
              </mat-select>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Start Date</mat-label>
              <input matInput [matDatepicker]="startPicker"
                     [(ngModel)]="startDate" (ngModelChange)="onSearch()">
              <mat-datepicker-toggle matIconSuffix [for]="startPicker"></mat-datepicker-toggle>
              <mat-datepicker #startPicker></mat-datepicker>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>End Date</mat-label>
              <input matInput [matDatepicker]="endPicker"
                     [(ngModel)]="endDate" (ngModelChange)="onSearch()">
              <mat-datepicker-toggle matIconSuffix [for]="endPicker"></mat-datepicker-toggle>
              <mat-datepicker #endPicker></mat-datepicker>
            </mat-form-field>
          </div>

          @if (loading()) {
            <div class="loading-container">
              <mat-spinner diameter="40"></mat-spinner>
            </div>
          }

          @if (!loading()) {
            <div class="table-container">
              <table mat-table [dataSource]="dataSource" matSort (matSortChange)="onSort($event)">
                <ng-container matColumnDef="entryNumber">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Entry #</th>
                  <td mat-cell *matCellDef="let entry">
                    <span class="entry-number">{{ entry.entryNumber }}</span>
                  </td>
                </ng-container>

                <ng-container matColumnDef="entryDate">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Date</th>
                  <td mat-cell *matCellDef="let entry">
                    {{ entry.entryDate | date:'shortDate' }}
                  </td>
                </ng-container>

                <ng-container matColumnDef="description">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Description</th>
                  <td mat-cell *matCellDef="let entry">{{ entry.description }}</td>
                </ng-container>

                <ng-container matColumnDef="totalDebit">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Total Debit</th>
                  <td mat-cell *matCellDef="let entry" class="text-right">
                    {{ entry.totalDebits | currency }}
                  </td>
                </ng-container>

                <ng-container matColumnDef="totalCredit">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Total Credit</th>
                  <td mat-cell *matCellDef="let entry" class="text-right">
                    {{ entry.totalCredits | currency }}
                  </td>
                </ng-container>

                <ng-container matColumnDef="status">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Status</th>
                  <td mat-cell *matCellDef="let entry">
                    <mat-chip [class]="'status-chip ' + entry.status.toLowerCase()">
                      {{ entry.status }}
                    </mat-chip>
                  </td>
                </ng-container>

                <ng-container matColumnDef="createdBy">
                  <th mat-header-cell *matHeaderCellDef>Created By</th>
                  <td mat-cell *matCellDef="let entry">{{ entry.createdByName }}</td>
                </ng-container>

                <ng-container matColumnDef="actions">
                  <th mat-header-cell *matHeaderCellDef>Actions</th>
                  <td mat-cell *matCellDef="let entry">
                    <button mat-icon-button [matMenuTriggerFor]="menu" (click)="$event.stopPropagation()">
                      <mat-icon>more_vert</mat-icon>
                    </button>
                    <mat-menu #menu="matMenu">
                      <button mat-menu-item (click)="viewEntry(entry)">
                        <mat-icon>visibility</mat-icon> View Details
                      </button>
                      @if (entry.status === 'Draft') {
                        <button mat-menu-item (click)="openDialog(entry)">
                          <mat-icon>edit</mat-icon> Edit
                        </button>
                        <button mat-menu-item (click)="submitForApproval(entry)">
                          <mat-icon>send</mat-icon> Submit for Approval
                        </button>
                        <button mat-menu-item (click)="deleteEntry(entry)" class="delete-item">
                          <mat-icon>delete</mat-icon> Delete
                        </button>
                      }
                      @if (entry.status === 'Pending') {
                        <button mat-menu-item (click)="openApproveDialog(entry)">
                          <mat-icon>check_circle</mat-icon> Approve
                        </button>
                        <button mat-menu-item (click)="openRejectDialog(entry)">
                          <mat-icon>cancel</mat-icon> Reject
                        </button>
                      }
                      @if (entry.status === 'Approved') {
                        <button mat-menu-item (click)="postEntry(entry)">
                          <mat-icon>publish</mat-icon> Post
                        </button>
                      }
                      @if (entry.status === 'Posted') {
                        <button mat-menu-item (click)="openReverseDialog(entry)">
                          <mat-icon>undo</mat-icon> Reverse
                        </button>
                      }
                      <button mat-menu-item (click)="copyEntry(entry)">
                        <mat-icon>content_copy</mat-icon> Copy
                      </button>
                      <button mat-menu-item (click)="printEntry(entry)">
                        <mat-icon>print</mat-icon> Print
                      </button>
                    </mat-menu>
                  </td>
                </ng-container>

                <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
                <tr mat-row *matRowDef="let row; columns: displayedColumns;"
                    (click)="viewEntry(row)"
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

          @if (!loading() && entries().length === 0) {
            <div class="empty-state">
              <mat-icon>receipt_long</mat-icon>
              <h3>No journal entries found</h3>
              <p>Create your first journal entry to get started</p>
              <button mat-raised-button color="primary" (click)="openDialog()">
                <mat-icon>add</mat-icon>
                Create Journal Entry
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
    }

    .filters-row {
      display: flex;
      gap: 16px;
      margin: 16px 0;
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

    .entry-number {
      font-family: monospace;
      font-weight: 500;
    }

    .clickable-row {
      cursor: pointer;
      transition: background-color 0.2s;
    }

    .clickable-row:hover {
      background-color: rgba(0, 0, 0, 0.04);
    }

    .status-chip {
      font-size: 12px;
    }

    .status-chip.draft {
      background-color: #e0e0e0;
      color: #616161;
    }

    .status-chip.pendingapproval {
      background-color: #fff3e0;
      color: #f57c00;
    }

    .status-chip.approved {
      background-color: #e3f2fd;
      color: #1976d2;
    }

    .status-chip.posted {
      background-color: #e8f5e9;
      color: #388e3c;
    }

    .status-chip.rejected {
      background-color: #ffebee;
      color: #c62828;
    }

    .status-chip.reversed {
      background-color: #f3e5f5;
      color: #7b1fa2;
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
  `]
})
export class JournalEntriesComponent implements OnInit {
  private accountingService = inject(AccountingService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);
  private router = inject(Router);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  entries = signal<JournalEntry[]>([]);
  periods = signal<AccountingPeriod[]>([]);
  loading = signal(false);
  totalRecords = signal(0);

  dataSource = new MatTableDataSource<JournalEntry>();

  displayedColumns = ['entryNumber', 'entryDate', 'description', 'totalDebit', 'totalCredit', 'status', 'createdBy', 'actions'];
  statuses = Object.values(JournalEntryStatus);

  selectedTabIndex = 0;
  searchTerm = '';
  selectedPeriodId = '';
  startDate: Date | null = null;
  endDate: Date | null = null;
  pageSize = 25;
  pageNumber = 1;
  sortBy = 'entryDate';
  sortDescending = true;

  private statusMap: Record<number, string> = {
    0: '',
    1: JournalEntryStatus.Draft,
    2: JournalEntryStatus.Pending,
    3: JournalEntryStatus.Approved,
    4: JournalEntryStatus.Posted,
    5: JournalEntryStatus.Rejected,
    6: JournalEntryStatus.Reversed
  };

  ngOnInit(): void {
    this.loadPeriods();
    this.loadEntries();
  }

  loadPeriods(): void {
    this.accountingService.getOpenPeriods().subscribe({
      next: (periods) => {
        this.periods.set(periods);
      }
    });
  }

  loadEntries(): void {
    this.loading.set(true);
    const request: AccountingSearchRequest = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      sortDescending: this.sortDescending,
      periodId: this.selectedPeriodId || undefined,
      startDate: this.startDate || undefined,
      endDate: this.endDate || undefined,
      status: this.statusMap[this.selectedTabIndex] || undefined
    };

    this.accountingService.getJournalEntries(request).subscribe({
      next: (result) => {
        this.entries.set(result.items);
        this.totalRecords.set(result.totalCount);
        this.dataSource.data = result.items;
        this.loading.set(false);
      },
      error: (error) => {
        console.error('Error loading journal entries:', error);
        this.snackBar.open('Failed to load journal entries', 'Close', { duration: 3000 });
        this.loading.set(false);
      }
    });
  }

  onSearch(): void {
    this.pageNumber = 1;
    this.loadEntries();
  }

  onTabChange(): void {
    this.pageNumber = 1;
    this.loadEntries();
  }

  onSort(sort: Sort): void {
    this.sortBy = sort.active;
    this.sortDescending = sort.direction === 'desc';
    this.loadEntries();
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadEntries();
  }

  openDialog(entry?: JournalEntry): void {
    const dialogRef = this.dialog.open(JournalEntryDialogComponent, {
      width: '900px',
      maxHeight: '90vh',
      data: { entry }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadEntries();
      }
    });
  }

  viewEntry(entry: JournalEntry): void {
    this.openDialog(entry);
  }

  submitForApproval(entry: JournalEntry): void {
    this.accountingService.submitJournalEntryForApproval(entry.id).subscribe({
      next: () => {
        this.snackBar.open('Journal entry submitted for approval', 'Close', { duration: 3000 });
        this.loadEntries();
      },
      error: (error) => {
        console.error('Error submitting entry:', error);
        this.snackBar.open('Failed to submit entry for approval', 'Close', { duration: 3000 });
      }
    });
  }

  openApproveDialog(entry: JournalEntry): void {
    const dialogRef = this.dialog.open(ApproveRejectDialogComponent, {
      width: '500px',
      data: { entry, action: 'approve' }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadEntries();
      }
    });
  }

  openRejectDialog(entry: JournalEntry): void {
    const dialogRef = this.dialog.open(ApproveRejectDialogComponent, {
      width: '500px',
      data: { entry, action: 'reject' }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadEntries();
      }
    });
  }

  postEntry(entry: JournalEntry): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Post Journal Entry',
        message: `Are you sure you want to post entry "${entry.entryNumber}"? This will update the general ledger.`,
        confirmText: 'Post',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.accountingService.postJournalEntry(entry.id).subscribe({
          next: () => {
            this.snackBar.open('Journal entry posted successfully', 'Close', { duration: 3000 });
            this.loadEntries();
          },
          error: (error) => {
            console.error('Error posting entry:', error);
            this.snackBar.open('Failed to post journal entry', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }

  openReverseDialog(entry: JournalEntry): void {
    const dialogRef = this.dialog.open(ReverseJournalEntryDialogComponent, {
      width: '500px',
      data: { entry }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadEntries();
      }
    });
  }

  copyEntry(entry: JournalEntry): void {
    // Open dialog with entry data but without ID (to create new)
    const copiedEntry: Partial<JournalEntry> = { ...entry, id: '', entryNumber: '', status: JournalEntryStatus.Draft };
    this.openDialog(copiedEntry as JournalEntry);
  }

  printEntry(entry: JournalEntry): void {
    // TODO: Implement print functionality
    window.print();
  }

  deleteEntry(entry: JournalEntry): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Delete Journal Entry',
        message: `Are you sure you want to delete entry "${entry.entryNumber}"? This action cannot be undone.`,
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.accountingService.deleteJournalEntry(entry.id).subscribe({
          next: () => {
            this.snackBar.open('Journal entry deleted successfully', 'Close', { duration: 3000 });
            this.loadEntries();
          },
          error: (error) => {
            console.error('Error deleting entry:', error);
            this.snackBar.open('Failed to delete journal entry', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }
}

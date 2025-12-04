import { Component, OnInit, inject, signal, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSortModule, MatSort, Sort } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';

import { AccountingService } from '@core/services/accounting.service';
import { FixedAsset, AccountingSearchRequest } from '@core/models/accounting.model';
import { FixedAssetDialogComponent } from './fixed-asset-dialog.component';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-fixed-assets',
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
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatDialogModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatTooltipModule
  ],
  template: `
    <div class="page-container">
      <mat-card>
        <mat-card-header>
          <mat-card-title>
            <div class="header-row">
              <span>Fixed Assets</span>
              <button mat-raised-button color="primary" (click)="openDialog()">
                <mat-icon>add</mat-icon>
                Add Asset
              </button>
            </div>
          </mat-card-title>
        </mat-card-header>

        <mat-card-content>
          <!-- Filters -->
          <div class="filters-row">
            <mat-form-field appearance="outline" class="search-field">
              <mat-label>Search Assets</mat-label>
              <input matInput [(ngModel)]="searchTerm" (ngModelChange)="onSearch()"
                     placeholder="Search by name, tag, serial number...">
              <mat-icon matSuffix>search</mat-icon>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Category</mat-label>
              <mat-select [(ngModel)]="selectedCategory" (ngModelChange)="onSearch()">
                <mat-option value="">All Categories</mat-option>
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
              <mat-label>Status</mat-label>
              <mat-select [(ngModel)]="selectedStatus" (ngModelChange)="onSearch()">
                <mat-option value="">All</mat-option>
                <mat-option value="Active">Active</mat-option>
                <mat-option value="Disposed">Disposed</mat-option>
                <mat-option value="FullyDepreciated">Fully Depreciated</mat-option>
              </mat-select>
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
                <ng-container matColumnDef="assetCode">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Asset Code</th>
                  <td mat-cell *matCellDef="let asset">
                    <span class="asset-tag">{{ asset.assetCode }}</span>
                  </td>
                </ng-container>

                <ng-container matColumnDef="assetName">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Name</th>
                  <td mat-cell *matCellDef="let asset">{{ asset.assetName }}</td>
                </ng-container>

                <ng-container matColumnDef="assetCategory">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Category</th>
                  <td mat-cell *matCellDef="let asset">{{ asset.assetCategory }}</td>
                </ng-container>

                <ng-container matColumnDef="purchaseDate">
                  <th mat-header-cell *matHeaderCellDef mat-sort-header>Acquired</th>
                  <td mat-cell *matCellDef="let asset">{{ asset.purchaseDate | date:'shortDate' }}</td>
                </ng-container>

                <ng-container matColumnDef="purchasePrice">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Cost</th>
                  <td mat-cell *matCellDef="let asset" class="text-right">
                    {{ asset.purchasePrice | currency }}
                  </td>
                </ng-container>

                <ng-container matColumnDef="accumulatedDepreciation">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Acc. Depreciation</th>
                  <td mat-cell *matCellDef="let asset" class="text-right">
                    {{ asset.accumulatedDepreciation | currency }}
                  </td>
                </ng-container>

                <ng-container matColumnDef="currentBookValue">
                  <th mat-header-cell *matHeaderCellDef class="text-right">Book Value</th>
                  <td mat-cell *matCellDef="let asset" class="text-right">
                    <span class="book-value">
                      {{ asset.currentBookValue | currency }}
                    </span>
                  </td>
                </ng-container>

                <ng-container matColumnDef="status">
                  <th mat-header-cell *matHeaderCellDef>Status</th>
                  <td mat-cell *matCellDef="let asset">
                    <mat-chip [class]="getStatusClass(getAssetStatus(asset))">
                      {{ getAssetStatus(asset) }}
                    </mat-chip>
                  </td>
                </ng-container>

                <ng-container matColumnDef="actions">
                  <th mat-header-cell *matHeaderCellDef>Actions</th>
                  <td mat-cell *matCellDef="let asset">
                    <button mat-icon-button [matMenuTriggerFor]="menu" (click)="$event.stopPropagation()">
                      <mat-icon>more_vert</mat-icon>
                    </button>
                    <mat-menu #menu="matMenu">
                      <button mat-menu-item (click)="openDialog(asset)">
                        <mat-icon>edit</mat-icon> Edit
                      </button>
                      <button mat-menu-item (click)="viewHistory(asset)">
                        <mat-icon>history</mat-icon> Depreciation History
                      </button>
                      @if (!asset.isDisposed && !asset.isFullyDepreciated) {
                        <button mat-menu-item (click)="recordDepreciation(asset)">
                          <mat-icon>trending_down</mat-icon> Record Depreciation
                        </button>
                        <button mat-menu-item (click)="disposeAsset(asset)">
                          <mat-icon>sell</mat-icon> Dispose/Sell
                        </button>
                      }
                      <button mat-menu-item (click)="viewMaintenanceLog(asset)">
                        <mat-icon>build</mat-icon> Maintenance Log
                      </button>
                      <button mat-menu-item (click)="deleteAsset(asset)" class="delete-item">
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

          @if (!loading() && fixedAssets().length === 0) {
            <div class="empty-state">
              <mat-icon>precision_manufacturing</mat-icon>
              <h3>No fixed assets found</h3>
              <p>Add your company's fixed assets to track depreciation</p>
              <button mat-raised-button color="primary" (click)="openDialog()">
                <mat-icon>add</mat-icon>
                Add Asset
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

    .filters-row {
      display: flex;
      gap: 16px;
      margin-bottom: 16px;
      flex-wrap: wrap;
    }

    .search-field {
      flex: 1;
      min-width: 200px;
    }

    .table-container {
      overflow-x: auto;
    }

    table {
      width: 100%;
    }

    .asset-tag {
      font-family: monospace;
      font-weight: 500;
      color: #1976d2;
    }

    .text-right {
      text-align: right;
    }

    .book-value {
      font-weight: 500;
      color: #388e3c;
    }

    .clickable-row {
      cursor: pointer;
      transition: background-color 0.2s;
    }

    .clickable-row:hover {
      background-color: rgba(0, 0, 0, 0.04);
    }

    .status-active { background-color: #e8f5e9; color: #388e3c; }
    .status-disposed { background-color: #fafafa; color: #9e9e9e; }
    .status-fullydepreciated { background-color: #fff3e0; color: #f57c00; }

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

    .delete-item {
      color: #f44336;
    }
  `]
})
export class FixedAssetsComponent implements OnInit {
  private accountingService = inject(AccountingService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  fixedAssets = signal<FixedAsset[]>([]);
  loading = signal(false);
  totalRecords = signal(0);

  dataSource = new MatTableDataSource<FixedAsset>();

  displayedColumns = ['assetCode', 'assetName', 'assetCategory', 'purchaseDate', 'purchasePrice', 'accumulatedDepreciation', 'currentBookValue', 'status', 'actions'];

  searchTerm = '';
  selectedCategory = '';
  selectedStatus = '';
  pageSize = 25;
  pageNumber = 1;
  sortBy = 'name';
  sortDescending = false;

  ngOnInit(): void {
    this.loadFixedAssets();
  }

  loadFixedAssets(): void {
    this.loading.set(true);
    const request: AccountingSearchRequest = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      sortDescending: this.sortDescending
    };

    this.accountingService.getFixedAssets(request).subscribe({
      next: (result) => {
        this.fixedAssets.set(result.items);
        this.totalRecords.set(result.totalCount);
        this.dataSource.data = result.items;
        this.loading.set(false);
      },
      error: (error) => {
        console.error('Error loading fixed assets:', error);
        this.snackBar.open('Failed to load fixed assets', 'Close', { duration: 3000 });
        this.loading.set(false);
      }
    });
  }

  onSearch(): void {
    this.pageNumber = 1;
    this.loadFixedAssets();
  }

  onSort(sort: Sort): void {
    this.sortBy = sort.active;
    this.sortDescending = sort.direction === 'desc';
    this.loadFixedAssets();
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadFixedAssets();
  }

  getStatusClass(status: string): string {
    return `status-${status.toLowerCase().replace(/\s/g, '')}`;
  }

  getAssetStatus(asset: FixedAsset): string {
    if (asset.isDisposed) return 'Disposed';
    if (asset.isFullyDepreciated) return 'Fully Depreciated';
    if (asset.isActive) return 'Active';
    return 'Inactive';
  }

  openDialog(asset?: FixedAsset): void {
    const dialogRef = this.dialog.open(FixedAssetDialogComponent, {
      width: '700px',
      maxHeight: '90vh',
      data: { asset }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadFixedAssets();
      }
    });
  }

  viewHistory(asset: FixedAsset): void {
    // TODO: Open depreciation history dialog
  }

  recordDepreciation(asset: FixedAsset): void {
    // TODO: Open record depreciation dialog
  }

  disposeAsset(asset: FixedAsset): void {
    // TODO: Open dispose asset dialog
  }

  viewMaintenanceLog(asset: FixedAsset): void {
    // TODO: Open maintenance log
  }

  deleteAsset(asset: FixedAsset): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Delete Asset',
        message: `Are you sure you want to delete "${asset.assetName}"? This action cannot be undone.`,
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.accountingService.deleteFixedAsset(asset.id).subscribe({
          next: () => {
            this.snackBar.open('Asset deleted successfully', 'Close', { duration: 3000 });
            this.loadFixedAssets();
          },
          error: (error) => {
            console.error('Error deleting asset:', error);
            this.snackBar.open('Failed to delete asset', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }
}

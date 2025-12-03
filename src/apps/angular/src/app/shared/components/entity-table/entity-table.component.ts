import { Component, Input, Output, EventEmitter, signal, computed, ContentChild, TemplateRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';

export interface TableColumn {
  key: string;
  label: string;
  sortable?: boolean;
  type?: 'text' | 'date' | 'boolean' | 'currency' | 'actions';
  format?: string;
}

@Component({
  selector: 'app-entity-table',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatInputModule,
    MatFormFieldModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatProgressSpinnerModule,
    MatTooltipModule,
    MatDialogModule
  ],
  template: `
    <div class="table-container">
      <!-- Toolbar -->
      <div class="table-toolbar">
        <mat-form-field appearance="outline" class="search-field">
          <mat-label>Search</mat-label>
          <input matInput 
                 [(ngModel)]="searchText" 
                 (keyup.enter)="onSearch()"
                 placeholder="Search...">
          <button mat-icon-button matSuffix (click)="onSearch()">
            <mat-icon>search</mat-icon>
          </button>
        </mat-form-field>
        
        <div class="toolbar-actions">
          <button mat-icon-button (click)="onRefresh()" matTooltip="Refresh">
            <mat-icon>refresh</mat-icon>
          </button>
          @if (showCreate) {
            <button mat-raised-button color="primary" (click)="onCreate()">
              <mat-icon>add</mat-icon>
              Add New
            </button>
          }
        </div>
      </div>

      <!-- Loading Spinner -->
      @if (loading()) {
        <div class="loading-overlay">
          <mat-spinner diameter="40"></mat-spinner>
        </div>
      }

      <!-- Table -->
      <div class="table-wrapper">
        <table mat-table [dataSource]="data()" matSort (matSortChange)="onSort($event)">
          @for (column of columns; track column.key) {
            @if (column.type !== 'actions') {
              <ng-container [matColumnDef]="column.key">
                <th mat-header-cell *matHeaderCellDef [mat-sort-header]="column.sortable ? column.key : ''">
                  {{ column.label }}
                </th>
                <td mat-cell *matCellDef="let row">
                  @switch (column.type) {
                    @case ('date') {
                      {{ row[column.key] | date:column.format || 'mediumDate' }}
                    }
                    @case ('boolean') {
                      <mat-icon [color]="row[column.key] ? 'primary' : 'warn'">
                        {{ row[column.key] ? 'check_circle' : 'cancel' }}
                      </mat-icon>
                    }
                    @case ('currency') {
                      {{ row[column.key] | currency }}
                    }
                    @default {
                      {{ row[column.key] }}
                    }
                  }
                </td>
              </ng-container>
            } @else {
              <ng-container matColumnDef="actions">
                <th mat-header-cell *matHeaderCellDef>Actions</th>
                <td mat-cell *matCellDef="let row">
                  <button mat-icon-button [matMenuTriggerFor]="actionsMenu">
                    <mat-icon>more_vert</mat-icon>
                  </button>
                  <mat-menu #actionsMenu="matMenu">
                    @if (showEdit) {
                      <button mat-menu-item (click)="onEdit(row)">
                        <mat-icon>edit</mat-icon>
                        <span>Edit</span>
                      </button>
                    }
                    @if (showDelete) {
                      <button mat-menu-item (click)="onDelete(row)">
                        <mat-icon color="warn">delete</mat-icon>
                        <span>Delete</span>
                      </button>
                    }
                    <ng-container *ngTemplateOutlet="extraActionsTemplate; context: { $implicit: row }"></ng-container>
                  </mat-menu>
                </td>
              </ng-container>
            }
          }

          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>

          <!-- No Data Row -->
          <tr class="mat-row no-data-row" *matNoDataRow>
            <td class="mat-cell" [attr.colspan]="displayedColumns.length">
              <div class="no-data">
                <mat-icon>inbox</mat-icon>
                <span>No data available</span>
              </div>
            </td>
          </tr>
        </table>
      </div>

      <!-- Paginator -->
      <mat-paginator
        [length]="totalCount()"
        [pageSize]="pageSize()"
        [pageSizeOptions]="[10, 25, 50, 100]"
        [pageIndex]="pageIndex()"
        (page)="onPageChange($event)"
        showFirstLastButtons>
      </mat-paginator>
    </div>
  `,
  styles: [`
    .table-container {
      position: relative;
      background: var(--surface-color);
      border-radius: 8px;
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    }

    .table-toolbar {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 16px;
      gap: 16px;
    }

    .search-field {
      flex: 1;
      max-width: 400px;
    }

    .toolbar-actions {
      display: flex;
      gap: 8px;
      align-items: center;
    }

    .table-wrapper {
      overflow-x: auto;
    }

    table {
      width: 100%;
    }

    .loading-overlay {
      position: absolute;
      top: 0;
      left: 0;
      right: 0;
      bottom: 0;
      background: rgba(255, 255, 255, 0.8);
      display: flex;
      align-items: center;
      justify-content: center;
      z-index: 10;
    }

    .no-data {
      display: flex;
      flex-direction: column;
      align-items: center;
      padding: 40px;
      color: var(--text-secondary);
    }

    .no-data mat-icon {
      font-size: 48px;
      width: 48px;
      height: 48px;
      margin-bottom: 16px;
    }

    :host-context(.dark-theme) .loading-overlay {
      background: rgba(48, 48, 48, 0.8);
    }
  `]
})
export class EntityTableComponent<T extends { id: string | number }> {
  @Input() columns: TableColumn[] = [];
  @Input() showCreate = true;
  @Input() showEdit = true;
  @Input() showDelete = true;
  @Input() entityName = 'Item';

  @Output() search = new EventEmitter<string>();
  @Output() refresh = new EventEmitter<void>();
  @Output() create = new EventEmitter<void>();
  @Output() edit = new EventEmitter<T>();
  @Output() delete = new EventEmitter<T>();
  @Output() pageChange = new EventEmitter<PageEvent>();
  @Output() sortChange = new EventEmitter<Sort>();

  @ContentChild('extraActions') extraActionsTemplate?: TemplateRef<unknown>;

  // Signals
  data = signal<T[]>([]);
  loading = signal(false);
  totalCount = signal(0);
  pageSize = signal(10);
  pageIndex = signal(0);

  searchText = '';

  private dialog = new MatDialog(null as never, null as never, null as never, null as never, null as never, null as never, null as never, null as never);

  get displayedColumns(): string[] {
    return this.columns.map(c => c.key);
  }

  setData(items: T[]): void {
    this.data.set(items);
  }

  setLoading(loading: boolean): void {
    this.loading.set(loading);
  }

  setTotalCount(count: number): void {
    this.totalCount.set(count);
  }

  setPagination(pageIndex: number, pageSize: number): void {
    this.pageIndex.set(pageIndex);
    this.pageSize.set(pageSize);
  }

  onSearch(): void {
    this.search.emit(this.searchText);
  }

  onRefresh(): void {
    this.refresh.emit();
  }

  onCreate(): void {
    this.create.emit();
  }

  onEdit(row: T): void {
    this.edit.emit(row);
  }

  onDelete(row: T): void {
    this.delete.emit(row);
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex.set(event.pageIndex);
    this.pageSize.set(event.pageSize);
    this.pageChange.emit(event);
  }

  onSort(sort: Sort): void {
    this.sortChange.emit(sort);
  }
}

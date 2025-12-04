import { Component, inject, signal, OnInit, ViewChild, TemplateRef, computed } from '@angular/core';
import { CommonModule, DatePipe, DecimalPipe } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { NotificationService } from '@core/services/notification.service';
import { PostingBatch, ApprovalStatus } from '@core/models/accounting.model';

@Component({
  selector: 'app-posting-batches',
  standalone: true,
  imports: [
    CommonModule, DatePipe, DecimalPipe, ReactiveFormsModule,
    MatTableModule, MatPaginatorModule, MatSortModule, MatButtonModule, MatIconModule,
    MatFormFieldModule, MatInputModule, MatSelectModule, MatDialogModule,
    MatProgressSpinnerModule, MatChipsModule, MatMenuModule, MatTooltipModule,
    MatCardModule, MatDatepickerModule, MatNativeDateModule, MatProgressBarModule, PageHeaderComponent
  ],
  template: `
    <div class="batches-container">
      <app-page-header 
        title="Posting Batches" 
        subtitle="Manage batch posting of journal entries"
        icon="batch_prediction">
      </app-page-header>

      <div class="toolbar">
        <div class="filters">
          <mat-form-field appearance="outline" class="search-field">
            <mat-label>Search batches</mat-label>
            <input matInput [value]="searchQuery()" (input)="onSearch($event)" placeholder="Search...">
            <mat-icon matPrefix>search</mat-icon>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Status</mat-label>
            <mat-select [value]="selectedStatus()" (selectionChange)="onStatusChange($event.value)">
              <mat-option value="">All</mat-option>
              <mat-option value="Open">Open</mat-option>
              <mat-option value="Pending">Pending</mat-option>
              <mat-option value="Posted">Posted</mat-option>
              <mat-option value="Error">Error</mat-option>
            </mat-select>
          </mat-form-field>
        </div>

        <button mat-raised-button color="primary" (click)="openBatchDialog()">
          <mat-icon>add</mat-icon>
          New Batch
        </button>
      </div>

      <div class="summary-cards">
        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon open">
              <mat-icon>folder_open</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Open Batches</span>
              <span class="summary-value">{{ openCount() }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon pending">
              <mat-icon>hourglass_empty</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Pending Posting</span>
              <span class="summary-value">{{ pendingCount() }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon posted">
              <mat-icon>check_circle</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Posted Today</span>
              <span class="summary-value">{{ postedTodayCount() }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card error" *ngIf="errorCount() > 0">
          <mat-card-content>
            <div class="summary-icon error">
              <mat-icon>error</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Errors</span>
              <span class="summary-value">{{ errorCount() }}</span>
            </div>
          </mat-card-content>
        </mat-card>
      </div>

      @if (isLoading()) {
        <div class="loading-container">
          <mat-spinner diameter="48"></mat-spinner>
        </div>
      } @else {
        <div class="table-container">
          <table mat-table [dataSource]="filteredBatches()" matSort>
            <ng-container matColumnDef="batchNumber">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Batch #</th>
              <td mat-cell *matCellDef="let batch">
                <span class="batch-number">{{ batch.batchNumber }}</span>
              </td>
            </ng-container>

            <ng-container matColumnDef="description">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Description</th>
              <td mat-cell *matCellDef="let batch">{{ batch.description }}</td>
            </ng-container>

            <ng-container matColumnDef="source">
              <th mat-header-cell *matHeaderCellDef>Source</th>
              <td mat-cell *matCellDef="let batch">{{ batch.source }}</td>
            </ng-container>

            <ng-container matColumnDef="entryCount">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Entries</th>
              <td mat-cell *matCellDef="let batch">{{ batch.entryCount | number }}</td>
            </ng-container>

            <ng-container matColumnDef="createdDate">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Created</th>
              <td mat-cell *matCellDef="let batch">{{ batch.createdDate | date:'medium' }}</td>
            </ng-container>

            <ng-container matColumnDef="postedDate">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Posted</th>
              <td mat-cell *matCellDef="let batch">
                @if (batch.postedDate) {
                  {{ batch.postedDate | date:'medium' }}
                } @else {
                  <span class="not-posted">-</span>
                }
              </td>
            </ng-container>

            <ng-container matColumnDef="status">
              <th mat-header-cell *matHeaderCellDef>Status</th>
              <td mat-cell *matCellDef="let batch">
                <mat-chip [class]="'status-' + batch.status.toLowerCase()">{{ batch.status }}</mat-chip>
              </td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef></th>
              <td mat-cell *matCellDef="let batch">
                <button mat-icon-button [matMenuTriggerFor]="menu">
                  <mat-icon>more_vert</mat-icon>
                </button>
                <mat-menu #menu="matMenu">
                  <button mat-menu-item (click)="viewBatch(batch)">
                    <mat-icon>visibility</mat-icon>
                    <span>View Entries</span>
                  </button>
                  @if (batch.status === 'Open') {
                    <button mat-menu-item (click)="openBatchDialog(batch)">
                      <mat-icon>edit</mat-icon>
                      <span>Edit</span>
                    </button>
                    <button mat-menu-item (click)="validateBatch(batch)">
                      <mat-icon>check</mat-icon>
                      <span>Validate</span>
                    </button>
                    <button mat-menu-item (click)="postBatch(batch)">
                      <mat-icon>publish</mat-icon>
                      <span>Post Batch</span>
                    </button>
                  }
                  @if (batch.status === 'Pending') {
                    <button mat-menu-item (click)="postBatch(batch)">
                      <mat-icon>publish</mat-icon>
                      <span>Post Batch</span>
                    </button>
                    <button mat-menu-item (click)="reopenBatch(batch)">
                      <mat-icon>lock_open</mat-icon>
                      <span>Reopen</span>
                    </button>
                  }
                  @if (batch.status === 'Error') {
                    <button mat-menu-item (click)="viewErrors(batch)">
                      <mat-icon>error_outline</mat-icon>
                      <span>View Errors</span>
                    </button>
                    <button mat-menu-item (click)="reopenBatch(batch)">
                      <mat-icon>lock_open</mat-icon>
                      <span>Reopen</span>
                    </button>
                  }
                  @if (batch.status === 'Open') {
                    <button mat-menu-item (click)="deleteBatch(batch)" class="delete-action">
                      <mat-icon color="warn">delete</mat-icon>
                      <span>Delete</span>
                    </button>
                  }
                </mat-menu>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns;" [class.error-row]="row.status === 'Error'"></tr>
          </table>

          <mat-paginator [length]="totalBatches()" [pageSize]="pageSize()" [pageSizeOptions]="[10, 25, 50]" showFirstLastButtons></mat-paginator>
        </div>
      }
    </div>

    <ng-template #batchDialog>
      <h2 mat-dialog-title>{{ editingBatch() ? 'Edit Batch' : 'New Posting Batch' }}</h2>
      <mat-dialog-content>
        <form [formGroup]="batchForm" class="batch-form">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Description</mat-label>
            <input matInput formControlName="description" required>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Source</mat-label>
            <mat-select formControlName="source" required>
              <mat-option value="Manual">Manual Entry</mat-option>
              <mat-option value="Import">Import</mat-option>
              <mat-option value="Recurring">Recurring Entries</mat-option>
              <mat-option value="System">System Generated</mat-option>
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Notes</mat-label>
            <textarea matInput formControlName="notes" rows="3"></textarea>
          </mat-form-field>
        </form>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button mat-dialog-close>Cancel</button>
        <button mat-raised-button color="primary" [disabled]="batchForm.invalid || isSaving()" (click)="saveBatch()">
          @if (isSaving()) { <mat-spinner diameter="20"></mat-spinner> }
          Save
        </button>
      </mat-dialog-actions>
    </ng-template>
  `,
  styles: [`
    .batches-container { padding: 24px; }
    .toolbar { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 24px; gap: 16px; flex-wrap: wrap; }
    .filters { display: flex; gap: 12px; flex-wrap: wrap; flex: 1; }
    .search-field { min-width: 250px; flex: 1; }
    .filter-field { min-width: 150px; }
    .summary-cards { display: grid; grid-template-columns: repeat(auto-fit, minmax(180px, 1fr)); gap: 16px; margin-bottom: 24px; }
    .summary-card mat-card-content { display: flex; align-items: center; gap: 16px; padding: 16px !important; }
    .summary-card.error { border-left: 4px solid #dc2626; }
    .summary-icon { width: 48px; height: 48px; border-radius: 12px; display: flex; align-items: center; justify-content: center; }
    .summary-icon.open { background: #dbeafe; color: #2563eb; }
    .summary-icon.pending { background: #fef3c7; color: #d97706; }
    .summary-icon.posted { background: #dcfce7; color: #16a34a; }
    .summary-icon.error { background: #fee2e2; color: #dc2626; }
    .summary-info { display: flex; flex-direction: column; }
    .summary-label { font-size: 12px; color: var(--text-secondary); }
    .summary-value { font-size: 20px; font-weight: 600; }
    .loading-container { display: flex; justify-content: center; padding: 48px; }
    .table-container { background: var(--surface-color); border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1); }
    table { width: 100%; }
    .batch-number { font-weight: 500; color: var(--primary-color); font-family: monospace; }
    .not-posted { color: #9ca3af; }
    .status-open { background: #dbeafe !important; color: #1d4ed8 !important; }
    .status-pending { background: #fef3c7 !important; color: #92400e !important; }
    .status-posted { background: #dcfce7 !important; color: #166534 !important; }
    .status-error { background: #fee2e2 !important; color: #991b1b !important; }
    tr.error-row { background: #fef2f2; }
    .batch-form { display: flex; flex-direction: column; gap: 16px; min-width: 400px; }
    .full-width { width: 100%; }
    .delete-action { color: #dc2626; }
  `]
})
export class PostingBatchesComponent implements OnInit {
  @ViewChild('batchDialog') batchDialogTemplate!: TemplateRef<any>;

  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private notification = inject(NotificationService);

  batches = signal<PostingBatch[]>([]);
  isLoading = signal(false);
  isSaving = signal(false);
  searchQuery = signal('');
  selectedStatus = signal('');
  editingBatch = signal<PostingBatch | null>(null);

  pageSize = signal(10);
  totalBatches = signal(0);

  displayedColumns = ['batchNumber', 'description', 'source', 'entryCount', 'createdDate', 'postedDate', 'status', 'actions'];

  batchForm: FormGroup = this.fb.group({
    description: ['', Validators.required],
    source: ['Manual', Validators.required],
    notes: ['']
  });

  filteredBatches = computed(() => {
    let result = this.batches();
    const query = this.searchQuery().toLowerCase();
    if (query) result = result.filter(b => b.batchNumber.toLowerCase().includes(query) || b.batchName.toLowerCase().includes(query));
    if (this.selectedStatus()) result = result.filter(b => b.status === this.selectedStatus());
    return result;
  });

  openCount = computed(() => this.batches().filter(b => b.status === ApprovalStatus.Draft).length);
  pendingCount = computed(() => this.batches().filter(b => b.status === ApprovalStatus.Pending).length);
  postedTodayCount = computed(() => {
    const today = new Date().toDateString();
    return this.batches().filter(b => b.isPosted && b.postedOn && new Date(b.postedOn).toDateString() === today).length;
  });
  errorCount = computed(() => this.batches().filter(b => b.status === ApprovalStatus.Rejected).length);

  ngOnInit(): void { this.loadData(); }

  async loadData(): Promise<void> {
    this.isLoading.set(true);
    try {
      const mockBatches: PostingBatch[] = [
        { id: '1', batchNumber: 'BATCH-001', batchName: 'January Journal Entries', batchDate: new Date('2024-01-28'), source: 'Manual', entryCount: 45, totalDebits: 45000, totalCredits: 45000, periodId: '1', periodName: 'January 2024', postedOn: new Date('2024-01-31'), postedBy: 'admin', status: ApprovalStatus.Approved, isPosted: true, isActive: true, entries: [] },
        { id: '2', batchNumber: 'BATCH-002', batchName: 'Recurring Entries - February', batchDate: new Date('2024-02-01'), source: 'Recurring', entryCount: 12, totalDebits: 12000, totalCredits: 12000, periodId: '2', periodName: 'February 2024', status: ApprovalStatus.Pending, isPosted: false, isActive: true, entries: [] },
        { id: '3', batchNumber: 'BATCH-003', batchName: 'AP Import Batch', batchDate: new Date('2024-02-05'), source: 'Import', entryCount: 156, totalDebits: 156000, totalCredits: 156000, periodId: '2', periodName: 'February 2024', status: ApprovalStatus.Draft, isPosted: false, isActive: true, entries: [] },
        { id: '4', batchNumber: 'BATCH-004', batchName: 'Payroll Entries', batchDate: new Date('2024-02-03'), source: 'System', entryCount: 8, totalDebits: 8000, totalCredits: 8000, periodId: '2', periodName: 'February 2024', status: ApprovalStatus.Rejected, isPosted: false, isActive: true, entries: [] }
      ];
      await new Promise(r => setTimeout(r, 300));
      this.batches.set(mockBatches);
      this.totalBatches.set(mockBatches.length);
    } finally { this.isLoading.set(false); }
  }

  onSearch(event: Event): void { this.searchQuery.set((event.target as HTMLInputElement).value); }
  onStatusChange(value: string): void { this.selectedStatus.set(value); }

  openBatchDialog(batch?: PostingBatch): void {
    this.editingBatch.set(batch || null);
    if (batch) this.batchForm.patchValue(batch);
    else this.batchForm.reset({ source: 'Manual' });
    this.dialog.open(this.batchDialogTemplate, { width: '450px' });
  }

  async saveBatch(): Promise<void> {
    if (this.batchForm.invalid) return;
    this.isSaving.set(true);
    try {
      await new Promise(r => setTimeout(r, 500));
      this.notification.success('Batch saved');
      this.dialog.closeAll();
      this.loadData();
    } finally { this.isSaving.set(false); }
  }

  viewBatch(batch: PostingBatch): void {}
  viewErrors(batch: PostingBatch): void { this.notification.error('Batch has errors'); }
  async validateBatch(batch: PostingBatch): Promise<void> { this.notification.success('Batch validated successfully'); }
  async postBatch(batch: PostingBatch): Promise<void> { this.notification.success('Batch posted'); this.loadData(); }
  async reopenBatch(batch: PostingBatch): Promise<void> { this.notification.success('Batch reopened'); this.loadData(); }
  async deleteBatch(batch: PostingBatch): Promise<void> { this.notification.success('Batch deleted'); this.loadData(); }
}

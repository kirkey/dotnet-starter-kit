import { Component, inject, signal, OnInit, ViewChild, TemplateRef, computed } from '@angular/core';
import { CommonModule, DecimalPipe } from '@angular/common';
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
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { NotificationService } from '@core/services/notification.service';
import { DepreciationMethod, DepreciationMethodConfig } from '@core/models/accounting.model';

@Component({
  selector: 'app-depreciation-methods',
  standalone: true,
  imports: [
    CommonModule, DecimalPipe, ReactiveFormsModule,
    MatTableModule, MatPaginatorModule, MatSortModule, MatButtonModule, MatIconModule,
    MatFormFieldModule, MatInputModule, MatSelectModule, MatDialogModule,
    MatProgressSpinnerModule, MatChipsModule, MatMenuModule, MatTooltipModule,
    MatCardModule, MatSlideToggleModule, PageHeaderComponent
  ],
  template: `
    <div class="methods-container">
      <app-page-header 
        title="Depreciation Methods" 
        subtitle="Configure depreciation calculation methods for fixed assets"
        icon="calculate">
      </app-page-header>

      <div class="toolbar">
        <div class="filters">
          <mat-form-field appearance="outline" class="search-field">
            <mat-label>Search methods</mat-label>
            <input matInput [value]="searchQuery()" (input)="onSearch($event)" placeholder="Search...">
            <mat-icon matPrefix>search</mat-icon>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Type</mat-label>
            <mat-select [value]="selectedType()" (selectionChange)="onTypeChange($event.value)">
              <mat-option value="">All Types</mat-option>
              <mat-option value="StraightLine">Straight Line</mat-option>
              <mat-option value="DecliningBalance">Declining Balance</mat-option>
              <mat-option value="DoubleDeclining">Double Declining</mat-option>
              <mat-option value="SumOfYears">Sum of Years Digits</mat-option>
              <mat-option value="UnitsOfProduction">Units of Production</mat-option>
            </mat-select>
          </mat-form-field>
        </div>

        <button mat-raised-button color="primary" (click)="openMethodDialog()">
          <mat-icon>add</mat-icon>
          New Method
        </button>
      </div>

      <div class="info-cards">
        <mat-card class="info-card">
          <mat-card-content>
            <mat-icon>info</mat-icon>
            <div class="info-text">
              <strong>Depreciation Methods</strong>
              <p>Define how fixed assets are depreciated over their useful life. Each method uses a different calculation formula to allocate the cost of an asset.</p>
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
          <table mat-table [dataSource]="filteredMethods()" matSort>
            <ng-container matColumnDef="code">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Code</th>
              <td mat-cell *matCellDef="let method">
                <span class="method-code">{{ method.methodCode }}</span>
              </td>
            </ng-container>

            <ng-container matColumnDef="name">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Name</th>
              <td mat-cell *matCellDef="let method">{{ method.methodName }}</td>
            </ng-container>

            <ng-container matColumnDef="type">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Type</th>
              <td mat-cell *matCellDef="let method">
                <mat-chip [class]="'type-' + method.depreciationMethod.toLowerCase()">{{ method.depreciationMethod }}</mat-chip>
              </td>
            </ng-container>

            <ng-container matColumnDef="rate">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Rate/Factor</th>
              <td mat-cell *matCellDef="let method">
                @if (method.ratePercentage) {
                  {{ method.ratePercentage | number:'1.2-2' }}%
                } @else {
                  -
                }
              </td>
            </ng-container>

            <ng-container matColumnDef="description">
              <th mat-header-cell *matHeaderCellDef>Description</th>
              <td mat-cell *matCellDef="let method">{{ method.description }}</td>
            </ng-container>

            <ng-container matColumnDef="status">
              <th mat-header-cell *matHeaderCellDef>Status</th>
              <td mat-cell *matCellDef="let method">
                @if (method.isActive) {
                  <mat-chip class="status-active">Active</mat-chip>
                } @else {
                  <mat-chip class="status-inactive">Inactive</mat-chip>
                }
              </td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef></th>
              <td mat-cell *matCellDef="let method">
                <button mat-icon-button [matMenuTriggerFor]="menu">
                  <mat-icon>more_vert</mat-icon>
                </button>
                <mat-menu #menu="matMenu">
                  <button mat-menu-item (click)="viewMethod(method)">
                    <mat-icon>visibility</mat-icon>
                    <span>View</span>
                  </button>
                  <button mat-menu-item (click)="openMethodDialog(method)">
                    <mat-icon>edit</mat-icon>
                    <span>Edit</span>
                  </button>
                  @if (method.isActive) {
                    <button mat-menu-item (click)="deactivateMethod(method)">
                      <mat-icon>block</mat-icon>
                      <span>Deactivate</span>
                    </button>
                  } @else {
                    <button mat-menu-item (click)="activateMethod(method)">
                      <mat-icon>check_circle</mat-icon>
                      <span>Activate</span>
                    </button>
                  }
                </mat-menu>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns;" [class.inactive]="!row.isActive"></tr>
          </table>

          <mat-paginator [length]="totalMethods()" [pageSize]="pageSize()" [pageSizeOptions]="[10, 25, 50]" showFirstLastButtons></mat-paginator>
        </div>
      }
    </div>

    <ng-template #methodDialog>
      <h2 mat-dialog-title>{{ editingMethod() ? 'Edit Method' : 'New Depreciation Method' }}</h2>
      <mat-dialog-content>
        <form [formGroup]="methodForm" class="method-form">
          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Code</mat-label>
              <input matInput formControlName="code" required>
            </mat-form-field>

            <mat-form-field appearance="outline" class="flex-2">
              <mat-label>Name</mat-label>
              <input matInput formControlName="name" required>
            </mat-form-field>
          </div>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Depreciation Type</mat-label>
            <mat-select formControlName="depreciationType" required>
              <mat-option value="StraightLine">Straight Line</mat-option>
              <mat-option value="DecliningBalance">Declining Balance</mat-option>
              <mat-option value="DoubleDeclining">Double Declining Balance</mat-option>
              <mat-option value="SumOfYears">Sum of Years' Digits</mat-option>
              <mat-option value="UnitsOfProduction">Units of Production</mat-option>
            </mat-select>
          </mat-form-field>

          @if (showRateField()) {
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Annual Rate (%)</mat-label>
              <input matInput type="number" formControlName="rate">
              <span matSuffix>%</span>
            </mat-form-field>
          }

          @if (showFactorField()) {
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Declining Factor</mat-label>
              <input matInput type="number" formControlName="factor" step="0.1">
              <span matSuffix>x</span>
            </mat-form-field>
          }

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Description</mat-label>
            <textarea matInput formControlName="description" rows="3"></textarea>
          </mat-form-field>

          <mat-slide-toggle formControlName="isActive">Active</mat-slide-toggle>
        </form>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button mat-dialog-close>Cancel</button>
        <button mat-raised-button color="primary" [disabled]="methodForm.invalid || isSaving()" (click)="saveMethod()">
          @if (isSaving()) { <mat-spinner diameter="20"></mat-spinner> }
          Save
        </button>
      </mat-dialog-actions>
    </ng-template>
  `,
  styles: [`
    .methods-container { padding: 24px; }
    .toolbar { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 24px; gap: 16px; flex-wrap: wrap; }
    .filters { display: flex; gap: 12px; flex-wrap: wrap; flex: 1; }
    .search-field { min-width: 250px; flex: 1; }
    .filter-field { min-width: 180px; }
    .info-cards { margin-bottom: 24px; }
    .info-card mat-card-content { display: flex; gap: 16px; align-items: flex-start; padding: 16px !important; }
    .info-card mat-icon { color: #3b82f6; font-size: 24px; width: 24px; height: 24px; }
    .info-text p { margin: 4px 0 0; color: var(--text-secondary); font-size: 14px; }
    .loading-container { display: flex; justify-content: center; padding: 48px; }
    .table-container { background: var(--surface-color); border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1); }
    table { width: 100%; }
    .method-code { font-weight: 500; color: var(--primary-color); font-family: monospace; }
    .type-straightline { background: #dcfce7 !important; color: #166534 !important; }
    .type-decliningbalance { background: #dbeafe !important; color: #1d4ed8 !important; }
    .type-doubledeclining { background: #e0e7ff !important; color: #4338ca !important; }
    .type-sumofyears { background: #fef3c7 !important; color: #92400e !important; }
    .type-unitsofproduction { background: #fce7f3 !important; color: #be185d !important; }
    .status-active { background: #dcfce7 !important; color: #166534 !important; }
    .status-inactive { background: #f3f4f6 !important; color: #374151 !important; }
    tr.inactive { opacity: 0.6; }
    .method-form { display: flex; flex-direction: column; gap: 16px; min-width: 450px; }
    .form-row { display: grid; grid-template-columns: 1fr 2fr; gap: 16px; }
    .flex-2 { flex: 2; }
    .full-width { width: 100%; }
  `]
})
export class DepreciationMethodsComponent implements OnInit {
  @ViewChild('methodDialog') methodDialogTemplate!: TemplateRef<any>;

  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private notification = inject(NotificationService);

  methods = signal<DepreciationMethodConfig[]>([]);
  isLoading = signal(false);
  isSaving = signal(false);
  searchQuery = signal('');
  selectedType = signal('');
  editingMethod = signal<DepreciationMethodConfig | null>(null);

  pageSize = signal(10);
  totalMethods = signal(0);

  displayedColumns = ['code', 'name', 'type', 'rate', 'description', 'status', 'actions'];

  methodForm: FormGroup = this.fb.group({
    code: ['', Validators.required],
    name: ['', Validators.required],
    depreciationType: ['StraightLine', Validators.required],
    rate: [null],
    factor: [null],
    description: [''],
    isActive: [true]
  });

  filteredMethods = computed(() => {
    let result = this.methods();
    const query = this.searchQuery().toLowerCase();
    if (query) result = result.filter(m => m.methodCode.toLowerCase().includes(query) || m.methodName.toLowerCase().includes(query));
    if (this.selectedType()) result = result.filter(m => m.depreciationMethod === this.selectedType());
    return result;
  });

  showRateField = computed(() => {
    const type = this.methodForm.get('depreciationType')?.value;
    return type === 'DecliningBalance';
  });

  showFactorField = computed(() => {
    const type = this.methodForm.get('depreciationType')?.value;
    return type === 'DoubleDeclining' || type === 'DecliningBalance';
  });

  ngOnInit(): void { this.loadData(); }

  async loadData(): Promise<void> {
    this.isLoading.set(true);
    try {
      const mockMethods: DepreciationMethodConfig[] = [
        { id: '1', methodCode: 'SL', methodName: 'Straight Line', depreciationMethod: DepreciationMethod.StraightLine, description: 'Equal depreciation each period over the asset life', isActive: true, isDefault: true },
        { id: '2', methodCode: 'DB-20', methodName: 'Declining Balance 20%', depreciationMethod: DepreciationMethod.DecliningBalance, ratePercentage: 20, description: 'Fixed percentage of remaining book value', isActive: true, isDefault: false },
        { id: '3', methodCode: 'DDB', methodName: 'Double Declining Balance', depreciationMethod: DepreciationMethod.DoubleDecliningBalance, description: 'Accelerated method using 2x straight line rate', isActive: true, isDefault: false },
        { id: '4', methodCode: 'SYD', methodName: 'Sum of Years Digits', depreciationMethod: DepreciationMethod.SumOfYearsDigits, description: 'Accelerated method based on remaining life', isActive: true, isDefault: false },
        { id: '5', methodCode: 'UOP', methodName: 'Units of Production', depreciationMethod: DepreciationMethod.UnitsOfProduction, description: 'Based on actual usage or production output', isActive: false, isDefault: false }
      ];
      await new Promise(r => setTimeout(r, 300));
      this.methods.set(mockMethods);
      this.totalMethods.set(mockMethods.length);
    } finally { this.isLoading.set(false); }
  }

  onSearch(event: Event): void { this.searchQuery.set((event.target as HTMLInputElement).value); }
  onTypeChange(value: string): void { this.selectedType.set(value); }

  openMethodDialog(method?: DepreciationMethodConfig): void {
    this.editingMethod.set(method || null);
    if (method) this.methodForm.patchValue(method);
    else this.methodForm.reset({ depreciationType: 'StraightLine', isActive: true });
    this.dialog.open(this.methodDialogTemplate, { width: '500px' });
  }

  async saveMethod(): Promise<void> {
    if (this.methodForm.invalid) return;
    this.isSaving.set(true);
    try {
      await new Promise(r => setTimeout(r, 500));
      this.notification.success('Depreciation method saved');
      this.dialog.closeAll();
      this.loadData();
    } finally { this.isSaving.set(false); }
  }

  viewMethod(method: DepreciationMethodConfig): void {}
  async activateMethod(method: DepreciationMethodConfig): Promise<void> { this.notification.success('Method activated'); this.loadData(); }
  async deactivateMethod(method: DepreciationMethodConfig): Promise<void> { this.notification.success('Method deactivated'); this.loadData(); }
}

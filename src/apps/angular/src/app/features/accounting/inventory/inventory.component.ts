import { Component, inject, signal, OnInit, ViewChild, TemplateRef, computed } from '@angular/core';
import { CommonModule, CurrencyPipe, DecimalPipe } from '@angular/common';
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
import { MatTabsModule } from '@angular/material/tabs';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { NotificationService } from '@core/services/notification.service';
import { InventoryItem } from '@core/models/accounting.model';

// Local interface for category dropdown
interface InventoryCategory {
  id: string;
  name: string;
  description?: string;
  isActive: boolean;
}

@Component({
  selector: 'app-inventory',
  standalone: true,
  imports: [
    CommonModule, CurrencyPipe, DecimalPipe, ReactiveFormsModule,
    MatTableModule, MatPaginatorModule, MatSortModule, MatButtonModule, MatIconModule,
    MatFormFieldModule, MatInputModule, MatSelectModule, MatDialogModule,
    MatProgressSpinnerModule, MatChipsModule, MatMenuModule, MatTooltipModule,
    MatCardModule, MatSlideToggleModule, MatTabsModule, PageHeaderComponent
  ],
  template: `
    <div class="inventory-container">
      <app-page-header 
        title="Inventory" 
        subtitle="Manage inventory items, quantities, and valuations"
        icon="inventory_2">
      </app-page-header>

      <div class="toolbar">
        <div class="filters">
          <mat-form-field appearance="outline" class="search-field">
            <mat-label>Search inventory</mat-label>
            <input matInput [value]="searchQuery()" (input)="onSearch($event)" placeholder="Search...">
            <mat-icon matPrefix>search</mat-icon>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Category</mat-label>
            <mat-select [value]="selectedCategory()" (selectionChange)="onCategoryChange($event.value)">
              <mat-option value="">All Categories</mat-option>
              @for (category of categories(); track category.id) {
                <mat-option [value]="category.id">{{ category.name }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Stock Level</mat-label>
            <mat-select [value]="selectedStockLevel()" (selectionChange)="onStockLevelChange($event.value)">
              <mat-option value="">All</mat-option>
              <mat-option value="low">Low Stock</mat-option>
              <mat-option value="out">Out of Stock</mat-option>
              <mat-option value="overstock">Overstock</mat-option>
            </mat-select>
          </mat-form-field>
        </div>

        <div class="toolbar-actions">
          <button mat-stroked-button (click)="exportInventory()">
            <mat-icon>download</mat-icon>
            Export
          </button>
          <button mat-raised-button color="primary" (click)="openItemDialog()">
            <mat-icon>add</mat-icon>
            New Item
          </button>
        </div>
      </div>

      <div class="summary-cards">
        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon total">
              <mat-icon>inventory_2</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Total Items</span>
              <span class="summary-value">{{ totalItems() }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon value">
              <mat-icon>account_balance</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Total Value</span>
              <span class="summary-value">{{ totalValue() | currency }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card warning">
          <mat-card-content>
            <div class="summary-icon low-stock">
              <mat-icon>warning</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Low Stock Items</span>
              <span class="summary-value">{{ lowStockCount() }}</span>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-icon out-stock">
              <mat-icon>remove_shopping_cart</mat-icon>
            </div>
            <div class="summary-info">
              <span class="summary-label">Out of Stock</span>
              <span class="summary-value">{{ outOfStockCount() }}</span>
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
          <table mat-table [dataSource]="filteredItems()" matSort>
            <ng-container matColumnDef="sku">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>SKU</th>
              <td mat-cell *matCellDef="let item">
                <span class="item-sku">{{ item.sku }}</span>
              </td>
            </ng-container>

            <ng-container matColumnDef="itemName">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Name</th>
              <td mat-cell *matCellDef="let item">
                <div class="item-name">{{ item.itemName }}</div>
                <div class="item-category">{{ item.category }}</div>
              </td>
            </ng-container>

            <ng-container matColumnDef="quantity">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Qty On Hand</th>
              <td mat-cell *matCellDef="let item">
                <div class="quantity-cell" [class.low-stock]="item.quantityOnHand <= item.reorderPoint && item.quantityOnHand > 0" [class.out-of-stock]="item.quantityOnHand === 0">
                  {{ item.quantityOnHand | number }} {{ item.unitOfMeasure }}
                </div>
              </td>
            </ng-container>

            <ng-container matColumnDef="reorderPoint">
              <th mat-header-cell *matHeaderCellDef>Reorder Point</th>
              <td mat-cell *matCellDef="let item">{{ item.reorderPoint | number }}</td>
            </ng-container>

            <ng-container matColumnDef="unitCost">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Unit Cost</th>
              <td mat-cell *matCellDef="let item">{{ item.unitCost | currency }}</td>
            </ng-container>

            <ng-container matColumnDef="totalValue">
              <th mat-header-cell *matHeaderCellDef mat-sort-header>Total Value</th>
              <td mat-cell *matCellDef="let item">{{ item.quantityOnHand * item.unitCost | currency }}</td>
            </ng-container>

            <ng-container matColumnDef="status">
              <th mat-header-cell *matHeaderCellDef>Status</th>
              <td mat-cell *matCellDef="let item">
                @if (item.quantityOnHand === 0) {
                  <mat-chip class="status-out">Out of Stock</mat-chip>
                } @else if (item.quantityOnHand <= item.reorderPoint) {
                  <mat-chip class="status-low">Low Stock</mat-chip>
                } @else {
                  <mat-chip class="status-ok">In Stock</mat-chip>
                }
              </td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef></th>
              <td mat-cell *matCellDef="let item">
                <button mat-icon-button [matMenuTriggerFor]="menu">
                  <mat-icon>more_vert</mat-icon>
                </button>
                <mat-menu #menu="matMenu">
                  <button mat-menu-item (click)="viewItem(item)">
                    <mat-icon>visibility</mat-icon>
                    <span>View Details</span>
                  </button>
                  <button mat-menu-item (click)="openItemDialog(item)">
                    <mat-icon>edit</mat-icon>
                    <span>Edit</span>
                  </button>
                  <button mat-menu-item (click)="adjustQuantity(item)">
                    <mat-icon>tune</mat-icon>
                    <span>Adjust Quantity</span>
                  </button>
                  <button mat-menu-item (click)="viewHistory(item)">
                    <mat-icon>history</mat-icon>
                    <span>View History</span>
                  </button>
                  @if (item.quantityOnHand <= item.reorderPoint) {
                    <button mat-menu-item (click)="createPurchaseOrder(item)">
                      <mat-icon>shopping_cart</mat-icon>
                      <span>Create PO</span>
                    </button>
                  }
                </mat-menu>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
          </table>

          <mat-paginator [length]="totalItemCount()" [pageSize]="pageSize()" [pageSizeOptions]="[10, 25, 50, 100]" showFirstLastButtons></mat-paginator>
        </div>
      }
    </div>

    <ng-template #itemDialog>
      <h2 mat-dialog-title>{{ editingItem() ? 'Edit Item' : 'New Inventory Item' }}</h2>
      <mat-dialog-content>
        <form [formGroup]="itemForm" class="item-form">
          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>SKU</mat-label>
              <input matInput formControlName="sku" required>
            </mat-form-field>

            <mat-form-field appearance="outline" class="flex-2">
              <mat-label>Item Name</mat-label>
              <input matInput formControlName="itemName" required>
            </mat-form-field>
          </div>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Category</mat-label>
            <mat-select formControlName="category" required>
              @for (category of categories(); track category.id) {
                <mat-option [value]="category.name">{{ category.name }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Description</mat-label>
            <textarea matInput formControlName="description" rows="2"></textarea>
          </mat-form-field>

          <div class="form-row three-cols">
            <mat-form-field appearance="outline">
              <mat-label>Unit of Measure</mat-label>
              <mat-select formControlName="unitOfMeasure" required>
                <mat-option value="Each">Each</mat-option>
                <mat-option value="Box">Box</mat-option>
                <mat-option value="Case">Case</mat-option>
                <mat-option value="Kg">Kg</mat-option>
                <mat-option value="Liter">Liter</mat-option>
              </mat-select>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Unit Cost</mat-label>
              <input matInput type="number" formControlName="unitCost" required>
              <span matPrefix>$&nbsp;</span>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Selling Price</mat-label>
              <input matInput type="number" formControlName="sellingPrice">
              <span matPrefix>$&nbsp;</span>
            </mat-form-field>
          </div>

          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Reorder Point</mat-label>
              <input matInput type="number" formControlName="reorderPoint" required>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Reorder Quantity</mat-label>
              <input matInput type="number" formControlName="reorderQuantity">
            </mat-form-field>
          </div>

          @if (!editingItem()) {
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Initial Quantity</mat-label>
              <input matInput type="number" formControlName="quantityOnHand">
            </mat-form-field>
          }

          <div class="form-row">
            <mat-slide-toggle formControlName="isActive">Active</mat-slide-toggle>
            <mat-slide-toggle formControlName="trackInventory">Track Inventory</mat-slide-toggle>
          </div>
        </form>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button mat-dialog-close>Cancel</button>
        <button mat-raised-button color="primary" [disabled]="itemForm.invalid || isSaving()" (click)="saveItem()">
          @if (isSaving()) { <mat-spinner diameter="20"></mat-spinner> }
          Save
        </button>
      </mat-dialog-actions>
    </ng-template>

    <ng-template #adjustDialog>
      <h2 mat-dialog-title>Adjust Quantity - {{ adjustingItem()?.itemName }}</h2>
      <mat-dialog-content>
        <form [formGroup]="adjustForm" class="adjust-form">
          <div class="current-quantity">
            Current Quantity: <strong>{{ adjustingItem()?.quantityOnHand }} {{ adjustingItem()?.unitOfMeasure }}</strong>
          </div>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Adjustment Type</mat-label>
            <mat-select formControlName="adjustmentType" required>
              <mat-option value="add">Add</mat-option>
              <mat-option value="subtract">Subtract</mat-option>
              <mat-option value="set">Set To</mat-option>
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Quantity</mat-label>
            <input matInput type="number" formControlName="quantity" required>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Reason</mat-label>
            <mat-select formControlName="reason" required>
              <mat-option value="Received">Received</mat-option>
              <mat-option value="Sold">Sold</mat-option>
              <mat-option value="Damaged">Damaged</mat-option>
              <mat-option value="Returned">Returned</mat-option>
              <mat-option value="Count">Physical Count</mat-option>
              <mat-option value="Other">Other</mat-option>
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Notes</mat-label>
            <textarea matInput formControlName="notes" rows="2"></textarea>
          </mat-form-field>
        </form>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button mat-dialog-close>Cancel</button>
        <button mat-raised-button color="primary" [disabled]="adjustForm.invalid || isSaving()" (click)="saveAdjustment()">
          @if (isSaving()) { <mat-spinner diameter="20"></mat-spinner> }
          Save Adjustment
        </button>
      </mat-dialog-actions>
    </ng-template>
  `,
  styles: [`
    .inventory-container { padding: 24px; }
    .toolbar { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 24px; gap: 16px; flex-wrap: wrap; }
    .filters { display: flex; gap: 12px; flex-wrap: wrap; flex: 1; }
    .search-field { min-width: 250px; flex: 1; }
    .filter-field { min-width: 150px; }
    .toolbar-actions { display: flex; gap: 8px; }
    .summary-cards { display: grid; grid-template-columns: repeat(auto-fit, minmax(180px, 1fr)); gap: 16px; margin-bottom: 24px; }
    .summary-card mat-card-content { display: flex; align-items: center; gap: 16px; padding: 16px !important; }
    .summary-card.warning { border-left: 4px solid #f59e0b; }
    .summary-icon { width: 48px; height: 48px; border-radius: 12px; display: flex; align-items: center; justify-content: center; }
    .summary-icon.total { background: #dbeafe; color: #2563eb; }
    .summary-icon.value { background: #dcfce7; color: #16a34a; }
    .summary-icon.low-stock { background: #fef3c7; color: #d97706; }
    .summary-icon.out-stock { background: #fee2e2; color: #dc2626; }
    .summary-info { display: flex; flex-direction: column; }
    .summary-label { font-size: 12px; color: var(--text-secondary); }
    .summary-value { font-size: 20px; font-weight: 600; }
    .loading-container { display: flex; justify-content: center; padding: 48px; }
    .table-container { background: var(--surface-color); border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1); }
    table { width: 100%; }
    .item-sku { font-weight: 500; color: var(--primary-color); font-family: monospace; }
    .item-name { font-weight: 500; }
    .item-category { font-size: 12px; color: var(--text-secondary); }
    .quantity-cell { font-weight: 500; }
    .quantity-cell.low-stock { color: #d97706; }
    .quantity-cell.out-of-stock { color: #dc2626; }
    .status-ok { background: #dcfce7 !important; color: #166534 !important; }
    .status-low { background: #fef3c7 !important; color: #92400e !important; }
    .status-out { background: #fee2e2 !important; color: #991b1b !important; }
    .item-form, .adjust-form { display: flex; flex-direction: column; gap: 16px; min-width: 500px; }
    .form-row { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; align-items: center; }
    .form-row.three-cols { grid-template-columns: 1fr 1fr 1fr; }
    .flex-2 { flex: 2; }
    .full-width { width: 100%; }
    .current-quantity { padding: 12px; background: #f3f4f6; border-radius: 8px; margin-bottom: 16px; }
  `]
})
export class InventoryComponent implements OnInit {
  @ViewChild('itemDialog') itemDialogTemplate!: TemplateRef<any>;
  @ViewChild('adjustDialog') adjustDialogTemplate!: TemplateRef<any>;

  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private notification = inject(NotificationService);

  items = signal<InventoryItem[]>([]);
  categories = signal<InventoryCategory[]>([]);
  isLoading = signal(false);
  isSaving = signal(false);
  searchQuery = signal('');
  selectedCategory = signal('');
  selectedStockLevel = signal('');
  editingItem = signal<InventoryItem | null>(null);
  adjustingItem = signal<InventoryItem | null>(null);

  pageSize = signal(25);
  totalItemCount = signal(0);

  displayedColumns = ['sku', 'itemName', 'quantity', 'reorderPoint', 'unitCost', 'totalValue', 'status', 'actions'];

  itemForm: FormGroup = this.fb.group({
    sku: ['', Validators.required],
    itemName: ['', Validators.required],
    category: ['', Validators.required],
    description: [''],
    unitOfMeasure: ['Each', Validators.required],
    unitCost: [0, [Validators.required, Validators.min(0)]],
    sellingPrice: [0],
    reorderPoint: [10, [Validators.required, Validators.min(0)]],
    reorderQuantity: [50],
    quantityOnHand: [0],
    isActive: [true]
  });

  adjustForm: FormGroup = this.fb.group({
    adjustmentType: ['add', Validators.required],
    quantity: [0, [Validators.required, Validators.min(1)]],
    reason: ['', Validators.required],
    notes: ['']
  });

  filteredItems = computed(() => {
    let result = this.items();
    const query = this.searchQuery().toLowerCase();
    if (query) result = result.filter(i => i.sku.toLowerCase().includes(query) || i.itemName.toLowerCase().includes(query));
    if (this.selectedCategory()) result = result.filter(i => i.category === this.selectedCategory());
    if (this.selectedStockLevel() === 'low') result = result.filter(i => i.quantityOnHand <= i.reorderPoint && i.quantityOnHand > 0);
    else if (this.selectedStockLevel() === 'out') result = result.filter(i => i.quantityOnHand === 0);
    return result;
  });

  totalItems = computed(() => this.items().length);
  totalValue = computed(() => this.items().reduce((sum, i) => sum + (i.quantityOnHand * i.unitCost), 0));
  lowStockCount = computed(() => this.items().filter(i => i.quantityOnHand <= i.reorderPoint && i.quantityOnHand > 0).length);
  outOfStockCount = computed(() => this.items().filter(i => i.quantityOnHand === 0).length);

  ngOnInit(): void { this.loadData(); }

  async loadData(): Promise<void> {
    this.isLoading.set(true);
    try {
      const mockItems: InventoryItem[] = [
        { id: '1', sku: 'WIDGET-001', itemName: 'Standard Widget', category: 'Parts', unitOfMeasure: 'Each', quantityOnHand: 150, quantityReserved: 0, quantityAvailable: 150, reorderPoint: 50, reorderQuantity: 200, unitCost: 12.50, averageCost: 12.50, lastCost: 12.50, standardCost: 12.50, sellingPrice: 25.00, inventoryAccountId: '1', cogsAccountId: '2', revenueAccountId: '3', isActive: true },
        { id: '2', sku: 'GADGET-002', itemName: 'Premium Gadget', category: 'Parts', unitOfMeasure: 'Each', quantityOnHand: 25, quantityReserved: 0, quantityAvailable: 25, reorderPoint: 30, reorderQuantity: 100, unitCost: 45.00, averageCost: 45.00, lastCost: 45.00, standardCost: 45.00, sellingPrice: 89.99, inventoryAccountId: '1', cogsAccountId: '2', revenueAccountId: '3', isActive: true },
        { id: '3', sku: 'SUPPLY-003', itemName: 'Office Supplies Kit', category: 'Supplies', unitOfMeasure: 'Box', quantityOnHand: 0, quantityReserved: 0, quantityAvailable: 0, reorderPoint: 10, reorderQuantity: 50, unitCost: 35.00, averageCost: 35.00, lastCost: 35.00, standardCost: 35.00, sellingPrice: 65.00, inventoryAccountId: '1', cogsAccountId: '2', revenueAccountId: '3', isActive: true }
      ];
      const mockCategories: InventoryCategory[] = [
        { id: '1', name: 'Parts', description: 'Component parts', isActive: true },
        { id: '2', name: 'Supplies', description: 'Office supplies', isActive: true },
        { id: '3', name: 'Equipment', description: 'Office equipment', isActive: true }
      ];
      await new Promise(r => setTimeout(r, 300));
      this.items.set(mockItems);
      this.categories.set(mockCategories);
      this.totalItemCount.set(mockItems.length);
    } finally { this.isLoading.set(false); }
  }

  onSearch(event: Event): void { this.searchQuery.set((event.target as HTMLInputElement).value); }
  onCategoryChange(value: string): void { this.selectedCategory.set(value); }
  onStockLevelChange(value: string): void { this.selectedStockLevel.set(value); }

  openItemDialog(item?: InventoryItem): void {
    this.editingItem.set(item || null);
    if (item) this.itemForm.patchValue(item);
    else this.itemForm.reset({ unitOfMeasure: 'Each', unitCost: 0, reorderPoint: 10, reorderQuantity: 50, quantityOnHand: 0, isActive: true, trackInventory: true });
    this.dialog.open(this.itemDialogTemplate, { width: '600px' });
  }

  async saveItem(): Promise<void> {
    if (this.itemForm.invalid) return;
    this.isSaving.set(true);
    try {
      await new Promise(r => setTimeout(r, 500));
      this.notification.success('Item saved');
      this.dialog.closeAll();
      this.loadData();
    } finally { this.isSaving.set(false); }
  }

  adjustQuantity(item: InventoryItem): void {
    this.adjustingItem.set(item);
    this.adjustForm.reset({ adjustmentType: 'add', quantity: 0, reason: '', notes: '' });
    this.dialog.open(this.adjustDialogTemplate, { width: '400px' });
  }

  async saveAdjustment(): Promise<void> {
    if (this.adjustForm.invalid) return;
    this.isSaving.set(true);
    try {
      await new Promise(r => setTimeout(r, 500));
      this.notification.success('Quantity adjusted');
      this.dialog.closeAll();
      this.loadData();
    } finally { this.isSaving.set(false); }
  }

  viewItem(item: InventoryItem): void {}
  viewHistory(item: InventoryItem): void {}
  createPurchaseOrder(item: InventoryItem): void { this.notification.info('Create PO feature coming soon'); }
  exportInventory(): void { this.notification.info('Export feature coming soon'); }
}

import { Component, inject, signal, OnInit, ViewChild, TemplateRef, computed } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSortModule, Sort } from '@angular/material/sort';
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
import { MatDividerModule } from '@angular/material/divider';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';
import { NotificationService } from '@core/services/notification.service';
import { Product, Category, Brand } from '@core/models/product.model';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [
    CommonModule,
    CurrencyPipe,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDialogModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatMenuModule,
    MatTooltipModule,
    MatCardModule,
    MatSlideToggleModule,
    MatDividerModule,
    PageHeaderComponent
  ],
  template: `
    <div class="products-container">
      <app-page-header 
        title="Products" 
        subtitle="Manage your product catalog"
        icon="inventory_2">
      </app-page-header>

      <!-- Toolbar -->
      <div class="toolbar">
        <div class="filters">
          <mat-form-field appearance="outline" class="search-field">
            <mat-label>Search products</mat-label>
            <input matInput 
                   [value]="searchQuery()" 
                   (input)="onSearch($event)"
                   placeholder="Search by name, SKU, or barcode...">
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
            <mat-label>Brand</mat-label>
            <mat-select [value]="selectedBrand()" (selectionChange)="onBrandChange($event.value)">
              <mat-option value="">All Brands</mat-option>
              @for (brand of brands(); track brand.id) {
                <mat-option [value]="brand.id">{{ brand.name }}</mat-option>
              }
            </mat-select>
          </mat-form-field>
        </div>

        <div class="actions">
          <button mat-icon-button [class.active]="viewMode() === 'grid'" (click)="viewMode.set('grid')" matTooltip="Grid View">
            <mat-icon>grid_view</mat-icon>
          </button>
          <button mat-icon-button [class.active]="viewMode() === 'table'" (click)="viewMode.set('table')" matTooltip="Table View">
            <mat-icon>view_list</mat-icon>
          </button>
          <button mat-raised-button color="primary" (click)="openProductDialog()">
            <mat-icon>add</mat-icon>
            Add Product
          </button>
        </div>
      </div>

      <!-- Loading -->
      @if (isLoading()) {
        <div class="loading-container">
          <mat-spinner diameter="48"></mat-spinner>
        </div>
      } @else {
        <!-- Grid View -->
        @if (viewMode() === 'grid') {
          <div class="products-grid">
            @for (product of filteredProducts(); track product.id) {
              <mat-card class="product-card" [class.inactive]="!product.isActive">
                <div class="product-image">
                  @if (product.imageUrl) {
                    <img [src]="product.imageUrl" [alt]="product.name">
                  } @else {
                    <mat-icon>inventory_2</mat-icon>
                  }
                  @if (!product.isActive) {
                    <div class="inactive-badge">Inactive</div>
                  }
                </div>
                <mat-card-content>
                  <h3 class="product-name">{{ product.name }}</h3>
                  <p class="product-category">{{ product.categoryName || 'No Category' }}</p>
                  <div class="product-details">
                    <span class="product-price">{{ product.price | currency }}</span>
                    <span class="product-stock" [class.low-stock]="product.quantity <= (product.reorderLevel || 10)">
                      {{ product.quantity }} in stock
                    </span>
                  </div>
                </mat-card-content>
                <mat-card-actions>
                  <button mat-icon-button (click)="openProductDialog(product)" matTooltip="Edit">
                    <mat-icon>edit</mat-icon>
                  </button>
                  <button mat-icon-button (click)="viewProduct(product)" matTooltip="View">
                    <mat-icon>visibility</mat-icon>
                  </button>
                  <button mat-icon-button (click)="deleteProduct(product)" matTooltip="Delete">
                    <mat-icon>delete</mat-icon>
                  </button>
                </mat-card-actions>
              </mat-card>
            }
          </div>
        }

        <!-- Table View -->
        @if (viewMode() === 'table') {
          <div class="table-container">
            <table mat-table [dataSource]="filteredProducts()" matSort>
              <!-- Image Column -->
              <ng-container matColumnDef="image">
                <th mat-header-cell *matHeaderCellDef></th>
                <td mat-cell *matCellDef="let product">
                  <div class="table-image">
                    @if (product.imageUrl) {
                      <img [src]="product.imageUrl" [alt]="product.name">
                    } @else {
                      <mat-icon>inventory_2</mat-icon>
                    }
                  </div>
                </td>
              </ng-container>

              <!-- Name Column -->
              <ng-container matColumnDef="name">
                <th mat-header-cell *matHeaderCellDef mat-sort-header>Product</th>
                <td mat-cell *matCellDef="let product">
                  <div class="product-info">
                    <span class="name">{{ product.name }}</span>
                    <span class="sku">SKU: {{ product.sku || 'N/A' }}</span>
                  </div>
                </td>
              </ng-container>

              <!-- Category Column -->
              <ng-container matColumnDef="category">
                <th mat-header-cell *matHeaderCellDef mat-sort-header>Category</th>
                <td mat-cell *matCellDef="let product">{{ product.categoryName || '-' }}</td>
              </ng-container>

              <!-- Brand Column -->
              <ng-container matColumnDef="brand">
                <th mat-header-cell *matHeaderCellDef mat-sort-header>Brand</th>
                <td mat-cell *matCellDef="let product">{{ product.brandName || '-' }}</td>
              </ng-container>

              <!-- Price Column -->
              <ng-container matColumnDef="price">
                <th mat-header-cell *matHeaderCellDef mat-sort-header>Price</th>
                <td mat-cell *matCellDef="let product">{{ product.price | currency }}</td>
              </ng-container>

              <!-- Quantity Column -->
              <ng-container matColumnDef="quantity">
                <th mat-header-cell *matHeaderCellDef mat-sort-header>Stock</th>
                <td mat-cell *matCellDef="let product">
                  <span [class.low-stock]="product.quantity <= (product.reorderLevel || 10)">
                    {{ product.quantity }}
                  </span>
                </td>
              </ng-container>

              <!-- Status Column -->
              <ng-container matColumnDef="status">
                <th mat-header-cell *matHeaderCellDef>Status</th>
                <td mat-cell *matCellDef="let product">
                  <mat-chip [class]="product.isActive ? 'status-active' : 'status-inactive'">
                    {{ product.isActive ? 'Active' : 'Inactive' }}
                  </mat-chip>
                </td>
              </ng-container>

              <!-- Actions Column -->
              <ng-container matColumnDef="actions">
                <th mat-header-cell *matHeaderCellDef></th>
                <td mat-cell *matCellDef="let product">
                  <button mat-icon-button [matMenuTriggerFor]="menu">
                    <mat-icon>more_vert</mat-icon>
                  </button>
                  <mat-menu #menu="matMenu">
                    <button mat-menu-item (click)="openProductDialog(product)">
                      <mat-icon>edit</mat-icon>
                      <span>Edit</span>
                    </button>
                    <button mat-menu-item (click)="viewProduct(product)">
                      <mat-icon>visibility</mat-icon>
                      <span>View</span>
                    </button>
                    <button mat-menu-item (click)="duplicateProduct(product)">
                      <mat-icon>content_copy</mat-icon>
                      <span>Duplicate</span>
                    </button>
                    <mat-divider></mat-divider>
                    <button mat-menu-item class="delete-action" (click)="deleteProduct(product)">
                      <mat-icon>delete</mat-icon>
                      <span>Delete</span>
                    </button>
                  </mat-menu>
                </td>
              </ng-container>

              <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
              <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
            </table>

            <mat-paginator 
              [length]="totalProducts()"
              [pageSize]="pageSize()"
              [pageSizeOptions]="[10, 25, 50, 100]"
              showFirstLastButtons>
            </mat-paginator>
          </div>
        }

        <!-- Empty State -->
        @if (filteredProducts().length === 0) {
          <div class="empty-state">
            <mat-icon>inventory_2</mat-icon>
            <h3>No products found</h3>
            <p>Try adjusting your filters or add a new product</p>
            <button mat-raised-button color="primary" (click)="openProductDialog()">
              <mat-icon>add</mat-icon>
              Add Product
            </button>
          </div>
        }
      }
    </div>

    <!-- Product Dialog -->
    <ng-template #productDialog>
      <h2 mat-dialog-title>{{ editingProduct() ? 'Edit Product' : 'Add New Product' }}</h2>
      <mat-dialog-content>
        <form [formGroup]="productForm" class="product-form">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Product Name</mat-label>
            <input matInput formControlName="name" placeholder="Enter product name">
            @if (productForm.get('name')?.hasError('required')) {
              <mat-error>Product name is required</mat-error>
            }
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Description</mat-label>
            <textarea matInput formControlName="description" rows="3" placeholder="Product description"></textarea>
          </mat-form-field>

          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>SKU</mat-label>
              <input matInput formControlName="sku" placeholder="Stock keeping unit">
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Barcode</mat-label>
              <input matInput formControlName="barcode" placeholder="Barcode">
            </mat-form-field>
          </div>

          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Price</mat-label>
              <input matInput type="number" formControlName="price" placeholder="0.00">
              <span matPrefix>$&nbsp;</span>
              @if (productForm.get('price')?.hasError('required')) {
                <mat-error>Price is required</mat-error>
              }
              @if (productForm.get('price')?.hasError('min')) {
                <mat-error>Price must be positive</mat-error>
              }
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Cost</mat-label>
              <input matInput type="number" formControlName="cost" placeholder="0.00">
              <span matPrefix>$&nbsp;</span>
            </mat-form-field>
          </div>

          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Quantity</mat-label>
              <input matInput type="number" formControlName="quantity" placeholder="0">
              @if (productForm.get('quantity')?.hasError('required')) {
                <mat-error>Quantity is required</mat-error>
              }
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Reorder Level</mat-label>
              <input matInput type="number" formControlName="reorderLevel" placeholder="10">
            </mat-form-field>
          </div>

          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Category</mat-label>
              <mat-select formControlName="categoryId">
                <mat-option value="">None</mat-option>
                @for (category of categories(); track category.id) {
                  <mat-option [value]="category.id">{{ category.name }}</mat-option>
                }
              </mat-select>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Brand</mat-label>
              <mat-select formControlName="brandId">
                <mat-option value="">None</mat-option>
                @for (brand of brands(); track brand.id) {
                  <mat-option [value]="brand.id">{{ brand.name }}</mat-option>
                }
              </mat-select>
            </mat-form-field>
          </div>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Image URL</mat-label>
            <input matInput formControlName="imageUrl" placeholder="https://...">
          </mat-form-field>

          <mat-slide-toggle formControlName="isActive" color="primary">
            Active
          </mat-slide-toggle>
        </form>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button mat-dialog-close>Cancel</button>
        <button mat-raised-button 
                color="primary" 
                [disabled]="productForm.invalid || isSaving()"
                (click)="saveProduct()">
          @if (isSaving()) {
            <mat-spinner diameter="20"></mat-spinner>
          }
          {{ editingProduct() ? 'Update' : 'Create' }}
        </button>
      </mat-dialog-actions>
    </ng-template>
  `,
  styles: [`
    .products-container {
      padding: 24px;
    }

    .toolbar {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      margin-bottom: 24px;
      gap: 16px;
      flex-wrap: wrap;
    }

    .filters {
      display: flex;
      gap: 12px;
      flex-wrap: wrap;
      flex: 1;
    }

    .search-field {
      min-width: 250px;
      flex: 1;
    }

    .filter-field {
      min-width: 150px;
    }

    .actions {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .actions button.active {
      background: rgba(var(--primary-color-rgb, 89, 74, 226), 0.1);
      color: var(--primary-color);
    }

    .loading-container {
      display: flex;
      justify-content: center;
      padding: 48px;
    }

    /* Grid View */
    .products-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
      gap: 24px;
    }

    .product-card {
      transition: transform 0.2s, box-shadow 0.2s;
    }

    .product-card:hover {
      transform: translateY(-4px);
      box-shadow: 0 8px 24px rgba(0, 0, 0, 0.15);
    }

    .product-card.inactive {
      opacity: 0.7;
    }

    .product-image {
      height: 180px;
      display: flex;
      align-items: center;
      justify-content: center;
      background: #f5f5f5;
      position: relative;
      overflow: hidden;
    }

    .product-image img {
      width: 100%;
      height: 100%;
      object-fit: cover;
    }

    .product-image mat-icon {
      font-size: 64px;
      width: 64px;
      height: 64px;
      color: #ccc;
    }

    .inactive-badge {
      position: absolute;
      top: 8px;
      right: 8px;
      background: #ef4444;
      color: white;
      padding: 4px 8px;
      border-radius: 4px;
      font-size: 11px;
      font-weight: 500;
    }

    .product-name {
      font-size: 16px;
      font-weight: 500;
      margin: 0 0 4px;
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
    }

    .product-category {
      font-size: 13px;
      color: var(--text-secondary);
      margin: 0 0 12px;
    }

    .product-details {
      display: flex;
      justify-content: space-between;
      align-items: center;
    }

    .product-price {
      font-size: 18px;
      font-weight: 600;
      color: var(--primary-color);
    }

    .product-stock {
      font-size: 13px;
      color: var(--text-secondary);
    }

    .low-stock {
      color: #ef4444 !important;
      font-weight: 500;
    }

    /* Table View */
    .table-container {
      background: var(--surface-color);
      border-radius: 8px;
      overflow: hidden;
      box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
    }

    table {
      width: 100%;
    }

    .table-image {
      width: 48px;
      height: 48px;
      border-radius: 8px;
      overflow: hidden;
      display: flex;
      align-items: center;
      justify-content: center;
      background: #f5f5f5;
    }

    .table-image img {
      width: 100%;
      height: 100%;
      object-fit: cover;
    }

    .table-image mat-icon {
      color: #ccc;
    }

    .product-info .name {
      display: block;
      font-weight: 500;
    }

    .product-info .sku {
      display: block;
      font-size: 12px;
      color: var(--text-secondary);
    }

    .status-active {
      background: #dcfce7 !important;
      color: #166534 !important;
    }

    .status-inactive {
      background: #fee2e2 !important;
      color: #991b1b !important;
    }

    .delete-action {
      color: #dc2626;
    }

    /* Empty State */
    .empty-state {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 64px 24px;
      text-align: center;
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
      color: var(--text-primary);
    }

    .empty-state p {
      margin: 0 0 24px;
      color: var(--text-secondary);
    }

    /* Dialog Form */
    .product-form {
      display: flex;
      flex-direction: column;
      gap: 8px;
      min-width: 450px;
    }

    .form-row {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 16px;
    }

    .full-width {
      width: 100%;
    }

    @media (max-width: 768px) {
      .filters {
        flex-direction: column;
      }

      .search-field,
      .filter-field {
        width: 100%;
        min-width: auto;
      }

      .product-form {
        min-width: auto;
      }

      .form-row {
        grid-template-columns: 1fr;
      }
    }
  `]
})
export class ProductsComponent implements OnInit {
  @ViewChild('productDialog') productDialogTemplate!: TemplateRef<any>;

  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private notification = inject(NotificationService);

  // State signals
  products = signal<Product[]>([]);
  categories = signal<Category[]>([]);
  brands = signal<Brand[]>([]);
  isLoading = signal(false);
  isSaving = signal(false);
  searchQuery = signal('');
  selectedCategory = signal('');
  selectedBrand = signal('');
  viewMode = signal<'grid' | 'table'>('grid');
  editingProduct = signal<Product | null>(null);

  // Pagination
  pageSize = signal(10);
  totalProducts = signal(0);

  displayedColumns = ['image', 'name', 'category', 'brand', 'price', 'quantity', 'status', 'actions'];

  productForm: FormGroup = this.fb.group({
    name: ['', Validators.required],
    description: [''],
    sku: [''],
    barcode: [''],
    price: [0, [Validators.required, Validators.min(0)]],
    cost: [0],
    quantity: [0, Validators.required],
    reorderLevel: [10],
    categoryId: [''],
    brandId: [''],
    imageUrl: [''],
    isActive: [true]
  });

  // Computed signals
  filteredProducts = computed(() => {
    let result = this.products();
    
    const query = this.searchQuery().toLowerCase();
    if (query) {
      result = result.filter(p => 
        p.name.toLowerCase().includes(query) ||
        p.sku?.toLowerCase().includes(query) ||
        p.barcode?.toLowerCase().includes(query)
      );
    }
    
    const categoryId = this.selectedCategory();
    if (categoryId) {
      result = result.filter(p => p.categoryId === categoryId);
    }
    
    const brandId = this.selectedBrand();
    if (brandId) {
      result = result.filter(p => p.brandId === brandId);
    }
    
    return result;
  });

  ngOnInit(): void {
    this.loadData();
  }

  async loadData(): Promise<void> {
    this.isLoading.set(true);
    
    try {
      await Promise.all([
        this.loadProducts(),
        this.loadCategories(),
        this.loadBrands()
      ]);
    } finally {
      this.isLoading.set(false);
    }
  }

  private async loadProducts(): Promise<void> {
    // Mock data - replace with actual API call
    const mockProducts: Product[] = [
      { id: '1', name: 'iPhone 15 Pro', description: 'Latest Apple smartphone', sku: 'IP15P-001', price: 1199, quantity: 50, categoryId: '1', categoryName: 'Electronics', brandId: '1', brandName: 'Apple', isActive: true, imageUrl: 'https://picsum.photos/seed/iphone/300/200' },
      { id: '2', name: 'MacBook Pro 14"', description: 'Professional laptop', sku: 'MBP14-001', price: 2499, quantity: 25, categoryId: '1', categoryName: 'Electronics', brandId: '1', brandName: 'Apple', isActive: true },
      { id: '3', name: 'Samsung Galaxy S24', description: 'Android flagship', sku: 'SGS24-001', price: 999, quantity: 75, categoryId: '1', categoryName: 'Electronics', brandId: '2', brandName: 'Samsung', isActive: true },
      { id: '4', name: 'Nike Air Max', description: 'Running shoes', sku: 'NAM-001', price: 179, quantity: 5, reorderLevel: 10, categoryId: '2', categoryName: 'Footwear', brandId: '3', brandName: 'Nike', isActive: true },
      { id: '5', name: 'Levi\'s 501 Jeans', description: 'Classic denim', sku: 'LV501-001', price: 89, quantity: 100, categoryId: '3', categoryName: 'Clothing', brandId: '4', brandName: 'Levi\'s', isActive: false },
    ];
    
    await new Promise(resolve => setTimeout(resolve, 300));
    this.products.set(mockProducts);
    this.totalProducts.set(mockProducts.length);
  }

  private async loadCategories(): Promise<void> {
    const mockCategories: Category[] = [
      { id: '1', name: 'Electronics', isActive: true },
      { id: '2', name: 'Footwear', isActive: true },
      { id: '3', name: 'Clothing', isActive: true },
      { id: '4', name: 'Accessories', isActive: true },
    ];
    this.categories.set(mockCategories);
  }

  private async loadBrands(): Promise<void> {
    const mockBrands: Brand[] = [
      { id: '1', name: 'Apple', isActive: true },
      { id: '2', name: 'Samsung', isActive: true },
      { id: '3', name: 'Nike', isActive: true },
      { id: '4', name: 'Levi\'s', isActive: true },
    ];
    this.brands.set(mockBrands);
  }

  onSearch(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.searchQuery.set(value);
  }

  onCategoryChange(value: string): void {
    this.selectedCategory.set(value);
  }

  onBrandChange(value: string): void {
    this.selectedBrand.set(value);
  }

  openProductDialog(product?: Product): void {
    this.editingProduct.set(product || null);
    
    if (product) {
      this.productForm.patchValue(product);
    } else {
      this.productForm.reset({ isActive: true, quantity: 0, price: 0, reorderLevel: 10 });
    }

    this.dialog.open(this.productDialogTemplate, {
      width: '550px',
      maxHeight: '90vh'
    });
  }

  async saveProduct(): Promise<void> {
    if (this.productForm.invalid) return;

    this.isSaving.set(true);
    
    try {
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      if (this.editingProduct()) {
        this.notification.success('Product updated successfully');
      } else {
        this.notification.success('Product created successfully');
      }
      
      this.dialog.closeAll();
      this.loadProducts();
    } catch (error) {
      this.notification.error('Failed to save product');
    } finally {
      this.isSaving.set(false);
    }
  }

  viewProduct(product: Product): void {
    console.log('View product:', product);
  }

  duplicateProduct(product: Product): void {
    const duplicate = { ...product, id: '', name: `${product.name} (Copy)` };
    this.openProductDialog(duplicate);
  }

  async deleteProduct(product: Product): Promise<void> {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete Product',
        message: `Are you sure you want to delete "${product.name}"?`,
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(async result => {
      if (result) {
        try {
          await new Promise(resolve => setTimeout(resolve, 500));
          this.products.update(products => products.filter(p => p.id !== product.id));
          this.notification.success('Product deleted successfully');
        } catch (error) {
          this.notification.error('Failed to delete product');
        }
      }
    });
  }
}

import { Component, inject, signal, OnInit, ViewChild, TemplateRef, computed } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDividerModule } from '@angular/material/divider';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatExpansionModule } from '@angular/material/expansion';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';
import { NotificationService } from '@core/services/notification.service';
import { TenantService } from '@core/services/tenant.service';
import { PermissionService } from '@core/services/permission.service';
import { Tenant, CreateTenantRequest, TenantDetail } from '@core/models/tenant.model';
import { FshActions, FshResources } from '@core/models/permission.model';

@Component({
  selector: 'app-tenants',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatDialogModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatMenuModule,
    MatTooltipModule,
    MatDividerModule,
    MatCardModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatExpansionModule,
    PageHeaderComponent,
    DatePipe
  ],
  template: `
    <div class="tenants-container">
      <app-page-header 
        title="Tenants" 
        subtitle="Manage your tenants and subscriptions"
        icon="business">
      </app-page-header>

      <!-- Toolbar -->
      <div class="toolbar">
        <div class="search-container">
          <mat-form-field appearance="outline" class="search-field">
            <mat-label>Search tenants</mat-label>
            <input matInput 
                   [value]="searchQuery()" 
                   (input)="onSearch($event)"
                   placeholder="Search by name or email...">
            <mat-icon matPrefix>search</mat-icon>
            @if (searchQuery()) {
              <button matSuffix mat-icon-button (click)="clearSearch()">
                <mat-icon>close</mat-icon>
              </button>
            }
          </mat-form-field>
        </div>

        <div class="actions">
          <button mat-stroked-button (click)="refreshTenants()">
            <mat-icon>refresh</mat-icon>
            Refresh
          </button>
          @if (canCreate()) {
            <button mat-raised-button color="primary" (click)="openTenantDialog()">
              <mat-icon>add_business</mat-icon>
              Add Tenant
            </button>
          }
        </div>
      </div>

      <!-- Table -->
      <div class="table-container">
        @if (isLoading()) {
          <div class="loading-overlay">
            <mat-spinner diameter="48"></mat-spinner>
          </div>
        }

        <table mat-table [dataSource]="filteredTenants()" matSort (matSortChange)="onSort($event)" multiTemplateDataRows>
          <!-- ID Column -->
          <ng-container matColumnDef="id">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>ID</th>
            <td mat-cell *matCellDef="let tenant">
              <code class="tenant-id">{{ tenant.id }}</code>
            </td>
          </ng-container>

          <!-- Name Column -->
          <ng-container matColumnDef="name">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Name</th>
            <td mat-cell *matCellDef="let tenant">
              <div class="tenant-name">{{ tenant.name }}</div>
            </td>
          </ng-container>

          <!-- Admin Email Column -->
          <ng-container matColumnDef="adminEmail">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Admin Email</th>
            <td mat-cell *matCellDef="let tenant">{{ tenant.adminEmail }}</td>
          </ng-container>

          <!-- Valid Upto Column -->
          <ng-container matColumnDef="validUpto">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Valid Until</th>
            <td mat-cell *matCellDef="let tenant">
              <span [class]="isExpired(tenant.validUpto) ? 'expired' : isExpiringSoon(tenant.validUpto) ? 'expiring-soon' : ''">
                {{ tenant.validUpto | date:'MMM dd, yyyy' }}
              </span>
            </td>
          </ng-container>

          <!-- Status Column -->
          <ng-container matColumnDef="isActive">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Status</th>
            <td mat-cell *matCellDef="let tenant">
              <mat-chip [class]="tenant.isActive ? 'status-active' : 'status-inactive'">
                {{ tenant.isActive ? 'Active' : 'Inactive' }}
              </mat-chip>
            </td>
          </ng-container>

          <!-- Actions Column -->
          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef></th>
            <td mat-cell *matCellDef="let tenant">
              <button mat-icon-button [matMenuTriggerFor]="menu">
                <mat-icon>more_vert</mat-icon>
              </button>
              <mat-menu #menu="matMenu">
                @if (canUpgrade()) {
                  <button mat-menu-item (click)="openUpgradeDialog(tenant)">
                    <mat-icon>upgrade</mat-icon>
                    <span>Upgrade Subscription</span>
                  </button>
                }
                <button mat-menu-item (click)="toggleDetails(tenant)">
                  <mat-icon>{{ tenant.showDetails ? 'visibility_off' : 'visibility' }}</mat-icon>
                  <span>{{ tenant.showDetails ? 'Hide' : 'Show' }} Details</span>
                </button>
                @if (canUpdate()) {
                  <mat-divider></mat-divider>
                  @if (!tenant.isActive) {
                    <button mat-menu-item class="activate-action" (click)="activateTenant(tenant)">
                      <mat-icon>check_circle</mat-icon>
                      <span>Activate Tenant</span>
                    </button>
                  } @else {
                    <button mat-menu-item class="deactivate-action" (click)="deactivateTenant(tenant)">
                      <mat-icon>block</mat-icon>
                      <span>Deactivate Tenant</span>
                    </button>
                  }
                }
              </mat-menu>
            </td>
          </ng-container>

          <!-- Expanded Detail Row -->
          <ng-container matColumnDef="expandedDetail">
            <td mat-cell *matCellDef="let tenant" [attr.colspan]="displayedColumns.length">
              @if (tenant.showDetails) {
                <div class="tenant-detail">
                  <mat-card appearance="outlined">
                    <mat-card-header>
                      <mat-card-title>Tenant Details: {{ tenant.id }}</mat-card-title>
                    </mat-card-header>
                    <mat-card-content>
                      <div class="detail-grid">
                        <div class="detail-item">
                          <span class="detail-label">Database:</span>
                          @if (tenant.connectionString) {
                            <mat-chip color="primary">Dedicated Database</mat-chip>
                          } @else {
                            <mat-chip color="accent">Shared Database</mat-chip>
                          }
                        </div>
                        @if (tenant.connectionString) {
                          <div class="detail-item full-width">
                            <span class="detail-label">Connection String:</span>
                            <code class="connection-string">{{ tenant.connectionString }}</code>
                          </div>
                        }
                        <div class="detail-item">
                          <span class="detail-label">Issuer:</span>
                          <span>{{ tenant.issuer || 'Default' }}</span>
                        </div>
                      </div>
                    </mat-card-content>
                  </mat-card>
                </div>
              }
            </td>
          </ng-container>

          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns;" class="tenant-row"></tr>
          <tr mat-row *matRowDef="let row; columns: ['expandedDetail']" class="detail-row"></tr>

          <!-- No Data Row -->
          <tr class="mat-row" *matNoDataRow>
            <td class="mat-cell no-data-cell" [attr.colspan]="displayedColumns.length">
              <div class="no-data">
                <mat-icon>search_off</mat-icon>
                <p>No tenants found</p>
                @if (searchQuery()) {
                  <button mat-button color="primary" (click)="clearSearch()">Clear search</button>
                }
              </div>
            </td>
          </tr>
        </table>

        <mat-paginator 
          [length]="totalTenants()"
          [pageSize]="pageSize()"
          [pageSizeOptions]="[5, 10, 25, 50]"
          [pageIndex]="pageIndex()"
          (page)="onPageChange($event)"
          showFirstLastButtons>
        </mat-paginator>
      </div>
    </div>

    <!-- Create Tenant Dialog -->
    <ng-template #tenantDialog>
      <h2 mat-dialog-title>Add New Tenant</h2>
      <mat-dialog-content>
        <form [formGroup]="tenantForm" class="tenant-form">
          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Key (ID)</mat-label>
              <input matInput formControlName="id" placeholder="Unique tenant key">
              @if (tenantForm.get('id')?.hasError('required')) {
                <mat-error>Key is required</mat-error>
              }
              @if (tenantForm.get('id')?.hasError('pattern')) {
                <mat-error>Only lowercase letters, numbers, and hyphens allowed</mat-error>
              }
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Name</mat-label>
              <input matInput formControlName="name" placeholder="Tenant name">
              @if (tenantForm.get('name')?.hasError('required')) {
                <mat-error>Name is required</mat-error>
              }
            </mat-form-field>
          </div>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Connection String</mat-label>
            <input matInput formControlName="connectionString" placeholder="Leave blank for shared database">
            <mat-hint>Leave blank to use the default shared database</mat-hint>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Admin Email</mat-label>
            <input matInput type="email" formControlName="adminEmail" placeholder="Admin email address">
            @if (tenantForm.get('adminEmail')?.hasError('required')) {
              <mat-error>Admin email is required</mat-error>
            }
            @if (tenantForm.get('adminEmail')?.hasError('email')) {
              <mat-error>Invalid email format</mat-error>
            }
          </mat-form-field>
        </form>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button mat-dialog-close>Cancel</button>
        <button mat-raised-button 
                color="primary" 
                [disabled]="tenantForm.invalid || isSaving()"
                (click)="saveTenant()">
          @if (isSaving()) {
            <mat-spinner diameter="20"></mat-spinner>
          }
          Create Tenant
        </button>
      </mat-dialog-actions>
    </ng-template>

    <!-- Upgrade Subscription Dialog -->
    <ng-template #upgradeDialog>
      <h2 mat-dialog-title>Upgrade Subscription</h2>
      <mat-dialog-content>
        <form [formGroup]="upgradeForm" class="upgrade-form">
          <p class="upgrade-info">
            Extend the subscription for tenant: <strong>{{ selectedTenant()?.name }}</strong>
          </p>
          
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>New Expiry Date</mat-label>
            <input matInput [matDatepicker]="picker" formControlName="extendedExpiryDate">
            <mat-datepicker-toggle matIconSuffix [for]="picker"></mat-datepicker-toggle>
            <mat-datepicker #picker></mat-datepicker>
            @if (upgradeForm.get('extendedExpiryDate')?.hasError('required')) {
              <mat-error>Expiry date is required</mat-error>
            }
          </mat-form-field>
        </form>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button mat-dialog-close>Cancel</button>
        <button mat-raised-button 
                color="primary" 
                [disabled]="upgradeForm.invalid || isSaving()"
                (click)="upgradeSubscription()">
          @if (isSaving()) {
            <mat-spinner diameter="20"></mat-spinner>
          }
          Upgrade
        </button>
      </mat-dialog-actions>
    </ng-template>
  `,
  styles: [`
    .tenants-container {
      padding: 24px;
    }

    .toolbar {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 24px;
      gap: 16px;
      flex-wrap: wrap;
    }

    .search-container {
      flex: 1;
      min-width: 250px;
      max-width: 400px;
    }

    .search-field {
      width: 100%;
    }

    .actions {
      display: flex;
      gap: 12px;
    }

    .table-container {
      position: relative;
      background: var(--surface-color);
      border-radius: 8px;
      overflow: hidden;
      box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
    }

    .loading-overlay {
      position: absolute;
      inset: 0;
      display: flex;
      align-items: center;
      justify-content: center;
      background: rgba(255, 255, 255, 0.8);
      z-index: 10;
    }

    table {
      width: 100%;
    }

    .tenant-id {
      font-family: monospace;
      background: rgba(0, 0, 0, 0.05);
      padding: 4px 8px;
      border-radius: 4px;
      font-size: 12px;
    }

    .tenant-name {
      font-weight: 500;
    }

    .expired {
      color: #dc2626;
      font-weight: 500;
    }

    .expiring-soon {
      color: #ea580c;
      font-weight: 500;
    }

    .status-active {
      background: #dcfce7 !important;
      color: #166534 !important;
    }

    .status-inactive {
      background: #fee2e2 !important;
      color: #991b1b !important;
    }

    .activate-action {
      color: #16a34a;
    }

    .deactivate-action {
      color: #dc2626;
    }

    .tenant-row {
      cursor: pointer;
    }

    .detail-row {
      height: 0;
    }

    .detail-row td {
      padding: 0 !important;
    }

    .tenant-detail {
      padding: 16px;
      background: rgba(0, 0, 0, 0.02);
    }

    .detail-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
      gap: 16px;
    }

    .detail-item {
      display: flex;
      flex-direction: column;
      gap: 4px;
    }

    .detail-item.full-width {
      grid-column: 1 / -1;
    }

    .detail-label {
      font-size: 12px;
      color: var(--text-secondary);
      font-weight: 500;
    }

    .connection-string {
      font-family: monospace;
      font-size: 12px;
      background: rgba(0, 0, 0, 0.05);
      padding: 8px;
      border-radius: 4px;
      word-break: break-all;
    }

    .no-data-cell {
      text-align: center;
      padding: 48px !important;
    }

    .no-data {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 8px;
      color: var(--text-secondary);
    }

    .no-data mat-icon {
      font-size: 48px;
      width: 48px;
      height: 48px;
      opacity: 0.5;
    }

    /* Dialog Styles */
    .tenant-form,
    .upgrade-form {
      display: flex;
      flex-direction: column;
      gap: 8px;
      min-width: 400px;
    }

    .form-row {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 16px;
    }

    .full-width {
      width: 100%;
    }

    .upgrade-info {
      margin-bottom: 16px;
      padding: 12px;
      background: rgba(0, 0, 0, 0.05);
      border-radius: 4px;
    }

    @media (max-width: 600px) {
      .toolbar {
        flex-direction: column;
        align-items: stretch;
      }

      .search-container {
        max-width: none;
      }

      .actions {
        justify-content: flex-end;
      }

      .tenant-form,
      .upgrade-form {
        min-width: auto;
      }

      .form-row {
        grid-template-columns: 1fr;
      }
    }
  `]
})
export class TenantsComponent implements OnInit {
  @ViewChild('tenantDialog') tenantDialogTemplate!: TemplateRef<any>;
  @ViewChild('upgradeDialog') upgradeDialogTemplate!: TemplateRef<any>;

  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private notification = inject(NotificationService);
  private tenantService = inject(TenantService);
  private permissionService = inject(PermissionService);

  // State signals
  tenants = signal<TenantDetail[]>([]);
  isLoading = signal(false);
  isSaving = signal(false);
  searchQuery = signal('');
  selectedTenant = signal<TenantDetail | null>(null);

  // Pagination signals
  pageIndex = signal(0);
  pageSize = signal(10);
  totalTenants = signal(0);

  displayedColumns = ['id', 'name', 'adminEmail', 'validUpto', 'isActive', 'actions'];

  tenantForm: FormGroup = this.fb.group({
    id: ['', [Validators.required, Validators.pattern(/^[a-z0-9-]+$/)]],
    name: ['', Validators.required],
    connectionString: [''],
    adminEmail: ['', [Validators.required, Validators.email]]
  });

  upgradeForm: FormGroup = this.fb.group({
    extendedExpiryDate: [null, Validators.required]
  });

  // Permission checks
  canCreate = computed(() => this.permissionService.canCreate(FshResources.Tenants));
  canUpdate = computed(() => this.permissionService.canUpdate(FshResources.Tenants));
  canUpgrade = computed(() => this.permissionService.hasPermission(FshActions.UpgradeSubscription, FshResources.Tenants));

  // Computed signals
  filteredTenants = computed(() => {
    const query = this.searchQuery().toLowerCase();
    if (!query) return this.tenants();
    
    return this.tenants().filter(tenant => 
      tenant.id?.toLowerCase().includes(query) ||
      tenant.name?.toLowerCase().includes(query) ||
      tenant.adminEmail?.toLowerCase().includes(query)
    );
  });

  ngOnInit(): void {
    this.loadTenants();
  }

  async loadTenants(): Promise<void> {
    this.isLoading.set(true);
    
    try {
      this.tenantService.getTenants().subscribe({
        next: (tenants) => {
          this.tenants.set(tenants.map(t => ({ ...t, showDetails: false })));
          this.totalTenants.set(tenants.length);
          this.isLoading.set(false);
        },
        error: (error) => {
          this.notification.error('Failed to load tenants');
          this.isLoading.set(false);
          // Load mock data for demo
          this.loadMockData();
        }
      });
    } catch (error) {
      this.notification.error('Failed to load tenants');
      this.isLoading.set(false);
    }
  }

  private loadMockData(): void {
    const mockTenants: TenantDetail[] = [
      { 
        id: 'root', 
        name: 'Root Tenant', 
        adminEmail: 'admin@root.com', 
        validUpto: new Date('2025-12-31'), 
        isActive: true,
        showDetails: false
      },
      { 
        id: 'demo', 
        name: 'Demo Company', 
        adminEmail: 'admin@demo.com', 
        validUpto: new Date('2025-06-30'), 
        isActive: true,
        connectionString: 'Server=localhost;Database=Demo;',
        showDetails: false
      },
      { 
        id: 'test', 
        name: 'Test Organization', 
        adminEmail: 'admin@test.com', 
        validUpto: new Date('2024-12-15'), 
        isActive: false,
        showDetails: false
      }
    ];
    this.tenants.set(mockTenants);
    this.totalTenants.set(mockTenants.length);
  }

  refreshTenants(): void {
    this.loadTenants();
  }

  onSearch(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.searchQuery.set(value);
  }

  clearSearch(): void {
    this.searchQuery.set('');
  }

  onSort(sort: Sort): void {
    console.log('Sort:', sort);
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex.set(event.pageIndex);
    this.pageSize.set(event.pageSize);
  }

  toggleDetails(tenant: TenantDetail): void {
    this.tenants.update(tenants => 
      tenants.map(t => ({
        ...t,
        showDetails: t.id === tenant.id ? !t.showDetails : false
      }))
    );
  }

  openTenantDialog(): void {
    this.tenantForm.reset();
    this.dialog.open(this.tenantDialogTemplate, {
      width: '500px'
    });
  }

  async saveTenant(): Promise<void> {
    if (this.tenantForm.invalid) return;

    this.isSaving.set(true);
    
    try {
      const request: CreateTenantRequest = this.tenantForm.value;
      this.tenantService.createTenant(request).subscribe({
        next: () => {
          this.notification.success('Tenant created successfully');
          this.dialog.closeAll();
          this.loadTenants();
          this.isSaving.set(false);
        },
        error: (error) => {
          this.notification.error(error?.error?.message || 'Failed to create tenant');
          this.isSaving.set(false);
        }
      });
    } catch (error) {
      this.notification.error('Failed to create tenant');
      this.isSaving.set(false);
    }
  }

  openUpgradeDialog(tenant: TenantDetail): void {
    this.selectedTenant.set(tenant);
    this.upgradeForm.patchValue({
      extendedExpiryDate: tenant.validUpto
    });
    this.dialog.open(this.upgradeDialogTemplate, {
      width: '400px'
    });
  }

  async upgradeSubscription(): Promise<void> {
    if (this.upgradeForm.invalid || !this.selectedTenant()) return;

    this.isSaving.set(true);
    
    try {
      const request = {
        tenant: this.selectedTenant()!.id,
        extendedExpiryDate: this.upgradeForm.value.extendedExpiryDate
      };
      
      this.tenantService.upgradeSubscription(request).subscribe({
        next: () => {
          this.notification.success('Subscription upgraded successfully');
          this.dialog.closeAll();
          this.loadTenants();
          this.isSaving.set(false);
        },
        error: (error) => {
          this.notification.error(error?.error?.message || 'Failed to upgrade subscription');
          this.isSaving.set(false);
        }
      });
    } catch (error) {
      this.notification.error('Failed to upgrade subscription');
      this.isSaving.set(false);
    }
  }

  async activateTenant(tenant: TenantDetail): Promise<void> {
    this.tenantService.activateTenant(tenant.id).subscribe({
      next: () => {
        this.notification.success('Tenant activated');
        this.loadTenants();
      },
      error: (error) => {
        this.notification.error('Failed to activate tenant');
      }
    });
  }

  async deactivateTenant(tenant: TenantDetail): Promise<void> {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Deactivate Tenant',
        message: `Are you sure you want to deactivate ${tenant.name}? Users will not be able to access this tenant.`,
        confirmText: 'Deactivate',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.tenantService.deactivateTenant(tenant.id).subscribe({
          next: () => {
            this.notification.success('Tenant deactivated');
            this.loadTenants();
          },
          error: (error) => {
            this.notification.error('Failed to deactivate tenant');
          }
        });
      }
    });
  }

  isExpired(date: Date): boolean {
    return new Date(date) < new Date();
  }

  isExpiringSoon(date: Date): boolean {
    const thirtyDaysFromNow = new Date();
    thirtyDaysFromNow.setDate(thirtyDaysFromNow.getDate() + 30);
    return new Date(date) <= thirtyDaysFromNow && !this.isExpired(date);
  }
}

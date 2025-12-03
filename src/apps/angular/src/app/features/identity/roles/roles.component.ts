import { Component, inject, signal, OnInit, ViewChild, TemplateRef, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
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
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDividerModule } from '@angular/material/divider';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';
import { NotificationService } from '@core/services/notification.service';
import { RoleService } from '@core/services/role.service';
import { PermissionService } from '@core/services/permission.service';
import { Role, CreateOrUpdateRoleRequest } from '@core/models/role.model';
import { FshActions, FshResources, isDefaultRole } from '@core/models/permission.model';

@Component({
  selector: 'app-roles',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
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
    MatMenuModule,
    MatTooltipModule,
    MatDividerModule,
    PageHeaderComponent
  ],
  template: `
    <div class="roles-container">
      <app-page-header 
        title="Roles" 
        subtitle="Manage system roles and permissions"
        icon="admin_panel_settings">
      </app-page-header>

      <!-- Toolbar -->
      <div class="toolbar">
        <div class="search-container">
          <mat-form-field appearance="outline" class="search-field">
            <mat-label>Search roles</mat-label>
            <input matInput 
                   [value]="searchQuery()" 
                   (input)="onSearch($event)"
                   placeholder="Search by name or description...">
            <mat-icon matPrefix>search</mat-icon>
            @if (searchQuery()) {
              <button matSuffix mat-icon-button (click)="clearSearch()">
                <mat-icon>close</mat-icon>
              </button>
            }
          </mat-form-field>
        </div>

        <div class="actions">
          <button mat-stroked-button (click)="refreshRoles()">
            <mat-icon>refresh</mat-icon>
            Refresh
          </button>
          @if (canCreate()) {
            <button mat-raised-button color="primary" (click)="openRoleDialog()">
              <mat-icon>add</mat-icon>
              Add Role
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

        <table mat-table [dataSource]="filteredRoles()" matSort (matSortChange)="onSort($event)">
          <!-- ID Column -->
          <ng-container matColumnDef="id">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>ID</th>
            <td mat-cell *matCellDef="let role">
              <code class="role-id">{{ role.id }}</code>
            </td>
          </ng-container>

          <!-- Name Column -->
          <ng-container matColumnDef="name">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Name</th>
            <td mat-cell *matCellDef="let role">
              <div class="role-name">
                {{ role.name }}
                @if (isDefaultRole(role.name)) {
                  <span class="default-badge">Default</span>
                }
              </div>
            </td>
          </ng-container>

          <!-- Description Column -->
          <ng-container matColumnDef="description">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Description</th>
            <td mat-cell *matCellDef="let role">{{ role.description || '-' }}</td>
          </ng-container>

          <!-- Actions Column -->
          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef></th>
            <td mat-cell *matCellDef="let role">
              <button mat-icon-button [matMenuTriggerFor]="menu">
                <mat-icon>more_vert</mat-icon>
              </button>
              <mat-menu #menu="matMenu">
                @if (canViewPermissions()) {
                  <button mat-menu-item [routerLink]="['/identity/roles', role.id, 'permissions']">
                    <mat-icon>security</mat-icon>
                    <span>Manage Permissions</span>
                  </button>
                }
                @if (canUpdate() && !isDefaultRole(role.name)) {
                  <button mat-menu-item (click)="openRoleDialog(role)">
                    <mat-icon>edit</mat-icon>
                    <span>Edit</span>
                  </button>
                }
                @if (canDelete() && !isDefaultRole(role.name)) {
                  <mat-divider></mat-divider>
                  <button mat-menu-item class="delete-action" (click)="deleteRole(role)">
                    <mat-icon>delete</mat-icon>
                    <span>Delete</span>
                  </button>
                }
              </mat-menu>
            </td>
          </ng-container>

          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>

          <!-- No Data Row -->
          <tr class="mat-row" *matNoDataRow>
            <td class="mat-cell no-data-cell" [attr.colspan]="displayedColumns.length">
              <div class="no-data">
                <mat-icon>search_off</mat-icon>
                <p>No roles found</p>
                @if (searchQuery()) {
                  <button mat-button color="primary" (click)="clearSearch()">Clear search</button>
                }
              </div>
            </td>
          </tr>
        </table>

        <mat-paginator 
          [length]="totalRoles()"
          [pageSize]="pageSize()"
          [pageSizeOptions]="[5, 10, 25, 50]"
          [pageIndex]="pageIndex()"
          (page)="onPageChange($event)"
          showFirstLastButtons>
        </mat-paginator>
      </div>
    </div>

    <!-- Role Dialog Template -->
    <ng-template #roleDialog>
      <h2 mat-dialog-title>{{ editingRole() ? 'Edit Role' : 'Add New Role' }}</h2>
      <mat-dialog-content>
        <form [formGroup]="roleForm" class="role-form">
          @if (editingRole()) {
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Role ID</mat-label>
              <input matInput [value]="editingRole()?.id" readonly disabled>
            </mat-form-field>
          }

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Role Name</mat-label>
            <input matInput formControlName="name" placeholder="Enter role name">
            @if (roleForm.get('name')?.hasError('required')) {
              <mat-error>Role name is required</mat-error>
            }
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Description</mat-label>
            <textarea matInput formControlName="description" placeholder="Enter description" rows="3"></textarea>
          </mat-form-field>
        </form>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button mat-dialog-close>Cancel</button>
        <button mat-raised-button 
                color="primary" 
                [disabled]="roleForm.invalid || isSaving()"
                (click)="saveRole()">
          @if (isSaving()) {
            <mat-spinner diameter="20"></mat-spinner>
          }
          {{ editingRole() ? 'Update' : 'Create' }}
        </button>
      </mat-dialog-actions>
    </ng-template>
  `,
  styles: [`
    .roles-container {
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

    .role-id {
      font-family: monospace;
      background: rgba(0, 0, 0, 0.05);
      padding: 4px 8px;
      border-radius: 4px;
      font-size: 12px;
    }

    .role-name {
      display: flex;
      align-items: center;
      gap: 8px;
      font-weight: 500;
    }

    .default-badge {
      font-size: 10px;
      background: #dbeafe;
      color: #1e40af;
      padding: 2px 6px;
      border-radius: 4px;
      font-weight: 500;
    }

    .delete-action {
      color: #dc2626;
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
    .role-form {
      display: flex;
      flex-direction: column;
      gap: 8px;
      min-width: 400px;
    }

    .full-width {
      width: 100%;
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

      .role-form {
        min-width: auto;
      }
    }
  `]
})
export class RolesComponent implements OnInit {
  @ViewChild('roleDialog') roleDialogTemplate!: TemplateRef<any>;

  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private notification = inject(NotificationService);
  private roleService = inject(RoleService);
  private permissionService = inject(PermissionService);

  // State signals
  roles = signal<Role[]>([]);
  isLoading = signal(false);
  isSaving = signal(false);
  searchQuery = signal('');
  editingRole = signal<Role | null>(null);

  // Pagination signals
  pageIndex = signal(0);
  pageSize = signal(10);
  totalRoles = signal(0);

  displayedColumns = ['id', 'name', 'description', 'actions'];

  roleForm: FormGroup = this.fb.group({
    id: [''],
    name: ['', Validators.required],
    description: ['']
  });

  // Permission checks
  canCreate = computed(() => this.permissionService.canCreate(FshResources.Roles));
  canUpdate = computed(() => this.permissionService.canUpdate(FshResources.Roles));
  canDelete = computed(() => this.permissionService.canDelete(FshResources.Roles));
  canViewPermissions = computed(() => this.permissionService.canView(FshResources.RoleClaims));

  // Computed signals
  filteredRoles = computed(() => {
    const query = this.searchQuery().toLowerCase();
    if (!query) return this.roles();
    
    return this.roles().filter(role => 
      role.name?.toLowerCase().includes(query) ||
      role.description?.toLowerCase().includes(query)
    );
  });

  isDefaultRole = isDefaultRole;

  ngOnInit(): void {
    this.loadRoles();
  }

  loadRoles(): void {
    this.isLoading.set(true);
    
    this.roleService.getRoles().subscribe({
      next: (roles) => {
        this.roles.set(roles);
        this.totalRoles.set(roles.length);
        this.isLoading.set(false);
      },
      error: () => {
        this.notification.error('Failed to load roles');
        this.isLoading.set(false);
        // Load mock data for demo
        this.loadMockData();
      }
    });
  }

  private loadMockData(): void {
    const mockRoles: Role[] = [
      { id: '1', name: 'Admin', description: 'Full administrative access to the system' },
      { id: '2', name: 'Basic', description: 'Basic user access' },
      { id: '3', name: 'Manager', description: 'Management level access' },
      { id: '4', name: 'Accountant', description: 'Access to accounting features' },
      { id: '5', name: 'HR Manager', description: 'Human resources management access' }
    ];
    this.roles.set(mockRoles);
    this.totalRoles.set(mockRoles.length);
  }

  refreshRoles(): void {
    this.loadRoles();
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

  openRoleDialog(role?: Role): void {
    this.editingRole.set(role || null);
    
    if (role) {
      this.roleForm.patchValue({
        id: role.id,
        name: role.name,
        description: role.description || ''
      });
    } else {
      this.roleForm.reset();
    }

    this.dialog.open(this.roleDialogTemplate, {
      width: '500px'
    });
  }

  saveRole(): void {
    if (this.roleForm.invalid) return;

    this.isSaving.set(true);
    
    const request: CreateOrUpdateRoleRequest = {
      id: this.editingRole()?.id,
      name: this.roleForm.value.name,
      description: this.roleForm.value.description
    };
    
    this.roleService.createOrUpdateRole(request).subscribe({
      next: () => {
        this.notification.success(this.editingRole() ? 'Role updated successfully' : 'Role created successfully');
        this.dialog.closeAll();
        this.loadRoles();
        this.isSaving.set(false);
      },
      error: (error) => {
        this.notification.error(error?.error?.message || 'Failed to save role');
        this.isSaving.set(false);
      }
    });
  }

  deleteRole(role: Role): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete Role',
        message: `Are you sure you want to delete the role "${role.name}"? This action cannot be undone.`,
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.roleService.deleteRole(role.id).subscribe({
          next: () => {
            this.notification.success('Role deleted successfully');
            this.loadRoles();
          },
          error: () => {
            this.notification.error('Failed to delete role');
          }
        });
      }
    });
  }
}

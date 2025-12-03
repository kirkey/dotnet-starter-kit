import { Component, inject, signal, OnInit, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatChipsModule } from '@angular/material/chips';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { NotificationService } from '@core/services/notification.service';
import { RoleService } from '@core/services/role.service';
import { PermissionService } from '@core/services/permission.service';
import { AuthService } from '@core/services/auth.service';
import { 
  Permission, 
  PermissionGroup, 
  FshActions, 
  FshResources,
  getAllPermissions,
  groupPermissionsByResource
} from '@core/models/permission.model';
import { RolePermissions, UpdatePermissionsRequest } from '@core/models/role.model';

interface PermissionViewModel extends Permission {
  enabled: boolean;
}

@Component({
  selector: 'app-role-permissions',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    MatTabsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatInputModule,
    MatChipsModule,
    PageHeaderComponent
  ],
  template: `
    <div class="permissions-container">
      <app-page-header 
        [title]="title()" 
        [subtitle]="description()"
        icon="security">
      </app-page-header>

      @if (isLoading()) {
        <div class="loading">
          <mat-spinner diameter="48"></mat-spinner>
        </div>
      } @else {
        <!-- Toolbar -->
        <div class="toolbar">
          <div class="toolbar-actions">
            <button mat-stroked-button routerLink="/identity/roles">
              <mat-icon>arrow_back</mat-icon>
              Back
            </button>
            @if (canEdit()) {
              <button mat-raised-button color="primary" (click)="savePermissions()" [disabled]="isSaving()">
                @if (isSaving()) {
                  <mat-spinner diameter="20"></mat-spinner>
                }
                <mat-icon>save</mat-icon>
                Update Permissions
              </button>
            }
          </div>
          <div class="search-container">
            @if (canSearch()) {
              <mat-form-field appearance="outline" class="search-field">
                <mat-label>Search Permissions</mat-label>
                <input matInput 
                       [(ngModel)]="searchString"
                       placeholder="Search for permissions...">
                <mat-icon matPrefix>search</mat-icon>
              </mat-form-field>
            }
          </div>
        </div>

        <!-- Permissions Tabs -->
        <mat-tab-group animationDuration="0ms" class="permissions-tabs">
          @for (group of groupedPermissions(); track group.resource) {
            <mat-tab>
              <ng-template mat-tab-label>
                <span class="tab-label">
                  {{ group.resource }}
                  <span class="tab-badge" [class]="getBadgeClass(group)">
                    {{ getSelectedCount(group) }}/{{ group.permissions.length }}
                  </span>
                </span>
              </ng-template>
              
              <div class="tab-content">
                <table mat-table [dataSource]="filterPermissions(group.permissions)">
                  <!-- Permission Name Column -->
                  <ng-container matColumnDef="name">
                    <th mat-header-cell *matHeaderCellDef>Permission Name</th>
                    <td mat-cell *matCellDef="let permission">
                      <span [class.highlight]="shouldHighlight(permission.name)">
                        {{ permission.name }}
                      </span>
                    </td>
                  </ng-container>

                  <!-- Description Column -->
                  <ng-container matColumnDef="description">
                    <th mat-header-cell *matHeaderCellDef>Description</th>
                    <td mat-cell *matCellDef="let permission">
                      <span [class.highlight]="shouldHighlight(permission.description)">
                        {{ permission.description }}
                      </span>
                    </td>
                  </ng-container>

                  <!-- Status Column -->
                  <ng-container matColumnDef="enabled">
                    <th mat-header-cell *matHeaderCellDef>Status</th>
                    <td mat-cell *matCellDef="let permission">
                      <mat-checkbox 
                        [(ngModel)]="permission.enabled"
                        [disabled]="!canEdit()"
                        color="primary">
                      </mat-checkbox>
                    </td>
                  </ng-container>

                  <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
                  <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>

                  <!-- No Data Row -->
                  <tr class="mat-row" *matNoDataRow>
                    <td class="mat-cell no-data-cell" [attr.colspan]="displayedColumns.length">
                      <div class="no-data">
                        <mat-icon>search_off</mat-icon>
                        <p>No permissions found</p>
                      </div>
                    </td>
                  </tr>
                </table>
              </div>
            </mat-tab>
          }
        </mat-tab-group>
      }
    </div>
  `,
  styles: [`
    .permissions-container {
      padding: 24px;
    }

    .loading {
      display: flex;
      justify-content: center;
      align-items: center;
      min-height: 400px;
    }

    .toolbar {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 24px;
      gap: 16px;
      flex-wrap: wrap;
    }

    .toolbar-actions {
      display: flex;
      gap: 12px;
    }

    .search-container {
      flex: 1;
      max-width: 300px;
    }

    .search-field {
      width: 100%;
    }

    .permissions-tabs {
      background: var(--surface-color);
      border-radius: 8px;
      overflow: hidden;
      box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
    }

    .tab-label {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .tab-badge {
      font-size: 11px;
      padding: 2px 8px;
      border-radius: 12px;
      font-weight: 500;
    }

    .tab-badge.none {
      background: #fee2e2;
      color: #991b1b;
    }

    .tab-badge.partial {
      background: #dbeafe;
      color: #1e40af;
    }

    .tab-badge.all {
      background: #dcfce7;
      color: #166534;
    }

    .tab-content {
      padding: 16px;
    }

    table {
      width: 100%;
    }

    .highlight {
      background: #fef08a;
      padding: 2px 4px;
      border-radius: 2px;
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

    @media (max-width: 600px) {
      .toolbar {
        flex-direction: column;
        align-items: stretch;
      }

      .search-container {
        max-width: none;
      }
    }
  `]
})
export class RolePermissionsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private notification = inject(NotificationService);
  private roleService = inject(RoleService);
  private permissionService = inject(PermissionService);
  private authService = inject(AuthService);

  roleId = signal('');
  roleName = signal('');
  allPermissions = signal<PermissionViewModel[]>([]);
  isLoading = signal(true);
  isSaving = signal(false);
  searchString = '';

  displayedColumns = ['name', 'description', 'enabled'];

  title = computed(() => `${this.roleName()} Permissions`);
  description = computed(() => `Manage ${this.roleName()} Role Permissions`);

  canEdit = computed(() => 
    this.permissionService.canUpdate(FshResources.RoleClaims)
  );

  canSearch = computed(() => 
    this.permissionService.canView(FshResources.RoleClaims)
  );

  groupedPermissions = computed(() => {
    const permissions = this.allPermissions();
    return groupPermissionsByResource(permissions);
  });

  ngOnInit(): void {
    const roleId = this.route.snapshot.paramMap.get('id');
    if (roleId) {
      this.roleId.set(roleId);
      this.loadRolePermissions(roleId);
    }
  }

  loadRolePermissions(roleId: string): void {
    this.isLoading.set(true);

    this.roleService.getRolePermissions(roleId).subscribe({
      next: (rolePerms) => {
        this.roleName.set(rolePerms.roleName || 'Role');
        this.initializePermissions(rolePerms.permissions || []);
        this.isLoading.set(false);
      },
      error: () => {
        // Mock data for demo
        this.roleName.set('Admin');
        this.initializePermissions(['Permissions.Users.View', 'Permissions.Users.Create', 'Permissions.Roles.View']);
        this.isLoading.set(false);
      }
    });
  }

  private initializePermissions(enabledPermissions: string[]): void {
    // Get all available permissions based on tenant
    const tenant = this.authService.getTenant();
    const availablePermissions = getAllPermissions();

    // Filter permissions based on tenant (root sees all, others see non-root)
    const filteredPermissions = tenant === 'root' 
      ? availablePermissions 
      : availablePermissions.filter(p => !p.isRoot);

    // Map to view model with enabled status
    const permissionViewModels: PermissionViewModel[] = filteredPermissions.map(p => ({
      ...p,
      enabled: enabledPermissions.includes(p.name)
    }));

    this.allPermissions.set(permissionViewModels);
  }

  filterPermissions(permissions: Permission[]): PermissionViewModel[] {
    const search = this.searchString.toLowerCase();
    if (!search) return permissions as PermissionViewModel[];
    
    return (permissions as PermissionViewModel[]).filter(p =>
      p.name.toLowerCase().includes(search) ||
      p.description.toLowerCase().includes(search)
    );
  }

  shouldHighlight(text: string): boolean {
    if (!text || !this.searchString) return false;
    return text.toLowerCase().includes(this.searchString.toLowerCase());
  }

  getSelectedCount(group: PermissionGroup): number {
    return (group.permissions as PermissionViewModel[]).filter(p => p.enabled).length;
  }

  getBadgeClass(group: PermissionGroup): string {
    const selected = this.getSelectedCount(group);
    const total = group.permissions.length;
    
    if (selected === 0) return 'none';
    if (selected === total) return 'all';
    return 'partial';
  }

  savePermissions(): void {
    this.isSaving.set(true);

    const selectedPermissions = this.allPermissions()
      .filter(p => p.enabled)
      .map(p => p.name);

    const request: UpdatePermissionsRequest = {
      roleId: this.roleId(),
      permissions: selectedPermissions
    };

    this.roleService.updateRolePermissions(this.roleId(), request).subscribe({
      next: () => {
        this.notification.success('Permissions updated successfully');
        this.router.navigate(['/identity/roles']);
        this.isSaving.set(false);
      },
      error: () => {
        this.notification.error('Failed to update permissions');
        this.isSaving.set(false);
      }
    });
  }
}

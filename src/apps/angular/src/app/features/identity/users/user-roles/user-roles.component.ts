import { Component, inject, signal, OnInit, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { NotificationService } from '@core/services/notification.service';
import { UserService } from '@core/services/user.service';
import { PermissionService } from '@core/services/permission.service';
import { User } from '@core/models/user.model';
import { UserRole, AssignUserRolesRequest } from '@core/models/role.model';
import { FshActions, FshResources } from '@core/models/permission.model';

@Component({
  selector: 'app-user-roles',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatInputModule,
    PageHeaderComponent
  ],
  template: `
    <div class="user-roles-container">
      <app-page-header 
        [title]="title()" 
        [subtitle]="description()"
        icon="admin_panel_settings">
      </app-page-header>

      @if (isLoading()) {
        <div class="loading">
          <mat-spinner diameter="48"></mat-spinner>
        </div>
      } @else {
        <!-- Toolbar -->
        <div class="toolbar">
          <div class="toolbar-actions">
            <button mat-stroked-button routerLink="/identity/users">
              <mat-icon>arrow_back</mat-icon>
              Back
            </button>
            @if (canEdit()) {
              <button mat-raised-button color="primary" (click)="saveRoles()" [disabled]="isSaving()">
                @if (isSaving()) {
                  <mat-spinner diameter="20"></mat-spinner>
                }
                <mat-icon>save</mat-icon>
                Update
              </button>
            }
          </div>
          <div class="search-container">
            @if (canSearch()) {
              <mat-form-field appearance="outline" class="search-field">
                <mat-label>Search Roles</mat-label>
                <input matInput 
                       [(ngModel)]="searchString"
                       placeholder="Search for user roles...">
                <mat-icon matPrefix>search</mat-icon>
              </mat-form-field>
            }
          </div>
        </div>

        <!-- Roles Table -->
        <div class="table-container">
          <table mat-table [dataSource]="filteredRoles()">
            <!-- Role Name Column -->
            <ng-container matColumnDef="roleName">
              <th mat-header-cell *matHeaderCellDef>Role Name</th>
              <td mat-cell *matCellDef="let role">
                <span [class.highlight]="shouldHighlight(role.roleName)">
                  {{ role.roleName }}
                </span>
              </td>
            </ng-container>

            <!-- Description Column -->
            <ng-container matColumnDef="description">
              <th mat-header-cell *matHeaderCellDef>Description</th>
              <td mat-cell *matCellDef="let role">
                <span [class.highlight]="shouldHighlight(role.description)">
                  {{ role.description || '-' }}
                </span>
              </td>
            </ng-container>

            <!-- Status Column -->
            <ng-container matColumnDef="enabled">
              <th mat-header-cell *matHeaderCellDef>Status</th>
              <td mat-cell *matCellDef="let role">
                <mat-checkbox 
                  [(ngModel)]="role.enabled"
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
                  <p>No roles found</p>
                </div>
              </td>
            </tr>
          </table>
        </div>
      }
    </div>
  `,
  styles: [`
    .user-roles-container {
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

    .table-container {
      background: var(--surface-color);
      border-radius: 8px;
      overflow: hidden;
      box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
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
export class UserRolesComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private notification = inject(NotificationService);
  private userService = inject(UserService);
  private permissionService = inject(PermissionService);

  user = signal<User | null>(null);
  userRoles = signal<UserRole[]>([]);
  isLoading = signal(true);
  isSaving = signal(false);
  searchString = '';

  displayedColumns = ['roleName', 'description', 'enabled'];

  title = computed(() => {
    const u = this.user();
    return u ? `${u.firstName} ${u.lastName}'s Roles` : 'User Roles';
  });

  description = computed(() => {
    const u = this.user();
    return u ? `Manage ${u.firstName} ${u.lastName}'s Roles` : 'Manage user roles';
  });

  canEdit = computed(() => 
    this.permissionService.canUpdate(FshResources.Users)
  );

  canSearch = computed(() => 
    this.permissionService.canView(FshResources.UserRoles)
  );

  filteredRoles = computed(() => {
    const search = this.searchString.toLowerCase();
    if (!search) return this.userRoles();
    
    return this.userRoles().filter(role =>
      role.roleName?.toLowerCase().includes(search) ||
      role.description?.toLowerCase().includes(search)
    );
  });

  ngOnInit(): void {
    const userId = this.route.snapshot.paramMap.get('id');
    if (userId) {
      this.loadData(userId);
    }
  }

  loadData(userId: string): void {
    this.isLoading.set(true);
    
    // Load user first
    this.userService.getUser(userId).subscribe({
      next: (user) => {
        this.user.set(user);
        this.loadUserRoles(userId);
      },
      error: () => {
        // Mock user for demo
        this.user.set({
          id: userId,
          firstName: 'John',
          lastName: 'Doe',
          email: 'john.doe@example.com',
          userName: 'johndoe',
          isActive: true
        });
        this.loadUserRoles(userId);
      }
    });
  }

  loadUserRoles(userId: string): void {
    this.userService.getUserRoles(userId).subscribe({
      next: (roles) => {
        this.userRoles.set(roles);
        this.isLoading.set(false);
      },
      error: () => {
        // Mock roles for demo
        this.userRoles.set([
          { roleId: '1', roleName: 'Admin', description: 'Full system access', enabled: true },
          { roleId: '2', roleName: 'Basic', description: 'Basic user access', enabled: false },
          { roleId: '3', roleName: 'Manager', description: 'Management access', enabled: true }
        ]);
        this.isLoading.set(false);
      }
    });
  }

  shouldHighlight(text: string | undefined): boolean {
    if (!text || !this.searchString) return false;
    return text.toLowerCase().includes(this.searchString.toLowerCase());
  }

  saveRoles(): void {
    const userId = this.route.snapshot.paramMap.get('id');
    if (!userId) return;

    this.isSaving.set(true);

    const request: AssignUserRolesRequest = {
      userRoles: this.userRoles()
    };

    this.userService.assignRolesToUser(userId, request).subscribe({
      next: () => {
        this.notification.success('User roles updated');
        this.router.navigate(['/identity/users']);
        this.isSaving.set(false);
      },
      error: () => {
        this.notification.error('Failed to update user roles');
        this.isSaving.set(false);
      }
    });
  }
}

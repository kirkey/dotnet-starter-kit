import { Component, inject, signal, OnInit, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatChipsModule } from '@angular/material/chips';
import { MatDividerModule } from '@angular/material/divider';
import { NotificationService } from '@core/services/notification.service';
import { RoleService } from '@core/services/role.service';
import { AuthService } from '@core/services/auth.service';
import { 
  Permission, 
  PermissionGroup, 
  FshActions,
  getAllPermissions,
  groupPermissionsByResource
} from '@core/models/permission.model';
import { UpdatePermissionsRequest } from '@core/models/role.model';

interface PermissionViewModel extends Permission {
  enabled: boolean;
}

interface DialogData {
  roleId: string;
  roleName: string;
}

@Component({
  selector: 'app-permission-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatTabsModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatInputModule,
    MatChipsModule,
    MatDividerModule
  ],
  template: `
    <div class="permission-dialog">
      <h2 mat-dialog-title>
        <mat-icon>security</mat-icon>
        {{ data.roleName }} Permissions
      </h2>
      
      <mat-dialog-content>
        @if (isLoading()) {
          <div class="loading">
            <mat-spinner diameter="48"></mat-spinner>
            <p>Loading permissions...</p>
          </div>
        } @else {
          <!-- Search -->
          <div class="search-container">
            <mat-form-field appearance="outline" class="search-field">
              <mat-label>Search Permissions</mat-label>
              <input matInput 
                     [(ngModel)]="searchQuery"
                     placeholder="Search...">
              <mat-icon matPrefix>search</mat-icon>
              @if (searchQuery) {
                <button matSuffix mat-icon-button (click)="searchQuery = ''">
                  <mat-icon>close</mat-icon>
                </button>
              }
            </mat-form-field>
          </div>

          <!-- Tabs by Resource -->
          <mat-tab-group animationDuration="200ms" class="permissions-tabs">
            @for (group of filteredGroups(); track group.resource) {
              <mat-tab>
                <ng-template mat-tab-label>
                  <div class="tab-label">
                    <span>{{ group.resource }}</span>
                    <span class="badge" [class]="getBadgeClass(group)">
                      {{ getSelectedCount(group) }}/{{ group.permissions.length }}
                    </span>
                  </div>
                </ng-template>
                
                <div class="tab-content">
                  <!-- Select All for this resource -->
                  <div class="select-all-row">
                    <mat-checkbox 
                      [checked]="isAllSelected(group)"
                      [indeterminate]="isSomeSelected(group)"
                      (change)="toggleAllInGroup(group, $event.checked)"
                      color="primary">
                      <strong>Select All {{ group.resource }}</strong>
                    </mat-checkbox>
                  </div>
                  
                  <mat-divider></mat-divider>
                  
                  <!-- Permission checkboxes -->
                  <div class="permissions-grid">
                    @for (permission of getPermissionsAsViewModel(group); track permission.name) {
                      <div class="permission-item" [class.highlighted]="shouldHighlight(permission)">
                        <mat-checkbox 
                          [(ngModel)]="permission.enabled"
                          color="primary">
                          <div class="permission-label">
                            <mat-icon class="action-icon" [class]="getActionClass(permission.action)">
                              {{ getActionIcon(permission.action) }}
                            </mat-icon>
                            <span class="action-name">{{ permission.action }}</span>
                          </div>
                        </mat-checkbox>
                        <span class="permission-description">{{ permission.description }}</span>
                      </div>
                    }
                  </div>
                </div>
              </mat-tab>
            }
          </mat-tab-group>
        }
      </mat-dialog-content>

      <mat-dialog-actions align="end">
        <div class="dialog-summary">
          <span class="summary-text">
            {{ getTotalSelected() }} of {{ getTotalPermissions() }} permissions selected
          </span>
        </div>
        <button mat-button mat-dialog-close [disabled]="isSaving()">Cancel</button>
        <button mat-raised-button 
                color="primary" 
                (click)="save()"
                [disabled]="isSaving()">
          @if (isSaving()) {
            <mat-spinner diameter="20"></mat-spinner>
          } @else {
            <mat-icon>save</mat-icon>
          }
          Save Permissions
        </button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .permission-dialog {
      min-width: 600px;
      max-width: 800px;
    }

    h2[mat-dialog-title] {
      display: flex;
      align-items: center;
      gap: 12px;
      margin: 0;
      padding: 16px 24px;
      background: linear-gradient(135deg, #6366f1, #8b5cf6);
      color: white;
      border-radius: 4px 4px 0 0;
    }

    h2[mat-dialog-title] mat-icon {
      font-size: 28px;
      width: 28px;
      height: 28px;
    }

    mat-dialog-content {
      padding: 24px !important;
      max-height: 60vh;
      overflow-y: auto;
    }

    .loading {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 48px;
      gap: 16px;
      color: #666;
    }

    .search-container {
      margin-bottom: 16px;
    }

    .search-field {
      width: 100%;
    }

    .permissions-tabs {
      border: 1px solid #e0e0e0;
      border-radius: 8px;
      overflow: hidden;
    }

    .tab-label {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .badge {
      font-size: 10px;
      padding: 2px 6px;
      border-radius: 10px;
      font-weight: 600;
    }

    .badge.none {
      background: #fee2e2;
      color: #991b1b;
    }

    .badge.partial {
      background: #dbeafe;
      color: #1e40af;
    }

    .badge.all {
      background: #dcfce7;
      color: #166534;
    }

    .tab-content {
      padding: 16px;
    }

    .select-all-row {
      padding: 8px 0;
    }

    mat-divider {
      margin: 12px 0;
    }

    .permissions-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
      gap: 12px;
    }

    .permission-item {
      display: flex;
      flex-direction: column;
      padding: 12px;
      border: 1px solid #e5e7eb;
      border-radius: 8px;
      background: #fafafa;
      transition: all 0.2s;
    }

    .permission-item:hover {
      border-color: #6366f1;
      background: #f5f3ff;
    }

    .permission-item.highlighted {
      background: #fef3c7;
      border-color: #f59e0b;
    }

    .permission-label {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .action-icon {
      font-size: 18px;
      width: 18px;
      height: 18px;
    }

    .action-icon.view { color: #3b82f6; }
    .action-icon.create { color: #10b981; }
    .action-icon.update { color: #f59e0b; }
    .action-icon.delete { color: #ef4444; }
    .action-icon.search { color: #8b5cf6; }
    .action-icon.export { color: #06b6d4; }

    .action-name {
      font-weight: 500;
    }

    .permission-description {
      font-size: 11px;
      color: #666;
      margin-top: 4px;
      margin-left: 26px;
    }

    mat-dialog-actions {
      padding: 16px 24px !important;
      border-top: 1px solid #e0e0e0;
      gap: 12px;
    }

    .dialog-summary {
      flex: 1;
    }

    .summary-text {
      font-size: 13px;
      color: #666;
    }

    @media (max-width: 768px) {
      .permission-dialog {
        min-width: auto;
        width: 100%;
      }

      .permissions-grid {
        grid-template-columns: 1fr;
      }
    }
  `]
})
export class PermissionDialogComponent implements OnInit {
  private dialogRef = inject(MatDialogRef<PermissionDialogComponent>);
  public data: DialogData = inject(MAT_DIALOG_DATA);
  private notification = inject(NotificationService);
  private roleService = inject(RoleService);
  private authService = inject(AuthService);

  isLoading = signal(true);
  isSaving = signal(false);
  allPermissions = signal<PermissionViewModel[]>([]);
  searchQuery = '';

  groupedPermissions = computed(() => {
    return groupPermissionsByResource(this.allPermissions());
  });

  filteredGroups = computed(() => {
    const groups = this.groupedPermissions();
    if (!this.searchQuery) return groups;
    
    const query = this.searchQuery.toLowerCase();
    return groups.map(group => ({
      ...group,
      permissions: group.permissions.filter(p => 
        p.name.toLowerCase().includes(query) ||
        p.action.toLowerCase().includes(query) ||
        p.resource.toLowerCase().includes(query) ||
        p.description.toLowerCase().includes(query)
      )
    })).filter(g => g.permissions.length > 0);
  });

  ngOnInit(): void {
    this.loadPermissions();
  }

  loadPermissions(): void {
    this.isLoading.set(true);

    this.roleService.getRolePermissions(this.data.roleId).subscribe({
      next: (rolePerms) => {
        this.initializePermissions(rolePerms.permissions || []);
        this.isLoading.set(false);
      },
      error: () => {
        // Initialize with empty permissions on error
        this.initializePermissions([]);
        this.isLoading.set(false);
      }
    });
  }

  private initializePermissions(enabledPermissions: string[]): void {
    const tenant = this.authService.getTenant();
    const availablePermissions = getAllPermissions();

    // Filter permissions based on tenant
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

  shouldHighlight(permission: PermissionViewModel): boolean {
    if (!this.searchQuery) return false;
    const query = this.searchQuery.toLowerCase();
    return permission.name.toLowerCase().includes(query) ||
           permission.action.toLowerCase().includes(query) ||
           permission.description.toLowerCase().includes(query);
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

  isAllSelected(group: PermissionGroup): boolean {
    return (group.permissions as PermissionViewModel[]).every(p => p.enabled);
  }

  isSomeSelected(group: PermissionGroup): boolean {
    const perms = group.permissions as PermissionViewModel[];
    const selected = perms.filter(p => p.enabled).length;
    return selected > 0 && selected < perms.length;
  }

  getPermissionsAsViewModel(group: PermissionGroup): PermissionViewModel[] {
    return group.permissions as PermissionViewModel[];
  }

  toggleAllInGroup(group: PermissionGroup, checked: boolean): void {
    const permissions = this.allPermissions();
    permissions.forEach(p => {
      if (p.resource === group.resource) {
        p.enabled = checked;
      }
    });
    this.allPermissions.set([...permissions]);
  }

  getTotalSelected(): number {
    return this.allPermissions().filter(p => p.enabled).length;
  }

  getTotalPermissions(): number {
    return this.allPermissions().length;
  }

  getActionIcon(action: string): string {
    const icons: Record<string, string> = {
      // Standard CRUD Actions
      [FshActions.View]: 'visibility',
      [FshActions.Create]: 'add_circle',
      [FshActions.Update]: 'edit',
      [FshActions.Delete]: 'delete',
      [FshActions.Search]: 'search',
      [FshActions.Export]: 'download',
      [FshActions.Import]: 'upload',
      [FshActions.Generate]: 'auto_fix_high',
      [FshActions.Clean]: 'cleaning_services',
      [FshActions.UpgradeSubscription]: 'upgrade',
      
      // HR & Employee Actions
      [FshActions.Regularize]: 'verified_user',
      [FshActions.Terminate]: 'person_remove',
      [FshActions.Assign]: 'person_add',
      [FshActions.Manage]: 'settings',
      
      // Workflow & Approval Actions
      [FshActions.Approve]: 'check_circle',
      [FshActions.Reject]: 'cancel',
      [FshActions.Submit]: 'send',
      [FshActions.Process]: 'settings',
      [FshActions.Complete]: 'done_all',
      [FshActions.Cancel]: 'block',
      [FshActions.Void]: 'do_not_disturb',
      [FshActions.Post]: 'post_add',
      [FshActions.Send]: 'send',
      [FshActions.Receive]: 'move_to_inbox',
      [FshActions.Acknowledge]: 'fact_check',
      
      // Financial Actions
      [FshActions.MarkAsPaid]: 'paid',
      [FshActions.Accrue]: 'trending_up',
      [FshActions.Disburse]: 'payments',
      [FshActions.Deposit]: 'account_balance',
      [FshActions.Withdraw]: 'money_off',
      [FshActions.Transfer]: 'swap_horiz',
      [FshActions.WriteOff]: 'money_off_csred',
      [FshActions.Mature]: 'schedule',
      
      // Status Actions
      [FshActions.Activate]: 'check_circle_outline',
      [FshActions.Deactivate]: 'highlight_off',
      [FshActions.Suspend]: 'pause_circle_outline',
      [FshActions.Close]: 'close',
      [FshActions.Freeze]: 'ac_unit',
      [FshActions.Unfreeze]: 'wb_sunny',
      [FshActions.Renew]: 'autorenew',
      [FshActions.Return]: 'keyboard_return',
      
      // MicroFinance - Collections & Recovery Actions
      [FshActions.Escalate]: 'priority_high',
      [FshActions.MarkBroken]: 'broken_image',
      [FshActions.RecordPayment]: 'receipt',
      
      // MicroFinance - AML & Compliance Actions
      [FshActions.FileSar]: 'report_problem',
      [FshActions.Confirm]: 'verified',
      [FshActions.Clear]: 'check',
      
      // MicroFinance - Insurance Actions
      [FshActions.RecordPremium]: 'payment',
      
      // MicroFinance - Investment Actions
      [FshActions.Invest]: 'trending_up',
      [FshActions.Redeem]: 'redeem',
      [FshActions.SetupSip]: 'schedule_send',
      
      // MicroFinance - Agent Banking Actions
      [FshActions.RecordAudit]: 'checklist',
      [FshActions.UpgradeTier]: 'upgrade',
      [FshActions.CreditFloat]: 'add_card',
      [FshActions.DebitFloat]: 'remove_circle',
      
      // MicroFinance - Loan Actions
      [FshActions.ApplyPayment]: 'payment',
      [FshActions.Restructure]: 'transform'
    };
    return icons[action] || 'check';
  }

  getActionClass(action: string): string {
    const classes: Record<string, string> = {
      [FshActions.View]: 'view',
      [FshActions.Create]: 'create',
      [FshActions.Update]: 'update',
      [FshActions.Delete]: 'delete',
      [FshActions.Search]: 'search',
      [FshActions.Export]: 'export'
    };
    return classes[action] || '';
  }

  save(): void {
    this.isSaving.set(true);

    const selectedPermissions = this.allPermissions()
      .filter(p => p.enabled)
      .map(p => p.name);

    const request: UpdatePermissionsRequest = {
      roleId: this.data.roleId,
      permissions: selectedPermissions
    };

    this.roleService.updateRolePermissions(this.data.roleId, request).subscribe({
      next: () => {
        this.notification.success('Permissions updated successfully');
        this.dialogRef.close(true);
      },
      error: () => {
        this.notification.error('Failed to update permissions');
        this.isSaving.set(false);
      }
    });
  }
}

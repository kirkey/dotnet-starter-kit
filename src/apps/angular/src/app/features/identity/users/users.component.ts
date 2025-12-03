import { Component, inject, signal, OnInit, computed, ViewChild, TemplateRef } from '@angular/core';
import { CommonModule } from '@angular/common';
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
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDividerModule } from '@angular/material/divider';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';
import { NotificationService } from '@core/services/notification.service';
import { ApiService } from '@core/services/api.service';
import { User, CreateUserRequest, UpdateUserRequest } from '@core/models/user.model';

@Component({
  selector: 'app-users',
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
    MatSelectModule,
    MatDialogModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatMenuModule,
    MatTooltipModule,
    MatCheckboxModule,
    MatDividerModule,
    PageHeaderComponent
  ],
  template: `
    <div class="users-container">
      <app-page-header 
        title="Users Management" 
        subtitle="Manage system users and their roles"
        icon="people">
      </app-page-header>

      <!-- Toolbar -->
      <div class="toolbar">
        <div class="search-container">
          <mat-form-field appearance="outline" class="search-field">
            <mat-label>Search users</mat-label>
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
          <button mat-stroked-button (click)="refreshUsers()">
            <mat-icon>refresh</mat-icon>
            Refresh
          </button>
          <button mat-raised-button color="primary" (click)="openUserDialog()">
            <mat-icon>person_add</mat-icon>
            Add User
          </button>
        </div>
      </div>

      <!-- Table -->
      <div class="table-container">
        @if (isLoading()) {
          <div class="loading-overlay">
            <mat-spinner diameter="48"></mat-spinner>
          </div>
        }

        <table mat-table [dataSource]="filteredUsers()" matSort (matSortChange)="onSort($event)">
          <!-- Checkbox Column -->
          <ng-container matColumnDef="select">
            <th mat-header-cell *matHeaderCellDef>
              <mat-checkbox 
                [checked]="allSelected()"
                [indeterminate]="someSelected()"
                (change)="toggleAll($event.checked)">
              </mat-checkbox>
            </th>
            <td mat-cell *matCellDef="let user">
              <mat-checkbox 
                [checked]="isSelected(user)"
                (change)="toggleSelection(user)">
              </mat-checkbox>
            </td>
          </ng-container>

          <!-- Avatar Column -->
          <ng-container matColumnDef="avatar">
            <th mat-header-cell *matHeaderCellDef></th>
            <td mat-cell *matCellDef="let user">
              <div class="user-avatar" [style.background-color]="getAvatarColor(user.firstName)">
                {{ getInitials(user.firstName, user.lastName) }}
              </div>
            </td>
          </ng-container>

          <!-- Name Column -->
          <ng-container matColumnDef="name">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Name</th>
            <td mat-cell *matCellDef="let user">
              <div class="user-name">{{ user.firstName }} {{ user.lastName }}</div>
              <div class="user-username">&#64;{{ user.userName }}</div>
            </td>
          </ng-container>

          <!-- Email Column -->
          <ng-container matColumnDef="email">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Email</th>
            <td mat-cell *matCellDef="let user">{{ user.email }}</td>
          </ng-container>

          <!-- Phone Column -->
          <ng-container matColumnDef="phone">
            <th mat-header-cell *matHeaderCellDef>Phone</th>
            <td mat-cell *matCellDef="let user">{{ user.phoneNumber || '-' }}</td>
          </ng-container>

          <!-- Status Column -->
          <ng-container matColumnDef="status">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Status</th>
            <td mat-cell *matCellDef="let user">
              <mat-chip [class]="user.isActive ? 'status-active' : 'status-inactive'">
                {{ user.isActive ? 'Active' : 'Inactive' }}
              </mat-chip>
            </td>
          </ng-container>

          <!-- Roles Column -->
          <ng-container matColumnDef="roles">
            <th mat-header-cell *matHeaderCellDef>Roles</th>
            <td mat-cell *matCellDef="let user">
              <div class="roles-list">
                @for (role of user.roles?.slice(0, 2); track role) {
                  <mat-chip class="role-chip">{{ role }}</mat-chip>
                }
                @if (user.roles?.length > 2) {
                  <span class="more-roles">+{{ user.roles.length - 2 }}</span>
                }
              </div>
            </td>
          </ng-container>

          <!-- Actions Column -->
          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef></th>
            <td mat-cell *matCellDef="let user">
              <button mat-icon-button [matMenuTriggerFor]="menu">
                <mat-icon>more_vert</mat-icon>
              </button>
              <mat-menu #menu="matMenu">
                <button mat-menu-item (click)="openUserDialog(user)">
                  <mat-icon>edit</mat-icon>
                  <span>Edit</span>
                </button>
                <button mat-menu-item (click)="viewUser(user)">
                  <mat-icon>visibility</mat-icon>
                  <span>View Details</span>
                </button>
                <button mat-menu-item (click)="toggleUserStatus(user)">
                  <mat-icon>{{ user.isActive ? 'block' : 'check_circle' }}</mat-icon>
                  <span>{{ user.isActive ? 'Deactivate' : 'Activate' }}</span>
                </button>
                <mat-divider></mat-divider>
                <button mat-menu-item class="delete-action" (click)="deleteUser(user)">
                  <mat-icon>delete</mat-icon>
                  <span>Delete</span>
                </button>
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
                <p>No users found</p>
                @if (searchQuery()) {
                  <button mat-button color="primary" (click)="clearSearch()">Clear search</button>
                }
              </div>
            </td>
          </tr>
        </table>

        <mat-paginator 
          [length]="totalUsers()"
          [pageSize]="pageSize()"
          [pageSizeOptions]="[5, 10, 25, 50]"
          [pageIndex]="pageIndex()"
          (page)="onPageChange($event)"
          showFirstLastButtons>
        </mat-paginator>
      </div>
    </div>

    <!-- User Dialog Template -->
    <ng-template #userDialog>
      <h2 mat-dialog-title>{{ editingUser() ? 'Edit User' : 'Add New User' }}</h2>
      <mat-dialog-content>
        <form [formGroup]="userForm" class="user-form">
          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>First Name</mat-label>
              <input matInput formControlName="firstName" placeholder="First name">
              @if (userForm.get('firstName')?.hasError('required')) {
                <mat-error>First name is required</mat-error>
              }
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Last Name</mat-label>
              <input matInput formControlName="lastName" placeholder="Last name">
              @if (userForm.get('lastName')?.hasError('required')) {
                <mat-error>Last name is required</mat-error>
              }
            </mat-form-field>
          </div>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Email</mat-label>
            <input matInput type="email" formControlName="email" placeholder="Email address">
            @if (userForm.get('email')?.hasError('required')) {
              <mat-error>Email is required</mat-error>
            }
            @if (userForm.get('email')?.hasError('email')) {
              <mat-error>Invalid email format</mat-error>
            }
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Username</mat-label>
            <input matInput formControlName="userName" placeholder="Username">
            @if (userForm.get('userName')?.hasError('required')) {
              <mat-error>Username is required</mat-error>
            }
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Phone Number</mat-label>
            <input matInput type="tel" formControlName="phoneNumber" placeholder="Phone number">
          </mat-form-field>

          @if (!editingUser()) {
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Password</mat-label>
              <input matInput type="password" formControlName="password" placeholder="Password">
              @if (userForm.get('password')?.hasError('required')) {
                <mat-error>Password is required</mat-error>
              }
              @if (userForm.get('password')?.hasError('minlength')) {
                <mat-error>Password must be at least 8 characters</mat-error>
              }
            </mat-form-field>
          }

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Roles</mat-label>
            <mat-select formControlName="roles" multiple>
              <mat-option value="Admin">Admin</mat-option>
              <mat-option value="Manager">Manager</mat-option>
              <mat-option value="User">User</mat-option>
              <mat-option value="Viewer">Viewer</mat-option>
            </mat-select>
          </mat-form-field>
        </form>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button mat-dialog-close>Cancel</button>
        <button mat-raised-button 
                color="primary" 
                [disabled]="userForm.invalid || isSaving()"
                (click)="saveUser()">
          @if (isSaving()) {
            <mat-spinner diameter="20"></mat-spinner>
          }
          {{ editingUser() ? 'Update' : 'Create' }}
        </button>
      </mat-dialog-actions>
    </ng-template>
  `,
  styles: [`
    .users-container {
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

    .user-avatar {
      width: 40px;
      height: 40px;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
      color: white;
      font-weight: 500;
      font-size: 14px;
    }

    .user-name {
      font-weight: 500;
    }

    .user-username {
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

    .roles-list {
      display: flex;
      gap: 4px;
      align-items: center;
    }

    .role-chip {
      font-size: 11px;
      min-height: 24px;
    }

    .more-roles {
      font-size: 12px;
      color: var(--text-secondary);
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
    .user-form {
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

      .user-form {
        min-width: auto;
      }

      .form-row {
        grid-template-columns: 1fr;
      }
    }
  `]
})
export class UsersComponent implements OnInit {
  @ViewChild('userDialog') userDialogTemplate!: TemplateRef<any>;

  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private notification = inject(NotificationService);
  private apiService = inject(ApiService);

  // State signals
  users = signal<User[]>([]);
  isLoading = signal(false);
  isSaving = signal(false);
  searchQuery = signal('');
  selectedUsers = signal<Set<string>>(new Set());
  editingUser = signal<User | null>(null);

  // Pagination signals
  pageIndex = signal(0);
  pageSize = signal(10);
  totalUsers = signal(0);

  displayedColumns = ['select', 'avatar', 'name', 'email', 'phone', 'status', 'roles', 'actions'];

  userForm: FormGroup = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    userName: ['', Validators.required],
    phoneNumber: [''],
    password: ['', [Validators.required, Validators.minLength(8)]],
    roles: [['User']]
  });

  // Computed signals
  filteredUsers = computed(() => {
    const query = this.searchQuery().toLowerCase();
    if (!query) return this.users();
    
    return this.users().filter(user => 
      user.firstName?.toLowerCase().includes(query) ||
      user.lastName?.toLowerCase().includes(query) ||
      user.email?.toLowerCase().includes(query) ||
      user.userName?.toLowerCase().includes(query)
    );
  });

  allSelected = computed(() => {
    const users = this.filteredUsers();
    const selected = this.selectedUsers();
    return users.length > 0 && users.every(u => selected.has(u.id));
  });

  someSelected = computed(() => {
    const users = this.filteredUsers();
    const selected = this.selectedUsers();
    return users.some(u => selected.has(u.id)) && !this.allSelected();
  });

  ngOnInit(): void {
    this.loadUsers();
  }

  async loadUsers(): Promise<void> {
    this.isLoading.set(true);
    
    try {
      // Simulated data - replace with actual API call
      const mockUsers: User[] = [
        { id: '1', firstName: 'John', lastName: 'Doe', email: 'john.doe@example.com', userName: 'johndoe', phoneNumber: '+1234567890', isActive: true, roles: ['Admin', 'Manager'] },
        { id: '2', firstName: 'Jane', lastName: 'Smith', email: 'jane.smith@example.com', userName: 'janesmith', phoneNumber: '+0987654321', isActive: true, roles: ['User'] },
        { id: '3', firstName: 'Bob', lastName: 'Wilson', email: 'bob.wilson@example.com', userName: 'bobwilson', isActive: false, roles: ['Viewer'] },
        { id: '4', firstName: 'Alice', lastName: 'Brown', email: 'alice.brown@example.com', userName: 'alicebrown', phoneNumber: '+1122334455', isActive: true, roles: ['Manager'] },
        { id: '5', firstName: 'Charlie', lastName: 'Davis', email: 'charlie.davis@example.com', userName: 'charlied', isActive: true, roles: ['User', 'Viewer'] }
      ];
      
      await new Promise(resolve => setTimeout(resolve, 500));
      this.users.set(mockUsers);
      this.totalUsers.set(mockUsers.length);
    } catch (error) {
      this.notification.error('Failed to load users');
    } finally {
      this.isLoading.set(false);
    }
  }

  refreshUsers(): void {
    this.loadUsers();
  }

  onSearch(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.searchQuery.set(value);
  }

  clearSearch(): void {
    this.searchQuery.set('');
  }

  onSort(sort: Sort): void {
    // Implement sorting logic
    console.log('Sort:', sort);
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex.set(event.pageIndex);
    this.pageSize.set(event.pageSize);
  }

  isSelected(user: User): boolean {
    return this.selectedUsers().has(user.id);
  }

  toggleSelection(user: User): void {
    const selected = new Set(this.selectedUsers());
    if (selected.has(user.id)) {
      selected.delete(user.id);
    } else {
      selected.add(user.id);
    }
    this.selectedUsers.set(selected);
  }

  toggleAll(checked: boolean): void {
    if (checked) {
      const allIds = new Set(this.filteredUsers().map(u => u.id));
      this.selectedUsers.set(allIds);
    } else {
      this.selectedUsers.set(new Set());
    }
  }

  openUserDialog(user?: User): void {
    this.editingUser.set(user || null);
    
    if (user) {
      this.userForm.patchValue({
        firstName: user.firstName,
        lastName: user.lastName,
        email: user.email,
        userName: user.userName,
        phoneNumber: user.phoneNumber || '',
        roles: user.roles || ['User']
      });
      this.userForm.get('password')?.clearValidators();
    } else {
      this.userForm.reset({ roles: ['User'] });
      this.userForm.get('password')?.setValidators([Validators.required, Validators.minLength(8)]);
    }
    this.userForm.get('password')?.updateValueAndValidity();

    this.dialog.open(this.userDialogTemplate, {
      width: '500px'
    });
  }

  async saveUser(): Promise<void> {
    if (this.userForm.invalid) return;

    this.isSaving.set(true);
    
    try {
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      if (this.editingUser()) {
        this.notification.success('User updated successfully');
      } else {
        this.notification.success('User created successfully');
      }
      
      this.dialog.closeAll();
      this.loadUsers();
    } catch (error) {
      this.notification.error('Failed to save user');
    } finally {
      this.isSaving.set(false);
    }
  }

  viewUser(user: User): void {
    // Implement view details
    console.log('View user:', user);
  }

  async toggleUserStatus(user: User): Promise<void> {
    try {
      user.isActive = !user.isActive;
      this.notification.success(`User ${user.isActive ? 'activated' : 'deactivated'} successfully`);
    } catch (error) {
      this.notification.error('Failed to update user status');
    }
  }

  async deleteUser(user: User): Promise<void> {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete User',
        message: `Are you sure you want to delete ${user.firstName} ${user.lastName}?`,
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(async result => {
      if (result) {
        try {
          await new Promise(resolve => setTimeout(resolve, 500));
          this.users.update(users => users.filter(u => u.id !== user.id));
          this.notification.success('User deleted successfully');
        } catch (error) {
          this.notification.error('Failed to delete user');
        }
      }
    });
  }

  getInitials(firstName: string, lastName: string): string {
    return `${firstName?.charAt(0) || ''}${lastName?.charAt(0) || ''}`.toUpperCase();
  }

  getAvatarColor(name: string): string {
    const colors = [
      '#f87171', '#fb923c', '#fbbf24', '#a3e635',
      '#34d399', '#22d3d8', '#60a5fa', '#a78bfa',
      '#f472b6', '#e879f9'
    ];
    const index = (name?.charCodeAt(0) || 0) % colors.length;
    return colors[index];
  }
}

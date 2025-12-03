import { Component, inject, signal, OnInit, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatDividerModule } from '@angular/material/divider';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { NotificationService } from '@core/services/notification.service';
import { UserService } from '@core/services/user.service';
import { PermissionService } from '@core/services/permission.service';
import { User } from '@core/models/user.model';
import { FshActions, FshResources } from '@core/models/permission.model';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatDividerModule,
    MatSlideToggleModule,
    PageHeaderComponent
  ],
  template: `
    <div class="profile-container">
      <app-page-header 
        [title]="title()" 
        [subtitle]="user()?.email || ''"
        icon="person">
        <button mat-stroked-button routerLink="/identity/users">
          <mat-icon>arrow_back</mat-icon>
          Back to Users
        </button>
      </app-page-header>

      @if (isLoading()) {
        <div class="loading">
          <mat-spinner diameter="48"></mat-spinner>
        </div>
      } @else if (user()) {
        <div class="profile-grid">
          <!-- Admin Settings Card -->
          @if (canToggleStatus()) {
            <mat-card class="admin-card" appearance="outlined">
              <mat-card-header>
                <mat-card-title>Administrator Settings</mat-card-title>
                <mat-card-subtitle>This is an Administrator Only View</mat-card-subtitle>
              </mat-card-header>
              <mat-card-content>
                <div class="admin-controls">
                  <mat-slide-toggle 
                    [checked]="user()?.isActive"
                    (change)="onStatusChange($event.checked)"
                    color="primary">
                    Active
                  </mat-slide-toggle>
                  <mat-slide-toggle 
                    [checked]="user()?.emailConfirmed"
                    [disabled]="true"
                    color="accent">
                    Email Confirmed
                  </mat-slide-toggle>
                  <button mat-raised-button color="primary" (click)="saveStatus()">
                    Save Changes
                  </button>
                </div>
              </mat-card-content>
            </mat-card>
          }

          <!-- Avatar Card -->
          <mat-card class="avatar-card" appearance="outlined">
            <mat-card-content>
              <div class="avatar-section">
                @if (user()?.imageUrl) {
                  <img [src]="user()?.imageUrl" alt="Profile" class="avatar-image">
                } @else {
                  <div class="avatar-placeholder" [style.background-color]="getAvatarColor()">
                    {{ getInitials() }}
                  </div>
                }
                <h2 class="user-name">{{ user()?.firstName }} {{ user()?.lastName }}</h2>
                <p class="user-email">{{ user()?.email }}</p>
                @if (user()?.imageUrl) {
                  <a mat-stroked-button [href]="user()?.imageUrl" target="_blank">
                    <mat-icon>visibility</mat-icon>
                    View Image
                  </a>
                }
              </div>
            </mat-card-content>
          </mat-card>

          <!-- Profile Details Card -->
          <mat-card class="details-card" appearance="outlined">
            <mat-card-header>
              <mat-card-title>Public Profile</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <div class="details-grid">
                <div class="detail-item">
                  <span class="detail-label">First Name</span>
                  <span class="detail-value">{{ user()?.firstName || '-' }}</span>
                </div>
                <div class="detail-item">
                  <span class="detail-label">Last Name</span>
                  <span class="detail-value">{{ user()?.lastName || '-' }}</span>
                </div>
                <div class="detail-item">
                  <span class="detail-label">Phone Number</span>
                  <span class="detail-value">{{ user()?.phoneNumber || '-' }}</span>
                </div>
                <div class="detail-item">
                  <span class="detail-label">Email</span>
                  <span class="detail-value">{{ user()?.email || '-' }}</span>
                </div>
                <div class="detail-item">
                  <span class="detail-label">Username</span>
                  <span class="detail-value">{{ user()?.userName || '-' }}</span>
                </div>
                <div class="detail-item">
                  <span class="detail-label">Status</span>
                  <mat-chip [class]="user()?.isActive ? 'status-active' : 'status-inactive'">
                    {{ user()?.isActive ? 'Active' : 'Inactive' }}
                  </mat-chip>
                </div>
              </div>
            </mat-card-content>
          </mat-card>
        </div>
      }
    </div>
  `,
  styles: [`
    .profile-container {
      padding: 24px;
    }

    .loading {
      display: flex;
      justify-content: center;
      align-items: center;
      min-height: 400px;
    }

    .profile-grid {
      display: grid;
      grid-template-columns: 1fr 2fr;
      gap: 24px;
    }

    .admin-card {
      grid-column: 1 / -1;
    }

    .admin-controls {
      display: flex;
      align-items: center;
      gap: 24px;
      padding: 16px 0;
    }

    .avatar-card {
      grid-row: span 1;
    }

    .avatar-section {
      display: flex;
      flex-direction: column;
      align-items: center;
      padding: 24px;
      text-align: center;
    }

    .avatar-image {
      width: 150px;
      height: 150px;
      border-radius: 8px;
      object-fit: cover;
      margin-bottom: 16px;
    }

    .avatar-placeholder {
      width: 150px;
      height: 150px;
      border-radius: 8px;
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 48px;
      font-weight: 500;
      color: white;
      margin-bottom: 16px;
    }

    .user-name {
      margin: 0;
      font-size: 20px;
      font-weight: 500;
    }

    .user-email {
      margin: 4px 0 16px;
      color: var(--text-secondary);
    }

    .details-card {
      grid-column: 2;
    }

    .details-grid {
      display: grid;
      grid-template-columns: repeat(2, 1fr);
      gap: 24px;
      padding: 16px 0;
    }

    .detail-item {
      display: flex;
      flex-direction: column;
      gap: 4px;
    }

    .detail-label {
      font-size: 12px;
      color: var(--text-secondary);
      font-weight: 500;
    }

    .detail-value {
      font-size: 14px;
    }

    .status-active {
      background: #dcfce7 !important;
      color: #166534 !important;
    }

    .status-inactive {
      background: #fee2e2 !important;
      color: #991b1b !important;
    }

    @media (max-width: 768px) {
      .profile-grid {
        grid-template-columns: 1fr;
      }

      .avatar-card,
      .details-card {
        grid-column: 1;
      }

      .details-grid {
        grid-template-columns: 1fr;
      }

      .admin-controls {
        flex-direction: column;
        align-items: flex-start;
      }
    }
  `]
})
export class UserProfileComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private notification = inject(NotificationService);
  private userService = inject(UserService);
  private permissionService = inject(PermissionService);

  user = signal<User | null>(null);
  isLoading = signal(true);
  private activeStatus = signal(false);

  title = computed(() => {
    const u = this.user();
    return u ? `${u.firstName} ${u.lastName}'s Profile` : 'User Profile';
  });

  canToggleStatus = computed(() => 
    this.permissionService.canUpdate(FshResources.Users)
  );

  ngOnInit(): void {
    const userId = this.route.snapshot.paramMap.get('id');
    if (userId) {
      this.loadUser(userId);
    }
  }

  loadUser(id: string): void {
    this.isLoading.set(true);
    this.userService.getUser(id).subscribe({
      next: (user) => {
        this.user.set(user);
        this.activeStatus.set(user.isActive);
        this.isLoading.set(false);
      },
      error: () => {
        this.notification.error('Failed to load user profile');
        this.isLoading.set(false);
        // Mock data for demo
        this.user.set({
          id,
          firstName: 'John',
          lastName: 'Doe',
          email: 'john.doe@example.com',
          userName: 'johndoe',
          phoneNumber: '+1234567890',
          isActive: true,
          emailConfirmed: true
        });
        this.activeStatus.set(true);
      }
    });
  }

  onStatusChange(active: boolean): void {
    this.activeStatus.set(active);
  }

  saveStatus(): void {
    const u = this.user();
    if (!u) return;

    this.userService.toggleUserStatus(u.id, this.activeStatus()).subscribe({
      next: () => {
        this.user.update(user => user ? { ...user, isActive: this.activeStatus() } : null);
        this.notification.success('User status updated');
        this.router.navigate(['/identity/users']);
      },
      error: () => {
        this.notification.error('Failed to update user status');
      }
    });
  }

  getInitials(): string {
    const u = this.user();
    if (!u) return '';
    return `${u.firstName?.charAt(0) || ''}${u.lastName?.charAt(0) || ''}`.toUpperCase();
  }

  getAvatarColor(): string {
    const colors = [
      '#f87171', '#fb923c', '#fbbf24', '#a3e635',
      '#34d399', '#22d3d8', '#60a5fa', '#a78bfa',
      '#f472b6', '#e879f9'
    ];
    const name = this.user()?.firstName || '';
    const index = (name.charCodeAt(0) || 0) % colors.length;
    return colors[index];
  }
}

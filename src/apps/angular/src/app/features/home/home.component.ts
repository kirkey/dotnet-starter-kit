import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatListModule } from '@angular/material/list';
import { MatChipsModule } from '@angular/material/chips';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { AuthService } from '@core/services/auth.service';

interface DashboardStats {
  totalUsers: number;
  totalProducts: number;
  totalOrders: number;
  revenue: number;
}

interface RecentActivity {
  id: string;
  type: 'user' | 'product' | 'order' | 'system';
  title: string;
  description: string;
  timestamp: string;
  icon: string;
}

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    CurrencyPipe,
    DatePipe,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatListModule,
    MatChipsModule,
    PageHeaderComponent
  ],
  template: `
    <div class="home-container">
      <app-page-header 
        [title]="getGreeting()" 
        [subtitle]="'Here\\'s what\\'s happening with your project today'"
        icon="dashboard">
      </app-page-header>

      @if (isLoading()) {
        <div class="loading-container">
          <mat-spinner diameter="48"></mat-spinner>
        </div>
      } @else {
        <!-- Stats Grid -->
        <div class="stats-grid">
          <mat-card class="stat-card">
            <div class="stat-icon users">
              <mat-icon>people</mat-icon>
            </div>
            <div class="stat-content">
              <span class="stat-value">{{ stats().totalUsers | number }}</span>
              <span class="stat-label">Total Users</span>
              <span class="stat-trend positive">
                <mat-icon>trending_up</mat-icon>
                +12% from last month
              </span>
            </div>
          </mat-card>

          <mat-card class="stat-card">
            <div class="stat-icon products">
              <mat-icon>inventory_2</mat-icon>
            </div>
            <div class="stat-content">
              <span class="stat-value">{{ stats().totalProducts | number }}</span>
              <span class="stat-label">Products</span>
              <span class="stat-trend positive">
                <mat-icon>trending_up</mat-icon>
                +5% from last month
              </span>
            </div>
          </mat-card>

          <mat-card class="stat-card">
            <div class="stat-icon orders">
              <mat-icon>shopping_cart</mat-icon>
            </div>
            <div class="stat-content">
              <span class="stat-value">{{ stats().totalOrders | number }}</span>
              <span class="stat-label">Orders</span>
              <span class="stat-trend positive">
                <mat-icon>trending_up</mat-icon>
                +23% from last month
              </span>
            </div>
          </mat-card>

          <mat-card class="stat-card">
            <div class="stat-icon revenue">
              <mat-icon>attach_money</mat-icon>
            </div>
            <div class="stat-content">
              <span class="stat-value">{{ stats().revenue | currency }}</span>
              <span class="stat-label">Revenue</span>
              <span class="stat-trend positive">
                <mat-icon>trending_up</mat-icon>
                +18% from last month
              </span>
            </div>
          </mat-card>
        </div>

        <!-- Main Content -->
        <div class="content-grid">
          <!-- Quick Actions -->
          <mat-card class="quick-actions-card">
            <mat-card-header>
              <mat-card-title>Quick Actions</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <div class="actions-grid">
                <a routerLink="/identity/users" class="action-item">
                  <div class="action-icon users">
                    <mat-icon>person_add</mat-icon>
                  </div>
                  <span>Add User</span>
                </a>
                <a routerLink="/catalog/products" class="action-item">
                  <div class="action-icon products">
                    <mat-icon>add_box</mat-icon>
                  </div>
                  <span>Add Product</span>
                </a>
                <a routerLink="/todos" class="action-item">
                  <div class="action-icon todos">
                    <mat-icon>add_task</mat-icon>
                  </div>
                  <span>Add Todo</span>
                </a>
                <a class="action-item">
                  <div class="action-icon reports">
                    <mat-icon>assessment</mat-icon>
                  </div>
                  <span>View Reports</span>
                </a>
              </div>
            </mat-card-content>
          </mat-card>

          <!-- Recent Activity -->
          <mat-card class="activity-card">
            <mat-card-header>
              <mat-card-title>Recent Activity</mat-card-title>
              <button mat-button color="primary">View All</button>
            </mat-card-header>
            <mat-card-content>
              <mat-list>
                @for (activity of recentActivity(); track activity.id) {
                  <mat-list-item>
                    <div class="activity-icon" [class]="activity.type" matListItemIcon>
                      <mat-icon>{{ activity.icon }}</mat-icon>
                    </div>
                    <div matListItemTitle>{{ activity.title }}</div>
                    <div matListItemLine class="activity-description">
                      {{ activity.description }}
                    </div>
                    <span matListItemMeta class="activity-time">
                      {{ getRelativeTime(activity.timestamp) }}
                    </span>
                  </mat-list-item>
                }
              </mat-list>
            </mat-card-content>
          </mat-card>
        </div>

        <!-- Welcome Card -->
        <mat-card class="welcome-card">
          <div class="welcome-content">
            <div class="welcome-text">
              <h2>Welcome to FSH Starter Kit! 🚀</h2>
              <p>
                This is a full-stack solution built with .NET 9 and Angular 18. 
                Explore the features and start building your next great application.
              </p>
              <div class="welcome-actions">
                <a mat-raised-button color="primary" href="https://github.com/fullstackhero/dotnet-starter-kit" target="_blank">
                  <mat-icon>code</mat-icon>
                  View on GitHub
                </a>
                <a mat-stroked-button href="https://fullstackhero.net/docs" target="_blank">
                  <mat-icon>menu_book</mat-icon>
                  Documentation
                </a>
              </div>
            </div>
            <div class="welcome-illustration">
              <mat-icon>rocket_launch</mat-icon>
            </div>
          </div>
        </mat-card>
      }
    </div>
  `,
  styles: [`
    .home-container {
      padding: 24px;
    }

    .loading-container {
      display: flex;
      justify-content: center;
      padding: 48px;
    }

    /* Stats Grid */
    .stats-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(240px, 1fr));
      gap: 24px;
      margin-bottom: 24px;
    }

    .stat-card {
      display: flex;
      align-items: center;
      padding: 20px;
      gap: 20px;
    }

    .stat-icon {
      width: 56px;
      height: 56px;
      border-radius: 12px;
      display: flex;
      align-items: center;
      justify-content: center;
    }

    .stat-icon mat-icon {
      font-size: 28px;
      width: 28px;
      height: 28px;
      color: white;
    }

    .stat-icon.users {
      background: linear-gradient(135deg, #6366f1, #8b5cf6);
    }

    .stat-icon.products {
      background: linear-gradient(135deg, #10b981, #34d399);
    }

    .stat-icon.orders {
      background: linear-gradient(135deg, #f59e0b, #fbbf24);
    }

    .stat-icon.revenue {
      background: linear-gradient(135deg, #3b82f6, #60a5fa);
    }

    .stat-content {
      display: flex;
      flex-direction: column;
    }

    .stat-value {
      font-size: 28px;
      font-weight: 600;
      color: var(--text-primary);
    }

    .stat-label {
      font-size: 14px;
      color: var(--text-secondary);
      margin-bottom: 4px;
    }

    .stat-trend {
      display: flex;
      align-items: center;
      gap: 4px;
      font-size: 12px;
    }

    .stat-trend mat-icon {
      font-size: 14px;
      width: 14px;
      height: 14px;
    }

    .stat-trend.positive {
      color: #10b981;
    }

    .stat-trend.negative {
      color: #ef4444;
    }

    /* Content Grid */
    .content-grid {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 24px;
      margin-bottom: 24px;
    }

    mat-card-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
    }

    /* Quick Actions */
    .actions-grid {
      display: grid;
      grid-template-columns: repeat(2, 1fr);
      gap: 16px;
      padding-top: 8px;
    }

    .action-item {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 8px;
      padding: 16px;
      border-radius: 12px;
      background: var(--surface-color);
      border: 1px solid var(--border-color);
      text-decoration: none;
      color: var(--text-primary);
      transition: all 0.2s;
      cursor: pointer;
    }

    .action-item:hover {
      border-color: var(--primary-color);
      transform: translateY(-2px);
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
    }

    .action-icon {
      width: 48px;
      height: 48px;
      border-radius: 12px;
      display: flex;
      align-items: center;
      justify-content: center;
    }

    .action-icon mat-icon {
      font-size: 24px;
      width: 24px;
      height: 24px;
      color: white;
    }

    .action-icon.users {
      background: linear-gradient(135deg, #6366f1, #8b5cf6);
    }

    .action-icon.products {
      background: linear-gradient(135deg, #10b981, #34d399);
    }

    .action-icon.todos {
      background: linear-gradient(135deg, #f59e0b, #fbbf24);
    }

    .action-icon.reports {
      background: linear-gradient(135deg, #3b82f6, #60a5fa);
    }

    /* Recent Activity */
    .activity-card mat-list {
      padding-top: 0;
    }

    .activity-icon {
      width: 36px;
      height: 36px;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
    }

    .activity-icon mat-icon {
      font-size: 18px;
      width: 18px;
      height: 18px;
      color: white;
    }

    .activity-icon.user {
      background: #6366f1;
    }

    .activity-icon.product {
      background: #10b981;
    }

    .activity-icon.order {
      background: #f59e0b;
    }

    .activity-icon.system {
      background: #64748b;
    }

    .activity-description {
      color: var(--text-secondary);
      font-size: 13px;
    }

    .activity-time {
      font-size: 12px;
      color: var(--text-secondary);
    }

    /* Welcome Card */
    .welcome-card {
      background: linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%);
      color: white;
    }

    .welcome-content {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 16px;
    }

    .welcome-text h2 {
      margin: 0 0 12px;
      font-size: 24px;
    }

    .welcome-text p {
      margin: 0 0 20px;
      opacity: 0.9;
      max-width: 500px;
      line-height: 1.6;
    }

    .welcome-actions {
      display: flex;
      gap: 12px;
    }

    .welcome-actions a {
      color: white;
    }

    .welcome-actions .mat-mdc-raised-button {
      background: white;
      color: #6366f1;
    }

    .welcome-actions .mat-mdc-outlined-button {
      border-color: rgba(255, 255, 255, 0.5);
    }

    .welcome-illustration mat-icon {
      font-size: 120px;
      width: 120px;
      height: 120px;
      opacity: 0.3;
    }

    @media (max-width: 900px) {
      .content-grid {
        grid-template-columns: 1fr;
      }

      .welcome-content {
        flex-direction: column;
        text-align: center;
      }

      .welcome-text p {
        max-width: none;
      }

      .welcome-actions {
        justify-content: center;
        flex-wrap: wrap;
      }

      .welcome-illustration {
        margin-top: 24px;
      }
    }

    @media (max-width: 600px) {
      .stats-grid {
        grid-template-columns: 1fr;
      }
    }
  `]
})
export class HomeComponent implements OnInit {
  private authService = inject(AuthService);

  isLoading = signal(false);
  stats = signal<DashboardStats>({
    totalUsers: 0,
    totalProducts: 0,
    totalOrders: 0,
    revenue: 0
  });
  recentActivity = signal<RecentActivity[]>([]);

  ngOnInit(): void {
    this.loadDashboardData();
  }

  getGreeting(): string {
    const hour = new Date().getHours();
    const user = this.authService.currentUser();
    const name = user?.firstName || 'there';
    
    if (hour < 12) return `Good morning, ${name}!`;
    if (hour < 18) return `Good afternoon, ${name}!`;
    return `Good evening, ${name}!`;
  }

  async loadDashboardData(): Promise<void> {
    this.isLoading.set(true);
    
    try {
      await new Promise(resolve => setTimeout(resolve, 500));
      
      this.stats.set({
        totalUsers: 1248,
        totalProducts: 856,
        totalOrders: 3642,
        revenue: 125430.50
      });

      this.recentActivity.set([
        { id: '1', type: 'user', title: 'New user registered', description: 'John Smith created an account', timestamp: new Date(Date.now() - 5 * 60000).toISOString(), icon: 'person_add' },
        { id: '2', type: 'order', title: 'New order received', description: 'Order #12345 placed for $299.99', timestamp: new Date(Date.now() - 15 * 60000).toISOString(), icon: 'shopping_cart' },
        { id: '3', type: 'product', title: 'Product updated', description: 'iPhone 15 Pro stock updated', timestamp: new Date(Date.now() - 30 * 60000).toISOString(), icon: 'inventory' },
        { id: '4', type: 'system', title: 'System backup completed', description: 'Daily backup finished successfully', timestamp: new Date(Date.now() - 60 * 60000).toISOString(), icon: 'backup' },
        { id: '5', type: 'user', title: 'User role updated', description: 'Jane Doe promoted to Admin', timestamp: new Date(Date.now() - 120 * 60000).toISOString(), icon: 'admin_panel_settings' },
      ]);
    } finally {
      this.isLoading.set(false);
    }
  }

  getRelativeTime(timestamp: string): string {
    const now = new Date();
    const date = new Date(timestamp);
    const diffMs = now.getTime() - date.getTime();
    const diffMins = Math.floor(diffMs / 60000);
    
    if (diffMins < 1) return 'Just now';
    if (diffMins < 60) return `${diffMins}m ago`;
    
    const diffHours = Math.floor(diffMins / 60);
    if (diffHours < 24) return `${diffHours}h ago`;
    
    const diffDays = Math.floor(diffHours / 24);
    return `${diffDays}d ago`;
  }
}

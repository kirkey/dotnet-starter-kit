import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { MatChipsModule } from '@angular/material/chips';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';
import { MatBadgeModule } from '@angular/material/badge';
import { AuthService } from '@core/services/auth.service';

interface Task {
  id: string;
  time: string;
  title: string;
  isNew?: boolean;
}

interface FileCategory {
  name: string;
  count: number;
  color: string;
  icon: string;
}

interface Project {
  status: string;
  count: number;
  color: string;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatTabsModule,
    MatChipsModule,
    MatMenuModule,
    MatProgressSpinnerModule,
    MatDividerModule,
    MatListModule,
    MatBadgeModule
  ],
  template: `
    <div class="dashboard-container">
      <!-- Breadcrumb -->
      <div class="breadcrumb">
        <span>App</span>
        <mat-icon>chevron_right</mat-icon>
        <span>User</span>
        <mat-icon>chevron_right</mat-icon>
        <span class="current">Profile</span>
      </div>

      <div class="dashboard-grid">
        <!-- Left Column -->
        <div class="left-column">
          <!-- Profile Card -->
          <mat-card class="profile-card">
            <div class="profile-header">
              <div class="avatar-container">
                <div class="avatar" [style.background]="getAvatarGradient()">
                  <img [src]="userImageUrl()" *ngIf="userImageUrl()" [alt]="userName()">
                  <span *ngIf="!userImageUrl()">{{ getInitials() }}</span>
                </div>
                <span class="pro-badge">Pro</span>
              </div>
              <div class="profile-info">
                <h2>{{ userName() }}</h2>
                <p class="role">{{ userRole() }}</p>
                <a class="website" href="#">{{ userWebsite() }}</a>
                <div class="tags">
                  <mat-chip class="tag-chip ui">UI/UX</mat-chip>
                  <mat-chip class="tag-chip art">Art</mat-chip>
                  <mat-chip class="tag-chip design">Design</mat-chip>
                  <mat-chip class="tag-chip illustration">Illustration</mat-chip>
                  <mat-chip class="tag-chip mobile">Mobile</mat-chip>
                </div>
                <div class="social-links">
                  <button mat-icon-button><mat-icon>facebook</mat-icon></button>
                  <button mat-icon-button><span class="social-icon">M</span></button>
                  <button mat-icon-button><mat-icon>settings</mat-icon></button>
                  <button mat-icon-button><span class="social-icon">Bē</span></button>
                  <button mat-icon-button><mat-icon>camera_alt</mat-icon></button>
                </div>
              </div>
            </div>
          </mat-card>

          <!-- Media Card -->
          <mat-card class="stats-card media-card">
            <mat-card-header>
              <mat-card-title>Media</mat-card-title>
              <button mat-button [matMenuTriggerFor]="mediaMenu">
                Daily <mat-icon>arrow_drop_down</mat-icon>
              </button>
              <mat-menu #mediaMenu="matMenu">
                <button mat-menu-item>Daily</button>
                <button mat-menu-item>Weekly</button>
                <button mat-menu-item>Monthly</button>
              </mat-menu>
            </mat-card-header>
            <mat-card-content>
              <div class="stats-grid">
                <div class="stat-item">
                  <div class="stat-icon posts"><mat-icon>dynamic_feed</mat-icon></div>
                  <div class="stat-value">194</div>
                  <div class="stat-label">Posts</div>
                </div>
                <div class="stat-item">
                  <div class="stat-icon projects"><mat-icon>photo_library</mat-icon></div>
                  <div class="stat-value">554</div>
                  <div class="stat-label">Projects</div>
                </div>
                <div class="stat-item">
                  <div class="stat-icon followers"><mat-icon>people</mat-icon></div>
                  <div class="stat-value">12.8k</div>
                  <div class="stat-label">Followers</div>
                </div>
                <div class="stat-item">
                  <div class="stat-icon following"><mat-icon>person_add</mat-icon></div>
                  <div class="stat-value">1.1k</div>
                  <div class="stat-label">Following</div>
                </div>
              </div>
            </mat-card-content>
          </mat-card>

          <!-- Article Card -->
          <mat-card class="article-card">
            <mat-card-header>
              <mat-card-title>Top 5 react native starter kits</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <div class="article-content">
                <img src="https://picsum.photos/seed/react/120/100" alt="Article" class="article-image">
                <div class="article-text">
                  <p class="author">Nastassia Ovchinnikova</p>
                  <p class="description">React Native allows us to create a boilerplate that have been crafted for both platforms. The ability to create an app both for Android and iOS...</p>
                  <p class="meta">11 Feb 2019 | 5 min read <mat-icon class="bookmark">bookmark</mat-icon></p>
                </div>
              </div>
            </mat-card-content>
          </mat-card>
        </div>

        <!-- Middle Column -->
        <div class="middle-column">
          <!-- Projects Card -->
          <mat-card class="projects-card">
            <mat-card-header>
              <mat-card-title>Projects</mat-card-title>
              <button mat-button [matMenuTriggerFor]="projectsMenu">
                Daily <mat-icon>arrow_drop_down</mat-icon>
              </button>
              <mat-menu #projectsMenu="matMenu">
                <button mat-menu-item>Daily</button>
                <button mat-menu-item>Weekly</button>
                <button mat-menu-item>Monthly</button>
              </mat-menu>
            </mat-card-header>
            <mat-card-content>
              <div class="donut-chart">
                <svg viewBox="0 0 100 100" class="donut">
                  <circle cx="50" cy="50" r="40" fill="none" stroke="#e8e8e8" stroke-width="12"/>
                  <circle cx="50" cy="50" r="40" fill="none" stroke="#4ade80" stroke-width="12"
                          stroke-dasharray="75 176" stroke-dashoffset="0" class="segment new"/>
                  <circle cx="50" cy="50" r="40" fill="none" stroke="#fbbf24" stroke-width="12"
                          stroke-dasharray="50 201" stroke-dashoffset="-75" class="segment progress"/>
                  <circle cx="50" cy="50" r="40" fill="none" stroke="#60a5fa" stroke-width="12"
                          stroke-dasharray="63 188" stroke-dashoffset="-125" class="segment completed"/>
                  <circle cx="50" cy="50" r="40" fill="none" stroke="#f87171" stroke-width="12"
                          stroke-dasharray="63 188" stroke-dashoffset="-188" class="segment canceled"/>
                </svg>
                <div class="donut-center">
                  <span class="total">121</span>
                </div>
              </div>
              <div class="legend">
                <div class="legend-item"><span class="dot new"></span> New</div>
                <div class="legend-item"><span class="dot progress"></span> Progress</div>
                <div class="legend-item"><span class="dot completed"></span> Completed</div>
                <div class="legend-item"><span class="dot canceled"></span> Canceled</div>
              </div>
              <button mat-stroked-button class="details-btn">Details</button>
            </mat-card-content>
          </mat-card>

          <!-- Tasks Card -->
          <mat-card class="tasks-card">
            <mat-card-header>
              <mat-card-title>Tasks</mat-card-title>
              <button mat-button [matMenuTriggerFor]="tasksMenu">
                Daily <mat-icon>arrow_drop_down</mat-icon>
              </button>
              <mat-menu #tasksMenu="matMenu">
                <button mat-menu-item>Daily</button>
                <button mat-menu-item>Weekly</button>
                <button mat-menu-item>Monthly</button>
              </mat-menu>
            </mat-card-header>
            <mat-card-content>
              <div class="task-tabs">
                <button mat-button [class.active]="activeTaskTab() === 'today'" (click)="activeTaskTab.set('today')">Today</button>
                <button mat-button [class.active]="activeTaskTab() === 'week'" (click)="activeTaskTab.set('week')">This Week</button>
                <button mat-button [class.active]="activeTaskTab() === 'month'" (click)="activeTaskTab.set('month')">This Month</button>
              </div>
              <div class="task-list">
                @for (task of tasks(); track task.id) {
                  <div class="task-item">
                    <span class="task-time">{{ task.time }}</span>
                    <span class="task-indicator" [class.new]="task.isNew"></span>
                    <span class="task-title">{{ task.title }}</span>
                    <button mat-icon-button class="task-menu">
                      <mat-icon>more_vert</mat-icon>
                    </button>
                  </div>
                }
              </div>
            </mat-card-content>
          </mat-card>
        </div>

        <!-- Right Column -->
        <div class="right-column">
          <!-- Files Card -->
          <mat-card class="files-card">
            <mat-card-header>
              <mat-card-title>Files</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <div class="file-tabs">
                <button mat-button [class.active]="activeFileTab() === 'work'" (click)="activeFileTab.set('work')">Work</button>
                <button mat-button [class.active]="activeFileTab() === 'private'" (click)="activeFileTab.set('private')">Private</button>
                <button mat-button [class.active]="activeFileTab() === 'social'" (click)="activeFileTab.set('social')">Social</button>
              </div>
              <div class="file-categories">
                @for (category of fileCategories(); track category.name) {
                  <div class="file-category" [style.background]="category.color">
                    <mat-icon>{{ category.icon }}</mat-icon>
                    <span class="category-name">{{ category.name }}</span>
                    <span class="category-count">{{ category.count }} files</span>
                  </div>
                }
              </div>
            </mat-card-content>
          </mat-card>

          <!-- Calendar Card -->
          <mat-card class="calendar-card">
            <mat-card-header>
              <button mat-icon-button><mat-icon>chevron_left</mat-icon></button>
              <mat-card-title>August 2020</mat-card-title>
              <button mat-icon-button><mat-icon>chevron_right</mat-icon></button>
            </mat-card-header>
            <mat-card-content>
              <div class="calendar-grid">
                <div class="calendar-header">
                  <span>S</span><span>M</span><span>T</span><span>W</span><span>T</span><span>F</span><span>S</span>
                </div>
                <div class="calendar-days">
                  @for (day of calendarDays(); track $index) {
                    <span class="day" 
                          [class.other-month]="day.otherMonth" 
                          [class.today]="day.isToday"
                          [class.has-event]="day.hasEvent">
                      {{ day.date }}
                    </span>
                  }
                </div>
              </div>
            </mat-card-content>
          </mat-card>

          <!-- Views Card -->
          <mat-card class="views-card">
            <mat-card-header>
              <mat-card-title>Views</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <div class="views-content">
                <div class="views-number">
                  <span class="big-number">7,156</span>
                  <span class="percentage positive">↑ 7.2%</span>
                </div>
                <div class="views-chart">
                  <svg viewBox="0 0 200 60" class="line-chart">
                    <defs>
                      <linearGradient id="chartGradient" x1="0%" y1="0%" x2="0%" y2="100%">
                        <stop offset="0%" style="stop-color:#f87171;stop-opacity:0.3" />
                        <stop offset="100%" style="stop-color:#f87171;stop-opacity:0" />
                      </linearGradient>
                    </defs>
                    <path d="M0,50 Q20,45 40,40 T80,35 T120,25 T160,30 T200,20" 
                          fill="none" stroke="#f87171" stroke-width="2"/>
                    <path d="M0,50 Q20,45 40,40 T80,35 T120,25 T160,30 T200,20 L200,60 L0,60 Z" 
                          fill="url(#chartGradient)"/>
                  </svg>
                </div>
                <button mat-stroked-button class="see-more-btn">See More</button>
              </div>
            </mat-card-content>
          </mat-card>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .dashboard-container {
      padding: 24px;
      background: #f5f7fb;
      min-height: 100vh;
    }

    .breadcrumb {
      display: flex;
      align-items: center;
      gap: 4px;
      margin-bottom: 24px;
      color: #666;
      font-size: 14px;
    }

    .breadcrumb .current {
      color: #333;
      font-weight: 500;
    }

    .breadcrumb mat-icon {
      font-size: 18px;
      width: 18px;
      height: 18px;
    }

    .dashboard-grid {
      display: grid;
      grid-template-columns: 1fr 1fr 1fr;
      gap: 24px;
    }

    @media (max-width: 1200px) {
      .dashboard-grid {
        grid-template-columns: 1fr 1fr;
      }
      .right-column {
        grid-column: span 2;
        display: grid;
        grid-template-columns: repeat(3, 1fr);
        gap: 24px;
      }
    }

    @media (max-width: 768px) {
      .dashboard-grid {
        grid-template-columns: 1fr;
      }
      .right-column {
        grid-column: span 1;
        grid-template-columns: 1fr;
      }
    }

    /* Profile Card */
    .profile-card {
      background: white;
      border-radius: 16px;
      padding: 24px;
      margin-bottom: 24px;
    }

    .profile-header {
      display: flex;
      gap: 24px;
    }

    .avatar-container {
      position: relative;
      flex-shrink: 0;
    }

    .avatar {
      width: 120px;
      height: 120px;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 36px;
      font-weight: 600;
      color: white;
      overflow: hidden;
      border: 4px solid #f0f0f0;
    }

    .avatar img {
      width: 100%;
      height: 100%;
      object-fit: cover;
    }

    .pro-badge {
      position: absolute;
      bottom: 0;
      left: 50%;
      transform: translateX(-50%);
      background: #60a5fa;
      color: white;
      padding: 4px 16px;
      border-radius: 12px;
      font-size: 12px;
      font-weight: 600;
    }

    .profile-info h2 {
      margin: 0 0 4px 0;
      font-size: 24px;
      font-weight: 600;
    }

    .profile-info .role {
      color: #666;
      margin: 0 0 4px 0;
    }

    .profile-info .website {
      color: #60a5fa;
      text-decoration: none;
      font-size: 14px;
    }

    .tags {
      display: flex;
      flex-wrap: wrap;
      gap: 8px;
      margin: 12px 0;
    }

    .tag-chip {
      font-size: 12px !important;
      height: 24px !important;
      padding: 0 12px !important;
    }

    .tag-chip.ui { background: #dbeafe !important; color: #2563eb !important; }
    .tag-chip.art { background: #fce7f3 !important; color: #db2777 !important; }
    .tag-chip.design { background: #d1fae5 !important; color: #059669 !important; }
    .tag-chip.illustration { background: #fef3c7 !important; color: #d97706 !important; }
    .tag-chip.mobile { background: #ede9fe !important; color: #7c3aed !important; }

    .social-links {
      display: flex;
      gap: 4px;
    }

    .social-links button {
      width: 32px;
      height: 32px;
    }

    .social-icon {
      font-weight: 600;
      font-size: 14px;
    }

    /* Stats Card */
    .stats-card {
      border-radius: 16px;
      margin-bottom: 24px;
    }

    .stats-card mat-card-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 16px 16px 0;
    }

    .stats-grid {
      display: grid;
      grid-template-columns: repeat(2, 1fr);
      gap: 16px;
      padding: 16px;
    }

    .stat-item {
      text-align: center;
    }

    .stat-icon {
      width: 48px;
      height: 48px;
      border-radius: 12px;
      display: flex;
      align-items: center;
      justify-content: center;
      margin: 0 auto 8px;
    }

    .stat-icon.posts { background: #dbeafe; color: #2563eb; }
    .stat-icon.projects { background: #fce7f3; color: #db2777; }
    .stat-icon.followers { background: #ede9fe; color: #7c3aed; }
    .stat-icon.following { background: #d1fae5; color: #059669; }

    .stat-value {
      font-size: 24px;
      font-weight: 700;
    }

    .stat-label {
      color: #666;
      font-size: 14px;
    }

    /* Article Card */
    .article-card {
      border-radius: 16px;
    }

    .article-content {
      display: flex;
      gap: 16px;
      padding: 16px;
    }

    .article-image {
      width: 100px;
      height: 80px;
      border-radius: 8px;
      object-fit: cover;
    }

    .article-text .author {
      font-weight: 600;
      margin: 0 0 8px;
    }

    .article-text .description {
      color: #666;
      font-size: 13px;
      margin: 0 0 8px;
      line-height: 1.5;
    }

    .article-text .meta {
      color: #999;
      font-size: 12px;
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .bookmark {
      color: #f87171;
      font-size: 16px;
      width: 16px;
      height: 16px;
    }

    /* Projects Card */
    .projects-card {
      border-radius: 16px;
      margin-bottom: 24px;
    }

    .projects-card mat-card-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 16px 16px 0;
    }

    .donut-chart {
      position: relative;
      width: 150px;
      height: 150px;
      margin: 16px auto;
    }

    .donut {
      width: 100%;
      height: 100%;
      transform: rotate(-90deg);
    }

    .donut-center {
      position: absolute;
      top: 50%;
      left: 50%;
      transform: translate(-50%, -50%);
      text-align: center;
    }

    .donut-center .total {
      font-size: 28px;
      font-weight: 700;
    }

    .legend {
      display: grid;
      grid-template-columns: repeat(2, 1fr);
      gap: 8px;
      padding: 0 16px;
    }

    .legend-item {
      display: flex;
      align-items: center;
      gap: 8px;
      font-size: 13px;
    }

    .dot {
      width: 10px;
      height: 10px;
      border-radius: 50%;
    }

    .dot.new { background: #4ade80; }
    .dot.progress { background: #fbbf24; }
    .dot.completed { background: #60a5fa; }
    .dot.canceled { background: #f87171; }

    .details-btn {
      display: block;
      margin: 16px auto;
    }

    /* Tasks Card */
    .tasks-card {
      border-radius: 16px;
    }

    .tasks-card mat-card-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 16px 16px 0;
    }

    .task-tabs {
      display: flex;
      gap: 8px;
      padding: 16px;
      border-bottom: 1px solid #eee;
    }

    .task-tabs button {
      color: #999;
    }

    .task-tabs button.active {
      color: #333;
      font-weight: 500;
    }

    .task-list {
      padding: 8px 0;
    }

    .task-item {
      display: flex;
      align-items: center;
      padding: 12px 16px;
      gap: 12px;
    }

    .task-time {
      color: #999;
      font-size: 13px;
      width: 45px;
    }

    .task-indicator {
      width: 8px;
      height: 8px;
      border-radius: 50%;
      background: #ddd;
    }

    .task-indicator.new {
      background: #4ade80;
    }

    .task-title {
      flex: 1;
      font-size: 14px;
    }

    /* Files Card */
    .files-card {
      border-radius: 16px;
      margin-bottom: 24px;
    }

    .files-card mat-card-header {
      padding: 16px 16px 0;
    }

    .file-tabs {
      display: flex;
      gap: 8px;
      padding: 16px;
      border-bottom: 1px solid #eee;
    }

    .file-tabs button {
      color: #999;
    }

    .file-tabs button.active {
      color: #333;
      font-weight: 500;
    }

    .file-categories {
      display: grid;
      grid-template-columns: repeat(2, 1fr);
      gap: 12px;
      padding: 16px;
    }

    .file-category {
      padding: 16px;
      border-radius: 12px;
      color: white;
      display: flex;
      flex-direction: column;
      gap: 8px;
    }

    .file-category mat-icon {
      font-size: 32px;
      width: 32px;
      height: 32px;
    }

    .category-name {
      font-weight: 600;
      font-size: 16px;
    }

    .category-count {
      font-size: 12px;
      opacity: 0.8;
    }

    /* Calendar Card */
    .calendar-card {
      border-radius: 16px;
      margin-bottom: 24px;
    }

    .calendar-card mat-card-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 8px;
    }

    .calendar-grid {
      padding: 0 16px 16px;
    }

    .calendar-header {
      display: grid;
      grid-template-columns: repeat(7, 1fr);
      text-align: center;
      font-size: 12px;
      color: #999;
      padding: 8px 0;
    }

    .calendar-days {
      display: grid;
      grid-template-columns: repeat(7, 1fr);
      text-align: center;
      gap: 4px;
    }

    .day {
      padding: 8px;
      font-size: 13px;
      border-radius: 50%;
      cursor: pointer;
    }

    .day:hover {
      background: #f0f0f0;
    }

    .day.other-month {
      color: #ccc;
    }

    .day.today {
      background: #60a5fa;
      color: white;
    }

    .day.has-event::after {
      content: '';
      display: block;
      width: 4px;
      height: 4px;
      background: #f87171;
      border-radius: 50%;
      margin: 2px auto 0;
    }

    /* Views Card */
    .views-card {
      border-radius: 16px;
    }

    .views-card mat-card-header {
      padding: 16px 16px 0;
    }

    .views-content {
      padding: 16px;
    }

    .views-number {
      display: flex;
      align-items: baseline;
      gap: 8px;
      margin-bottom: 16px;
    }

    .big-number {
      font-size: 32px;
      font-weight: 700;
    }

    .percentage {
      font-size: 14px;
    }

    .percentage.positive {
      color: #4ade80;
    }

    .views-chart {
      height: 60px;
      margin-bottom: 16px;
    }

    .line-chart {
      width: 100%;
      height: 100%;
    }

    .see-more-btn {
      width: 100%;
    }

    /* General Card Styles */
    mat-card {
      box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05) !important;
    }

    mat-card-title {
      font-size: 16px !important;
      font-weight: 600 !important;
      margin: 0 !important;
    }
  `]
})
export class DashboardComponent implements OnInit {
  private authService = inject(AuthService);

  // State signals
  userName = signal('Julee Cruise');
  userRole = signal('Product Designer');
  userWebsite = signal('NewGenerArt.com');
  userImageUrl = signal<string | null>(null);
  
  activeTaskTab = signal<'today' | 'week' | 'month'>('today');
  activeFileTab = signal<'work' | 'private' | 'social'>('work');

  tasks = signal<Task[]>([
    { id: '1', time: '09:24', title: 'Call conference with a New Client', isNew: true },
    { id: '2', time: '10:30', title: 'Presentation Demo Ecological Project', isNew: false },
    { id: '3', time: '12:30', title: 'Call with PR Manager', isNew: false },
    { id: '4', time: '14:00', title: 'Interview with a new UI/UX', isNew: true },
    { id: '5', time: '15:00', title: 'Call conference with a New Client', isNew: true },
    { id: '6', time: '15:45', title: 'Presentation Demo Ecological Project', isNew: false },
    { id: '7', time: '19:00', title: 'Sales Presentation', isNew: true }
  ]);

  fileCategories = signal<FileCategory[]>([
    { name: 'UX', count: 176, color: '#60a5fa', icon: 'grid_view' },
    { name: 'Design', count: 154, color: '#4ade80', icon: 'palette' },
    { name: 'Mobile', count: 98, color: '#fbbf24', icon: 'smartphone' },
    { name: 'Illustration', count: 154, color: '#f87171', icon: 'brush' }
  ]);

  calendarDays = signal<{ date: number; otherMonth?: boolean; isToday?: boolean; hasEvent?: boolean }[]>([]);

  ngOnInit(): void {
    this.initCalendar();
    this.loadUserProfile();
  }

  loadUserProfile(): void {
    // Load from auth service if available
    const currentUser = this.authService.currentUser();
    if (currentUser) {
      this.userName.set(`${currentUser.firstName} ${currentUser.lastName}`);
    }
  }

  initCalendar(): void {
    const days: { date: number; otherMonth?: boolean; isToday?: boolean; hasEvent?: boolean }[] = [];
    
    // Previous month days
    for (let i = 26; i <= 31; i++) {
      days.push({ date: i, otherMonth: true });
    }
    
    // Current month days
    for (let i = 1; i <= 31; i++) {
      days.push({ 
        date: i, 
        isToday: i === 4,
        hasEvent: i === 12 || i === 18 || i === 25
      });
    }
    
    // Next month days
    for (let i = 1; i <= 5; i++) {
      days.push({ date: i, otherMonth: true });
    }
    
    this.calendarDays.set(days);
  }

  getInitials(): string {
    const name = this.userName();
    const parts = name.split(' ');
    return parts.map(p => p.charAt(0)).join('').toUpperCase().substring(0, 2);
  }

  getAvatarGradient(): string {
    return 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)';
  }
}

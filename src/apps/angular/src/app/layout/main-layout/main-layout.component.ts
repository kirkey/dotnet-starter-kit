import { Component, inject, signal, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule, MatSidenav } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatListModule } from '@angular/material/list';
import { MatDividerModule } from '@angular/material/divider';
import { MatTooltipModule } from '@angular/material/tooltip';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { AuthService } from '@core/services/auth.service';
import { ThemeService } from '@core/services/theme.service';
import { NavMenuComponent } from '../nav-menu/nav-menu.component';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    RouterOutlet,
    MatToolbarModule,
    MatSidenavModule,
    MatIconModule,
    MatButtonModule,
    MatMenuModule,
    MatListModule,
    MatDividerModule,
    MatTooltipModule,
    NavMenuComponent
  ],
  template: `
    <mat-sidenav-container class="sidenav-container">
      <!-- Sidenav -->
      <mat-sidenav 
        #sidenav
        [mode]="isMobile() ? 'over' : 'side'"
        [opened]="!isMobile()"
        class="sidenav">
        
        <!-- Brand -->
        <div class="brand">
          <img src="assets/logo.svg" alt="Logo" class="logo" onerror="this.style.display='none'">
          <span class="brand-text">FSH Starter</span>
        </div>

        <mat-divider></mat-divider>

        <!-- Navigation Menu -->
        <app-nav-menu></app-nav-menu>
      </mat-sidenav>

      <!-- Main Content -->
      <mat-sidenav-content class="content">
        <!-- Toolbar -->
        <mat-toolbar color="primary" class="toolbar">
          <button mat-icon-button (click)="sidenav.toggle()" matTooltip="Toggle menu">
            <mat-icon>menu</mat-icon>
          </button>
          
          <span class="toolbar-title">{{ title() }}</span>
          
          <span class="spacer"></span>

          <!-- Theme Toggle -->
          <button mat-icon-button (click)="toggleTheme()" [matTooltip]="themeService.isDarkMode() ? 'Light Mode' : 'Dark Mode'">
            <mat-icon>{{ themeService.isDarkMode() ? 'light_mode' : 'dark_mode' }}</mat-icon>
          </button>

          <!-- Notifications -->
          <button mat-icon-button matTooltip="Notifications">
            <mat-icon>notifications</mat-icon>
          </button>

          <!-- User Menu -->
          <button mat-icon-button [matMenuTriggerFor]="userMenu">
            <mat-icon>account_circle</mat-icon>
          </button>
          <mat-menu #userMenu="matMenu">
            <div class="user-info">
              <div class="user-avatar">{{ authService.userInitials() }}</div>
              <div class="user-details">
                <span class="user-name">{{ authService.userFullName() }}</span>
                <span class="user-email">{{ authService.currentUser()?.email }}</span>
              </div>
            </div>
            <mat-divider></mat-divider>
            <button mat-menu-item routerLink="/identity/account">
              <mat-icon>person</mat-icon>
              <span>Profile</span>
            </button>
            <button mat-menu-item routerLink="/settings">
              <mat-icon>settings</mat-icon>
              <span>Settings</span>
            </button>
            <mat-divider></mat-divider>
            <button mat-menu-item (click)="logout()">
              <mat-icon color="warn">logout</mat-icon>
              <span>Logout</span>
            </button>
          </mat-menu>
        </mat-toolbar>

        <!-- Page Content -->
        <main class="main-content">
          <router-outlet></router-outlet>
        </main>
      </mat-sidenav-content>
    </mat-sidenav-container>
  `,
  styles: [`
    .sidenav-container {
      height: 100vh;
    }

    .sidenav {
      width: 280px;
      background: var(--surface-color);
    }

    .brand {
      display: flex;
      align-items: center;
      gap: 12px;
      padding: 16px;
      height: 64px;
    }

    .logo {
      width: 40px;
      height: 40px;
    }

    .brand-text {
      font-size: 20px;
      font-weight: 500;
      color: var(--primary-color);
    }

    .content {
      display: flex;
      flex-direction: column;
    }

    .toolbar {
      position: sticky;
      top: 0;
      z-index: 100;
    }

    .toolbar-title {
      margin-left: 8px;
      font-size: 18px;
    }

    .spacer {
      flex: 1 1 auto;
    }

    .main-content {
      flex: 1;
      padding: 24px;
      overflow-y: auto;
      background: var(--background-color);
    }

    .user-info {
      display: flex;
      align-items: center;
      gap: 12px;
      padding: 16px;
    }

    .user-avatar {
      width: 40px;
      height: 40px;
      border-radius: 50%;
      background: var(--primary-color);
      color: white;
      display: flex;
      align-items: center;
      justify-content: center;
      font-weight: 500;
    }

    .user-details {
      display: flex;
      flex-direction: column;
    }

    .user-name {
      font-weight: 500;
    }

    .user-email {
      font-size: 12px;
      color: var(--text-secondary);
    }

    :host-context(.dark-theme) .sidenav {
      background: #1e1e1e;
    }

    :host-context(.dark-theme) .main-content {
      background: #121212;
    }
  `]
})
export class MainLayoutComponent {
  @ViewChild('sidenav') sidenav!: MatSidenav;
  
  authService = inject(AuthService);
  themeService = inject(ThemeService);
  private breakpointObserver = inject(BreakpointObserver);

  title = signal('FSH Starter Kit');
  isMobile = signal(false);

  constructor() {
    this.breakpointObserver.observe([Breakpoints.Handset])
      .subscribe(result => {
        this.isMobile.set(result.matches);
      });
  }

  toggleTheme(): void {
    this.themeService.toggleDarkMode();
  }

  logout(): void {
    this.authService.logout();
  }
}

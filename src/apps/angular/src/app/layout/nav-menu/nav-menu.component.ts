import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatBadgeModule } from '@angular/material/badge';
import { MenuService } from '@core/services/menu.service';
import { MenuItem } from '@core/models/menu.model';

@Component({
  selector: 'app-nav-menu',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatListModule,
    MatIconModule,
    MatExpansionModule,
    MatBadgeModule
  ],
  template: `
    <nav class="nav-menu">
      @for (section of menuService.menuSections(); track section.title) {
        <div class="menu-section">
          <h3 class="section-title">{{ section.title }}</h3>
          
          <mat-nav-list>
            @for (item of section.items; track item.title) {
              @if (item.children && item.children.length > 0) {
                <!-- Parent with children -->
                <mat-expansion-panel [expanded]="item.expanded" class="nav-expansion">
                  <mat-expansion-panel-header>
                    <mat-panel-title>
                      <mat-icon class="nav-icon">{{ item.icon }}</mat-icon>
                      <span>{{ item.title }}</span>
                      @if (item.badge) {
                        <span class="badge" [class]="'badge-' + (item.badgeColor || 'primary')">
                          {{ item.badge }}
                        </span>
                      }
                    </mat-panel-title>
                  </mat-expansion-panel-header>
                  
                  <mat-nav-list class="child-nav">
                    @for (child of item.children; track child.title) {
                      <a mat-list-item 
                         [routerLink]="child.route" 
                         routerLinkActive="active"
                         [disabled]="child.disabled">
                        <mat-icon matListItemIcon>{{ child.icon }}</mat-icon>
                        <span matListItemTitle>{{ child.title }}</span>
                        @if (child.badge) {
                          <span class="badge" [class]="'badge-' + (child.badgeColor || 'primary')">
                            {{ child.badge }}
                          </span>
                        }
                      </a>
                    }
                  </mat-nav-list>
                </mat-expansion-panel>
              } @else {
                <!-- Simple link -->
                <a mat-list-item 
                   [routerLink]="item.route" 
                   routerLinkActive="active"
                   [disabled]="item.disabled">
                  <mat-icon matListItemIcon>{{ item.icon }}</mat-icon>
                  <span matListItemTitle>{{ item.title }}</span>
                  @if (item.badge) {
                    <span class="badge" [class]="'badge-' + (item.badgeColor || 'primary')">
                      {{ item.badge }}
                    </span>
                  }
                </a>
              }
            }
          </mat-nav-list>
        </div>
      }
    </nav>
  `,
  styles: [`
    .nav-menu {
      padding: 8px 0;
    }

    .menu-section {
      margin-bottom: 8px;
    }

    .section-title {
      font-size: 12px;
      font-weight: 500;
      text-transform: uppercase;
      color: var(--text-secondary);
      padding: 16px 16px 8px;
      margin: 0;
      letter-spacing: 0.5px;
    }

    .nav-icon {
      margin-right: 12px;
    }

    .nav-expansion {
      box-shadow: none !important;
      background: transparent !important;
    }

    .nav-expansion ::ng-deep .mat-expansion-panel-header {
      padding: 0 16px;
      height: 48px;
    }

    .nav-expansion ::ng-deep .mat-expansion-panel-body {
      padding: 0;
    }

    .nav-expansion ::ng-deep mat-panel-title {
      display: flex;
      align-items: center;
    }

    .child-nav {
      padding-left: 24px;
    }

    .active {
      background: rgba(var(--primary-color-rgb, 89, 74, 226), 0.1) !important;
      color: var(--primary-color) !important;
    }

    .active mat-icon {
      color: var(--primary-color);
    }

    .badge {
      margin-left: auto;
      padding: 2px 8px;
      border-radius: 12px;
      font-size: 10px;
      font-weight: 500;
    }

    .badge-primary {
      background: var(--primary-color);
      color: white;
    }

    .badge-accent {
      background: var(--accent-color);
      color: white;
    }

    .badge-warn {
      background: var(--warn-color);
      color: white;
    }

    a[disabled] {
      opacity: 0.5;
      pointer-events: none;
    }
  `]
})
export class NavMenuComponent {
  menuService = inject(MenuService);
}

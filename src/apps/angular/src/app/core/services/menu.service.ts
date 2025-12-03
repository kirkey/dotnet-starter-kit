import { Injectable, signal } from '@angular/core';
import { MenuItem, MenuSection } from '../models/menu.model';

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  private _menuSections = signal<MenuSection[]>(this.getDefaultMenu());
  
  readonly menuSections = this._menuSections.asReadonly();

  private getDefaultMenu(): MenuSection[] {
    return [
      {
        title: 'Start',
        items: [
          {
            title: 'Home',
            icon: 'home',
            route: '/home'
          },
          {
            title: 'Dashboard',
            icon: 'dashboard',
            route: '/dashboard',
            disabled: true,
            badge: 'Soon',
            badgeColor: 'accent'
          }
        ]
      },
      {
        title: 'Modules',
        items: [
          {
            title: 'Catalog',
            icon: 'inventory_2',
            expanded: false,
            children: [
              {
                title: 'Products',
                icon: 'category',
                route: '/catalog/products'
              },
              {
                title: 'Brands',
                icon: 'branding_watermark',
                route: '/catalog/brands'
              }
            ]
          },
          {
            title: 'Todos',
            icon: 'checklist',
            route: '/todos'
          },
          {
            title: 'Accounting',
            icon: 'account_balance',
            route: '/accounting',
            disabled: true,
            badge: 'Soon',
            badgeColor: 'accent'
          },
          {
            title: 'Store',
            icon: 'storefront',
            route: '/store',
            disabled: true,
            badge: 'Soon',
            badgeColor: 'accent'
          }
        ]
      },
      {
        title: 'Administration',
        items: [
          {
            title: 'Identity',
            icon: 'admin_panel_settings',
            expanded: false,
            children: [
              {
                title: 'Users',
                icon: 'people',
                route: '/identity/users'
              },
              {
                title: 'Roles',
                icon: 'security',
                route: '/identity/roles',
                disabled: true
              }
            ]
          },
          {
            title: 'Tenants',
            icon: 'apartment',
            route: '/multitenancy/tenants',
            disabled: true
          }
        ]
      }
    ];
  }

  toggleMenuItem(item: MenuItem): void {
    item.expanded = !item.expanded;
  }
}

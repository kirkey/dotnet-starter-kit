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
            title: 'Dashboard',
            icon: 'dashboard',
            route: '/dashboard'
          },
          {
            title: 'Home',
            icon: 'home',
            route: '/home'
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
            expanded: false,
            children: [
              {
                title: 'Chart of Accounts',
                icon: 'account_tree',
                route: '/accounting/chart-of-accounts'
              },
              {
                title: 'Journal Entries',
                icon: 'book',
                route: '/accounting/journal-entries'
              },
              {
                title: 'General Ledger',
                icon: 'library_books',
                route: '/accounting/general-ledger'
              },
              {
                title: 'Trial Balance',
                icon: 'balance',
                route: '/accounting/trial-balance'
              },
              {
                title: 'Accounting Periods',
                icon: 'date_range',
                route: '/accounting/accounting-periods'
              },
              {
                title: 'Accounts Payable',
                icon: 'money_off',
                expanded: false,
                children: [
                  {
                    title: 'Vendors',
                    icon: 'people_alt',
                    route: '/accounting/vendors'
                  },
                  {
                    title: 'Bills',
                    icon: 'receipt_long',
                    route: '/accounting/bills'
                  }
                ]
              },
              {
                title: 'Accounts Receivable',
                icon: 'attach_money',
                expanded: false,
                children: [
                  {
                    title: 'Customers',
                    icon: 'people',
                    route: '/accounting/customers'
                  },
                  {
                    title: 'Invoices',
                    icon: 'receipt',
                    route: '/accounting/invoices'
                  },
                  {
                    title: 'Payments',
                    icon: 'payments',
                    route: '/accounting/payments'
                  }
                ]
              },
              {
                title: 'Banking',
                icon: 'account_balance_wallet',
                expanded: false,
                children: [
                  {
                    title: 'Bank Accounts',
                    icon: 'account_balance',
                    route: '/accounting/banks'
                  },
                  {
                    title: 'Bank Reconciliations',
                    icon: 'check_circle',
                    route: '/accounting/bank-reconciliations'
                  }
                ]
              },
              {
                title: 'Budgets',
                icon: 'assessment',
                route: '/accounting/budgets'
              },
              {
                title: 'Fixed Assets',
                icon: 'precision_manufacturing',
                route: '/accounting/fixed-assets'
              },
              {
                title: 'Tax Codes',
                icon: 'percent',
                route: '/accounting/tax-codes'
              },
              {
                title: 'Financial Statements',
                icon: 'description',
                route: '/accounting/financial-statements'
              }
            ]
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
            title: 'Tenants',
            icon: 'business',
            route: '/tenants'
          },
          {
            title: 'Identity',
            icon: 'admin_panel_settings',
            expanded: true,
            children: [
              {
                title: 'Users',
                icon: 'people',
                route: '/identity/users'
              },
              {
                title: 'Roles',
                icon: 'security',
                route: '/identity/roles'
              }
            ]
          }
        ]
      }
    ];
  }

  toggleMenuItem(item: MenuItem): void {
    item.expanded = !item.expanded;
  }
}

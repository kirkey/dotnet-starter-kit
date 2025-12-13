import { Injectable, signal, inject, computed } from '@angular/core';
import { MenuItem, MenuSection } from '../models/menu.model';
import { PermissionService } from './permission.service';
import { FshActions, FshResources } from '../models/permission.model';

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  private permissionService = inject(PermissionService);
  
  private _menuSections = signal<MenuSection[]>(this.getDefaultMenu());
  
  // Filtered menu based on user permissions
  readonly menuSections = computed(() => this.filterMenuByPermissions(this._menuSections()));

  private filterMenuByPermissions(sections: MenuSection[]): MenuSection[] {
    const filtered: MenuSection[] = [];
    
    for (const section of sections) {
      const filteredItems: MenuItem[] = [];
      
      for (const item of section.items) {
        // Check if user has permission for this item
        if (item.action && item.resource) {
          if (!this.permissionService.hasPermission(item.action, item.resource)) {
            continue; // Skip this item if no permission
          }
        }
        
        // If item has children, filter them too
        if (item.children && item.children.length > 0) {
          const filteredChildren = item.children.filter(child => {
            // Skip group headers in permission check
            if (child.isGroupHeader) return true;
            
            if (child.action && child.resource) {
              return this.permissionService.hasPermission(child.action, child.resource);
            }
            return true; // If no permission required, include it
          });
          
          // Only include parent if it has visible children
          if (filteredChildren.length > 0) {
            filteredItems.push({ ...item, children: filteredChildren });
          }
        } else {
          filteredItems.push(item);
        }
      }
      
      // Only include section if it has items
      if (filteredItems.length > 0) {
        filtered.push({ ...section, items: filteredItems });
      }
    }
    
    return filtered;
  }

  private getDefaultMenu(): MenuSection[] {
    return [
      {
        title: 'Start',
        items: [
          {
            title: 'Dashboard',
            icon: 'dashboard',
            route: '/dashboard',
            action: FshActions.View,
            resource: FshResources.Dashboard
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
                route: '/catalog/products',
                action: FshActions.View,
                resource: FshResources.Products
              },
              {
                title: 'Brands',
                icon: 'branding_watermark',
                route: '/catalog/brands',
                action: FshActions.View,
                resource: FshResources.Brands
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
            action: FshActions.View,
            resource: FshResources.Accounting,
            children: [
              {
                title: 'General Ledger',
                isGroupHeader: true
              },
              {
                title: 'Chart of Accounts',
                icon: 'account_tree',
                route: '/accounting/chart-of-accounts',
                action: FshActions.View,
                resource: FshResources.Accounting
              },
              {
                title: 'General Ledger',
                icon: 'library_books',
                route: '/accounting/general-ledger',
                action: FshActions.View,
                resource: FshResources.Accounting
              },
              {
                title: 'Journal Entries',
                icon: 'book',
                route: '/accounting/journal-entries',
                action: FshActions.View,
                resource: FshResources.Accounting
              },
              {
                title: 'Trial Balance',
                icon: 'balance',
                route: '/accounting/trial-balance',
                action: FshActions.View,
                resource: FshResources.Accounting
              },
              {
                title: 'Accounting Periods',
                icon: 'date_range',
                route: '/accounting/accounting-periods',
                action: FshActions.View,
                resource: FshResources.Accounting
              },
              {
                title: 'Accounts Payable',
                isGroupHeader: true
              },
              {
                title: 'Vendors',
                icon: 'people_alt',
                route: '/accounting/vendors',
                action: FshActions.View,
                resource: FshResources.Accounting
              },
              {
                title: 'Bills',
                icon: 'receipt_long',
                route: '/accounting/bills',
                action: FshActions.View,
                resource: FshResources.Accounting
              },
              {
                title: 'Accounts Receivable',
                isGroupHeader: true
              },
              {
                title: 'Customers',
                icon: 'people',
                route: '/accounting/customers',
                action: FshActions.View,
                resource: FshResources.Accounting
              },
              {
                title: 'Invoices',
                icon: 'receipt',
                route: '/accounting/invoices',
                action: FshActions.View,
                resource: FshResources.Accounting
              },
              {
                title: 'Payments',
                icon: 'payments',
                route: '/accounting/payments',
                action: FshActions.View,
                resource: FshResources.Accounting
              },
              {
                title: 'Banking',
                isGroupHeader: true
              },
              {
                title: 'Bank Accounts',
                icon: 'account_balance',
                route: '/accounting/banks',
                action: FshActions.View,
                resource: FshResources.Accounting
              },
              {
                title: 'Bank Reconciliations',
                icon: 'check_circle',
                route: '/accounting/bank-reconciliations',
                action: FshActions.View,
                resource: FshResources.Accounting
              },
              {
                title: 'Configuration',
                isGroupHeader: true
              },
              {
                title: 'Budgets',
                icon: 'assessment',
                route: '/accounting/budgets',
                action: FshActions.View,
                resource: FshResources.Accounting
              },
              {
                title: 'Fixed Assets',
                icon: 'precision_manufacturing',
                route: '/accounting/fixed-assets',
                action: FshActions.View,
                resource: FshResources.Accounting
              },
              {
                title: 'Tax Codes',
                icon: 'percent',
                route: '/accounting/tax-codes',
                action: FshActions.View,
                resource: FshResources.Accounting
              },
              {
                title: 'Financial Statements',
                icon: 'description',
                route: '/accounting/financial-statements',
                action: FshActions.View,
                resource: FshResources.Accounting
              }
            ]
          },
          {
            title: 'Store',
            icon: 'storefront',
            expanded: false,
            action: FshActions.View,
            resource: FshResources.Store,
            children: [
              {
                title: 'Dashboard & Setup',
                isGroupHeader: true
              },
              {
                title: 'Dashboard',
                icon: 'dashboard',
                route: '/store/dashboard',
                action: FshActions.View,
                resource: FshResources.Store
              },
              {
                title: 'Categories',
                icon: 'category',
                route: '/store/categories',
                action: FshActions.View,
                resource: FshResources.Store
              },
              {
                title: 'Items',
                icon: 'inventory_2',
                route: '/store/items',
                action: FshActions.View,
                resource: FshResources.Store
              },
              {
                title: 'Procurement',
                isGroupHeader: true
              },
              {
                title: 'Suppliers',
                icon: 'local_shipping',
                route: '/store/suppliers',
                action: FshActions.View,
                resource: FshResources.Store
              },
              {
                title: 'Purchase Orders',
                icon: 'shopping_cart',
                route: '/store/purchase-orders',
                action: FshActions.View,
                resource: FshResources.Store
              },
              {
                title: 'Inventory',
                isGroupHeader: true
              },
              {
                title: 'Stock Levels',
                icon: 'inventory',
                route: '/store/stock-levels',
                action: FshActions.View,
                resource: FshResources.Store
              },
              {
                title: 'Stock Adjustments',
                icon: 'trending_down',
                route: '/store/stock-adjustments',
                action: FshActions.View,
                resource: FshResources.Store
              }
            ]
          },
          {
            title: 'Warehouse',
            icon: 'warehouse',
            expanded: false,
            action: FshActions.View,
            resource: FshResources.Warehouse,
            children: [
              {
                title: 'Setup',
                isGroupHeader: true
              },
              {
                title: 'Warehouses',
                icon: 'warehouse',
                route: '/warehouse/warehouses',
                action: FshActions.View,
                resource: FshResources.Warehouse
              },
              {
                title: 'Locations',
                icon: 'location_on',
                route: '/warehouse/locations',
                action: FshActions.View,
                resource: FshResources.Warehouse
              },
              {
                title: 'Operations',
                isGroupHeader: true
              },
              {
                title: 'Pick Lists',
                icon: 'playlist_add_check',
                route: '/warehouse/pick-lists',
                action: FshActions.View,
                resource: FshResources.Warehouse
              }
            ]
          },
          {
            title: 'MicroFinance',
            icon: 'account_balance',
            expanded: false,
            action: FshActions.View,
            resource: FshResources.MicroFinance,
            children: [
              {
                title: 'Organization & Setup',
                isGroupHeader: true
              },
              {
                title: 'Branches',
                icon: 'business',
                route: '/microfinance/branches',
                action: FshActions.View,
                resource: FshResources.Branches
              },
              {
                title: 'Staff',
                icon: 'badge',
                route: '/microfinance/staff',
                action: FshActions.View,
                resource: FshResources.Staff
              },
              {
                title: 'Member Management',
                isGroupHeader: true
              },
              {
                title: 'Members',
                icon: 'people',
                route: '/microfinance/members',
                action: FshActions.View,
                resource: FshResources.Members
              },
              {
                title: 'Member Groups',
                icon: 'groups',
                route: '/microfinance/member-groups',
                action: FshActions.View,
                resource: FshResources.MemberGroups
              },
              {
                title: 'Product Catalog',
                isGroupHeader: true
              },
              {
                title: 'Loan Products',
                icon: 'credit_score',
                route: '/microfinance/loan-products',
                action: FshActions.View,
                resource: FshResources.LoanProducts
              },
              {
                title: 'Savings Products',
                icon: 'savings',
                route: '/microfinance/savings-products',
                action: FshActions.View,
                resource: FshResources.SavingsProducts
              },
              {
                title: 'Loan Operations',
                isGroupHeader: true
              },
              {
                title: 'Loans',
                icon: 'monetization_on',
                route: '/microfinance/loans',
                action: FshActions.View,
                resource: FshResources.Loans
              },
              {
                title: 'Loan Applications',
                icon: 'assignment',
                route: '/microfinance/loan-applications',
                action: FshActions.View,
                resource: FshResources.LoanApplications
              },
              {
                title: 'Accounts',
                isGroupHeader: true
              },
              {
                title: 'Savings Accounts',
                icon: 'account_balance',
                route: '/microfinance/savings-accounts',
                action: FshActions.View,
                resource: FshResources.SavingsAccounts
              }
            ]
          },
          {
            title: 'Human Resources',
            icon: 'people',
            expanded: false,
            action: FshActions.View,
            resource: FshResources.Organization,
            children: [
              {
                title: 'Organization',
                isGroupHeader: true
              },
              {
                title: 'Organizational Units',
                icon: 'account_tree',
                route: '/hr/organizational-units',
                action: FshActions.View,
                resource: FshResources.Organization
              },
              {
                title: 'Employees',
                isGroupHeader: true
              },
              {
                title: 'Employees',
                icon: 'badge',
                route: '/hr/employees',
                action: FshActions.View,
                resource: FshResources.Employees
              },
              {
                title: 'Time & Attendance',
                isGroupHeader: true
              },
              {
                title: 'Attendance',
                icon: 'fingerprint',
                route: '/hr/attendance',
                action: FshActions.View,
                resource: FshResources.Attendance
              },
              {
                title: 'Timesheets',
                icon: 'access_time',
                route: '/hr/timesheets',
                action: FshActions.View,
                resource: FshResources.Timesheets
              },
              {
                title: 'Leave Management',
                isGroupHeader: true
              },
              {
                title: 'Leave Requests',
                icon: 'event_available',
                route: '/hr/leave-requests',
                action: FshActions.View,
                resource: FshResources.Leaves
              },
              {
                title: 'Payroll',
                isGroupHeader: true
              },
              {
                title: 'Payroll Run',
                icon: 'payments',
                route: '/hr/payrolls',
                action: FshActions.View,
                resource: FshResources.Payroll
              },
              {
                title: 'Benefits',
                isGroupHeader: true
              },
              {
                title: 'Benefits',
                icon: 'card_giftcard',
                route: '/hr/benefits',
                action: FshActions.View,
                resource: FshResources.Benefits
              }
            ]
          },
          {
            title: 'Messaging',
            icon: 'chat',
            route: '/messaging',
            action: FshActions.View,
            resource: FshResources.Messaging
          }
        ]
      },
      {
        title: 'Administration',
        items: [
          {
            title: 'Tenants',
            icon: 'business',
            route: '/tenants',
            action: FshActions.View,
            resource: FshResources.Tenants
          },
          {
            title: 'Identity',
            icon: 'admin_panel_settings',
            expanded: false,
            children: [
              {
                title: 'Users',
                icon: 'people',
                route: '/identity/users',
                action: FshActions.View,
                resource: FshResources.Users
              },
              {
                title: 'Roles',
                icon: 'security',
                route: '/identity/roles',
                action: FshActions.View,
                resource: FshResources.Roles
              }
            ]
          },
          {
            title: 'Audit Trail',
            icon: 'history',
            route: '/identity/audit-trail',
            action: FshActions.View,
            resource: FshResources.AuditTrails
          },
          {
            title: 'Hangfire',
            icon: 'engineering',
            route: '/hangfire',
            action: FshActions.View,
            resource: FshResources.Hangfire
          }
        ]
      }
    ];
  }

  toggleMenuItem(item: MenuItem): void {
    item.expanded = !item.expanded;
  }

  /**
   * Refresh menu when permissions change (e.g., after login/logout)
   */
  refreshMenu(): void {
    // Trigger recomputation by updating the signal
    this._menuSections.set(this.getDefaultMenu());
  }
}

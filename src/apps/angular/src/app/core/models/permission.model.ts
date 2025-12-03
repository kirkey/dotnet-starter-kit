// FSH Resources - matches the backend FshResources
export const FshResources = {
  Tenants: 'Tenants',
  Analytics: 'Analytics',
  Dashboard: 'Dashboard',
  Hangfire: 'Hangfire',
  Users: 'Users',
  UserRoles: 'UserRoles',
  Roles: 'Roles',
  RoleClaims: 'RoleClaims',
  AuditTrails: 'AuditTrails',
  Brands: 'Brands',
  Products: 'Products',
  Todos: 'Todos',
  Accounting: 'Accounting',
  Store: 'Store',
  Warehouse: 'Warehouse',
  Messaging: 'Messaging',
  Employees: 'Employees',
  Attendance: 'Attendance',
  Timesheets: 'Timesheets',
  Leaves: 'Leaves',
  Payroll: 'Payroll',
  Benefits: 'Benefits',
  Taxes: 'Taxes',
  Organization: 'Organization'
} as const;

// FSH Actions - matches the backend FshActions
export const FshActions = {
  View: 'View',
  Search: 'Search',
  Create: 'Create',
  Update: 'Update',
  Delete: 'Delete',
  Import: 'Import',
  Export: 'Export',
  Generate: 'Generate',
  Clean: 'Clean',
  UpgradeSubscription: 'UpgradeSubscription',
  Regularize: 'Regularize',
  Terminate: 'Terminate',
  Assign: 'Assign',
  Manage: 'Manage',
  Approve: 'Approve',
  Reject: 'Reject',
  Submit: 'Submit',
  Process: 'Process',
  Complete: 'Complete',
  Cancel: 'Cancel',
  Void: 'Void',
  Post: 'Post',
  Send: 'Send',
  Receive: 'Receive',
  MarkAsPaid: 'MarkAsPaid',
  Accrue: 'Accrue',
  Acknowledge: 'Acknowledge'
} as const;

// FSH Roles - matches the backend FshRoles
export const FshRoles = {
  Admin: 'Admin',
  Basic: 'Basic'
} as const;

export const DefaultRoles: string[] = [FshRoles.Admin, FshRoles.Basic];

export function isDefaultRole(roleName: string): boolean {
  return DefaultRoles.includes(roleName);
}

export interface Permission {
  name: string;
  description: string;
  action: string;
  resource: string;
  isBasic: boolean;
  isRoot: boolean;
  enabled?: boolean;
}

export interface PermissionGroup {
  resource: string;
  permissions: Permission[];
}

// Generate permission name from action and resource
export function generatePermissionName(action: string, resource: string): string {
  return `Permissions.${resource}.${action}`;
}

// All available permissions
export function getAllPermissions(): Permission[] {
  const permissions: Permission[] = [];
  const resources = Object.values(FshResources);
  const actions = [FshActions.View, FshActions.Create, FshActions.Update, FshActions.Delete, FshActions.Search, FshActions.Export];
  
  resources.forEach(resource => {
    actions.forEach(action => {
      permissions.push({
        name: generatePermissionName(action, resource),
        description: `${action} ${resource}`,
        action,
        resource,
        isBasic: action === FshActions.View,
        isRoot: resource === FshResources.Tenants
      });
    });
  });
  
  return permissions;
}

// Group permissions by resource
export function groupPermissionsByResource(permissions: Permission[]): PermissionGroup[] {
  const groups: Map<string, Permission[]> = new Map();
  
  permissions.forEach(permission => {
    const existing = groups.get(permission.resource) || [];
    existing.push(permission);
    groups.set(permission.resource, existing);
  });
  
  return Array.from(groups.entries()).map(([resource, perms]) => ({
    resource,
    permissions: perms
  }));
}

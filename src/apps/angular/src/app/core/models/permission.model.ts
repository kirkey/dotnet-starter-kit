// FSH Resources - matches the backend FshResources
export const FshResources = {
  // Core System
  Tenants: 'Tenants',
  Analytics: 'Analytics',
  Dashboard: 'Dashboard',
  Hangfire: 'Hangfire',
  
  // Identity
  Users: 'Users',
  UserRoles: 'UserRoles',
  Roles: 'Roles',
  RoleClaims: 'RoleClaims',
  AuditTrails: 'AuditTrails',
  
  // Catalog
  Brands: 'Brands',
  Products: 'Products',
  Todos: 'Todos',
  
  // Business Modules
  Accounting: 'Accounting',
  Store: 'Store',
  Warehouse: 'Warehouse',
  Messaging: 'Messaging',
  
  // Human Resources
  Employees: 'Employees',
  Attendance: 'Attendance',
  Timesheets: 'Timesheets',
  Leaves: 'Leaves',
  Payroll: 'Payroll',
  Benefits: 'Benefits',
  Taxes: 'Taxes',
  Organization: 'Organization',
  
  // MicroFinance - Organization & Setup
  MicroFinance: 'MicroFinance',
  Branches: 'Branches',
  BranchTargets: 'BranchTargets',
  MfiConfigurations: 'MfiConfigurations',
  Staff: 'Staff',
  StaffTrainings: 'StaffTrainings',
  
  // MicroFinance - Member Management
  Members: 'Members',
  MemberGroups: 'MemberGroups',
  GroupMemberships: 'GroupMemberships',
  KycDocuments: 'KycDocuments',
  CustomerSegments: 'CustomerSegments',
  CustomerSurveys: 'CustomerSurveys',
  
  // MicroFinance - Product Catalog
  LoanProducts: 'LoanProducts',
  SavingsProducts: 'SavingsProducts',
  ShareProducts: 'ShareProducts',
  FeeDefinitions: 'FeeDefinitions',
  InsuranceProducts: 'InsuranceProducts',
  InvestmentProducts: 'InvestmentProducts',
  
  // MicroFinance - Accounts
  SavingsAccounts: 'SavingsAccounts',
  ShareAccounts: 'ShareAccounts',
  FixedDeposits: 'FixedDeposits',
  InvestmentAccounts: 'InvestmentAccounts',
  InsurancePolicies: 'InsurancePolicies',
  
  // MicroFinance - Loan Operations
  Loans: 'Loans',
  LoanApplications: 'LoanApplications',
  LoanSchedules: 'LoanSchedules',
  LoanRepayments: 'LoanRepayments',
  LoanCollaterals: 'LoanCollaterals',
  LoanGuarantors: 'LoanGuarantors',
  LoanDisbursementTranches: 'LoanDisbursementTranches',
  LoanOfficerAssignments: 'LoanOfficerAssignments',
  LoanOfficerTargets: 'LoanOfficerTargets',
  LoanRestructures: 'LoanRestructures',
  LoanWriteOffs: 'LoanWriteOffs',
  
  // MicroFinance - Collateral Management
  CollateralTypes: 'CollateralTypes',
  CollateralValuations: 'CollateralValuations',
  CollateralInsurances: 'CollateralInsurances',
  CollateralReleases: 'CollateralReleases',
  
  // MicroFinance - Collections & Recovery
  CollectionCases: 'CollectionCases',
  CollectionActions: 'CollectionActions',
  CollectionStrategies: 'CollectionStrategies',
  PromiseToPays: 'PromiseToPays',
  DebtSettlements: 'DebtSettlements',
  LegalActions: 'LegalActions',
  
  // MicroFinance - Transactions
  SavingsTransactions: 'SavingsTransactions',
  ShareTransactions: 'ShareTransactions',
  FeeCharges: 'FeeCharges',
  MobileTransactions: 'MobileTransactions',
  InvestmentTransactions: 'InvestmentTransactions',
  
  // MicroFinance - Insurance
  InsuranceClaims: 'InsuranceClaims',
  
  // MicroFinance - Risk & Compliance
  AmlAlerts: 'AmlAlerts',
  CreditScores: 'CreditScores',
  CreditBureauInquiries: 'CreditBureauInquiries',
  CreditBureauReports: 'CreditBureauReports',
  RiskAlerts: 'RiskAlerts',
  RiskCategories: 'RiskCategories',
  RiskIndicators: 'RiskIndicators',
  Documents: 'Documents',
  
  // MicroFinance - Workflows & Approvals
  ApprovalWorkflows: 'ApprovalWorkflows',
  ApprovalRequests: 'ApprovalRequests',
  CustomerCases: 'CustomerCases',
  
  // MicroFinance - Communications
  CommunicationTemplates: 'CommunicationTemplates',
  CommunicationLogs: 'CommunicationLogs',
  MarketingCampaigns: 'MarketingCampaigns',
  
  // MicroFinance - Digital Channels
  AgentBankings: 'AgentBankings',
  MobileWallets: 'MobileWallets',
  PaymentGateways: 'PaymentGateways',
  QrPayments: 'QrPayments',
  UssdSessions: 'UssdSessions',
  
  // MicroFinance - Cash Management
  CashVaults: 'CashVaults',
  TellerSessions: 'TellerSessions',
  
  // MicroFinance - Reporting
  ReportDefinitions: 'ReportDefinitions',
  ReportGenerations: 'ReportGenerations'
} as const;

// FSH Actions - matches the backend FshActions
export const FshActions = {
  // Standard CRUD Actions
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
  
  // HR & Employee Actions
  Regularize: 'Regularize',
  Terminate: 'Terminate',
  Assign: 'Assign',
  Manage: 'Manage',
  
  // Workflow & Approval Actions
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
  Acknowledge: 'Acknowledge',
  
  // Financial Actions
  MarkAsPaid: 'MarkAsPaid',
  Accrue: 'Accrue',
  Disburse: 'Disburse',
  Deposit: 'Deposit',
  Withdraw: 'Withdraw',
  Transfer: 'Transfer',
  WriteOff: 'WriteOff',
  Mature: 'Mature',
  
  // Status Actions
  Activate: 'Activate',
  Deactivate: 'Deactivate',
  Suspend: 'Suspend',
  Close: 'Close',
  Freeze: 'Freeze',
  Unfreeze: 'Unfreeze',
  Renew: 'Renew',
  Return: 'Return',
  
  // MicroFinance - Collections & Recovery Actions
  Escalate: 'Escalate',
  MarkBroken: 'MarkBroken',
  RecordPayment: 'RecordPayment',
  
  // MicroFinance - AML & Compliance Actions
  FileSar: 'FileSar',
  Confirm: 'Confirm',
  Clear: 'Clear',
  
  // MicroFinance - Insurance Actions
  RecordPremium: 'RecordPremium',
  
  // MicroFinance - Investment Actions
  Invest: 'Invest',
  Redeem: 'Redeem',
  SetupSip: 'SetupSip',
  
  // MicroFinance - Agent Banking Actions
  RecordAudit: 'RecordAudit',
  UpgradeTier: 'UpgradeTier',
  CreditFloat: 'CreditFloat',
  DebitFloat: 'DebitFloat',
  
  // MicroFinance - Loan Actions
  ApplyPayment: 'ApplyPayment',
  Restructure: 'Restructure'
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
// Note: This is a simplified version. The actual permissions are defined in the backend
// (FshPermissions.cs) and should be fetched from the API for accuracy.
// Not all actions apply to all resources - see backend AllPermissions array for specifics.
export function getAllPermissions(): Permission[] {
  const permissions: Permission[] = [];
  const resources = Object.values(FshResources);
  
  // Common actions that apply to most resources
  const commonActions = [
    FshActions.View, 
    FshActions.Search,
    FshActions.Create, 
    FshActions.Update, 
    FshActions.Delete, 
    FshActions.Export,
    FshActions.Import
  ];
  
  resources.forEach(resource => {
    commonActions.forEach(action => {
      permissions.push({
        name: generatePermissionName(action, resource),
        description: `${action} ${resource}`,
        action,
        resource,
        isBasic: action === FshActions.View && resource !== FshResources.Tenants,
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

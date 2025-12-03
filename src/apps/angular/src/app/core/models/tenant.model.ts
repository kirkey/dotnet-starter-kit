export interface Tenant {
  id: string;
  name: string;
  connectionString?: string;
  adminEmail: string;
  validUpto: Date;
  isActive: boolean;
  issuer?: string;
}

export interface CreateTenantRequest {
  id: string;
  name: string;
  connectionString?: string;
  adminEmail: string;
}

export interface UpgradeSubscriptionRequest {
  tenant: string;
  extendedExpiryDate: Date;
}

export interface TenantDetail extends Tenant {
  showDetails?: boolean;
}

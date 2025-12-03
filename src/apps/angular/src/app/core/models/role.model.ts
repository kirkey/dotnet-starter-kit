export interface Role {
  id: string;
  name: string;
  description?: string;
}

export interface CreateOrUpdateRoleRequest {
  id?: string;
  name: string;
  description?: string;
}

export interface RolePermissions {
  roleId: string;
  roleName: string;
  permissions: string[];
}

export interface UpdatePermissionsRequest {
  roleId: string;
  permissions: string[];
}

export interface UserRole {
  roleId: string;
  roleName: string;
  description?: string;
  enabled: boolean;
}

export interface AssignUserRolesRequest {
  userRoles: UserRole[];
}

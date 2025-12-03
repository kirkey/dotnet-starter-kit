export interface User {
  id: string;
  userName: string;
  firstName: string;
  lastName: string;
  email: string;
  imageUrl?: string;
  isActive: boolean;
  emailConfirmed?: boolean;
  phoneNumber?: string;
  roles?: string[];
}

export interface UserListResponse {
  id: string;
  userName: string;
  firstName: string;
  lastName: string;
  email: string;
  isActive: boolean;
}

export interface CreateUserRequest {
  firstName: string;
  lastName: string;
  email: string;
  userName: string;
  password: string;
  confirmPassword: string;
  phoneNumber?: string;
}

export interface UpdateUserRequest {
  id: string;
  firstName: string;
  lastName: string;
  phoneNumber?: string;
  email: string;
}

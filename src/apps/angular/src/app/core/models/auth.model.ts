export interface TokenRequest {
  email: string;
  password: string;
  deviceType?: string; // 'Web', 'Mobile', etc. Defaults to 'Web'
}

export interface TokenResponse {
  token: string;
  refreshToken: string;
  refreshTokenExpiryTime: Date;
}

export interface RefreshTokenRequest {
  token: string;
  refreshToken: string;
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  userName: string;
  password: string;
  confirmPassword?: string;
  phoneNumber?: string;
}

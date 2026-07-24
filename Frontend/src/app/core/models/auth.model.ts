export interface RegisterRequest {
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  password: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  success: boolean;
  message: string;
  accessToken: string;
  refreshToken: string;
}

export interface MeDto {
  username: string;
  email: string;
}
export interface User {
  id: number;
  fullName: string;
  email: string;
  roleName: string;
  createdAt: string;
}

export interface CreateUserDto {
  fullName: string;
  email: string;
  password?: string;
  roleName: string;
}

export interface UpdateUserDto {
  fullName: string;
  email: string;
  roleName: string;
  password?: string;
}

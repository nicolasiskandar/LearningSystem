import { Injectable, inject, signal } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { User, CreateUserDto, UpdateUserDto } from '../models/user.model';
import { AuthResponse, LoginRequest, RegisterRequest } from '../models/auth.models';
import { Course } from '../models/course.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5142/api';

  currentUser = signal<AuthResponse | null>(this.getUserFromStorage());

  getUsers(): Observable<User[]> {
    const headers = this.getHeadersForLogin();
    return this.http.get<User[]>(`${this.apiUrl}/Users`, { headers });
  }

  createUser(user: CreateUserDto): Observable<User> {
    const headers = this.getHeadersForLogin();
    return this.http.post<User>(`${this.apiUrl}/Users`, user, { headers });
  }

  updateUser(id: number, user: UpdateUserDto): Observable<User> {
    const headers = this.getHeadersForLogin();
    return this.http.put<User>(`${this.apiUrl}/Users/${id}`, user, { headers });
  }

  deleteUser(id: number): Observable<void> {
    const headers = this.getHeadersForLogin();
    return this.http.delete<void>(`${this.apiUrl}/Users/${id}`, { headers });
  }

  getUser(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/Users/${id}`);
  }

  getMe(): Observable<User> {
    const headers = this.getHeadersForLogin();
    return this.http.get<User>(`${this.apiUrl}/Users/me`, { headers });
  }

  getCreatedCourses(userId: number): Observable<Course[]> {
    const headers = this.getHeadersForLogin();
    return this.http.get<Course[]>(`${this.apiUrl}/Users/${userId}/courses/created`, { headers });
  }

  getEnrolledCourses(userId: number): Observable<Course[]> {
    const headers = this.getHeadersForLogin();
    return this.http.get<Course[]>(`${this.apiUrl}/Users/${userId}/courses/enrolled`, { headers });
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/auth/login`, request)
      .pipe(tap((response) => this.handleAuthSuccess(response)));
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/auth/register`, request)
      .pipe(tap((response) => this.handleAuthSuccess(response)));
  }

  changePassword(request: { oldPassword: string; newPassword: string }): Observable<void> {
    const headers = this.getHeadersForLogin();
    return this.http.post<void>(`${this.apiUrl}/auth/change-password`, request, { headers });
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }

  public getHeadersForLogin(): HttpHeaders {
    const user = this.currentUser();
    const token = user?.token;

    if (token && this.isTokenExpired(token)) {
      this.logout();
      return new HttpHeaders();
    }

    let headers = new HttpHeaders();
    if (token) headers = headers.set('Authorization', `Bearer ${token}`);
    return headers;
  }

  private handleAuthSuccess(response: AuthResponse) {
    localStorage.setItem('user', JSON.stringify(response));
    this.currentUser.set(response);
  }

  private getUserFromStorage(): AuthResponse | null {
    const userStr = localStorage.getItem('user');
    return userStr ? JSON.parse(userStr) : null;
  }

  private isTokenExpired(token: string): boolean {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const now = Math.floor(Date.now() / 1000);
      return payload.exp < now;
    } catch {
      return true;
    }
  }
}

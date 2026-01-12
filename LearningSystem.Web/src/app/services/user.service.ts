import { Injectable, inject, signal } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { User } from '../models/user.model';
import { AuthResponse, LoginRequest, RegisterRequest } from '../models/auth.models';
import { Course } from '../models/course.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5142/api';

  currentUser = signal<AuthResponse | null>(this.getUserFromStorage());

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

  private handleAuthSuccess(response: AuthResponse) {
    localStorage.setItem('user', JSON.stringify(response));
    this.currentUser.set(response);
  }

  private getUserFromStorage(): AuthResponse | null {
    const userStr = localStorage.getItem('user');
    return userStr ? JSON.parse(userStr) : null;
  }

  public getHeadersForLogin(): HttpHeaders {
    const user = this.currentUser();
    const token = user?.token;
    let headers = new HttpHeaders();
    if (token) headers = headers.set('Authorization', `Bearer ${token}`);
    return headers;
  }
}

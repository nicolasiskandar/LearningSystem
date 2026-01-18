import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Category } from '../models/category.model';
import { UserService } from './user.service';

export interface CreateCategoryDto {
  name: string;
}

export interface UpdateCategoryDto {
  name: string;
}

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private http = inject(HttpClient);
  private userService = inject(UserService);
  private apiUrl = 'http://localhost:5142/api';

  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.apiUrl}/categories`);
  }

  getCategory(id: number): Observable<Category> {
    return this.http.get<Category>(`${this.apiUrl}/categories/${id}`);
  }

  createCategory(category: CreateCategoryDto): Observable<Category> {
    const headers = this.userService.getHeadersForLogin();
    return this.http.post<Category>(`${this.apiUrl}/categories`, category, { headers });
  }

  updateCategory(id: number, category: UpdateCategoryDto): Observable<Category> {
    const headers = this.userService.getHeadersForLogin();
    return this.http.put<Category>(`${this.apiUrl}/categories/${id}`, category, { headers });
  }

  deleteCategory(id: number): Observable<void> {
    const headers = this.userService.getHeadersForLogin();
    return this.http.delete<void>(`${this.apiUrl}/categories/${id}`, { headers });
  }
}

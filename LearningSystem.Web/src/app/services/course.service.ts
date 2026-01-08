import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Course, Lesson } from '../models/course.model';
import { Category } from '../models/category.model';

@Injectable({
  providedIn: 'root',
})
export class CourseService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5142/api';

  getCourses(page: number = 1, pageSize: number = 40, categoryId?: number): Observable<Course[]> {
    let url = `${this.apiUrl}/Courses?page=${page}&pageSize=${pageSize}`;
    if (categoryId) {
      url += `&categoryId=${categoryId}`;
    }
    return this.http.get<Course[]>(url);
  }

  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.apiUrl}/categories`);
  }

  getCourse(id: number): Observable<Course> {
    return this.http.get<Course>(`${this.apiUrl}/Courses/${id}`);
  }

  getLessons(courseId: number): Observable<Lesson[]> {
    return this.http.get<Lesson[]>(`${this.apiUrl}/lessons/course/${courseId}`);
  }

  enrollCourse(courseId: number): Observable<any> {
    const token = localStorage.getItem('token');
    let headers = new HttpHeaders();
    if (token) headers = headers.set('Authorization', `Bearer ${token}`);

    return this.http.post(`${this.apiUrl}/Courses/${courseId}/enroll`, {}, { headers });
  }
}

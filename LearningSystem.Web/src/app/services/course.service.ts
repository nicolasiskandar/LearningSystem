import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Course, Lesson } from '../models/course.model';
import { Category } from '../models/category.model';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root',
})
export class CourseService {
  private http = inject(HttpClient);
  private userService = inject(UserService);
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

  getLesson(id: number): Observable<Lesson> {
    return this.http.get<Lesson>(`${this.apiUrl}/lessons/${id}`);
  }

  completeLesson(lessonId: number): Observable<void> {
    const headers = this.userService.getHeadersForLogin();
    return this.http.post<void>(`${this.apiUrl}/lessons/${lessonId}/complete`, {}, { headers });
  }

  isLessonCompleted(lessonId: number, userId: number): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/lessons/completed/${lessonId}/${userId}`);
  }

  enrollCourse(courseId: number): Observable<any> {
    const headers = this.userService.getHeadersForLogin();
    return this.http.post(`${this.apiUrl}/Courses/${courseId}/enroll`, {}, { headers });
  }
}

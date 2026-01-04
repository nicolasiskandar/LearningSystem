import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Course, Lesson } from '../models/course.model';

@Injectable({
  providedIn: 'root',
})
export class CourseService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5142/api';

  getCourses(page: number = 1, pageSize: number = 40): Observable<Course[]> {
    return this.http.get<Course[]>(`${this.apiUrl}/Courses?page=${page}&pageSize=${pageSize}`);
  }

  getCourse(id: number): Observable<Course> {
    return this.http.get<Course>(`${this.apiUrl}/Courses/${id}`);
  }

  getLessons(courseId: number): Observable<Lesson[]> {
    return this.http.get<Lesson[]>(`${this.apiUrl}/lessons/course/${courseId}`);
  }
}

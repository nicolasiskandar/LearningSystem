import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Course } from '../models/course.model';

@Injectable({
  providedIn: 'root',
})
export class CourseService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5142/api/Courses';

  getCourses(page: number = 1, pageSize: number = 40): Observable<Course[]> {
    return this.http.get<Course[]>(`${this.apiUrl}?page=${page}&pageSize=${pageSize}`);
  }
}

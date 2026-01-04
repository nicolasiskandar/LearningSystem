import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CourseService } from '../services/course.service';
import { Course } from '../models/course.model';

@Component({
  selector: 'app-courses',
  imports: [CommonModule],
  templateUrl: './courses.html',
  styleUrl: './courses.css',
})
export class Courses implements OnInit {
  private courseService = inject(CourseService);

  courses = signal<Course[]>([]);
  isLoading = signal(true);
  isMoreLoading = signal(false);
  error = signal<string | null>(null);
  currentPage = signal(1);
  pageSize = 40;
  hasMore = signal(true);

  ngOnInit(): void {
    this.loadCourses();
  }

  loadCourses(): void {
    this.isLoading.set(true);
    this.courseService.getCourses(this.currentPage(), this.pageSize).subscribe({
      next: (data) => {
        this.courses.set(data);
        this.isLoading.set(false);
        this.hasMore.set(data.length === this.pageSize);
      },
      error: (err) => {
        console.error('Error fetching courses:', err);
        this.error.set('Failed to load courses. Please try again later.');
        this.isLoading.set(false);
      },
    });
  }

  loadMore(): void {
    if (this.isMoreLoading() || !this.hasMore()) return;

    this.isMoreLoading.set(true);
    const nextPage = this.currentPage() + 1;
    
    this.courseService.getCourses(nextPage, this.pageSize).subscribe({
      next: (data) => {
        if (data.length > 0) {
          this.courses.update(prev => [...prev, ...data]);
          this.currentPage.set(nextPage);
        }
        this.hasMore.set(data.length === this.pageSize);
        this.isMoreLoading.set(false);
      },
      error: (err) => {
        console.error('Error loading more courses:', err);
        this.isMoreLoading.set(false);
      },
    });
  }
}

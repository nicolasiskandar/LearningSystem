import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CourseService } from '../services/course.service';
import { UserService } from '../services/user.service';
import { Course } from '../models/course.model';
import { Category } from '../models/category.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-courses',
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './courses.html',
  styleUrl: './courses.css',
})
export class Courses implements OnInit {
  private courseService = inject(CourseService);
  private userService = inject(UserService);

  courses = signal<Course[]>([]);
  categories = signal<Category[]>([]);
  selectedCategoryId = signal<number | undefined>(undefined);
  instructorNames = signal<Record<number, string>>({});
  isLoading = signal(true);
  isMoreLoading = signal(false);
  error = signal<string | null>(null);
  currentPage = signal(1);
  pageSize = 40;
  hasMore = signal(true);

  ngOnInit(): void {
    this.loadCategories();
    this.loadCourses();
  }

  loadCategories(): void {
    this.courseService.getCategories().subscribe({
      next: (data) => this.categories.set(data),
      error: (err) => console.error('Error loading categories:', err),
    });
  }

  onCategoryChange(): void {
    this.currentPage.set(1);
    this.courses.set([]);
    this.loadCourses();
  }

  loadCourses(): void {
    this.isLoading.set(true);
    this.courseService.getCourses(this.currentPage(), this.pageSize, this.selectedCategoryId()).subscribe({
      next: (data) => {
        this.courses.set(data);
        this.loadInstructors(data);
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

    this.courseService.getCourses(nextPage, this.pageSize, this.selectedCategoryId()).subscribe({
      next: (data) => {
        if (data.length > 0) {
          this.courses.update((prev) => [...prev, ...data]);
          this.loadInstructors(data);
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

  private loadInstructors(courses: Course[]): void {
    const uniqueIds = [...new Set(courses.map((c) => c.createdBy))];
    const currentNames = this.instructorNames();

    uniqueIds.forEach((id) => {
      if (!currentNames[id]) {
        this.userService.getUser(id).subscribe({
          next: (user) => {
            this.instructorNames.update((prev) => ({
              ...prev,
              [id]: user.fullName,
            }));
          },
          error: (err) => console.error(`Error loading instructor ${id}`, err),
        });
      }
    });
  }
}

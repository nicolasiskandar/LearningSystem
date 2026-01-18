import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CourseService } from '../../services/course.service';
import { CategoryService } from '../../services/category.service';
import { Course, CreateCourseDto, UpdateCourseDto } from '../../models/course.model';
import { Category } from '../../models/category.model';
import { UserService } from '../../services/user.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-admin-courses',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './admin-courses.html',
  styleUrls: ['./admin-courses.css'],
})
export class AdminCoursesComponent implements OnInit {
  private courseService = inject(CourseService);
  private categoryService = inject(CategoryService);
  private userService = inject(UserService);
  private router = inject(Router);

  courses = signal<Course[]>([]);
  categories = signal<Category[]>([]);
  isLoading = signal<boolean>(false);
  isNotAuthorized = signal<boolean>(false);

  // Modal state
  showModal = signal<boolean>(false);
  modalMode = signal<'create' | 'edit'>('create');
  selectedCourse = signal<Course | null>(null);

  // Form model
  formData = {
    title: '',
    shortDescription: '',
    longDescription: '',
    categoryId: 0,
    difficulty: 'Beginner',
    thumbnail: '',
    isPublished: false,
  };

  difficulties = ['Beginner', 'Intermediate', 'Advanced'];

  ngOnInit() {
    const user = this.userService.currentUser();
    if (!user) {
      this.router.navigate(['/login']);
      return;
    }

    this.loadCourses();
    this.loadCategories();
  }

  loadCourses() {
    this.isLoading.set(true);
    this.courseService.getCourses(1, 100).subscribe({
      next: (courses) => {
        this.courses.set(courses);
        this.isLoading.set(false);
      },
      error: (err: HttpErrorResponse) => {
        this.isLoading.set(false);
        if (err.status === 401) {
          this.router.navigate(['/login']);
        } else if (err.status === 403) {
          this.isNotAuthorized.set(true);
        } else {
          console.error('Error loading courses', err);
        }
      },
    });
  }

  loadCategories() {
    this.categoryService.getCategories().subscribe({
      next: (categories) => this.categories.set(categories),
      error: (err) => console.error('Error loading categories', err),
    });
  }

  openCreateModal() {
    this.modalMode.set('create');
    this.formData = {
      title: '',
      shortDescription: '',
      longDescription: '',
      categoryId: this.categories().length > 0 ? this.categories()[0].id : 0,
      difficulty: 'Beginner',
      thumbnail: '',
      isPublished: false,
    };
    this.showModal.set(true);
  }

  openEditModal(course: Course) {
    this.modalMode.set('edit');
    this.selectedCourse.set(course);
    
    // Find category ID from category name (Backend returns category name in Course object)
    // This is a bit brittle, but Course model has category: string.
    // Ideally Course model should have categoryId.
    const category = this.categories().find(c => c.name === course.category);

    this.formData = {
      title: course.title,
      shortDescription: course.shortDescription,
      longDescription: course.longDescription,
      categoryId: category ? category.id : 0,
      difficulty: course.difficulty,
      thumbnail: course.thumbnail,
      isPublished: course.isPublished,
    };
    this.showModal.set(true);
  }

  closeModal() {
    this.showModal.set(false);
    this.selectedCourse.set(null);
  }

  onSubmit() {
    if (this.modalMode() === 'create') {
      this.createCourse();
    } else {
      this.updateCourse();
    }
  }

  createCourse() {
    const user = this.userService.currentUser();
    if (!user) return;

    const dto: CreateCourseDto = {
      ...this.formData,
      createdBy: user.id,
    };

    this.courseService.createCourse(dto).subscribe({
      next: (newCourse) => {
        this.courses.update((courses) => [...courses, newCourse]);
        this.closeModal();
      },
      error: (err) => {
        console.error('Error creating course', err);
        alert('Failed to create course');
      },
    });
  }

  updateCourse() {
    const course = this.selectedCourse();
    if (!course) return;

    const dto: UpdateCourseDto = {
      ...this.formData,
    };

    this.courseService.updateCourse(course.id, dto).subscribe({
      next: (updatedCourse) => {
        this.courses.update((courses) =>
          courses.map((c) => (c.id === updatedCourse.id ? updatedCourse : c)),
        );
        this.closeModal();
      },
      error: (err) => {
        console.error('Error updating course', err);
        alert('Failed to update course');
      },
    });
  }

  deleteCourse(course: Course) {
    if (!confirm(`Are you sure you want to delete ${course.title}?`)) return;

    this.courseService.deleteCourse(course.id).subscribe({
      next: () => {
        this.courses.update((courses) => courses.filter((c) => c.id !== course.id));
      },
      error: (err) => {
        console.error('Error deleting course', err);
        alert('Failed to delete course');
      },
    });
  }

  manageLessons(courseId: number) {
    this.router.navigate(['/admin/courses', courseId, 'lessons']);
  }
}

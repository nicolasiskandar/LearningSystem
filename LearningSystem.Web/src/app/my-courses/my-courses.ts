import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CourseService } from '../services/course.service';
import { UserService } from '../services/user.service';
import { Course } from '../models/course.model';

@Component({
  selector: 'app-my-courses',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './my-courses.html',
  styleUrl: './my-courses.css',
})
export class MyCourses implements OnInit {
  private courseService = inject(CourseService);
  private userService = inject(UserService);

  courses = signal<Course[]>([]);
  isLoading = signal(true);
  error = signal<string | null>(null);
  instructorNames = signal<Record<number, string>>({});

  ngOnInit(): void {
    const user = this.userService.currentUser();
    if (user) {
      this.loadEnrolledCourses(user.id);
    } else {
      this.error.set('Please log in to view your courses.');
      this.isLoading.set(false);
    }
  }

  loadEnrolledCourses(userId: number): void {
    this.isLoading.set(true);
    this.userService.getEnrolledCourses(userId).subscribe({
      next: (data) => {
        this.courses.set(data);
        this.loadInstructors(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Error fetching enrolled courses:', err);
        this.error.set('Failed to load your courses.');
        this.isLoading.set(false);
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

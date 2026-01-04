import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { CourseService } from '../services/course.service';
import { UserService } from '../services/user.service';
import { Course, Lesson } from '../models/course.model';

@Component({
  selector: 'app-course-details',
  imports: [CommonModule, RouterModule],
  templateUrl: './course-details.html',
  styleUrl: './course-details.css',
})
export class CourseDetails implements OnInit {
  private route = inject(ActivatedRoute);
  private courseService = inject(CourseService);
  private userService = inject(UserService);

  course = signal<Course | null>(null);
  instructorName = signal<string | null>(null);
  lessons = signal<Lesson[]>([]);
  isLoading = signal(true);
  error = signal<string | null>(null);

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadData(+id);
    } else {
      this.error.set('Invalid course ID');
      this.isLoading.set(false);
    }
  }

  loadData(id: number): void {
    this.isLoading.set(true);

    this.courseService.getCourse(id).subscribe({
      next: (course) => {
        this.course.set(course);
        this.checkLoading();

        // Fetch instructor details
        this.userService.getUser(course.createdBy).subscribe({
          next: (user) => this.instructorName.set(user.fullName),
          error: (err) => console.error('Error fetching instructor:', err),
        });
      },
      error: (err) => {
        console.error('Error fetching course:', err);
        this.error.set('Failed to load course details.');
        this.isLoading.set(false);
      },
    });

    this.courseService.getLessons(id).subscribe({
      next: (lessons) => {
        this.lessons.set(lessons.sort((a, b) => a.order - b.order));
        this.checkLoading();
      },
      error: (err) => {
        console.error('Error fetching lessons:', err);
      },
    });
  }

  private checkLoading() {
    if (this.course()) {
      this.isLoading.set(false);
    }
  }
}

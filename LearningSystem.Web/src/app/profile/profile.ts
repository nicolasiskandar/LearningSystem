import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { User } from '../models/user.model';
import { Course } from '../models/course.model';

@Component({
  selector: 'app-profile',
  imports: [CommonModule, RouterModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})
export class Profile implements OnInit {
  private userService = inject(UserService);
  private router = inject(Router);

  user = signal<User | null>(null);
  createdCourses = signal<Course[]>([]);
  enrolledCourses = signal<Course[]>([]);
  isLoading = signal(true);
  error = signal<string | null>(null);
  activeTab = signal<'created' | 'enrolled'>('created');

  ngOnInit(): void {
    this.loadProfile();
  }

  logout(): void {
    this.userService.logout();
    this.router.navigate(['/login']);
  }

  loadProfile(): void {
    this.isLoading.set(true);
    this.userService.getMe().subscribe({
      next: (user) => {
        this.user.set(user);
        this.loadCourses(user.id);
      },
      error: (err) => {
        console.error('Error loading profile:', err);
        this.error.set('Failed to load profile. Please try again.');
        this.isLoading.set(false);
      },
    });
  }

  loadCourses(userId: number): void {
    this.userService.getCreatedCourses(userId).subscribe({
      next: (courses) => {
        this.createdCourses.set(courses);
        // If user has no created courses but has enrolled, switch default tab
        if (courses.length === 0) {
          this.activeTab.set('enrolled');
        }
      },
      error: (err) => console.error('Error loading created courses:', err),
    });

    this.userService.getEnrolledCourses(userId).subscribe({
      next: (courses) => {
        this.enrolledCourses.set(courses);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Error loading enrolled courses:', err);
        this.isLoading.set(false);
      },
    });
  }

  setActiveTab(tab: 'created' | 'enrolled'): void {
    this.activeTab.set(tab);
  }
}

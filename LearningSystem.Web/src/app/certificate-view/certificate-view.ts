import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CertificateService } from '../services/certificate.service';
import { CourseService } from '../services/course.service';
import { UserService } from '../services/user.service';
import { Certificate } from '../models/certificate.model';
import { Course } from '../models/course.model';
import { User } from '../models/user.model';

@Component({
  selector: 'app-certificate-view',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './certificate-view.html',
  styleUrls: ['./certificate-view.css']
})
export class CertificateView implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private certificateService = inject(CertificateService);
  private courseService = inject(CourseService);
  private userService = inject(UserService);

  certificate = signal<Certificate | null>(null);
  course = signal<Course | null>(null);
  user = signal<User | null>(null);
  loading = signal(true);
  error = signal<string | null>(null);

  ngOnInit(): void {
    const courseIdParam = this.route.snapshot.paramMap.get('courseId');
    const courseId = Number(courseIdParam);
    const currentUser = this.userService.currentUser();

    if (!currentUser || isNaN(courseId)) {
      this.error.set('Invalid certificate request.');
      this.loading.set(false);
      return;
    }

    this.certificateService.getCertificateForCourse(currentUser.id, courseId).subscribe({
      next: (cert) => {
        if (!cert) {
            this.error.set('Certificate not found.');
            this.loading.set(false);
            return;
        }
        this.certificate.set(cert);
        this.fetchDetails(cert.courseId, cert.userId);
      },
      error: (err) => {
        this.error.set('Certificate not found or you are not authorized to view it.');
        this.loading.set(false);
      }
    });
  }

  fetchDetails(courseId: number, userId: number): void {
    this.courseService.getCourse(courseId).subscribe({
      next: (course) => {
        this.course.set(course);
        // We set loading false only when we have at least the course and cert
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Error fetching course:', err);
        this.loading.set(false);
      }
    });

    const currentUser = this.userService.currentUser();
    if (currentUser && currentUser.id === userId) {
      // Create a User object from AuthResponse if needed, or just use fullName
      this.user.set({
        id: currentUser.id,
        fullName: currentUser.fullName,
        email: currentUser.email,
        roleName: currentUser.roleName,
        createdAt: '' // We don't have this in AuthResponse
      });
    } else {
        // If it's another user, we'd fetch them with getUser(userId)
        this.userService.getUser(userId).subscribe({
            next: (user) => this.user.set(user),
            error: (err) => console.error('Error fetching user:', err)
        });
    }
  }

  printCertificate(): void {
    window.print();
  }

  getIssueDate(): string {
    // Since we don't have issue date in the model yet, we can use a placeholder or current date
    // In a real app, this should come from the backend.
    return new Date().toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    });
  }
}

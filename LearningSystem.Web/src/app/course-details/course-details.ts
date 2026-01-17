import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CourseService } from '../services/course.service';
import { UserService } from '../services/user.service';
import { Course, Lesson } from '../models/course.model';
import { Certificate } from '../models/certificate.model';
import { CertificateService } from '../services/certificate.service';

@Component({
  selector: 'app-course-details',
  imports: [CommonModule, RouterModule],
  templateUrl: './course-details.html',
  styleUrl: './course-details.css',
})
export class CourseDetails implements OnInit {
  private route = inject(ActivatedRoute);
  protected router = inject(Router);
  private courseService = inject(CourseService);
  private userService = inject(UserService);
  private certificateService = inject(CertificateService);

  course = signal<Course | null>(null);
  instructorName = signal<string | null>(null);
  lessons = signal<Lesson[]>([]);
  isLoading = signal(true);
  error = signal<string | null>(null);
  isEnrolling = signal(false);
  isEnrolled = signal(false);
  allLessonsCompleted = signal(false);
  hasCertificate = signal(false);
  canGenerateCertificate = signal(false);

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
        this.checkEnrollmentStatus(id);
        this.checkCertificateStatus(id);

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

  checkEnrollmentStatus(courseId: number): void {
    const user = this.userService.currentUser();
    if (user) {
      this.userService.getEnrolledCourses(user.id).subscribe({
        next: (courses) => {
          this.isEnrolled.set(courses.some((c) => c.id === courseId));
          if (this.isEnrolled()) {
            this.checkAllLessonsCompleted(courseId, user.id);
          }
        },
        error: (err) => console.error('Error checking enrollment status:', err),
      });
    }
  }

  enroll(): void {
    const token = localStorage.getItem('user');
    if (!token) {
      this.router.navigate(['/login']);
      return;
    }

    const currentCourse = this.course();
    if (!currentCourse) return;

    this.isEnrolling.set(true);
    this.courseService.enrollCourse(currentCourse.id).subscribe({
      next: () => {
        alert('Successfully enrolled!');
        this.isEnrolling.set(false);
        this.isEnrolled.set(true);
      },
      error: (err) => {
        console.error('Enrollment failed', err);
        this.isEnrolling.set(false);
        if (err.status === 401) {
          this.router.navigate(['/login']);
        } else {
          alert('Enrollment failed. Please try again.');
        }
      },
    });
  }

  startCourse(): void {
    const currentCourse = this.course();
    const currentLessons = this.lessons();
    if (currentCourse && currentLessons.length > 0) {
      // In a real app, we might check for the last completed lesson
      this.router.navigate([
        '/courses',
        currentCourse.id,
        'learn',
        'lecture',
        currentLessons[0].id,
      ]);
    } else {
      alert('This course has no lessons yet.');
    }
  }

  navigateToLesson(lessonId: number): void {
    const courseId = this.course()?.id;
    if (courseId && this.isEnrolled()) {
      this.router.navigate(['/courses', courseId, 'learn', 'lecture', lessonId]);
    }
  }

  private checkLoading() {
    if (this.course()) {
      this.isLoading.set(false);
    }
  }

  checkCertificateStatus(courseId: number): void {
    this.userService.getMe().subscribe({
      next: (user) => {
        if (user) {
          this.certificateService.getCertificateForCourse(user.id, courseId).subscribe({
            next: (cert) => {
              this.hasCertificate.set(cert != null);
              this.updateCanGenerateCertificate();
            },
            error: (err) => {
              if (err.status !== 404) {
                console.error('Error checking certificate status:', err);
              }
              this.hasCertificate.set(false);
              this.updateCanGenerateCertificate();
            },
          });
        }
      },
      error: (error) => {
        console.error('Error fetching user:', error);
      },
    });
  }

  checkAllLessonsCompleted(courseId: number, userId: number): void {
    this.courseService.areAllLessonsCompleted(courseId, userId).subscribe({
      next: (allCompleted) => {
        this.allLessonsCompleted.set(allCompleted);
        this.updateCanGenerateCertificate();
      },
      error: (err) => {
        console.error('Error checking lesson completion:', err);
      },
    });
  }

  updateCanGenerateCertificate(): void {
    this.canGenerateCertificate.set(
      this.isEnrolled() && this.allLessonsCompleted() && !this.hasCertificate()
    );
  }

  generateCertificate(): void {
    const course = this.course();
    if (!course) return;

    this.userService.getMe().subscribe({
      next: (user) => {
        if (!user) {
          alert('You must be logged in to generate a certificate.');
          this.router.navigate(['/login']);
          return;
        }

        this.certificateService.generateCertificate(user.id, course.id).subscribe({
          next: (cert) => {
            this.hasCertificate.set(true);
            this.updateCanGenerateCertificate();
            alert('Certificate generated successfully! You can view it in "My Certificates".');
          },
          error: (err) => {
            if (err.status === 400) {
              alert(err.error.message);
            } else if (err.status === 403) {
              alert('You have not completed all lessons in this course.');
            } else {
              alert('An error occurred while generating the certificate.');
            }
          },
        });
      },
      error: (err) => {
        console.error('Error fetching user:', err);
        alert('Unable to verify user. Please log in again.');
        this.router.navigate(['/login']);
      },
    });
  }
}

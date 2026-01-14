import { Component, OnInit, signal } from '@angular/core';
import { CertificateService } from '../services/certificate.service';
import { UserService } from '../services/user.service';
import { CourseService } from '../services/course.service';
import { CommonModule } from '@angular/common';
import { Certificate } from '../models/certificate.model';
import { Course } from '../models/course.model';

@Component({
  selector: 'app-my-certificates',
  templateUrl: './my-certificates.html',
  styleUrls: ['./my-certificates.css'],
  imports: [CommonModule],
})
export class MyCertificatesComponent implements OnInit {
  // Use a signal for certificates
  certificates = signal<(Certificate & { course?: Course })[]>([]);
  loading = signal(true);

  constructor(
    private certificateService: CertificateService,
    private userService: UserService,
    private courseService: CourseService
  ) {}

  ngOnInit(): void {
    this.loading.set(true);

    this.userService.getMe().subscribe({
      next: (user) => {
        if (user) {
          this.certificateService.getUserCertificates(user.id).subscribe({
            next: (data) => {
              this.certificates.set(data);
              this.loadCourseDetails();
            },
            error: (error) => {
              console.error('Error fetching certificates:', error);
              this.loading.set(false);
            },
          });
        }
      },
      error: (error) => {
        console.error('Error fetching user:', error);
        this.loading.set(false);
      },
    });
  }

  private loadCourseDetails(): void {
    const certs = this.certificates();
    if (certs.length === 0) {
      this.loading.set(false);
      return;
    }

    let loadedCount = 0;
    certs.forEach((cert, index) => {
      this.courseService.getCourse(cert.courseId).subscribe({
        next: (course) => {
          const updatedCerts = [...this.certificates()];
          updatedCerts[index] = { ...cert, course };
          this.certificates.set(updatedCerts);

          loadedCount++;
          if (loadedCount === certs.length) {
            this.loading.set(false); // all course details loaded
          }
        },
        error: (err) => {
          console.error('Error fetching course details:', err);
          loadedCount++;
          if (loadedCount === certs.length) this.loading.set(false);
        },
      });
    });
  }

  downloadCertificate(url: string) {
    window.open(url, '_blank');
  }

  viewCourse(courseId: number) {
    window.open(`/courses/${courseId}`, '_blank');
  }
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserService } from './user.service';
import { Certificate } from '../models/certificate.model';

@Injectable({
  providedIn: 'root',
})
export class CertificateService {
  private apiUrl = 'http://localhost:5142/api/Certificates';

  constructor(private http: HttpClient, private userService: UserService) {}

  generateCertificate(userId: number, courseId: number): Observable<Certificate> {
    return this.http.post<Certificate>(
      `${this.apiUrl}/generate/${userId}/${courseId}`,
      {},
      { headers: this.userService.getHeadersForLogin() }
    );
  }

  getCertificateForCourse(userId: number, courseId: number): Observable<Certificate> {
    return this.http.get<Certificate>(`${this.apiUrl}/${userId}/${courseId}`, {
      headers: this.userService.getHeadersForLogin(),
    });
  }

  getUserCertificates(userId: number): Observable<Certificate[]> {
    return this.http.get<Certificate[]>(`${this.apiUrl}/user/${userId}`, {
      headers: this.userService.getHeadersForLogin(),
    });
  }
}

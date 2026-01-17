import { Routes } from '@angular/router';
import { Courses } from './courses/courses';
import { CourseDetails } from './course-details/course-details';
import { LoginComponent } from './login/login';
import { RegisterComponent } from './register/register';
import { MyCourses } from './my-courses/my-courses';
import { Profile } from './profile/profile';
import { ChangePasswordComponent } from './change-password/change-password';
import { LessonPlayer } from './lesson-player/lesson-player';
import { MyCertificatesComponent } from './my-certificates/my-certificates';
import { AdminUsersComponent } from './admin/users/admin-users';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'profile', component: Profile },
  { path: 'change-password', component: ChangePasswordComponent },
  { path: 'courses', component: Courses },
  { path: 'my-courses', component: MyCourses },
  { path: 'courses/:courseId/learn/lecture/:lessonId', component: LessonPlayer },
  { path: 'courses/:id', component: CourseDetails },
  { path: 'certificates', component: MyCertificatesComponent },
  { path: 'admin/users', component: AdminUsersComponent },
  { path: '', redirectTo: 'courses', pathMatch: 'full' },
];

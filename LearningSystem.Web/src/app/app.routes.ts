import { Routes } from '@angular/router';
import { Courses } from './courses/courses';
import { CourseDetails } from './course-details/course-details';
import { LoginComponent } from './login/login';
import { RegisterComponent } from './register/register';
import { MyCourses } from './my-courses/my-courses';
import { Profile } from './profile/profile';
import { ChangePasswordComponent } from './change-password/change-password';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'profile', component: Profile },
  { path: 'change-password', component: ChangePasswordComponent },
  { path: 'courses', component: Courses },
  { path: 'my-courses', component: MyCourses },
  { path: 'courses/:id', component: CourseDetails },
  { path: '', redirectTo: 'courses', pathMatch: 'full' }
];
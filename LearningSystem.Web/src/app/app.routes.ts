import { Routes } from '@angular/router';
import { Courses } from './courses/courses';
import { CourseDetails } from './course-details/course-details';

export const routes: Routes = [
  { path: 'courses', component: Courses },
  { path: 'courses/:id', component: CourseDetails },
  { path: '', redirectTo: 'courses', pathMatch: 'full' }
];
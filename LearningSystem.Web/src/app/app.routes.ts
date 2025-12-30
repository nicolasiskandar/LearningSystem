import { Routes } from '@angular/router';
import { Courses } from './courses/courses';

export const routes: Routes = [
  { path: 'courses', component: Courses },
  { path: '', redirectTo: 'courses', pathMatch: 'full' }
];
import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css',
})
export class Sidebar {
  userService = inject(UserService);

  isAdmin() {
    return this.userService.currentUser()?.roleName === 'SuperAdmin';
  }

  isInstructor() {
    return this.userService.currentUser()?.roleName === 'Instructor';
  }

  hasAdminAccess() {
    const role = this.userService.currentUser()?.roleName;
    return role === 'SuperAdmin' || role === 'Instructor';
  }
}

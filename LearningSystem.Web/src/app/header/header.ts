import { Component, inject, OnInit } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-header',
  imports: [RouterLink, CommonModule, FormsModule],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header implements OnInit {
  userService = inject(UserService);
  router = inject(Router);
  searchTerm = '';

  ngOnInit() {
    if (this.userService.currentUser()) {
      this.userService.getMe().subscribe((user) => {
        const current = this.userService.currentUser();
        if (current) {
          const updatedUser = {
            ...current,
            ...user,
          };
          this.userService.currentUser.set(updatedUser);
          localStorage.setItem('user', JSON.stringify(updatedUser));
        }
      });
    }
  }

  search() {
    this.router.navigate(['/courses'], { queryParams: { searchTerm: this.searchTerm } });
  }
}

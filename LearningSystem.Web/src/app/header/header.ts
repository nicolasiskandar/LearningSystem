import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UserService } from '../services/user.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-header',
  imports: [RouterLink, CommonModule],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header implements OnInit {
  userService = inject(UserService);

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
}

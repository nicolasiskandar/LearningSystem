import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private userService = inject(UserService);
  private router = inject(Router);

  loginForm: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });

  errorMessage = signal('');

  onSubmit() {
    if (this.loginForm.valid) {
      this.userService.login(this.loginForm.value).subscribe({
        next: () => {
          this.router.navigate(['/courses']);
        },
        error: (err) => {
          console.error('Login failed', err);
          this.errorMessage.set(err.error?.message || err.error || 'Invalid email or password');
        },
      });
    }
  }
}

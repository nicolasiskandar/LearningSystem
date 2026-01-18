import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { User, CreateUserDto, UpdateUserDto } from '../../models/user.model';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-admin-users',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-users.html',
  styleUrls: ['./admin-users.css'],
})
export class AdminUsersComponent implements OnInit {
  private userService = inject(UserService);
  private router = inject(Router);

  users = signal<User[]>([]);
  isLoading = signal<boolean>(false);
  isNotAuthorized = signal<boolean>(false);

  // Modal state
  showModal = signal<boolean>(false);
  modalMode = signal<'create' | 'edit'>('create');
  selectedUser = signal<User | null>(null);

  // Form model
  formData: CreateUserDto | UpdateUserDto = {
    fullName: '',
    email: '',
    roleName: 'student',
    password: '',
  };

  ngOnInit() {
    if (!this.userService.currentUser()) {
      this.router.navigate(['/login']);
      return;
    }
    this.loadUsers();
  }

  loadUsers() {
    this.isLoading.set(true);
    this.userService.getUsers().subscribe({
      next: (users) => {
        this.users.set(users);
        this.isLoading.set(false);
      },
      error: (err: HttpErrorResponse) => {
        this.isLoading.set(false);
        if (err.status === 401) {
          this.router.navigate(['/login']);
        } else if (err.status === 403) {
          this.isNotAuthorized.set(true);
        } else {
          console.error('Error loading users', err);
          alert('Failed to load users');
        }
      },
    });
  }

  openCreateModal() {
    this.modalMode.set('create');
    this.formData = { fullName: '', email: '', roleName: 'Student', password: '' };
    this.showModal.set(true);
  }

  openEditModal(user: User) {
    this.modalMode.set('edit');
    this.selectedUser.set(user);
    this.formData = {
      fullName: user.fullName,
      email: user.email,
      roleName: user.roleName,
      password: '', // Password usually empty on edit unless changing
    };
    this.showModal.set(true);
  }

  closeModal() {
    this.showModal.set(false);
    this.selectedUser.set(null);
  }

  onSubmit() {
    if (this.modalMode() === 'create') {
      this.createUser();
    } else {
      this.updateUser();
    }
  }

  createUser() {
    this.userService.createUser(this.formData as CreateUserDto).subscribe({
      next: (newUser) => {
        this.users.update((users) => [...users, newUser]);
        this.closeModal();
      },
      error: (err) => {
        console.error('Error creating user', err);
        alert('Failed to create user');
      },
    });
  }

  updateUser() {
    const user = this.selectedUser();
    if (!user) return;

    const updateData = { ...this.formData };
    if (!updateData.password) {
      delete updateData.password;
    }

    this.userService.updateUser(user.id, updateData as UpdateUserDto).subscribe({
      next: (updatedUser) => {
        this.users.update((users) => users.map((u) => (u.id === updatedUser.id ? updatedUser : u)));
        this.closeModal();
      },
      error: (err) => {
        console.error('Error updating user', err);
        alert('Failed to update user');
      },
    });
  }

  deleteUser(user: User) {
    if (!confirm(`Are you sure you want to delete ${user.fullName}?`)) return;

    this.userService.deleteUser(user.id).subscribe({
      next: () => {
        this.users.update((users) => users.filter((u) => u.id !== user.id));
      },
      error: (err) => {
        console.error('Error deleting user', err);
        alert('Failed to delete user');
      },
    });
  }
}

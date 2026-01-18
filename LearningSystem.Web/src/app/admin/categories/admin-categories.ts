import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import {
  CategoryService,
  CreateCategoryDto,
  UpdateCategoryDto,
} from '../../services/category.service';
import { Category } from '../../models/category.model';
import { UserService } from '../../services/user.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-admin-categories',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-categories.html',
  styleUrls: ['./admin-categories.css'],
})
export class AdminCategoriesComponent implements OnInit {
  private categoryService = inject(CategoryService);
  private userService = inject(UserService);
  private router = inject(Router);

  categories = signal<Category[]>([]);
  isLoading = signal<boolean>(false);
  isNotAuthorized = signal<boolean>(false);

  // Modal state
  showModal = signal<boolean>(false);
  modalMode = signal<'create' | 'edit'>('create');
  selectedCategory = signal<Category | null>(null);

  // Form model
  formData: CreateCategoryDto | UpdateCategoryDto = {
    name: '',
  };

  ngOnInit() {
    if (!this.userService.currentUser()) {
      this.router.navigate(['/login']);
      return;
    }

    // Simple check if user is instructor or admin could be added here
    // For now rely on backend 403
    this.loadCategories();
  }

  loadCategories() {
    this.isLoading.set(true);
    this.categoryService.getCategories().subscribe({
      next: (categories) => {
        this.categories.set(categories);
        this.isLoading.set(false);
      },
      error: (err: HttpErrorResponse) => {
        this.isLoading.set(false);
        if (err.status === 401) {
          this.router.navigate(['/login']);
        } else if (err.status === 403) {
          this.isNotAuthorized.set(true);
        } else {
          console.error('Error loading categories', err);
          alert('Failed to load categories');
        }
      },
    });
  }

  openCreateModal() {
    this.modalMode.set('create');
    this.formData = { name: '' };
    this.showModal.set(true);
  }

  openEditModal(category: Category) {
    this.modalMode.set('edit');
    this.selectedCategory.set(category);
    this.formData = {
      name: category.name,
    };
    this.showModal.set(true);
  }

  closeModal() {
    this.showModal.set(false);
    this.selectedCategory.set(null);
  }

  onSubmit() {
    if (this.modalMode() === 'create') {
      this.createCategory();
    } else {
      this.updateCategory();
    }
  }

  createCategory() {
    this.categoryService.createCategory(this.formData as CreateCategoryDto).subscribe({
      next: (newCategory) => {
        this.categories.update((categories) => [...categories, newCategory]);
        this.closeModal();
      },
      error: (err) => {
        console.error('Error creating category', err);
        alert('Failed to create category');
      },
    });
  }

  updateCategory() {
    const category = this.selectedCategory();
    if (!category) return;

    this.categoryService.updateCategory(category.id, this.formData as UpdateCategoryDto).subscribe({
      next: (updatedCategory) => {
        this.categories.update((categories) =>
          categories.map((c) => (c.id === updatedCategory.id ? updatedCategory : c)),
        );
        this.closeModal();
      },
      error: (err) => {
        console.error('Error updating category', err);
        alert('Failed to update category');
      },
    });
  }

  deleteCategory(category: Category) {
    if (!confirm(`Are you sure you want to delete ${category.name}?`)) return;

    this.categoryService.deleteCategory(category.id).subscribe({
      next: () => {
        this.categories.update((categories) => categories.filter((c) => c.id !== category.id));
      },
      error: (err) => {
        console.error('Error deleting category', err);
        alert('Failed to delete category');
      },
    });
  }
}

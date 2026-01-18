import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CourseService } from '../../services/course.service';
import { Lesson, CreateLessonDto, UpdateLessonDto } from '../../models/course.model';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-admin-lessons',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-lessons.html',
  styleUrls: ['./admin-lessons.css'],
})
export class AdminLessonsComponent implements OnInit {
  private courseService = inject(CourseService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  courseId = signal<number>(0);
  lessons = signal<Lesson[]>([]);
  isLoading = signal<boolean>(false);

  // Modal state
  showModal = signal<boolean>(false);
  modalMode = signal<'create' | 'edit'>('create');
  selectedLesson = signal<Lesson | null>(null);

  // Form model
  formData = {
    title: '',
    content: '',
    videoUrl: '',
    order: 1,
    estimatedDuration: 10,
  };

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('courseId');
    if (id) {
      this.courseId.set(+id);
      this.loadLessons();
    } else {
      this.router.navigate(['/admin/courses']);
    }
  }

  loadLessons() {
    this.isLoading.set(true);
    this.courseService.getLessons(this.courseId()).subscribe({
      next: (lessons) => {
        this.lessons.set(lessons.sort((a, b) => a.order - b.order));
        this.isLoading.set(false);
      },
      error: (err: HttpErrorResponse) => {
        this.isLoading.set(false);
        console.error('Error loading lessons', err);
      },
    });
  }

  openCreateModal() {
    this.modalMode.set('create');
    const nextOrder = this.lessons().length > 0 
      ? Math.max(...this.lessons().map(l => l.order)) + 1 
      : 1;
      
    this.formData = {
      title: '',
      content: '',
      videoUrl: '',
      order: nextOrder,
      estimatedDuration: 10,
    };
    this.showModal.set(true);
  }

  openEditModal(lesson: Lesson) {
    this.modalMode.set('edit');
    this.selectedLesson.set(lesson);
    this.formData = {
      title: lesson.title,
      content: lesson.content,
      videoUrl: lesson.videoUrl || '',
      order: lesson.order,
      estimatedDuration: lesson.estimatedDuration,
    };
    this.showModal.set(true);
  }

  closeModal() {
    this.showModal.set(false);
    this.selectedLesson.set(null);
  }

  onSubmit() {
    if (this.modalMode() === 'create') {
      this.createLesson();
    } else {
      this.updateLesson();
    }
  }

  createLesson() {
    const dto: CreateLessonDto = {
      ...this.formData,
      courseId: this.courseId(),
    };

    this.courseService.createLesson(dto).subscribe({
      next: (newLesson) => {
        this.lessons.update((lessons) => [...lessons, newLesson].sort((a, b) => a.order - b.order));
        this.closeModal();
      },
      error: (err) => {
        console.error('Error creating lesson', err);
        alert('Failed to create lesson');
      },
    });
  }

  updateLesson() {
    const lesson = this.selectedLesson();
    if (!lesson) return;

    const dto: UpdateLessonDto = {
      ...this.formData,
      courseId: this.courseId(),
    };

    this.courseService.updateLesson(lesson.id, dto).subscribe({
      next: (updatedLesson) => {
        this.lessons.update((lessons) =>
          lessons.map((l) => (l.id === updatedLesson.id ? updatedLesson : l)).sort((a, b) => a.order - b.order),
        );
        this.closeModal();
      },
      error: (err) => {
        console.error('Error updating lesson', err);
        alert('Failed to update lesson');
      },
    });
  }

  deleteLesson(lesson: Lesson) {
    if (!confirm(`Are you sure you want to delete ${lesson.title}?`)) return;

    this.courseService.deleteLesson(lesson.id).subscribe({
      next: () => {
        this.lessons.update((lessons) => lessons.filter((l) => l.id !== lesson.id));
      },
      error: (err) => {
        console.error('Error deleting lesson', err);
        alert('Failed to delete lesson');
      },
    });
  }

  goBack() {
    this.router.navigate(['/admin/courses']);
  }
}

import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { QuizService } from '../../services/quiz.service';
import { CourseService } from '../../services/course.service';
import { UserService } from '../../services/user.service';
import { Quiz, CreateQuizDto, UpdateQuizDto } from '../../models/quiz.model';
import { Course, Lesson } from '../../models/course.model';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-admin-quizzes',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './admin-quizzes.html',
  styleUrls: ['./admin-quizzes.css'],
})
export class AdminQuizzesComponent implements OnInit {
  private quizService = inject(QuizService);
  private courseService = inject(CourseService);
  private userService = inject(UserService);
  private router = inject(Router);

  quizzes = signal<Quiz[]>([]);
  courses = signal<Course[]>([]);
  lessons = signal<Lesson[]>([]);
  isLoading = signal<boolean>(false);
  isNotAuthorized = signal<boolean>(false);

  // Modal state
  showModal = signal<boolean>(false);
  modalMode = signal<'create' | 'edit'>('create');
  selectedQuiz = signal<Quiz | null>(null);

  // Form model
  formData = {
    title: '',
    passingScore: 0,
    timeLimit: 0,
    courseId: 0,
    lessonId: 0,
  };

  ngOnInit() {
    const user = this.userService.currentUser();
    if (!user) {
      this.router.navigate(['/login']);
      return;
    }

    this.loadQuizzes();
    this.loadCourses();
  }

  loadQuizzes() {
    this.isLoading.set(true);
    this.quizService.getQuizzes().subscribe({
      next: (quizzes) => {
        this.quizzes.set(quizzes);
        this.isLoading.set(false);
      },
      error: (err: HttpErrorResponse) => {
        this.isLoading.set(false);
        if (err.status === 401) {
          this.router.navigate(['/login']);
        } else if (err.status === 403) {
          this.isNotAuthorized.set(true);
        } else {
          console.error('Error loading quizzes', err);
        }
      },
    });
  }

  loadCourses() {
    // Fetch a reasonable number of courses for the dropdown
    this.courseService.getCourses(1, 100).subscribe({
      next: (courses) => this.courses.set(courses),
      error: (err) => console.error('Error loading courses', err),
    });
  }

  loadLessons(courseId: number) {
    this.courseService.getLessons(courseId).subscribe({
      next: (lessons) => this.lessons.set(lessons),
      error: (err) => console.error('Error loading lessons', err),
    });
  }

  onCourseChange() {
    if (this.formData.courseId) {
      this.loadLessons(this.formData.courseId);
      this.formData.lessonId = 0; // Reset lesson selection
    } else {
      this.lessons.set([]);
    }
  }

  openCreateModal() {
    this.modalMode.set('create');
    this.formData = {
      title: '',
      passingScore: 70, // Default passing score
      timeLimit: 30, // Default time limit
      courseId: this.courses().length > 0 ? this.courses()[0].id : 0,
      lessonId: 0,
    };
    if (this.formData.courseId) {
      this.loadLessons(this.formData.courseId);
    }
    this.showModal.set(true);
  }

  openEditModal(quiz: Quiz) {
    this.modalMode.set('edit');
    this.selectedQuiz.set(quiz);

    this.formData = {
      title: quiz.title,
      passingScore: quiz.passingScore,
      timeLimit: quiz.timeLimit,
      courseId: quiz.courseId,
      lessonId: quiz.lessonId,
    };
    
    // Load lessons for the quiz's course so the dropdown works and shows the current lesson
    this.loadLessons(quiz.courseId);
    
    this.showModal.set(true);
  }

  closeModal() {
    this.showModal.set(false);
    this.selectedQuiz.set(null);
  }

  onSubmit() {
    if (this.modalMode() === 'create') {
      this.createQuiz();
    } else {
      this.updateQuiz();
    }
  }

  createQuiz() {
    const dto: CreateQuizDto = {
      title: this.formData.title,
      passingScore: this.formData.passingScore,
      timeLimit: this.formData.timeLimit,
      courseId: Number(this.formData.courseId),
      lessonId: Number(this.formData.lessonId),
    };

    this.quizService.createQuiz(dto).subscribe({
      next: (newQuiz) => {
        this.quizzes.update((quizzes) => [...quizzes, newQuiz]);
        this.closeModal();
      },
      error: (err) => {
        console.error('Error creating quiz', err);
        alert('Failed to create quiz');
      },
    });
  }

  updateQuiz() {
    const quiz = this.selectedQuiz();
    if (!quiz) return;

    // Note: CreateQuizDto and UpdateQuizDto are slightly different. 
    // UpdateQuizDto doesn't typically allow changing Course/Lesson, but check backend DTO.
    // I checked UpdateQuizDto: it only has Title, PassingScore, TimeLimit.
    
    const dto: UpdateQuizDto = {
      title: this.formData.title,
      passingScore: this.formData.passingScore,
      timeLimit: this.formData.timeLimit,
    };

    this.quizService.updateQuiz(quiz.id, dto).subscribe({
      next: (updatedQuiz) => {
        // The backend returns the updated quiz, but we should make sure we keep the courseId/lessonId if the backend doesn't return them or if we want to be safe, 
        // though the backend response should be a full QuizDto.
        // Let's assume updatedQuiz is complete.
        this.quizzes.update((quizzes) =>
          quizzes.map((q) => (q.id === updatedQuiz.id ? updatedQuiz : q)),
        );
        this.closeModal();
      },
      error: (err) => {
        console.error('Error updating quiz', err);
        alert('Failed to update quiz');
      },
    });
  }

  deleteQuiz(quiz: Quiz) {
    if (!confirm(`Are you sure you want to delete ${quiz.title}?`)) return;

    this.quizService.deleteQuiz(quiz.id).subscribe({
      next: () => {
        this.quizzes.update((quizzes) => quizzes.filter((q) => q.id !== quiz.id));
      },
      error: (err) => {
        console.error('Error deleting quiz', err);
        alert('Failed to delete quiz');
      },
    });
  }
  
  getCourseName(courseId: number): string {
    const course = this.courses().find(c => c.id === courseId);
    return course ? course.title : `Course #${courseId}`;
  }

  manageQuestions(quizId: number) {
    this.router.navigate(['/admin/quizzes', quizId, 'questions']);
  }
}

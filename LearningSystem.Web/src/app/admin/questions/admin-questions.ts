import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { QuizService } from '../../services/quiz.service';
import { UserService } from '../../services/user.service';
import {
  Question,
  Quiz,
  QuestionType,
  CreateQuestionDto,
  UpdateQuestionDto,
  CreateAnswerDto,
  UpdateAnswerDto,
  Answer
} from '../../models/quiz.model';

@Component({
  selector: 'app-admin-questions',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './admin-questions.html',
  styleUrls: ['./admin-questions.css'],
})
export class AdminQuestionsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private quizService = inject(QuizService);
  private userService = inject(UserService);

  quizId!: number;
  quiz = signal<Quiz | null>(null);
  questions = signal<Question[]>([]);
  questionTypes = signal<QuestionType[]>([]);
  isLoading = signal<boolean>(false);

  // Modal state
  showModal = signal<boolean>(false);
  modalMode = signal<'create' | 'edit'>('create');
  selectedQuestion = signal<Question | null>(null);

  // Form model
  formData = {
    questionText: '',
    questionTypeId: 0,
    order: 0,
    answers: [] as { id?: number; answerText: string; isCorrect: boolean }[],
  };

  ngOnInit() {
    const user = this.userService.currentUser();
    if (!user) {
      this.router.navigate(['/login']);
      return;
    }

    this.quizId = Number(this.route.snapshot.paramMap.get('quizId'));
    if (this.quizId) {
      this.loadData();
    } else {
      this.router.navigate(['/admin/quizzes']);
    }
  }

  loadData() {
    this.isLoading.set(true);
    
    // Load Quiz Details
    this.quizService.getQuiz(this.quizId).subscribe({
      next: (quiz) => this.quiz.set(quiz),
      error: (err) => console.error('Error loading quiz', err),
    });

    // Load Questions
    this.quizService.getQuestionsByQuizId(this.quizId).subscribe({
      next: (questions) => {
        this.questions.set(questions.sort((a, b) => a.order - b.order));
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Error loading questions', err);
        this.isLoading.set(false);
      },
    });

    // Load Question Types
    this.quizService.getQuestionTypes().subscribe({
      next: (types) => this.questionTypes.set(types),
      error: (err) => console.error('Error loading question types', err),
    });
  }

  openCreateModal() {
    this.modalMode.set('create');
    this.formData = {
      questionText: '',
      questionTypeId: this.questionTypes().length > 0 ? this.questionTypes()[0].id : 0,
      order: this.questions().length + 1,
      answers: [
        { answerText: '', isCorrect: false },
        { answerText: '', isCorrect: false },
      ],
    };
    this.showModal.set(true);
  }

  openEditModal(question: Question) {
    this.modalMode.set('edit');
    this.selectedQuestion.set(question);

    // Find type ID from name
    const type = this.questionTypes().find(t => t.name === question.questionType);
    const typeId = type ? type.id : (this.questionTypes().length > 0 ? this.questionTypes()[0].id : 0);

    this.formData = {
      questionText: question.questionText,
      questionTypeId: typeId,
      order: question.order,
      answers: question.answers.map(a => ({
        id: a.id,
        answerText: a.answerText,
        isCorrect: a.isCorrect
      })),
    };
    this.showModal.set(true);
  }

  closeModal() {
    this.showModal.set(false);
    this.selectedQuestion.set(null);
  }

  addAnswer() {
    this.formData.answers.push({ answerText: '', isCorrect: false });
  }

  removeAnswer(index: number) {
    this.formData.answers.splice(index, 1);
  }

  onSubmit() {
    if (this.modalMode() === 'create') {
      this.createQuestion();
    } else {
      this.updateQuestion();
    }
  }

  createQuestion() {
    const dto: CreateQuestionDto = {
      questionText: this.formData.questionText,
      questionTypeId: Number(this.formData.questionTypeId),
      order: this.formData.order,
      quizId: this.quizId,
      answers: this.formData.answers.map(a => ({
        answerText: a.answerText,
        isCorrect: a.isCorrect
      })),
    };

    this.quizService.createQuestion(dto).subscribe({
      next: (newQuestion) => {
        this.questions.update(qs => [...qs, newQuestion].sort((a, b) => a.order - b.order));
        this.closeModal();
      },
      error: (err) => {
        console.error('Error creating question', err);
        alert('Failed to create question');
      },
    });
  }

  updateQuestion() {
    const question = this.selectedQuestion();
    if (!question) return;

    const dto: UpdateQuestionDto = {
      questionText: this.formData.questionText,
      questionTypeId: Number(this.formData.questionTypeId),
      order: this.formData.order,
      answers: this.formData.answers.map(a => ({
        id: a.id, // Include ID for existing answers
        answerText: a.answerText,
        isCorrect: a.isCorrect
      })),
    };

    this.quizService.updateQuestion(question.id, dto).subscribe({
      next: (updatedQuestion) => {
        this.questions.update(qs => 
          qs.map(q => q.id === updatedQuestion.id ? updatedQuestion : q).sort((a, b) => a.order - b.order)
        );
        this.closeModal();
      },
      error: (err) => {
        console.error('Error updating question', err);
        alert('Failed to update question');
      },
    });
  }

  deleteQuestion(question: Question) {
    if (!confirm('Are you sure you want to delete this question?')) return;

    this.quizService.deleteQuestion(question.id).subscribe({
      next: () => {
        this.questions.update(qs => qs.filter(q => q.id !== question.id));
      },
      error: (err) => {
        console.error('Error deleting question', err);
        alert('Failed to delete question');
      },
    });
  }

  goBack() {
    this.router.navigate(['/admin/quizzes']);
  }
}

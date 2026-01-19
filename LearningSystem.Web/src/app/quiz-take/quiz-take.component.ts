import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { QuizService } from '../services/quiz.service';
import {
  Quiz,
  QuizAttempt,
  QuizAttemptAnswer,
  SubmitQuizAttempt,
} from '../models/quiz.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-quiz-take',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './quiz-take.component.html',
  styleUrl: './quiz-take.component.css',
})
export class QuizTakeComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private quizService = inject(QuizService);

  quiz = signal<Quiz | null>(null);
  attempt = signal<QuizAttempt | null>(null);
  result = signal<QuizAttempt | null>(null);
  
  selectedAnswers: { [questionId: number]: number } = {};
  
  loading = signal<boolean>(true);
  submitting = signal<boolean>(false);
  error = signal<string | null>(null);

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.loadQuiz(id);
    } else {
      this.error.set('Invalid Quiz ID');
      this.loading.set(false);
    }
  }

  loadQuiz(id: number) {
    this.quizService.getQuiz(id).subscribe({
      next: (quiz) => {
        this.quiz.set(quiz);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Error loading quiz', err);
        this.error.set('Failed to load quiz');
        this.loading.set(false);
      },
    });
  }

  startQuiz() {
    const quiz = this.quiz();
    if (!quiz) return;

    this.loading.set(true);
    this.quizService.createQuizAttempt(quiz.id).subscribe({
      next: (attempt) => {
        this.attempt.set(attempt);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Error creating attempt', err);
        this.error.set('Failed to start quiz');
        this.loading.set(false);
      },
    });
  }

  onSelectAnswer(questionId: number, answerId: number) {
    this.selectedAnswers[questionId] = answerId;
  }

  submitQuiz() {
    const attempt = this.attempt();
    if (!attempt) return;

    this.submitting.set(true);
    
    const answers: QuizAttemptAnswer[] = Object.keys(this.selectedAnswers).map(qId => ({
      questionId: Number(qId),
      answerId: this.selectedAnswers[Number(qId)]
    }));

    const submitDto: SubmitQuizAttempt = { answers };

    this.quizService.submitQuizAttempt(attempt.id, submitDto).subscribe({
      next: (result) => {
        this.result.set(result);
        this.submitting.set(false);
      },
      error: (err) => {
        console.error('Error submitting quiz', err);
        this.error.set('Failed to submit quiz');
        this.submitting.set(false);
      },
    });
  }
  
  goBack() {
    // If we have a quiz, try to go back to course, otherwise just go back in history or home
    const quiz = this.quiz();
    if (quiz) {
        this.router.navigate(['/courses', quiz.courseId]);
    } else {
        this.router.navigate(['/']);
    }
  }
}

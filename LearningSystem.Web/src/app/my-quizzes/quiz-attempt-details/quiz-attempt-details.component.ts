import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { QuizService } from '../../services/quiz.service';
import { Quiz, QuizAttempt } from '../../models/quiz.model';
import { switchMap } from 'rxjs';

@Component({
  selector: 'app-quiz-attempt-details',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './quiz-attempt-details.component.html',
  styleUrl: './quiz-attempt-details.component.css',
})
export class QuizAttemptDetailsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private quizService = inject(QuizService);

  attempt = signal<QuizAttempt | null>(null);
  quiz = signal<Quiz | null>(null);
  loading = signal<boolean>(true);

  ngOnInit() {
    const attemptId = this.route.snapshot.paramMap.get('id');
    if (attemptId) {
      this.loadData(+attemptId);
    }
  }

  loadData(attemptId: number) {
    this.quizService.getQuizAttempt(attemptId).pipe(
      switchMap((attempt) => {
        this.attempt.set(attempt);
        return this.quizService.getQuiz(attempt.quizId);
      })
    ).subscribe({
      next: (quiz) => {
        this.quiz.set(quiz);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Error loading quiz attempt details', err);
        this.loading.set(false);
      },
    });
  }

  getUserAnswerId(questionId: number): number | undefined {
    const attempt = this.attempt();
    if (!attempt) return undefined;
    const answer = attempt.answers.find(a => a.questionId === questionId);
    return answer?.answerId;
  }

  isUserAnswer(questionId: number, answerId: number): boolean {
    return this.getUserAnswerId(questionId) === answerId;
  }

  isCorrectAnswer(questionId: number, answerId: number): boolean {
    const quiz = this.quiz();
    if (!quiz) return false;
    const question = quiz.questions.find(q => q.id === questionId);
    const answer = question?.answers.find(a => a.id === answerId);
    return answer?.isCorrect ?? false;
  }

  getQuestionStatusClass(questionId: number): string {
      const userAnswerId = this.getUserAnswerId(questionId);
      if (!userAnswerId) return 'unanswered';
      
      return this.isCorrectAnswer(questionId, userAnswerId) ? 'correct' : 'incorrect';
  }
}

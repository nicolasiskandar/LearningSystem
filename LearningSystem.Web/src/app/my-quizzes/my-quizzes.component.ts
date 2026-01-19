import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { QuizService } from '../services/quiz.service';
import { UserService } from '../services/user.service';
import { QuizAttempt, Quiz } from '../models/quiz.model';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-my-quizzes',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './my-quizzes.component.html',
  styleUrl: './my-quizzes.component.css',
})
export class MyQuizzesComponent implements OnInit {
  private quizService = inject(QuizService);
  private userService = inject(UserService);

  attempts = signal<QuizAttempt[]>([]);
  quizTitles = signal<Map<number, string>>(new Map());
  loading = signal<boolean>(true);

  ngOnInit() {
    const user = this.userService.currentUser();
    if (user) {
      this.loadData(user.id);
    } else {
        this.loading.set(false);
    }
  }

  loadData(userId: number) {
    forkJoin({
      attempts: this.quizService.getQuizAttemptsByUser(userId),
      quizzes: this.quizService.getQuizzes()
    }).subscribe({
      next: ({ attempts, quizzes }) => {
        this.attempts.set(attempts);
        
        const titleMap = new Map<number, string>();
        quizzes.forEach(q => titleMap.set(q.id, q.title));
        this.quizTitles.set(titleMap);
        
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Error loading my quizzes', err);
        this.loading.set(false);
      }
    });
  }

  getQuizTitle(quizId: number): string {
    return this.quizTitles().get(quizId) || 'Unknown Quiz';
  }
}

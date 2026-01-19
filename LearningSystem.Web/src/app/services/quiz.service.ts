import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  Quiz,
  CreateQuizAttempt,
  QuizAttempt,
  SubmitQuizAttempt,
  CreateQuizDto,
  UpdateQuizDto,
  Question,
  QuestionType,
  CreateQuestionDto,
  UpdateQuestionDto,
} from '../models/quiz.model';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root',
})
export class QuizService {
  private http = inject(HttpClient);
  private userService = inject(UserService);
  private apiUrl = 'http://localhost:5142/api';

  // Quiz Endpoints
  getQuizzes(): Observable<Quiz[]> {
    const headers = this.userService.getHeadersForLogin();
    return this.http.get<Quiz[]>(`${this.apiUrl}/Quizzes`, { headers });
  }

  getQuiz(id: number): Observable<Quiz> {
    const headers = this.userService.getHeadersForLogin();
    return this.http.get<Quiz>(`${this.apiUrl}/Quizzes/${id}`, { headers });
  }

  createQuiz(quiz: CreateQuizDto): Observable<Quiz> {
    const headers = this.userService.getHeadersForLogin();
    return this.http.post<Quiz>(`${this.apiUrl}/Quizzes`, quiz, { headers });
  }

  updateQuiz(id: number, quiz: UpdateQuizDto): Observable<Quiz> {
    const headers = this.userService.getHeadersForLogin();
    return this.http.put<Quiz>(`${this.apiUrl}/Quizzes/${id}`, quiz, { headers });
  }

  deleteQuiz(id: number): Observable<void> {
    const headers = this.userService.getHeadersForLogin();
    return this.http.delete<void>(`${this.apiUrl}/Quizzes/${id}`, { headers });
  }

  // Question Endpoints
  getQuestionsByQuizId(quizId: number): Observable<Question[]> {
    const headers = this.userService.getHeadersForLogin();
    return this.http.get<Question[]>(`${this.apiUrl}/Questions/quiz/${quizId}`, { headers });
  }

  createQuestion(question: CreateQuestionDto): Observable<Question> {
    const headers = this.userService.getHeadersForLogin();
    return this.http.post<Question>(`${this.apiUrl}/Questions`, question, { headers });
  }

  updateQuestion(id: number, question: UpdateQuestionDto): Observable<Question> {
    const headers = this.userService.getHeadersForLogin();
    return this.http.put<Question>(`${this.apiUrl}/Questions/${id}`, question, { headers });
  }

  deleteQuestion(id: number): Observable<void> {
    const headers = this.userService.getHeadersForLogin();
    return this.http.delete<void>(`${this.apiUrl}/Questions/${id}`, { headers });
  }

  getQuestionTypes(): Observable<QuestionType[]> {
    const headers = this.userService.getHeadersForLogin();
    return this.http.get<QuestionType[]>(`${this.apiUrl}/QuestionTypes`, { headers });
  }

  // Quiz Attempt Endpoints
  createQuizAttempt(quizId: number): Observable<QuizAttempt> {
    const headers = this.userService.getHeadersForLogin();
    const dto: CreateQuizAttempt = { quizId };
    return this.http.post<QuizAttempt>(`${this.apiUrl}/QuizAttempts`, dto, { headers });
  }

  getQuizAttempt(id: number): Observable<QuizAttempt> {
    const headers = this.userService.getHeadersForLogin();
    return this.http.get<QuizAttempt>(`${this.apiUrl}/QuizAttempts/${id}`, { headers });
  }

  getQuizAttemptsByUser(userId: number): Observable<QuizAttempt[]> {
    const headers = this.userService.getHeadersForLogin();
    return this.http.get<QuizAttempt[]>(`${this.apiUrl}/QuizAttempts/user/${userId}`, { headers });
  }

  submitQuizAttempt(id: number, dto: SubmitQuizAttempt): Observable<QuizAttempt> {
    const headers = this.userService.getHeadersForLogin();
    return this.http.post<QuizAttempt>(
      `${this.apiUrl}/QuizAttempts/${id}/submit`,
      dto,
      { headers }
    );
  }
}

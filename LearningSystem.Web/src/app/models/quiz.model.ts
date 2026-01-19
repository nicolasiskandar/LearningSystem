export interface Answer {
  id: number;
  answerText: string;
  isCorrect: boolean;
  questionId: number;
}

export interface Question {
  id: number;
  questionText: string;
  questionType: string;
  order: number;
  quizId: number;
  answers: Answer[];
}

export interface Quiz {
  id: number;
  title: string;
  passingScore: number;
  timeLimit: number; // in minutes
  courseId: number;
  lessonId: number;
  questions: Question[];
}

export interface QuizAttemptAnswer {
  questionId: number;
  answerId: number;
}

export interface QuizAttempt {
  id: number;
  quizId: number;
  userId: number;
  score: number;
  attemptDate?: Date;
  answers: QuizAttemptAnswer[];
}

export interface CreateQuizAttempt {
  quizId: number;
}

export interface SubmitQuizAttempt {
  answers: QuizAttemptAnswer[];
}

export interface CreateQuizDto {
  title: string;
  passingScore: number;
  timeLimit: number;
  courseId: number;
  lessonId: number;
}

export interface UpdateQuizDto {
  title: string;
  passingScore: number;
  timeLimit: number;
}

export interface QuestionType {
  id: number;
  name: string;
}

export interface CreateAnswerDto {
  answerText: string;
  isCorrect: boolean;
}

export interface UpdateAnswerDto extends CreateAnswerDto {
  id?: number; // Optional for new answers added during update
}

export interface CreateQuestionDto {
  questionText: string;
  questionTypeId: number;
  order: number;
  quizId: number;
  answers: CreateAnswerDto[];
}

export interface UpdateQuestionDto {
  questionText: string;
  questionTypeId: number;
  order: number;
  answers: UpdateAnswerDto[];
}

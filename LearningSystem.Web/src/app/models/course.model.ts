export interface Course {
  id: number;
  title: string;
  shortDescription: string;
  longDescription: string;
  category: string;
  difficulty: string;
  createdBy: number;
  thumbnail: string;
  createdAt: string;
  isPublished: boolean;
}

export interface Lesson {
  id: number;
  courseId: number;
  title: string;
  content: string;
  videoUrl: string;
  order: number;
  estimatedDuration: number;
  isCompleted?: boolean;
}

export interface CreateCourseDto {
  title: string;
  shortDescription: string;
  longDescription: string;
  categoryId: number;
  difficulty: string;
  createdBy: number;
  thumbnail: string;
  isPublished: boolean;
}

export interface UpdateCourseDto {
  title: string;
  shortDescription: string;
  longDescription: string;
  categoryId: number;
  difficulty: string;
  thumbnail: string;
  isPublished: boolean;
}

export interface CreateLessonDto {
  courseId: number;
  title: string;
  content: string;
  videoUrl?: string;
  order: number;
  estimatedDuration: number;
}

export interface UpdateLessonDto {
  courseId: number;
  title: string;
  content: string;
  videoUrl?: string;
  order: number;
  estimatedDuration: number;
}

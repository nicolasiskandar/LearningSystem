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
}
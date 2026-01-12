import { Component, OnInit, inject, signal, computed, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { CourseService } from '../services/course.service';
import { UserService } from '../services/user.service';
import { Course, Lesson } from '../models/course.model';

@Component({
  selector: 'app-lesson-player',
  imports: [CommonModule, RouterModule],
  templateUrl: './lesson-player.html',
  styleUrl: './lesson-player.css',
})
export class LessonPlayer implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private courseService = inject(CourseService);
  private userService = inject(UserService);
  private sanitizer = inject(DomSanitizer);

  courseId = signal<number | null>(null);
  lessonId = signal<number | null>(null);

  course = signal<Course | null>(null);
  lessons = signal<Lesson[]>([]);
  currentLesson = signal<Lesson | null>(null);

  isLoading = signal(true);
  isCompleting = signal(false);

  safeVideoUrl = computed(() => {
    const lesson = this.currentLesson();
    if (!lesson || !lesson.videoUrl) return null;

    let url = lesson.videoUrl;

    if (url.includes('youtube.com/watch?v=')) {
      const videoId = url.split('v=')[1].split('&')[0];
      url = `https://www.youtube.com/embed/${videoId}`;
    } else if (url.includes('youtu.be/')) {
      const videoId = url.split('youtu.be/')[1].split('?')[0];
      url = `https://www.youtube.com/embed/${videoId}`;
    }

    return this.sanitizer.bypassSecurityTrustResourceUrl(url);
  });

  nextLessonId = computed(() => {
    const all = this.lessons();
    const current = this.currentLesson();
    if (!all.length || !current) return null;

    const currentIndex = all.findIndex((l) => l.id === current.id);
    if (currentIndex >= 0 && currentIndex < all.length - 1) {
      return all[currentIndex + 1].id;
    }
    return null;
  });

  prevLessonId = computed(() => {
    const all = this.lessons();
    const current = this.currentLesson();
    if (!all.length || !current) return null;

    const currentIndex = all.findIndex((l) => l.id === current.id);
    if (currentIndex > 0) {
      return all[currentIndex - 1].id;
    }
    return null;
  });

  constructor() {
    effect(() => {}, { allowSignalWrites: true });
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      const cId = params.get('courseId');
      const lId = params.get('lessonId');

      if (cId) {
        const courseIdNum = +cId;
        if (this.courseId() !== courseIdNum) {
          this.courseId.set(courseIdNum);
          this.loadCourseData(courseIdNum);
        }
      }

      if (lId) {
        const lessonIdNum = +lId;
        this.lessonId.set(lessonIdNum);
        if (this.lessons().length > 0) {
          this.selectLesson(lessonIdNum);
        } else {
          this.loadLesson(lessonIdNum);
        }
      }
    });
  }

  loadCourseData(courseId: number) {
    this.courseService.getCourse(courseId).subscribe({
      next: (c) => this.course.set(c),
      error: (e) => console.error('Error loading course', e),
    });

    this.courseService.getLessons(courseId).subscribe({
      next: (list) => {
        const sorted = list.sort((a, b) => a.order - b.order);
        this.lessons.set(sorted);
        this.checkLessonsCompletion(sorted);
        
        // If we have a lessonId but no currentLesson (e.g. deep link landing), select it now
        const currentLId = this.lessonId();
        if (currentLId && !this.currentLesson()) {
            this.selectLesson(currentLId);
        }
      },
      error: (e) => console.error('Error loading lessons', e)
    });
  }

  checkLessonsCompletion(lessons: Lesson[]) {
    const user = this.userService.currentUser();
    if (!user) return;

    lessons.forEach(lesson => {
      this.courseService.isLessonCompleted(lesson.id, user.id).subscribe({
        next: (isCompleted) => {
          if (isCompleted) {
            this.updateLessonStatus(lesson.id, true);
          }
        },
        error: (err) => console.error(`Error checking status for lesson ${lesson.id}`, err)
      });
    });
  }

  loadLesson(lessonId: number) {
    this.isLoading.set(true);
    this.courseService.getLesson(lessonId).subscribe({
      next: (l) => {
        this.currentLesson.set(l);
        this.isLoading.set(false);

        // Check specific completion status for this lesson
        const user = this.userService.currentUser();
        if (user) {
          this.courseService.isLessonCompleted(l.id, user.id).subscribe({
            next: (isComp) => {
              if (isComp) {
                this.updateLessonStatus(l.id, true);
              }
            },
          });
        }
      },
      error: (e) => {
        console.error('Error loading lesson', e);
        this.isLoading.set(false);
      },
    });
  }

  selectLesson(lessonId: number) {
    this.loadLesson(lessonId);
  }

  navigateToLesson(id: number) {
    if (!this.courseId()) return;
    this.router.navigate(['/courses', this.courseId(), 'learn', 'lecture', id]);
  }

  markComplete() {
    const l = this.currentLesson();
    if (!l) return;

    // If already completed, just navigate to next
    if (l.isCompleted) {
      const next = this.nextLessonId();
      if (next) {
        this.navigateToLesson(next);
      }
      return;
    }

    this.isCompleting.set(true);
    this.courseService.completeLesson(l.id).subscribe({
      next: () => {
        this.handleCompletionSuccess(l.id);
      },
      error: (e) => {
        if (e.status === 409) {
          // Already completed on server, treat as success
          this.handleCompletionSuccess(l.id);
        } else {
          console.error('Failed to complete lesson', e);
          this.isCompleting.set(false);
        }
      },
    });
  }

  private handleCompletionSuccess(lessonId: number) {
    this.isCompleting.set(false);
    this.updateLessonStatus(lessonId, true);

    const next = this.nextLessonId();
    if (next) {
      this.navigateToLesson(next);
    }
  }

  updateLessonStatus(id: number, completed: boolean) {
    this.lessons.update((list) =>
      list.map((item) => {
        if (item.id === id) return { ...item, isCompleted: completed };
        return item;
      })
    );

    if (this.currentLesson()?.id === id) {
      this.currentLesson.update((c) => (c ? { ...c, isCompleted: completed } : null));
    }
  }
}

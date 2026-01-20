import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SidebarService {
  sidebarOpen = signal(false);

  toggleSidebar() {
    this.sidebarOpen.set(!this.sidebarOpen());
  }
}

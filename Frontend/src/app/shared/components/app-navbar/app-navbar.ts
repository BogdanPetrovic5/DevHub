import { Component, signal, HostListener, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth/auth-service';

@Component({
  selector: 'app-app-navbar',
  imports: [],
  templateUrl: './app-navbar.html',
  styleUrl: './app-navbar.scss',
})
export class AppNavbar implements OnInit {
  private _router = inject(Router);
  private _authService = inject(AuthService);

  currentUser = this._authService.currentUser;
  drawerOpen = signal(false);
  profileOpen = signal(false);

  ngOnInit(): void {
    if (!this.currentUser()) {
      this._authService.getMe().subscribe();
    }
  }

  navigate(route: string) {
    this._router.navigate([route]);
  }

  toggleDrawer()  { this.drawerOpen.update(v => !v); }
  closeDrawer()   { this.drawerOpen.set(false); }
  toggleProfile() { this.profileOpen.update(v => !v); }
  closeProfile()  { this.profileOpen.set(false); }

  logout() {
    this._authService.logout().subscribe({
      next: () => { this._router.navigate(['auth']); },
      error: (err) => { console.log(err.error.message); }
    });
    this.closeProfile();
    this.closeDrawer();
  }

  @HostListener('document:keydown.escape')
  onEscape() {
    this.closeDrawer();
    this.closeProfile();
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(e: MouseEvent) {
    const target = e.target as HTMLElement;
    if (!target.closest('.navbar__avatar')) {
      this.closeProfile();
    }
  }
}

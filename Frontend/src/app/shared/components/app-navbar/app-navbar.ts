import { Component, signal, HostListener } from '@angular/core';

@Component({
  selector: 'app-app-navbar',
  imports: [],
  templateUrl: './app-navbar.html',
  styleUrl: './app-navbar.scss',
})
export class AppNavbar {
  drawerOpen = signal(false);

  toggleDrawer() { this.drawerOpen.update(v => !v); }
  closeDrawer()  { this.drawerOpen.set(false); }

  @HostListener('document:keydown.escape')
  onEscape() { this.closeDrawer(); }
}

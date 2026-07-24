import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { AppNavbar } from '../../shared/components/app-navbar/app-navbar';
import { ProfileDto } from '../../core/models/user.model';
import { UserService } from '../../core/services/user/user-service';

@Component({
  selector: 'app-profile',
  imports: [AppNavbar, DatePipe],
  templateUrl: './profile.html',
  styleUrl: './profile.scss',
})
export class Profile implements OnInit {
  private _route = inject(ActivatedRoute);
  private _router = inject(Router);
  private _userService = inject(UserService);

  profile = signal<ProfileDto | null>(null);

  ngOnInit(): void {
    const username = this._route.snapshot.paramMap.get('username')!;
    this._userService.getProfile(username).subscribe({
      next: response =>{ 
        this.profile.set(response)
        console.log(this.profile())
      },
      error: err => {
        console.log(err);
      }
    });
  }

  navigate(route: string): void {
    this._router.navigate([route]);
  }

  initials(): string {
    const p = this.profile();
    if (!p) return '';
    return (p.firstName[0] ?? '') + (p.lastName[0] ?? '');
  }
}

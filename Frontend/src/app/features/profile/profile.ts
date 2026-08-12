import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { AppNavbar } from '../../shared/components/app-navbar/app-navbar';
import { ErrorState } from '../../shared/components/error-state/error-state';
import { ProfileDto } from '../../core/models/user.model';
import { ApiError } from '../../core/models/validation.model';
import { UserService } from '../../core/services/user/user-service';

@Component({
  selector: 'app-profile',
  imports: [AppNavbar, DatePipe, ErrorState],
  templateUrl: './profile.html',
  styleUrl: './profile.scss',
})
export class Profile implements OnInit {
  private _route = inject(ActivatedRoute);
  private _router = inject(Router);
  private _userService = inject(UserService);

  profile = signal<ProfileDto | null>(null);
  error = signal<ApiError | null>(null);

  ngOnInit(): void {
    const username = this._route.snapshot.paramMap.get('username')!;
    this._userService.getProfile(username).subscribe({
      next: response => {
        this.profile.set(response);
      },
      error: err => {
        console.error(err);
        this.error.set({
          status: err.status,
          message: err.error?.error ?? 'Something went wrong.'
        });
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

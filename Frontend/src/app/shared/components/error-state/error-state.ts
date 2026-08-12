import { Component, computed, input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-error-state',
  imports: [RouterLink],
  templateUrl: './error-state.html',
  styleUrl: './error-state.scss',
})
export class ErrorState {
  status = input<number>(0);
  message = input<string>('Something went wrong. Please try again.');
  actionLabel = input<string>('Back to dashboard');
  actionLink = input<string>('/dashboard');

  title = computed(() => {
    switch (this.status()) {
      case 404: return 'Not found';
      case 403: return 'Access denied';
      case 500: return 'Server error';
      default: return 'Something went wrong';
    }
  });

  variant = computed<'forbidden' | 'server' | 'default'>(() => {
    switch (this.status()) {
      case 403: return 'forbidden';
      case 500: return 'server';
      default: return 'default';
    }
  });
}

import { HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from '../services/auth/auth-service';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, switchMap, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return next(req).pipe(
    catchError(error => {
      if (error.status === 401 && !req.url.includes('/auth/refresh') && !req.url.includes('/auth/logout')) {
        return authService.refresh().pipe(
          switchMap(() => next(req)),
          catchError(refreshError => {
            router.navigate(['/auth']);
            return throwError(() => refreshError);
          })
        );
      }
      return throwError(() => error);
    })
  );
};

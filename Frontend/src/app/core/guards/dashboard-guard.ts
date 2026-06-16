import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth/auth-service';
import { catchError, map, of } from 'rxjs';

export const dashboardGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService)
  const router = inject(Router);
  return authService.refresh().pipe(
    map(()=>true),
    catchError(()=>{
      router.navigate(['/auth'])
      return of(false);
    })
  );
};

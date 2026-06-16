import { Routes } from '@angular/router';
import { Landing } from './features/landing/landing';
import { Authentication } from './features/authentication/authentication';
import { Dashboard } from './features/dashboard/dashboard';
import { dashboardGuard } from './core/guards/dashboard-guard';
import { authGuard } from './core/guards/auth-guard';

export const routes: Routes = [
  { path: '', component: Landing },
  { path: 'auth', component: Authentication, canActivate: [authGuard] },
  { path: 'dashboard', component: Dashboard, canActivate: [dashboardGuard] },
];

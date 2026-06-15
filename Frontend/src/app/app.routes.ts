import { Routes } from '@angular/router';
import { Landing } from './features/landing/landing';
import { Authentication } from './features/authentication/authentication';
import { Dashboard } from './features/dashboard/dashboard';

export const routes: Routes = [
  { path: '', component: Landing },
  { path: 'auth', component: Authentication },
  { path: 'dashboard', component: Dashboard },
];

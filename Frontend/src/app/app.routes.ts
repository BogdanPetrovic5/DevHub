import { Routes } from '@angular/router';
import { Landing } from './features/landing/landing';
import { Authentication } from './features/authentication/authentication';

export const routes: Routes = [
  { path: '', component: Landing },
  { path: 'auth', component: Authentication },
];

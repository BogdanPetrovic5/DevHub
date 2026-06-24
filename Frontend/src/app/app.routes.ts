import { Routes } from '@angular/router';
import { Landing } from './features/landing/landing';
import { Authentication } from './features/authentication/authentication';
import { Dashboard } from './features/dashboard/dashboard';
import { New } from './features/repository/new/new';
import { Details } from './features/repository/details/details';
import { dashboardGuard } from './core/guards/dashboard-guard';
import { authGuard } from './core/guards/auth-guard';
import { Blob } from './features/repository/blob/blob';

export const routes: Routes = [
  { path: '', component: Landing },
  { path: 'auth', component: Authentication, canActivate: [authGuard] },
  { path: 'dashboard', component: Dashboard, canActivate: [dashboardGuard] },
  { path: 'repository/new', component: New, canActivate: [dashboardGuard] },
  { path: 'repository/:username/:repoName', component: Details, canActivate: [dashboardGuard] },
  {path:'repository/:username/:repoName/blob', component:Blob, canActivate:[dashboardGuard]}
];

import { Component, signal } from '@angular/core';
import { Login } from './components/login/login';
import { Register } from './components/register/register';

@Component({
  selector: 'app-authentication',
  imports: [Login, Register],
  templateUrl: './authentication.html',
  styleUrl: './authentication.scss',
})
export class Authentication {
  activeTab = signal<'login' | 'register'>('login');
}

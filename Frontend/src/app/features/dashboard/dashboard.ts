import { Component } from '@angular/core';
import { AppNavbar } from '../../shared/components/app-navbar/app-navbar';

@Component({
  selector: 'app-dashboard',
  imports: [AppNavbar],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard {}

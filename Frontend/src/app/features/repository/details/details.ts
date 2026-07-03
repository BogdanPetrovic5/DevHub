import { Component, inject } from '@angular/core';
import { AppNavbar } from '../../../shared/components/app-navbar/app-navbar';
import { ActivatedRoute, Router, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-details',
  imports: [AppNavbar, RouterOutlet],
  templateUrl: './details.html',
  styleUrl: './details.scss',
})
export class Details {
  private _route = inject(ActivatedRoute);
  private _router = inject(Router);

  username = this._route.snapshot.paramMap.get('username')!;
  repoName = this._route.snapshot.paramMap.get('repoName')!;

  get isCommitsTab(): boolean {
    return this._router.url.includes('/commits');
  }

  navigateToCode(): void {
    this._router.navigate(['repository', this.username, this.repoName]);
  }

  navigateToCommits(): void {
    this._router.navigate(['repository', this.username, this.repoName, 'commits']);
  }
  navigate(url:string){
    this._router.navigate([url]);
  }
}

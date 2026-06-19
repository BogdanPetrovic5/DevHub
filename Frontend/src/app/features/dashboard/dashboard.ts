import { Component, inject, OnInit, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AppNavbar } from '../../shared/components/app-navbar/app-navbar';
import { RepoService } from '../../core/services/repository/repo-service';
import { RepoDto } from '../../core/models/repo.model';

@Component({
  selector: 'app-dashboard',
  imports: [AppNavbar, DatePipe, RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard implements OnInit{
  private _repoService = inject(RepoService);
  public repos = signal<RepoDto[]>([]);
  private _router = inject(Router);
  ngOnInit(): void {
    this._repoService.getUserRepos().subscribe({
      next:response=>{
        this.repos.set(response);
        console.log(this.repos)
      }
    })
  }

  navigate(route:string){
    this._router.navigate([route]);
  }
}

import { Component, inject, OnInit, signal } from '@angular/core';
import { AppNavbar } from '../../../shared/components/app-navbar/app-navbar';
import { RepoDetailDto } from '../../../core/models/repo.model';
import { RepoService } from '../../../core/services/repository/repo-service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-details',
  imports: [AppNavbar],
  templateUrl: './details.html',
  styleUrl: './details.scss',
})
export class Details implements OnInit{
  public repo = signal<RepoDetailDto | null>(null); //for now
  private _repoService = inject(RepoService )
  private _route = inject(ActivatedRoute);
    ngOnInit(): void {
      const username = this._route.snapshot.paramMap.get('username')!;
      const repoName = this._route.snapshot.paramMap.get('repoName')!;
      const path = this._route.snapshot.queryParamMap.get('path') ?? '';

      this._repoService.getRepo(username, repoName, path).subscribe({
        next: response => {
          console.log(response)
          this.repo.set(response)
        }
      });
  }
}

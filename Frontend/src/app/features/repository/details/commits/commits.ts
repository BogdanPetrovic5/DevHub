import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RepoCommitSummaryDto } from '../../../../core/models/repo.model';
import { RepoService } from '../../../../core/services/repository/repo-service';
import { TimeAgoPipe } from '../../../../shared/pipes/time-ago-pipe';

@Component({
  selector: 'app-commits',
  imports: [TimeAgoPipe],
  templateUrl: './commits.html',
  styleUrl: './commits.scss',
})
export class Commits implements OnInit {
  private _route = inject(ActivatedRoute);
  private _repoService = inject(RepoService);
  private _router = inject(Router);
  commits = signal<RepoCommitSummaryDto[] | null>(null);

  username = this._route.parent!.snapshot.paramMap.get('username')!;
  repoName = this._route.parent!.snapshot.paramMap.get('repoName')!;

  ngOnInit(): void {
    this._repoService.getCommits(this.username, this.repoName).subscribe({
      next: response => {
        this.commits.set(response)
        console.log(this.commits())
      }
    });
  }
  navigate(url: string): void { 
    this._router.navigate([url]);
  }
}

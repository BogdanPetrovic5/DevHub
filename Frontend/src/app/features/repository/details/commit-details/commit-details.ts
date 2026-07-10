import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommitFilesDto } from '../../../../core/models/repo.model';
import { RepoService } from '../../../../core/services/repository/repo-service';
import { TimeAgoPipe } from '../../../../shared/pipes/time-ago-pipe';

@Component({
  selector: 'app-commit-details',
  imports: [TimeAgoPipe],
  templateUrl: './commit-details.html',
  styleUrl: './commit-details.scss',
})
export class CommitDetails implements OnInit {
  private _route = inject(ActivatedRoute);
  private _router = inject(Router);
  private _repoService = inject(RepoService);

  private _username = this._route.parent!.snapshot.paramMap.get('username')!;
  private _repoName = this._route.parent!.snapshot.paramMap.get('repoName')!;
  private _commitId = this._route.snapshot.paramMap.get('commitId')!;

  commit = signal<CommitFilesDto | null>(null);

  ngOnInit(): void {
    this._repoService.getCommitDetails(this._username, this._repoName, this._commitId).subscribe({
      next: response => {
        console.log(response)
        this.commit.set(response)
        console.log(this.commit())
      }
    });
  }

  goBack(): void {
    this._router.navigate(['repository', this._username, this._repoName, 'commits']);
  }

  getChangeClass(changeType: string): string {
    return changeType.toLowerCase();
  }

  shortHash(): string {
    return this._commitId.slice(0, 7);
  }
}

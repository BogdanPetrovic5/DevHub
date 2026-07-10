import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { RepoDetailDto, RepoLanguageDto } from '../../../../core/models/repo.model';
import { RepoService } from '../../../../core/services/repository/repo-service';
import { TimeAgoPipe } from '../../../../shared/pipes/time-ago-pipe';

@Component({
  selector: 'app-code',
  imports: [FormsModule, DatePipe, TimeAgoPipe],
  templateUrl: './code.html',
  styleUrl: './code.scss',
})
export class Code implements OnInit {
  private _route = inject(ActivatedRoute);
  private _router = inject(Router);
  private _repoService = inject(RepoService);
  private _destroyRef = inject(DestroyRef);

  repo = signal<RepoDetailDto | null>(null);
  showCommitModal = signal(false);
  commitMessage = signal('Upload via web');
  isUploading = signal(false);
  repoLanguagePercentageMap = signal<RepoLanguageDto[]>([]);

  private _pendingFile: File | null = null;
  private _username = this._route.parent!.snapshot.paramMap.get('username')!;
  private _repoName = this._route.parent!.snapshot.paramMap.get('repoName')!;



  ngOnInit(): void {
    this._route.queryParamMap.pipe(
      takeUntilDestroyed(this._destroyRef)
    ).subscribe(params => {
      const path = params.get('path') ?? '';
      this._repoService.getDetails(this._username, this._repoName, path).subscribe({
        next: response => {
          this.repo.set(response);
          this.repoLanguagePercentageMap.set(response.languages);
          console.log(this.repo());
        }
      });
    });
  }

  onFileSelected(event: Event): void {
    const file = (event.target as HTMLInputElement)?.files?.[0];
    if (!file) return;
    this._pendingFile = file;
    this.showCommitModal.set(true);
  }

  confirmUpload(): void {
    if (!this._pendingFile || !this.repo()) return;
    const formData = new FormData();
    formData.append('File', this._pendingFile);
    formData.append('Message', this.commitMessage());
    this.isUploading.set(true);
    this._repoService.upload(formData, this.repo()!.id).subscribe({
      next: () => {
        this.showCommitModal.set(false);
        this.isUploading.set(false);
        this._pendingFile = null;
      },
      error: () => this.isUploading.set(false)
    });
  }

  cancelUpload(): void {
    this._pendingFile = null;
    this.showCommitModal.set(false);
  }

  viewFile(path: string): void {
    this._router.navigate(
      ['repository', this._username, this._repoName, 'blob'],
      { queryParams: { path } }
    );
  }

  loadMore(path: string): void {
    this._router.navigate([], {
      relativeTo: this._route,
      queryParams: { path },
    });
  }

  getLanguageColor(language: string): string {
    return this._languageColors[language] ?? '#8b8b8b';
  }

    private readonly _languageColors: Record<string, string> = {
    'TypeScript': '#3178c6',
    'C#': '#178600',
    'SCSS': '#c6538c',
    'CSS': '#663399',
    'HTML': '#e34c26',
    'JavaScript': '#f1e05a',
    'Markdown': '#083fa1',
    'JSON': '#002b36'
  };
}

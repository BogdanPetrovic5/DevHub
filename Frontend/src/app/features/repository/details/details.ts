import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { AppNavbar } from '../../../shared/components/app-navbar/app-navbar';
import { RepoDetailDto } from '../../../core/models/repo.model';
import { RepoService } from '../../../core/services/repository/repo-service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-details',
  imports: [AppNavbar, FormsModule],
  templateUrl: './details.html',
  styleUrl: './details.scss',
})
export class Details implements OnInit {
  public repo = signal<RepoDetailDto | null>(null);
  private _repoService = inject(RepoService);
  private _route = inject(ActivatedRoute);
  private _router = inject(Router);
  private _destroyRef = inject(DestroyRef)
  showCommitModal = signal(false);
  commitMessage = signal('Upload via web');
  isUploading = signal(false);

  private _pendingFile: File | null = null;

  onFileSelected(event: Event) {
    const file = (event.target as HTMLInputElement)?.files?.[0];
    if (!file) return;
    this._pendingFile = file;
    this.showCommitModal.set(true);
  }

  confirmUpload() {
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

  cancelUpload() {
    this._pendingFile = null;
    this.showCommitModal.set(false);
  }

  ngOnInit(): void {
    const username = this._route.snapshot.paramMap.get('username')!;
    const repoName = this._route.snapshot.paramMap.get('repoName')!;

    this._route.queryParamMap.pipe(
      takeUntilDestroyed(this._destroyRef)
    ).subscribe(params => {
      const path = params.get('path') ?? '';
      this._repoService.getDetails(username, repoName, path).subscribe({
        next: response => this.repo.set(response)
      });
    });
  }

  loadMore(path: string): void {
    this._router.navigate([], {
      relativeTo: this._route,
      queryParams: { path },
    });
  }
}

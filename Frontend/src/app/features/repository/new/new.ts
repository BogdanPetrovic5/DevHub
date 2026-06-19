import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AppNavbar } from '../../../shared/components/app-navbar/app-navbar';
import { RepoService } from '../../../core/services/repository/repo-service';
import { CreateRepoRequest } from '../../../core/models/repo.model';

@Component({
  selector: 'app-new',
  imports: [ReactiveFormsModule, AppNavbar, RouterLink],
  templateUrl: './new.html',
  styleUrl: './new.scss',
})
export class New {
  private _fb = inject(FormBuilder);
  private _repoService = inject(RepoService);
  private _router = inject(Router);

  isLoading = signal(false);
  errorMessage = signal<string | null>(null);
  success = signal(false);
  createdRepoName = signal('');

  form = this._fb.group({
    name: ['', Validators.required],
    description: [''],
    isPrivate: [false]
  });

  get isPrivate() {
    return this.form.get('isPrivate')?.value;
  }

  submit() {
    if (this.form.invalid) return;

    this.isLoading.set(true);
    this.errorMessage.set(null);
    const formData = this.form.value as CreateRepoRequest;

    this._repoService.submit(formData).subscribe({
      next: () => {
        this.createdRepoName.set(this.form.value.name ?? '');
        this.isLoading.set(false);
        this.success.set(true);
        
        setTimeout(() => this._router.navigate(['/dashboard']), 2200);
      },
      error: err => {
        this.isLoading.set(false);
        this.errorMessage.set(err?.error?.message ?? 'Something went wrong');
      }
    });
  }
}

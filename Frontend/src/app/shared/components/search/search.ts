import { Component, DestroyRef, HostListener, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { SearchService } from '../../../core/services/search/search-service';
import { SearchResultDto } from '../../../core/models/search.model';
import { catchError, debounceTime, distinctUntilChanged, of, switchMap } from 'rxjs';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-search',
  imports: [],
  templateUrl: './search.html',
  styleUrl: './search.scss',
})
export class Search {
  private _searchService = inject(SearchService);
  private _destroyRef = inject(DestroyRef);
  private _router = inject(Router);

  searchQuery = signal<string>('');
  searchResults = signal<SearchResultDto | null>(null);
  isOpen = signal(false);

  constructor() {
    toObservable(this.searchQuery).pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(query => query.trim()
        ? this._searchService.search(query).pipe(catchError(() => of(null)))
        : of(null)
      ),
      takeUntilDestroyed(this._destroyRef)
    ).subscribe(results => {
      this.searchResults.set(results);
      console.log('Search results:', results);
    });
   
  }

  onInput(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.searchQuery.set(value);
    this.isOpen.set(value.trim().length > 0);
  }

  onFocus(): void {
    if (this.searchQuery().trim().length > 0) {
      this.isOpen.set(true);
    }
  }

  goToRepo(ownerUsername: string, repoName: string): void {
    this.isOpen.set(false);
    this._router.navigate(['repository', ownerUsername, repoName]);
  }

  goToUser(username: string): void {
    this.isOpen.set(false);
    this._router.navigate(['profile', username]);
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const target = event.target as HTMLElement;
    if (!target.closest('app-search')) {
      this.isOpen.set(false);
    }
  }

  @HostListener('document:keydown.escape')
  onEscape(): void {
    this.isOpen.set(false);
  }
}

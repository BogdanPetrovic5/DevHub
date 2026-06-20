import { Component, signal } from '@angular/core';
import { AppNavbar } from '../../../shared/components/app-navbar/app-navbar';
import { RepoDetailDto } from '../../../core/models/repo.model';

@Component({
  selector: 'app-details',
  imports: [AppNavbar],
  templateUrl: './details.html',
  styleUrl: './details.scss',
})
export class Details {
  public repo = signal<RepoDetailDto | null>(null); //for now

}

import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RepoFileContentDto } from '../../../core/models/repo.model';
import { RepoService } from '../../../core/services/repository/repo-service';
import { AppNavbar } from "../../../shared/components/app-navbar/app-navbar";

@Component({
  selector: 'app-blob',
  imports: [AppNavbar],
  templateUrl: './blob.html',
  styleUrl: './blob.scss',
})
export class Blob implements OnInit{

  private _route = inject(ActivatedRoute);
  private _router = inject(Router);
  public  file = signal<RepoFileContentDto | null>(null)
  public lineCount = computed(()=>this.file()?.content.split('\n').length);
  public lineNumbers = computed(()=> Array.from({length: this.lineCount() ?? 0}, (_, i) => i+1));
  
  private _repoService = inject(RepoService);

  public username = signal<string>("")
  public repoName = signal<string>("")


  public pathSegments = signal<string[]>([]);

  ngOnInit(): void {
    this.username.set( this._route.snapshot.paramMap.get('username')!);    
    this.repoName.set(this._route.snapshot.paramMap.get('repoName')!);    
    const path = this._route.snapshot.queryParamMap.get('path') ?? ''
    
    this._repoService.viewFile(this.username(),this.repoName(),path).subscribe({
      next:response=>{
        this.file.set(response);
        this.createPathSegments(response.path)
        console.log(this.lineNumbers())
      }
    })
  }
  createPathSegments(path:string){
    this.pathSegments.set(path.split('/'))
    console.log(this.pathSegments())
  }

  goBack(): void {
    this._router.navigate(['repository', this.username(), this.repoName()]);
  }
  navigateToSegment(index: number) {
    const path = this.pathSegments().slice(0, index+1).join("/");
    this._router.navigate(['repository', this.username(), this.repoName()],{
      queryParams:{path}
    })
  }
}

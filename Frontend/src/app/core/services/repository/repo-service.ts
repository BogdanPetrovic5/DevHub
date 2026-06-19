import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateRepoRequest, RepoResponse } from '../../models/repo.model';
import { environment } from '../../../../environments/environment';

@Service()
export class RepoService {
    private _httpClient = inject(HttpClient);

    submit(data:CreateRepoRequest):Observable<RepoResponse>{
        const url = `${environment.apiUrl}/api/repo/new`
        return this._httpClient.post<RepoResponse>(url, data, {withCredentials:true})
    }
}

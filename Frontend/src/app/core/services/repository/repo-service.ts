import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { Observable } from 'rxjs';
import { CommitFilesDto, CreateRepoRequest, RepoCommitSummaryDto, RepoDetailDto, RepoDto, RepoFileContentDto, RepoResponse } from '../../models/repo.model';
import { environment } from '../../../../environments/environment';

@Service()
export class RepoService {
    private _httpClient = inject(HttpClient);

    submit(data: CreateRepoRequest): Observable<RepoResponse> {
        const url = `${environment.apiUrl}/api/repo/new`
        return this._httpClient.post<RepoResponse>(url, data, { withCredentials: true })
    }

    getUserRepos(): Observable<RepoDto[]> {
        const url = `${environment.apiUrl}/api/repo/user`
        return this._httpClient.get<RepoDto[]>(url, { withCredentials: true })
    }

    getDetails(username: string, repoName: string, path: string = ''):Observable<RepoDetailDto>{
        const url = `${environment.apiUrl}/api/repo/${username}/${repoName}`;

        return this._httpClient.get<RepoDetailDto>(url, { params: { path }, withCredentials: true, headers: { 'Cache-Control': 'no-cache' } });
    }

    upload(data:FormData, repoId:string):Observable<RepoResponse>{
        const url = `${environment.apiUrl}/api/repo/${repoId}/upload`

        return this._httpClient.post<RepoResponse>(url, data, { withCredentials: true });
    }

    viewFile(username:string, repoName:string, path:string):Observable<RepoFileContentDto>{
        const url = `${environment.apiUrl}/api/repo/${username}/${repoName}/blob?path=${path}`
        return this._httpClient.get<RepoFileContentDto>(url, {withCredentials:true});
    }

    getCommits(username:string, repoName:string):Observable<RepoCommitSummaryDto[]>{
        const url = `${environment.apiUrl}/api/repo/${username}/${repoName}/commits`

        return this._httpClient.get<RepoCommitSummaryDto[]>(url, {withCredentials:true});
    }

    getCommitDetails(username:string, repoName:string, commitId:string):Observable<CommitFilesDto>{
        const url = `${environment.apiUrl}/api/repo/${username}/${repoName}/commits/${commitId}`

        return this._httpClient.get<CommitFilesDto>(url, {withCredentials:true});
    }
}

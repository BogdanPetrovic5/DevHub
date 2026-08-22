import { inject, Service } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { SearchResultDto } from '../../models/search.model';

@Service()
export class SearchService {
    url: string = ""
    private _httpClient = inject(HttpClient);
    search(query: string):Observable<SearchResultDto> {
        this.url = `${environment.apiUrl}/api/search?q=${encodeURIComponent(query)}`;

        return this._httpClient.get<SearchResultDto>(this.url);

    }
}

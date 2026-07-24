import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from '../../../../environments/environment.development';
import { ProfileDto } from '../../models/user.model';

@Service()
export class UserService {

    private _httpClient = inject(HttpClient);

    getProfile(username: string): Observable<ProfileDto> {
        const url = `${environment.apiUrl}/api/user/${username}`;
        return this._httpClient.get<ProfileDto>(url, { withCredentials: true });
    }
}

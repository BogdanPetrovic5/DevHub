import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { AuthResponse, LoginRequest, RegisterRequest } from '../../models/auth.model';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';

@Service()
export class AuthService {
    private _httpClient = inject(HttpClient);

    register(registerData:RegisterRequest):Observable<AuthResponse>{
        const url = `${environment.apiUrl}/api/auth/register`
        
        return this._httpClient.post<AuthResponse>(url, registerData, { withCredentials: true })
    }

    login(logindData:LoginRequest):Observable<AuthResponse>{
        const url = `${environment.apiUrl}/api/auth/login`

        return this._httpClient.post<AuthResponse>(url, logindData, {withCredentials:true})

    }

    logout(): Observable<AuthResponse> {
        const url = `${environment.apiUrl}/api/auth/logout`
        return this._httpClient.delete<AuthResponse>(url, {})
    }

    refresh():Observable<AuthResponse>{
        const url = `${environment.apiUrl}/api/auth/refresh`;
        return this._httpClient.post<AuthResponse>(url, {}, {withCredentials:true});
    }
}

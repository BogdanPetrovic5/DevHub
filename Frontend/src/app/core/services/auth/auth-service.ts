import { HttpClient } from '@angular/common/http';
import { inject, Service, signal } from '@angular/core';
import { AuthResponse, LoginRequest, MeDto, RegisterRequest } from '../../models/auth.model';
import { environment } from '../../../../environments/environment';
import { Observable, tap } from 'rxjs';

const USER_KEY = 'devhub_user';

@Service()
export class AuthService {
    private _httpClient = inject(HttpClient);
    currentUser = signal<MeDto | null>(this._loadUser());


    getMe(): Observable<MeDto> {
        const url = `${environment.apiUrl}/api/auth/me`;
        return this._httpClient.get<MeDto>(url, { withCredentials: true }).pipe(
            tap(user => {
                this.currentUser.set(user);
                this._saveUser(user);
            })
        );
    }

    register(registerData: RegisterRequest): Observable<AuthResponse> {
        const url = `${environment.apiUrl}/api/auth/register`;
        return this._httpClient.post<AuthResponse>(url, registerData, { withCredentials: true });
    }

    login(loginData: LoginRequest): Observable<AuthResponse> {
        const url = `${environment.apiUrl}/api/auth/login`;
        return this._httpClient.post<AuthResponse>(url, loginData, { withCredentials: true });
    }

    logout(): Observable<AuthResponse> {
        const url = `${environment.apiUrl}/api/auth/logout`;
        return this._httpClient.delete<AuthResponse>(url, { withCredentials: true }).pipe(
            tap(() => {
                this.currentUser.set(null);
                this._clearUser();
            })
        );
    }

    refresh(): Observable<AuthResponse> {
        const url = `${environment.apiUrl}/api/auth/refresh`;
        return this._httpClient.post<AuthResponse>(url, {}, { withCredentials: true });
    }

        private _loadUser(): MeDto | null {
        const raw = localStorage.getItem(USER_KEY);
        return raw ? JSON.parse(raw) : null;
    }

    private _saveUser(user: MeDto): void {
        localStorage.setItem(USER_KEY, JSON.stringify(user));
    }

    private _clearUser(): void {
        localStorage.removeItem(USER_KEY);
    }

}

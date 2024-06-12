import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { CookieService } from 'ngx-cookie-service';

interface LoginResponse {
  token: string;
}

interface ChangePasswordRequest {
  password: string;
  newPassword: string;
}

interface ChangeUsernameRequest {
  newUsername: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private BASE_URL = 'http://localhost:7062/api';
  private tokenSubject: BehaviorSubject<string> = new BehaviorSubject<string>('');

  constructor(private http: HttpClient, private cookieService: CookieService) {
    const token = this.cookieService.get('cozy');
    if (token) {
      this.tokenSubject.next(token);
    }
  }

  login(username: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.BASE_URL}/login`, { username, password })
      .pipe(tap(response => {
        this.cookieService.set('cozy', response.token);
        this.tokenSubject.next(response.token);
      }));
  }

  logout(): void {
    this.cookieService.delete('cozy');
    this.tokenSubject.next('');
  }

  getToken(): string {
    return this.tokenSubject.value;
  }

  changePassword(request: ChangePasswordRequest): Observable<void> {
    return this.http.put<void>(`${this.BASE_URL}/change/password`, request);
  }

  changeUsername(request: ChangeUsernameRequest): Observable<void> {
    return this.http.put<void>(`${this.BASE_URL}/change/username`, request);
  }
}
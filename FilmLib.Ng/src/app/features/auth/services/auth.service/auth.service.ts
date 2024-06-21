import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject, interval, of } from 'rxjs';
import { map, tap, startWith, switchMap } from 'rxjs/operators';
import { environment } from '../../../../../environments/environment.prod';
import { CookieService } from 'ngx-cookie-service';
import { LocalService } from '../local.service/local.service';
import { jwtDecode } from 'jwt-decode';

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private BASE_URL = environment.theMovieDBApi;
  private token = new BehaviorSubject<string | null>(
    this.LocalService.get(environment.cookie)
  );
  private authStatus = new BehaviorSubject<boolean>(
    this.token.value ? true : false
  );

  private tokenExpirationChecker$ = interval(5000).pipe(
    startWith(0),
    switchMap(() => this.isTokenExpired())
  );
  httpOptions = {
    withCredentials: true,
    headers: new HttpHeaders({ ContentType: 'application/json' }),
  };

  constructor(
    private http: HttpClient,
    private cookieService: CookieService,
    private LocalService: LocalService
  ) {}

  register(request: RegisterRequest): Observable<void> {
    return this.http
      .post<void>(`${this.BASE_URL}/register`, request, this.httpOptions)
      .pipe(
        tap(() => {
          const token = this.cookieService.get(environment.cookie);
          this.LocalService.set(environment.cookie, token);
          this.token.next(token);
          this.authStatus.next(true);
        })
      );
  }

  login(email: string, password: string): Observable<void> {
    return this.http
      .post<void>(
        `${this.BASE_URL}/login`,
        { email, password },
        this.httpOptions
      )
      .pipe(
        tap(() => {
          const token = this.cookieService.get(environment.cookie);
          this.LocalService.set(environment.cookie, token);
          this.token.next(token);
          this.authStatus.next(true);
        })
      );
  }

  logout(): void {
    this.cookieService.delete(environment.cookie);
    this.LocalService.delete(environment.cookie);
    this.token.next(null);
    this.authStatus.next(false);
  }

  isAuthenticated(): Observable<boolean> {
    return this.authStatus.asObservable();
  }

  getUserDetails(): Observable<any> {
    return this.isAuthenticated().pipe(
      map((isAuthenticated) => {
        if (!isAuthenticated) return 'guest';
        const token = this.LocalService.get(environment.cookie);
        const decodedToken: any = jwtDecode(token!);
        const userDetails = {
          username: decodedToken.unique_name,
          email: decodedToken.email,
          role: decodedToken.role,
        };
        return userDetails;
      })
    );
  }

  private isTokenExpired(): Observable<boolean> {
    return of(true).pipe(
      map(() => {
        const token = this.LocalService.get(environment.cookie);
        if (!token) return true;
        const decodedToken: any = jwtDecode(token!);
        const isExpired = decodedToken.exp < Date.now() / 1000;
        if (isExpired) this.logout();
        return isExpired;
      })
    );
  }

  getToken(): Observable<string | null> {
    return this.token.asObservable();
  }

  changePassword(request: FormData): Observable<void> {
    return this.http.put<void>(
      `${this.BASE_URL}/change/password`,
      request,
      this.httpOptions
    );
  }

  changeUsername(request: FormData): Observable<void> {
    return this.http.put<void>(
      `${this.BASE_URL}/change/username`,
      request,
      this.httpOptions
    );
  }
}

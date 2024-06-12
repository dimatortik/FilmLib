// genre.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../auth.service/auth.service';
import { GenreModel} from '../../models/genre.model';

@Injectable({
  providedIn: 'root'
})
export class GenreService {
  private BASE_URL = 'http://localhost:7062/api';

  constructor(private http: HttpClient, private authService: AuthService) { }

  getHttpOptions() {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${this.authService.getToken()}`
      })
    };
    return httpOptions;
  }

  addGenre(genre: GenreModel): Observable<void> {
    return this.http.post<void>(`${this.BASE_URL}/genre`, genre, this.getHttpOptions());
  }

  deleteGenre(id: number): Observable<void> {
    return this.http.delete<void>(`${this.BASE_URL}/genre/${id}`, this.getHttpOptions());
  }

  updateGenre(id: number, genre: GenreModel): Observable<void> {
    return this.http.put<void>(`${this.BASE_URL}/genre/${id}`, genre, this.getHttpOptions());
  }

  getGenres(): Observable<GenreModel[]> {
    return this.http.get<GenreModel[]>(`${this.BASE_URL}/genres`, this.getHttpOptions());
  }

  getGenre(id: number): Observable<GenreModel> {
    return this.http.get<GenreModel>(`${this.BASE_URL}/genre/${id}`, this.getHttpOptions());
  }

  getGenresByFilm(id: string): Observable<GenreModel[]> {
    return this.http.get<GenreModel[]>(`${this.BASE_URL}/film/${id}/genres`, this.getHttpOptions());
  }
}
// genre.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../../../auth/services/auth.service/auth.service';
import { GenreModel } from '../../models/genre.model';
import { environment } from '../../../../../environments/environment.prod';

@Injectable({
  providedIn: 'root',
})
export class GenreService {
  private BASE_URL = environment.theMovieDBApi;

  constructor(private http: HttpClient, private authService: AuthService) {}

  addGenre(genre: FormData): Observable<void> {
    return this.http.post<void>(`${this.BASE_URL}/genre`, genre);
  }

  deleteGenre(id: number): Observable<void> {
    return this.http.delete<void>(`${this.BASE_URL}/genre/${id}`);
  }

  updateGenre(id: number, genre: FormData): Observable<void> {
    return this.http.put<void>(`${this.BASE_URL}/genre/${id}`, genre);
  }

  getGenres(): Observable<GenreModel[]> {
    return this.http.get<GenreModel[]>(`${this.BASE_URL}/genres`);
  }

  getGenre(id: number): Observable<GenreModel> {
    return this.http.get<GenreModel>(`${this.BASE_URL}/genre/${id}`);
  }

  getGenresByFilm(id: string): Observable<GenreModel[]> {
    return this.http.get<GenreModel[]>(`${this.BASE_URL}/film/${id}/genres`);
  }
}

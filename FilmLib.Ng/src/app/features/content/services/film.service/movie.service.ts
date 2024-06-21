// film.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../../../auth/services/auth.service/auth.service';
import { MovieModel } from '../../models/movie.model';
import { PaginationModel } from '../../../../core/models/pagination.model';
import { environment } from '../../../../../environments/environment.prod';
import { IMovie } from '../../interfaces/movie.interface';

@Injectable({
  providedIn: 'root',
})
export class FilmService {
  private BASE_URL = environment.theMovieDBApi;

  constructor(private http: HttpClient, private authService: AuthService) {}

  addFilm(film: FormData): Observable<any> {
    // const formData = new FormData();
    // formData.append('titleImage', film.titleImageLink);
    // formData.append('title', film.title);
    // formData.append('description', film.description);
    // formData.append('year', film.year.toString());
    // formData.append('country', film.country);
    // formData.append('filmVideo', film.filmVideoLink);
    // formData.append('actors', JSON.stringify(film.actors));
    // formData.append('genres', JSON.stringify(film.genres));

    return this.http.post<void>(`${this.BASE_URL}/film`, film);
  }

  deleteFilm(id: string): Observable<void> {
    return this.http.delete<void>(`${this.BASE_URL}/film/${id}`);
  }

  updateFilm(id: string, film: FormData): Observable<void> {
    // const formData = new FormData();
    // formData.append(
    //   'titleImage',
    //   film.titleImageLink ? film.titleImageLink : ''
    // );
    // formData.append('title', film.title ? film.title : '');
    // formData.append('description', film.description ? film.description : '');
    // formData.append('year', film.year?.toString() ? film.year.toString() : '');
    // formData.append('country', film.country ? film.country : '');
    // formData.append('filmVideo', film.filmVideoLink ? film.filmVideoLink : '');
    // formData.append(
    //   'actors',
    //   JSON.stringify(film.actors) ? JSON.stringify(film.actors) : ''
    // );
    // formData.append(
    //   'genres',
    //   JSON.stringify(film.genres) ? JSON.stringify(film.genres) : ''
    // );

    return this.http.put<void>(`${this.BASE_URL}/film/${id}`, film);
  }

  getFilms(
    page: number,
    pageSize: number,
    searchTerm?: string,
    sortColumn?: string,
    sortOrder?: string
  ): Observable<PaginationModel<MovieModel>> {
    return this.http.get<PaginationModel<MovieModel>>(
      `${this.BASE_URL}/films?searchTerm=${searchTerm}&sortColumn=${sortColumn}&sortOrder=${sortOrder}&page=${page}&pageSize=${pageSize}`
    );
  }

  getFilm(id: string): Observable<MovieModel> {
    return this.http.get<MovieModel>(`${this.BASE_URL}/film/${id}`);
  }

  voteFilm(id: string, voteValue: number): Observable<void> {
    return this.http.post<void>(`${this.BASE_URL}/film/${id}/vote`, voteValue);
  }

  getFilmsByActorId(id: string): Observable<MovieModel[]> {
    return this.http.get<MovieModel[]>(`${this.BASE_URL}/actor/${id}/films`);
  }

  geFilmsByGenreId(id: number): Observable<MovieModel[]> {
    return this.http.get<MovieModel[]>(`${this.BASE_URL}/genre/${id}/films`);
  }
}

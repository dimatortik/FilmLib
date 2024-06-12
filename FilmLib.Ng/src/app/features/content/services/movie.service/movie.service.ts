// film.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../auth.service/auth.service';
import { MovieModel } from '../../models/movie.model';
import { PaginationModel } from '../../../../core/models/pagination.model';

@Injectable({
  providedIn: 'root'
})
export class FilmService {
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

  addFilm(film: MovieModel): Observable<void> {
    const formData = new FormData();
    formData.append('titleImage', film.titleImageLink);
    formData.append('title', film.title);
    formData.append('description', film.description);
    formData.append('year', film.year.toString());
    formData.append('country', film.country);
    formData.append('filmVideo', film.videoLink);
    formData.append('actors', JSON.stringify(film.actors));
    formData.append('genres', JSON.stringify(film.genres));
    
    return this.http.post<void>(`${this.BASE_URL}/film`, formData, this.getHttpOptions());
  }

  deleteFilm(id: string): Observable<void> {
    return this.http.delete<void>(`${this.BASE_URL}/film/${id}`, this.getHttpOptions());
  }

  updateFilm(id: string, film: MovieModel): Observable<void> {
      const formData = new FormData();
      formData.append('titleImage', film.titleImageLink? film.titleImageLink : '');
      formData.append('title', film.title? film.title : '');
      formData.append('description', film.description? film.description : '');
      formData.append('year', film.year.toString()? film.year.toString() : '');
      formData.append('country', film.country? film.country : '');
      formData.append('filmVideo', film.videoLink? film.videoLink : '');
      formData.append('actors', JSON.stringify(film.actors)? JSON.stringify(film.actors) : '');
      formData.append('genres', JSON.stringify(film.genres)? JSON.stringify(film.genres) : '');
      
    return this.http.put<void>(`${this.BASE_URL}/film/${id}`, formData, this.getHttpOptions());
  }

  getFilms(
    pageNumber: number,
    pageSize: number,
    serchTerm?: string, 
    sortColimn?: string, 
    sortOrder?: string): Observable<PaginationModel<MovieModel>> {
      
    return this.http.get<PaginationModel<MovieModel>>(`${this.BASE_URL}/films?searchTerm=${serchTerm}&sortColumn=${sortColimn}&sortOrder=${sortOrder}&pageNumber=${pageNumber}&pageSize=${pageSize}`, this.getHttpOptions());
  }

  getFilm(id: string): Observable<MovieModel> {
    return this.http.get<MovieModel>(`${this.BASE_URL}/film/${id}`, this.getHttpOptions());
  }

  voteFilm(id: string, voteValue: number): Observable<void> {
    return this.http.post<void>(`${this.BASE_URL}/film/${id}/vote`, voteValue, this.getHttpOptions());
  }

  getFilmsByActorId(id: string): Observable<MovieModel[]> {
    return this.http.get<MovieModel[]>(`${this.BASE_URL}/actor/${id}/films`, this.getHttpOptions());
  }
}
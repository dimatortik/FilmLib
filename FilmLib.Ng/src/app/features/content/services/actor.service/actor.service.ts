import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../../../auth/services/auth.service/auth.service';
import { ActorModel } from '../../models/actor.model';
import { environment } from '../../../../../environments/environment.prod';
import { map, switchMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class ActorService {
  private BASE_URL = environment.theMovieDBApi;

  constructor(private http: HttpClient, private authService: AuthService) {}

  addActor(actor: FormData): Observable<void> {
    return this.http.post<void>(`${this.BASE_URL}/actor`, actor);
  }

  deleteActor(id: string): Observable<void> {
    return this.http.delete<void>(`${this.BASE_URL}/actor/${id}`);
  }

  updateActor(id: string, actor: FormData): Observable<any> {
    return this.http.put<void>(`${this.BASE_URL}/actor/${id}`, actor);
  }

  getActorsByFilmId(id: string): Observable<ActorModel[]> {
    return this.http.get<ActorModel[]>(`${this.BASE_URL}/film/${id}/actors`);
  }

  getActors(): Observable<ActorModel[]> {
    return this.http.get<ActorModel[]>(`${this.BASE_URL}/actors`);
  }

  getActorById(id: string): Observable<ActorModel> {
    return this.http.get<ActorModel>(`${this.BASE_URL}/actor/${id}`);
  }
}

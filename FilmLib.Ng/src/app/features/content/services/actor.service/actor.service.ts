import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../auth.service/auth.service';
import { ActorModel } from '../../models/actor.model';

@Injectable({
  providedIn: 'root'
})
export class ActorService {
  private BASE_URL = 'http://localhost:7062/api';

  constructor(private http: HttpClient, private authService: AuthService) { }

  getHttpOptions() {
    const httpOptions = {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${this.authService.getToken()}`
      })
    };
    return httpOptions;
  }

  addActor(actor: ActorModel): Observable<void> {
    const formData = new FormData();
    formData.append('actorImage', actor.imageLink);
    formData.append('actorName', actor.name);
    formData.append('actorDescription', actor.description);
    return this.http.post<void>(`${this.BASE_URL}/actor`, actor, this.getHttpOptions());
  }

  deleteActor(id: string): Observable<void> {
    return this.http.delete<void>(`${this.BASE_URL}/actor/${id}`, this.getHttpOptions());
  }

  updateActor(id: string, actor: ActorModel): Observable<void> {
    const formData = new FormData();
    formData.append('actorImage', actor.imageLink? actor.imageLink : '');
    formData.append('actorName', actor.name? actor.name : '');
    formData.append('actorDescription', actor.description? actor.description : '');
    return this.http.put<void>(`${this.BASE_URL}/actor/${id}`, actor, this.getHttpOptions());
  }

  getActorsByFilmId(id: string): Observable<ActorModel[]> {
    return this.http.get<ActorModel[]>(`${this.BASE_URL}/film/${id}/actors`, this.getHttpOptions());
  }

  getActors(): Observable<ActorModel[]> {
    return this.http.get<ActorModel[]>(`${this.BASE_URL}/actors`, this.getHttpOptions());
  }

  getActorById(id: string): Observable<ActorModel> {
    return this.http.get<ActorModel>(`${this.BASE_URL}/actor/${id}`, this.getHttpOptions());
  }
}
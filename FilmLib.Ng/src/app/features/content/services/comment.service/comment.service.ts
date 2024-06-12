// film-comment.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../auth.service/auth.service';
import { CommentModel } from '../../models/comment.model';

@Injectable({
  providedIn: 'root'
})
export class FilmCommentService {
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

  addComment(id: string, comment: CommentModel): Observable<CommentModel> {
    return this.http.post<CommentModel>(`${this.BASE_URL}/film/${id}/comment`, comment, this.getHttpOptions());
  }

  deleteComment(id: string, commentId: string): Observable<void> {
    return this.http.delete<void>(`${this.BASE_URL}/film/${id}/comment/${commentId}`, this.getHttpOptions());
  }

  getComments(id: string): Observable<CommentModel[]> {
    return this.http.get<CommentModel[]>(`${this.BASE_URL}/film/${id}/comments`, this.getHttpOptions());
  }
}
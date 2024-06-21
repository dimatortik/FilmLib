// film-comment.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { AuthService } from '../../../auth/services/auth.service/auth.service';
import { CommentModel } from '../../models/comment.model';
import { IComment } from '../../interfaces/comment.interface';
import { environment } from '../../../../../environments/environment.prod';

@Injectable({
  providedIn: 'root',
})
export class FilmCommentService {
  private BASE_URL = environment.theMovieDBApi;

  constructor(private http: HttpClient, private authService: AuthService) {}

  getHttpOptions() {
    const httpOptions = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + this.authService.getToken(),
      }),
    };
    return httpOptions;
  }

  addComment(id: string, comment: string): Observable<void> {
    return this.http.post<void>(
      `${this.BASE_URL}/film/${id}/comment`,
      { comment },
      this.getHttpOptions()
    );
  }

  deleteComment(id: string, commentId: string): Observable<void> {
    return this.http.delete<void>(
      `${this.BASE_URL}/film/${id}/comment/${commentId}`,
      this.getHttpOptions()
    );
  }

  getComments(id: string): Observable<IComment[]> {
    return this.http
      .get<CommentModel[]>(
        `${this.BASE_URL}/film/${id}/comments`,
        this.getHttpOptions()
      )
      .pipe(
        map((comments: CommentModel[]) =>
          comments.map((comment: CommentModel) => new CommentModel(comment))
        )
      );
  }
}

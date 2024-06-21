import { Data } from '@angular/router';

export interface IComment {
  id: number;
  body: string;
  createdAt: string;
  filmId: string;
  userName: string;
}

import { Route } from '@angular/router';
import { GenresComponent } from './genres.component';

export const GENRES_ROUTES: Route[] = [
  {
    path: '',
    children: [{ path: ':url', component: GenresComponent }],
  },
];

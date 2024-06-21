import { Route } from '@angular/router';
import { AuthGuard } from './guards/authentication/auth.guard';
import { SignInComponent } from '../auth/sign-in/sign-in.component';
import { SignUpComponent } from '../auth/sign-up/sign-up.component';
import { AddMovieComponent } from '../add-movie/movies.component';

export const AUTH_ROUTES: Route[] = [
  {
    path: '',
    children: [
      {
        path: 'login',
        component: SignInComponent,
      },
      { path: 'register', component: SignUpComponent },
    ],
  },
];

export const ROLE_ROUTES: Route[] = [
  {
    path: '',
    children: [
      {
        path: 'movie',
        component: AddMovieComponent,
      },
    ],
  },
];

import { Routes, CanActivateChildFn, CanActivateFn } from '@angular/router';
import { roleGuard } from './features/auth/guards/authorization/role.guard';
import { AuthGuard } from './features/auth/guards/authentication/auth.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./features/home/home.component').then((m) => m.HomeComponent),
  },
  {
    path: '404',
    loadComponent: () =>
      import('./core/components/not-found/not-found.component').then(
        (m) => m.NotFoundComponent
      ),
  },

  {
    path: '403',
    loadComponent: () =>
      import('./core/components/forbidden/forbidden.component').then(
        (m) => m.ForbiddenComponent
      ),
  },
  {
    path: 'movies',
    loadChildren: () =>
      import('./features/content/content.routes').then((m) => m.CONTENT_ROUTES),
  },

  {
    path: 'actor',
    loadChildren: () =>
      import('./features/people/people.routes').then((m) => m.ACTOR_ROUTES),
  },

  {
    path: 'genre',
    loadChildren: () =>
      import('./features/genres/genres.routes').then((m) => m.GENRES_ROUTES),
  },

  {
    path: 'auth',
    loadChildren: () =>
      import('./features/auth/auth.routes').then((m) => m.AUTH_ROUTES),
  },

  {
    path: 'admin',
    loadChildren: () =>
      import('./features/auth/auth.routes').then((m) => m.ROLE_ROUTES),
    canActivateChild: [roleGuard],
  },

  {
    path: 'user',
    loadComponent: () =>
      import('./features/auth/user/user.component').then(
        (m) => m.UserComponent
      ),
    canActivate: [AuthGuard],
  },

  { path: '**', redirectTo: '404' },
];

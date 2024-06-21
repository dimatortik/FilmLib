import { Route } from '@angular/router';
import { PersonComponent } from './person/person.component';

export const ACTOR_ROUTES: Route[] = [
  {
    path: '',
    children: [{ path: ':url', component: PersonComponent }],
  },
];

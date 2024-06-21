import { ApplicationConfig, isDevMode } from '@angular/core';
import { provideRouter, withViewTransitions } from '@angular/router';
import { routes } from './app.routes';
import { provideServiceWorker } from '@angular/service-worker';
import {
  HTTP_INTERCEPTORS,
  provideHttpClient,
  withFetch,
  withInterceptorsFromDi,
} from '@angular/common/http';
import { register } from 'swiper/element/bundle';
import { provideAnimations } from '@angular/platform-browser/animations';
import { AuthInterceptor } from '././features/auth/services/auth.interceptor';
import { AuthService } from './features/auth/services/auth.service/auth.service';
import { LocalService } from './features/auth/services/local.service/local.service';
import { CookieService } from 'ngx-cookie-service';

register();

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes, withViewTransitions()),
    provideHttpClient(withFetch(), withInterceptorsFromDi()),
    { provide: AuthService, useClass: AuthService },
    { provide: LocalService, useClass: LocalService },
    { provide: CookieService, useClass: CookieService },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    provideAnimations(),
    provideServiceWorker('ngsw-worker.js', {
      enabled: !isDevMode(),
      registrationStrategy: 'registerWhenStable:30000',
    }),
  ],
};

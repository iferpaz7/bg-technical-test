import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { authenticationProvider } from '@modules/auth/providers';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import {
  ENVIRONMENT,
  errorInterceptor,
  injectSessionInterceptor,
  loaderInterceptor,
} from 'acontplus-utils'; //this library is public, created by me
import { environment } from '../environments/environment';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimationsAsync(),
    provideHttpClient(
      withInterceptors([
        injectSessionInterceptor,
        loaderInterceptor, //to show a loader when a request is in progress
        errorInterceptor, //to handle errors globally
      ]),
    ),
    ...authenticationProvider,
    { provide: ENVIRONMENT, useValue: environment }, //pass environment to the library to inject values to interceptors
    provideAnimationsAsync(),
  ],
};

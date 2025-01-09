import { Routes } from '@angular/router';
import { sessionGuard } from '@core/guards/session.guard';

export const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () =>
      import(`./modules/auth/auth.routes`).then((m) => m.routes),
  },
  {
    path: '',
    canActivate: [sessionGuard],
    loadChildren: () =>
      import(`./layouts/main-layout/main-layout.routes`).then((m) => m.routes),
  },
];

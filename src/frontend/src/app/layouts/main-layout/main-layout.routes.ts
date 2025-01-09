import { Routes } from '@angular/router';
import { MainLayoutComponent } from './main-layout.component';

export const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      {
        path: 'home',
        title: 'Home',
        loadChildren: () =>
          import('@modules/home/home.routes').then((r) => r.routes),
      },
      {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full',
      },
      {
        path: 'config',
        loadChildren: () =>
          import(`@config/config.routes`).then((m) => m.routes),
        data: {
          breadcrumb: 'Configuración',
        },
      },
    ],
  },
];

import { Routes } from '@angular/router';
import { LoginComponent } from '@modules/auth/ui/login/login.component';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: '**',
    redirectTo: '/auth/login',
  },
];

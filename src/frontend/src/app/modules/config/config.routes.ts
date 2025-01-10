import { Route } from '@angular/router';
import { PersonComponent } from '@config/ui/person/person.component';
import { UserComponent } from '@config/ui/user/user.component';

export const routes: Route[] = [
  {
    path: 'person',
    component: PersonComponent,
  },
  {
    path: 'user',
    component: UserComponent,
  },
];

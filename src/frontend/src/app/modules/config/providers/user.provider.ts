import { ClassProvider } from '@angular/core';
import { UserRepository } from '@config/domain/repositories/user.repository';
import { UserWebRepository } from '@config/data/user/user-web.repository';

export const userProvider: ClassProvider = {
  provide: UserRepository,
  useClass: UserWebRepository,
};

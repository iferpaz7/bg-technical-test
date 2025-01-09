import { ClassProvider } from '@angular/core';
import { PersonRepository } from '@config/domain/repositories/person.repository';
import { PersonWebRepository } from '@config/data/person/person-web.repository';

export const personProvider: ClassProvider = {
  provide: PersonRepository,
  useClass: PersonWebRepository,
};

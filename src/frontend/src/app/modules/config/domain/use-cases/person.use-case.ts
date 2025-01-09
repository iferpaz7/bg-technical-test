import { inject, Injectable } from '@angular/core';
import { PersonRepository } from '@config/domain/repositories/person.repository';
import { MatPagination } from '@shared/models/pagination';

@Injectable({
  providedIn: 'root',
})
export class PersonUseCase {
  private readonly _personRepository = inject(PersonRepository);

  get(parameters: MatPagination) {
    return this._personRepository.get(parameters);
  }

  getById(id: number) {
    return this._personRepository.getById(id);
  }

  create(person: any) {
    return this._personRepository.create(person);
  }

  update(id: number, person: any) {
    return this._personRepository.update(id, person);
  }

  delete(id: number) {
    return this._personRepository.delete(id);
  }
}

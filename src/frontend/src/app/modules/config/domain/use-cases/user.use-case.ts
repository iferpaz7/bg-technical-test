import { inject, Injectable } from '@angular/core';
import { MatPagination } from '@shared/models/pagination';
import { UserRepository } from '@config/domain/repositories/user.repository';

@Injectable({
  providedIn: 'root',
})
export class UserUseCase {
  private readonly _userRepository = inject(UserRepository);

  get(parameters: MatPagination) {
    return this._userRepository.get(parameters);
  }

  getById(id: number) {
    return this._userRepository.getById(id);
  }

  create(person: any) {
    return this._userRepository.create(person);
  }

  update(id: number, person: any) {
    return this._userRepository.update(id, person);
  }

  delete(id: number) {
    return this._userRepository.delete(id);
  }
}

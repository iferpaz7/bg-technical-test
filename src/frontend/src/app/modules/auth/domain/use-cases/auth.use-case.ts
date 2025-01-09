import { Injectable } from '@angular/core';
import { TokenUseCase } from './token.use-case';
import { AuthRepository } from '../repositories/auth.repository';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthUseCase {
  constructor(
    private readonly _authRepository: AuthRepository,
    private readonly _tokenUseCase: TokenUseCase,
  ) {}

  login(credentials: { username: string; password: string }) {
    return this._authRepository.login(credentials).pipe(
      tap((response) => {
        if (response.code === '1') {
          const [mainData] = JSON.parse(response.payload as string);
          this._tokenUseCase.setToken(mainData.token);
        }
      }),
    );
  }

  logout() {
    return this._authRepository.logout();
  }

  signup<T>(user: T) {
    return this._authRepository.signup<T>(user);
  }
}

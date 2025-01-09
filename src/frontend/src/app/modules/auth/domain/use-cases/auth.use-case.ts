import { inject, Injectable } from '@angular/core';
import { TokenUseCase } from './token.use-case';
import { AuthRepository } from '../repositories/auth.repository';
import { map } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthUseCase {
  private readonly _authRepository = inject(AuthRepository);
  private readonly _tokenUseCase = inject(TokenUseCase);
  private readonly _router = inject(Router);

  login(credentials: { username: string; password: string }) {
    return this._authRepository.login(credentials);
  }

  logout() {
    return this._authRepository.logout();
  }

  signup<T>(user: T) {
    return this._authRepository.signup<T>(user).pipe(
      map((response) => {
        if (response.code === '1') {
          const [mainData] = JSON.parse(response.payload);
          console.log(mainData);
          this._tokenUseCase.setToken(mainData.token);
          this._router.navigate(['/']);
        }
        return response;
      }),
    );
  }
}

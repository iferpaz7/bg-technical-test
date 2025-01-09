import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { TokenUseCase } from '@modules/auth/domain/use-cases/token.use-case';
import { AuthUseCase } from '@modules/auth/domain/use-cases/auth.use-case';

export const sessionGuard: CanActivateFn = (route, state) => {
  const _tokenUseCase = inject(TokenUseCase);
  const _authUseCase = inject(AuthUseCase);
  if (_tokenUseCase.isTokenExpired()) {
    _authUseCase.logout();
  }
  return true;
};

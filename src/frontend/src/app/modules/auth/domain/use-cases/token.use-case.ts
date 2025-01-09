import { inject, Injectable } from '@angular/core';
import { TokenRepository } from '../repositories/token.repository';

@Injectable({
  providedIn: 'root',
})
export class TokenUseCase {
  private readonly _tokenRepository = inject(TokenRepository);

  getToken() {
    return this._tokenRepository.getToken();
  }

  getDecodedToken() {
    return this._tokenRepository.getDecodedToken();
  }

  setToken(token: string) {
    this._tokenRepository.setToken(token);
  }

  removeToken() {
    this._tokenRepository.removeToken();
  }

  isTokenExpired() {
    return this._tokenRepository.isTokenExpired();
  }

  parseJwt() {
    return this._tokenRepository.parseJwt();
  }
}

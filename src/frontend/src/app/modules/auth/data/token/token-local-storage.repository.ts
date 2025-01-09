import { inject, Injectable } from '@angular/core';
import { TokenRepository } from '../../domain/repositories/token.repository';
import { TokenModel } from '../../domain/models/token.model';
import { jwtDecode } from 'jwt-decode';
import { ENVIRONMENT } from 'acontplus-utils';
import { UserData } from '@modules/auth/domain/models/user-data.model';

@Injectable({
  providedIn: 'root',
})
export class TokenLocalStorageRepository extends TokenRepository {
  private readonly _key = inject(ENVIRONMENT).storageKey;

  getToken(): string | null {
    return localStorage.getItem(this._key);
  }

  getDecodedToken(): UserData | null {
    const jwt = this.getToken();
    if (jwt) {
      let token = jwtDecode<any>(jwt);
      const username =
        token['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] ||
        token.name;
      return {
        username: username,
      };
    }
    return null; // Return null if no JWT or something goes wrong
  }

  setToken(token: string): void {
    localStorage.setItem(this._key, token);
  }

  removeToken(): void {
    localStorage.removeItem(this._key);
  }

  parseJwt(): TokenModel['decodedToken'] | null {
    try {
      const token = this.getToken();
      if (token) {
        return JSON.parse(atob(token.split('.')[1]));
      }
      return null;
    } catch (e) {
      return null;
    }
  }
  isTokenExpired(): boolean {
    const token = this.getToken();
    if (!token) return true;

    const decodedToken = jwtDecode<TokenModel['decodedToken']>(token);
    const expiration = Number(decodedToken.exp);
    const now = Math.floor(Date.now() / 1000);

    return expiration < now;
  }
}

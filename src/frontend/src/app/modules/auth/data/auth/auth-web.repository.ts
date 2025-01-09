import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { AuthRepository } from '../../domain/repositories/auth.repository';
import { PATHS_AUTH } from '@modules/auth/domain/constants/auth.constants';
import { ApiResponse, ENVIRONMENT } from 'acontplus-utils';
import { Router } from '@angular/router';
import { TokenUseCase } from '@modules/auth/domain/use-cases/token.use-case';

@Injectable({
  providedIn: 'root',
})
export class AuthWebRepository extends AuthRepository {
  private readonly _url = `${PATHS_AUTH.ACCOUNT}`;
  private readonly _router = inject(Router);
  private readonly _key = inject(ENVIRONMENT).storageKey;
  private readonly _http = inject(HttpClient);
  private readonly _tokenUseCase = inject(TokenUseCase);

  login(credentials: {
    username: string;
    password: string;
  }): Observable<ApiResponse> {
    return this._http.post<ApiResponse>(`${this._url}login`, credentials).pipe(
      map((response) => {
        if (response.code === '1') {
          const { token } = response.payload;
          this._tokenUseCase.setToken(token);
          this._router.navigate(['/']);
        }
        return response;
      }),
    );
  }

  logout() {
    const removeToken = localStorage?.removeItem(this._key);
    if (removeToken == null) {
      this._router.navigate(['/', 'auth']);
    }
  }

  signup<T>(user: T): Observable<ApiResponse> {
    return this._http.post<ApiResponse>(`${this._url}register`, user).pipe(
      map((response) => {
        if (response.code === '1') {
          const { token } = response.payload;
          this._tokenUseCase.setToken(token);
          this._router.navigate(['/']);
        }
        return response;
      }),
    );
  }
}

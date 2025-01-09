import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthRepository } from '../../domain/repositories/auth.repository';
import { PATHS_AUTH } from '@modules/auth/domain/constants/auth.constants';
import { ApiResponse, ENVIRONMENT } from 'acontplus-utils';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthWebRepository extends AuthRepository {
  private readonly _url = `${PATHS_AUTH.ACCOUNT}${PATHS_AUTH.ACCOUNT}`;
  private readonly _router = inject(Router);
  private readonly _environment = inject(ENVIRONMENT);

  constructor(private readonly _http: HttpClient) {
    super();
  }

  login(credentials: {
    username: string;
    password: string;
  }): Observable<ApiResponse> {
    return this._http.post<ApiResponse>(`${this._url}login`, credentials);
  }

  logout() {
    const removeToken = localStorage?.removeItem(this._environment.storageKey);
    if (removeToken == null) {
      this._router.navigate(['/', 'auth']);
    }
  }

  signup<T>(user: T): Observable<ApiResponse> {
    return this._http.post<ApiResponse>(`${this._url}register`, user);
  }
}

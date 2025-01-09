import { Observable } from 'rxjs';
import { ApiResponse } from 'acontplus-utils';

export abstract class AuthRepository {
  abstract login(obj: {
    username: string;
    password: string;
  }): Observable<ApiResponse>;

  abstract signup<T>(user: T): Observable<ApiResponse>;

  abstract logout(): void;
}

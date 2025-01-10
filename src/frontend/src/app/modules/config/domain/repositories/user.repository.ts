import { Observable } from 'rxjs';
import { ApiResponse } from 'acontplus-utils';
import { MatPagination } from '@shared/models/pagination';

export abstract class UserRepository {
  abstract get(parameters: MatPagination): Observable<any>;
  abstract getById(id: number): Observable<ApiResponse>;
  abstract create<T>(data: T): Observable<ApiResponse>;
  abstract update<T>(id: number, data: T): Observable<ApiResponse>;
  abstract delete(id: number): Observable<ApiResponse>;
}

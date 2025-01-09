import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PersonRepository } from '@config/domain/repositories/person.repository';
import { map, Observable } from 'rxjs';
import { ApiResponse } from 'acontplus-utils';
import { PATHS_CONFIG } from '@config/constants/config.constants';
import { MatPagination } from '@shared/models/pagination';

@Injectable({
  providedIn: 'root',
})
export class PersonWebRepository implements PersonRepository {
  private readonly _url = `${PATHS_CONFIG.PERSON}`;
  private readonly _http = inject(HttpClient);

  create<T>(data: T): Observable<ApiResponse> {
    return this._http
      .post<ApiResponse>(`${this._url}`, data)
      .pipe(map((response) => response));
  }

  delete(id: number): Observable<ApiResponse> {
    return this._http
      .delete<ApiResponse>(`${this._url}${id}`)
      .pipe(map((response) => response));
  }

  get(params: MatPagination): Observable<any> {
    return this._http
      .get<ApiResponse>(
        `${this._url}?pageIndex=${params.pageIndex}&pageSize=${params.pageSize}&textSearch=${params.textSearch}`,
      )
      .pipe(
        map((response) => {
          if (response.code !== '1') {
            return { persons: [], totalRecords: 0 };
          }
          const persons = JSON.parse(response.payload);
          return {
            persons,
            totalRecords: persons[0]?.totalRecords,
          };
        }),
      );
  }

  getById(id: number): Observable<ApiResponse> {
    return this._http
      .get<ApiResponse>(`${this._url}${id}`)
      .pipe(map((response) => response));
  }

  update<T>(id: number, data: T): Observable<ApiResponse> {
    return this._http
      .put<ApiResponse>(`${this._url}${id}`, data)
      .pipe(map((response) => response));
  }
}

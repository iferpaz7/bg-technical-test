import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SHARED_PATHS } from '@shared/enums/shared.constants';
import { map } from 'rxjs';
import { ApiResponse } from 'acontplus-utils';
import { IdentificationType } from '@shared/models/identification-type';

@Injectable({
  providedIn: 'root',
})
export class IdentificationTypeService {
  private readonly _http = inject(HttpClient);
  private readonly _url = `${SHARED_PATHS.IDENTIFICATION_TYPE}`;

  get() {
    return this._http.get<ApiResponse>(this._url).pipe(
      map((response) => {
        if (response.code !== '1') {
          return { identificationTypes: [] };
        }

        return {
          identificationTypes: response.payload as IdentificationType[],
        };
      }),
    );
  }
}

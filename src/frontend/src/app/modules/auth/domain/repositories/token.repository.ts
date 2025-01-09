import { TokenModel } from '../models/token.model';

export abstract class TokenRepository {
  abstract getToken(): string | null;
  abstract setToken(token: string): void;
  abstract getDecodedToken(): any;
  abstract removeToken(): void;
  abstract parseJwt(): TokenModel['decodedToken'] | null;
  abstract isTokenExpired(): boolean;
}

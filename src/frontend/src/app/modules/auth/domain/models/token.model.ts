export interface TokenModel {
  token: string;
  decodedToken: {
    name?: string;
    userId?: number;
    exp?: number;
    [key: string]: any;
  };
}

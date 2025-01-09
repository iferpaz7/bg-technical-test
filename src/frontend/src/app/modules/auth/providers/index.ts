import { authProvider } from './auth.provider';
import { tokenProvider } from './token.provider';

export const authenticationProvider = [authProvider, tokenProvider];

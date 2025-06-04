import { HttpInterceptorFn, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../authService/auth-service';
import { catchError, switchMap, throwError } from 'rxjs';
import { RefreshTokenRequest } from '../../models/authModels/RefreshTokenRequest';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();

  let authReq = req;
  if (token) {
    authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        let refreshTokenRequest : RefreshTokenRequest = { refreshToken: authService.getRefreshToken() || ''}
        
        return authService.refreshToken(refreshTokenRequest).pipe(
          switchMap((loginResponse) => {
            authService.setAuthToken(loginResponse.token, loginResponse.refreshToken);

            const newReq = req.clone({
              setHeaders: {
                Authorization: `Bearer ${loginResponse.token}`
              }
            });
            return next(newReq);
          }),
          catchError(() => {
            authService.clearToken();
            return throwError(() => error);
          })
        );
      }
      return throwError(() => error);
    })
  );
};

import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../authService/auth-service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const token = authService.getToken();
  const router = inject(Router);
  
  if (!token) {
    console.error('Access denied. No token found.');
    router.navigate(['/login']);
  }
  
  return true;
};

import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { LogInToken } from '../shared/token';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);

  const token = localStorage.getItem(LogInToken);

  if(token) {
    console.log('Access granted.');
    return true;
  }
  router.navigateByUrl('/log-in');
  return false;
};

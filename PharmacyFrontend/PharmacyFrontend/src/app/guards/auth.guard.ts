import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { LogInToken } from '../shared/token';

function decodeToken(token: string): any {
  try {
    return JSON.parse(atob(token.split('.')[1]));
  } catch {
    return null;
  }
}

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);

  const token = localStorage.getItem(LogInToken);

  if(!token) {
    router.navigateByUrl('/log-in');
    return false;
  }

  const decoded = decodeToken(token);
  const requiredRoles = route.data['roles'] as string[];

   const userRole =
    decoded?.[
      'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
    ];


  if(decoded && requiredRoles.includes(userRole)) {
    return true;
  }

  
  router.navigateByUrl('/forbidden');
  return false;
};

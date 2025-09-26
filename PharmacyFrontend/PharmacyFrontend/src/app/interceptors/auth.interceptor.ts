import { HttpInterceptorFn } from '@angular/common/http';
import { LogInToken } from '../shared/token';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem(LogInToken);

  if(!token) {
    console.log(`Token does not exist.`);
  }

  const clonedReq = req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`
    }
  });
  return next(req);
};

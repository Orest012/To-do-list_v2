import { HttpInterceptorFn } from '@angular/common/http';
import { HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { inject } from '@angular/core';
import { AuthService } from '../data/services/login/auth.service';
import { CookieService } from 'ngx-cookie-service';

const addToken = (req: HttpRequest<any>, token: string | null): HttpRequest<any> => {
  if (token) {
    return req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }
  return req; 
};

export const AuthInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService); 
  const token = authService.getToken();

  if (token) {
    console.log('Token found:', token);
  } else {
    console.warn('No token available!');
  }

  return next(addToken(req, token)).pipe(
    catchError(error => {
      console.error('Interceptor error occurred:', error);
      return throwError(() => error);
    })
  );
};




// @Injectable()
// export class AuthInterceptor implements HttpInterceptor {
//   intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
//     // Отримуємо токен з localStorage
//     const token = localStorage.getItem('authToken');
    
//     // Якщо токен існує, додаємо його в заголовки
//     if (token) {
//       const cloned = req.clone({
//         setHeaders: {
//           Authorization: `Bearer ${token}`  // Додаємо токен до заголовків
//         }
//       });
//       return next.handle(cloned);
//     }

//     // Якщо токен не знайдений, просто пропускаємо запит без змін
//     return next.handle(req);
//   }
// }



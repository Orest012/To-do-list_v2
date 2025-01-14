import { inject } from '@angular/core';
import { CanActivate } from '@angular/router';
import { AuthService } from '../data/services/login/auth.service';
import { Router } from '@angular/router';
export const authGuard = () => {
  const isLoggendIn = inject(AuthService).isAuth;
  if(isLoggendIn){
    return true;
  }
  return inject(Router).createUrlTree(['/login'])
};

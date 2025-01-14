import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'https://localhost:7287/api/Account';
  token: string | null = null;
  cookieService = inject(CookieService);

  constructor(private http: HttpClient) {
    this.token = this.cookieService.get('authToken');
  }

  get isAuth(){
    if (!this.token) {
      this.token = this.cookieService.get('authToken');
    }
    return !!this.token;
  }

  login(loginData: { username: string, password: string }): Observable<any> {
    return this.http.post<any>(this.apiUrl + "/login", loginData).pipe(
      tap(response => {
        localStorage.setItem('authToken', response.token);
        this.cookieService.set('authToken', response.token);

        this.token =  response.token;
        console.log("saved");
      })
    );
  }

  logout() {
    this.cookieService.delete('authToken');
  }

  getToken() : string{
    return this.cookieService.get('authToken');
  }
}

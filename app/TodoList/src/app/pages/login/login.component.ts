import { Component } from '@angular/core';
import {FormsModule} from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AuthService } from '../../data/services/login/auth.service';
import { Router } from '@angular/router';
import { inject } from '@angular/core';

@Component({
  selector: 'app-login',
  imports: [FormsModule, HttpClientModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  router = inject(Router);
  constructor(private authService: AuthService) {}

  onSubmit(form: any) {
    console.log("before");
    if (form.valid) {
      console.log("if");

      const loginData = form.value;
      this.authService.login(loginData).subscribe({
        next: (response) => {
          console.log('Login successful:', response);
          this.router.navigate(['home'])
        },
        error: (error) => {
          console.error('Login failed:', error);
        }
      });
    }
  }
}

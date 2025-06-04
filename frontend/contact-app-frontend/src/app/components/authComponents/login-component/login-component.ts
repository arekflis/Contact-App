import { Component } from '@angular/core';
import { AuthService } from '../../../services/authService/auth-service';
import { LoginRequest } from '../../../models/authModels/LoginRequest';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login-component',
  standalone: true,
  imports: [RouterModule, FormsModule],
  templateUrl: './login-component.html',
  styleUrl: './login-component.css'
})
export class LoginComponent {

  constructor(private authService: AuthService, private router: Router) { }

  email: string = '';
  password: string = '';

  onSubmit(): void {
    const loginRequest : LoginRequest = {
      email: this.email,
      password: this.password
    };

    this.authService.login(loginRequest).subscribe({
      next: (response) => {
        this.authService.setAuthToken(response.token);
        console.log('Login successful');
        this.router.navigate(['/contacts']);
      },
      error: (error) => {
        console.error('Login failed:', error);
      }
    });
  }
}

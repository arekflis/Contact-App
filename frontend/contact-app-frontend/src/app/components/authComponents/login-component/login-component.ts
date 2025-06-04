import { Component } from '@angular/core';
import { AuthService } from '../../../services/authService/auth-service';
import { LoginRequest } from '../../../models/authModels/LoginRequest';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { FormGroup, FormsModule, Validators, FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login-component',
  standalone: true,
  imports: [RouterModule, FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './login-component.html',
  styleUrl: './login-component.css'
})
export class LoginComponent {

  constructor(private authService: AuthService, private router: Router) { }

  loginForm: FormGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(8), Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\W)\\S+$')])
  });

  onSubmit(): void {
    if (this.loginForm.invalid) {
      console.error('Form is invalid');
      return;
    }

    const loginRequest : LoginRequest = {
      email: this.loginForm.get('email')?.value ?? "",
      password: this.loginForm.get('password')?.value ?? "" 
    };

    this.authService.login(loginRequest).subscribe({
      next: (response) => {
        this.authService.setAuthToken(response.token, response.refreshToken);
        console.log('Login successful');
        this.router.navigate(['/contacts']);
      },
      error: (error) => {
        console.error('Login failed:', error);
      }
    });
  }
}

import { Component } from '@angular/core';
import { AuthService } from '../../../services/authService/auth-service';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register-component',
  imports: [RouterModule, FormsModule],
  templateUrl: './register-component.html',
  styleUrl: './register-component.css'
})
export class RegisterComponent {

  constructor(private authService: AuthService, private router: Router) { }

  email: string = '';
  password: string = '';

  onSubmit(): void {
    const registerRequest = {
      email: this.email,
      password: this.password
    };

    this.authService.register(registerRequest).subscribe({
      next: (response) => {
        console.log(response);
        this.router.navigate(['/login']);
      },
      error: (error) => {
        console.error('Registration failed:', error);
      }
    });
  }
}

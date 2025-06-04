import { Component } from '@angular/core';
import { AuthService } from '../../../services/authService/auth-service';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { FormsModule, FormGroup, Validators, FormControl, ReactiveFormsModule} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RegisterRequest } from '../../../models/authModels/RegisterRequest';

@Component({
  selector: 'app-register-component',
  standalone: true,
  imports: [RouterModule, FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './register-component.html',
  styleUrl: './register-component.css'
})
export class RegisterComponent {

  constructor(private authService: AuthService, private router: Router) { }

  registrationForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(8), Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\W)\\S+$')])
  });

  onSubmit(): void {
    if (this.registrationForm.invalid) {
      console.error('Form is invalid');
      return;
    }

    const registerRequest: RegisterRequest = {
      email: this.registrationForm.get('email')?.value ?? "",
      password: this.registrationForm.get('password')?.value ?? ""
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

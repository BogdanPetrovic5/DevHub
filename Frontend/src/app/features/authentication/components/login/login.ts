import { Component, inject, output, signal } from '@angular/core';
import { AuthService } from '../../../../core/services/auth/auth-service';
import { FormBuilder, Validators, ɵInternalFormsSharedModule, ReactiveFormsModule, ValidationErrors } from '@angular/forms';
import { LoginRequest } from '../../../../core/models/auth.model';
import { Router } from '@angular/router';
import { GoogleButton } from '../../../../shared/components/google-button/google-button';

@Component({
  selector: 'app-login',
  imports: [ɵInternalFormsSharedModule, ReactiveFormsModule, GoogleButton],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  switchToRegister = output<void>();

  private _authService = inject(AuthService);
  private _fb = inject(FormBuilder);
  private _router = inject(Router)
  
  isLoading = signal(false);
  errorMessage = signal<string | null>(null);
  validationErrors = signal<ValidationErrors | null>(null);

  loginForm = this._fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    rememberMe: [false]
  })

  validationErrorValue(key: string): string | null {
    const errors = this.validationErrors();
    return errors ? errors[key]?.[0] || null : null;
  }
  onCredentials(credentials: { idToken: string, nonce: string | null }) {
    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.validationErrors.set(null);
    this._authService.authenticateWithGoogle(credentials).subscribe({
      next: (response) => {
        this.isLoading.set(false)
        if (response.success) {
          this._router.navigate(['dashboard'])
        }
      },
      error: (err) => {
        this.isLoading.set(false);
        console.log('Error during Google authentication:', err);
        if (err.error?.errors) {
          this.validationErrors.set(err.error.errors);
        } else {
          this.errorMessage.set(err.error?.message ?? 'Login failed')
        } 
      }

    })
  }
  login(){
    if(this.loginForm.invalid) return;

    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.validationErrors.set(null);
    const loginData = this.loginForm.value as LoginRequest;

    this._authService.login(loginData).subscribe({
      next:(response)=>{
        this.isLoading.set(false)
        if(response.success){
          this._router.navigate(['dashboard'])
        }
      },
      error:(err)=>{
        this.isLoading.set(false);
        if (err.error?.errors) {
          this.validationErrors.set(err.error.errors);
        } else {
          this.errorMessage.set(err.error?.message ?? 'Login failed')
        }
      }
    })
  }
}

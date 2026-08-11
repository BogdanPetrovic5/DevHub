import { Component, inject, output, signal } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { AuthService } from '../../../../core/services/auth/auth-service';
import { RegisterRequest } from '../../../../core/models/auth.model';
import { Router } from '@angular/router';


function passwordMatchValidator(form: AbstractControl) {
  const password = form.get('password')?.value;
  const confirm = form.get('confirmPassword')?.value;
  return password === confirm ? null : { passwordMismatch: true };
}

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {
  switchToLogin = output<void>();

  private _authService = inject(AuthService);
  private _fb = inject(FormBuilder);
  private _router = inject(Router);
  isLoading = signal(false);
  errorMessage = signal<string | null>(null);
  validationErrors = signal<ValidationErrors | null>(null);

  registerForm = this._fb.group({
    username: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    password: ['', [Validators.required]],
    confirmPassword: ['', Validators.required],
  }, { validators: passwordMatchValidator });

  get passwordMismatch(): boolean {
    return this.registerForm.hasError('passwordMismatch') &&
      !!this.registerForm.get('confirmPassword')?.touched;
  }
  validationErrorValue(key:string): string | null {
    const errors = this.validationErrors();
    
    return errors ? errors[key]?.[0] || null : null;
  }
  
  register() {

    if (this.registerForm.invalid) return;
    this.validationErrors.set(null);
    this.isLoading.set(true);
    this.errorMessage.set(null);

    const { confirmPassword, ...registerData } = this.registerForm.value;

    this._authService.register(registerData as RegisterRequest).subscribe({
      next: (response) => {
        this.isLoading.set(false);
        if (response.success) {
          this._router.navigate(['dashboard']);
        }
      },
      error: (err) => {
        this.isLoading.set(false);
        if(err.error?.errors) {
          this.validationErrors.set(err.error.errors);
          console.log('Validation Errors:', err.error.errors);
        } else {
          this.errorMessage.set(err.error?.message || 'An error occurred during registration.');
        }
      }
    });
  }
}

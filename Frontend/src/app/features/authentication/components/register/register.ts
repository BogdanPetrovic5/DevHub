import { Component, inject, output, signal } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../../core/services/auth/auth-service';
import { RegisterRequest } from '../../../../core/models/auth.model';

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

  isLoading = signal(false);
  errorMessage = signal<string | null>(null);

  registerForm = this._fb.group({
    username: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    password: ['', [Validators.required, Validators.minLength(6)]],
    confirmPassword: ['', Validators.required],
  }, { validators: passwordMatchValidator });

  get passwordMismatch(): boolean {
    return this.registerForm.hasError('passwordMismatch') &&
      !!this.registerForm.get('confirmPassword')?.touched;
  }

  register() {

    if (this.registerForm.invalid) return;
    this.isLoading.set(true);
    this.errorMessage.set(null);

    const { confirmPassword, ...registerData } = this.registerForm.value;

    this._authService.register(registerData as RegisterRequest).subscribe({
      next: () => {
        this.isLoading.set(false);
      },
      error: (err) => {
        console.log(err)
        this.isLoading.set(false);
        this.errorMessage.set(err.error?.message ?? 'Registration failed');
      }
    });
  }
}

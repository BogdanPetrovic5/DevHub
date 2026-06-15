import { Component, inject, output, signal } from '@angular/core';
import { AuthService } from '../../../../core/services/auth/auth-service';
import { FormBuilder, Validators, ɵInternalFormsSharedModule, ReactiveFormsModule } from '@angular/forms';
import { LoginRequest } from '../../../../core/models/auth.model';

@Component({
  selector: 'app-login',
  imports: [ɵInternalFormsSharedModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  switchToRegister = output<void>();

  private _authService = inject(AuthService);
  private _fb = inject(FormBuilder);

  isLoading = signal(false);
  errorMessage = signal<string | null>(null);

  loginForm = this._fb.group({
    email: ['', Validators.required],
    password: ['', [Validators.required, Validators.minLength(6)]],
    rememberMe: [false]
  })

  login(){
    if(this.loginForm.invalid) return;

    this.isLoading.set(false);
    this.errorMessage.set(null);
    const loginData = this.loginForm.value as LoginRequest;

    this._authService.login(loginData).subscribe({
      next:()=>{
        this.isLoading.set(false)
      },
      error:(err)=>{
        console.log(err)
        this.isLoading.set(false);
        this.errorMessage.set(err.error?.message ?? 'Login failed')
      }
    })
  }
}

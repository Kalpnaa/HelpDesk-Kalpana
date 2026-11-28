import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-create-user',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './create-user.component.html',
  styleUrl: './create-user.component.scss'
})
export class CreateUserComponent {

  userForm: FormGroup;
  isLoading = false;
  successMessage = '';
  errorMessage = '';
  showPassword = false;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private router: Router
  ) {
    this.userForm = this.fb.group({
      fullName: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      role: ['Agent', Validators.required]
    });
  }

  createUser(): void {
    this.successMessage = '';
    this.errorMessage = '';

    if (this.userForm.invalid) {
      this.userForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;

    this.userService.createUser(this.userForm.value).subscribe({
      next: () => {
        this.isLoading = false;
        this.successMessage = 'User created successfully!';

        this.userForm.reset({
          fullName: '',
          email: '',
          password: '',
          role: 'Agent'
        });
      },
      error: (error) => {
        this.isLoading = false;

        this.errorMessage =
          error.error?.message ||
          error.error?.title ||
          'Unable to create user. Please try again.';
      }
    });
  }
}
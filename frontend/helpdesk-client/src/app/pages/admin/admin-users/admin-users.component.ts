import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { UserService } from '../../../services/user.service';
import { AuthService } from '../../../services/auth.service';
import { UserResponse } from '../../../models/user.model';

@Component({
  selector: 'app-admin-users',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './admin-users.component.html',
  styleUrl: './admin-users.component.scss'
})
export class AdminUsersComponent implements OnInit {

  users: UserResponse[] = [];
  isLoading = true;
  errorMessage = '';
  searchTerm = '';

  constructor(
    private userService: UserService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.userService.getAllUsers().subscribe({
      next: (data) => {
        this.users = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading users:', error);
        this.errorMessage =
          error.status === 403
            ? 'You are not authorized to view users.'
            : 'Unable to load users. Please try again.';
        this.isLoading = false;
      }
    });
  }

  get managers(): number {
    return this.users.filter(user => user.role === 'Manager').length;
  }

  get agents(): number {
    return this.users.filter(user => user.role === 'Agent').length;
  }

  get customers(): number {
    return this.users.filter(user => user.role === 'Customer').length;
  }

  get filteredUsers(): UserResponse[] {
    const search = this.searchTerm.toLowerCase().trim();

    if (!search) {
      return this.users;
    }

    return this.users.filter(user =>
      user.fullName.toLowerCase().includes(search) ||
      user.email.toLowerCase().includes(search) ||
      user.role.toLowerCase().includes(search)
    );
  }

  logout(): void {
    this.authService.logout();
  }
}
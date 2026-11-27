import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import {
  RegisterRequest,
  RegisterResponse,
  LoginRequest,
  LoginResponse,
  UserProfile,
  UserData
} from '.././models/auth.model';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'http://localhost:5029/api/auth';
  constructor(private http: HttpClient) {}

  register(data: RegisterRequest): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${this.apiUrl}/register`, data);
  }

 login(data: LoginRequest): Observable<LoginResponse> {
  return this.http.post<LoginResponse>(
    `${this.apiUrl}/login`,
    data
  ).pipe(
    tap(response => {
      this.saveToken(response.token);

      this.saveUser({
        id: response.id,
        fullName: response.fullname,
        email: response.email,
        role: response.role
      });
    })
  );
}

  saveToken(token: string): void {
    sessionStorage.setItem('token', token);
  }

  getToken(): string | null {
    return sessionStorage.getItem('token');
  }

  removeToken(): void {
    sessionStorage.removeItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  logout(): void {
    this.removeToken();
    sessionStorage.removeItem(this.userKey);
  }

  getCurrentUser(): Observable<UserProfile> {
    return this.http.get<UserProfile>(`${this.apiUrl}/me`);
  }

  private userKey = 'currentUser';

  saveUser(user: UserData): void {
    sessionStorage.setItem(this.userKey, JSON.stringify(user));
  }

  getUser(): UserData | null {
    const user = sessionStorage.getItem(this.userKey);

    return user ? (JSON.parse(user) as UserData) : null;
  }

  getUserRole(): string | null {
    return this.getUser()?.role ?? null;
  }
}

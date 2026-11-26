import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable,tap } from 'rxjs';
import {
  RegisterRequest,
  RegisterResponse,
  LoginRequest,
  LoginResponse,
  UserProfile,
} from '../../models/auth.model';
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
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, data).pipe(tap(response=>this.saveToken(response.token)));
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

  logout():void{
    this.removeToken();
  }

  getCurrentUser(): Observable<UserProfile> {
  return this.http.get<UserProfile>(`${this.apiUrl}/me`);
}
}

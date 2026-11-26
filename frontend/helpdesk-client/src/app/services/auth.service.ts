import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RegisterRequest, RegisterResponse,LoginRequest,LoginResponse } from '../../models/auth.model';
@Injectable({
  providedIn: 'root'
})


export class AuthService {

  private apiUrl = 'http://localhost:5029/api/auth';
  constructor(private http: HttpClient) { }

  register(data: RegisterRequest) : Observable<RegisterResponse>
  {
    return this.http.post<RegisterResponse>(
      `${this.apiUrl}/register`,
      data
    )
  }

  login(data: LoginRequest):Observable<LoginResponse>
  {
    return this.http.post<LoginResponse>(
      `${this.apiUrl}/login`,
      data
    );
  }
}

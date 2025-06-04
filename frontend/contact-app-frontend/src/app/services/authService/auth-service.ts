import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoginRequest } from '../../models/authModels/LoginRequest';
import { RegisterRequest } from '../../models/authModels/RegisterRequest';
import { BehaviorSubject, Observable } from 'rxjs';
import { LoginResponse } from '../../models/authModels/LoginResponse';
import { RefreshTokenRequest } from '../../models/authModels/RefreshTokenRequest';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private httpClient: HttpClient) {}

  private loggedInUser$ = new BehaviorSubject<boolean>(this.isLoggedIn())

  isLoggedIn$ = this.loggedInUser$.asObservable();

  login(loginRequest: LoginRequest): Observable<LoginResponse>{
    return this.httpClient.post<LoginResponse>("/api/Auth/login", loginRequest);
  }

  register(registerRequest: RegisterRequest): Observable<string>{
    return this.httpClient.post("/api/Auth/register", registerRequest, { responseType: 'text' });
  }

  refreshToken(refreshTokenRequest: RefreshTokenRequest): Observable<LoginResponse> {
    return this.httpClient.post<LoginResponse>("/api/Auth/refresh-token", refreshTokenRequest);
  }

  setAuthToken(token: string, refreshToken: string): void {
    localStorage.setItem("authToken", token);
    localStorage.setItem("refreshToken", refreshToken);
    this.loggedInUser$.next(true);
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem("authToken");
  }

  clearToken(): void {
    localStorage.removeItem("authToken");
    localStorage.removeItem("refreshToken");
    this.loggedInUser$.next(false);
  }

  getToken(): string | null {
    return localStorage.getItem("authToken");
  }

  getRefreshToken(): string | null {
    return localStorage.getItem("refreshToken");
  }
}

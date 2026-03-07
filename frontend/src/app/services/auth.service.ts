import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { getApiUrl, shouldUseMockData } from '../config/api.config';

export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  password: string;
}

export interface AuthResponse {
  success: boolean;
  token?: string;
  message?: string;
  user?: {
    userId: string;
    username: string;
    isActive: boolean;
  };
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly authUrl = getApiUrl('auth');
  private readonly authStatusSubject = new BehaviorSubject<boolean>(this.isLoggedIn());
  readonly authStatus$ = this.authStatusSubject.asObservable();

  constructor(private http: HttpClient) {}

  login(credentials: LoginRequest): Observable<AuthResponse> {
    if (shouldUseMockData()) {
      // Mock login for development
      return new Observable(observer => {
        setTimeout(() => {
          if (credentials.username === 'admin' && credentials.password === 'password') {
            observer.next({
              success: true,
              token: 'mock-jwt-token',
              message: 'Login successful',
              user: { userId: 'mock-user-001', username: 'admin', isActive: true }
            });
          } else {
            observer.next({
              success: false,
              message: 'Invalid credentials'
            });
          }
          observer.complete();
        }, 1000);
      });
    }

    return this.http.post<AuthResponse>(`${this.authUrl}/login`, credentials)
      .pipe(
        map(response => ({ ...response, success: !!response.token })),
        catchError(this.handleError)
      );
  }

  register(credentials: RegisterRequest): Observable<AuthResponse> {
    if (shouldUseMockData()) {
      // Mock register for development
      return new Observable(observer => {
        setTimeout(() => {
          observer.next({
            success: true,
            token: 'mock-jwt-token',
            message: 'Registration successful',
            user: { userId: 'mock-user-001', username: credentials.username, isActive: true }
          });
          observer.complete();
        }, 1000);
      });
    }

    return this.http.post<AuthResponse>(`${this.authUrl}/register`, credentials)
      .pipe(
        map(response => ({ ...response, success: !!response.token })),
        catchError(this.handleError)
      );
  }

  logout(): void {
    localStorage.removeItem('authToken');
    localStorage.removeItem('user');
    this.setAuthState(false);
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('authToken');
  }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  getUser(): any {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred!';
    if (error.error instanceof ErrorEvent) {
      // Client-side or network error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Backend returned an unsuccessful response code
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.error(errorMessage);
    return throwError(() => new Error(errorMessage));
  }

  setAuthState(isLoggedIn: boolean): void {
    this.authStatusSubject.next(isLoggedIn);
  }
}
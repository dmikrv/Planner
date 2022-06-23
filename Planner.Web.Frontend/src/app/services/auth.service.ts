import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable, tap } from 'rxjs';

import { Token } from '../models/token.model';
import {AUTH_API_URL, RESOURCE_API_URL} from "../app-injections-tokens";

export const ACCESS_TOKEN_KEY = 'planner_access_token';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private _token: string | null = null;

  constructor(
    private http: HttpClient,
    @Inject(AUTH_API_URL) private apiUrl: string,
    @Inject(RESOURCE_API_URL) private resUrl: string,
    private jwtHelper: JwtHelperService,
    private router: Router
  ) { }

  login(email: string, password: string, rememberMe: boolean): Observable<Token> {
    return this.http.post<Token>(`${this.apiUrl}/api/auth/login`, {
      email, password
    }).pipe(
      tap(token => {
        if (rememberMe) {
          localStorage.setItem(ACCESS_TOKEN_KEY, token.access_token);
          console.log('auth service: save token to the local storage');
        } else {
          sessionStorage.setItem(ACCESS_TOKEN_KEY, token.access_token);
          console.log('auth service: save token to the session storage');
        }
      })
    );
  }

  signup(email: string, password: string): Observable<object> {
    return this.http.post(`${this.resUrl}/api/accounts/signup`, {
      email, password
    });
  }

  public get isAuthenticated(): boolean {
    if (!this._token) {
      this._token = localStorage.getItem(ACCESS_TOKEN_KEY) ?? sessionStorage.getItem(ACCESS_TOKEN_KEY);
    }
    return this._token != null && !this.jwtHelper.isTokenExpired(this._token);
  }

  public get email(): string | null {
    return this._token
      ? this.jwtHelper.decodeToken(this._token)['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']
      : null;
  }

  logout() {
    this._token = null;
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    sessionStorage.removeItem(ACCESS_TOKEN_KEY);
  }
}

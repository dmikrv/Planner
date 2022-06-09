import {Inject, Injectable} from '@angular/core';
import {Observable, tap} from "rxjs";

import {Token} from "../models/token.model";
import {HttpClient} from "@angular/common/http";
import {AUTH_API_URL} from "../app-injections-tokens";
import {JwtHelperService} from "@auth0/angular-jwt";
import {Router} from "@angular/router";

export const ACCESS_TOKEN_KEY = 'planner_access_token';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private http: HttpClient,
    @Inject(AUTH_API_URL) private apiUrl: string,
    private jwtHelper: JwtHelperService,
    private router: Router
  ) { }

  login(email: string, password: string, rememberMe: boolean): Observable<Token> {
    return this.http.post<Token>(`${this.apiUrl}/api/auth/login`, {
      email, password
    }).pipe(
      tap(token => {
        if (rememberMe) {
          localStorage.setItem(ACCESS_TOKEN_KEY, token.access_token)
          console.log("auth service: save token to the local storage")
        }
        else {
          sessionStorage.setItem(ACCESS_TOKEN_KEY, token.access_token)
          console.log("auth service: save token to the session storage")
        }
      })
    );
  }

  isAuthenticated(): boolean {
    let token = localStorage.getItem(ACCESS_TOKEN_KEY) ?? sessionStorage.getItem(ACCESS_TOKEN_KEY);
    return token != null && !this.jwtHelper.isTokenExpired(token);
  }

  logout(): void {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    sessionStorage.removeItem(ACCESS_TOKEN_KEY);
    this.router.navigate(['']);
  }
}

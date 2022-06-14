import { Injectable } from '@angular/core';
import {CanActivate, Router, UrlTree} from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class LogoutGuard implements CanActivate {
  constructor(private router: Router) {
  }

  canActivate(): UrlTree {
    return this.router.createUrlTree(['login']);
  }
}

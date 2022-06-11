import { Component, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';

import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Planner.Frontend';

  @ViewChild('sidenav') sidenav?: MatSidenav;

  public get isAuthenticated(): boolean {
    return this.as.isAuthenticated;
  }

  logout(): void {
    this.as.logout();
  }


  constructor(private as: AuthService) {
  }
}

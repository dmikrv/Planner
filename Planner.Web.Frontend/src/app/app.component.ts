import { Component, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import {AuthService} from "../../../Planner.Web.Frontend/src/app/services/auth.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Planner';

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

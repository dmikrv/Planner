import { Component } from '@angular/core';
import {AuthService} from "./services/auth.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Planner.Frontend';

  public get isAuthenticated(): boolean {
    return this.as.isAuthenticated();
  }

  logout(): void {
    this.as.logout();
  }


  constructor(private as: AuthService) {
  }
}

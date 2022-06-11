import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {

  @Input() title: string | undefined;

  @Output() sidebarToggle = new EventEmitter();

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
  }

  public logout(): void {
    if (this.authService.isAuthenticated) {
      this.authService.logout();
    }
  }

  public get isAuthenticated(): boolean {
    return this.authService.isAuthenticated;
  }

  public get userEmail(): string | null {
    return this.authService.email;
  }

  public toggle(): void {
    this.sidebarToggle.emit();
  }

}

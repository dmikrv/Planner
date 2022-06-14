import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { AuthService } from '../services/auth.service';
import {Router} from "@angular/router";

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {

  @Input() title: string | undefined;

  @Output() sidebarToggle = new EventEmitter();

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
  }

  public logout(): void {
    this.router.navigate(['/logout']);
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

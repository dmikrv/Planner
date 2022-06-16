import {Component, EventEmitter, HostBinding, Input, OnInit, Output} from '@angular/core';

import {Router} from "@angular/router";
import {AuthService} from "../../services/auth.service";
import {FormControl} from "@angular/forms";
import {OverlayContainer} from "@angular/cdk/overlay";

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {

  @Input() title: string | undefined;
  @Output() sidebarToggle = new EventEmitter();

  toggleControl = new FormControl(false);
  @HostBinding('class') className = '';

  constructor(private authService: AuthService, private router: Router, private overlay: OverlayContainer) { }

  ngOnInit(): void {
    this.toggleControl.valueChanges.subscribe((darkMode) => {
      const darkClassName = 'darkMode';
      this.className = darkMode ? darkClassName : '';
      if (darkMode) {
        this.overlay.getContainerElement().classList.add(darkClassName);
      } else {
        this.overlay.getContainerElement().classList.remove(darkClassName);
      }
    });
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

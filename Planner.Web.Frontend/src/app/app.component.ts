import {Component, OnInit, ViewChild} from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import {AuthService} from "../../../Planner.Web.Frontend/src/app/services/auth.service";
import {Project} from "./models/project.model";
import {ProjectService} from "./services/project.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  title = 'Planner';

  @ViewChild('sidenav') sidenav?: MatSidenav;

  projects: Project[] = [];

  public get isAuthenticated(): boolean {
    return this.as.isAuthenticated;
  }

  logout(): void {
    this.as.logout();
  }


  constructor(private as: AuthService, private projectService: ProjectService) {
  }

  ngOnInit(): void {
    this.projectService.get().subscribe(res => this.projects = res
    );
  }

}

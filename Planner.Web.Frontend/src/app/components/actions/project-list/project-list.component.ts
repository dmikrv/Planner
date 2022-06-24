import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {Subscription, tap} from "rxjs";

@Component({
  selector: 'app-next',
  templateUrl: './project-list.component.html',
  styleUrls: ['./project-list.component.scss']
})
export class ProjectListComponent implements OnInit {
  id?: number;
  private subscription: Subscription;

  constructor(private activateRoute: ActivatedRoute) {
    this.subscription = activateRoute.params.pipe(tap(console.log)).subscribe(params=>this.id=+params['id']);
  }

  ngOnInit(): void {


  }

}

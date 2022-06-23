import {Component, OnInit} from '@angular/core';
import {ActionState} from "../../../models/state.action.model";

@Component({
  selector: 'app-next',
  templateUrl: './someday.component.html',
  styleUrls: ['./someday.component.scss']
})
export class SomedayComponent implements OnInit {
  state: ActionState = ActionState.Someday;

  constructor() { }

  ngOnInit(): void {
  }

}

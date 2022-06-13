import {Component, OnInit} from '@angular/core';
import {ActionState} from "../../../models/state.action.model";

@Component({
  selector: 'app-next',
  templateUrl: './next.component.html',
  styleUrls: ['./next.component.css']
})
export class NextComponent implements OnInit {
  state: ActionState = ActionState.Next;

  constructor() { }

  ngOnInit(): void {
  }

}

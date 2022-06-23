import {Component, OnInit} from '@angular/core';
import {ActionState} from "../../../models/state.action.model";

@Component({
  selector: 'app-next',
  templateUrl: './inbox.component.html',
  styleUrls: ['./inbox.component.scss']
})
export class InboxComponent implements OnInit {
  state: ActionState = ActionState.Inbox;

  constructor() { }

  ngOnInit(): void {
  }

}

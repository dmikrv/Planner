import {Component, ElementRef, Input, OnInit, ViewChild} from '@angular/core';
import {MatTableDataSource} from '@angular/material/table';

import {Action, BaseActionColumns} from "../../../../models/action.model";
import {ActionService} from "../../../../services/action.service";
import {ActionState} from "../../../../models/state.action.model";


@Component({
  selector: 'app-base-table',
  templateUrl: './base.component.html',
  styleUrls: ['./base.component.css']
})
export class BaseTableComponent implements OnInit {
  displayedColumns: string[] = BaseActionColumns.map((col) => col.key);
  columnsSchema: any = BaseActionColumns;
  dataSource = new MatTableDataSource<Action>();
  valid: any = {};

  @Input() state: ActionState = ActionState.Next;

  constructor(private resService: ActionService) { }

  ngOnInit(): void {
    this.resService.get(this.state).subscribe((res: Action[]) => {
      this.dataSource.data = res;
    });
  }

  editRow(row: Action) {
    if (row.dueDate) {
      const d = new Date(row.dueDate)
      row.dueDate = new Date(d.getFullYear(), d.getMonth(), d.getDate(), d.getHours(), d.getMinutes() - d.getTimezoneOffset());
    }

    if (row.id === 0) {
      this.resService.add(row).subscribe((newRow: Action) => {
        row.id = newRow.id;
        row.isEdit = false;
      });
    } else {
      this.resService.update(row).subscribe((res) => {row.id = res.id; row.isEdit = false});
      console.log("update", row);
    }
  }

  addRow() {
    const newRow: Action = {
      id: 0,
      text: 'to do',
      state: this.state,
      isDone: false,
      isFocused: false,
      isEdit: true,
      labelTags: [],
      areaTags: [],
      contactTags: []
    };
    this.dataSource.data = [newRow, ...this.dataSource.data];
  }

  removeRow(id: number) {
    this.resService.delete(id).subscribe(() => {
      this.dataSource.data = this.dataSource.data.filter(
        (u: Action) => u.id !== id
      );
    });
  }

  inputHandler(e: any, element: Action, key: string) {
    if (!this.valid[element.id]) {
      this.valid[element.id] = {};
    }
    this.valid[element.id][key] = e.target.validity.valid;
  }

  isDone(e: any, element: Action) {
    element.isDone = e.checked;
    this.resService.update(element).subscribe((res) => {element.id = res.id;});
  }

  isFocused(e: any, element: Action) {
    element.isFocused = e.checked;
    this.resService.update(element).subscribe((res) => {element.id = res.id;});
  }

  disableSubmit(id: number) {
    if (this.valid[id]) {
      return Object.values(this.valid[id]).some((item) => item === false);
    }
    return false;
  }
}

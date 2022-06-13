import {Component, Input, OnInit} from '@angular/core';
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
    this.resService.get(this.state).subscribe((res: any) => {
      this.dataSource.data = res;
    });
  }

  editRow(row: Action) {
    if (row.id === 0) {
      this.resService.add(row).subscribe((newRow: Action) => {
        row.id = newRow.id;
        row.isEdit = false;
      });
    } else {
      this.resService.update(row).subscribe((res) => {row.id = res.id; row.isEdit = false});
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

  inputHandler(e: any, id: number, key: string) {
    if (!this.valid[id]) {
      this.valid[id] = {};
    }
    this.valid[id][key] = e.target.validity.valid;
  }

  disableSubmit(id: number) {
    if (this.valid[id]) {
      return Object.values(this.valid[id]).some((item) => item === false);
    }
    return false;
  }

}

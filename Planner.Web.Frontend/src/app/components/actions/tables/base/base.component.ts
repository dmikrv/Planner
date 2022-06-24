import {Component, Input, OnInit} from '@angular/core';
import {MatTableDataSource} from '@angular/material/table';

import {Action, BaseActionColumns} from "../../../../models/action.model";
import {ActionService} from "../../../../services/action.service";
import {ActionState} from "../../../../models/state.action.model";
import {Project} from "../../../../models/project.model";
import {ProjectService} from "../../../../services/project.service";
import {ConfirmDeleteDialogComponent} from "../../../confirm-delete-dialog/confirm-delete-dialog.component";
import {MatDialog} from "@angular/material/dialog";


@Component({
  selector: 'app-base-table',
  templateUrl: './base.component.html',
  styleUrls: ['./base.component.scss']
})
export class BaseTableComponent implements OnInit {
  displayedColumns: string[] = BaseActionColumns.map((col) => col.key);
  columnsSchema: any = BaseActionColumns;
  dataSource = new MatTableDataSource<Action>();
  projects: Project[] = [];
  valid: any = {};

  @Input() stateShow?: ActionState = undefined;
  @Input() focusShow?: boolean = undefined;
  @Input() doneShow?: boolean = undefined;
  id?: number;
  @Input() public set projectIdShow(id: number | undefined) {
    this.id = id;
    this.ngOnInit();
  }
  @Input() addActionButtonShow: boolean = true;
  @Input() removeAllButtonShow: boolean = false;

  constructor(private resService: ActionService,
              private projectService: ProjectService,
              public dialog: MatDialog) { }

  ngOnInit(): void {
    this.resService.get(this.stateShow, this.doneShow, this.focusShow, this.id).subscribe((res: Action[]) => {
      this.dataSource.data = res;
    });
    this.projectService.get().subscribe((res: Project[]) => {
      this.projects = res;
    });
  }

  getProjectNameById(id: number): string {
    return this.projects.find(x => x.id === id)?.name ?? '';
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
      this.resService.update(row).subscribe((res) => {
        row.id = res.id;
        row.dueDate = res.dueDate;
        row.isEdit = false
      });
    }

    this.dataSource.data = this.dataSource.data.filter(
      (u: Action) => ( (this.stateShow ? u.state == this.stateShow : true)
        && (this.projectIdShow ? u.projectId == this.projectIdShow : true))
    );
  }

  addRow() {
    const newRow: Action = {
      id: 0,
      text: 'to do',
      state: this.stateShow ?? ActionState.Next,
      isDone: false,
      isFocused: this.focusShow ?? false,
      isEdit: true,
      projectId: this.projectIdShow ?? undefined,
      labelTags: [],
      areaTags: [],
      contactTags: []
    };
    this.dataSource.data = [newRow, ...this.dataSource.data];
  }

  removeRow(id: number) {
    this.dialog
      .open(ConfirmDeleteDialogComponent)
      .afterClosed()
      .subscribe((confirm) => {
        if (confirm) {
          this.resService.delete(id).subscribe(() => {
            this.dataSource.data = this.dataSource.data.filter(
              (u: Action) => u.id !== id
            );
          });
        }
      });
  }

  removeAllRows() {
    this.dialog
      .open(ConfirmDeleteDialogComponent)
      .afterClosed()
      .subscribe((confirm) => {
        if (confirm) {
          this.resService.deleteMany(this.dataSource.data).subscribe(() => {
            this.dataSource.data = []
          });
        }
      });
  }

  inputHandler(e: any, element: Action, key: string) {
    if (!this.valid[element.id]) {
      this.valid[element.id] = {};
    }
    this.valid[element.id][key] = e.target.validity.valid;
  }

  done(e: any, element: Action) {
    element.isDone = e.checked;
    this.resService.update(element).subscribe((res) => {
      element.id = res.id;
      this.dataSource.data = this.dataSource.data.filter(
        (u: Action) => (this.doneShow === undefined || !this.doneShow && !u.isDone || this.doneShow && u.isDone)
      );
    });
  }

  focus(element: Action) {
    element.isFocused = !element.isFocused;
    this.resService.update(element).subscribe((res) => {
      element.id = res.id;
      this.dataSource.data = this.dataSource.data.filter(
        (u: Action) => ( this.focusShow === undefined || !this.focusShow && !u.isFocused || this.focusShow && u.isFocused)
      );
    });
  }

  disableSubmit(id: number) {
    if (this.valid[id]) {
      return Object.values(this.valid[id]).some((item) => item === false);
    }
    return false;
  }
}

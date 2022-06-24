import {Component, OnInit} from '@angular/core';
import {MatTableDataSource} from '@angular/material/table';

import {MatDialog} from "@angular/material/dialog";
import {BaseProjectColumns, Project} from "../../models/project.model";
import {ProjectService} from "../../services/project.service";
import {ProjectState} from "../../models/state.project.model";
import {ConfirmDeleteDialogComponent} from "../confirm-delete-dialog/confirm-delete-dialog.component";


@Component({
  selector: 'app-base-table',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.scss']
})
export class ProjectsComponent implements OnInit {
  displayedColumns: string[] = BaseProjectColumns.map((col) => col.key);
  columnsSchema: any = BaseProjectColumns;
  dataSource = new MatTableDataSource<Project>();
  valid: any = {};

  constructor(private projectService: ProjectService,
              public dialog: MatDialog) { }

  ngOnInit(): void {
    this.projectService.get().subscribe((res: Project[]) => {
      this.dataSource.data = res;
    });
  }

  editRow(row: Project) {
    if (row.dueDate) {
      const d = new Date(row.dueDate)
      row.dueDate = new Date(d.getFullYear(), d.getMonth(), d.getDate(), d.getHours(), d.getMinutes() - d.getTimezoneOffset());
    }

    if (row.id === 0) {
      this.projectService.add(row).subscribe((newRow: Project) => {
        row.id = newRow.id;
        row.isEdit = false;
      });
    } else {
      this.projectService.update(row).subscribe((res) => {
        row.id = res.id;
        row.dueDate = res.dueDate;
        row.isEdit = false
      });
    }
  }

  addRow() {
    const newRow: Project = {
      id: 0,
      name: 'New project',
      state: ProjectState.Active,
      isEdit: true
    };
    this.dataSource.data = [newRow, ...this.dataSource.data];
  }

  removeRow(id: number) {
    this.dialog
      .open(ConfirmDeleteDialogComponent)
      .afterClosed()
      .subscribe((confirm) => {
        if (confirm) {
          this.projectService.delete(id).subscribe(() => {
            this.dataSource.data = this.dataSource.data.filter(
              (u: Project) => u.id !== id
            );
          });
        }
      });
  }

  inputHandler(e: any, element: Project, key: string) {
    if (!this.valid[element.id]) {
      this.valid[element.id] = {};
    }
    this.valid[element.id][key] = e.target.validity.valid;
  }

  disableSubmit(id: number) {
    if (this.valid[id]) {
      return Object.values(this.valid[id]).some((item) => item === false);
    }
    return false;
  }
}

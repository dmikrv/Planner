import { Component, OnInit } from '@angular/core';
import {MatTableDataSource} from "@angular/material/table";
import {TrashAction, TrashActionColumns} from "../../../models/trash-action.model";
import {MatDialog} from "@angular/material/dialog";
import {TrashActionService} from "../../../services/trash-action.service";
import {ConfirmDeleteDialogComponent} from "../../confirm-delete-dialog/confirm-delete-dialog.component";
import {Action} from "../../../models/action.model";

@Component({
  selector: 'app-trash',
  templateUrl: './trash.component.html',
  styleUrls: ['./trash.component.scss']
})
export class TrashComponent implements OnInit {
  displayedColumns: string[] = TrashActionColumns.map((col) => col.key);
  columnsSchema: any = TrashActionColumns;
  dataSource = new MatTableDataSource<TrashAction>();
  valid: any = {};

  constructor(public dialog: MatDialog, private resService: TrashActionService) { }

  ngOnInit(): void {
    this.resService.get().subscribe((res: any) => {
      this.dataSource.data = res;
    });
  }

  get isAnySelected(): boolean {
    return this.dataSource.data.filter((u: TrashAction) => u.isSelected).length > 0;
  }

  editRow(row: TrashAction) {
      this.resService.update(row).subscribe(() => (row.isEdit = false));
    }

  removeRow(id: number) {
    this.resService.delete(id).subscribe(() => {
      this.dataSource.data = this.dataSource.data.filter(
        (u: TrashAction) => u.id !== id
      );
    });
  }

  removeAllRows() {
    this.dialog
      .open(ConfirmDeleteDialogComponent)
      .afterClosed()
      .subscribe((confirm) => {
        if (confirm) {
          this.resService.deleteAll().subscribe(() => {
            this.dataSource.data = this.dataSource.data.filter(
              (u: TrashAction) => false
            );
          });
        }
      });
  }

  removeSelectedRows() {
    const rows = this.dataSource.data.filter((u: TrashAction) => u.isSelected);
    this.dialog
      .open(ConfirmDeleteDialogComponent)
      .afterClosed()
      .subscribe((confirm) => {
        if (confirm) {
          this.resService.deleteMany(rows).subscribe(() => {
            this.dataSource.data = this.dataSource.data.filter(
              (u: TrashAction) => !u.isSelected
            );
          });
        }
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

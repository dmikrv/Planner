import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

import { BaseTagColumns } from '../../../models/base.tag.model';
import {MatDialog} from "@angular/material/dialog";
import {ConfirmDeleteDialogComponent} from "../../confirm-delete-dialog/confirm-delete-dialog.component";
import {AreaTag} from "../../../models/area.tag.model";
import {AreaTagService} from "../../../services/area-tag.service";

@Component({
  selector: 'app-areas',
  templateUrl: './areas.component.html',
  styleUrls: ['./areas.component.scss']
})
export class AreasComponent implements OnInit {
  displayedColumns: string[] = BaseTagColumns.map((col) => col.key);
  columnsSchema: any = BaseTagColumns;
  dataSource = new MatTableDataSource<AreaTag>();
  valid: any = {};

  constructor(public dialog: MatDialog, private resService: AreaTagService) { }

  ngOnInit(): void {
    this.resService.get().subscribe((res: any) => {
      this.dataSource.data = res;
    });
  }

  get isAnySelected(): boolean {
    return this.dataSource.data.filter((u: AreaTag) => u.isSelected).length > 0;
  }

  editRow(row: AreaTag) {
    if (row.id === 0) {
      this.resService.add(row).subscribe((newRow: AreaTag) => {
        row.id = newRow.id;
        row.isEdit = false;
      });
    } else {
      this.resService.update(row).subscribe(() => (row.isEdit = false));
    }
  }

  addRow() {
    const newRow: AreaTag = {
      id: 0,
      name: 'new row',
      color: undefined,
      isEdit: true,
      isSelected: false,
    };
    this.dataSource.data = [newRow, ...this.dataSource.data];
  }

  removeRow(id: number) {
    this.resService.delete(id).subscribe(() => {
      this.dataSource.data = this.dataSource.data.filter(
        (u: AreaTag) => u.id !== id
      );
    });
  }

  removeSelectedRows() {
    const rows = this.dataSource.data.filter((u: AreaTag) => u.isSelected);
    this.dialog
      .open(ConfirmDeleteDialogComponent)
      .afterClosed()
      .subscribe((confirm) => {
        if (confirm) {
          this.resService.deleteMany(rows).subscribe(() => {
            this.dataSource.data = this.dataSource.data.filter(
              (u: AreaTag) => !u.isSelected
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

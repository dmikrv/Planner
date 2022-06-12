import { Component, OnInit } from '@angular/core';

import {LabelTagService} from "../../../services/label-tag.service";
import {LabelTag} from "../../../models/label.tag.model";

import {Observable} from "rxjs";
import {BaseTagColumns} from "../../../models/base.tag.model";
import {MatTableDataSource} from "@angular/material/table";

@Component({
  selector: 'app-labels',
  templateUrl: './labels.component.html',
  styleUrls: ['./labels.component.css']
})
export class LabelsComponent implements OnInit {
  displayedColumns: string[] = BaseTagColumns.map((col) => col.key);
  columnsSchema: any = BaseTagColumns;
  dataSource = new MatTableDataSource<LabelTag>();
  valid: any = {};

  constructor(private resService: LabelTagService) { }

  ngOnInit(): void {
    this.resService.get().subscribe((res: any) => {
      this.dataSource.data = res;
    })
  }

  editRow(row: LabelTag) {
    if (row.id === 0) {
      this.resService.add(row).subscribe((newRow: LabelTag) => {
        row.id = newRow.id;
        row.isEdit = false;
      });
    } else {
      this.resService.update(row).subscribe(() => (row.isEdit = false));
    }
  }

  addRow() {
    const newRow: LabelTag = {
      id: 0,
      name: 'new raw',
      color: undefined,
      isEdit: true,
      isSelected: false,
    };
    this.dataSource.data = [newRow, ...this.dataSource.data];
  }

  removeRow(id: number) {
    this.resService.delete(id).subscribe(() => {
      this.dataSource.data = this.dataSource.data.filter(
        (u: LabelTag) => u.id !== id
      );
    });
  }

  // removeSelectedRows() {
  //   const tags = this.dataSource.data.filter((u: LabelTag) => u.isSelected);
  //   this.dialog
  //     .open(ConfirmDialogComponent)
  //     .afterClosed()
  //     .subscribe((confirm) => {
  //       if (confirm) {
  //         this.userService.deleteUsers(users).subscribe(() => {
  //           this.dataSource.data = this.dataSource.data.filter(
  //             (u: User) => !u.isSelected
  //           );
  //         });
  //       }
  //     });
  // }

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

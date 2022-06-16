import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

import { BaseTagColumns } from '../../../models/base.tag.model';
import {MatDialog} from "@angular/material/dialog";
import {ConfirmDeleteDialogComponent} from "../../confirm-delete-dialog/confirm-delete-dialog.component";
import {ContactTag} from "../../../models/contact.tag.model";
import {ContactTagService} from "../../../services/contact-tag.service";

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.scss']
})
export class ContactsComponent implements OnInit {
  displayedColumns: string[] = BaseTagColumns.map((col) => col.key);
  columnsSchema: any = BaseTagColumns;
  dataSource = new MatTableDataSource<ContactTag>();
  valid: any = {};

  constructor(public dialog: MatDialog, private resService: ContactTagService) { }

  ngOnInit(): void {
    this.resService.get().subscribe((res: any) => {
      this.dataSource.data = res;
    });
  }

  get isAnySelected(): boolean {
    return this.dataSource.data.filter((u: ContactTag) => u.isSelected).length > 0;
  }

  editRow(row: ContactTag) {
    if (row.id === 0) {
      this.resService.add(row).subscribe((newRow: ContactTag) => {
        row.id = newRow.id;
        row.isEdit = false;
      });
    } else {
      this.resService.update(row).subscribe(() => (row.isEdit = false));
    }
  }

  addRow() {
    const newRow: ContactTag = {
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
        (u: ContactTag) => u.id !== id
      );
    });
  }

  removeSelectedRows() {
    const rows = this.dataSource.data.filter((u: ContactTag) => u.isSelected);
    this.dialog
      .open(ConfirmDeleteDialogComponent)
      .afterClosed()
      .subscribe((confirm) => {
        if (confirm) {
          this.resService.deleteMany(rows).subscribe(() => {
            this.dataSource.data = this.dataSource.data.filter(
              (u: ContactTag) => !u.isSelected
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

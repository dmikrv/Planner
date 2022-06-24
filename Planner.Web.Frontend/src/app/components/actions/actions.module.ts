import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {NextComponent} from "./next/next.component";
import { TrashComponent } from './trash/trash.component';
import {MatTableModule} from "@angular/material/table";
import {MatCheckboxModule} from "@angular/material/checkbox";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatDatepickerModule} from "@angular/material/datepicker";
import {MatSelectModule} from "@angular/material/select";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatCardModule} from "@angular/material/card";
import {MatInputModule} from "@angular/material/input";
import {MatButtonModule} from "@angular/material/button";
import {MatIconModule} from "@angular/material/icon";
import {MatDialogModule} from "@angular/material/dialog";
import {BaseTableComponent} from './tables/base/base.component';
import {MatChipsModule} from "@angular/material/chips";
import {MatAutocompleteModule} from "@angular/material/autocomplete";
import { TagControlComponent } from './tables/tag-control/tag-control.component';
import {DoneComponent} from "./done/done.component";
import {InboxComponent} from "./inbox/inbox.component";
import {SomedayComponent} from "./someday/someday.component";
import {FocusComponent} from "./focus/focus.component";
import {ProjectListComponent} from "./project-list/project-list.component";

@NgModule({
  declarations: [
    NextComponent,
    SomedayComponent,
    InboxComponent,
    DoneComponent,
    FocusComponent,
    TrashComponent,
    BaseTableComponent,
    TagControlComponent,
    ProjectListComponent
  ],
  imports: [
    CommonModule,
    MatTableModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    FormsModule,
    MatCheckboxModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatChipsModule,
    MatAutocompleteModule,
    ReactiveFormsModule
  ]
})
export class ActionsModule { }

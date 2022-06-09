import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {LabelsComponent} from "./labels/labels.component";
import {MatTableModule} from "@angular/material/table";
import {MatCardModule} from "@angular/material/card";



@NgModule({
  declarations: [
    LabelsComponent
  ],
    imports: [
      CommonModule,
      MatTableModule,
      MatCardModule,
    ]
})
export class TagsModule { }

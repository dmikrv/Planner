import { Component, OnInit } from '@angular/core';
import {LabelTagService} from "../../../services/label-tag.service";
import {LabelTag} from "../../../models/label.tag.model";
import {delay, Observable} from "rxjs";

@Component({
  selector: 'app-labels',
  templateUrl: './labels.component.html',
  styleUrls: ['./labels.component.css']
})
export class LabelsComponent implements OnInit {

  labels?: Observable<LabelTag[]>;
  columns = ['Id', 'Name', 'Color'];

  constructor(private lbs: LabelTagService) { }

  ngOnInit(): void {
    this.labels = this.lbs.getAll();
  }

}

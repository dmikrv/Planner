import {Component, ElementRef, Input, OnInit, ViewChild} from '@angular/core';
import {Action} from "../../../../models/action.model";
import {COMMA, SPACE} from "@angular/cdk/keycodes";
import {FormControl} from "@angular/forms";
import {map, Observable, startWith} from "rxjs";
import {BaseTag} from "../../../../models/base.tag.model";
import {AreaTag} from "../../../../models/area.tag.model";
import {MatChipInputEvent} from "@angular/material/chips";
import {MatAutocompleteSelectedEvent} from "@angular/material/autocomplete";
import {LabelTagService} from "../../../../services/label-tag.service";
import {AreaTagService} from "../../../../services/area-tag.service";
import {ContactTagService} from "../../../../services/contact-tag.service";
import {LabelTag} from "../../../../models/label.tag.model";
import {ContactTag} from "../../../../models/contact.tag.model";

@Component({
  selector: 'app-tag-control',
  templateUrl: './tag-control.component.html',
  styleUrls: ['./tag-control.component.scss']
})
export class TagControlComponent implements OnInit {
  @Input() action!: Action

  separatorKeysCodes: number[] = [SPACE, COMMA];
  tagCtrl = new FormControl();
  filteredTags = new Observable<BaseTag[]>();

  areaTags: AreaTag[] = [];
  labelTags: LabelTag[] = [];
  contactTags: ContactTag[] = [];
  allTags: BaseTag[] = [];

  @ViewChild('tagInput') tagInput!: ElementRef<HTMLInputElement>;

  constructor(private labelTagService: LabelTagService,
              private areaTagService: AreaTagService,
              private contactTagService: ContactTagService) {

    this.areaTagService.get().subscribe((areaTags: AreaTag[]) => {
      this.areaTags.push(...areaTags);
      this.labelTagService.get().subscribe((labelTags: LabelTag[]) => {
        this.labelTags.push(...labelTags);
        this.contactTagService.get().subscribe((contactTags: ContactTag[]) => {
          this.contactTags.push(...contactTags);
          this.allTags = [...this.areaTags, ...this.labelTags, ...this.contactTags];
        });
      });
    });
  }

  ngOnInit(): void {
    this.filteredTags = this.tagCtrl.valueChanges.pipe(
      startWith(null),
      map((inputText: string) => {
        let returns: BaseTag[];
        if (typeof inputText === 'string')
          returns = this._filter(inputText);
        else
          returns = this.allTags.slice();

        return returns.filter(tag => !(this.action.areaTags?.find(x => x.name === tag.name)
          || this.action.contactTags?.find(x => x.name === tag.name) || this.action.labelTags?.find(x => x.name === tag.name)));
      }),
    );
  }

  removeAreaTag(tagId: number) {
    this.action.areaTags = this.action.areaTags?.filter(
      (u: AreaTag) => u.id !== tagId
    );
  }

  removeLabelTag(tagId: number) {
    this.action.labelTags = this.action.labelTags?.filter(
      (u: AreaTag) => u.id !== tagId
    );
  }

  removeContactTag(tagId: number) {
    this.action.contactTags = this.action.contactTags?.filter(
      (u: AreaTag) => u.id !== tagId
    );
  }

  addTag(event: MatChipInputEvent) {
    const value = (event.value || '').trim();

    if (value) {
      this.action.labelTags?.push({id: 0, name: value.trim(), isSelected: false, isEdit: false})
    }

    event.chipInput!.clear();
    this.tagCtrl.setValue(null);
  }

  selectedTag(event: MatAutocompleteSelectedEvent): void {
    const tag = <BaseTag>event.option.value;
    console.log("selected");

    if (this.areaTags.includes(tag))
      this.action.areaTags?.push(tag);
    else if (this.contactTags.includes(tag))
      this.action.contactTags?.push(tag);
    else if (this.labelTags.includes(tag))
      this.action.labelTags?.push(tag);

    this.tagInput.nativeElement.value = '';
    this.tagCtrl.setValue(null);
  }

  private _filter(value: string): BaseTag[] {
    const filterValue = value.toLowerCase();
    return this.allTags.filter(tag => tag.name.toLowerCase().includes(filterValue));
  }
}

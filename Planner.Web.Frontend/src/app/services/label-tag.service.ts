import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import {forkJoin, Observable} from 'rxjs';

import {RESOURCE_API_URL} from "../app-injections-tokens";
import { LabelTag } from '../models/label.tag.model';

@Injectable({
  providedIn: 'root'
})
export class LabelTagService {

  constructor(
    private http: HttpClient,
    @Inject(RESOURCE_API_URL) private apiUrl: string,
  ) { }

  get(): Observable<LabelTag[]> {
    return this.http.get<LabelTag[]>(`${this.apiUrl}/api/tags/labels`);
  }

  add(label: LabelTag): Observable<LabelTag> {
    return this.http.post<LabelTag>(`${this.apiUrl}/api/tags/labels`, label);
  }

  update(label: LabelTag): Observable<LabelTag> {
    return this.http.put<LabelTag>(`${this.apiUrl}/api/tags/labels`, label);
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/api/tags/labels/${id}`);
  }

  deleteMany(labels: LabelTag[]) {
    return forkJoin(
      labels.map((label) =>
        this.http.delete<LabelTag>(`${this.apiUrl}/api/tags/labels/${label.id}`)
      )
    );
  }
}

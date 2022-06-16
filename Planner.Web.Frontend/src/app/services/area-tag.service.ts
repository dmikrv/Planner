import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import {forkJoin, Observable} from 'rxjs';

import {RESOURCE_API_URL} from "../app-injections-tokens";
import {AreaTag} from "../models/area.tag.model";

@Injectable({
  providedIn: 'root'
})
export class AreaTagService {

  constructor(
    private http: HttpClient,
    @Inject(RESOURCE_API_URL) private apiUrl: string,
  ) { }

  get(): Observable<AreaTag[]> {
    return this.http.get<AreaTag[]>(`${this.apiUrl}/api/tags/areas`);
  }

  add(area: AreaTag): Observable<AreaTag> {
    return this.http.post<AreaTag>(`${this.apiUrl}/api/tags/areas`, area);
  }

  update(area: AreaTag): Observable<AreaTag> {;
    return this.http.put<AreaTag>(`${this.apiUrl}/api/tags/areas`, area);
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/api/tags/areas/${id}`);
  }

  deleteMany(areas: AreaTag[]) {
    return forkJoin(
      areas.map((area) =>
        this.http.delete<AreaTag>(`${this.apiUrl}/api/tags/areas/${area.id}`)
      )
    );
  }
}

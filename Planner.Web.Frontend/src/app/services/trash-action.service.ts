import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import {forkJoin, Observable} from 'rxjs';

import {TrashAction} from "../models/trash-action.model";
import {RESOURCE_API_URL} from "../app-injections-tokens";

@Injectable({
  providedIn: 'root'
})
export class TrashActionService {

  constructor(
    private http: HttpClient,
    @Inject(RESOURCE_API_URL) private apiUrl: string,
  ) { }

  get(): Observable<TrashAction[]> {
    return this.http.get<TrashAction[]>(`${this.apiUrl}/api/trashactions`);
  }

  update(action: TrashAction): Observable<TrashAction> {
    return this.http.put<TrashAction>(`${this.apiUrl}/api/trashactions`, action);
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/api/trashactions/${id}`);
  }

  deleteAll() {
    return this.http.delete(`${this.apiUrl}/api/trashactions`);
  }

  deleteMany(actions: TrashAction[]) {
    return forkJoin(
      actions.map((action) =>
        this.http.delete<TrashAction>(`${this.apiUrl}/api/trashactions/${action.id}`)
      )
    );
  }
}

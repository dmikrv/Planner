import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import {forkJoin, map, Observable, switchMap, tap} from 'rxjs';

import {RESOURCE_API_URL} from "../app-injections-tokens";
import {Action} from "../models/action.model";
import {ActionState} from "../models/state.action.model";

@Injectable({
  providedIn: 'root'
})
export class ActionService {

  constructor(
    private http: HttpClient,
    @Inject(RESOURCE_API_URL) private apiUrl: string,
  ) { }

  get(state?: ActionState, done?:boolean, focused?: boolean, projectId?: number): Observable<Action[]> {
    console.log(done);
    let url = `${this.apiUrl}/api/actions?`;
    if (state !== undefined)
      url += `state=${state}&`;
    if (done !== undefined)
      url += `done=${done}&`;
    if (focused !== undefined)
      url += `focused=${focused}&`;
    if (projectId !== undefined)
      url += `projectId=${projectId}`;
    return this.http.get<Action[]>(url);
  }

  add(action: Action): Observable<Action> {
    return this.http.post<Action>(`${this.apiUrl}/api/actions`, action);
  }

  update(action: Action): Observable<Action> {
    let deleteId = action.id;
    return this.http.post<Action>(`${this.apiUrl}/api/actions`, action).pipe(
      switchMap((action) => this.http.delete(`${this.apiUrl}/api/actions/${deleteId}?toTrashAction=false`)
        .pipe(map(() => action)))
    );
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/api/actions/${id}`);
  }

  deleteMany(actions: Action[]) {
    return forkJoin(
      actions.map((action) =>
        this.http.delete<Action>(`${this.apiUrl}/api/actions/${action.id}`)
      )
    );
  }
}

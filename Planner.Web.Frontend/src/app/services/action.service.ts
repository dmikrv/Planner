import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import {forkJoin, Observable, switchMap, tap} from 'rxjs';

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

  get(state?: ActionState, done?:boolean, focused?: boolean ): Observable<Action[]> {
    let url = `${this.apiUrl}/api/actions?`;
    if (state)
      url += `state=${state}&`;
    else if (done)
      url += `done=${focused}&`;
    else if (focused)
      url += `focused=${focused}`;
    return this.http.get<Action[]>(url);
  }

  add(action: Action): Observable<Action> {
    return this.http.post<Action>(`${this.apiUrl}/api/actions`, action);
  }

  update(action: Action): Observable<Action> {
    return this.http.put<Action>(`${this.apiUrl}/api/actions`, action);
  //   return this.http.delete(`${this.apiUrl}/api/actions/${action.id}?toTrashAction=false`).pipe(
  //     switchMap(() => this.http.post<Action>(`${this.apiUrl}/api/actions`, action)),
  // );
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

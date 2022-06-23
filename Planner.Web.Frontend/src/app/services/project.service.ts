import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import {forkJoin, Observable, switchMap, tap} from 'rxjs';

import {RESOURCE_API_URL} from "../app-injections-tokens";
import {Project} from "../models/project.model";

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  constructor(
    private http: HttpClient,
    @Inject(RESOURCE_API_URL) private apiUrl: string,
  ) { }

  get(): Observable<Project[]> {
    return this.http.get<Project[]>(`${this.apiUrl}/api/projects`);
  }

  add(project: Project): Observable<Project> {
    return this.http.post<Project>(`${this.apiUrl}/api/projects`, project);
  }

  update(project: Project): Observable<Project> {
    return this.http.put<Project>(`${this.apiUrl}/api/projects`, project);
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/api/projects/${id}`);
  }
}

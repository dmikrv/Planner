import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import {forkJoin, Observable} from 'rxjs';

import {RESOURCE_API_URL} from "../app-injections-tokens";
import {ContactTag} from "../models/contact.tag.model";

@Injectable({
  providedIn: 'root'
})
export class ContactTagService {

  constructor(
    private http: HttpClient,
    @Inject(RESOURCE_API_URL) private apiUrl: string,
  ) { }

  get(): Observable<ContactTag[]> {
    return this.http.get<ContactTag[]>(`${this.apiUrl}/api/tags/contacts`);
  }

  add(contact: ContactTag): Observable<ContactTag> {
    return this.http.post<ContactTag>(`${this.apiUrl}/api/tags/contacts`, contact);
  }

  update(contact: ContactTag): Observable<ContactTag> {;
    return this.http.put<ContactTag>(`${this.apiUrl}/api/tags/contacts`, contact);
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/api/tags/contacts/${id}`);
  }

  deleteMany(contacts: ContactTag[]) {
    return forkJoin(
      contacts.map((contact) =>
        this.http.delete<ContactTag>(`${this.apiUrl}/api/tags/contacts/${contact.id}`)
      )
    );
  }
}

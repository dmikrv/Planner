import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LoaderService {
  private _isLoading$ = new Subject<boolean>();

  constructor() {
  }

  public get isLoading$(): Subject<boolean> {
    return this._isLoading$;
  }

  public show() {
    this._isLoading$.next(true);
  }

  public hide() {
    this._isLoading$.next(false);
  }
}

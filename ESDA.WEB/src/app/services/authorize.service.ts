import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

@Injectable({
  providedIn: 'root',
})
export class AuthorizeService {
  public isAuthorized: boolean;
  public observableAuth: BehaviorSubject<boolean>;
  public user_name: string;

  constructor() {
    this.isAuthorized = false;
    this.observableAuth = new BehaviorSubject<boolean>(this.isAuthorized);
  }

  notifyAuthChange() {
    this.observableAuth.next(this.isAuthorized);
  }
}

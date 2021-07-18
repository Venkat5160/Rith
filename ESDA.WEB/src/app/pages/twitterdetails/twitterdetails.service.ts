import { Injectable } from '@angular/core';
import { CommonService } from 'src/app/core/http/common.service';

@Injectable({
  providedIn: 'root',
})
export class TwitterDetailsService {
  constructor(private _http: CommonService) {

  }
  [index: string]: any;
 
  userServiceURL: string = '/Twitter';

  getTwitterDetails() {
    return this._http.get(this.userServiceURL + '/GetTwitterDetails');
  }
  saveTwitterDetails(twitter) {
    return this._http.post(this.userServiceURL + '/SaveTwitterDetails', twitter);
  }
  deleteTwitterDetails(id) {
    return this._http.delete(this.userServiceURL + '/DeleteTwitterDetails', id)
  }

}

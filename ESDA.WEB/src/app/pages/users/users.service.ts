import { Injectable } from '@angular/core';
import { CommonService } from 'src/app/core/http/common.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private _http: CommonService) {

  }
  [index: string]: any;

  userServiceURL: string = '/Account';

  getUsersList(data) {
    return this._http.post(this.userServiceURL + '/UsersList', data);
  }
  getUsersById(userId) {

    return this._http.get(this.userServiceURL + '/GetUserById' + '?userId=' + userId);
  };
  getUserDetails() {
    return this._http.get(this.userServiceURL + '/GetUserDetails');
  };
  getRoles() {
    return this._http.get(this.userServiceURL + '/Roles');
  };
  addUser(user) {
    return this._http.post(this.userServiceURL + '/SaveUser', user);
  }
  updatePassword(creds) {
    debugger;
    return this._http.post(this.userServiceURL + '/UpdatePassword', creds);
  }
  updateUser(user) {
    return this._http.post(this.userServiceURL + '/UpdateUser', user);
  }
  deleteUser(id) {
    return this._http.delete(this.userServiceURL + '/DeleteUser', id)
  }

  sendOtp(userName) {
    debugger;
    return this._http.get(this.userServiceURL + '/SendOTP' + '?userName=' + userName)
  }
}

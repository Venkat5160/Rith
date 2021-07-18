import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import 'rxjs/Rx';
import { Observable } from 'rxjs/Rx';
import { throwError } from 'rxjs';
import { BaseURL } from '../../common/constants/baseURL.enum';
import {} from 'rxjs/Observable';

@Injectable({
  providedIn: 'root',
})
export class CommonService {
  constructor(private _http: HttpClient) {}

  login(user: any): any {
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    let options = { headers: headers };
    let url = BaseURL.url + '/api/account/login';
    return this._http
      .post(url, user, options)
      .map((response: any) => {
        if (response.Result) {
          var res = response.Result;
          var userData = UserData(res.Token);
          var expiryDate = new Date(0);
          // expiryDate.setSeconds(expiryDate.getSeconds() + userData.exp)
          expiryDate.setUTCSeconds(userData.exp);
          localStorage.setItem(
            'currentUser',
            JSON.stringify({
              UserName: res.UserName,
              Token: res.Token,
              UserId: res.UserId,
              RoleId: res.RoleId,
              SessionId: res.SessionId,
              ExpiryDate: expiryDate,
            })
          );
          return { StatusCode: true, Message: res.Message };
        } else {
          return { StatusCode: false, Message: response.Message };
        }
      })
      .catch((error) => {
        error.message = 'Error occurred. Please try again';
        return throwError(error);
      });
  }

  get(url: string): Observable<any> {
    const serviceUrl = BaseURL.url + url;
    return this.getResponse(this._http.get(serviceUrl));
  }

  getPay(url: string): Observable<any> {
    const serviceUrl = url;
    return this.getResponse(this._http.get(serviceUrl));
  }

  post(url: string, data: any): Observable<any> {
    const serviceUrl = BaseURL.url + url;
    return this.getResponse(this._http.post(serviceUrl, data));
  }

  put(url: string, data: any): Observable<any> {
    const serviceUrl = BaseURL.url + url;
    return this.getResponse(this._http.put(serviceUrl, data));
  }

  delete(url: string, data: any) {
    const serviceUrl = BaseURL.url + url + '/' + data;
    return this.getResponse(this._http.delete<any>(serviceUrl));
  }

  postUpload(url: any, data: any) {
    const serviceUrl = BaseURL.url + url;
    return this.getResponse(this._http.post(serviceUrl, data));
  }

  getCurrentUserToken() {
    var currentUser = localStorage.getItem('currentUser');
    if (currentUser != null && currentUser != undefined) {
      var token = JSON.parse(currentUser).Token;
      return token;
    }
    return null;
  }

  getResponse(observable: Observable<Object>): Observable<any> {
    return observable
      .map((res: any) => {
        if (res.statusCode == 200) return res;
        else {
          throw res;
        }
      })
      .catch((error) => {
        if (error.status != null && error.status != undefined) {
          if (error.status === 401) return throwError(error.Message);
          return throwError('Error occurred. Please try again');
        } else if (error.StatusCode != null && error.StatusCode != undefined)
          // If error comes from server
          return throwError(error.Message);
        // In other conditions
        else return throwError(error);
      });
  }
  userData() {
    const token = this.getCurrentUserToken();
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace('-', '+').replace('_', '/');
    var data = window.atob(base64);
    return JSON.parse(window.atob(base64));
  }
}

function UserData(token: any) {
  var base64Url = token.split('.')[1];
  var base64 = base64Url.replace('-', '+').replace('_', '/');
  return JSON.parse(window.atob(base64));
}

import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class SharedService {
    constructor() {

    }


    UserData(token: any) {
        var base64Url = token.split(".")[1];
        var base64 = base64Url.replace("-", "+").replace("_", "/");
        return JSON.parse(window.atob(base64));
    }

    GetCurrentUserIdentity() {
        return this.UserData(localStorage.getItem("zen-net-current-user"));
    }


}
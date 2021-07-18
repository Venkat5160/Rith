import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router) {
  }

  canActivate() {
    var currentUser = JSON.parse(localStorage.getItem('currentUser'));
    // let roleId = parseInt(currentUser.RoleId);
    if (localStorage.getItem('currentUser')) {
      return true;
    }


    // not logged in so redirect to login page
    this.router.navigate(['/login']);
    return false;
  }
}

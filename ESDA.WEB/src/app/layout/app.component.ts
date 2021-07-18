import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../services/authorize.service';
import {
  Router,
  NavigationStart,
  NavigationEnd,
  NavigationError,
  ActivatedRoute,
} from '@angular/router';
import { filter, map, switchMap, mergeMap } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'askanexpert-admin-web';
  isAuthorized: boolean;
  activeClassName: string = '';
  breadcrumb: string = '';
  userName: string = '';
  //Agent Call History
  private subscription: any;

  constructor(
    public authService: AuthorizeService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {

    this.isAuthorized = false;
  }

  logOut() {
    debugger;
    localStorage.removeItem('token');
    this.authService.isAuthorized = false;
    localStorage.removeItem("currentUser");
    localStorage.removeItem("user_name");
    this.authService.notifyAuthChange();
  }

  ngOnInit(): void {
    debugger;
    this.router.events
      .pipe(
        filter((event) => event instanceof NavigationEnd),
        map(() => this.activatedRoute),
        map((route) => {
          while (route.firstChild) route = route.firstChild;
          return route;
        }),
        filter((route) => route.outlet === 'primary'),
        mergeMap((route) => route.data)
      ).subscribe((data: any) => {
        this.activeClassName = data.menu;
        this.breadcrumb = data.breadcrumb;
      });

    if (localStorage.getItem('token') != null) {
      debugger;
      this.authService.isAuthorized = true;
      this.userName = localStorage.getItem('user_name');
      if (this.userName)
        // this.userName = this.userName
        //   .toString()
        //   .substring(0, this.userName.toString().indexOf('@'));
        this.authService.notifyAuthChange();
      this.authService.user_name = localStorage.getItem('user_name');
    }

    this.subscription = this.authService.observableAuth.subscribe((item) => {
      debugger;
      this.isAuthorized = item;
      if (item == false) {
        this.router.navigate(['/login']);
        return;
      }
    });
  }
}

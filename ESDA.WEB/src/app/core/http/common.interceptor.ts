import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AuthorizeService } from '../../services/authorize.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private router: Router, public authService: AuthorizeService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const currentUser = localStorage.getItem('token');
    if (currentUser) {
      request = request.clone({
        setHeaders: {
          'Content-Type': 'application/json',
          Authorization: 'bearer ' + currentUser,
        },
      });
    }

    return next.handle(request).pipe(
      tap(
        () => {},
        (err: any) => {
          if (err instanceof HttpErrorResponse) {
            if (err.status !== 401) {
              return;
            }
            localStorage.removeItem('token');
            localStorage.removeItem('user_name');
            this.authService.isAuthorized = false;
            this.authService.notifyAuthChange();
          }
        }
      )
    );
  }
}

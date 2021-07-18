import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './layout/app.component';
import { LoginComponent } from './pages/public/login/login.component';
import {
  NgbDropdownModule,
  NgbModule,
  NgbPaginationModule,
} from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from './pages/Shared/shared.module';
import { AuthInterceptor } from './core/http/common.interceptor';
import {
  NgxUiLoaderModule,
  SPINNER,
  POSITION,
  PB_DIRECTION,
  NgxUiLoaderRouterModule,
  NgxUiLoaderHttpModule,
} from 'ngx-ui-loader';

const ngxUiLoaderConfig: NgxUiLoaderModule = {
  bgsColor: '#OOACC1',
  fgsColor: '#00ACC1',
  fgsPosition: POSITION.centerCenter,
  fgsSize: 60,
  fgsType: SPINNER.squareJellyBox,
  pbColor: '#01afed',
  pbDirection: PB_DIRECTION.leftToRight,
  pbThickness: 5,
  textColor: '#FFFFFF',
  textPosition: POSITION.centerCenter,
  //logoUrl: `/assets/images/Progress-bar-v1.gif`,
};

@NgModule({
  declarations: [AppComponent, LoginComponent],
  imports: [
    HttpClientModule,
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    ToastrModule.forRoot(),
    NgxUiLoaderHttpModule.forRoot({
      showForeground: true,
    }),
    NgxUiLoaderModule.forRoot(ngxUiLoaderConfig),
    NgxUiLoaderRouterModule,
    NgbPaginationModule,
    NgbDropdownModule,
    NgbModule,
    RouterModule,
    SharedModule.forRoot(),
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}

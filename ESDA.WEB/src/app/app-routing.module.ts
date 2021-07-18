import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './layout/app.component';
import { LoginComponent } from './pages/public/login/login.component';
import { CommonModule } from '@angular/common';
//import { ForgotPasswordComponent } from './pages/public/forgotPassword/forgotPassword.component';

const routes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import('../app/pages/home/home.module').then(
        (m) => m.HomeModule
      ),
  },
  {
    path: 'dashboard',
    loadChildren: () =>
      import('../app/pages/home/home.module').then(
        (m) => m.HomeModule
      ),
  },
  {
    path: 'keywords',
    loadChildren: () =>
      import('../app/pages/keywords/keywords.module').then(
        (m) => m.KeywordsModule
      ),
  },
  {
    path: 'users',
    loadChildren: () =>
      import('../app/pages/users/users.module').then(
        (m) => m.UsersModule
      ),
  },
  {
    path: 'twitterDetails',
    loadChildren: () =>
      import('../app/pages/twitterdetails/twitterdetails.module').then(
        (m) => m.TwitterDetailsModule
      ),
  },
  { path: 'login', component: LoginComponent },
  // {
  //   path: "forgotpassword", component: ForgotPasswordComponent,
  // },
];

@NgModule({
  imports: [CommonModule, RouterModule.forRoot(routes)],
  exports: [RouterModule],
  declarations: [],
  providers: [],
})
export class AppRoutingModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { UserslistComponent } from './userslist/userslist.component';
import { AddUserComponent } from './addorupdate/adduser.component';

const productLink = "/users";

const userRoutes: Routes = [
  {
    path: '',
    component: UserslistComponent, data: { menu: 'user', breadcrumb: '<a>Users</a>' }
  },
  { path: 'users', component: UserslistComponent, data: { menu: 'user', breadcrumb: '<a>Users</a>' } },
  { path: 'adduser', component: AddUserComponent, data: { menu: 'user', breadcrumb: '<a href=' + productLink + '>Users</a> >> <a>Add</a>' } },
  {
    path: 'updateuser/:id', component: AddUserComponent, data: { menu: 'user', breadcrumb: '<a href=' + productLink + '>Users</a> >> <a>Update</a>' }
  },
];

@NgModule({
  imports: [RouterModule.forChild(userRoutes)],

})
export class UsersRoutingModule { }


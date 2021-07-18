import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersRoutingModule } from './users-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../Shared/shared.module';
 import { AddUserComponent } from './addorupdate/adduser.component';
import { UserslistComponent } from './userslist/userslist.component';
import { UserService } from './users.service';
@NgModule({
  imports: [
    ReactiveFormsModule,
    FormsModule,
    CommonModule,
    UsersRoutingModule, // Added,  
    SharedModule,
    RouterModule
    
  ],
 
  declarations: [UserslistComponent,AddUserComponent ],
  providers:[UserService],
  exports: [RouterModule],
})
export class UsersModule {}



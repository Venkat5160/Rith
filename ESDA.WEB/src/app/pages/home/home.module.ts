import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeRoutingModule } from './home-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../Shared/shared.module';
import { HomeComponent } from './home/home.component';
@NgModule({
  imports: [
    ReactiveFormsModule,
    FormsModule,
    CommonModule,
    HomeRoutingModule, // Added,  
    SharedModule,
    RouterModule
    
  ],
 
  declarations: [HomeComponent ],
  exports: [RouterModule],
})
export class HomeModule {}



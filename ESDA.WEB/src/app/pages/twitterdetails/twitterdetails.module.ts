import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TwitterRoutingModule } from './twitterdetails-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../Shared/shared.module';
import { TwitterDetailsComponent } from './addupdate/twitterdetails.component';
import { TwitterDetailsService } from './twitterdetails.service';
@NgModule({
  imports: [
    ReactiveFormsModule,
    FormsModule,
    CommonModule,
    TwitterRoutingModule, // Added,  
    SharedModule,
    RouterModule
    
  ],
 
  declarations: [TwitterDetailsComponent ],
  providers:[TwitterDetailsService],
  exports: [RouterModule],
})
export class TwitterDetailsModule {}



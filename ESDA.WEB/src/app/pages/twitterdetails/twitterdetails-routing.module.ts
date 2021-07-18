import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { TwitterDetailsComponent } from './addupdate/twitterdetails.component';


const TwitterDetailsRoutes: Routes = [
 
  { path: '', component: TwitterDetailsComponent, data: { menu: 'twitter', breadcrumb: '<a>Twitter</a>' } },
  
];

@NgModule({
  imports: [RouterModule.forChild(TwitterDetailsRoutes)],

})
export class TwitterRoutingModule { }


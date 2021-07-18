import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { KeywordslistComponent } from './keywordslist/keywordslist.component';
import { AddKeywordsComponent } from './addorupdate/addkeywords.component';

const productLink = "/keywords";

const keywordRoutes: Routes = [
  {
    path: '',
    component: KeywordslistComponent, data: { menu: 'keyword', breadcrumb: '<a>Keywords</a>' }
  },
  { path: 'keywords', component: KeywordslistComponent, data: { menu: 'keyword', breadcrumb: '<a>Keywords</a>' } },
  { path: 'addkeyword', component: AddKeywordsComponent, data: { menu: 'keyword', breadcrumb: '<a href=' + productLink + '>Keywords</a> >> <a>Add</a>' } },
  {
    path: 'updateproduct/:id', component: AddKeywordsComponent, data: { menu: 'keyword', breadcrumb: '<a href=' + productLink + '>Keywords</a> >> <a>Update</a>' }
  },
];

@NgModule({
  imports: [RouterModule.forChild(keywordRoutes)],

})
export class KeywordsRoutingModule { }


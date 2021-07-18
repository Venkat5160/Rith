import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { KeywordsRoutingModule } from './keywords-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../Shared/shared.module';
import { AddKeywordsComponent } from './addorupdate/addkeywords.component';
import { KeywordslistComponent } from './keywordslist/keywordslist.component';
import { KeywordService } from './keywords.service';
@NgModule({
  imports: [
    ReactiveFormsModule,
    FormsModule,
    CommonModule,
    KeywordsRoutingModule, // Added,  
    SharedModule,
    RouterModule
    
  ],
 
  declarations: [KeywordslistComponent,AddKeywordsComponent ],
  providers:[KeywordService],
  exports: [RouterModule],
})
export class KeywordsModule {}



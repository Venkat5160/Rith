import { NgModule, CUSTOM_ELEMENTS_SCHEMA, ModuleWithProviders } from '@angular/core';
import { CustomPagination } from './pagination/pagination';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";
import { DeleteModalPopupComponent } from './deletemodal/delete-modal';
import { NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  imports: [
    ReactiveFormsModule,
    FormsModule,
    CommonModule,
    NgbModule,
    NgbPaginationModule
  ],
  //schemas: [CUSTOM_ELEMENTS_SCHEMA],
  declarations: [
    CustomPagination,
    DeleteModalPopupComponent,
  ],
  exports: [
    CustomPagination,
  ],
  entryComponents: [DeleteModalPopupComponent, CustomPagination]
})

export class SharedModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: SharedModule
    }
  }
}

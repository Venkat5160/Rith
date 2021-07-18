import { Component, OnInit } from '@angular/core';
import { KeywordService } from '../keywords.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { PageDetails } from 'src/app/common/constants/baseURL.enum';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { RolesEnum } from 'src/app/common/constants/validationType.enum';

@Component({
  selector: 'app-keywordslist',
  templateUrl: './keywordslist.component.html',
  styleUrls: ['./keywordslist.component.scss'],
})
export class KeywordslistComponent implements OnInit {
  keywordsList: any;
  keywords = [];
  brandsList = [];
  searchKeyword = "";
  page: number = PageDetails.PageNumber;
  pageSize: number = PageDetails.PageSize;
  totalRecords: number;
  pageLoaded = false;
  SearchForm = new FormGroup({
    Keyword: new FormControl(''),
    AddedBy: new FormControl('')
  });
  roles: typeof RolesEnum;
  roleId: RolesEnum;
  role: any;
  constructor(
    private _keywordService: KeywordService,
    private _toastrService: ToastrService,
    private _route: Router

  ) { }

  ngOnInit(): void {
    debugger;
    const currentUser = JSON.parse(localStorage.getItem('currentUser'));
    this.roles = RolesEnum;
    if (currentUser.Role == 'Executive') this.roleId = RolesEnum.Executive
    if (currentUser.Role == 'Admin') this.roleId = RolesEnum.Admin
    this.role = currentUser.Role;
    //this.getBrands();
    this.searchKeywords();
  }
  searchKeywords(isAfterDelete: boolean = false) {
    debugger;
    let filterObj =
    {
      Keyword: this.SearchForm.value.Keyword.trim(),
      AddedBy: this.SearchForm.value.AddedBy.trim(),
      PageSize: this.pageSize,
      PageNumber: this.page
    }
    this._keywordService.getKeywordsList(filterObj).subscribe(
      (data) => {
        this.pageLoaded = true;
        debugger;
        if (data.successful == true) {
          this.keywordsList = data.result.sdaekeywords;
          this.totalRecords = data.result.totalRecords;

        }
        else {
          this.keywordsList = [];

        }

      },
      (error) => {
        // this._toastrService.error(error);
        this.keywordsList = [];
        this.totalRecords = 0;
        if (isAfterDelete == true) {
          this.SearchForm.reset({ Keyword: '', AddedBy: '' });
        }
      }
    );
  }


  deleteKeyword(PMId) {
    if (confirm('Are you sure to delete this record ?')) {
      this._keywordService.deleteKeyword(PMId)
        .subscribe(res => {
          var result = res;
          this._toastrService.success(result.message);
          this.searchKeywords(true);
        },
          error => {
            this._toastrService.error(error.message);

          });

    }
  }
  navigateToAdd() {
    this._route.navigate(['/keywords/addkeyword']);
  }
  navigateToUpdate(productId) {
    this._route.navigate(['/keywords/updateproduct', productId]);
  }

  setPage(page: number) {
    this.page = page;
    this.searchKeywords();

  }
  setPageSize(pageSize) {
    this.pageSize = parseInt(pageSize);
    this.setPage(this.page);
  }
  reset() {
    this.resetPageDetails()
    this.SearchForm.reset({ Keyword: '', AddedBy: '' });
    this.searchKeywords();
  }
  resetPageDetails() {
    this.page = PageDetails.PageNumber;
    this.pageSize = PageDetails.PageSize;
  }

}

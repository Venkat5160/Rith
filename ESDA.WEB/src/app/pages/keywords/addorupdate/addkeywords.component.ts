import { Component, OnInit } from '@angular/core';
import { KeywordService } from '../keywords.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { PageDetails } from 'src/app/common/constants/baseURL.enum';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { RolesEnum } from 'src/app/common/constants/validationType.enum';

@Component({
  selector: 'app-addkeyword',
  templateUrl: './addkeywords.component.html',
  styleUrls: ['./addkeywords.component.scss'],
})
export class AddKeywordsComponent implements OnInit {
  submitted = false;
  isUpdate = false;
  orignalKeywordvalue:string
  SdaekeywordId: number;
  addKeywordForm = new FormGroup({
    SdaekeywordId: new FormControl('0'),
    Keyword: new FormControl('', Validators.required),
  });
  roles: typeof RolesEnum;
  roleId: RolesEnum;
  role: any;

  constructor(
    private _keywordService: KeywordService,
    private _toastrService: ToastrService,
    private _router: Router,
    private _route: ActivatedRoute

  ) { }

  ngOnInit(): void {
    debugger;
    const currentUser = JSON.parse(localStorage.getItem('currentUser'));
    this.roles = RolesEnum;
    if (currentUser.Role == 'Executive') this.roleId = RolesEnum.Executive
    if (currentUser.Role == 'Admin') this.roleId = RolesEnum.Admin
    this.role = currentUser.Role;
    this.SdaekeywordId = parseInt(this._route.snapshot.params.id);
    if (this.SdaekeywordId) {
      this.isUpdate = true;
      this.getKeywordDetails(this.SdaekeywordId);
    }
  }

  get f() {
    return this.addKeywordForm.controls;
  }

  saveDetails() {
    debugger;
    this.submitted = true;
    if (this.addKeywordForm.invalid) {
      return;
    }
    if(this.orignalKeywordvalue === this.addKeywordForm.value.Keyword)
    {
      this._toastrService.success("Data updated successfully");
      this.backtoListScreen();
      return;
    }
    console.log('form', this.addKeywordForm.value);
    let obj = {
      SdaekeywordId: this.addKeywordForm.value.SdaekeywordId,
      Keyword: this.addKeywordForm.value.Keyword.trim()
    };
    this._keywordService.addKeyword(obj).subscribe((response) => {
      if (response) {        
        var result = response;
        this._toastrService.success(result.message);
        this.backtoListScreen();
      }
    },
      (error) => {
        this._toastrService.error(error.message);
      }
    );
  }

  getKeywordDetails(keywordId) {
    this._keywordService.getKeywordsById(keywordId).subscribe((response) => {
      if (response) {
        debugger;
        this.addKeywordForm.setValue({
          SdaekeywordId: response.result.sdaekeywordId,
          Keyword: response.result.keyword
        });
        this.orignalKeywordvalue =response.result.keyword;
      }
      (error) => {
        this._toastrService.error(error.message);
      };
    });
  }

  backtoListScreen() {
    this._router.navigate(['/keywords']);
  }




}

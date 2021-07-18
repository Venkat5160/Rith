import { Component, OnInit } from '@angular/core';
import { TwitterDetailsService } from '../twitterdetails.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { PageDetails } from 'src/app/common/constants/baseURL.enum';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-twitterdetails',
  templateUrl: './twitterdetails.component.html',
  styleUrls: ['./twitterdetails.component.scss'],
})
export class TwitterDetailsComponent implements OnInit {
  twitterDetails: any;
  twitterId:number
  users = [];
  brandsList = [];
  searchUser = "";
  page: number = PageDetails.PageNumber;
  pageSize: number = PageDetails.PageSize;
  totalRecords: number;
  pageLoaded = false;
  submitted = false;
  isEditMode : boolean=false;

  TwitterDetailsForm = new FormGroup({
    TwitterId:new FormControl('0'),
    Username: new FormControl('',Validators.required),
    Password: new FormControl('',Validators.required) ,
    Url: new FormControl('',Validators.required),
    ClientId: new FormControl('',Validators.required),
    SecretKey: new FormControl('',Validators.required)    
    });

  constructor(
    private _twitterService: TwitterDetailsService,
    private _toastrService: ToastrService,
    private _route: Router    
  ) { }

  ngOnInit(): void {    
    this.getTwitterDetails();
  }
  get f() {
    return this.TwitterDetailsForm.controls;
  }

  updateEditMode(value)
  {
    this.isEditMode=value;
    if(value==false)
    {
    this.setFormValues();
    this.submitted=false;
    this._toastrService.info("Edit mode disabled");
  }
  else{
    this._toastrService.info("Edit mode enabled");
  }
  }
  getTwitterDetails() {
    this._twitterService.getTwitterDetails().subscribe(
      (data) => {
        this.twitterDetails = data.result.twitterDetails;  
        this.isEditMode=false;
        this.setFormValues();
      },
      (error) => {
        this._toastrService.error(error);
      }
    );
  }

  setFormValues()
  {
    this.TwitterDetailsForm.setValue({
      TwitterId:this.twitterDetails.twitterId,
  Username: this.twitterDetails.username,
  Password: this.twitterDetails.password,
  Url: this.twitterDetails.url,
  ClientId: this.twitterDetails.clientId,
  SecretKey: this.twitterDetails.secretKey, });
  }

  saveDetails() {
    this.submitted=true;
    if (this.TwitterDetailsForm.invalid) {
      return;
    }
    console.log('form', this.TwitterDetailsForm.value);
    let obj = {
      TwitterId:this.TwitterDetailsForm.value.TwitterId,
      Username: this.TwitterDetailsForm.value.Username,
      Password: this.TwitterDetailsForm.value.Password,
      Url: this.TwitterDetailsForm.value.Url,
      ClientId: this.TwitterDetailsForm.value.ClientId,
      SecretKey: this.TwitterDetailsForm.value.SecretKey,
    };
    this._twitterService.saveTwitterDetails(obj).subscribe((response) => {
      if (response) {
        var result = response;
        this._toastrService.success(result.message);    
        this.getTwitterDetails();    
      }
      (error) => {
        this._toastrService.error(error.message);
      };
    });
  }


  deleteTwitterDetails(tId) {
    if (confirm('Are you sure to delete this record ?')) {
      this._twitterService.deleteTwitterDetails(tId)
        .subscribe(res => {
          var result = res;
          this._toastrService.success(result.message);
          this.getTwitterDetails();
        },
          error => {
            this._toastrService.error(error.message);

          });

    }
  } 
 
  setPage(page: number) {
    this.page = page;
    this.getTwitterDetails();

  }
  setPageSize(pageSize) {
    this.pageSize = parseInt(pageSize);
    this.setPage(this.page);
  } 


}

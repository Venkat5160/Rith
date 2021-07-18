import { Component, OnInit } from '@angular/core';
import { UserService } from '../users.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { PageDetails } from 'src/app/common/constants/baseURL.enum';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { RolesEnum } from 'src/app/common/constants/validationType.enum';

@Component({
  selector: 'app-userslist',
  templateUrl: './userslist.component.html',
  styleUrls: ['./userslist.component.scss'],
})
export class UserslistComponent implements OnInit {
  usersList: any;
  users = [];
  brandsList = [];
  searchUser = "";
  page: number = PageDetails.PageNumber;
  pageSize: number = PageDetails.PageSize;
  totalRecords: number;
  pageLoaded = false;
  SearchForm = new FormGroup({
    Name: new FormControl(''),
    Email: new FormControl('')
  });

  roleId: any;
  roles: any;
  userId: any;
  constructor(
    private _userService: UserService,
    private _toastrService: ToastrService,
    private _route: Router
  ) { }

  ngOnInit(): void {
    debugger;
    const currentUser = JSON.parse(localStorage.getItem('currentUser'));
    this.roles = RolesEnum;
    this.userId = currentUser.UserId;
    if (currentUser.Role == 'Executive') {
      this.roleId = RolesEnum.Executive
    }
    if (currentUser.Role == 'Admin') {
      this.roleId = RolesEnum.Admin
    }
    this.searchUsers();

  }
  searchUsers(isAfterDelete: boolean = false) {
    let filterObj =
    {
      Name: this.SearchForm.value.Name.trim(),
      Email: this.SearchForm.value.Email.trim(),
      PageSize: this.pageSize,
      PageNumber: this.page,


    }
    this._userService.getUsersList(filterObj).subscribe(
      (data) => {
        debugger;
        this.usersList = data.result.lstUsers;
        // if (this.roleId == RolesEnum.Executive) {
        //   this.usersList = this.usersList.filter(x => x.userId == this.userId);
        // }
        this.totalRecords = data.result.totalRecords;
        this.pageLoaded = true;

        if (this.totalRecords == 0 && isAfterDelete == true) {
          this.SearchForm.reset({ Name: '', Email: '' });
        }
      },
      (error) => {
        this._toastrService.error(error);

      }
    );
  }

  getUserDetails() {
    this._userService.getUsersById(this.userId).subscribe(
      (data) => {
        debugger;
        this.usersList = data.result.lstUsers;
        this.totalRecords = data.result.totalRecords;
        this.pageLoaded = true;
      }, (error) => {
        this._toastrService.error(error);
      })

  }
  deleteUser(PMId) {
    if (confirm('Are you sure to delete this record ?')) {
      this._userService.deleteUser(PMId)
        .subscribe(res => {
          var result = res;
          this._toastrService.success(result.message);
          this.searchUsers(true);
        },
          error => {
            this._toastrService.error(error.message);

          });

    }
  }
  navigateToAdd() {
    this._route.navigate(['/users/adduser']);
  }
  navigateToUpdate(userId) {
    this._route.navigate(['/users/updateuser', userId]);
  }

  setPage(page: number) {
    this.page = page;
    this.searchUsers();

  }
  setPageSize(pageSize) {
    this.pageSize = parseInt(pageSize);
    this.setPage(this.page);
  }

  // filterBrands(){    
  //   let obj ={
  //     Name: this.searchUser, PageSize:this.pageSize,  PageNumber:this.page   }
  //   this._userService.UsersList(obj).subscribe(response => {
  //     if (response) {
  //       this.users = response.result.users;
  //       this.totalRecords = response.result.totalRecords;
  //     }
  //   })
  // }
  // reset() {
  //   this.SearchForm.reset({ BrandId: '', Name: '', Mpn: '' });
  //   this.searchUsers();
  // }
  reset() {
    this.SearchForm.reset({ Name: '', Email: '' });
    this.searchUsers();
  }

}

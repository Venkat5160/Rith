import { Component, OnInit } from '@angular/core';
import { UserService } from '../users.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { PageDetails } from 'src/app/common/constants/baseURL.enum';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { RolesEnum } from 'src/app/common/constants/validationType.enum';
import { ValidationService } from 'src/app/common/services/validation.service';
import { MustMatch } from 'src/app/common/services/comparevalidation.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthorizeService } from 'src/app/services/authorize.service';

@Component({
  selector: 'app-adduser',
  templateUrl: './adduser.component.html',
  styleUrls: ['./adduser.component.scss'],
})
export class AddUserComponent implements OnInit {
  submitted = false;
  isUpdateMode = false;
  userId: string;
  roleId: any;
  role: string;
  roles: any;
  addUserForm: FormGroup;
  RoleList: any = ['Admin', 'Excutive']
  createNewPwdForm: any;
  updatePwdModelRef: any;
  lstRoles: any;

  constructor(
    private _userService: UserService,
    private _toastrService: ToastrService,
    private _router: Router,
    private _route: ActivatedRoute,
    private validationSrvc: ValidationService,
    private formBuilder: FormBuilder,
    private modalService: NgbModal,
    public authService: AuthorizeService,
  ) { }

  ngOnInit(): void {
    debugger;
    const currentUser = JSON.parse(localStorage.getItem('currentUser'));
    this.roles = RolesEnum;
    this.userId = currentUser.UserId;
    if (currentUser.Role == 'Executive') this.roleId = RolesEnum.Executive
    if (currentUser.Role == 'Admin') this.roleId = RolesEnum.Admin
    this.getRoles();
    this.role = currentUser.Role;
    this.addUserForm = this.formBuilder.group({
      UserId: new FormControl(''),
      FirstName: new FormControl('', Validators.required),
      LastName: new FormControl('', Validators.required),
      Email: new FormControl('', [Validators.required, Validators.email]),
      RoleId: new FormControl('', Validators.required),
      Phone: new FormControl('',),
      Password: new FormControl('', [Validators.required, Validators.pattern("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$")]),
      ConfirmPassword: new FormControl('', Validators.required),

    },
      { validator: this.validationSrvc.PasswordMatch }
    );
    this.userId = this._route.snapshot.params.id;
    if (this.userId) {
      this.createNewPasswordForm();
      this.isUpdateMode = true;
      this.addUserForm.get("RoleId").disable();
      this.addUserForm.get("Password").clearValidators();
      this.addUserForm.get("ConfirmPassword").clearValidators();
      this.addUserForm.updateValueAndValidity();
      this.getUserDetails(this.userId);
    }
  }

  get f() {
    return this.addUserForm.controls;
  }
  get g() {
    return this.createNewPwdForm.controls;
  }
  createNewPasswordForm() {
    this.createNewPwdForm = this.formBuilder.group(
      {
        NewPassword: ["", Validators.compose([Validators.required, Validators.pattern("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$")])],
        ConfirmPassword: ["", Validators.compose([Validators.required])]
      },
      {
        validator: MustMatch("NewPassword", "ConfirmPassword")
      }
    );
  }
  UpdatePwdModel(updatePwd) {
    this.updatePwdModelRef = this.modalService.open(updatePwd);
  }
  updatePassword() {
    debugger;
    this.submitted = true;
    if (this.createNewPwdForm.invalid) {
      return;
    } else {
      let obj = {
        userId: this.userId,
        password: this.createNewPwdForm.value.NewPassword
      };
      this._userService.updatePassword(obj).subscribe((response) => {
        debugger;
        if (response.statusCode == '200') {
          this._toastrService.success(response.message);
          this.updatePwdModelRef.dismiss();
          localStorage.removeItem('token');
          this.authService.isAuthorized = false;
          localStorage.removeItem("currentUser");
          localStorage.removeItem("user_name");
          this.authService.notifyAuthChange();
          this._router.navigate(["/login"]);
        }
        (error) => {
          this._toastrService.error(error.message);
        };
      });

    }
  }

  getRoles() {
    debugger;
    this._userService.getRoles().subscribe((data) => {
      if (data) {
        debugger;
        this.lstRoles = data.result.roles;
      }
      (error) => {
        this._toastrService.error(error.message);
      };
    })
  }
  saveDetails() {
    debugger;
    this.submitted = true;
    if (this.addUserForm.invalid) {
      return;
    }
    console.log('form', this.addUserForm.value);
    let obj = {
      UserId: this.addUserForm.value.UserId,
      FirstName: this.addUserForm.value.FirstName,
      LastName: this.addUserForm.value.LastName,
      Email: this.addUserForm.value.Email,
      Contact: this.addUserForm.value.Phone,
      // Role: this.addUserForm.value.Role,
      Password: this.addUserForm.value.Password,
      RoleId: this.addUserForm.value.RoleId
    };
    if (this.addUserForm.value.UserId == null || this.addUserForm.value.UserId == '') {
      this._userService.addUser(obj).subscribe((response) => {
        if (response) {
          debugger;
          var result = response;
          this._toastrService.success(result.message);
          this.backtoListScreen();
        }
        (error) => {
          this._toastrService.error(error.message);
        };
      });
    } else {
      this._userService.updateUser(obj).subscribe((response) => {
        if (response) {
          var result = response;
          this._toastrService.success(result.message);
          this.backtoListScreen();
        }
        (error) => {
          this._toastrService.error(error.message);
        };
      });
    }
  }

  getUserDetails(userId) {
    this._userService.getUsersById(userId).subscribe((response) => {
      if (response) {
        debugger;
        this.addUserForm.setValue({
          UserId: response.result.userId,
          FirstName: response.result.firstName,
          LastName: response.result.lastName,
          Email: response.result.email,
          Phone: response.result.contact,
          RoleId: response.result.roleId,
          Password: "",
          ConfirmPassword: ""
        });
      }
      (error) => {
        this._toastrService.error(error.message);
      };
    });
  }

  backtoListScreen() {
    this._router.navigate(['/users']);
  }


  // openAddModal(content) {
  //   this.createUpdatePwdForm();
  //   this.adminSrvc.sendOtp(this.addUserForm.controls["UserId"].value).subscribe(
  //     data => {
  //       this.otp = data.Result.OTP;
  //     }
  //   )
  //   this.updateModalRef = this.modalService.open(content);
  // }


}

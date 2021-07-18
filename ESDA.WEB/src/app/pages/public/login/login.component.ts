import { Component, OnInit, ElementRef, Renderer2, Inject, OnDestroy } from '@angular/core';
import { AuthorizeService } from 'src/app/services/authorize.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { PublicService } from '../public.services';
import { ActivatedRoute, Router } from '@angular/router';
import { DOCUMENT } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { constants } from 'buffer';
import { parse } from 'path';
import { AnyARecord } from 'dns';
import { UserService } from '../../users/users.service';
import { ModalDismissReasons, NgbModal, NgbModalConfig } from '@ng-bootstrap/ng-bootstrap';
import { MustMatch } from 'src/app/common/services/comparevalidation.service';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  // providers: [NgbModalConfig, NgbModal]
})
export class LoginComponent implements OnInit, OnDestroy {
  loginForm: FormGroup;
  updatePwdForm: FormGroup;
  forgotPwdModalRef: any;
  validateOTPModalRef: any;
  closeResult: string;
  otp: any;
  generatedOTP: any;
  userName: any;
  updatePwdModelRef: any;
  isSubmitted: boolean;
  userId: any;
  saveduser: any;
  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthorizeService,
    private publicService: PublicService,
    private renderer: Renderer2,
    private route: ActivatedRoute,
    private modalService: NgbModal,
    private router: Router,
    private _toastrService: ToastrService,
    private _userService: UserService,
    //config: NgbModalConfig,
    @Inject(DOCUMENT) private document: Document
  ) {
    // config.backdrop = 'static';
    //config.keyboard = false;
  }

  ngOnInit(): void {

    this.createLoginForm();
    this.saveduser = JSON.parse(localStorage.getItem("RememberMe"))
    if (this.saveduser) {
      this.loginForm.setValue({
        userName: this.saveduser.userName,
        password: this.saveduser.password,
        rememberMe: this.saveduser.rememberMe
      });
    }

    // this.createLoginForm();
    // this.createLoginForm();
    this.createUpdatePwdForm();
    this.document.body.classList.add('login-body');
  }

  ngOnDestroy() {
    this.document.body.classList.remove('login-body');
  }
  createLoginForm() {
    this.loginForm = new FormGroup({
      userName: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
      rememberMe: new FormControl(''),

    });
  }

  login() {
    if (this.loginForm.valid) {
      let formDetails = this.loginForm.value;

      this.publicService
        .loginUser(formDetails)
        .then((data) => {
          if (formDetails.rememberMe) {
            localStorage.setItem("RememberMe", JSON.stringify(
              formDetails
            ))
          }
          else {
            localStorage.removeItem("RememberMe")
          }
          let token = data.data.data.access_token;
          localStorage.setItem('token', token);
          if (token != null && token != "" && token != undefined) {
            this._userService.getUserDetails().subscribe((response) => {
              debugger;
              if (response.result) {
                debugger;
                localStorage.setItem('currentUser', JSON.stringify({
                  UserName: response.result.userName,
                  FirstName: response.result.firstName,
                  LastName: response.result.lastName,
                  Email: response.result.Email,
                  Contact: response.result.contact,
                  Role: response.result.role,
                  RoleId: response.result.roleId,
                  UserId: response.result.userId
                }));
                localStorage.setItem('user_name', response.result.firstName);
                this.authService.user_name = localStorage.getItem('user_name');
                this.authService.isAuthorized = true;
                this.authService.notifyAuthChange();
                this.router.navigate(['/dashboard']);
              }
              (error) => {
                this._toastrService.error(error.message);
              };
            });
          }
          else {
            localStorage.setItem('user_name', "");
          }

        })
        .catch((error) => {
          var errMsg = '';
          if (error.response != null) {
            errMsg = error.response.data.error_description;
            if (errMsg == null) errMsg = 'Error occured. Please try again';
          } else errMsg = 'Error occured. Please try again';
          this._toastrService.error(errMsg);
        });
    } else {
      this._toastrService.error('Enter valid details');
    }
  }
  createUpdatePwdForm() {
    this.updatePwdForm = this.formBuilder.group(
      {
        password: ["", Validators.compose([Validators.required, Validators.pattern("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$")])],
        confompassword: ["", Validators.compose([Validators.required])],
      },
      {
        validator: MustMatch("password", "confompassword")
      });
  }
  get f() {
    return this.updatePwdForm.controls;
  }
  openAddModal(content) {
    const dialogConfig = new NgbModalConfig();
    dialogConfig.backdrop = 'static';
    dialogConfig.keyboard = false;
    this.forgotPwdModalRef = this.modalService.open(content);
  }

  onSendOtp(validateOtp) {
    debugger;
    if (this.userName) {
      this._userService.sendOtp(this.userName).subscribe(
        data => {
          debugger;
          this.generatedOTP = data.result.otp;
          this.userId = data.result.userId;
          if (this.forgotPwdModalRef) this.forgotPwdModalRef.dismiss();
          this.validateOTPModalRef = this.modalService.open(validateOtp);
          this._toastrService.success(data.message);
        });
    } else this._toastrService.error("Please enter Email");
    // this.forgotPwdModalRef.dismiss();
    // this.validateOTPModalRef = this.modalService.open(validateOtp);
  }

  onValidateOtp(updatePwd) {
    debugger;
    if (this.otp != null && this.otp != undefined) {
      if (this.generatedOTP == this.otp) {
        if (this.validateOTPModalRef) this.validateOTPModalRef.dismiss();
        this.updatePwdModelRef = this.modalService.open(updatePwd);
      } else {
        this._toastrService.error("OTP does not match");
      }
    } else this._toastrService.error("Please enter valid OTP");
  }

  onPwdUpdateSubmit() {
    this.isSubmitted = true;
    debugger;
    if (this.updatePwdForm.valid) {
      let obj = {
        userId: this.userId,
        password: this.updatePwdForm.value.password
      };
      this._userService.updatePassword(obj).subscribe((response) => {
        debugger;
        if (response.statusCode == '200') {
          this._toastrService.success(response.message);
          this.updatePwdModelRef.dismiss();
          this.router.navigate(["/login"]);

        }
        (error) => {
          this._toastrService.error(error.message);
        };
      });
    } else {
      this._toastrService.error('Enter valid details');
    }
  }
}

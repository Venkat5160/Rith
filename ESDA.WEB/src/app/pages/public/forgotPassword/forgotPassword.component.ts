// import { Component, ViewChild, ElementRef, Renderer2 } from "@angular/core";
// import { FormBuilder, FormGroup, Validators } from "@angular/forms";
// import { CommonService } from "../../../core/http/common.service";
// import { ToastrService } from "ngx-toastr";
// import { Router } from "@angular/router";
// import { MustMatch } from "src/app/common/services/comparevalidation.service";

// @Component({
//     selector: "forgotpassword",
//     templateUrl: "./forgotpassword.html",
//     styleUrls: ["./forgotpassword.scss"]
// })
// export class ForgotPasswordComponent {
//     forgotPwdForm: FormGroup;
//     createNewPwdForm: FormGroup;
//     enteredOTP: string;
//     generatedOTP: string;
//     screenName: string = "Request OTP";
//     requestedUserId: number;
//     @ViewChild("requestotp", { static: false }) requestOTP: ElementRef;
//     @ViewChild("enterotp", { static: false }) enterOTP: ElementRef;
//     @ViewChild("enternewpwd", { static: false }) enterNewPwd: ElementRef;
//     submitted: boolean;

//     get f() {
//         return this.createNewPwdForm.controls;
//     }

//     constructor(
//         private _formBuilder: FormBuilder,
//         private _renderer: Renderer2,
//         private _http: CommonService,
//         private _toastr: ToastrService,
//         private _router: Router
//     ) { }

//     ngOnInit() {
//         this.createForgotPasswordForm();
//         this.createNewPasswordForm();
//     }

//     createForgotPasswordForm() {
//         this.forgotPwdForm = this._formBuilder.group({
//             Email: [""],
//             Mobile: [""]
//         });
//     }

//     createNewPasswordForm() {
//         this.createNewPwdForm = this._formBuilder.group(
//             {
//                 NewPassword: [
//                     "",
//                     Validators.compose([
//                         Validators.required,
//                         Validators.minLength(6),
//                         Validators.pattern(/^\S*$/)
//                     ])
//                 ],
//                 ConfirmPassword: ["", Validators.compose([Validators.required])]
//             },
//             {
//                 validator: MustMatch("NewPassword", "ConfirmPassword")
//             }
//         );
//     }

//     submitRequestOTPForm() {
//         if (this.forgotPwdForm.valid) {
//             const forgotDto = this.forgotPwdForm.value;
//             if (this.validateFormValues(forgotDto)) {
//                 this._http.post("/api/Account/ForgotPassword", forgotDto).subscribe(
//                     data => {
//                         this.generatedOTP = data.Result.OTP;
//                         this.requestedUserId = data.Result.UserId;
//                         this._toastr.success(data.Message);
//                         this.showValidateOTP();
//                     },
//                     error => {
//                         this._toastr.error(error);
//                     }
//                 );
//             } else {
//                 this._toastr.error("Provide either Email or Mobile to get OTP");
//             }
//         }
//     }

//     validateOTP() {
//         if (this.enteredOTP != null && this.enteredOTP != undefined) {
//             if (this.enteredOTP.trim() == this.generatedOTP.trim()) {
//                 this.showEnterNewPWD();
//             } else {
//                 this._toastr.error("OTP does not match");
//             }
//         } else this._toastr.error("Please enter valid OTP");
//     }

//     updatePassword() {
//         this.submitted = true;
//         if (this.createNewPwdForm.valid) {
//             const updatePwdDt = this.createNewPwdForm.value;
//             updatePwdDt.UserId = this.requestedUserId;
//             this._http.post("/api/Account/UpdatePassword", updatePwdDt).subscribe(
//                 data => {
//                     this._toastr.success(data.Message);
//                     this._router.navigate(["/login"]);
//                 },
//                 error => {
//                     this._toastr.error(error);
//                 }
//             );
//         }
//     }

//     backToRequestOTP() {
//         this.showRequestOTP();
//         this.enteredOTP = "";
//     }

//     showRequestOTP() {
//         this.screenName = "Request OTP";
//         this._renderer.setStyle(this.requestOTP.nativeElement, "display", "block");
//         this._renderer.setStyle(this.enterOTP.nativeElement, "display", "none");
//         this._renderer.setStyle(this.enterNewPwd.nativeElement, "display", "none");
//     }

//     showValidateOTP() {
//         this.screenName = "Validate OTP";
//         this._renderer.setStyle(this.requestOTP.nativeElement, "display", "none");
//         this._renderer.setStyle(this.enterOTP.nativeElement, "display", "block");
//     }

//     showEnterNewPWD() {
//         this.screenName = "Create new Password";
//         this._renderer.setStyle(this.enterOTP.nativeElement, "display", "none");
//         this._renderer.setStyle(this.enterNewPwd.nativeElement, "display", "block");
//     }

//     validateFormValues(forgotDto: any) {
//         if (
//             (forgotDto.Email != null &&
//                 forgotDto.Email != undefined &&
//                 forgotDto.Email.trim() != "") ||
//             (forgotDto.Mobile != null &&
//                 forgotDto.Mobile != undefined &&
//                 forgotDto.Mobile.trim() != "")
//         )
//             return true;
//         return false;
//     }
// }

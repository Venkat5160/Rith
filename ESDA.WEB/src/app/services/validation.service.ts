import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import 'rxjs/add/operator/map';
import { Injectable } from '@angular/core';
declare var $: any;

@Injectable({
  providedIn: 'root'
})

export class ValidationService {
  emailPattern: any = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,3})$/;
  // passwordPattern = '^(?=.*?[a-zA-Z])(?=.*?[@#%$])[a-zA-Z@#%$0-9]{6,15}$';

  passwordPattern = new RegExp(
    '^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])(?=.{8,})'
  );
  // passwordPattern = '^(?=(.*[a-z]){1,})(?=(.*[\d]){1,})(?=(.*[\W]){1,})(?!.*\s).{6,15}$';
  mobileNumberPattern = '^[0-9+_@#%$]+$';
  numberPattern: string = '^[+0-9.]+';
  PasswordMatch(group: FormGroup) {
    let valid = true;
    if (group.controls["confirmPassword"].value === group.controls["password"].value)
      valid = true;
    else {
      group.controls['confirmPassword'].setErrors({ mustMatch: true });
      valid = false;
    }
    if (valid) {
      return null;
    }
    return {
      PasswordMatch: true
    };
  }

  ValidateDropdownList(control: FormControl) {
    let valid = true;
    let selectedValue = parseInt(control.value);
    if (selectedValue !== 0)
      valid = true;
    else
      valid = false;
    if (valid) {
      return null;
    }
    return {
      ValidateDropdownList: true
    };
  }
  ValidateStringValDropdownList(control: FormControl) {
    let valid = true;
    if (control.value !== '' && control.value !== undefined && control.value !== '0')
      valid = true;
    else
      valid = false;
    if (valid) {
      return null;
    }
    return {
      ValidateStringValDropdownList: true
    };
  }
}

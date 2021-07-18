import { Directive, ElementRef, Renderer2, Attribute } from "@angular/core";
import {
  NG_VALIDATORS,
  Validator,
  AbstractControl,
  Validators
} from "@angular/forms";
import { ValidationType } from "../constants/validationType.enum";

@Directive({
  selector:
    "[appCustomValidator][formControlName],[appCustomValidator][formControl],[appCustomValidator][ngModel]",
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: CustomValidatorDirective,
      multi: true
    }
  ]
})
export class CustomValidatorDirective implements Validator {
  isInvalid = false;
  constructor(
    @Attribute("appCustomValidator") public _appCustomValidator: string,
    private _el: ElementRef,
    private _render: Renderer2
  ) { }
  _element = this._render.createElement("span");
  validate(control: AbstractControl): { [key: string]: any } {
    this.isInvalid = false;
    this._element.innerText = null;
    let text = this._render.createText("");
    if (control.value) {
      const type = this._el.nativeElement.attributes.type;
      const custommax = this._el.nativeElement.attributes.custommax;
      const custommin = this._el.nativeElement.attributes.custommin;
      if (
        parseInt(control.value.length, 10) >
        parseInt(custommax && custommax.value, 10)
      ) {
        this.isInvalid = true;
        text = this._render.createText(
          `The ${this._el.nativeElement.placeholder.replace(
            "*",
            ""
          )} length should not be grater than ${custommax.value}`
        );
      }

      if (
        parseInt(control.value.length, 10) <
        parseInt(custommin && custommin.value, 10)
      ) {
        this.isInvalid = true;
        text = this._render.createText(
          `The ${this._el.nativeElement.placeholder.replace(
            "*",
            ""
          )} length should not be less than ${custommin.value}`
        );
      }
      if (type && type.value === "email") {
        const emailVal = control.value;
        if (!emailVal.match(ValidationType.EMAIL_PATTERN)) {
          this.isInvalid = true;
          text = this._render.createText(`Please enter valid Email`);
        }
      }
      if (type && type.value === "mobile") {
        const mobileVal = control.value;
        if (!mobileVal.match(ValidationType.MOBILE_NUMBER_PATTERN)) {
          this.isInvalid = true;
          text = this._render.createText(`Please enter valid Mobile`);
        }
      }

      this._render.addClass(this._element, "error-dir-message");
      const parentEl = this._el.nativeElement.parentNode
      .parentNode.parentNode.parentNode.parentNode;
      this._render.removeChild(parentEl, this._element);
      if (this.isInvalid == true) {
        this._render.appendChild(this._element, text);
        const refChild = this._el.nativeElement.parentNode.parentNode.parentNode.parentNode;
        this._render.insertBefore(parentEl, this._element, refChild);
      }
    } else {
      this._render.removeClass(this._element, "error-dir-message");
    }

    if (this.isInvalid === true) {
      return { appCustomValidator: true };
    } else {
      return null;
    }
  }
}

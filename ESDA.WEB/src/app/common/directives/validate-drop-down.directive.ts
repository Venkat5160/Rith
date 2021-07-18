import { Directive, Attribute, ElementRef, Renderer2 } from "@angular/core";
import { NG_VALIDATORS, Validator, AbstractControl } from "@angular/forms";

@Directive({
  selector:
    "[appValidateDropDown][ngModel],[appValidateDropDown][formControlName]",
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: ValidateDropDownDirective,
      multi: true
    }
  ]
})
export class ValidateDropDownDirective implements Validator {
  isInvalid = false;
  constructor(
    @Attribute("appValidateDropDown") public _appDropdownValidator: string,
    private _el: ElementRef,
    private _render: Renderer2
  ) {}
  _element = this._render.createElement("span");
  validate(control: AbstractControl): { [key: string]: any } {
    this.isInvalid = false;
    this._element.innerText = null;
    let text = this._render.createText("");
    if (control.value) {
      if (parseInt(control.value.length, 10) === 0) {
        this.isInvalid = true;
        text = this._render.createText(
          `  Please select ${this._el.nativeElement.placeholder.replace(
            "*",
            ""
          )}`
        );
      }
      this._render.addClass(this._element, "error-dir-message");
      const parentEl = this._el.nativeElement.parentNode;
      this._render.removeChild(parentEl, this._element);
      if (this.isInvalid === true) {
        this._render.appendChild(this._element, text);
        const refChild = this._el.nativeElement;
        this._render.insertBefore(parentEl, this._element, refChild);
      }
    }
    if (this.isInvalid === true) {
      return { appCustomValidator: true };
    } else {
      return null;
    }
  }
}

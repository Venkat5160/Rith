import {
  Directive,
  forwardRef,
  Attribute,
  ElementRef,
  Renderer2,
  Compiler
} from "@angular/core";
import {
  Validator,
  AbstractControl,
  NG_VALIDATORS,
  FormControl
} from "@angular/forms";
import { DomSanitizer } from "@angular/platform-browser";

@Directive({
  selector:
    "[appInputValidator][ngModel],[appInputValidator][formControl],[appInputValidator][formControlName]",
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => InputvalidatorDirective),
      multi: true
    }
  ]
})
export class InputvalidatorDirective implements Validator {
  constructor(
    @Attribute("appInputValidator") public _appInputValidator: string,
    private _el: ElementRef,
    private _render: Renderer2,
    private _domSanitizer: DomSanitizer
  ) {}
  _element = this._render.createElement("span");

  validate(c: AbstractControl): { [key: string]: any } {
    this._element.innerText = null;
    let text = this._render.createText("");
    if (c.errors != null && !c.errors.required) {
      if (c.errors.maxlength && c.errors.maxlength != null) {
        text = this._render.createText(
          `The length should not be grater than ${
            c.errors.maxlength.requiredLength
          }`
        );
      }
      if (c.errors.minlength && c.errors.minlength != null) {
        text = this._render.createText(
          `The length should not be less than ${
            c.errors.minlength.requiredLength
          }`
        );
      }
      if (c.errors.PasswordMatch && c.errors.PasswordMatch != null) {
        text = this._render.createText(
          `Password and confirm password not matched`
        );
      }
      this._render.addClass(this._element, "error-dir-message");

      const parentEl = this._el.nativeElement.parentNode.parentNode.parentNode
        .parentNode.parentNode;
      this._render.removeChild(parentEl, this._element);
      this._render.appendChild(this._element, text);
      const refChild = this._el.nativeElement.parentNode.parentNode.parentNode
        .parentNode;
      this._render.insertBefore(parentEl, this._element, refChild);
    } else {
      this._render.destroy();
    }

    return null;
  }
}

import {
  Directive,
  forwardRef,
  Attribute,
  ElementRef,
  Renderer2
} from "@angular/core";
import { Validator, AbstractControl, NG_VALIDATORS } from "@angular/forms";

@Directive({
  selector:
    "[validateEqual][formControlName],[validateEqual][formControl],[validateEqual][ngModel]",
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => EqualValidator),
      multi: true
    }
  ]
})
export class EqualValidator implements Validator {
  isInvalid = false;
  constructor(
    @Attribute("validateEqual") public validateEqual: string,
    private _el: ElementRef,
    private _render: Renderer2
  ) {}
  _element = this._render.createElement("span");
  validate(c: AbstractControl): { [key: string]: any } {
    this.isInvalid = false;
    this._element.innerText = null;
    let text = this._render.createText("");
    if (c.value) {
      const v = c.value;
      const e = c.root.get(this.validateEqual);
      if (e && v !== e.value) {
        this.isInvalid = true;
        text = this._render.createText(
          `The ${this._el.nativeElement.placeholder.replace("*", "")} and ${
            this.validateEqual
          }  should match`
        );
        this.isInvalid = false;
      }
      this._render.addClass(this._element, "error-dir-message");
      const parentEl = this._el.nativeElement.parentNode.parentNode.parentNode
        .parentNode.parentNode;
      this._render.removeChild(parentEl, this._element);
      this._render.appendChild(this._element, text);
      const refChild = this._el.nativeElement.parentNode.parentNode.parentNode
        .parentNode;
      this._render.insertBefore(parentEl, this._element, refChild);
    }
    if (this.isInvalid) {
      return { validateEqual: true };
    } else {
      return null;
    }
  }
}

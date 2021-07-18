import { Directive, HostListener, ElementRef } from '@angular/core';
@Directive({
  selector: '[appBlockCopyPaste]'
})
export class BlockCopyPasteDirective {
  constructor(Element: ElementRef) {
    Element.nativeElement.innerText = "Text is changed by changeText Directive. ";
  }

  @HostListener('paste', ['$event']) blockPaste(e: KeyboardEvent) {
    e.preventDefault();
  }

  @HostListener('copy', ['$event']) blockCopy(e: KeyboardEvent) {
    e.preventDefault();
  }

  @HostListener('cut', ['$event']) blockCut(e: KeyboardEvent) {
    e.preventDefault();
  }
  // @HostListener('keydown', ['$event'])
  // public onKeydownHandler(e: KeyboardEvent): void {
  //   if (e.keyCode === 13) {
  //     alert("enter")
  //   }
  // }
}

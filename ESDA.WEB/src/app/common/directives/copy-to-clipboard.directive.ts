import {
  Directive,
  Input,
  EventEmitter,
  HostListener,
  Output
} from "@angular/core";

@Directive({
  selector: "[appCopyToClipboard]"
})
export class CopyToClipboardDirective {
  // @ts-ignore
  @Input("appCopyToClipboard")
  public payload: string;

  @Input("context")
  public context: string;

  @Output("copied")
  public copied: EventEmitter<string> = new EventEmitter<string>();

  @HostListener("click", ["$event"])
  public onClick(event: MouseEvent): void {
    event.preventDefault();
    if (!this.payload) {
      return;
    }

    const listener = (e: ClipboardEvent) => {
      const clipboard = e.clipboardData || window["clipboardData"];
      clipboard.setData("text", this.payload.toString());
      e.preventDefault();
      this.copied.emit(this.payload);
    };

    document.addEventListener("copy", listener, false);
    document.execCommand("copy");
    document.removeEventListener("copy", listener, false);
  }
}

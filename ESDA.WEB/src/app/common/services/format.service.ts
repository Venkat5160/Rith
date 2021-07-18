import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root"
})
export class FormatService {
  options = {
    year: "numeric",
    month: "short",
    day: "numeric"
  };
  formatDate(date: any) {
    if (date != null && date != '' && date != undefined) {
      let localDate = new Date(date)
        .toLocaleString(undefined, this.options)
        .replace(
          /[^A-Za-z 0-9 \.,\?""!@#\$%\^&\*\(\)-_=\+;:<>\/\\\|\}\{\[\]`~]*/g,
          ""
        );

      return localDate;
    } else {
      return null;
    }
  }
}

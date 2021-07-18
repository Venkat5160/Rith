import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { HomeService } from '../home.service';
declare function BindReport(embedConfig: any): any;
@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.scss'],
})
export class ReportComponent implements OnInit {

  @ViewChild('embeddedReport')
  embeddedReport: ElementRef;
  constructor(private _homeService: HomeService,
  ) { }

  ngOnInit(): void {
    // this._homeService.getreport().subscribe((config) => {
    //   debugger;
    //   const embedConfig = {
    //     accessToken: config.token,
    //     embedUrl: config.embedUrl,
    //     reportId: config.reportId,
    //   }
    //   BindReport(embedConfig);
    // });
  }
}

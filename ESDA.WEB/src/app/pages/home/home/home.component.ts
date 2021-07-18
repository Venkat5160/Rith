import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { PowerBiReportIdsEnum } from 'src/app/common/constants/validationType.enum';
import { HomeService } from '../home.service';
declare function BindReport(embedConfig: any): any;
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {


  constructor(private _homeService: HomeService) {

  }
  @ViewChild('embeddedReport')
  embeddedReport: ElementRef;
  ngOnInit() {
    debugger;
    this._homeService.getreport(PowerBiReportIdsEnum.DashboardReportId).subscribe((config) => {
      debugger;
      const embedConfig = {
        accessToken: config.token,
        embedUrl: config.embedUrl,
        reportId: config.reportId,
      }
      BindReport(embedConfig);
    });
  }
  print() {
    print();
  }
}

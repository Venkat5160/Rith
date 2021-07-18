import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { PowerBiReportIdsEnum } from 'src/app/common/constants/validationType.enum';
import { HomeService } from '../home.service';
declare function BindReport(embedConfig: any): any;
@Component({
    selector: 'app-daywisesentiment',
    templateUrl: './daywisesentiment.component.html',
    styleUrls: ['./daywisesentiment.component.scss'],
})
export class DayWiseSentimentComponent implements OnInit {

    @ViewChild('embeddedReport')
    embeddedReport: ElementRef;
    constructor(private _homeService: HomeService,
    ) { }

    ngOnInit() {
        debugger;
        this._homeService.getreport(PowerBiReportIdsEnum.DayWiseSentimentReportId).subscribe((config) => {
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

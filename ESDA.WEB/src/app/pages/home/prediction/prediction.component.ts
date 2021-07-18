import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { PowerBiReportIdsEnum } from 'src/app/common/constants/validationType.enum';
import { HomeService } from '../home.service';
declare function BindReport(embedConfig: any): any;
@Component({
    selector: 'app-prediction',
    templateUrl: './prediction.component.html',
    styleUrls: ['./prediction.component.scss'],
})
export class PredictionComponent implements OnInit {

    @ViewChild('embeddedReport')
    embeddedReport: ElementRef;
    constructor(private _homeService: HomeService,
    ) { }

    ngOnInit() {
        debugger;
        this._homeService.getreport(PowerBiReportIdsEnum.PredictionReportId).subscribe((config) => {
            debugger;
            const embedConfig = {
                accessToken: config.token,
                embedUrl: config.embedUrl,
                reportId: config.reportId,
            }
            BindReport(embedConfig);
        });
    };

    print() {
        print();
    };
}

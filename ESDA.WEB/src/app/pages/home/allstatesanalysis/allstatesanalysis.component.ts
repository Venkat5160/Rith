import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { PowerBiReportIdsEnum } from 'src/app/common/constants/validationType.enum';
import { HomeService } from '../home.service';
declare function BindReport(embedConfig: any): any;
@Component({
    selector: 'app-allstatesanalysis',
    templateUrl: './allstatesanalysis.component.html',
    styleUrls: ['./allstatesanalysis.component.scss'],
})
export class AllstatesAnalysisComponent implements OnInit {

    @ViewChild('embeddedReport')
    embeddedReport: ElementRef;
    constructor(private _homeService: HomeService,
    ) { }

    ngOnInit() {
        debugger;
        this._homeService.getreport(PowerBiReportIdsEnum.AllStatesAnalysisReportId).subscribe((config) => {
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

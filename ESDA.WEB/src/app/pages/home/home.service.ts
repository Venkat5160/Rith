import { Injectable } from '@angular/core';
import { CommonService } from 'src/app/core/http/common.service';

@Injectable({
    providedIn: 'root',
})
export class HomeService {
    constructor(private _http: CommonService) {

    }
    [index: string]: any;
    homeServiceURL: string = '/Reports';
    getreport(reportId) {
        return this._http.get(this.homeServiceURL + '/EmbedReport' + '?reportId=' + reportId);;
    };

}

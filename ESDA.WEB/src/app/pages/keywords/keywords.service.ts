import { Injectable } from '@angular/core';
import { CommonService } from 'src/app/core/http/common.service';

@Injectable({
  providedIn: 'root',
})
export class KeywordService {
  constructor(private _http: CommonService) {

  }
  [index: string]: any;
  //keywordServiceURL: string = "/Merchandiser";
 
  keywordServiceURL: string = '/SDAEKeywords';

  getKeywordsList(data) {
    return this._http.post(this.keywordServiceURL + '/KeywordsList', data);
  }
  getKeywordsById(keywordId) {

    return this._http.get(this.keywordServiceURL + '/GetKeywordsById' + '?keywordId=' + keywordId);
  };

  addKeyword(keyword) {
    return this._http.post(this.keywordServiceURL + '/SaveKeywords', keyword);
  }
  deleteKeyword(id) {
    return this._http.delete(this.keywordServiceURL + '/DeleteKeyword', id)
  }

}

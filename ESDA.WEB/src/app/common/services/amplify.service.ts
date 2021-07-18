import { Injectable } from '@angular/core';
import * as amplifyStore from "amplify-store"

@Injectable({
  providedIn: 'root'
})
export class AmplifyService {

  constructor() {

  }

  setStorage(key: string, value: any) {
    amplifyStore(key, value);
  }

  getStorage(key: string) {
    return amplifyStore(key);
  }

  clearStorage(key: string) {
    amplifyStore(key, null);
  }

  clearMultiple(storageArray: any[]) {
    for (let storageItem of storageArray) {
      this.clearStorage(storageItem);
    }
  }

  clearAll() {
    let storageArray = ['productsSrchData', 'productsPageSize', 'productsPageNo', 'supplierSearchData', 'supplierpageSize', 'supplierpageNo', 'agentSearchData', 'agentpageSize', 'agentpageNo'];
    this.clearMultiple(storageArray);
  }
}

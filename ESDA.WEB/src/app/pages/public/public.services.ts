import { Injectable } from '@angular/core';
import { BaseURL } from '../../common/constants/baseURL.enum';
import { loginPost } from '../../core/http/common.service.api';
@Injectable({
  providedIn: 'root',
})
export class PublicService {
  baseURL: string;
  constructor() {}

  loginUser = async (formDetails: any) => {
    const data = `username=${formDetails.userName}&password=${formDetails.password}&client_id=SDAE_Web&client_secret=secret&grant_type=password&deviceToken=web&deviceModel=web&loginDeviceTypeId=1&isAdmin=true`;
    const queryUrl = '/connect/token';
    return await loginPost(BaseURL.url, queryUrl, data);
  };
}

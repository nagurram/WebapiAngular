import { Injectable } from '@angular/core';
import { PlatformLocation } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Global } from '../Shared/global';




@Injectable()
export class UserService {
    rootUrl: string;
    baseurl: string;
    getUserClaims() {
        return this._http.get(this.rootUrl + '/api/GetUserClaims');
    }
    constructor(private _http: HttpClient, platformLocation: PlatformLocation) {
        this.baseurl = Global.BASE_URL// "http://vmtest.australiaeast.cloudapp.azure.com/Dataapi/";
        this.rootUrl = this.baseurl;//(platformLocation as any).location.origin;


    }
    post(url: string, model: any): Observable<any> {
        let body = JSON.stringify(model);
        let headers = new HttpHeaders().set('Content-Type', 'application/json');
        let options = { headers: headers };
        return this._http.post(url, body, options);
    }

    userAuthentication(userName: string, password: string): Observable<any> {
        var body = "username=" + userName + "&password=" + password + "&grant_type=password";
        var url = this.rootUrl +  'token' ;//'api/userapi/authenticate';
        let headers = new HttpHeaders().set('Access-Control-Allow-Origin', '*').set('Content-Type', 'application/x-www-urlencoded').set('No-Auth', 'True').set('Access-Control-Allow-Methods', 'POST,GET,OPTIONS, PUT, DELETE').set('Access-Control-Allow-Headers', '*');
        let options = { headers: headers };
        return this._http.post(url, body, options);

    }

    getusermenu(): Observable<any> {
        var url = this.rootUrl + Global.BASE_USER_ENDPOINT + Global.BASE_USER_MENU;
        return this._http.get(url);
    }
    userlogout(): void {
        console.log('logoutservice called');
        var body = {};
        localStorage.removeItem('userToken');
        var url = this.rootUrl + Global.BASE_USER_ENDPOINT + Global.BASE_USER_LOGOUT;
        let headers = new HttpHeaders().set('Content-Type', 'application/json');
        let options = { headers: headers };
        this._http.post(url, body, options);
    }

    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json() || 'Server error');
    }
}
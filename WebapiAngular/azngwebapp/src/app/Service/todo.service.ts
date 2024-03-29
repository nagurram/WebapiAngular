import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Global } from '../Shared/global';

@Injectable()
export class TodoService {
    constructor(private _http: HttpClient) {
        this.baseurl = Global.BASE_URL;//"http://vmtest.australiaeast.cloudapp.azure.com/Dataapi/";
    }
    baseurl: string;

    get(url: string): Observable<any> {
        console.log(this.baseurl + url);
        return this._http.get(this.baseurl + url);

    }

    getById(url: string, id: number): Observable<any> {
        return this._http.get(this.baseurl + url + id, { observe: 'response' });

    }

    post(url: string, model: any): Observable<any> {
        let body = JSON.stringify(model);
        let headers = new HttpHeaders().set('Content-Type', 'application/json');
        let options = { headers: headers };
        return this._http.post(this.baseurl + url, body, options);

    }

    put(url: string, id: number, model: any): Observable<any> {
        let body = JSON.stringify(model);
        let headers = new HttpHeaders().set('Content-Type', 'application/json');
        let options = { headers: headers };
        return this._http.put(this.baseurl + url + id, body, options);

    }

    delete(url: string, id: number): Observable<any> {
        let headers = new HttpHeaders().set('Content-Type', 'application/json');
        let options = { headers: headers };
        return this._http.delete(url + id, options);

    }

    private handleError(error: Response) {
        console.error(error);
        return throwError(()=>(error.json() || 'Server error'));
    }



}

import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/catch';
//import 'rxjs/Rx';

@Injectable()
export class TicketService {
    constructor(private _http: HttpClient) { }

    get(url: string): Observable<any> {
        return this._http.get(url);
           // .map((response: Response) => <any>response.json())
          //  .catch(this.handleError);
    }

    getById(url: string, id: number): Observable<any> {        
        return this._http.get(url + id, { observe: 'response' });
        //.map((response: Response) => <any>response.json())
         //   .catch(this.handleError);
    }

    post(url: string, model: any): Observable<any> {
        let body = JSON.stringify(model);
        let headers = new HttpHeaders().set( 'Content-Type', 'application/json' );
        let options =   { headers: headers };
        return this._http.post(url, body, options);
           // .map((response: Response) => <any>response.json())
           // .catch(this.handleError);
    }

    put(url: string, id: number, model: any): Observable<any> {
        let body = JSON.stringify(model);
        let headers = new HttpHeaders().set('Content-Type', 'application/json');
        let options = { headers: headers };
        return this._http.put(url + id, body, options);
          //  .map((response: Response) => <any>response.json())
          //  .catch(this.handleError);
    }

    delete(url: string, id: number): Observable<any> {
        let headers = new HttpHeaders().set('Content-Type', 'application/json');
        let options = { headers: headers };
        return this._http.delete(url + id, options);
      //      .map((response: Response) => <any>response.json())
         //   .catch(this.handleError);
    }

    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json() || 'Server error');
    }

    uploadFile(url: string, fileToUpload: File ): Observable<any>  {
        const _formData = new FormData();
        _formData.append('file', fileToUpload, fileToUpload.name);
        let body = _formData;
        let headers = new HttpHeaders();
        let options = { headers: headers };
        return this._http.post(url, body, options); //note: no HttpHeaders passed as 3d param to POST!
        //So no Content-Type constructed manually.
        //Angular 4.x-6.x does it automatically.
    }

    postDownloadFile(url: string): Observable<any> {
        let body = JSON.stringify('');
        //let headers = ;
       // let options = ;
        return this._http.post(url, body, { responseType: 'blob', headers: new HttpHeaders().append('Content-Type', 'application/json') });
        // .map((response: Response) => <any>response.json())
        // .catch(this.handleError);
    }

}

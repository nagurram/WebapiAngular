import {Injectable} from '@angular/core';
import {HttpResponse,HttpClient, HttpHeaders  } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/catch';

@Injectable({ providedIn: 'root' })
export class FileService {

  constructor(private http: HttpClient) {}

  downloadFile(url: string):Observable<any>{	
      console.log('download url is: '+url);
      console.log(this.http.get(url));
		return this.http.get(url);
   }
   
}
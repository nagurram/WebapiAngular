import {Injectable} from '@angular/core';
import {HttpResponse,HttpClient, HttpHeaders  } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/catch';
import { tap } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class FileService {

  constructor(private http: HttpClient) {}

  downloadFile(url: string){	
      console.log('download url is: '+url);
      console.log(this.http.get(url));
      return this.http.get(url,{responseType: 'text'})
      .pipe(
         tap( // Log the result or error
           data => console.log(data),
           error => console.log(error)
         )
       );
   }
   
}
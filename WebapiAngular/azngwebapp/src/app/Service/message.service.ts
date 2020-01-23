import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { IkeyValuePair } from '../Model/keyValuePair';
import { IUserModel } from '../Model/userModel';


@Injectable()
export class MessageService {
    public message = new Subject<IUserModel>();
    setMessage(value: IUserModel) {
        this.message.next(value); 
    }
}
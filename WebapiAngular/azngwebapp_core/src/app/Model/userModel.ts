import { IkeyValuePair } from './keyValuePair';

export interface IUserModel {
    Userid: string,
    UserName: string,
    routeCollection: IkeyValuePair[];
}
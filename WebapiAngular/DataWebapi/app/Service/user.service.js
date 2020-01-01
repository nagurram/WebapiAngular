"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var common_1 = require("@angular/common");
var http_1 = require("@angular/common/http");
var Observable_1 = require("rxjs/Observable");
require("rxjs/add/operator/map");
require("rxjs/add/operator/do");
require("rxjs/add/operator/catch");
var global_1 = require("../Shared/global");
var UserService = /** @class */ (function () {
    function UserService(_http, platformLocation) {
        this._http = _http;
        this.rootUrl = platformLocation.location.origin;
    }
    UserService.prototype.getUserClaims = function () {
        return this._http.get(this.rootUrl + '/api/GetUserClaims');
    };
    UserService.prototype.userAuthentication = function (userName, password) {
        var body = "username=" + userName + "&password=" + password + "&grant_type=password";
        var url = this.rootUrl + '/token';
        var headers = new http_1.HttpHeaders().set('Content-Type', 'application/x-www-urlencoded').set('No-Auth', 'True');
        var options = { headers: headers };
        return this._http.post(url, body, options);
    };
    UserService.prototype.getusermenu = function () {
        var url = global_1.Global.BASE_USER_ENDPOINT + global_1.Global.BASE_USER_MENU;
        return this._http.get(url);
    };
    UserService.prototype.userlogout = function () {
        console.log('logoutservice called');
        var body = {};
        localStorage.removeItem('userToken');
        var url = global_1.Global.BASE_USER_ENDPOINT + global_1.Global.BASE_USER_LOGOUT;
        var headers = new http_1.HttpHeaders().set('Content-Type', 'application/json');
        var options = { headers: headers };
        this._http.post(url, body, options);
    };
    UserService.prototype.handleError = function (error) {
        console.error(error);
        return Observable_1.Observable.throw(error.json() || 'Server error');
    };
    UserService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [http_1.HttpClient, common_1.PlatformLocation])
    ], UserService);
    return UserService;
}());
exports.UserService = UserService;
//# sourceMappingURL=user.service.js.map
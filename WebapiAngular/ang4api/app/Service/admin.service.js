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
var http_1 = require("@angular/common/http");
var Observable_1 = require("rxjs/Observable");
require("rxjs/add/operator/map");
require("rxjs/add/operator/do");
require("rxjs/add/operator/catch");
//import 'rxjs/Rx';
var AdminService = /** @class */ (function () {
    function AdminService(_http) {
        this._http = _http;
    }
    AdminService.prototype.get = function (url) {
        return this._http.get(url);
        // .map((response: Response) => <any>response.json())
        //  .catch(this.handleError);
    };
    AdminService.prototype.getById = function (url, id) {
        return this._http.get(url + id, { observe: 'response' });
        //.map((response: Response) => <any>response.json())
        //   .catch(this.handleError);
    };
    AdminService.prototype.post = function (url, model) {
        var body = JSON.stringify(model);
        var headers = new http_1.HttpHeaders().set('Content-Type', 'application/json');
        var options = { headers: headers };
        return this._http.post(url, body, options);
        // .map((response: Response) => <any>response.json())
        // .catch(this.handleError);
    };
    AdminService.prototype.put = function (url, id, model) {
        var body = JSON.stringify(model);
        var headers = new http_1.HttpHeaders().set('Content-Type', 'application/json');
        var options = { headers: headers };
        return this._http.put(url + id, body, options);
        //  .map((response: Response) => <any>response.json())
        //  .catch(this.handleError);
    };
    AdminService.prototype.delete = function (url, id) {
        var headers = new http_1.HttpHeaders().set('Content-Type', 'application/json');
        var options = { headers: headers };
        return this._http.delete(url + id, options);
        //      .map((response: Response) => <any>response.json())
        //   .catch(this.handleError);
    };
    AdminService.prototype.handleError = function (error) {
        console.error(error);
        return Observable_1.Observable.throw(error.json() || 'Server error');
    };
    AdminService.prototype.uploadFile = function (url, fileToUpload) {
        var _formData = new FormData();
        _formData.append('file', fileToUpload, fileToUpload.name);
        var body = _formData;
        var headers = new http_1.HttpHeaders();
        var options = { headers: headers };
        return this._http.post(url, body, options); //note: no HttpHeaders passed as 3d param to POST!
        //So no Content-Type constructed manually.
        //Angular 4.x-6.x does it automatically.
    };
    AdminService.prototype.postDownloadFile = function (url) {
        var body = JSON.stringify('');
        //let headers = ;
        // let options = ;
        return this._http.post(url, body, { responseType: 'blob', headers: new http_1.HttpHeaders().append('Content-Type', 'application/json') });
        // .map((response: Response) => <any>response.json())
        // .catch(this.handleError);
    };
    AdminService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [http_1.HttpClient])
    ], AdminService);
    return AdminService;
}());
exports.AdminService = AdminService;
//# sourceMappingURL=admin.service.js.map
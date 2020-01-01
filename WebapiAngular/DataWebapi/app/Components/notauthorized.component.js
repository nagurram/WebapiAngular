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
var router_1 = require("@angular/router");
var common_1 = require("@angular/common");
var NotAuthorizedComponent = /** @class */ (function () {
    function NotAuthorizedComponent(_router, location) {
        this._router = _router;
        this.location = location;
    }
    NotAuthorizedComponent.prototype.goBack = function () {
        this.location.back();
    };
    NotAuthorizedComponent = __decorate([
        core_1.Component({
            template: "<h1> Page Not Authorized!</h1>\n<br/>\n<a href=\"javascript:void(0)\" (click)=\"goBack()\" class=\"btn btn-info btn-md\">\n                    <span class=\"glyphicon glyphicon-chevron-left\"></span> Back\n                </a>\n"
        }),
        __metadata("design:paramtypes", [router_1.Router, common_1.Location])
    ], NotAuthorizedComponent);
    return NotAuthorizedComponent;
}());
exports.NotAuthorizedComponent = NotAuthorizedComponent;
//# sourceMappingURL=notauthorized.component.js.map
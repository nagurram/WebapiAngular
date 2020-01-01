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
var user_service_1 = require("../Service/user.service");
var message_service_1 = require("../Service/message.service");
var router_1 = require("@angular/router");
var MenuComponent = /** @class */ (function () {
    function MenuComponent(messageService, userService, router) {
        this.messageService = messageService;
        this.userService = userService;
        this.router = router;
        // this.routeCollection = [];
    }
    MenuComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.userModel == null) {
            this.LoadMenus();
        }
        this.subscription = this.messageService.message.subscribe(function (message) {
            _this.userModel = message;
        });
    };
    MenuComponent.prototype.LoadMenus = function () {
        var _this = this;
        this.userService.getusermenu()
            .subscribe(function (routeCollection) {
            _this.messageService.setMessage(routeCollection);
        }, function (error) { return _this.msg = error; });
    };
    MenuComponent.prototype.logout = function () {
        this.userModel = null;
        this.userService.userlogout();
        this.router.navigateByUrl('/login');
    };
    MenuComponent = __decorate([
        core_1.Component({
            selector: 'menu-items',
            template: "<div *ngIf='userModel'>\n    <nav class='navbar navbar-inverse' >\n        <div class='container-fluid' >\n            <ul class='nav navbar-nav' >\n               <li  *ngFor=\"let cols of userModel.routeCollection\"> <a [routerLink]=\"[cols.key]\" > {{ cols.keyValue }} </a></li >\n            </ul>\n        <ul class=\"nav navbar-nav navbar-right\">\n        <li><a href=\"#\"><span class=\"glyphicon glyphicon-user\"></span> {{userModel.UserName}}</a></li>\n        <li><a href=\"#\" (click)=\"logout()\"><span class=\"glyphicon glyphicon-log-out\"></span> Logout</a></li>\n        </ul>\n        </div>\n      </nav>\n        </div>"
        }),
        __metadata("design:paramtypes", [message_service_1.MessageService, user_service_1.UserService, router_1.Router])
    ], MenuComponent);
    return MenuComponent;
}());
exports.MenuComponent = MenuComponent;
//# sourceMappingURL=menu.component.js.map
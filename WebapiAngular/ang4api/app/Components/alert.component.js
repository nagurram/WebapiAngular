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
var alert_service_1 = require("../Service/alert.service");
var AlertComponent = /** @class */ (function () {
    function AlertComponent(alertService) {
        this.alertService = alertService;
    }
    AlertComponent.prototype.ngOnInit = function () {
        var _this = this;
        //this function waits for a message from alert service, it gets 
        //triggered when we call this from any other component
        this.alertService.getMessage().subscribe(function (message) {
            _this.message = message;
        });
    };
    AlertComponent = __decorate([
        core_1.Component({
            //moduleId: module.id,
            selector: 'alert',
            templateUrl: 'app/Components/alert.component.html',
        }),
        __metadata("design:paramtypes", [alert_service_1.AlertService])
    ], AlertComponent);
    return AlertComponent;
}());
exports.AlertComponent = AlertComponent;
//# sourceMappingURL=alert.component.js.map
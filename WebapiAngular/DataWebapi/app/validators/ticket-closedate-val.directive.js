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
var forms_1 = require("@angular/forms");
var ticketdateValidatorDirective = /** @class */ (function () {
    function ticketdateValidatorDirective() {
    }
    ticketdateValidatorDirective_1 = ticketdateValidatorDirective;
    ticketdateValidatorDirective.prototype.validate = function (control) {
        return null;
    };
    var ticketdateValidatorDirective_1;
    __decorate([
        core_1.Input('tktclosedate'),
        __metadata("design:type", String)
    ], ticketdateValidatorDirective.prototype, "closedate", void 0);
    __decorate([
        core_1.Input('tktcreatedate'),
        __metadata("design:type", String)
    ], ticketdateValidatorDirective.prototype, "createddate", void 0);
    ticketdateValidatorDirective = ticketdateValidatorDirective_1 = __decorate([
        core_1.Directive({
            selector: '[ticketclosedate]',
            providers: [{ provide: forms_1.NG_VALIDATORS, useExisting: ticketdateValidatorDirective_1, multi: true }]
        })
    ], ticketdateValidatorDirective);
    return ticketdateValidatorDirective;
}());
exports.ticketdateValidatorDirective = ticketdateValidatorDirective;
//# sourceMappingURL=ticket-closedate-val.directive.js.map
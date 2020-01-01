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
var DropdownComponent = /** @class */ (function () {
    function DropdownComponent() {
        this.show = false;
    }
    DropdownComponent_1 = DropdownComponent;
    DropdownComponent.prototype.writeValue = function (value) {
        this.value = value || -1;
    };
    DropdownComponent.prototype.registerOnChange = function (fn) { this.onChange = fn; };
    DropdownComponent.prototype.registerOnTouched = function (fn) { this.onTouched = fn; };
    DropdownComponent.prototype.setDisabledState = function (isDisabled) {
        this.disabled = isDisabled;
    };
    var DropdownComponent_1;
    __decorate([
        core_1.Input(),
        __metadata("design:type", Array)
    ], DropdownComponent.prototype, "drpcollection", void 0);
    DropdownComponent = DropdownComponent_1 = __decorate([
        core_1.Component({
            selector: 'dropdown',
            template: " <select  class=\"form-control\" [disabled]=\"disabled\"  (change)=\"onChange($event.target.value)\" (blur)=\"onTouched()\" >\n                                <option value=-1>Select</option>\n                                <option *ngFor=\"let coll of drpcollection\" [value]=\"coll.Id\" [selected]=\"coll.Id ==value\"  (change)=\"pushChange($event)\">{{coll.keyValue}}</option>\n                            </select>",
            providers: [{ provide: forms_1.NG_VALUE_ACCESSOR, useExisting: core_1.forwardRef(function () { return DropdownComponent_1; }), multi: true }]
        }),
        __metadata("design:paramtypes", [])
    ], DropdownComponent);
    return DropdownComponent;
}());
exports.DropdownComponent = DropdownComponent;
//# sourceMappingURL=dropdown.component.js.map
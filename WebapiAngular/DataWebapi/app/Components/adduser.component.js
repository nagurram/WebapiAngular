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
var admin_service_1 = require("../Service/admin.service");
var forms_1 = require("@angular/forms");
var adduserModel_1 = require("../Model/adduserModel");
var global_1 = require("../Shared/global");
var AddUserComponent = /** @class */ (function () {
    //isValidationError: boolean = false;
    function AddUserComponent(formBuilder, _adminservice) {
        this.formBuilder = formBuilder;
        this._adminservice = _adminservice;
    }
    AddUserComponent.prototype.ngOnInit = function () {
        this.addUsermodel = new adduserModel_1.AddUserModel();
        this.addUserForm = this.formBuilder.group({});
        this.addUserForm = this.formBuilder.group({
            'FirstName': new forms_1.FormControl(this.addUsermodel.FirstName, [forms_1.Validators.required]),
            'LastName': new forms_1.FormControl(this.addUsermodel.LastName, [forms_1.Validators.required]),
            'EmailId': new forms_1.FormControl(this.addUsermodel.EmailId, [forms_1.Validators.required, forms_1.Validators.email])
        });
        console.log(this.addUserForm);
    };
    AddUserComponent.prototype.adduser = function () {
        var _this = this;
        if (this.addUserForm.status == 'INVALID') {
            return;
        }
        var result = Object.assign({}, this.addUserForm.value);
        this.addUserForm.reset();
        console.log(result);
        this._adminservice.post(global_1.Global.BASE_ADMIN_ENDPOINT + global_1.Global.BASE_ADMIN_ADDUSER, result).subscribe(function (data) {
            if (data == 1) //Success
             {
                //    this.backtosummary();
            }
            else {
                _this.msg = "There is some issue in saving records, please contact to system administrator!";
            }
        }, function (error) {
            _this.msg = error;
        });
    };
    AddUserComponent = __decorate([
        core_1.Component({
            templateUrl: 'app/Components/adduser.component.html'
        }),
        __metadata("design:paramtypes", [forms_1.FormBuilder, admin_service_1.AdminService])
    ], AddUserComponent);
    return AddUserComponent;
}());
exports.AddUserComponent = AddUserComponent;
//# sourceMappingURL=adduser.component.js.map
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
var ng2_bs3_modal_1 = require("ng2-bs3-modal/ng2-bs3-modal");
var enum_1 = require("../Shared/enum");
var global_1 = require("../Shared/global");
var AdminComponent = /** @class */ (function () {
    function AdminComponent(fb, _adminservice) {
        this.fb = fb;
        this._adminservice = _adminservice;
        this.indLoading = false;
    }
    AdminComponent.prototype.ngOnInit = function () {
        this.applicationFrm = this.fb.group({
            Id: [''],
            keyValue: ['', forms_1.Validators.required],
            IsDeleted: ['']
        });
        this.LoadApplications();
    };
    AdminComponent.prototype.LoadApplications = function () {
        var _this = this;
        this.indLoading = true;
        this._adminservice.get(global_1.Global.BASE_ADMIN_ENDPOINT)
            .subscribe(function (applications) { _this.applications = applications; _this.indLoading = false; }, function (error) { return _this.msg = error; });
    };
    AdminComponent.prototype.addApplication = function () {
        this.dbops = enum_1.DBOperation.create;
        this.SetControlsState(true);
        this.modalTitle = "Add New Application";
        this.modalBtnTitle = "Add";
        this.applicationFrm.reset();
        this.modal.open();
    };
    AdminComponent.prototype.editApplication = function (id) {
        this.dbops = enum_1.DBOperation.update;
        this.SetControlsState(true);
        this.modalTitle = "Edit Application";
        this.modalBtnTitle = "Update";
        this.application = this.applications.filter(function (x) { return x.Id == id; })[0];
        this.applicationFrm.setValue(this.application);
        this.modal.open();
    };
    AdminComponent.prototype.deleteApplication = function (id) {
        this.dbops = enum_1.DBOperation.delete;
        this.SetControlsState(false);
        this.modalTitle = "Confirm to Delete?";
        this.modalBtnTitle = "Delete";
        this.application = this.applications.filter(function (x) { return x.Id == id; })[0];
        this.applicationFrm.setValue(this.application);
        this.modal.open();
    };
    AdminComponent.prototype.onSubmit = function (formData) {
        var _this = this;
        this.msg = "";
        switch (this.dbops) {
            case enum_1.DBOperation.create:
                this._adminservice.post(global_1.Global.BASE_ADMIN_ENDPOINT, formData.value).subscribe(function (data) {
                    if (data == 1) //Success
                     {
                        _this.msg = "Data successfully added.";
                        _this.LoadApplications();
                    }
                    else {
                        _this.msg = "There is some issue in saving records, please contact to system administrator!";
                    }
                    _this.modal.dismiss();
                }, function (error) {
                    _this.msg = error;
                });
                break;
            case enum_1.DBOperation.update:
                this._adminservice.put(global_1.Global.BASE_ADMIN_ENDPOINT + global_1.Global.BASE_UPDATE_APPLICATION, formData.value.Id, formData.value).subscribe(function (data) {
                    if (data == 1) //Success
                     {
                        _this.msg = "Data successfully updated.";
                        _this.LoadApplications();
                    }
                    else {
                        _this.msg = "There is some issue in saving records, please contact to system administrator!";
                    }
                    _this.modal.dismiss();
                }, function (error) {
                    _this.msg = error;
                });
                break;
            case enum_1.DBOperation.delete:
                this._adminservice.delete(global_1.Global.BASE_ADMIN_ENDPOINT + global_1.Global.BASE_DELETE_APPLICATION, formData.value.Id).subscribe(function (data) {
                    if (data == 1) //Success
                     {
                        _this.msg = "Data successfully deleted.";
                        _this.LoadApplications();
                    }
                    else {
                        _this.msg = "There is some issue in saving records, please contact to system administrator!";
                    }
                    _this.modal.dismiss();
                }, function (error) {
                    _this.msg = error;
                });
                break;
        }
    };
    AdminComponent.prototype.SetControlsState = function (isEnable) {
        isEnable ? this.applicationFrm.enable() : this.applicationFrm.disable();
    };
    __decorate([
        core_1.ViewChild('modal'),
        __metadata("design:type", ng2_bs3_modal_1.ModalComponent)
    ], AdminComponent.prototype, "modal", void 0);
    AdminComponent = __decorate([
        core_1.Component({
            templateUrl: 'app/Components/admin.component.html'
        }),
        __metadata("design:paramtypes", [forms_1.FormBuilder, admin_service_1.AdminService])
    ], AdminComponent);
    return AdminComponent;
}());
exports.AdminComponent = AdminComponent;
//# sourceMappingURL=admin.component.js.map
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
var router_1 = require("@angular/router");
var loginModel_1 = require("../Model/loginModel");
var message_service_1 = require("../Service/message.service");
var forms_1 = require("@angular/forms");
var LoginComponent = /** @class */ (function () {
    function LoginComponent(userService, router, messageService, formBuilder) {
        this.userService = userService;
        this.router = router;
        this.messageService = messageService;
        this.formBuilder = formBuilder;
        this.isLoginError = false;
    }
    LoginComponent.prototype.ngOnInit = function () {
        this.loginmodel = new loginModel_1.LoginModel();
        this.loginmodel.Userid = "naren.7090@gmail.com";
        this.loginmodel.Password = "1234";
        this.loginForm = this.formBuilder.group({
            'Userid': new forms_1.FormControl(this.loginmodel.Userid, [forms_1.Validators.required, forms_1.Validators.email]),
            'Password': new forms_1.FormControl(this.loginmodel.Password, {
                validators: forms_1.Validators.required
            })
        });
        this.logout();
    };
    LoginComponent.prototype.logout = function () {
        this.userService.userlogout();
    };
    LoginComponent.prototype.loadMenus = function () {
        var _this = this;
        this.userService.getusermenu()
            .subscribe(function (routeCollection) {
            _this.messageService.setMessage(routeCollection);
        }, function (error) { return _this.msg = error; });
    };
    LoginComponent.prototype.authenticate = function () {
        var _this = this;
        if (this.loginForm.status == 'INVALID') {
            return;
        }
        var result = Object.assign({}, this.loginForm.value);
        this.loginForm.reset();
        this.userService.userAuthentication(result.Userid, result.Password).subscribe(function (data) {
            localStorage.setItem('userToken', data.access_token);
            _this.loadMenus();
            _this.router.navigate(['/home']);
        }, function (err) {
            _this.msg = err.error.error_description;
        });
    };
    LoginComponent = __decorate([
        core_1.Component({
            templateUrl: 'app/Components/login.component.html'
        }),
        core_1.Injectable(),
        __metadata("design:paramtypes", [user_service_1.UserService, router_1.Router, message_service_1.MessageService, forms_1.FormBuilder])
    ], LoginComponent);
    return LoginComponent;
}());
exports.LoginComponent = LoginComponent;
//# sourceMappingURL=login.component.js.map
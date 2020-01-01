"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var common_1 = require("@angular/common");
var platform_browser_1 = require("@angular/platform-browser");
var forms_1 = require("@angular/forms");
var ng2_bs3_modal_1 = require("ng2-bs3-modal/ng2-bs3-modal");
var app_component_1 = require("./app.component");
var http_1 = require("@angular/common/http");
var app_routing_1 = require("./app.routing");
var adduser_component_1 = require("./Components/adduser.component");
var admin_component_1 = require("./components/admin.component");
var home_component_1 = require("./components/home.component");
var ticket_component_1 = require("./components/ticket.component");
var dropdown_component_1 = require("./components/dropdown.component");
var admin_service_1 = require("./Service/admin.service");
var login_component_1 = require("./components/login.component");
var PageNotFound_component_1 = require("./components/PageNotFound.component");
var auth_guard_1 = require("./auth/auth.guard");
var user_service_1 = require("././Service/user.service");
var auth_interceptor_1 = require("./auth/auth.interceptor");
var menu_component_1 = require("./Components/menu.component");
var message_service_1 = require("./Service/message.service");
var notauthorized_component_1 = require("./Components/notauthorized.component");
var alert_component_1 = require("./Components/alert.component");
var alert_service_1 = require("./Service/alert.service");
var AppModule = /** @class */ (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        core_1.NgModule({
            imports: [platform_browser_1.BrowserModule, forms_1.ReactiveFormsModule, http_1.HttpClientModule, app_routing_1.routing, ng2_bs3_modal_1.Ng2Bs3ModalModule, forms_1.FormsModule],
            declarations: [app_component_1.AppComponent, admin_component_1.AdminComponent, home_component_1.HomeComponent, adduser_component_1.AddUserComponent, ticket_component_1.TicketComponent, dropdown_component_1.DropdownComponent, login_component_1.LoginComponent, PageNotFound_component_1.PageNotFoundComponent, menu_component_1.MenuComponent, notauthorized_component_1.NotAuthorizedComponent, alert_component_1.AlertComponent],
            providers: [{ provide: common_1.APP_BASE_HREF, useValue: '/' }, admin_service_1.AdminService, auth_guard_1.AuthGuard, user_service_1.UserService, { provide: http_1.HTTP_INTERCEPTORS, useClass: auth_interceptor_1.AuthInterceptor, multi: true }, message_service_1.MessageService, alert_service_1.AlertService, common_1.DatePipe],
            bootstrap: [app_component_1.AppComponent]
        })
    ], AppModule);
    return AppModule;
}());
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map
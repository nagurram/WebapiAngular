"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var router_1 = require("@angular/router");
var admin_component_1 = require("./components/admin.component");
var home_component_1 = require("./components/home.component");
var ticket_component_1 = require("./components/ticket.component");
var login_component_1 = require("./components/login.component");
var PageNotFound_component_1 = require("./components/PageNotFound.component");
var auth_guard_1 = require("./auth/auth.guard");
var notauthorized_component_1 = require("./Components/notauthorized.component");
var adduser_component_1 = require("./Components/adduser.component");
var appRoutes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: 'home', component: home_component_1.HomeComponent, canActivate: [auth_guard_1.AuthGuard] },
    { path: 'Admin', component: admin_component_1.AdminComponent, canActivate: [auth_guard_1.AuthGuard] },
    { path: 'login', component: login_component_1.LoginComponent },
    { path: 'adduser', component: adduser_component_1.AddUserComponent },
    { path: 'Ticket', component: ticket_component_1.TicketComponent, canActivate: [auth_guard_1.AuthGuard] },
    { path: 'NotAuthorized', component: notauthorized_component_1.NotAuthorizedComponent },
    { path: '**', component: PageNotFound_component_1.PageNotFoundComponent, canActivate: [auth_guard_1.AuthGuard] }
];
exports.routing = router_1.RouterModule.forRoot(appRoutes);
//# sourceMappingURL=app.routing.js.map
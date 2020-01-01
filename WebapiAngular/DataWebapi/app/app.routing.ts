import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AdminComponent } from './components/admin.component';
import { HomeComponent } from './components/home.component';
import { TicketComponent } from './components/ticket.component';
import { LoginComponent } from './components/login.component';
import { PageNotFoundComponent } from './components/PageNotFound.component';
import { AuthGuard } from './auth/auth.guard';
import { NotAuthorizedComponent } from './Components/notauthorized.component';
import { AddUserComponent } from './Components/adduser.component';

const appRoutes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
    { path: 'Admin', component: AdminComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'adduser', component: AddUserComponent },
    { path: 'Ticket', component: TicketComponent, canActivate: [AuthGuard] },
    { path: 'NotAuthorized', component: NotAuthorizedComponent },
    { path: '**', component: PageNotFoundComponent, canActivate: [AuthGuard] }
];

export const routing: ModuleWithProviders =
    RouterModule.forRoot(appRoutes);
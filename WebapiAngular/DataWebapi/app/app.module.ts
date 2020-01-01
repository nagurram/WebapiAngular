import { NgModule } from '@angular/core';
import { APP_BASE_HREF, DatePipe } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { Ng2Bs3ModalModule } from 'ng2-bs3-modal/ng2-bs3-modal';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { routing } from './app.routing';
import { AddUserComponent } from './Components/adduser.component';
import { AdminComponent } from './components/admin.component';
import { HomeComponent } from './components/home.component';
import { TicketComponent } from './components/ticket.component';
import { DropdownComponent } from './components/dropdown.component';
import { AdminService } from './Service/admin.service';
import { LoginComponent } from './components/login.component';
import { PageNotFoundComponent } from './components/PageNotFound.component';
import { AuthGuard } from './auth/auth.guard';
import { UserService } from "././Service/user.service";
import { AuthInterceptor } from "./auth/auth.interceptor";
import { MenuComponent } from './Components/menu.component';
import { MessageService } from './Service/message.service';
import { NotAuthorizedComponent } from './Components/notauthorized.component';
import { AlertComponent } from './Components/alert.component';
import { AlertService } from './Service/alert.service';


@NgModule({
    imports: [BrowserModule, ReactiveFormsModule, HttpClientModule, routing, Ng2Bs3ModalModule, FormsModule],
    declarations: [AppComponent, AdminComponent, HomeComponent, AddUserComponent, TicketComponent, DropdownComponent, LoginComponent, PageNotFoundComponent, MenuComponent, NotAuthorizedComponent, AlertComponent],
    providers: [{ provide: APP_BASE_HREF, useValue: '/' }, AdminService, AuthGuard, UserService, { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }, MessageService, AlertService,DatePipe],
    bootstrap: [AppComponent]
})
export class AppModule { }

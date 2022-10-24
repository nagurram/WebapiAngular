import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { APP_BASE_HREF, DatePipe } from '@angular/common';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './Components/app.component';
import { ToDoComponent } from './Components/todo.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbDatePipe } from './Pipes/ngbDatePipe';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TodoService } from "././Service/todo.service";
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { LoginComponent } from './Components/login.component';
import { PageNotFoundComponent } from './Components/PageNotFound.component';
import { AuthGuard } from './auth/auth.guard';
import { UserService } from "././Service/user.service";
import { AuthInterceptor } from "./auth/auth.interceptor";
import { MenuComponent } from './Components/menu.component';
import { MessageService } from './Service/message.service';
import { NotAuthorizedComponent } from './Components/notauthorized.component';
import { AddUserComponent } from './Components/add-user.component';
import { BaseComponent } from './Components/BaseComponent';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TicketComponent } from './Components/ticket.component';
import { DropdownComponent } from './Components/dropdown.component';
import { TicketService } from '././Service/ticket.service';
import { AlertModule } from 'ngx-bootstrap/alert'
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ApplicationStateService } from './Service/application-state.service';
import { ButtonsModule } from 'ngx-bootstrap/buttons';

@NgModule({
  declarations: [
    AppComponent, ToDoComponent, NgbDatePipe, LoginComponent, PageNotFoundComponent, MenuComponent, BaseComponent, NotAuthorizedComponent, AddUserComponent,
    TicketComponent, DropdownComponent
  ],
  imports: [
    BrowserModule, NgbModule, BrowserAnimationsModule,
    AppRoutingModule, FormsModule, BsDatepickerModule.forRoot(),ButtonsModule.forRoot(), ModalModule.forRoot(), AlertModule.forRoot(), TabsModule.forRoot(), HttpClientModule, ReactiveFormsModule
  ],
  providers: [{ provide: APP_BASE_HREF, useValue: '/angtodo/' }, DatePipe, TodoService, AuthGuard, UserService, TicketService, {
    provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor,
    multi: true
  }, MessageService, ApplicationStateService],
  bootstrap: [AppComponent]
})
export class AppModule { }

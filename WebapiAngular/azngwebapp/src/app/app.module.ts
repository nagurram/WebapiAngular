import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { APP_BASE_HREF, DatePipe } from '@angular/common';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './Components/app.component';
import { ToDoComponent } from './Components/todo.component';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

import { FormsModule } from '@angular/forms';
import {NgbDatePipe} from './Pipes/ngbDatePipe';
import {BsDatepickerModule} from 'ngx-bootstrap/datepicker';
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


@NgModule({
  declarations: [
    AppComponent,ToDoComponent,NgbDatePipe,LoginComponent, PageNotFoundComponent, MenuComponent, NotAuthorizedComponent
  ],
  imports: [
    BrowserModule,NgbModule,BrowserAnimationsModule,
    AppRoutingModule,FormsModule,BsDatepickerModule.forRoot(),HttpClientModule
  ],
  providers: [{ provide: APP_BASE_HREF, useValue: '/angtodo/' },DatePipe,TodoService,AuthGuard, UserService, { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }, MessageService],
  bootstrap: [AppComponent]
})
export class AppModule { }

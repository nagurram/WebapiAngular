import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { APP_BASE_HREF, DatePipe } from '@angular/common';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './Components/app.component';
import { ToDoComponent } from './Components/todo.component';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {ToDoListComponent} from './Components/todolist.component';
import { FormsModule } from '@angular/forms';
import {NgbDatePipe} from './Pipes/date.pipe';


@NgModule({
  declarations: [
    AppComponent,ToDoComponent,ToDoListComponent,NgbDatePipe
  ],
  imports: [
    BrowserModule,NgbModule,
    AppRoutingModule,FormsModule
  ],
  providers: [{ provide: APP_BASE_HREF, useValue: '/' },DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }

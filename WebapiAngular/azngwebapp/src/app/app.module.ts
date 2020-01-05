import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { APP_BASE_HREF, DatePipe } from '@angular/common';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './Components/app.component';
import { ToDoComponent } from './Components/todo.component';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {ToDoListComponent} from './Components/todolist.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,ToDoComponent,ToDoListComponent
  ],
  imports: [
    BrowserModule,NgbModule,
    AppRoutingModule,FormsModule
  ],
  providers: [{ provide: APP_BASE_HREF, useValue: '/' },DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }

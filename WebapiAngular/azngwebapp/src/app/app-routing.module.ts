import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ToDoComponent } from './Components/todo.component';
import {ToDoListComponent} from './Components/todolist.component';


const routes: Routes = [
  { path: '', redirectTo: 'Addtodo', pathMatch: 'full' },
  { path: 'Addtodo', component: ToDoComponent },
  { path: 'angtodo', component: ToDoComponent },
  { path: 'todoList', component: ToDoListComponent },
  
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ToDoComponent } from './Components/todo.component';
import { LoginComponent } from './Components/login.component';
import { PageNotFoundComponent } from './Components/PageNotFound.component';
import { AuthGuard } from './auth/auth.guard';
import { NotAuthorizedComponent } from './Components/notauthorized.component';


const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'Addtodo', component: ToDoComponent },
  { path: 'angtodo', component: ToDoComponent },
  { path: 'login', component: LoginComponent },
  { path: 'NotAuthorized', component: NotAuthorizedComponent },
  { path: '**', component: PageNotFoundComponent, canActivate: [AuthGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

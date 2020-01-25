import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ToDoComponent } from './Components/todo.component';
import { LoginComponent } from './Components/login.component';
import { PageNotFoundComponent } from './Components/PageNotFound.component';
import { AuthGuard } from './auth/auth.guard';
import { NotAuthorizedComponent } from './Components/notauthorized.component';
import { AddUserComponent } from './Components/add-user.component';


const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'angtodo', component: ToDoComponent },
  { path: 'login', component: LoginComponent },
  { path: 'NotAuthorized', component: NotAuthorizedComponent },
  { path: 'adduser', component: AddUserComponent },
  { path: '**', component: PageNotFoundComponent, canActivate: [AuthGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

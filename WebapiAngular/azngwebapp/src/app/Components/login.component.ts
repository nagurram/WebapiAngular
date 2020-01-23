import { Component, OnInit, Injectable } from '@angular/core';
import { UserService } from '../Service/user.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { LoginModel } from '../Model/loginModel';
import { MenuComponent } from './menu.component';
import { MessageService } from '../Service/message.service';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
    templateUrl: './login.component.html'
})

@Injectable()
export class LoginComponent implements OnInit {
    routeCollection: any;
    loginmodel: LoginModel;
    msg: string;
    isLoginError: boolean = false;

    constructor(private userService: UserService, private router: Router, public messageService: MessageService) {
    }

    ngOnInit() {
        this.loginmodel = new LoginModel();
        this.loginmodel.Userid = "naren.7090@gmail.com";
        this.loginmodel.Password = "1234";
        this.logout();
    }

    logout() {
        this.userService.userlogout();
    }

    loadMenus(): void {
        this.userService.getusermenu()
            .subscribe(routeCollection => {
                this.messageService.setMessage(routeCollection);
            },
                error => this.msg = <any>error);
    }

    authenticate(event) {        
        this.userService.userAuthentication(this.loginmodel.Userid, this.loginmodel.Password).subscribe(
            (data: any) => {
                localStorage.setItem('userToken', data.access_token);
                this.loadMenus();
                this.router.navigate(['/angtodo']);
            },
            err => {
                this.msg = err.error.error_description;
            });
    }

}
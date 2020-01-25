import { Component, OnInit, Injectable } from '@angular/core';
import { UserService } from '../Service/user.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { LoginModel } from '../Model/loginModel';
import { MenuComponent } from './menu.component';
import { MessageService } from '../Service/message.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BaseComponent } from './BaseComponent';

@Component({
    templateUrl: './login.component.html'
})

@Injectable()
export class LoginComponent extends BaseComponent implements OnInit {
    loginForm: FormGroup;
    routeCollection: any;
    loginmodel: LoginModel;
    msg: string;
    isLoginError: boolean = false;

    constructor(private userService: UserService, private router: Router, public messageService: MessageService, private formBuilder: FormBuilder) {
        super();
    }


    ngOnInit() {

        this.loginmodel = new LoginModel();
        //this.loginmodel.Userid = "naren.7090@gmail.com";
        //this.loginmodel.Password = "1234";
        this.loginForm = this.formBuilder.group({
            'Userid': new FormControl(this.loginmodel.Userid, [Validators.required, Validators.email]),
            'Password': new FormControl(this.loginmodel.Password, {
                validators: Validators.required
            })
        });
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

    authenticate() {
        if (this.loginForm.status == 'INVALID') {
            this.validateAllFields(this.loginForm); 
            return;
        }
        const result: LoginModel = Object.assign({}, this.loginForm.value);
        this.loginForm.reset();
        this.userService.userAuthentication(result.Userid, result.Password).subscribe(
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
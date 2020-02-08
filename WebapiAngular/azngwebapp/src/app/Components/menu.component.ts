import { Component, OnInit } from '@angular/core';
import { IkeyValuePair } from '../Model/keyValuePair';
import { IUserModel } from '../Model/userModel';
import { UserService } from '../Service/user.service';
import { Observable } from 'rxjs/';
import { Subscription } from 'rxjs';
import { MessageService } from '../Service/message.service';
import { Router } from "@angular/router";
@Component({
    selector: 'menu-items',
    template: `<div *ngIf='userModel'>
    <nav class='navbar navbar-inverse' >
        <div class='container-fluid' >
            <ul class='nav navbar-nav' >
               <li  *ngFor="let cols of userModel.routeCollection"> <a [routerLink]="[cols.key]" routerLinkActive="active" > {{ cols.keyValue }} </a></li >
            </ul>
        <ul class="nav navbar-nav navbar-right">
        <li><a href="#"><span class="glyphicon glyphicon-user"></span> {{userModel.UserName}}</a></li>
        <li><a href="#" (click)="logout()"><span class="glyphicon glyphicon-log-out"></span> Logout</a></li>
        </ul>
        </div>
      </nav>
        </div>` 
})


export class MenuComponent implements OnInit {
    msg: any;
    userModel: IUserModel;
    //routeCollection: IkeyValuePair[];
    subscription: Subscription;
    constructor(public messageService: MessageService, private userService: UserService, private router: Router) {
       // this.routeCollection = [];
    }

    ngOnInit(): void {
        if (this.userModel == null) {
            this.LoadMenus();
        }
        this.subscription = this.messageService.message.subscribe(
            (message) => {
                this.userModel = message;
            }
        );
    }

    LoadMenus(): void {
        this.userService.getusermenu()
            .subscribe(routeCollection => {
                this.messageService.setMessage(routeCollection);
            },
                error => this.msg = <any>error);
    }

    logout(): void {
        this.userModel = null;
        this.messageService.setMessage(null);
        this.userService.userlogout();
        this.router.navigateByUrl('/login');
    }
}
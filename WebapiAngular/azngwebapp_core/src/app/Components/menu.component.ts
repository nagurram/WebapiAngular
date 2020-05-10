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
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
    <div class="collapse navbar-collapse">
            <ul class='navbar-nav mr-auto mt-2 mt-lg-0' >
               <li  *ngFor="let cols of userModel.routeCollection" class="nav-item"> <a  class="nav-link" [routerLink]="[cols.key]" routerLinkActive="active" > {{ cols.keyValue }} </a></li >
            </ul>
        <ul class='navbar-nav ml-auto'>
        <li class="nav-item"><a class="nav-link" href="#"><span class="glyphicon glyphicon-user"></span> {{userModel.UserName}}</a></li>
        <li class="nav-item"><a class="nav-link" href="#" (click)="logout()"><span class="glyphicon glyphicon-log-out"></span> Logout</a></li>
        </ul>
        </div>
      </nav>
        </div>` ,
    // styles: ['.navbar-nav {background-color:#337AB7 !important;}']
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
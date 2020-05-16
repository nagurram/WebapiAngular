import { Component, OnInit } from '@angular/core';
import { IkeyValuePair } from '../Model/keyValuePair';
import { IUserModel } from '../Model/userModel';
import { UserService } from '../Service/user.service';
import { Observable } from 'rxjs/';
import { Subscription } from 'rxjs';
import { MessageService } from '../Service/message.service';
import { Router } from "@angular/router";
import {TitleCasePipe} from "@angular/common"

@Component({
    selector: 'menu-items',
    template: `<nav class="navbar navbar-expand-lg navbar-dark  bg-dark " *ngIf='userModel'>    
    <button class="navbar-toggler" type="button"  (click)="isCollapsed = !isCollapsed" [attr.aria-expanded]="!isCollapsed" aria-controls="collapseMenu">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="collapseMenu" [collapse]="isCollapsed">
            <ul class='navbar-nav mr-auto' >
               <li  *ngFor="let cols of userModel.routeCollection" class="nav-item"> <a  class="nav-link" [routerLink]="[cols.key]" routerLinkActive="active" > {{ cols.keyValue | titlecase}} </a></li >
            </ul>
        <ul class='navbar-nav ml-auto'>
        <li class="nav-item"><a class="nav-link disabled" href="#"><span class="fas fa-user"></span> {{userModel.UserName}}</a></li>
        <li class="nav-item"><a class="nav-link" href="#" (click)="logout()"><i class="fas fa-sign-out-alt"></i> Logout</a></li>
        </ul>
        </div>
      </nav>` 
    // styles: ['.navbar-nav {background-color:#337AB7 !important;}']
})


export class MenuComponent implements OnInit {
    msg: any;
    userModel: IUserModel;
    isCollapsed :boolean;
    //routeCollection: IkeyValuePair[];
    subscription: Subscription;
    constructor(public messageService: MessageService, private userService: UserService, private router: Router) {
        // this.routeCollection = [];
        this.isCollapsed=true;
        this.userModel=null;
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
        this.userService.userlogout().subscribe(
            (data: any) => {
                this.userModel = null;
                this.messageService.setMessage(null);
                localStorage.removeItem('userToken');
                this.router.navigateByUrl('/login');
            },
            err => {
                localStorage.removeItem('userToken');
                this.router.navigateByUrl('/login');
            });;
 
    }
}
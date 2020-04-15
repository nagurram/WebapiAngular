import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { Title } from '@angular/platform-browser';
@Component({
    template: `<h1> Page Not Authorized!</h1>
<br/>
<a href="javascript:void(0)" (click)="goBack()" class="btn btn-info btn-md">
                    <span class="glyphicon glyphicon-chevron-left"></span> Back
                </a>
`
})

export class NotAuthorizedComponent {
    constructor(private _router: Router, private location: Location, private titleService: Title) {
        this.titleService.setTitle('Page Not Authorized!');
    }
    goBack() {
        this.location.back();
    }
}
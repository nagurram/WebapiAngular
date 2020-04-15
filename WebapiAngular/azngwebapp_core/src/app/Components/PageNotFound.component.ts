import { Component, OnInit, TemplateRef } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
    template: `<h1> Page Not found!</h1>`
})

export class PageNotFoundComponent {
    constructor(private titleService: Title)
    {
        this.titleService.setTitle('Page Not found!'); 
    }
    
}
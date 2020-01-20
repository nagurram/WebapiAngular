import { Component, OnInit, NgModule } from '@angular/core';
import { TodoModel } from '../Model/todoModel';
import { NgbDateAdapter, NgbDateStruct, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';

import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
    templateUrl: './todo.component.html',
    styleUrls: ['./todo.component.css']
})
export class ToDoComponent implements OnInit {
    ngOnInit(): void {
        this.Loadtodolist();
    }
    showlist: boolean = true;
    model: NgbDateStruct;
    todomodel: TodoModel;
    placement = 'bottom';
    todolist: TodoModel[] = new Array();
    constructor(private router: Router, private datepipe: DatePipe) {
     
    }
    addtodoitem($event) {
        this.todolist.push(this.todomodel)
        localStorage.setItem('itemsintodo', '1');
        this.showlist = true;
    }

    additems($event) {
        this.todomodel = new TodoModel();
        this.todomodel.Id = this.todolist.length + 1;   
        this.todomodel.IsActive=  ((this.todomodel.Id%2)==0);
        this.showlist = false;
    }

    Loadtodolist(): void {        
        console.log('length of list is ' + this.todolist.length);
    }
}
import { Component, OnInit ,NgModule } from '@angular/core';
import { TodoModel } from '../Model/todoModel';
import {NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';

@Component({
    templateUrl: './todo.component.html',
    styleUrls: ['./todo.component.css']
})
export class ToDoComponent implements OnInit {
    ngOnInit(): void {
        this.todomodel = new TodoModel();
        this.todomodel.Id=2;
    }
    model: NgbDateStruct;
    todomodel: TodoModel;
    placement = 'bottom';
    constructor( )  {
    }
    addtodoitem() {
    }
}
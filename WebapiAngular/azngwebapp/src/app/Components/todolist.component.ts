import { Component, OnInit ,NgModule } from '@angular/core';
import { TodoModel } from '../Model/todoModel';
import {NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';
import {FormsModule} from '@angular/forms';


@Component({
    templateUrl: './todolist.component.html',
    styleUrls: ['./todo.component.css']
})
export class ToDoListComponent implements OnInit {
    ngOnInit(): void {
        this.todos =[];
        this.Loadtodolist();
    }
    model: NgbDateStruct;
    todomodel: TodoModel;
    placement = 'bottom';
    title="";
    todos:any[];
    constructor( private ngbDatePipe: NgbDatePipe)  {

    }
    Loadtodolist(): void {
        console.log('Loading items '+localStorage.todos);
        this.title = "To List";
        var tdlist= JSON.parse(JSON.stringify(localStorage.getItem('todos')));

        for(var i in tdlist)
        {
        this.todos.push(tdlist [i]);
        }
        
        console.log('length of list is '+ this.todos.length);
        
    }
}
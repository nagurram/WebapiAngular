import { Component, OnInit ,NgModule } from '@angular/core';
import { TodoModel } from '../Model/todoModel';
import {NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import {FormsModule} from '@angular/forms';

@Component({
    templateUrl: './todo.component.html',
    styleUrls: ['./todo.component.css']
})
export class ToDoComponent implements OnInit {
    ngOnInit(): void {
        console.log('todo model is: '+localStorage.todos);
        this.todomodel = new TodoModel();
        this.todomodel.Id=2;
    }
    model: NgbDateStruct;
    todomodel: TodoModel;
    placement = 'bottom';
    constructor( private router: Router, private datepipe: DatePipe)  {
        console.log(localStorage.todos);
    }
    addtodoitem($event) {
        console.log(this.todomodel);        
        var obj = JSON.parse(JSON.stringify(localStorage.todos));
        if(obj)
        {
            obj={};
        }
        obj.push(JSON.stringify(this.todomodel));
        localStorage.todos=JSON.stringify(obj);
        this.router.navigateByUrl('/todoList')
    

    }
}
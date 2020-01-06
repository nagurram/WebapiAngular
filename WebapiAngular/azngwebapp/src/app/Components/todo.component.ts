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
        if(JSON.stringify(localStorage.todos))
        {
            console.log('I am in JSON.stringify(localStorage.todos');
       // this.todolist=JSON.parse(JSON.stringify(localStorage.todos));
        }
    }
    model: NgbDateStruct;
    todomodel: TodoModel;
    placement = 'bottom';
    todolist:any[]=[];
    constructor( private router: Router, private datepipe: DatePipe)  {
        console.log(localStorage.todos);
    }
    addtodoitem($event) {
        console.log(this.todomodel); 
        console.log(JSON.stringify(this.todomodel));
      //  console.log(JSON.parse(JSON.stringify(localStorage.todos)));
        if(localStorage.todos ==null)
        {
            console.log('declaring new array');
            localStorage.todos=[]; 
            console.log(localStorage.todos);
        }
        else{

        var tdlist= JSON.parse(JSON.stringify(localStorage.todos));
        console.log('tdlist list is '+tdlist);
        }
        this.todolist.push(JSON.parse(JSON.stringify(this.todomodel)));
        for(var i in tdlist)
        {
        this.todolist.push(tdlist [i]);
        }

        localStorage.todos =JSON.stringify(this.todolist);
        this.router.navigateByUrl('/todoList');
    }
}
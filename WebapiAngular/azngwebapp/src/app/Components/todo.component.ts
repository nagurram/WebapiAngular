import { Component, OnInit  } from '@angular/core';
import { TodoModel } from '../Model/todoModel';


@Component({
    templateUrl: './todo.component.html'
})
export class ToDoComponent implements OnInit {
    ngOnInit(): void {
        this.todomodel = new TodoModel();
        this.todomodel.Id=2;
    }
    todomodel: TodoModel;
    constructor( )  {
    }
    addtodoitem() {
    }
}
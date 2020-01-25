import { Component, OnInit, NgModule } from '@angular/core';
import { TodoModel } from '../Model/todoModel';
import { NgbDateAdapter, NgbDateStruct, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { BsDatepickerConfig} from 'ngx-bootstrap/datepicker';
import { DatePipe } from '@angular/common';
import { FormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { TodoService } from '../Service/todo.service';
import { Global } from '../Shared/global';

@Component({
    templateUrl: './todo.component.html',
    styleUrls: ['./todo.component.css']
})
export class ToDoComponent implements OnInit {
    datepickerconfig: Partial<BsDatepickerConfig>;
    ngOnInit(): void {
        this.Loadtodolist();


    }
    showlist: boolean = true;
    model: NgbDateStruct;
    todomodel: TodoModel;
    todoForm: FormGroup;
    placement = 'bottom';
    msg: string;
    todolist: TodoModel[] = new Array();
    constructor(private router: Router, private datepipe: DatePipe,private _todoService: TodoService,private formBuilder: FormBuilder) {
     this.datepickerconfig= Object.assign({},{containerClass:'theme-dark-blue',  dateInputFormat: 'DD/MM/YYYY'})
    }
    addtodoitem() {
        if (this.todoForm.status == 'INVALID') {
            this.validateAllFields(this.todoForm); 
            return;
          }
        this._todoService.put(Global.BASE_TODO_UPDATE, this.todomodel.TodoId, this.todomodel).subscribe(
            data => {
                if (data == 1) //Success
                {
                    console.log('saved to db');
                    this.Loadtodolist();
                }
                else {
                    this.msg = "There is some issue in saving records, please contact to system administrator!"
                }
            },
            error => {
                this.msg = error;
            }
        );     
        this.showlist = true;
    }

    additems($event) {
        this.todomodel = new TodoModel();
        this.todomodel.TodoId=0;
        this.todomodel.IsActive=true;
        this.showlist = false;
        this.todoForm = this.formBuilder.group({});
        this.todoForm = this.formBuilder.group({
          'Titile': new FormControl(this.todomodel.Titile, [Validators.required]),
          'Description': new FormControl(this.todomodel.Description, [Validators.required]),
          'ActionDate': new FormControl(this.todomodel.actionDate, [Validators.required, Validators.required])
        });
    }

    CancelItem($event) {
        this.showlist = true;
        this.Loadtodolist();
    }

    Loadtodolist(): void {   
        this._todoService.get(Global.BASE_TODOLIST_ENDPOINT)
        .subscribe(todolist => { this.todolist = todolist;  },
        error => this.msg = <any>error);

        console.log('length of list is ' + this.todolist.length);
    }

    validateAllFields(formGroup: FormGroup) {         
        Object.keys(formGroup.controls).forEach(field => {  
            const control = formGroup.get(field);            
            if (control instanceof FormControl) {             
                control.markAsTouched({ onlySelf: true });
            } else if (control instanceof FormGroup) {        
                this.validateAllFields(control);  
            }
        });
    } 
        

}
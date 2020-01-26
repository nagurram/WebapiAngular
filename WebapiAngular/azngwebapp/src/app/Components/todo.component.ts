import { Component, OnInit, TemplateRef } from '@angular/core';
import { TodoModel } from '../Model/todoModel';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { BsDatepickerConfig} from 'ngx-bootstrap/datepicker';
import { DatePipe } from '@angular/common';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { TodoService } from '../Service/todo.service';
import { Global } from '../Shared/global';
import { BaseComponent } from './BaseComponent';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';

@Component({
    templateUrl: './todo.component.html',
    styleUrls: ['./todo.component.css']
})
export class ToDoComponent  extends BaseComponent  implements OnInit {
    datepickerconfig: Partial<BsDatepickerConfig>;
    ngOnInit(): void {
        this.Loadtodolist();


    }
    modalRef: BsModalRef;
    showlist: boolean = true;
    model: NgbDateStruct;
    todomodel: TodoModel;
    todoForm: FormGroup;
    placement = 'bottom';
    msg: string;
    todolist: TodoModel[] = new Array();


    
    constructor(private _todoService: TodoService,private formBuilder: FormBuilder,private modalService: BsModalService) {
        super();
     this.datepickerconfig= Object.assign({},{containerClass:'theme-dark-blue',  dateInputFormat: 'DD/MM/YYYY'})
    }

    openAddtodoModal(template: TemplateRef<any>) {
        this.modalRef = this.modalService.show(template,{ animated: true, keyboard: true, backdrop: true, ignoreBackdropClick: true });
        this.additems();
      }

    addtodoitem() {
        if (this.todoForm.status == 'INVALID') {
            this.validateAllFields(this.todoForm); 
            return;
          }
          const result: TodoModel = Object.assign({}, this.todoForm.value);
          this.todoForm.reset();
          console.log(result);
        this._todoService.put(Global.BASE_TODO_UPDATE, this.todomodel.TodoId, result).subscribe(
            data => {
                if (data == 1) //Success
                {
                    console.log('saved to db');
                    this.Loadtodolist();
                    this.modalRef.hide(); 
                }
                else {
                    this.msg = "There is some issue in saving records, please contact to system administrator!"
                }
            },
            error => {
                this.msg = error;
            }
        );     
    }

    additems() {
        this.todomodel = new TodoModel();
        this.todomodel.TodoId=0;
        this.todomodel.IsActive=true;
        this.todoForm = this.formBuilder.group({});
        this.todoForm = this.formBuilder.group({
          'Titile': new FormControl(this.todomodel.Titile, [Validators.required]),
          'Description': new FormControl(this.todomodel.Description, [Validators.required]),
          'ActionDate': new FormControl(this.todomodel.actionDate, [Validators.required, Validators.required])
        });
    }

    CancelItem() {
        this.modalRef.hide(); 
        this.Loadtodolist();
    }

    Loadtodolist(): void {   
        this._todoService.get(Global.BASE_TODOLIST_ENDPOINT)
        .subscribe(todolist => { this.todolist = todolist;  },
        error => this.msg = <any>error);

        console.log('length of list is ' + this.todolist.length);
    }

   
        

}
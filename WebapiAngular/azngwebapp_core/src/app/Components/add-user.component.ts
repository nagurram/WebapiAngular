import { Component, OnInit, TemplateRef, ViewChild, ElementRef } from '@angular/core';
import { UserService } from '../Service/user.service';
import { FormBuilder, FormGroup, Validators, FormControl, ValidatorFn } from '@angular/forms';
import { AddUserModel } from '../Model/adduserModel';
import { DBOperation } from '../Shared/enum';
import { Observable } from 'rxjs';
import { Global } from '../Shared/global';
import { Router, ActivatedRoute, Route } from '@angular/router';
import { BaseComponent } from './BaseComponent';
import { Title } from '@angular/platform-browser';


@Component({
  templateUrl: './add-user.component.html'
})

export class AddUserComponent extends BaseComponent implements OnInit {
  addUsermodel: AddUserModel;
  addUserForm: FormGroup;
  msg: string;
  savedsuccess: boolean;
  //isValidationError: boolean = false;

  constructor(private formBuilder: FormBuilder, private _userService: UserService, titleService: Title) {
    super(titleService);
    this.pagetitile = 'Add new User';
  }

  ngOnInit(): void {
    this.savedsuccess = false;
    this.addUsermodel = new AddUserModel();
    this.addUserForm = this.formBuilder.group({});
    this.addUserForm = this.formBuilder.group({
      'FirstName': new FormControl(this.addUsermodel.FirstName, [Validators.required]),
      'LastName': new FormControl(this.addUsermodel.LastName, [Validators.required]),
      'EmailId': new FormControl(this.addUsermodel.EmailId, [Validators.required, Validators.email])
    });
    console.log(this.addUserForm);
  }

  adduser() {
    if (this.addUserForm.status == 'INVALID') {
      this.validateAllFields(this.addUserForm);
      return;
    }
    const result: AddUserModel = Object.assign({}, this.addUserForm.value);
    this.addUserForm.reset();
    console.log(result);
    this._userService.post(Global.BASE_URL + Global.BASE_ADMIN_ADDUSER, result).subscribe(
      data => {
        if (data == 1) //Success
        {
          this.savedsuccess = true;
          console.log('User Saved!');
          this.msg = "User Saved!"
        }
        else {
          this.savedsuccess = false;
          console.log('error');
          this.msg = "There is some issue in saving records, please contact to system administrator!"
        }
      },
      error => {
        console.log('error');
        this.savedsuccess = false;
        this.msg = error;
      }
    );
  }

}
import { Component, OnInit, TemplateRef, ViewChild, ElementRef } from '@angular/core';
import { UserService } from '../Service/user.service';
import { FormBuilder, FormGroup, Validators, FormControl, ValidatorFn } from '@angular/forms';
import { AddUserModel } from '../Model/adduserModel';
import { DBOperation } from '../Shared/enum';
import { Observable } from 'rxjs/Rx';
import { Global } from '../Shared/global';
import { Router, ActivatedRoute, Route } from '@angular/router';
import { removeSpaces } from '../Validators/removeSpaces.validator';



@Component({
  templateUrl: './add-user.component.html'
})

export class AddUserComponent implements OnInit {
  addUsermodel: AddUserModel;
  addUserForm: FormGroup;
  msg: string;
  //isValidationError: boolean = false;

  constructor(private formBuilder: FormBuilder, private _userService: UserService) {

  }

  ngOnInit(): void {

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
      return;
    }
    const result: AddUserModel = Object.assign({}, this.addUserForm.value);
    this.addUserForm.reset();
    console.log(result);
    this._userService.post(Global.BASE_URL + Global.BASE_ADMIN_ADDUSER, result).subscribe(
      data => {
        if (data == 1) //Success
        {
          console.log('User Saved!');
          this.msg = "User Saved!"
        }
        else {
          console.log('error');
          this.msg = "There is some issue in saving records, please contact to system administrator!"
        }
      },
      error => {
        console.log('error');
        this.msg = error;
      }
    );
  }

}
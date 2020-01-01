import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { AdminService } from '../Service/admin.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ModalComponent  } from 'ng2-bs3-modal/ng2-bs3-modal';
import { IkeyValuePair } from '../Model/keyValuePair';
import { DBOperation } from '../Shared/enum';
import { Observable } from 'rxjs/Rx';
import { Global } from '../Shared/global';

@Component({
    templateUrl: 'app/Components/admin.component.html'
})

export class AdminComponent implements OnInit{

    @ViewChild('modal') modal: ModalComponent;
    applications: IkeyValuePair[];
    application: IkeyValuePair;
    msg: string;
    indLoading: boolean = false;
    applicationFrm: FormGroup;
    dbops: DBOperation;
    modalTitle: string;
    modalBtnTitle: string;
 
    constructor(private fb: FormBuilder,private _adminservice: AdminService) { }

    ngOnInit(): void {
        this.applicationFrm = this.fb.group({
            Id: [''],
            keyValue: ['', Validators.required],
            IsDeleted: ['']
        });
        this.LoadApplications();
    }

    LoadApplications(): void {
        this.indLoading = true;
        this._adminservice.get(Global.BASE_ADMIN_ENDPOINT)
            .subscribe(applications => { this.applications = applications; this.indLoading = false; },
            error => this.msg = <any>error);
    }

    addApplication() {
        this.dbops = DBOperation.create;
        this.SetControlsState(true);
        this.modalTitle = "Add New Application";
        this.modalBtnTitle = "Add";
        this.applicationFrm.reset();
        this.modal.open();
    }

    editApplication(id: number) {
        this.dbops = DBOperation.update;
        this.SetControlsState(true);
        this.modalTitle = "Edit Application";
        this.modalBtnTitle = "Update";
        this.application = this.applications.filter(x => x.Id == id)[0];
        this.applicationFrm.setValue(this.application);
        this.modal.open();
    }

    deleteApplication(id: number) {
        this.dbops = DBOperation.delete;
        this.SetControlsState(false);
        this.modalTitle = "Confirm to Delete?";
        this.modalBtnTitle = "Delete";
        this.application = this.applications.filter(x => x.Id == id)[0];
        this.applicationFrm.setValue(this.application);
        this.modal.open();
    }

    onSubmit(formData: any) {
        this.msg = "";

        switch (this.dbops) {
            case DBOperation.create:
                this._adminservice.post(Global.BASE_ADMIN_ENDPOINT, formData.value).subscribe(
                    data => {
                        if (data == 1) //Success
                        {
                            this.msg = "Data successfully added.";
                            this.LoadApplications();
                        }
                        else {
                            this.msg = "There is some issue in saving records, please contact to system administrator!"
                        }

                        this.modal.dismiss();
                    },
                    error => {
                        this.msg = error;
                    }
                );
                break;
            case DBOperation.update:
                this._adminservice.put(Global.BASE_ADMIN_ENDPOINT + Global.BASE_UPDATE_APPLICATION, formData.value.Id, formData.value).subscribe(
                    data => {
                        if (data == 1) //Success
                        {
                            this.msg = "Data successfully updated.";
                            this.LoadApplications();
                        }
                        else {
                            this.msg = "There is some issue in saving records, please contact to system administrator!"
                        }

                        this.modal.dismiss();
                    },
                    error => {
                        this.msg = error;
                    }
                );
                break;
            case DBOperation.delete:
                this._adminservice.delete(Global.BASE_ADMIN_ENDPOINT + Global.BASE_DELETE_APPLICATION, formData.value.Id).subscribe(
                    data => {
                        if (data == 1) //Success
                        {
                            this.msg = "Data successfully deleted.";
                            this.LoadApplications();
                        }
                        else {
                            this.msg = "There is some issue in saving records, please contact to system administrator!"
                        }

                        this.modal.dismiss();
                    },
                    error => {
                        this.msg = error;
                    }
                );
                break;

        }
    }

    SetControlsState(isEnable: boolean) {
        isEnable ? this.applicationFrm.enable() : this.applicationFrm.disable();
    }

}
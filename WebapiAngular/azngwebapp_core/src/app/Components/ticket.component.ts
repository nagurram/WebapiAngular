import { Component, OnInit, TemplateRef, ViewChild, ElementRef, ViewChildren, QueryList, AfterViewInit } from '@angular/core';
import { TicketService } from '../Service/ticket.service';
import { FormBuilder, FormGroup, Validators, FormControl, ValidatorFn } from '@angular/forms';
import { IkeyValuePair } from '../Model/keyValuePair';
import { Ticket } from '../Model/ticketModel';
import { DBOperation } from '../Shared/enum';
import { Observable } from 'rxjs/Rx';
import { Global } from '../Shared/global';
import { ActivatedRoute, Router, NavigationStart } from '@angular/router';
import { Location, DatePipe } from '@angular/common';
import { DropdownComponent } from './dropdown.component';
import { HttpHeaders } from '@angular/common/http';
import { TabsetComponent, TabDirective } from 'ngx-bootstrap/tabs';
import { saveAs } from 'file-saver';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { BaseComponent } from './BaseComponent';
import { ApplicationStateService } from '../Service/application-state.service';
import { Title } from '@angular/platform-browser';
import { removeSpaces } from '../validators/removeSpaces.validator';
import { PaginationConfig } from '../Model/paginationconfig';

const MIME_TYPES = {
    pdf: 'application/pdf',
    xls: 'application/vnd.ms-excel',
    xlsx: 'application/vnd.openxmlformats-officedocument.spreadsheetxml.sheet'
}

@Component({
    templateUrl: './ticket.component.html',
    styleUrls: ['./ticket.component.css']
})

export class TicketComponent extends BaseComponent implements OnInit {

    @ViewChild('downloadZipLink') private downloadZipLink: ElementRef;
    @ViewChild('tabset') tabset: TabsetComponent;
    disableSwitching: boolean;
    indLoading: boolean = false;
    applications: IkeyValuePair[];
    users: IkeyValuePair[];
    modules: IkeyValuePair[];
    statuses: IkeyValuePair[];
    priorities: IkeyValuePair[];
    rootcauses: IkeyValuePair[];
    types: IkeyValuePair[];
    tickets: any[];
    attachments: any[];
    msg: string;
    ticketId: number = 0;
    sub: any;
    ticket: Ticket;
    ticketForm: FormGroup;
    fileblob: any;
    modalRef: BsModalRef;
    datepickerconfig: Partial<BsDatepickerConfig>;
    dropdownminval: number = 1;
    config: PaginationConfig;




    constructor(private _tickservice: TicketService, private _route: ActivatedRoute, private location: Location,
        private formBuilder: FormBuilder, private router: Router, private datepipe: DatePipe, private modalService: BsModalService,
        private _appstate: ApplicationStateService, titleService: Title) {
        super(titleService);
        this.datepickerconfig = Object.assign({}, { containerClass: 'theme-dark-blue', dateInputFormat: 'MM/DD/YYYY' })
        this.pagetitile = 'Ticket Summary';
        this.config = new PaginationConfig();
        this.config.totalItems = 10;

    }

    ngOnInit(): void {
        this.ticketId = -1;
        this.ticket = new Ticket();
        this.ticketForm = this.formBuilder.group({});
        this.iniTicketdata();
        this.sub = this._route
            .queryParams
            .subscribe(params => {
                // Defaults to 0 if no query param provided.
                this.ticketId = params.ticketId || -1;
                if (this.ticketId >= 0) {
                    this.attachments = null;
                    this.iniTicketdata();
                    this.GetTicketById(this.ticketId);
                    //console.log('in query params and ticket id : ' + this.ticketId);
                }
                else if (this.ticketId == -1) {

                    // this.ticketId = 0;
                    // console.log('Loading all tickets');
                    this.LoadTickets();
                    this.pagetitile = 'Ticket Summary';
                }
            });

        if (this.ticketId == -1) {
            this.LoadTickets();
        }

    }

    //ngOnDestroy() {
    //    this.sub.unsubscribe();
    //}

    // #region LoadTicketData
    iniTicketdata() {

        this.dropdownminval = this.ticketId > 0 ? 1 : -1;
        this.ticket.TicketId = this.ticketId;
        this.ticketForm = this.formBuilder.group({
            'TicketId': new FormControl(this.ticket.TicketId),
            'Title': new FormControl(this.ticket.Title, [removeSpaces, Validators.required]),
            'Tdescription': new FormControl(this.ticket.Tdescription, [removeSpaces, Validators.required]),
            'CreatedBy': new FormControl(this.ticket.CreatedBy, [Validators.required]),
            'StatusId': new FormControl(this.ticket.StatusId, [Validators.required, Validators.min(this.dropdownminval)]),
            'Createddate': new FormControl(this.ticket.Createddate, [Validators.required]),
            'AssignedTo': new FormControl(this.ticket.AssignedTo, [Validators.required, Validators.min(this.dropdownminval)]),
            'PriorityId': new FormControl(this.ticket.PriorityId, [Validators.required, Validators.min(this.dropdownminval)]),
            'TypeId': new FormControl(this.ticket.TypeId, [Validators.required, Validators.min(this.dropdownminval)]),
            'ApplicationId': new FormControl(this.ticket.ApplicationId, [Validators.required, Validators.min(this.dropdownminval)]),
            'ModuleId': new FormControl(this.ticket.ModuleId, [Validators.required, Validators.min(this.dropdownminval)]),
            'ResponseDeadline': new FormControl(this.ticket.ResponseDeadline, [Validators.required]),
            'ResolutionDeadline': new FormControl(this.ticket.ResolutionDeadline, [Validators.required]),
            'RootCauseId': new FormControl(this.ticket.RootCauseId, [Validators.required, Validators.min(this.dropdownminval)]),
            'Comments': new FormControl(this.ticket.Comments, [removeSpaces, Validators.required]),
            'UpdatedBy': new FormControl(this.ticket.UpdatedBy),
            'LastModifiedon': new FormControl(this.ticket.LastModifiedon)
        }, { validator: statusValidator });
        /*
                this.ticketForm.controls['Createddate'].valueChanges.subscribe(value => {
                    this.ticketForm.controls['Createddate'].setValue(this.datepipe.transform(value, 'dd/MM/yyyy'),
                        {
                            onlySelf: false,
                            emitEvent: false,
                            emitModelToViewChange: false,
                            emitViewToModelChange: false
                        });
                });
                this.ticketForm.controls['ResolutionDeadline'].valueChanges.subscribe(value => {
                    this.ticketForm.controls['ResolutionDeadline'].setValue(this.datepipe.transform(value, 'dd/MM/yyyy'),
                        {
                            onlySelf: false,
                            emitEvent: false,
                            emitModelToViewChange: false,
                            emitViewToModelChange: false
                        });
                });
        
                this.ticketForm.controls['ResponseDeadline'].valueChanges.subscribe(value => {
                    this.ticketForm.controls['ResponseDeadline'].setValue(this.datepipe.transform(value, 'dd/MM/yyyy'),
                        {
                            onlySelf: false,
                            emitEvent: false,
                            emitModelToViewChange: false,
                            emitViewToModelChange: false
                        });
                });
                */
        if (this.ticketId == 0) {
            this.attachments = null;
            return;
        }
    }
    // #endregion 
    goBack(template) {
        if (this.ticketForm.dirty) {
            //todo Need to add a model to ask for confirmation of
            /*  this.alertService.confirmThis("Your changes will be lost, you want to continue?", function () {
                 this.backtosummary();
             }, function () {
                 return;
             }) */
            this.modalRef = this.modalService.show(template, { animated: true, keyboard: true, backdrop: true, ignoreBackdropClick: true, class: 'backgroundTransparent' });
        }
        else {
            if (this.modalRef != null) {
                this.modalRef.hide();
            }
            this.backtosummary();
        }
    }

    LoadTickets(): void {
        this.indLoading = true;
        this.pagetitile = "Ticket Summary";
        this._tickservice.get(Global.BASE_TICKET_ENDPOINT)
            .subscribe(tickets => { this.tickets = tickets; this.indLoading = false; this.config.totalItems = tickets.length; },
                error => this.msg = <any>error);
    }
    // #region Load DropdownData  
    LoadAttachments(id: number): void {

        this._tickservice.getById(Global.BASE_TICKET_ENDPOINT + Global.BASE_TICKET_ATTCHEMENTS, id)
            .subscribe(attachments => { this.attachments = attachments.body; this.indLoading = false; },
                error => this.msg = <any>error);
    }

    Loadapplications(): void {

        this._tickservice.get(Global.BASE_TICKET_ENDPOINT + Global.BASE_TICKET_APPMASTER)
            .subscribe(applications => { this.applications = applications; },
                error => this.msg = <any>error);
    }

    Loadusers(): void {

        this._tickservice.get(Global.BASE_TICKET_ENDPOINT + Global.BASE_TICKET_USERMASTER)
            .subscribe(users => { this.users = users; },
                error => this.msg = <any>error);
    }

    Loadmodules(): void {

        this._tickservice.get(Global.BASE_TICKET_ENDPOINT + Global.BASE_TICKET_MODULEMASTER)
            .subscribe(modules => { this.modules = modules; },
                error => this.msg = <any>error);
    }

    Loadstatuses(): void {

        this._tickservice.get(Global.BASE_TICKET_ENDPOINT + Global.BASE_TICKET_STATUSMASTER)
            .subscribe(statuses => { this.statuses = statuses; },
                error => this.msg = <any>error);
    }

    Loadpriorities(): void {

        this._tickservice.get(Global.BASE_TICKET_ENDPOINT + Global.BASE_TICKET_PRIORITYMASTER)
            .subscribe(priorities => { this.priorities = priorities; },
                error => this.msg = <any>error);
    }

    Loadrootcauses(): void {

        this._tickservice.get(Global.BASE_TICKET_ENDPOINT + Global.BASE_TICKET_RCMASTER)
            .subscribe(rootcauses => { this.rootcauses = rootcauses; },
                error => this.msg = <any>error);
    }

    Loadtypes(): void {

        this._tickservice.get(Global.BASE_TICKET_ENDPOINT + Global.BASE_TICKET_TYPEMASTER)
            .subscribe(types => { this.types = types; },
                error => this.msg = <any>error);
    }
    // #endregion 
    GetTicketById(id: number): void {
        this.ticketId = id;
        this.loaddropdowns();

        this._tickservice.getById(Global.BASE_TICKET_ENDPOINT, id)
            .subscribe(ticket => {
                this.pagetitile = "";
                if (id > 0) {
                    this.ticket = ticket.body[0];

                    var adate = this.ticket.Createddate;
                    this.ticket.Createddate = new Date(adate);
                    adate = this.ticket.ResolutionDeadline;
                    this.ticket.ResolutionDeadline = new Date(adate);
                    adate = this.ticket.ResponseDeadline;
                    this.ticket.ResponseDeadline = new Date(adate);

                    this.pagetitile = this.ticket.Title;
                    this.ticketForm.setValue(Object.assign({}, this.ticket));
                }
            },
                error => this.msg = <any>error);

        this.ticketForm.controls['TicketId'].disable();
        if (id > 0) {
            this.LoadAttachments(id);
        }
    }

    backtosummary(): void {
        this.ticketId = -1;
        this.ticket = new Ticket();
        if (this.modalRef != null) {
            this.modalRef.hide();
        }
        this.ticket.TicketId = this.ticketId;
        this.router.navigate(['/ticket']);
    }

    saveticket(): void {
        this.dropdownminval = 1;
        if (this.ticketForm.status == 'INVALID') {
            this.validateAllFields(this.ticketForm);
            return;
        }
        const tktresult: Ticket = Object.assign({}, this.ticketForm.getRawValue());
        this._tickservice.put(Global.BASE_TICKET_ENDPOINT + Global.BASE_TICKET_UPDATE, tktresult.TicketId, tktresult).subscribe(
            data => {
                if (data == 1) //Success
                {
                    this.backtosummary();
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

    fileEvent($event: { target: { files: { [x: string]: File; }; }; }) {
        const fileSelected: File = $event.target.files[0];
        this._tickservice.uploadFile(Global.BASE_TICKET_ENDPOINT + Global.BASE_TICKET_UPLOAD + this.ticketId, fileSelected).subscribe(
            data => {
                if (data.Message == "1") //Success
                {
                    // $('#fileupload').val('');
                    this.LoadAttachments(this.ticketId);
                    this.msg = "File Upload successfull"
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

    downloadfile(id: number, fileName: string): void {

        const EXT = fileName.substr(fileName.lastIndexOf('.') + 1);
        this._tickservice.downloadFile(Global.BASE_TICKET_ENDPOINT + Global.BASE_TICKET_FILE + id)
            .subscribe(data => {
                //save it on the client machine.
                saveAs(new Blob([data], { type: MIME_TYPES[EXT] }), fileName);
            })
            , error =>// console.log('Error downloading the file'),
                () => console.info('File downloaded successfully');
    }

    goto(id) {
        this.tabset.tabs[id].active = true;
    }

    CancelItem() {
        this.modalRef.hide();
    }

    loaddropdowns() {
        this.Loadapplications();
        this.Loadusers();
        this.Loadmodules();
        this.Loadstatuses();
        this.Loadpriorities();
        this.Loadrootcauses();
        this.Loadtypes();
    }

    pageChanged(event) {
        this.config.currentPage = event;
    }
}

const statusValidator: ValidatorFn = (fg: FormGroup) => {
    if (fg.get('TicketId').value <= '0') {
        return null;
    }
    const start = new Date(fg.get('Createddate').value);
    const end = new Date(fg.get('ResolutionDeadline').value);

    var diff = (start !== null && end !== null) ? end > start : 0
    return diff > 0 ? null : { range: true };
};
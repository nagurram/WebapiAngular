import { Component, OnInit, TemplateRef, ViewChild, ElementRef , ViewChildren , QueryList , AfterViewInit} from '@angular/core';
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
import { removeSpaces } from '../Validators/removeSpaces.validator';
import { TabsetComponent, TabDirective } from 'ngx-bootstrap/tabs';
import { FileService } from '../Service/file.service';

@Component({
    templateUrl: './ticket.component.html'
})

export class TicketComponent implements OnInit {

    @ViewChild('downloadZipLink', {static: false}) private downloadZipLink: ElementRef;
    @ViewChild('tabset', {static: false}) tabset: TabsetComponent;
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
    title: string;
    ticketForm: FormGroup;
    fileblob: any;
    constructor(private _tickservice: TicketService, private _route: ActivatedRoute, private location: Location,
         private formBuilder: FormBuilder, private router: Router,private datepipe: DatePipe,private fileService: FileService) { }

    ngOnInit(): void {
        this.ticketId = 0;
        this.Loadapplications();
        this.Loadusers();
        this.Loadmodules();
        this.Loadstatuses();
        this.Loadpriorities();
        this.Loadrootcauses();
        this.Loadtypes();
        this.ticketForm = this.formBuilder.group({});
        this.sub = this._route
            .queryParams
            .subscribe(params => {
                // Defaults to 0 if no query param provided.
                this.ticketId = params.ticketId || 0;
                if (this.ticketId > 0) {
                    this.GetTicketById(this.ticketId);
                    console.log('in query params and ticket id : ' + this.ticketId);
                }
                else {
                    this.ticketForm.reset;
                    this.ticketId = 0;
                    console.log('Loading all tickets');
                    this.LoadTickets();
                }
            });

        if (this.ticketId == 0) {
            this.LoadTickets();
        }

        this.ticket = new Ticket();
        this.ticket.TicketId = -1;
        this.ticketForm = this.formBuilder.group({
            'TicketId': new FormControl(this.ticket.TicketId),
            'Title': new FormControl(this.ticket.Title, [removeSpaces, Validators.required]),
            'TDescription': new FormControl(this.ticket.TDescription, [removeSpaces, Validators.required]),
            'CreatedBy': new FormControl(this.ticket.CreatedBy, [Validators.required]),
            'StatusId': new FormControl(this.ticket.StatusId, [Validators.required, Validators.min(1)]),
            'Createddate': new FormControl(this.ticket.Createddate, [Validators.required]),
            'AssignedTo': new FormControl(this.ticket.AssignedTo, [Validators.required, Validators.min(1)]),
            'PriorityId': new FormControl(this.ticket.PriorityId, [Validators.required, Validators.min(1)]),
            'TypeId': new FormControl(this.ticket.TypeId, [Validators.required, Validators.min(1)]),
            'ApplicationId': new FormControl(this.ticket.ApplicationId, [Validators.required, Validators.min(1)]),
            'ModuleID': new FormControl(this.ticket.ModuleID, [Validators.required, Validators.min(1)]),
            'ResponseDeadline': new FormControl(this.ticket.ResponseDeadline, [Validators.required]),
            'ResolutionDeadline': new FormControl(this.ticket.ResolutionDeadline, [Validators.required]),
            'RootCauseId': new FormControl(this.ticket.RootCauseId, [Validators.required, Validators.min(1)]),
            'Coommnets': new FormControl(this.ticket.Coommnets, [removeSpaces, Validators.required]),
            'UpdatedBy': new FormControl(this.ticket.UpdatedBy),
            'LastModifiedon': new FormControl(this.ticket.LastModifiedon)
        }, { validator: statusValidator });

        this.ticketForm.controls['Createddate'].valueChanges.subscribe(value => {
            console.log(value);
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
    }

    //ngOnDestroy() {
    //    this.sub.unsubscribe();
    //}


    goBack() {
        if (this.ticketForm.dirty) {
            //todo Need to add a model to ask for confirmation of
           /*  this.alertService.confirmThis("Your changes will be lost, you want to continue?", function () {
                this.backtosummary();
            }, function () {
                return;
            }) */
        }
        else {
            this.backtosummary();
        }
    }

    LoadTickets(): void {
        this.indLoading = true;
        this.title = "Ticket Summary";
        this._tickservice.get(Global.BASE_TICKET_ENDPOINT)
            .subscribe(tickets => { this.tickets = tickets; this.indLoading = false; },
                error => this.msg = <any>error);
    }

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

    GetTicketById(id: number): void {
        this.ticketId = id;
        this._tickservice.getById(Global.BASE_TICKET_ENDPOINT, id)
            .subscribe(ticket => {
                this.ticket = ticket.body[0];
                this.title = this.ticket.Title;
                this.ticketForm.setValue(Object.assign({}, this.ticket));
            },
                error => this.msg = <any>error);

        this.ticketForm.controls['TicketId'].disable();
        this.LoadAttachments(id);
    }


    backtosummary(): void {
        this.ticketId = 0;
        this.ticket = new Ticket();
        this.router.navigate(['/Ticket']);
    }
    saveticket(): void {
        console.log(this.ticketForm);
        console.log(this.ticketForm.status);
        if (this.ticketForm.status == 'INVALID') {
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
                    console.log(this.ticketId);
                    this.LoadAttachments(this.ticketId);
                    console.log(this.attachments)
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

    downloadfile(id: number): void {
        console.log('in download file file id : '+id);
        this.fileService.downloadFile(Global.BASE_TICKET_ENDPOINT + Global.BASE_TICKET_FILE + id).subscribe(response => {
			let blob:any = new Blob([response.blob()], { type: 'text' });
			const url= window.URL.createObjectURL(blob);
			window.open(url);
			window.location.href = response.url;
			//fileSaver.saveAs(blob, 'employees.json');
		}), error => console.log('Error downloading the file'),
                 () => console.info('File downloaded successfully');
    }

    downLoadResponse(data: any, type: string, filename: string) {
        var blob = new Blob([data], { type: type });
        const url = window.URL.createObjectURL(blob);

        const link = this.downloadZipLink.nativeElement;
        link.href = url;
        link.download = filename;
        link.click();

        window.URL.revokeObjectURL(url)

        var pwa = window.open(url);
        if (!pwa || pwa.closed || typeof pwa.closed == 'undefined') {
            alert('Please disable your Pop-up blocker and try again.');
        }
    }
    
    goto(id){
        this.tabset.tabs[id].active = true;
      }

}



const statusValidator: ValidatorFn = (fg: FormGroup) => {
    const start = new Date(fg.get('Createddate').value);
    const end = new Date(fg.get('ResolutionDeadline').value);
    console.log(start);
    console.log(end);
    console.log(end > start);
    var diff = (start !== null && end !== null) ? end > start : 0
    return diff > 0
        ? null
        : { range: true };
};
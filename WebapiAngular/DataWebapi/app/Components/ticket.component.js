"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var admin_service_1 = require("../Service/admin.service");
var forms_1 = require("@angular/forms");
var ticketModel_1 = require("../Model/ticketModel");
var global_1 = require("../Shared/global");
var router_1 = require("@angular/router");
var common_1 = require("@angular/common");
var alert_service_1 = require("../Service/alert.service");
var removeSpaces_validator_1 = require("../Validators/removeSpaces.validator");
//import { concat } from 'rxjs/operator/concat';
//import { saveAs } from 'file-saver';
var TicketComponent = /** @class */ (function () {
    function TicketComponent(_adminservice, _route, location, formBuilder, router, alertService, datepipe) {
        this._adminservice = _adminservice;
        this._route = _route;
        this.location = location;
        this.formBuilder = formBuilder;
        this.router = router;
        this.alertService = alertService;
        this.datepipe = datepipe;
        this.indLoading = false;
        this.ticketId = 0;
    }
    TicketComponent.prototype.ngOnInit = function () {
        var _this = this;
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
            .subscribe(function (params) {
            // Defaults to 0 if no query param provided.
            _this.ticketId = params.ticketId || 0;
            if (_this.ticketId > 0) {
                _this.GetTicketById(_this.ticketId);
                console.log('in query params and ticket id : ' + _this.ticketId);
            }
            else {
                _this.ticketForm.reset;
                _this.ticketId = 0;
                console.log('Loading all tickets');
                _this.LoadTickets();
            }
        });
        if (this.ticketId == 0) {
            this.LoadTickets();
        }
        this.ticket = new ticketModel_1.Ticket();
        this.ticket.TicketId = -1;
        this.ticketForm = this.formBuilder.group({
            'TicketId': new forms_1.FormControl(this.ticket.TicketId),
            'Title': new forms_1.FormControl(this.ticket.Title, [removeSpaces_validator_1.removeSpaces, forms_1.Validators.required]),
            'TDescription': new forms_1.FormControl(this.ticket.TDescription, [removeSpaces_validator_1.removeSpaces, forms_1.Validators.required]),
            'CreatedBy': new forms_1.FormControl(this.ticket.CreatedBy, [forms_1.Validators.required]),
            'StatusId': new forms_1.FormControl(this.ticket.StatusId, [forms_1.Validators.required, forms_1.Validators.min(1)]),
            'Createddate': new forms_1.FormControl(this.ticket.Createddate, [forms_1.Validators.required]),
            'AssignedTo': new forms_1.FormControl(this.ticket.AssignedTo, [forms_1.Validators.required, forms_1.Validators.min(1)]),
            'PriorityId': new forms_1.FormControl(this.ticket.PriorityId, [forms_1.Validators.required, forms_1.Validators.min(1)]),
            'TypeId': new forms_1.FormControl(this.ticket.TypeId, [forms_1.Validators.required, forms_1.Validators.min(1)]),
            'ApplicationId': new forms_1.FormControl(this.ticket.ApplicationId, [forms_1.Validators.required, forms_1.Validators.min(1)]),
            'ModuleID': new forms_1.FormControl(this.ticket.ModuleID, [forms_1.Validators.required, forms_1.Validators.min(1)]),
            'ResponseDeadline': new forms_1.FormControl(this.ticket.ResponseDeadline, [forms_1.Validators.required]),
            'ResolutionDeadline': new forms_1.FormControl(this.ticket.ResolutionDeadline, [forms_1.Validators.required]),
            'RootCauseId': new forms_1.FormControl(this.ticket.RootCauseId, [forms_1.Validators.required, forms_1.Validators.min(1)]),
            'Coommnets': new forms_1.FormControl(this.ticket.Coommnets, [removeSpaces_validator_1.removeSpaces, forms_1.Validators.required]),
            'UpdatedBy': new forms_1.FormControl(this.ticket.UpdatedBy),
            'LastModifiedon': new forms_1.FormControl(this.ticket.LastModifiedon)
        }, { validator: statusValidator });
        this.ticketForm.controls['Createddate'].valueChanges.subscribe(function (value) {
            console.log(value);
            _this.ticketForm.controls['Createddate'].setValue(_this.datepipe.transform(value, 'dd/MM/yyyy'), {
                onlySelf: false,
                emitEvent: false,
                emitModelToViewChange: false,
                emitViewToModelChange: false
            });
        });
        this.ticketForm.controls['ResolutionDeadline'].valueChanges.subscribe(function (value) {
            _this.ticketForm.controls['ResolutionDeadline'].setValue(_this.datepipe.transform(value, 'dd/MM/yyyy'), {
                onlySelf: false,
                emitEvent: false,
                emitModelToViewChange: false,
                emitViewToModelChange: false
            });
        });
    };
    //ngOnDestroy() {
    //    this.sub.unsubscribe();
    //}
    TicketComponent.prototype.goBack = function () {
        if (this.ticketForm.dirty) {
            this.alertService.confirmThis("Your changes will be lost, you want to continue?", function () {
                this.backtosummary();
            }, function () {
                return;
            });
        }
        else {
            this.backtosummary();
        }
    };
    TicketComponent.prototype.LoadTickets = function () {
        var _this = this;
        this.indLoading = true;
        this.title = "Ticket Summary";
        this._adminservice.get(global_1.Global.BASE_TICKET_ENDPOINT)
            .subscribe(function (tickets) { _this.tickets = tickets; _this.indLoading = false; }, function (error) { return _this.msg = error; });
    };
    TicketComponent.prototype.LoadAttachments = function (id) {
        var _this = this;
        this._adminservice.getById(global_1.Global.BASE_TICKET_ENDPOINT + global_1.Global.BASE_TICKET_ATTCHEMENTS, id)
            .subscribe(function (attachments) { _this.attachments = attachments.body; _this.indLoading = false; }, function (error) { return _this.msg = error; });
    };
    TicketComponent.prototype.Loadapplications = function () {
        var _this = this;
        this._adminservice.get(global_1.Global.BASE_TICKET_ENDPOINT + global_1.Global.BASE_TICKET_APPMASTER)
            .subscribe(function (applications) { _this.applications = applications; }, function (error) { return _this.msg = error; });
    };
    TicketComponent.prototype.Loadusers = function () {
        var _this = this;
        this._adminservice.get(global_1.Global.BASE_TICKET_ENDPOINT + global_1.Global.BASE_TICKET_USERMASTER)
            .subscribe(function (users) { _this.users = users; }, function (error) { return _this.msg = error; });
    };
    TicketComponent.prototype.Loadmodules = function () {
        var _this = this;
        this._adminservice.get(global_1.Global.BASE_TICKET_ENDPOINT + global_1.Global.BASE_TICKET_MODULEMASTER)
            .subscribe(function (modules) { _this.modules = modules; }, function (error) { return _this.msg = error; });
    };
    TicketComponent.prototype.Loadstatuses = function () {
        var _this = this;
        this._adminservice.get(global_1.Global.BASE_TICKET_ENDPOINT + global_1.Global.BASE_TICKET_STATUSMASTER)
            .subscribe(function (statuses) { _this.statuses = statuses; }, function (error) { return _this.msg = error; });
    };
    TicketComponent.prototype.Loadpriorities = function () {
        var _this = this;
        this._adminservice.get(global_1.Global.BASE_TICKET_ENDPOINT + global_1.Global.BASE_TICKET_PRIORITYMASTER)
            .subscribe(function (priorities) { _this.priorities = priorities; }, function (error) { return _this.msg = error; });
    };
    TicketComponent.prototype.Loadrootcauses = function () {
        var _this = this;
        this._adminservice.get(global_1.Global.BASE_TICKET_ENDPOINT + global_1.Global.BASE_TICKET_RCMASTER)
            .subscribe(function (rootcauses) { _this.rootcauses = rootcauses; }, function (error) { return _this.msg = error; });
    };
    TicketComponent.prototype.Loadtypes = function () {
        var _this = this;
        this._adminservice.get(global_1.Global.BASE_TICKET_ENDPOINT + global_1.Global.BASE_TICKET_TYPEMASTER)
            .subscribe(function (types) { _this.types = types; }, function (error) { return _this.msg = error; });
    };
    TicketComponent.prototype.GetTicketById = function (id) {
        var _this = this;
        this.ticketId = id;
        this._adminservice.getById(global_1.Global.BASE_TICKET_ENDPOINT, id)
            .subscribe(function (ticket) {
            _this.ticket = ticket.body[0];
            _this.title = _this.ticket.Title;
            _this.ticketForm.setValue(Object.assign({}, _this.ticket));
        }, function (error) { return _this.msg = error; });
        this.ticketForm.controls['TicketId'].disable();
        this.LoadAttachments(id);
    };
    TicketComponent.prototype.backtosummary = function () {
        this.ticketId = 0;
        this.ticket = new ticketModel_1.Ticket();
        this.router.navigate(['/Ticket']);
    };
    TicketComponent.prototype.saveticket = function () {
        var _this = this;
        console.log(this.ticketForm);
        console.log(this.ticketForm.status);
        if (this.ticketForm.status == 'INVALID') {
            return;
        }
        var tktresult = Object.assign({}, this.ticketForm.getRawValue());
        this._adminservice.put(global_1.Global.BASE_TICKET_ENDPOINT + global_1.Global.BASE_TICKET_UPDATE, tktresult.TicketId, tktresult).subscribe(function (data) {
            if (data == 1) //Success
             {
                _this.backtosummary();
            }
            else {
                _this.msg = "There is some issue in saving records, please contact to system administrator!";
            }
        }, function (error) {
            _this.msg = error;
        });
    };
    TicketComponent.prototype.fileEvent = function ($event) {
        var _this = this;
        var fileSelected = $event.target.files[0];
        this._adminservice.uploadFile(global_1.Global.BASE_TICKET_ENDPOINT + global_1.Global.BASE_TICKET_UPLOAD + this.ticketId, fileSelected).subscribe(function (data) {
            if (data.Message == "1") //Success
             {
                console.log(_this.ticketId);
                _this.LoadAttachments(_this.ticketId);
                console.log(_this.attachments);
                _this.msg = "File Upload successfull";
            }
            else {
                _this.msg = "There is some issue in saving records, please contact to system administrator!";
            }
        }, function (error) {
            _this.msg = error;
        });
    };
    TicketComponent.prototype.downloadfile = function (id) {
        var _this = this;
        this._adminservice.get(global_1.Global.BASE_TICKET_ENDPOINT + global_1.Global.BASE_TICKET_FILE + id)
            .subscribe(function (response) {
            //if (response) {
            //    var condisposition = response.headers.get('Content-Disposition');
            //    console.log(response.headers.get('Content-Type'));
            //    var contentType = response.headers.get('Content-Type');
            //    var filename = response.headers.get('x-FileName');
            //    this.downLoadResponse(response.body, contentType, filename);
            //    //var blob = new Blob([response.body], { type: contentType });
            //   // saveAs(blob, filename);
            //}
        }, function (error) { return _this.msg = error; });
    };
    TicketComponent.prototype.downLoadResponse = function (data, type, filename) {
        var blob = new Blob([data], { type: type });
        var url = window.URL.createObjectURL(blob);
        var link = this.downloadZipLink.nativeElement;
        link.href = url;
        link.download = filename;
        link.click();
        window.URL.revokeObjectURL(url);
        var pwa = window.open(url);
        if (!pwa || pwa.closed || typeof pwa.closed == 'undefined') {
            alert('Please disable your Pop-up blocker and try again.');
        }
    };
    __decorate([
        core_1.ViewChild('downloadZipLink'),
        __metadata("design:type", core_1.ElementRef)
    ], TicketComponent.prototype, "downloadZipLink", void 0);
    TicketComponent = __decorate([
        core_1.Component({
            templateUrl: 'app/Components/ticket.component.html'
        }),
        __metadata("design:paramtypes", [admin_service_1.AdminService, router_1.ActivatedRoute, common_1.Location, forms_1.FormBuilder, router_1.Router, alert_service_1.AlertService, common_1.DatePipe])
    ], TicketComponent);
    return TicketComponent;
}());
exports.TicketComponent = TicketComponent;
var statusValidator = function (fg) {
    var start = new Date(fg.get('Createddate').value);
    var end = new Date(fg.get('ResolutionDeadline').value);
    console.log(start);
    console.log(end);
    console.log(end > start);
    var diff = (start !== null && end !== null) ? end > start : 0;
    return diff > 0
        ? null
        : { range: true };
};
//# sourceMappingURL=ticket.component.js.map
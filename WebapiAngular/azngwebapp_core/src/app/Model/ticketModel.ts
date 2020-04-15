export class Ticket {
    TicketId: number=0;
    Title: string="";
    Tdescription: string="";
    CreatedBy: number=-1;
    StatusId: number=-1;
    Createddate: Date = new Date();
    AssignedTo: number=-1;
    PriorityId: number=-1;
    TypeId: number=-1;
    ApplicationId: number=-1;
    ModuleId: number=-1;
    ResponseDeadline: Date= new Date();
    ResolutionDeadline: Date= new Date();
    RootCauseId: number=-1;
    Comments: string="";
    UpdatedBy: number=-1;
    LastModifiedon: Date= new Date();
}
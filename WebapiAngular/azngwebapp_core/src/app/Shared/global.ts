export class Global {


    //public static BASE_URL="http://vmtest.australiaeast.cloudapp.azure.com/Dataapi/"
    //public static BASE_URL="http://localhost/dataapi/"
    //public static BASE_URL = "http://192.168.2.34:90/"
    //public static BASE_URL = "http://chandus8411c.mylabserver.com/dataservice/"
    public static BASE_URL = "http://localhost:52865/"
    //public static BASE_URL = "http://chandus8411c.mylabserver.com/dataservice/"
    
    //ENDPOINTS
    public static BASE_TODOLIST_ENDPOINT = "api/Todoapi/todolist";
    public static BASE_TODO_UPDATE = "api/Todoapi/Updatetodo/";
    public static BASE_USER_ENDPOINT = "api/userapi/";
    //METHOD CONSTANTS
    //public static BASE_TICKET_ENDPOINT = "http://vmtest.australiaeast.cloudapp.azure.com/Dataapi/"+"api/ticketapi/";
    public static BASE_TICKET_ENDPOINT = Global.BASE_URL + "api/ticketapi/";
    public static BASE_UPDATE_APPLICATION = "Updateapplication/";
    public static BASE_DELETE_APPLICATION = "deleteapplication/";
    public static BASE_TICKET_USERMASTER = "UserMaster/";
    public static BASE_TICKET_STATUSMASTER = "StatusMaster/";
    public static BASE_TICKET_PRIORITYMASTER = "PriorityMaster/";
    public static BASE_TICKET_RCMASTER = "RootcauseMaster/";
    public static BASE_TICKET_MODULEMASTER = "ModuleMaster/";
    public static BASE_TICKET_APPMASTER = "AppMaster/";
    public static BASE_TICKET_TYPEMASTER = "TypeMaster/";

    public static BASE_TICKET_UPLOAD = "Uploadattachments/";

    public static BASE_TICKET_ATTCHEMENTS = "Getattachments/";
    public static BASE_TICKET_FILE = "GetfileAttachemnet/";
    public static BASE_ADMIN_ADDUSER = "api/adminapi/adduser";
    public static BASE_TICKET_UPDATE = "Updateticket/";

    public static BASE_USER_MENU = "GetMenuitems";
    public static BASE_USER_LOGOUT = "Logout";




}
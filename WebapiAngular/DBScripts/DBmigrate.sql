 
USE [TicketTracker]
GO
/****** Object:  UserDefinedFunction [dbo].[ufn_CSVToTable]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Name:			ufn_CSVToTable
-- Purpose: 		to Get comma separated values into Table valued function
-- =============================================
-- Change History
-- Date    Author   Comments
-- -----   ------   -----------------------------
-- 27/06/2014 	Narendranath	   Fn ALTERd.
-- =============================================
CREATE FUNCTION [dbo].[ufn_CSVToTable] ( @StringInput VARCHAR(8000) )
RETURNS @OutputTable TABLE ( [String] VARCHAR(10) )
AS
BEGIN

    DECLARE @String    VARCHAR(10)

    WHILE LEN(@StringInput) > 0
    BEGIN
        SET @String      = LEFT(@StringInput, 
                                ISNULL(NULLIF(CHARINDEX(',', @StringInput) - 1, -1),
                                LEN(@StringInput)))
        SET @StringInput = SUBSTRING(@StringInput,
                                     ISNULL(NULLIF(CHARINDEX(',', @StringInput), 0),
                                     LEN(@StringInput)) + 1, LEN(@StringInput))

        INSERT INTO @OutputTable ( [String] )
        VALUES ( @String )
    END
    
    RETURN
END

GO
/****** Object:  UserDefinedFunction [dbo].[Ufn_RoleDEscription]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Ufn_RoleDEscription](@Roles Nvarchar(20) ) RETURNS nvarchar(200)
AS
BEGIN
DECLARE @RoleDEsc nvarchar(200)
SELECT @RoleDEsc =Stuff(( SELECT ', ' + roleDescription  FROM rolemaster where RoleId in (select cast(string  as INT) FROM  [dbo].[ufn_CSVToTable](@Roles))
    FOR XML PATH('')
    ), 1, 2, '') 

  FROM RoleMaster 
  RETURN @RoleDEsc
END

GO
/****** Object:  UserDefinedFunction [dbo].[ufnGetChilds]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ufnGetChilds](@parentId int)
RETURNS @Chids TABLE 
(

  parent_entity_id int  , 
    child_entity_id int
	)

	AS 
	BEGIN
	;with CTE as (
select  @parentId 'Parent_Entity_ID',Child_Entity_ID from ENTITY_LINK WHERE parent_entity_id=@parentId
)
insert into  @Chids Select Parent_Entity_ID,child_entity_id from ENTITY_LINK WHERE Child_Entity_ID in (select Child_Entity_ID from CTE)


return;
	END

GO
/****** Object:  Table [dbo].[MenuItems]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MenuItems](
	[MenuItemId] [int] IDENTITY(1,1) NOT NULL,
	[MenuId] [int] NOT NULL,
	[link] [nvarchar](100) NULL,
	[parentmenuid] [int] NULL,
	[Sortorder] [int] NULL,
 CONSTRAINT [PK_MenuItems] PRIMARY KEY CLUSTERED 
(
	[MenuItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MenuPermission]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MenuPermission](
	[MenuItemId] [int] NOT NULL,
	[RoleId] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menus]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menus](
	[MenuId] [int] IDENTITY(1,1) NOT NULL,
	[DisplayName] [nvarchar](100) NULL,
 CONSTRAINT [PK_Menus] PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserMaster]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserMaster](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FName] [nvarchar](100) NULL,
	[LName] [nvarchar](100) NULL,
	[Email] [nvarchar](100) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_UserMaster] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[Mappingid] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[Mappingid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_user_permissions]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  view [dbo].[vw_user_permissions]
as
select
    distinct  mn.displayname,   mi.menuitemid, mi.link, mi.parentmenuid, mi.sortorder, u.userid	,ur.roleid
from
    UserMaster u
    join userroles ur on (u.userid = ur.userid)    
    join MenuPermission per on (ur.roleid = per.roleid  )
    join menuitems mi on (per.menuitemid = mi.menuitemid)
	JOIN menus mn on mn.menuid=mi.menuid
GO
/****** Object:  Table [dbo].[ApplicationMaster]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationMaster](
	[ApplicationId] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationName] [nvarchar](100) NULL,
	[statusOrder] [int] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_ApplicationMaster] PRIMARY KEY CLUSTERED 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[empid] [int] IDENTITY(1,1) NOT NULL,
	[DateofJoining] [date] NULL,
	[managerid] [int] NULL,
	[BusinessUnit] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeDetails]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeeDetails](
	[empid] [int] NULL,
	[FirstName] [nvarchar](100) NULL,
	[LastName] [nvarchar](100) NULL,
	[EmailId] [nvarchar](200) NULL,
	[Address] [nvarchar](1000) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Entity]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Entity](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NULL,
	[Reference_ID] [int] NULL,
 CONSTRAINT [PK_Entity] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Entity_Link]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Entity_Link](
	[Link_ID] [int] IDENTITY(1,1) NOT NULL,
	[Parent_Entity_ID] [int] NULL,
	[Child_Entity_ID] [int] NULL,
 CONSTRAINT [PK_Entity_Link] PRIMARY KEY CLUSTERED 
(
	[Link_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Exception]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Exception](
	[ExceptionId] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](150) NULL,
	[StackTrace] [nvarchar](4000) NULL,
	[UserId] [int] NULL,
	[ExceptionDateTime] [datetime] NULL,
	[InnerExceptionmessage] [nvarchar](4000) NULL,
	[ControllerName] [nvarchar](50) NULL,
	[ActionName] [nvarchar](50) NULL,
 CONSTRAINT [PK_Exception] PRIMARY KEY CLUSTERED 
(
	[ExceptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FileUpload]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileUpload](
	[Fileid] [int] IDENTITY(1,1) NOT NULL,
	[Filedata] [varbinary](max) NULL,
	[TicketId] [int] NULL,
	[Filetype] [nvarchar](15) NULL,
	[UploadDate] [datetime] NULL,
	[FileName] [nvarchar](150) NULL,
 CONSTRAINT [PK_FileUpload] PRIMARY KEY CLUSTERED 
(
	[Fileid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinancialEventSource]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinancialEventSource](
	[FinancialEventSourceId] [int] IDENTITY(1,1) NOT NULL,
	[FinancialEventSourceTypeId] [int] NULL,
	[CaptureDateTime] [datetime] NULL,
 CONSTRAINT [PK_FinancialEventSource] PRIMARY KEY CLUSTERED 
(
	[FinancialEventSourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinancialEventSourceFlatFile]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinancialEventSourceFlatFile](
	[FinancialEventSourceFlatFileId] [int] IDENTITY(1,1) NOT NULL,
	[FinancialEventSourceId] [int] NULL,
	[ClientProfileId] [int] NULL,
	[FlatFileProcessingStatusId] [int] NULL,
	[FileName] [nvarchar](200) NULL,
	[ReceiveDir] [nvarchar](200) NULL,
	[ReceiveDateTime] [datetime] NULL,
	[ProcessingNotes] [nvarchar](200) NULL,
	[LastUpdatedUser] [int] NULL,
	[LastUpdatedDateTime] [datetime] NULL,
 CONSTRAINT [PK_FinancialEventSourceFlatFile] PRIMARY KEY CLUSTERED 
(
	[FinancialEventSourceFlatFileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinancialEventSourceSwift]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinancialEventSourceSwift](
	[FinancialEventSourceSwiftId] [int] IDENTITY(1,1) NOT NULL,
	[FinancialEventSourceId] [int] NULL,
	[SwiftMessageId] [int] NULL,
	[SwiftMessageTypeId] [int] NULL,
	[BatchId] [int] NULL,
	[SenderBic] [int] NULL,
	[Message] [nvarchar](200) NULL,
	[MessageXML] [nvarchar](2000) NULL,
	[ReceiveDateTime] [datetime] NULL,
 CONSTRAINT [PK_FinancialEventSourceSwift] PRIMARY KEY CLUSTERED 
(
	[FinancialEventSourceSwiftId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FXTrade]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FXTrade](
	[FXTradeId] [int] IDENTITY(1,1) NOT NULL,
	[FinancialEventId] [int] NULL,
	[PortfolioId] [int] NULL,
	[BrokerId] [int] NULL,
	[TradeDate] [datetime] NULL,
	[SettlementDate] [datetime] NULL,
	[SpotForward] [decimal](18, 2) NULL,
	[Rate] [decimal](18, 2) NULL,
	[HiportRef] [nvarchar](200) NULL,
	[TradeCurrencyId] [int] NULL,
	[CounterPartyCurrencyId] [int] NULL,
	[TradeNominal] [nvarchar](200) NULL,
	[CounterPartyNominal] [nvarchar](200) NULL,
	[Position] [int] NULL,
	[SenderToReceieverInfo] [nvarchar](2000) NULL,
	[TradeAllocationFinancialEventId] [int] NULL,
	[MatchStatusId] [int] NULL,
 CONSTRAINT [PK_FXTrade] PRIMARY KEY CLUSTERED 
(
	[FXTradeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ModuleMaster]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModuleMaster](
	[ModuleId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleName] [nvarchar](100) NULL,
	[statusOrder] [int] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_ModuleMaster] PRIMARY KEY CLUSTERED 
(
	[ModuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PriorityMaster]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PriorityMaster](
	[PriorityId] [int] IDENTITY(1,1) NOT NULL,
	[PriorityDescription] [nvarchar](100) NULL,
	[PriorityOrder] [int] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_PriorityMaster] PRIMARY KEY CLUSTERED 
(
	[PriorityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Resource]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Resource](
	[ResourceId] [int] IDENTITY(1,1) NOT NULL,
	[FName] [nvarchar](50) NULL,
	[Lname] [nvarchar](50) NULL,
	[Email] [nvarchar](80) NULL,
	[Pwd] [nvarchar](50) NULL,
	[Roles] [nvarchar](10) NOT NULL,
	[Isactive] [bit] NOT NULL,
 CONSTRAINT [PK_Resource] PRIMARY KEY CLUSTERED 
(
	[ResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleMaster]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleMaster](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleDescription] [nvarchar](100) NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_RoleMaster] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RootCauseMaster]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RootCauseMaster](
	[RootCauseId] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](100) NULL,
	[Isdelete] [bit] NULL,
 CONSTRAINT [PK_RootCauseMaster] PRIMARY KEY CLUSTERED 
(
	[RootCauseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StatusMaster]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StatusMaster](
	[StatusId] [int] IDENTITY(1,1) NOT NULL,
	[StatusDescription] [nvarchar](100) NULL,
	[statusOrder] [int] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_StatusMaster] PRIMARY KEY CLUSTERED 
(
	[StatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tickets]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tickets](
	[TicketId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](4000) NULL,
	[TDescription] [nvarchar](4000) NULL,
	[CreatedBy] [int] NULL,
	[StatusId] [int] NULL,
	[Createddate] [datetime] NULL,
	[AssignedTo] [int] NULL,
	[PriorityId] [int] NULL,
	[TypeId] [int] NULL,
	[ApplicationId] [int] NULL,
	[ModuleID] [int] NULL,
	[ResponseDeadline] [datetime] NULL,
	[ResolutionDeadline] [datetime] NULL,
	[RootCauseId] [int] NULL,
	[Comments] [nvarchar](4000) NULL,
	[UpdatedBy] [int] NULL,
	[LastModifiedon] [datetime] NULL,
 CONSTRAINT [PK_Tickets] PRIMARY KEY CLUSTERED 
(
	[TicketId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TodoList]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TodoList](
	[TodoId] [int] IDENTITY(1,1) NOT NULL,
	[Titile] [nvarchar](500) NULL,
	[Description] [nvarchar](2000) NULL,
	[actionDate] [date] NULL,
	[IsActive] [bit] NULL,
	[Userid] [int] NULL,
 CONSTRAINT [PK_TodoList] PRIMARY KEY CLUSTERED 
(
	[TodoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Trade]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trade](
	[TradeId] [int] IDENTITY(1,1) NOT NULL,
	[TradeFileID] [int] NULL,
	[TradeTypeId] [int] NULL,
	[ClientTradeRef] [nvarchar](200) NULL,
	[SecurityCode] [nvarchar](200) NULL,
	[Units] [nvarchar](200) NULL,
	[Price] [decimal](18, 2) NULL,
	[Brokerage] [int] NULL,
	[BrokerageGST] [decimal](18, 2) NULL,
	[NetSettlementValue] [decimal](18, 2) NULL,
	[GrossSettlementValue] [decimal](18, 2) NULL,
 CONSTRAINT [PK_Trade] PRIMARY KEY CLUSTERED 
(
	[TradeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TradeValidationError]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TradeValidationError](
	[TradeValidationErrorID] [int] IDENTITY(1,1) NOT NULL,
	[TradeId] [int] NULL,
	[ErrorCode] [nvarchar](300) NULL,
 CONSTRAINT [PK_TradeValidationError] PRIMARY KEY CLUSTERED 
(
	[TradeValidationErrorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrainingHistory]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrainingHistory](
	[Userid] [int] NULL,
	[YearNo] [int] NULL,
	[TrainigId] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrainingMaster]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrainingMaster](
	[TrianingId] [int] IDENTITY(1,1) NOT NULL,
	[Details] [nchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TypeMaster]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeMaster](
	[TypeId] [int] IDENTITY(1,1) NOT NULL,
	[TypeDescription] [nvarchar](100) NULL,
	[TypeOrder] [int] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_TypeMaster] PRIMARY KEY CLUSTERED 
(
	[TypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Entity_Link]  WITH CHECK ADD  CONSTRAINT [FK_Entity_Link_Entity] FOREIGN KEY([Parent_Entity_ID])
REFERENCES [dbo].[Entity] ([ID])
GO
ALTER TABLE [dbo].[Entity_Link] CHECK CONSTRAINT [FK_Entity_Link_Entity]
GO
ALTER TABLE [dbo].[Entity_Link]  WITH CHECK ADD  CONSTRAINT [FK_Entity_Link_Entity1] FOREIGN KEY([Child_Entity_ID])
REFERENCES [dbo].[Entity] ([ID])
GO
ALTER TABLE [dbo].[Entity_Link] CHECK CONSTRAINT [FK_Entity_Link_Entity1]
GO
ALTER TABLE [dbo].[FileUpload]  WITH CHECK ADD  CONSTRAINT [FK_FileUpload_Tickets] FOREIGN KEY([TicketId])
REFERENCES [dbo].[Tickets] ([TicketId])
GO
ALTER TABLE [dbo].[FileUpload] CHECK CONSTRAINT [FK_FileUpload_Tickets]
GO
ALTER TABLE [dbo].[FinancialEventSourceFlatFile]  WITH CHECK ADD  CONSTRAINT [FK_FinancialEventSourceFlatFile_FinancialEventSource] FOREIGN KEY([FinancialEventSourceId])
REFERENCES [dbo].[FinancialEventSource] ([FinancialEventSourceId])
GO
ALTER TABLE [dbo].[FinancialEventSourceFlatFile] CHECK CONSTRAINT [FK_FinancialEventSourceFlatFile_FinancialEventSource]
GO
ALTER TABLE [dbo].[FinancialEventSourceSwift]  WITH CHECK ADD  CONSTRAINT [FK_FinancialEventSourceSwift_FinancialEventSource] FOREIGN KEY([FinancialEventSourceId])
REFERENCES [dbo].[FinancialEventSource] ([FinancialEventSourceId])
GO
ALTER TABLE [dbo].[FinancialEventSourceSwift] CHECK CONSTRAINT [FK_FinancialEventSourceSwift_FinancialEventSource]
GO
ALTER TABLE [dbo].[MenuItems]  WITH CHECK ADD  CONSTRAINT [FK_MenuItems_Menus] FOREIGN KEY([MenuId])
REFERENCES [dbo].[Menus] ([MenuId])
GO
ALTER TABLE [dbo].[MenuItems] CHECK CONSTRAINT [FK_MenuItems_Menus]
GO
ALTER TABLE [dbo].[MenuPermission]  WITH CHECK ADD  CONSTRAINT [FK_MenuPermission_MenuItems] FOREIGN KEY([MenuItemId])
REFERENCES [dbo].[MenuItems] ([MenuItemId])
GO
ALTER TABLE [dbo].[MenuPermission] CHECK CONSTRAINT [FK_MenuPermission_MenuItems]
GO
ALTER TABLE [dbo].[MenuPermission]  WITH CHECK ADD  CONSTRAINT [FK_MenuPermission_RoleMaster] FOREIGN KEY([RoleId])
REFERENCES [dbo].[RoleMaster] ([RoleId])
GO
ALTER TABLE [dbo].[MenuPermission] CHECK CONSTRAINT [FK_MenuPermission_RoleMaster]
GO
ALTER TABLE [dbo].[ModuleMaster]  WITH CHECK ADD  CONSTRAINT [FK_ModuleMaster_ModuleMaster] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[ModuleMaster] ([ModuleId])
GO
ALTER TABLE [dbo].[ModuleMaster] CHECK CONSTRAINT [FK_ModuleMaster_ModuleMaster]
GO
ALTER TABLE [dbo].[Tickets]  WITH CHECK ADD  CONSTRAINT [FK_Tickets_ApplicationMaster] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[ApplicationMaster] ([ApplicationId])
GO
ALTER TABLE [dbo].[Tickets] CHECK CONSTRAINT [FK_Tickets_ApplicationMaster]
GO
ALTER TABLE [dbo].[Tickets]  WITH CHECK ADD  CONSTRAINT [FK_Tickets_ModuleMaster] FOREIGN KEY([ModuleID])
REFERENCES [dbo].[ModuleMaster] ([ModuleId])
GO
ALTER TABLE [dbo].[Tickets] CHECK CONSTRAINT [FK_Tickets_ModuleMaster]
GO
ALTER TABLE [dbo].[Tickets]  WITH CHECK ADD  CONSTRAINT [FK_Tickets_PriorityMaster] FOREIGN KEY([PriorityId])
REFERENCES [dbo].[PriorityMaster] ([PriorityId])
GO
ALTER TABLE [dbo].[Tickets] CHECK CONSTRAINT [FK_Tickets_PriorityMaster]
GO
ALTER TABLE [dbo].[Tickets]  WITH CHECK ADD  CONSTRAINT [FK_Tickets_Resource] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Resource] ([ResourceId])
GO
ALTER TABLE [dbo].[Tickets] CHECK CONSTRAINT [FK_Tickets_Resource]
GO
ALTER TABLE [dbo].[Tickets]  WITH CHECK ADD  CONSTRAINT [FK_Tickets_RootCauseMaster1] FOREIGN KEY([RootCauseId])
REFERENCES [dbo].[RootCauseMaster] ([RootCauseId])
GO
ALTER TABLE [dbo].[Tickets] CHECK CONSTRAINT [FK_Tickets_RootCauseMaster1]
GO
ALTER TABLE [dbo].[Tickets]  WITH CHECK ADD  CONSTRAINT [FK_Tickets_StatusMaster] FOREIGN KEY([StatusId])
REFERENCES [dbo].[StatusMaster] ([StatusId])
GO
ALTER TABLE [dbo].[Tickets] CHECK CONSTRAINT [FK_Tickets_StatusMaster]
GO
ALTER TABLE [dbo].[Tickets]  WITH CHECK ADD  CONSTRAINT [FK_Tickets_TypeMaster] FOREIGN KEY([TypeId])
REFERENCES [dbo].[TypeMaster] ([TypeId])
GO
ALTER TABLE [dbo].[Tickets] CHECK CONSTRAINT [FK_Tickets_TypeMaster]
GO
ALTER TABLE [dbo].[TradeValidationError]  WITH CHECK ADD  CONSTRAINT [FK_TradeValidationError_Trade] FOREIGN KEY([TradeId])
REFERENCES [dbo].[Trade] ([TradeId])
GO
ALTER TABLE [dbo].[TradeValidationError] CHECK CONSTRAINT [FK_TradeValidationError_Trade]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_RoleMaster] FOREIGN KEY([RoleId])
REFERENCES [dbo].[RoleMaster] ([RoleId])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_RoleMaster]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_UserMaster] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserMaster] ([UserId])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_UserMaster]
GO
/****** Object:  StoredProcedure [dbo].[Fact]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Fact]
(
@Number Integer,
@RetVal Integer OUTPUT
)
AS
DECLARE @In Integer
DECLARE @Out Integer
IF @Number != 1
BEGIN
SELECT @In = @Number - 1
EXEC Fact @In, @Out OUTPUT -- Same stored procedure has been called again(Recursively)
SELECT @RetVal = @Number * @Out
END
ELSE
BEGIN
SELECT @RetVal = 1
END
RETURN


GO
/****** Object:  StoredProcedure [dbo].[INS]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure  [dbo].[INS]                              
(                                                          
   @Query  Varchar(MAX)                                                          
)                              

AS                              

	   Set nocount ON                  

DEclare @WithStrINdex as INT                            
DEclare @WhereStrINdex as INT                            
DEclare @INDExtouse as INT                            

Declare @SchemaAndTAble VArchar(270)                            
Declare @Schema_name  varchar(30)                            
Declare @Table_name  varchar(240)                            
declare @Condition  Varchar(MAX)                             

SET @WithStrINdex=0                            

SELECT @WithStrINdex=CHARINDEX('With',@Query )                            
, @WhereStrINdex=CHARINDEX('WHERE', @Query)                            

IF(@WithStrINdex!=0)                            
Select @INDExtouse=@WithStrINdex                            
ELSE                            
Select @INDExtouse=@WhereStrINdex                            

Select @SchemaAndTAble=Left (@Query,@INDExtouse-1)                                                     
select @SchemaAndTAble=Ltrim (Rtrim( @SchemaAndTAble))                            

Select @Schema_name= Left (@SchemaAndTAble, CharIndex('.',@SchemaAndTAble )-1)                            
,      @Table_name = SUBSTRING(  @SchemaAndTAble , CharIndex('.',@SchemaAndTAble )+1,LEN(@SchemaAndTAble) )                            

,      @CONDITION=SUBSTRING(@Query,@WhereStrINdex+6,LEN(@Query))--27+6                            


Declare   @COLUMNS  table (Row_number SmallINT , Column_Name VArchar(Max) )                              
Declare @CONDITIONS as varchar(MAX)                              
Declare @Total_Rows as SmallINT                              
Declare @Counter as SmallINT              

declare @ComaCol as varchar(max)            
select @ComaCol=''                   

Set @Counter=1                              
set @CONDITIONS=''                              

INsert INTO @COLUMNS                              
Select  Row_number()Over (Order by ORDINAL_POSITION ) [Count] ,Column_Name FRom INformation_schema.columns Where Table_schema=@Schema_name                              
And table_name=@Table_name         
and Column_Name not in ('SyncDestination','PendingSyncDestination' ,'SkuID','SaleCreditedto')                  

select @Total_Rows= Count(1) FRom  @COLUMNS                              

			 Select @Table_name= '['+@Table_name+']'                      

			 Select @Schema_name='['+@Schema_name+']'                      

While (@Counter<=@Total_Rows )                              
begin                               
--PRINT @Counter                              

	select @ComaCol= @ComaCol+'['+Column_Name+'],'            
	FROM @COLUMNS                              
Where [Row_number]=@Counter                          

select @CONDITIONS=@CONDITIONS+ ' +Case When ['+Column_Name+'] is null then ''Null'' Else ''''''''+                              

 Replace( Convert(varchar(Max),['+Column_Name+']  ) ,'''''''',''''  )                              

  +'''''''' end+'+''','''                              

FROM @COLUMNS                              
Where [Row_number]=@Counter                              

SET @Counter=@Counter+1                              

End                              

select @CONDITIONS=Right(@CONDITIONS,LEN(@CONDITIONS)-2)                              

select @CONDITIONS=LEFT(@CONDITIONS,LEN(@CONDITIONS)-4)              
select @ComaCol= substring (@ComaCol,0,  len(@ComaCol) )                            

select @CONDITIONS= '''INSERT INTO '+@Schema_name+'.'+@Table_name+ '('+@ComaCol+')' +' Values( '+'''' + '+'+@CONDITIONS                              

select @CONDITIONS=@CONDITIONS+'+'+ ''')'''                              

Select @CONDITIONS= 'Select  '+@CONDITIONS +'FRom  ' +@Schema_name+'.'+@Table_name+' With(NOLOCK) ' + ' Where '+@Condition                              
print(@CONDITIONS)                              
Exec(@CONDITIONS)

GO
/****** Object:  StoredProcedure [dbo].[Proc_GetTickets]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURe [dbo].[Proc_GetTickets]
(
@Ticketid int
)
AS BEGIN
	SELECT * FROM tickets WHERE ticketid=@Ticketid
END

GO
/****** Object:  StoredProcedure [dbo].[sp_getDEtails]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_getDEtails]

AS
BEGIN

Begin Transaction
Begin Try
SET NOCOUNT ON;  
DECLARE @maxval int
DECLARE  @MyTable Table
(
myid int identity(100,1),
Numbers int
)

 SET @maxval=0
while @maxval<1500
BEGIN
INSERT into @MyTable values(@maxval)
SET @maxval=@maxval+1
END
--print @maxval
select * from @MyTable
SET NOCOUNT OFF;  
End Try
Begin catch
If @@TRANCOUNT>0
   ROLLBACK TRANSACTION;
RAISERROR('Failed to insert',16,0);
End catch
If @@TRANCOUNT>0
   Commit TRANSACTION;
end


GO
/****** Object:  StoredProcedure [dbo].[SP_LogException]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_LogException]
(
	@xmlException  nvarchar(max) 
	)
AS
BEGIN
BEGIN TRANSACTION;
BEGIN TRY
	DECLARE @idoc INT
	EXEC sp_xml_preparedocument @idoc OUTPUT, @xmlException
	INSERT INTO dbo.Exception
           (Message,StackTrace,UserId,ExceptionDateTime,InnerExceptionmessage,ControllerName,ActionName) 
		   
		   SELECT Message,StackTrace,UserId,GETDATE(),InnerExceptionmessage,ControllerName,ActionName FROM  OPENXML(@idoc, '/ExceptionModel',2) with (	  
		  	Message nvarchar(150) ,	StackTrace nvarchar(4000) ,	UserId Nvarchar(5) ,	ExceptionDateTime datetime ,	InnerExceptionmessage nvarchar(4000) ,
	ControllerName nvarchar(50) ,	ActionName nvarchar(50) )
EXEC sp_xml_removedocument @idoc  	 


END TRY
BEGIN CATCH
	DECLARE @ErrorMessage NVARCHAR(4000);
	DECLARE @ErrorSeverity INT;
	DECLARE @ErrorState INT;
	SELECT 
        @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE();
		RAISERROR (@ErrorMessage, 
					   @ErrorSeverity,
					   @ErrorState 
					   );

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
END CATCH;

IF @@TRANCOUNT > 0
  COMMIT TRANSACTION;

  END

GO
/****** Object:  StoredProcedure [dbo].[USP_AuthenticateUser]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_AuthenticateUser]
(
@UserId Nvarchar(150),
@password Nvarchar(150)
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT ResourceId,FName,Lname,Email, Lname+', '+FName As 'Username',dbo.Ufn_RoleDEscription(Roles) 'Roles' FROM RESOURCE WHERE  Email=@UserId and ASCII(@password) =ASCII(Pwd)
	SET NOCOUNT OFF;
END

GO
/****** Object:  StoredProcedure [dbo].[usp_FinancialEventSource]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[usp_FinancialEventSource]
as
 BEGIN
 DECLARE @rank INT = 0;

WHILE @rank < 25000 --(you can use Select count (*) from your table name as well)
BEGIN
   DECLARE @FromDate DATETIME = DATEADD(DAY, -720, GETDATE()) -- 2 years back
   DECLARE @ToDate   DATETIME = DATEADD(DAY, -1, GETDATE()) -- until yesterday

   DECLARE @Seconds INT = DATEDIFF(SECOND, @FromDate, @ToDate)
   DECLARE @Random INT = ROUND(((@Seconds-1) * RAND()), 0)
   DECLARE @Milliseconds INT = ROUND((999 * RAND()), 0)

   insert into FinancialEventSource values(@rank+FLOOR(Rand()*((99-1)-1)),DATEADD(MILLISECOND, @Milliseconds, DATEADD(SECOND, @Random, @FromDate)))
/*
update FXTrade
Set SettlementDate = DATEADD(MILLISECOND, @Milliseconds, DATEADD(SECOND, @Random, @FromDate))
WHERE FXTradeId = @rank ANd cast(Tradedate as date)<>cast(SettlementDate as date)
*/
   SET @rank = @rank + 1;       
END
END

Declare @maxrun int
Set @maxrun=0
While @maxrun<50
BEGIN
EXEC usp_mlprun
SET @maxrun=@maxrun+1
END


GO
/****** Object:  StoredProcedure [dbo].[usp_FinancialEventSourceFlatFile]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[usp_FinancialEventSourceFlatFile]
as
 BEGIN
 DECLARE @rank INT = 0;

WHILE @rank < 25000 --(you can use Select count (*) from your table name as well)
BEGIN

   insert into FinancialEventSourceFlatFile(FinancialEventSourceId,FileName) values(FLOOR(Rand()*((25000-1)-1)),'FileName'+cast(@rank as nvarchar))
/*
update FXTrade
Set SettlementDate = DATEADD(MILLISECOND, @Milliseconds, DATEADD(SECOND, @Random, @FromDate))
WHERE FXTradeId = @rank ANd cast(Tradedate as date)<>cast(SettlementDate as date)
*/
   SET @rank = @rank + 1;       
END
END

Declare @maxrun int
Set @maxrun=0
While @maxrun<50
BEGIN
EXEC usp_mlprun
SET @maxrun=@maxrun+1
END


GO
/****** Object:  StoredProcedure [dbo].[usp_FinancialEventSourceSwift]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[usp_FinancialEventSourceSwift]
as
 BEGIN
 DECLARE @rank INT = 0;

WHILE @rank < 25000 --(you can use Select count (*) from your table name as well)
BEGIN

   insert into FinancialEventSourceSwift(FinancialEventSourceId,Message) values(FLOOR(Rand()*((25000-1)-1)),'FileName'+cast(@rank as nvarchar))
/*
update FXTrade
Set SettlementDate = DATEADD(MILLISECOND, @Milliseconds, DATEADD(SECOND, @Random, @FromDate))
WHERE FXTradeId = @rank ANd cast(Tradedate as date)<>cast(SettlementDate as date)
*/
   SET @rank = @rank + 1;       
END
END

Declare @maxrun int
Set @maxrun=0
While @maxrun<50
BEGIN
EXEC usp_mlprun
SET @maxrun=@maxrun+1
END


GO
/****** Object:  StoredProcedure [dbo].[USP_GET_ApplicationeMASTER]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_GET_ApplicationeMASTER]	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * from ApplicationMaster order by statusOrder
	SET NOCOUNT OFF;
END

GO
/****** Object:  StoredProcedure [dbo].[USP_GET_ModuleMASTER]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_GET_ModuleMASTER]	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * from ModuleMaster order by statusOrder
	SET NOCOUNT OFF;
END

GO
/****** Object:  StoredProcedure [dbo].[USP_GET_PRIORITYMASTER]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_GET_PRIORITYMASTER]	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * from PriorityMaster
	SET NOCOUNT OFF;
END

GO
/****** Object:  StoredProcedure [dbo].[USP_GET_Resource]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_GET_Resource]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT resourceid, RTRIM(Lname)+', '+RTRIM(Fname)  as'ResourceName' from Resource 
	SET NOCOUNT OFF;
END

GO
/****** Object:  StoredProcedure [dbo].[USP_GET_RootCause]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_GET_RootCause]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT  ROOTCAUSEID, DESCRIPTION FROM ROOTCAUSEMASTER ORDER BY ROOTCAUSEID ASC
	SET NOCOUNT OFF;
END

GO
/****** Object:  StoredProcedure [dbo].[USP_GET_StatusMASTER]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_GET_StatusMASTER]	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * from StatusMaster
	SET NOCOUNT OFF;
END

GO
/****** Object:  StoredProcedure [dbo].[USP_GETTypdMASTER]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_GETTypdMASTER]	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * from TypeMaster order by typeOrder
	SET NOCOUNT OFF;
END

GO
/****** Object:  StoredProcedure [dbo].[USP_INSERT_RootCause]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_INSERT_RootCause]
(

@Description Nvarchar(200)

)
AS
BEGIN
	INSERT INTO  RootCauseMaster(Description,Isdelete) VALUES(@Description,1)	
END

GO
/****** Object:  StoredProcedure [dbo].[usp_mlprun]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[usp_mlprun]
as
 BEGIN
 DECLARE @rank INT = 0;

WHILE @rank < 15000 --(you can use Select count (*) from your table name as well)
BEGIN
   DECLARE @FromDate DATETIME = DATEADD(DAY, -720, GETDATE()) -- 2 years back
   DECLARE @ToDate   DATETIME = DATEADD(DAY, -1, GETDATE()) -- until yesterday

   DECLARE @Seconds INT = DATEDIFF(SECOND, @FromDate, @ToDate)
   DECLARE @Random INT = ROUND(((@Seconds-1) * RAND()), 0)
   DECLARE @Milliseconds INT = ROUND((999 * RAND()), 0)

update FXTrade
Set SettlementDate = DATEADD(MILLISECOND, @Milliseconds, DATEADD(SECOND, @Random, @FromDate))
WHERE FXTradeId = @rank ANd cast(Tradedate as date)<>cast(SettlementDate as date)
   SET @rank = @rank + 1;       
END
END

GO
/****** Object:  StoredProcedure [dbo].[USP_UPDATE_RootCause]    Script Date: 05-05-2020 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_UPDATE_RootCause]
(
@RootCauseId INT,
@Description Nvarchar(200)

)
AS
BEGIN
	UPDATE RootCauseMaster SET Description=@Description WHERE RootCauseId=@RootCauseId	
END
GO 
 
GO
SET IDENTITY_INSERT [dbo].[Menus] ON 
GO
INSERT [dbo].[Menus] ([MenuId], [DisplayName]) VALUES (1, N'Home')
GO
INSERT [dbo].[Menus] ([MenuId], [DisplayName]) VALUES (2, N'Admin Setup')
GO
INSERT [dbo].[Menus] ([MenuId], [DisplayName]) VALUES (3, N'Ticket Summary')
GO
INSERT [dbo].[Menus] ([MenuId], [DisplayName]) VALUES (4, N'Add User')
GO
SET IDENTITY_INSERT [dbo].[Menus] OFF
GO
SET IDENTITY_INSERT [dbo].[MenuItems] ON 
GO
INSERT [dbo].[MenuItems] ([MenuItemId], [MenuId], [link], [parentmenuid], [Sortorder]) VALUES (1, 1, N'Home', 0, 1)
GO
INSERT [dbo].[MenuItems] ([MenuItemId], [MenuId], [link], [parentmenuid], [Sortorder]) VALUES (2, 2, N'Admin', 0, 1)
GO
INSERT [dbo].[MenuItems] ([MenuItemId], [MenuId], [link], [parentmenuid], [Sortorder]) VALUES (3, 3, N'Ticket', 0, 1)
GO
INSERT [dbo].[MenuItems] ([MenuItemId], [MenuId], [link], [parentmenuid], [Sortorder]) VALUES (4, 4, N'adduser', NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[MenuItems] OFF
GO
SET IDENTITY_INSERT [dbo].[RoleMaster] ON 
GO
INSERT [dbo].[RoleMaster] ([RoleId], [RoleDescription], [IsActive]) VALUES (1, N'Admin', 0)
GO
INSERT [dbo].[RoleMaster] ([RoleId], [RoleDescription], [IsActive]) VALUES (2, N'BasicUser', 0)
GO
SET IDENTITY_INSERT [dbo].[RoleMaster] OFF
GO
INSERT [dbo].[MenuPermission] ([MenuItemId], [RoleId]) VALUES (1, 1)
GO
INSERT [dbo].[MenuPermission] ([MenuItemId], [RoleId]) VALUES (1, 2)
GO
INSERT [dbo].[MenuPermission] ([MenuItemId], [RoleId]) VALUES (2, 1)
GO
INSERT [dbo].[MenuPermission] ([MenuItemId], [RoleId]) VALUES (3, 1)
GO
INSERT [dbo].[MenuPermission] ([MenuItemId], [RoleId]) VALUES (3, 2)
GO
INSERT [dbo].[MenuPermission] ([MenuItemId], [RoleId]) VALUES (4, 1)
GO
SET IDENTITY_INSERT [dbo].[UserMaster] ON 
GO
INSERT [dbo].[UserMaster] ([UserId], [FName], [LName], [Email], [IsDeleted]) VALUES (1, N'Narendra', N'Gurram', N'naren.7090@testmail.com', 0)
GO
INSERT [dbo].[UserMaster] ([UserId], [FName], [LName], [Email], [IsDeleted]) VALUES (2, N'Naresh', N'Kunchem', N'naresh.k@testmail.com', 0)
GO
INSERT [dbo].[UserMaster] ([UserId], [FName], [LName], [Email], [IsDeleted]) VALUES (3, N'Manish', N'Tiwary', N'Ma.am@yahoo.com', 0)
GO
SET IDENTITY_INSERT [dbo].[UserMaster] OFF
GO
SET IDENTITY_INSERT [dbo].[UserRoles] ON 
GO
INSERT [dbo].[UserRoles] ([Mappingid], [UserId], [RoleId]) VALUES (1, 1, 1)
GO
INSERT [dbo].[UserRoles] ([Mappingid], [UserId], [RoleId]) VALUES (3, 2, 2)
GO
INSERT [dbo].[UserRoles] ([Mappingid], [UserId], [RoleId]) VALUES (4, 3, 2)
GO
SET IDENTITY_INSERT [dbo].[UserRoles] OFF
GO
SET IDENTITY_INSERT [dbo].[ApplicationMaster] ON 
GO
INSERT [dbo].[ApplicationMaster] ([ApplicationId], [ApplicationName], [statusOrder], [IsDeleted]) VALUES (1, N'Tariff Manager app', NULL, 0)
GO
INSERT [dbo].[ApplicationMaster] ([ApplicationId], [ApplicationName], [statusOrder], [IsDeleted]) VALUES (2, N'Floor System', 2, 0)
GO
INSERT [dbo].[ApplicationMaster] ([ApplicationId], [ApplicationName], [statusOrder], [IsDeleted]) VALUES (3, N'Business Insurance', 3, 0)
GO
INSERT [dbo].[ApplicationMaster] ([ApplicationId], [ApplicationName], [statusOrder], [IsDeleted]) VALUES (4, N'Bank of america', NULL, 0)
GO
SET IDENTITY_INSERT [dbo].[ApplicationMaster] OFF
GO
SET IDENTITY_INSERT [dbo].[ModuleMaster] ON 
GO
INSERT [dbo].[ModuleMaster] ([ModuleId], [ModuleName], [statusOrder], [IsDeleted]) VALUES (1, N'QC', 0, 0)
GO
INSERT [dbo].[ModuleMaster] ([ModuleId], [ModuleName], [statusOrder], [IsDeleted]) VALUES (2, N'Production', 1, 0)
GO
INSERT [dbo].[ModuleMaster] ([ModuleId], [ModuleName], [statusOrder], [IsDeleted]) VALUES (3, N'Final Packing', 2, 0)
GO
SET IDENTITY_INSERT [dbo].[ModuleMaster] OFF
GO
SET IDENTITY_INSERT [dbo].[PriorityMaster] ON 
GO
INSERT [dbo].[PriorityMaster] ([PriorityId], [PriorityDescription], [PriorityOrder], [IsDeleted]) VALUES (1, N'Critical', 1, 0)
GO
INSERT [dbo].[PriorityMaster] ([PriorityId], [PriorityDescription], [PriorityOrder], [IsDeleted]) VALUES (2, N'High', 2, 0)
GO
INSERT [dbo].[PriorityMaster] ([PriorityId], [PriorityDescription], [PriorityOrder], [IsDeleted]) VALUES (3, N'Medium', 3, 0)
GO
INSERT [dbo].[PriorityMaster] ([PriorityId], [PriorityDescription], [PriorityOrder], [IsDeleted]) VALUES (4, N'Low', 4, 0)
GO
SET IDENTITY_INSERT [dbo].[PriorityMaster] OFF
GO
SET IDENTITY_INSERT [dbo].[Resource] ON 
GO
INSERT [dbo].[Resource] ([ResourceId], [FName], [Lname], [Email], [Pwd], [Roles], [Isactive]) VALUES (1, N'Narendra', N'Gurram', N'naren.7090@testmail.com', N'1234', N'1', 1)
GO
INSERT [dbo].[Resource] ([ResourceId], [FName], [Lname], [Email], [Pwd], [Roles], [Isactive]) VALUES (2, N'Naresh', N'K', N'naresh.k@testmail.com', N'1234', N'2', 1)
GO
INSERT [dbo].[Resource] ([ResourceId], [FName], [Lname], [Email], [Pwd], [Roles], [Isactive]) VALUES (3, N'Ramesh', N'goli', N'ramsh.goli@testmail.com', N'1234', N'2', 1)
GO
INSERT [dbo].[Resource] ([ResourceId], [FName], [Lname], [Email], [Pwd], [Roles], [Isactive]) VALUES (4, N'Mango', N'apple', N'Mango.apple@testmail.com', N'1234', N'2', 1)
GO
INSERT [dbo].[Resource] ([ResourceId], [FName], [Lname], [Email], [Pwd], [Roles], [Isactive]) VALUES (5, N'boris', N'john', N'boris.john@testmail.com', N'1234', N'2', 1)
GO
INSERT [dbo].[Resource] ([ResourceId], [FName], [Lname], [Email], [Pwd], [Roles], [Isactive]) VALUES (6, N'abc', N'deb', N'abc.deb@testmail.com', N'1234', N'2', 1)
GO
SET IDENTITY_INSERT [dbo].[Resource] OFF
GO
SET IDENTITY_INSERT [dbo].[RootCauseMaster] ON 
GO
INSERT [dbo].[RootCauseMaster] ([RootCauseId], [Description], [Isdelete]) VALUES (1056, N'Hello World!!', 1)
GO
INSERT [dbo].[RootCauseMaster] ([RootCauseId], [Description], [Isdelete]) VALUES (1057, N'testing routing', 1)
GO
INSERT [dbo].[RootCauseMaster] ([RootCauseId], [Description], [Isdelete]) VALUES (1058, N'Welcome back', 1)
GO
INSERT [dbo].[RootCauseMaster] ([RootCauseId], [Description], [Isdelete]) VALUES (1059, N'Testing Async', 1)
GO
INSERT [dbo].[RootCauseMaster] ([RootCauseId], [Description], [Isdelete]) VALUES (1060, N'Some working', 1)
GO
INSERT [dbo].[RootCauseMaster] ([RootCauseId], [Description], [Isdelete]) VALUES (1061, N'Great working', 1)
GO
SET IDENTITY_INSERT [dbo].[RootCauseMaster] OFF
GO
SET IDENTITY_INSERT [dbo].[StatusMaster] ON 
GO
INSERT [dbo].[StatusMaster] ([StatusId], [StatusDescription], [statusOrder], [IsDeleted]) VALUES (1, N'Open', 1, 0)
GO
INSERT [dbo].[StatusMaster] ([StatusId], [StatusDescription], [statusOrder], [IsDeleted]) VALUES (2, N'Closed', 2, 0)
GO
INSERT [dbo].[StatusMaster] ([StatusId], [StatusDescription], [statusOrder], [IsDeleted]) VALUES (3, N'InProgress', 3, 0)
GO
INSERT [dbo].[StatusMaster] ([StatusId], [StatusDescription], [statusOrder], [IsDeleted]) VALUES (4, N'Hold', 4, 0)
GO
INSERT [dbo].[StatusMaster] ([StatusId], [StatusDescription], [statusOrder], [IsDeleted]) VALUES (5, N'ReOpen', 5, 0)
GO
SET IDENTITY_INSERT [dbo].[StatusMaster] OFF
GO
SET IDENTITY_INSERT [dbo].[TypeMaster] ON 
GO
INSERT [dbo].[TypeMaster] ([TypeId], [TypeDescription], [TypeOrder], [IsDeleted]) VALUES (1, N'Issue', 1, 0)
GO
INSERT [dbo].[TypeMaster] ([TypeId], [TypeDescription], [TypeOrder], [IsDeleted]) VALUES (2, N'Enhancement', 2, 0)
GO
INSERT [dbo].[TypeMaster] ([TypeId], [TypeDescription], [TypeOrder], [IsDeleted]) VALUES (3, N'Defect', 3, 0)
GO
SET IDENTITY_INSERT [dbo].[TypeMaster] OFF
GO
SET IDENTITY_INSERT [dbo].[Tickets] ON 
GO
INSERT [dbo].[Tickets] ([TicketId], [Title], [TDescription], [CreatedBy], [StatusId], [Createddate], [AssignedTo], [PriorityId], [TypeId], [ApplicationId], [ModuleID], [ResponseDeadline], [ResolutionDeadline], [RootCauseId], [Comments], [UpdatedBy], [LastModifiedon]) VALUES (1, N'Testing my First Ng app', N'Testing my First Ng app Description', 2, 3, CAST(N'2017-08-06T00:00:00.000' AS DateTime), 1, 1, 1, 1, 1, CAST(N'2018-10-13T08:40:50.360' AS DateTime), CAST(N'2018-10-03T00:00:00.000' AS DateTime), 1056, N'Testing comments updated', NULL, NULL)
GO
INSERT [dbo].[Tickets] ([TicketId], [Title], [TDescription], [CreatedBy], [StatusId], [Createddate], [AssignedTo], [PriorityId], [TypeId], [ApplicationId], [ModuleID], [ResponseDeadline], [ResolutionDeadline], [RootCauseId], [Comments], [UpdatedBy], [LastModifiedon]) VALUES (2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL)
GO
INSERT [dbo].[Tickets] ([TicketId], [Title], [TDescription], [CreatedBy], [StatusId], [Createddate], [AssignedTo], [PriorityId], [TypeId], [ApplicationId], [ModuleID], [ResponseDeadline], [ResolutionDeadline], [RootCauseId], [Comments], [UpdatedBy], [LastModifiedon]) VALUES (3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL)
GO
INSERT [dbo].[Tickets] ([TicketId], [Title], [TDescription], [CreatedBy], [StatusId], [Createddate], [AssignedTo], [PriorityId], [TypeId], [ApplicationId], [ModuleID], [ResponseDeadline], [ResolutionDeadline], [RootCauseId], [Comments], [UpdatedBy], [LastModifiedon]) VALUES (4, N'fourth ticket title work', N'fourth ticket', 2, 3, CAST(N'2015-08-06T00:00:00.000' AS DateTime), 2, 2, 2, 2, 2, CAST(N'2017-05-09T00:31:03.000' AS DateTime), CAST(N'2017-07-06T00:00:00.000' AS DateTime), 1059, N'Working conditions', 1, NULL)
GO
INSERT [dbo].[Tickets] ([TicketId], [Title], [TDescription], [CreatedBy], [StatusId], [Createddate], [AssignedTo], [PriorityId], [TypeId], [ApplicationId], [ModuleID], [ResponseDeadline], [ResolutionDeadline], [RootCauseId], [Comments], [UpdatedBy], [LastModifiedon]) VALUES (5, N'Titile Seek', N'Testing description for 6  ghfghf', 2, 3, CAST(N'2017-06-08T00:00:00.000' AS DateTime), 1, 2, 1, 1, 2, CAST(N'1970-01-01T00:00:00.000' AS DateTime), CAST(N'2018-01-10T00:00:00.000' AS DateTime), 1057, N'testing 03 aug 2017 Comments in progress', 1, NULL)
GO
INSERT [dbo].[Tickets] ([TicketId], [Title], [TDescription], [CreatedBy], [StatusId], [Createddate], [AssignedTo], [PriorityId], [TypeId], [ApplicationId], [ModuleID], [ResponseDeadline], [ResolutionDeadline], [RootCauseId], [Comments], [UpdatedBy], [LastModifiedon]) VALUES (6, N'Testing remove space validations', N'looks like its working now', 1, 1, CAST(N'2013-05-24T02:00:00.000' AS DateTime), 1, 2, 2, 1, 2, CAST(N'2020-03-06T02:00:00.000' AS DateTime), CAST(N'2017-02-08T02:00:00.000' AS DateTime), 1057, N'looks like its working now comments', 1, NULL)
GO
INSERT [dbo].[Tickets] ([TicketId], [Title], [TDescription], [CreatedBy], [StatusId], [Createddate], [AssignedTo], [PriorityId], [TypeId], [ApplicationId], [ModuleID], [ResponseDeadline], [ResolutionDeadline], [RootCauseId], [Comments], [UpdatedBy], [LastModifiedon]) VALUES (7, N'Working ticket failed', N'Working ticket Description', 1, 4, CAST(N'2017-06-07T18:30:00.000' AS DateTime), 1, 3, 1, 2, 2, CAST(N'2017-06-10T18:30:00.000' AS DateTime), CAST(N'2017-06-09T18:30:00.000' AS DateTime), 1057, N'Testing comments now working from linux', 1, NULL)
GO
INSERT [dbo].[Tickets] ([TicketId], [Title], [TDescription], [CreatedBy], [StatusId], [Createddate], [AssignedTo], [PriorityId], [TypeId], [ApplicationId], [ModuleID], [ResponseDeadline], [ResolutionDeadline], [RootCauseId], [Comments], [UpdatedBy], [LastModifiedon]) VALUES (8, N'Working find title', N'Second Ticket Description after moving to Angular 5 Testing  angular interceptor', 1, 1, CAST(N'2017-06-08T17:46:50.497' AS DateTime), 1, 1, 2, 2, 2, CAST(N'2017-06-04T14:00:00.000' AS DateTime), CAST(N'2017-06-19T14:00:00.000' AS DateTime), 1056, N'Comments', NULL, NULL)
GO
INSERT [dbo].[Tickets] ([TicketId], [Title], [TDescription], [CreatedBy], [StatusId], [Createddate], [AssignedTo], [PriorityId], [TypeId], [ApplicationId], [ModuleID], [ResponseDeadline], [ResolutionDeadline], [RootCauseId], [Comments], [UpdatedBy], [LastModifiedon]) VALUES (9, N'testing --nine Ticket title', N'testing --Second Ticket Description after moving to Angular 5', 1, 1, CAST(N'2017-03-07T13:00:00.000' AS DateTime), 1, 1, 1, 1, 2, CAST(N'2017-06-11T03:00:00.000' AS DateTime), CAST(N'2020-08-04T13:00:00.000' AS DateTime), 1057, N'Second Ticket Comments testing', 1, NULL)
GO
INSERT [dbo].[Tickets] ([TicketId], [Title], [TDescription], [CreatedBy], [StatusId], [Createddate], [AssignedTo], [PriorityId], [TypeId], [ApplicationId], [ModuleID], [ResponseDeadline], [ResolutionDeadline], [RootCauseId], [Comments], [UpdatedBy], [LastModifiedon]) VALUES (10, N'Working find title', N'Looks like its working now.', 1, 2, CAST(N'2020-03-03T00:00:00.000' AS DateTime), 1, 1, 2, 2, 2, NULL, CAST(N'2020-05-02T00:00:00.000' AS DateTime), 1056, N'Comments', 1, NULL)
GO
INSERT [dbo].[Tickets] ([TicketId], [Title], [TDescription], [CreatedBy], [StatusId], [Createddate], [AssignedTo], [PriorityId], [TypeId], [ApplicationId], [ModuleID], [ResponseDeadline], [ResolutionDeadline], [RootCauseId], [Comments], [UpdatedBy], [LastModifiedon]) VALUES (11, N'Working find', N'Looks like its working now.', 1, 2, CAST(N'2017-08-03T21:00:47.663' AS DateTime), 1, 1, 2, 2, 2, CAST(N'2017-06-04T14:00:00.000' AS DateTime), CAST(N'2017-06-19T14:00:00.000' AS DateTime), 1056, N'Comments', NULL, NULL)
GO
INSERT [dbo].[Tickets] ([TicketId], [Title], [TDescription], [CreatedBy], [StatusId], [Createddate], [AssignedTo], [PriorityId], [TypeId], [ApplicationId], [ModuleID], [ResponseDeadline], [ResolutionDeadline], [RootCauseId], [Comments], [UpdatedBy], [LastModifiedon]) VALUES (12, N'reactive  working', N'looks like its working now', 1, 1, CAST(N'2017-08-03T00:00:00.000' AS DateTime), 2, 3, 3, 1, 1, CAST(N'2017-06-11T14:00:00.000' AS DateTime), CAST(N'2017-06-11T14:00:00.000' AS DateTime), 1057, N' ', NULL, NULL)
GO
INSERT [dbo].[Tickets] ([TicketId], [Title], [TDescription], [CreatedBy], [StatusId], [Createddate], [AssignedTo], [PriorityId], [TypeId], [ApplicationId], [ModuleID], [ResponseDeadline], [ResolutionDeadline], [RootCauseId], [Comments], [UpdatedBy], [LastModifiedon]) VALUES (15, N'testing create ticket', N'testing create ticket', 2, 1, CAST(N'2020-04-03T05:53:37.303' AS DateTime), 1, 1, 1, 1, 1, CAST(N'2020-04-03T05:53:37.303' AS DateTime), CAST(N'2020-04-03T05:53:37.303' AS DateTime), 1056, N'testing create ticket', 1, CAST(N'2020-04-03T05:53:37.303' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Tickets] OFF
GO
SET IDENTITY_INSERT [dbo].[Entity] ON 
GO
INSERT [dbo].[Entity] ([ID], [Name], [Reference_ID]) VALUES (1, N'abc', 1)
GO
INSERT [dbo].[Entity] ([ID], [Name], [Reference_ID]) VALUES (2, N'efg', 2)
GO
INSERT [dbo].[Entity] ([ID], [Name], [Reference_ID]) VALUES (3, N'xyz', 3)
GO
INSERT [dbo].[Entity] ([ID], [Name], [Reference_ID]) VALUES (4, N'mno', 2)
GO
INSERT [dbo].[Entity] ([ID], [Name], [Reference_ID]) VALUES (5, N'Yahoo', 4)
GO
INSERT [dbo].[Entity] ([ID], [Name], [Reference_ID]) VALUES (6, N'google', 4)
GO
INSERT [dbo].[Entity] ([ID], [Name], [Reference_ID]) VALUES (7, N'AOL', 4)
GO
INSERT [dbo].[Entity] ([ID], [Name], [Reference_ID]) VALUES (8, N'Facebook', 4)
GO
SET IDENTITY_INSERT [dbo].[Entity] OFF
GO
SET IDENTITY_INSERT [dbo].[Entity_Link] ON 
GO
INSERT [dbo].[Entity_Link] ([Link_ID], [Parent_Entity_ID], [Child_Entity_ID]) VALUES (1, 1, 2)
GO
INSERT [dbo].[Entity_Link] ([Link_ID], [Parent_Entity_ID], [Child_Entity_ID]) VALUES (2, 1, 3)
GO
INSERT [dbo].[Entity_Link] ([Link_ID], [Parent_Entity_ID], [Child_Entity_ID]) VALUES (3, 2, 4)
GO
INSERT [dbo].[Entity_Link] ([Link_ID], [Parent_Entity_ID], [Child_Entity_ID]) VALUES (4, 2, 5)
GO
INSERT [dbo].[Entity_Link] ([Link_ID], [Parent_Entity_ID], [Child_Entity_ID]) VALUES (5, 5, 6)
GO
INSERT [dbo].[Entity_Link] ([Link_ID], [Parent_Entity_ID], [Child_Entity_ID]) VALUES (6, 6, 7)
GO
SET IDENTITY_INSERT [dbo].[Entity_Link] OFF
GO
SET IDENTITY_INSERT [dbo].[Employee] ON 
GO
INSERT [dbo].[Employee] ([empid], [DateofJoining], [managerid], [BusinessUnit]) VALUES (1, CAST(N'2007-02-02' AS Date), 0, 5)
GO
INSERT [dbo].[Employee] ([empid], [DateofJoining], [managerid], [BusinessUnit]) VALUES (2, CAST(N'2004-03-02' AS Date), 1, 5)
GO
INSERT [dbo].[Employee] ([empid], [DateofJoining], [managerid], [BusinessUnit]) VALUES (3, CAST(N'2007-03-02' AS Date), 1, 5)
GO
INSERT [dbo].[Employee] ([empid], [DateofJoining], [managerid], [BusinessUnit]) VALUES (4, CAST(N'2007-03-02' AS Date), 1, 5)
GO
INSERT [dbo].[Employee] ([empid], [DateofJoining], [managerid], [BusinessUnit]) VALUES (5, CAST(N'2007-03-02' AS Date), 2, 5)
GO
INSERT [dbo].[Employee] ([empid], [DateofJoining], [managerid], [BusinessUnit]) VALUES (6, CAST(N'2008-03-02' AS Date), 3, 5)
GO
INSERT [dbo].[Employee] ([empid], [DateofJoining], [managerid], [BusinessUnit]) VALUES (7, CAST(N'2007-03-05' AS Date), 4, 5)
GO
INSERT [dbo].[Employee] ([empid], [DateofJoining], [managerid], [BusinessUnit]) VALUES (8, CAST(N'2008-03-03' AS Date), 5, 5)
GO
INSERT [dbo].[Employee] ([empid], [DateofJoining], [managerid], [BusinessUnit]) VALUES (9, CAST(N'2008-03-06' AS Date), 3, 5)
GO
INSERT [dbo].[Employee] ([empid], [DateofJoining], [managerid], [BusinessUnit]) VALUES (10, CAST(N'2007-03-08' AS Date), 3, 5)
GO
INSERT [dbo].[Employee] ([empid], [DateofJoining], [managerid], [BusinessUnit]) VALUES (11, CAST(N'2009-03-09' AS Date), 5, 5)
GO
INSERT [dbo].[Employee] ([empid], [DateofJoining], [managerid], [BusinessUnit]) VALUES (12, CAST(N'2010-03-10' AS Date), 5, 5)
GO
INSERT [dbo].[Employee] ([empid], [DateofJoining], [managerid], [BusinessUnit]) VALUES (13, CAST(N'2007-03-04' AS Date), 2, 5)
GO
INSERT [dbo].[Employee] ([empid], [DateofJoining], [managerid], [BusinessUnit]) VALUES (14, CAST(N'2007-03-05' AS Date), 3, 5)
GO
INSERT [dbo].[Employee] ([empid], [DateofJoining], [managerid], [BusinessUnit]) VALUES (15, CAST(N'2007-03-03' AS Date), 14, 5)
GO
SET IDENTITY_INSERT [dbo].[Employee] OFF
GO
INSERT [dbo].[EmployeeDetails] ([empid], [FirstName], [LastName], [EmailId], [Address]) VALUES (1, N'Naresh', N'kunchem', N'', N'')
GO
INSERT [dbo].[EmployeeDetails] ([empid], [FirstName], [LastName], [EmailId], [Address]) VALUES (2, N'Narendra', N'Gurram', N'', N'')
GO
INSERT [dbo].[EmployeeDetails] ([empid], [FirstName], [LastName], [EmailId], [Address]) VALUES (3, N'Manish', N'tivari', N'', N'')
GO
INSERT [dbo].[EmployeeDetails] ([empid], [FirstName], [LastName], [EmailId], [Address]) VALUES (4, N'Ramesh', N'manchu', N'', N'')
GO
INSERT [dbo].[EmployeeDetails] ([empid], [FirstName], [LastName], [EmailId], [Address]) VALUES (5, N'Vikas', N'pilli', N'', N'')
GO
INSERT [dbo].[EmployeeDetails] ([empid], [FirstName], [LastName], [EmailId], [Address]) VALUES (6, N'rockky', N'kane', N'', N'')
GO
INSERT [dbo].[EmployeeDetails] ([empid], [FirstName], [LastName], [EmailId], [Address]) VALUES (7, N'peter', N'anderson', N'', N'')
GO
INSERT [dbo].[EmployeeDetails] ([empid], [FirstName], [LastName], [EmailId], [Address]) VALUES (8, N'David', N'warner', N'', N'')
GO
INSERT [dbo].[EmployeeDetails] ([empid], [FirstName], [LastName], [EmailId], [Address]) VALUES (9, N'Rickey', N'ponting', N'', N'')
GO
INSERT [dbo].[EmployeeDetails] ([empid], [FirstName], [LastName], [EmailId], [Address]) VALUES (10, N'lawrence', N'mattew', N'', N'')
GO
INSERT [dbo].[EmployeeDetails] ([empid], [FirstName], [LastName], [EmailId], [Address]) VALUES (11, N'John', N'hayden', N'', N'')
GO
INSERT [dbo].[EmployeeDetails] ([empid], [FirstName], [LastName], [EmailId], [Address]) VALUES (12, N'Nanada', N'Bandari', N'', N'')
GO
INSERT [dbo].[EmployeeDetails] ([empid], [FirstName], [LastName], [EmailId], [Address]) VALUES (13, N'chandu', N'kall', N'', N'')
GO
INSERT [dbo].[EmployeeDetails] ([empid], [FirstName], [LastName], [EmailId], [Address]) VALUES (14, N'Phani', N'tadi', N'', N'')
GO
SET IDENTITY_INSERT [dbo].[TodoList] ON 
GO
INSERT [dbo].[TodoList] ([TodoId], [Titile], [Description], [actionDate], [IsActive], [Userid]) VALUES (1, N'Testing', N'Testing Description', CAST(N'2020-02-21' AS Date), NULL, NULL)
GO
INSERT [dbo].[TodoList] ([TodoId], [Titile], [Description], [actionDate], [IsActive], [Userid]) VALUES (2, N'Testing 2', N'Description Testing 2', CAST(N'2020-02-13' AS Date), NULL, NULL)
GO
INSERT [dbo].[TodoList] ([TodoId], [Titile], [Description], [actionDate], [IsActive], [Userid]) VALUES (1002, N'Prasu', N'Testing the app
', CAST(N'2020-03-18' AS Date), NULL, NULL)
GO
INSERT [dbo].[TodoList] ([TodoId], [Titile], [Description], [actionDate], [IsActive], [Userid]) VALUES (2002, N'Durga ', N'Testing the app for naren', CAST(N'2020-03-04' AS Date), NULL, NULL)
GO
INSERT [dbo].[TodoList] ([TodoId], [Titile], [Description], [actionDate], [IsActive], [Userid]) VALUES (2003, N'test', N'test app', CAST(N'2020-02-05' AS Date), NULL, NULL)
GO
INSERT [dbo].[TodoList] ([TodoId], [Titile], [Description], [actionDate], [IsActive], [Userid]) VALUES (2004, N'core app', N'Testing dot net core in this angular 9', CAST(N'2020-04-16' AS Date), NULL, NULL)
GO
INSERT [dbo].[TodoList] ([TodoId], [Titile], [Description], [actionDate], [IsActive], [Userid]) VALUES (2005, N'tewt', N'fgdfgfd', CAST(N'2020-04-01' AS Date), NULL, NULL)
GO
INSERT [dbo].[TodoList] ([TodoId], [Titile], [Description], [actionDate], [IsActive], [Userid]) VALUES (3005, N'in .net core', N'in .net core', CAST(N'2020-04-08' AS Date), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[TodoList] OFF
GO
select * from Resource where Email='naren.7090@testmail.com'
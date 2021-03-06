USE [master]
GO
/****** Object:  Database [TicketTracker]    Script Date: 1/27/2020 12:51:32 PM ******/
CREATE DATABASE [TicketTracker]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TicketTracker', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\TicketTracker.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'TicketTracker_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\TicketTracker_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [TicketTracker] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TicketTracker].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TicketTracker] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TicketTracker] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TicketTracker] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TicketTracker] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TicketTracker] SET ARITHABORT OFF 
GO
ALTER DATABASE [TicketTracker] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TicketTracker] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TicketTracker] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TicketTracker] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TicketTracker] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TicketTracker] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TicketTracker] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TicketTracker] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TicketTracker] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TicketTracker] SET  DISABLE_BROKER 
GO
ALTER DATABASE [TicketTracker] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TicketTracker] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TicketTracker] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TicketTracker] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TicketTracker] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TicketTracker] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TicketTracker] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TicketTracker] SET RECOVERY FULL 
GO
ALTER DATABASE [TicketTracker] SET  MULTI_USER 
GO
ALTER DATABASE [TicketTracker] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TicketTracker] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TicketTracker] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TicketTracker] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [TicketTracker] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'TicketTracker', N'ON'
GO
ALTER DATABASE [TicketTracker] SET QUERY_STORE = OFF
GO
USE [TicketTracker]
GO
/****** Object:  User [NT AUTHORITY\SYSTEM]    Script Date: 1/27/2020 12:51:32 PM ******/
CREATE USER [NT AUTHORITY\SYSTEM] FOR LOGIN [NT AUTHORITY\SYSTEM] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [naren]    Script Date: 1/27/2020 12:51:32 PM ******/
CREATE USER [naren] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [lcluser]    Script Date: 1/27/2020 12:51:32 PM ******/
CREATE USER [lcluser] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [appuser]    Script Date: 1/27/2020 12:51:32 PM ******/
CREATE USER [appuser] FOR LOGIN [appuser] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [NT AUTHORITY\SYSTEM]
GO
ALTER ROLE [db_owner] ADD MEMBER [naren]
GO
ALTER ROLE [db_owner] ADD MEMBER [lcluser]
GO
ALTER ROLE [db_owner] ADD MEMBER [appuser]
GO
/****** Object:  UserDefinedFunction [dbo].[ufn_CSVToTable]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  UserDefinedFunction [dbo].[Ufn_RoleDEscription]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  UserDefinedFunction [dbo].[ufnGetChilds]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  Table [dbo].[MenuItems]    Script Date: 1/27/2020 12:51:32 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MenuPermission]    Script Date: 1/27/2020 12:51:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MenuPermission](
	[MenuItemId] [int] NOT NULL,
	[RoleId] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menus]    Script Date: 1/27/2020 12:51:32 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserMaster]    Script Date: 1/27/2020 12:51:32 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 1/27/2020 12:51:32 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_user_permissions]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  Table [dbo].[ApplicationMaster]    Script Date: 1/27/2020 12:51:32 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  Table [dbo].[EmployeeDetails]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  Table [dbo].[Entity]    Script Date: 1/27/2020 12:51:32 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Entity_Link]    Script Date: 1/27/2020 12:51:32 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Exception]    Script Date: 1/27/2020 12:51:32 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FileUpload]    Script Date: 1/27/2020 12:51:32 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ModuleMaster]    Script Date: 1/27/2020 12:51:32 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PriorityMaster]    Script Date: 1/27/2020 12:51:32 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Resource]    Script Date: 1/27/2020 12:51:32 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleMaster]    Script Date: 1/27/2020 12:51:32 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RootCauseMaster]    Script Date: 1/27/2020 12:51:32 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StatusMaster]    Script Date: 1/27/2020 12:51:32 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tickets]    Script Date: 1/27/2020 12:51:32 PM ******/
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
	Comments [nvarchar](4000) NULL,
	[UpdatedBy] [int] NULL,
	[LastModifiedon] [datetime] NULL,
 CONSTRAINT [PK_Tickets] PRIMARY KEY CLUSTERED 
(
	[TicketId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TodoList]    Script Date: 1/27/2020 12:51:32 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TypeMaster]    Script Date: 1/27/2020 12:51:32 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
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
/****** Object:  StoredProcedure [dbo].[Fact]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  StoredProcedure [dbo].[INS]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  StoredProcedure [dbo].[Proc_GetTickets]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_getDEtails]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  StoredProcedure [dbo].[SP_LogException]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  StoredProcedure [dbo].[USP_AuthenticateUser]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_FinancialEventSource]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_FinancialEventSourceFlatFile]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_FinancialEventSourceSwift]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  StoredProcedure [dbo].[USP_GET_ApplicationeMASTER]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  StoredProcedure [dbo].[USP_GET_ModuleMASTER]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  StoredProcedure [dbo].[USP_GET_PRIORITYMASTER]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  StoredProcedure [dbo].[USP_GET_Resource]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  StoredProcedure [dbo].[USP_GET_RootCause]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  StoredProcedure [dbo].[USP_GET_StatusMASTER]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  StoredProcedure [dbo].[USP_GETTypdMASTER]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  StoredProcedure [dbo].[USP_INSERT_RootCause]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_mlprun]    Script Date: 1/27/2020 12:51:32 PM ******/
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
/****** Object:  StoredProcedure [dbo].[USP_UPDATE_RootCause]    Script Date: 1/27/2020 12:51:32 PM ******/
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
USE [master]
GO
ALTER DATABASE [TicketTracker] SET  READ_WRITE 
GO

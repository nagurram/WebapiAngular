using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace LSDataApi.DBContext
{
    public partial class TicketTrackerContext : DbContext
    {
        private IConfiguration Configuration { get; set; }
        public TicketTrackerContext()
        {
            var builder = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json");
            Configuration = builder.Build();
        }

        public TicketTrackerContext(DbContextOptions<TicketTrackerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplicationMaster> ApplicationMaster { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<EmployeeDetails> EmployeeDetails { get; set; }
        public virtual DbSet<Entity> Entity { get; set; }
        public virtual DbSet<EntityLink> EntityLink { get; set; }
        public virtual DbSet<Exception> Exception { get; set; }
        public virtual DbSet<FileUpload> FileUpload { get; set; }
        public virtual DbSet<FinancialEventSource> FinancialEventSource { get; set; }
        public virtual DbSet<FinancialEventSourceFlatFile> FinancialEventSourceFlatFile { get; set; }
        public virtual DbSet<FinancialEventSourceSwift> FinancialEventSourceSwift { get; set; }
        public virtual DbSet<Fxtrade> Fxtrade { get; set; }
        public virtual DbSet<MenuItems> MenuItems { get; set; }
        public virtual DbSet<MenuPermission> MenuPermission { get; set; }
        public virtual DbSet<Menus> Menus { get; set; }
        public virtual DbSet<ModuleMaster> ModuleMaster { get; set; }
        public virtual DbSet<PriorityMaster> PriorityMaster { get; set; }
        public virtual DbSet<Resource> Resource { get; set; }
        public virtual DbSet<RoleMaster> RoleMaster { get; set; }
        public virtual DbSet<RootCauseMaster> RootCauseMaster { get; set; }
        public virtual DbSet<StatusMaster> StatusMaster { get; set; }
        public virtual DbSet<Tickets> Tickets { get; set; }
        public virtual DbSet<TodoList> TodoList { get; set; }
        public virtual DbSet<Trade> Trade { get; set; }
        public virtual DbSet<TradeValidationError> TradeValidationError { get; set; }
        public virtual DbSet<TrainingHistory> TrainingHistory { get; set; }
        public virtual DbSet<TrainingMaster> TrainingMaster { get; set; }
        public virtual DbSet<TypeMaster> TypeMaster { get; set; }
        public virtual DbSet<UserMaster> UserMaster { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<VwUserPermissions> VwUserPermissions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationMaster>(entity =>
            {
                entity.HasKey(e => e.ApplicationId);

                entity.Property(e => e.ApplicationName).HasMaxLength(100);

                entity.Property(e => e.StatusOrder).HasColumnName("statusOrder");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.DateofJoining).HasColumnType("date");

                entity.Property(e => e.Empid)
                    .HasColumnName("empid")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Managerid).HasColumnName("managerid");
            });

            modelBuilder.Entity<EmployeeDetails>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Address).HasMaxLength(1000);

                entity.Property(e => e.EmailId).HasMaxLength(200);

                entity.Property(e => e.Empid).HasColumnName("empid");

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);
            });

            modelBuilder.Entity<Entity>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ReferenceId).HasColumnName("Reference_ID");
            });

            modelBuilder.Entity<EntityLink>(entity =>
            {
                entity.HasKey(e => e.LinkId);

                entity.ToTable("Entity_Link");

                entity.Property(e => e.LinkId).HasColumnName("Link_ID");

                entity.Property(e => e.ChildEntityId).HasColumnName("Child_Entity_ID");

                entity.Property(e => e.ParentEntityId).HasColumnName("Parent_Entity_ID");

                entity.HasOne(d => d.ChildEntity)
                    .WithMany(p => p.EntityLinkChildEntity)
                    .HasForeignKey(d => d.ChildEntityId)
                    .HasConstraintName("FK_Entity_Link_Entity1");

                entity.HasOne(d => d.ParentEntity)
                    .WithMany(p => p.EntityLinkParentEntity)
                    .HasForeignKey(d => d.ParentEntityId)
                    .HasConstraintName("FK_Entity_Link_Entity");
            });

            modelBuilder.Entity<Exception>(entity =>
            {
                entity.Property(e => e.ActionName).HasMaxLength(50);

                entity.Property(e => e.ControllerName).HasMaxLength(50);

                entity.Property(e => e.ExceptionDateTime).HasColumnType("datetime");

                entity.Property(e => e.InnerExceptionmessage).HasMaxLength(4000);

                entity.Property(e => e.Message).HasMaxLength(150);

                entity.Property(e => e.StackTrace).HasMaxLength(4000);
            });

            modelBuilder.Entity<FileUpload>(entity =>
            {
                entity.HasKey(e => e.Fileid);

                entity.Property(e => e.FileName).HasMaxLength(150);

                entity.Property(e => e.Filetype).HasMaxLength(15);

                entity.Property(e => e.UploadDate).HasColumnType("datetime");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.FileUpload)
                    .HasForeignKey(d => d.TicketId)
                    .HasConstraintName("FK_FileUpload_Tickets");
            });

            modelBuilder.Entity<FinancialEventSource>(entity =>
            {
                entity.Property(e => e.CaptureDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<FinancialEventSourceFlatFile>(entity =>
            {
                entity.Property(e => e.FileName).HasMaxLength(200);

                entity.Property(e => e.LastUpdatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.ProcessingNotes).HasMaxLength(200);

                entity.Property(e => e.ReceiveDateTime).HasColumnType("datetime");

                entity.Property(e => e.ReceiveDir).HasMaxLength(200);

                entity.HasOne(d => d.FinancialEventSource)
                    .WithMany(p => p.FinancialEventSourceFlatFile)
                    .HasForeignKey(d => d.FinancialEventSourceId)
                    .HasConstraintName("FK_FinancialEventSourceFlatFile_FinancialEventSource");
            });

            modelBuilder.Entity<FinancialEventSourceSwift>(entity =>
            {
                entity.Property(e => e.Message).HasMaxLength(200);

                entity.Property(e => e.MessageXml)
                    .HasColumnName("MessageXML")
                    .HasMaxLength(2000);

                entity.Property(e => e.ReceiveDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.FinancialEventSource)
                    .WithMany(p => p.FinancialEventSourceSwift)
                    .HasForeignKey(d => d.FinancialEventSourceId)
                    .HasConstraintName("FK_FinancialEventSourceSwift_FinancialEventSource");
            });

            modelBuilder.Entity<Fxtrade>(entity =>
            {
                entity.ToTable("FXTrade");

                entity.Property(e => e.FxtradeId).HasColumnName("FXTradeId");

                entity.Property(e => e.CounterPartyNominal).HasMaxLength(200);

                entity.Property(e => e.HiportRef).HasMaxLength(200);

                entity.Property(e => e.Rate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SenderToReceieverInfo).HasMaxLength(2000);

                entity.Property(e => e.SettlementDate).HasColumnType("datetime");

                entity.Property(e => e.SpotForward).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TradeDate).HasColumnType("datetime");

                entity.Property(e => e.TradeNominal).HasMaxLength(200);
            });

            modelBuilder.Entity<MenuItems>(entity =>
            {
                entity.HasKey(e => e.MenuItemId);

                entity.Property(e => e.Link)
                    .HasColumnName("link")
                    .HasMaxLength(100);

                entity.Property(e => e.Parentmenuid).HasColumnName("parentmenuid");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.MenuItems)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MenuItems_Menus");
            });

            modelBuilder.Entity<MenuPermission>(entity =>
            {
                entity.HasNoKey();

                entity.HasOne(d => d.MenuItem)
                    .WithMany()
                    .HasForeignKey(d => d.MenuItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MenuPermission_MenuItems");

                entity.HasOne(d => d.Role)
                    .WithMany()
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_MenuPermission_RoleMaster");
            });

            modelBuilder.Entity<Menus>(entity =>
            {
                entity.HasKey(e => e.MenuId);

                entity.Property(e => e.DisplayName).HasMaxLength(100);
            });

            modelBuilder.Entity<ModuleMaster>(entity =>
            {
                entity.HasKey(e => e.ModuleId);

                entity.Property(e => e.ModuleName).HasMaxLength(100);

                entity.Property(e => e.StatusOrder).HasColumnName("statusOrder");
            });

            modelBuilder.Entity<PriorityMaster>(entity =>
            {
                entity.HasKey(e => e.PriorityId);

                entity.Property(e => e.PriorityDescription).HasMaxLength(100);
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(80);

                entity.Property(e => e.Fname)
                    .HasColumnName("FName")
                    .HasMaxLength(50);

                entity.Property(e => e.Lname).HasMaxLength(50);

                entity.Property(e => e.Pwd).HasMaxLength(50);

                entity.Property(e => e.Roles)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<RoleMaster>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.RoleDescription).HasMaxLength(100);
            });

            modelBuilder.Entity<RootCauseMaster>(entity =>
            {
                entity.HasKey(e => e.RootCauseId);

                entity.Property(e => e.Description).HasMaxLength(100);
            });

            modelBuilder.Entity<StatusMaster>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.Property(e => e.StatusDescription).HasMaxLength(100);

                entity.Property(e => e.StatusOrder).HasColumnName("statusOrder");
            });

            modelBuilder.Entity<Tickets>(entity =>
            {
                entity.HasKey(e => e.TicketId);

                entity.Property(e => e.Comments).HasMaxLength(4000);

                entity.Property(e => e.Createddate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedon).HasColumnType("datetime");

                entity.Property(e => e.ModuleId).HasColumnName("ModuleID");

                entity.Property(e => e.ResolutionDeadline).HasColumnType("datetime");

                entity.Property(e => e.ResponseDeadline).HasColumnType("datetime");

                entity.Property(e => e.Tdescription)
                    .HasColumnName("TDescription")
                    .HasMaxLength(4000);

                entity.Property(e => e.Title).HasMaxLength(4000);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.ApplicationId)
                    .HasConstraintName("FK_Tickets_ApplicationMaster");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.ModuleId)
                    .HasConstraintName("FK_Tickets_ModuleMaster");

                entity.HasOne(d => d.Priority)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.PriorityId)
                    .HasConstraintName("FK_Tickets_PriorityMaster");

                entity.HasOne(d => d.RootCause)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.RootCauseId)
                    .HasConstraintName("FK_Tickets_RootCauseMaster1");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_Tickets_StatusMaster");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_Tickets_TypeMaster");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Tickets_Resource");
            });

            modelBuilder.Entity<TodoList>(entity =>
            {
                entity.HasKey(e => e.TodoId);

                entity.Property(e => e.ActionDate)
                    .HasColumnName("actionDate")
                    .HasColumnType("date");

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.Titile).HasMaxLength(500);
            });

            modelBuilder.Entity<Trade>(entity =>
            {
                entity.Property(e => e.BrokerageGst)
                    .HasColumnName("BrokerageGST")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ClientTradeRef).HasMaxLength(200);

                entity.Property(e => e.GrossSettlementValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NetSettlementValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SecurityCode).HasMaxLength(200);

                entity.Property(e => e.TradeFileId).HasColumnName("TradeFileID");

                entity.Property(e => e.Units).HasMaxLength(200);
            });

            modelBuilder.Entity<TradeValidationError>(entity =>
            {
                entity.Property(e => e.TradeValidationErrorId).HasColumnName("TradeValidationErrorID");

                entity.Property(e => e.ErrorCode).HasMaxLength(300);

                entity.HasOne(d => d.Trade)
                    .WithMany(p => p.TradeValidationError)
                    .HasForeignKey(d => d.TradeId)
                    .HasConstraintName("FK_TradeValidationError_Trade");
            });

            modelBuilder.Entity<TrainingHistory>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<TrainingMaster>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Details)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.TrianingId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<TypeMaster>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.Property(e => e.TypeDescription).HasMaxLength(100);
            });

            modelBuilder.Entity<UserMaster>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Fname)
                    .HasColumnName("FName")
                    .HasMaxLength(100);

                entity.Property(e => e.Lname)
                    .HasColumnName("LName")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<UserRoles>(entity =>
            {
                entity.HasKey(e => e.Mappingid);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRoles_RoleMaster");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRoles_UserMaster");
            });

            modelBuilder.Entity<VwUserPermissions>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_user_permissions");

                entity.Property(e => e.Displayname)
                    .HasColumnName("displayname")
                    .HasMaxLength(100);

                entity.Property(e => e.Link)
                    .HasColumnName("link")
                    .HasMaxLength(100);

                entity.Property(e => e.Menuitemid).HasColumnName("menuitemid");

                entity.Property(e => e.Parentmenuid).HasColumnName("parentmenuid");

                entity.Property(e => e.Roleid).HasColumnName("roleid");

                entity.Property(e => e.Sortorder).HasColumnName("sortorder");

                entity.Property(e => e.Userid).HasColumnName("userid");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

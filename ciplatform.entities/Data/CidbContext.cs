using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ciplatform.entities.Models;

namespace ciplatform.entities.Data;

public partial class CidbContext : DbContext
{
    public CidbContext()
    {
    }

    public CidbContext(DbContextOptions<CidbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Banner> Banners { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<CmsPage> CmsPages { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<ContactU> ContactUs { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<EnableUserStatus> EnableUserStatuses { get; set; }

    public virtual DbSet<FavouritMission> FavouritMissions { get; set; }

    public virtual DbSet<GoalMission> GoalMissions { get; set; }

    public virtual DbSet<MessageTable> MessageTables { get; set; }

    public virtual DbSet<Mission> Missions { get; set; }

    public virtual DbSet<MissionApplication> MissionApplications { get; set; }

    public virtual DbSet<MissionDocument> MissionDocuments { get; set; }

    public virtual DbSet<MissionInvite> MissionInvites { get; set; }

    public virtual DbSet<MissionMedium> MissionMedia { get; set; }

    public virtual DbSet<MissionRating> MissionRatings { get; set; }

    public virtual DbSet<MissionSkill> MissionSkills { get; set; }

    public virtual DbSet<NotificationTitle> NotificationTitles { get; set; }

    public virtual DbSet<PasswordReset> PasswordResets { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<Story> Stories { get; set; }

    public virtual DbSet<StoryMedium> StoryMedia { get; set; }

    public virtual DbSet<StotyInvite> StotyInvites { get; set; }

    public virtual DbSet<Theme> Themes { get; set; }

    public virtual DbSet<Timesheet> Timesheets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSkill> UserSkills { get; set; }

    public virtual DbSet<Userpermission> Userpermissions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__4A311D2F9899BB34");

            entity.ToTable("Admin", "dbo");

            entity.HasIndex(e => e.Email, "UQ__Admin__A9D105348870D8E6").IsUnique();

            entity.Property(e => e.AdminId).HasColumnName("Admin_id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.Email)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("First_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Last_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
        });

        modelBuilder.Entity<Banner>(entity =>
        {
            entity.HasKey(e => e.BannerId).HasName("PK__Banner__8174A02CB7436546");

            entity.ToTable("Banner", "dbo");

            entity.Property(e => e.BannerId).HasColumnName("Banner_id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.Image)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.SortOrder)
                .HasDefaultValueSql("((0))")
                .HasColumnName("Sort_Order");
            entity.Property(e => e.Text).HasColumnType("text");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__City__DE9CEC389C81677B");

            entity.ToTable("City", "dbo");

            entity.Property(e => e.CityId).HasColumnName("City_id");
            entity.Property(e => e.CountryId).HasColumnName("Country_id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__City__Country_id__46E78A0C");
        });

        modelBuilder.Entity<CmsPage>(entity =>
        {
            entity.HasKey(e => e.CmsPageId).HasName("PK__cms_page__B46D5B5222B54F47");

            entity.ToTable("cms_page", "dbo");

            entity.Property(e => e.CmsPageId).HasColumnName("cms_page_id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comment", "dbo");

            entity.Property(e => e.CommentId).HasColumnName("Comment_id");
            entity.Property(e => e.ApprovalStatus).HasColumnName("Approval_status");
            entity.Property(e => e.ComentDescription)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Coment_description");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("Mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.Comments)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__Mission__6477ECF3");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__User_id__628FA481");
        });

        modelBuilder.Entity<ContactU>(entity =>
        {
            entity.HasKey(e => e.Contactusid).HasName("PK__contactu__E7FED8B597DB36FD");

            entity.ToTable("ContactUs", "dbo");

            entity.Property(e => e.Contactusid).HasColumnName("contactusid");
            entity.Property(e => e.Message)
                .HasMaxLength(50)
                .HasColumnName("message");
            entity.Property(e => e.Subject)
                .HasMaxLength(50)
                .HasColumnName("subject");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.ContactUs)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__contactus__useri__41B8C09B");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK__Country__8037C7D635219D8D");

            entity.ToTable("Country", "dbo");

            entity.Property(e => e.CountryId).HasColumnName("Country_id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.Iso)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("ISO");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
        });

        modelBuilder.Entity<EnableUserStatus>(entity =>
        {
            entity.HasKey(e => e.Enableuserid).HasName("PK__enable_u__E4421F1894AAD031");

            entity.ToTable("enable_user_status", "dbo");

            entity.Property(e => e.Enableuserid).HasColumnName("enableuserid");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("((0))")
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Notification).WithMany(p => p.EnableUserStatuses)
                .HasForeignKey(d => d.NotificationId)
                .HasConstraintName("FK__enable_us__notif__2E3BD7D3");

            entity.HasOne(d => d.User).WithMany(p => p.EnableUserStatuses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__enable_us__user___2F2FFC0C");
        });

        modelBuilder.Entity<FavouritMission>(entity =>
        {
            entity.HasKey(e => e.FavouritMissionId).HasName("PK__favourit__16E34B4CA6FD2053");

            entity.ToTable("favourit_mission", "dbo");

            entity.Property(e => e.FavouritMissionId).HasColumnName("favourit_mission_id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("Mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.FavouritMissions)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__favourit___Missi__71D1E811");

            entity.HasOne(d => d.User).WithMany(p => p.FavouritMissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__favourit___User___6FE99F9F");
        });

        modelBuilder.Entity<GoalMission>(entity =>
        {
            entity.HasKey(e => e.GoalMissionId).HasName("PK__goal_mis__358E02C7FF38210C");

            entity.ToTable("goal_mission", "dbo");

            entity.Property(e => e.GoalMissionId).HasColumnName("goal_mission_id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.GoalObjectiveText)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("goal_objective_text");
            entity.Property(e => e.GoalValue).HasColumnName("goal_value");
            entity.Property(e => e.MissionId).HasColumnName("Mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");

            entity.HasOne(d => d.Mission).WithMany(p => p.GoalMissions)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__goal_miss__Missi__778AC167");
        });

        modelBuilder.Entity<MessageTable>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__message___0BBF6EE68BBA9F14");

            entity.ToTable("message_table", "dbo");

            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.AvatarUser).HasColumnName("avatar_user");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Message)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("message");
            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.Url).HasColumnName("url");

            entity.HasOne(d => d.Notification).WithMany(p => p.MessageTables)
                .HasForeignKey(d => d.NotificationId)
                .HasConstraintName("FK__message_t__notif__2B5F6B28");
        });

        modelBuilder.Entity<Mission>(entity =>
        {
            entity.HasKey(e => e.MissionId).HasName("PK__Mission__93DC24EAB57DA299");

            entity.ToTable("Mission", "dbo");

            entity.Property(e => e.MissionId).HasColumnName("Mission_id");
            entity.Property(e => e.Availability)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CityId).HasColumnName("City_id");
            entity.Property(e => e.CountryId).HasColumnName("Country_id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeadlineDate).HasColumnName("Deadline_Date");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.EndDate)
                .HasPrecision(0)
                .HasColumnName("End_Date");
            entity.Property(e => e.MissionType).HasColumnName("Mission_type");
            entity.Property(e => e.OrganizationDetail)
                .HasColumnType("text")
                .HasColumnName("Organization_detail");
            entity.Property(e => e.OrganizationName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Organization_name");
            entity.Property(e => e.ShortDescription)
                .HasColumnType("text")
                .HasColumnName("Short_Description");
            entity.Property(e => e.StartDate)
                .HasPrecision(0)
                .HasColumnName("Start_Date");
            entity.Property(e => e.ThemeId).HasColumnName("Theme_id");
            entity.Property(e => e.Title)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.TotalSeat).HasColumnName("total_seat");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");

            entity.HasOne(d => d.City).WithMany(p => p.Missions)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mission__City_id__5BE2A6F2");

            entity.HasOne(d => d.Country).WithMany(p => p.Missions)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mission__Country__5DCAEF64");

            entity.HasOne(d => d.Theme).WithMany(p => p.Missions)
                .HasForeignKey(d => d.ThemeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mission__Theme_i__59FA5E80");
        });

        modelBuilder.Entity<MissionApplication>(entity =>
        {
            entity.HasKey(e => e.MissionApplicationId).HasName("PK__mission___DF92838A6B3173C3");

            entity.ToTable("mission_application", "dbo");

            entity.Property(e => e.MissionApplicationId).HasColumnName("mission_application_id");
            entity.Property(e => e.AppliedAt)
                .HasPrecision(0)
                .HasColumnName("Applied_at");
            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))")
                .HasColumnName("Approval_status");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("Mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionApplications)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_a__Missi__7D439ABD");

            entity.HasOne(d => d.User).WithMany(p => p.MissionApplications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_a__User___7F2BE32F");
        });

        modelBuilder.Entity<MissionDocument>(entity =>
        {
            entity.HasKey(e => e.MissionDocumentId).HasName("PK__mission___5379476601197333");

            entity.ToTable("mission_document", "dbo");

            entity.Property(e => e.MissionDocumentId).HasColumnName("Mission_document_id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.DocumentName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Document_name");
            entity.Property(e => e.DocumentPath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Document_path");
            entity.Property(e => e.DocumentType)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Document_type");
            entity.Property(e => e.MissionId).HasColumnName("Mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionDocuments)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_d__Missi__05D8E0BE");
        });

        modelBuilder.Entity<MissionInvite>(entity =>
        {
            entity.HasKey(e => e.MissionInviteId).HasName("PK__mission___A1126E845351FD4A");

            entity.ToTable("mission_invite", "dbo");

            entity.Property(e => e.MissionInviteId).HasColumnName("Mission_invite_id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.FromUserId).HasColumnName("from_user_id");
            entity.Property(e => e.MissionId).HasColumnName("Mission_id");
            entity.Property(e => e.ToUserId).HasColumnName("to_user_id");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");

            entity.HasOne(d => d.FromUser).WithMany(p => p.MissionInviteFromUsers)
                .HasForeignKey(d => d.FromUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_i__from___0D7A0286");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionInvites)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_i__Missi__0B91BA14");

            entity.HasOne(d => d.ToUser).WithMany(p => p.MissionInviteToUsers)
                .HasForeignKey(d => d.ToUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_i__to_us__0F624AF8");
        });

        modelBuilder.Entity<MissionMedium>(entity =>
        {
            entity.HasKey(e => e.MissionMediaId).HasName("PK__mission___848A78E8A0551D6B");

            entity.ToTable("mission_media", "dbo");

            entity.Property(e => e.MissionMediaId).HasColumnName("mission_media_id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.Defaults).HasColumnName("defaults");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.MediaName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("media_name");
            entity.Property(e => e.MediaPath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("media_path");
            entity.Property(e => e.MediaType)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("media_type");
            entity.Property(e => e.MissionId).HasColumnName("Mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionMedia)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_m__Missi__151B244E");
        });

        modelBuilder.Entity<MissionRating>(entity =>
        {
            entity.HasKey(e => e.MissionRatingId).HasName("PK__mission___320E51729CFF267E");

            entity.ToTable("mission_rating", "dbo");

            entity.Property(e => e.MissionRatingId).HasColumnName("mission_rating_id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("Mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionRatings)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_r__Missi__1DB06A4F");

            entity.HasOne(d => d.User).WithMany(p => p.MissionRatings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_r__User___1BC821DD");
        });

        modelBuilder.Entity<MissionSkill>(entity =>
        {
            entity.HasKey(e => e.MissionSkillId).HasName("PK__mission___82712008C6370B44");

            entity.ToTable("mission_skill", "dbo");

            entity.Property(e => e.MissionSkillId).HasColumnName("mission_skill_id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("Mission_id");
            entity.Property(e => e.SkillId).HasColumnName("skill_id");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionSkills)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_s__Missi__2B0A656D");

            entity.HasOne(d => d.Skill).WithMany(p => p.MissionSkills)
                .HasForeignKey(d => d.SkillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_s__skill__29221CFB");
        });

        modelBuilder.Entity<NotificationTitle>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__notifica__E059842FBFD46620");

            entity.ToTable("notification_title", "dbo");

            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<PasswordReset>(entity =>
        {
            entity.HasKey(e => e.Token);

            entity.ToTable("password_reset", "dbo");

            entity.Property(e => e.Token)
                .HasMaxLength(191)
                .IsUnicode(false)
                .HasColumnName("token");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(191)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.SkillId).HasName("PK__Skills__B4AAE688A68CF529");

            entity.ToTable("Skills", "dbo");

            entity.Property(e => e.SkillId).HasColumnName("Skill_id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.SkillName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("skill_name");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("((1))")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
        });

        modelBuilder.Entity<Story>(entity =>
        {
            entity.HasKey(e => e.StoryId).HasName("PK__story__66339C5674CEEED0");

            entity.ToTable("story", "dbo");

            entity.Property(e => e.StoryId).HasColumnName("story_id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.MissionId).HasColumnName("Mission_id");
            entity.Property(e => e.PublishedAt)
                .HasPrecision(0)
                .HasColumnName("Published_at");
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
            entity.Property(e => e.UserId).HasColumnName("User_id");
            entity.Property(e => e.Viewcount).HasColumnName("viewcount");

            entity.HasOne(d => d.Mission).WithMany(p => p.Stories)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__story__Mission_i__3587F3E0");

            entity.HasOne(d => d.User).WithMany(p => p.Stories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__story__User_id__339FAB6E");
        });

        modelBuilder.Entity<StoryMedium>(entity =>
        {
            entity.HasKey(e => e.StoryMediaId).HasName("PK__story_me__29BD053C2E7463E4");

            entity.ToTable("story_media", "dbo");

            entity.Property(e => e.StoryMediaId).HasColumnName("story_media_id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.Path)
                .HasColumnType("text")
                .HasColumnName("path");
            entity.Property(e => e.StoryId).HasColumnName("story_id");
            entity.Property(e => e.Type)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");

            entity.HasOne(d => d.Story).WithMany(p => p.StoryMedia)
                .HasForeignKey(d => d.StoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__story_med__story__45BE5BA9");
        });

        modelBuilder.Entity<StotyInvite>(entity =>
        {
            entity.HasKey(e => e.StoryInviteId).HasName("PK__stoty_in__0449786774D7846A");

            entity.ToTable("stoty_invite", "dbo");

            entity.Property(e => e.StoryInviteId).HasColumnName("story_invite_id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.FromUserId).HasColumnName("from_user_id");
            entity.Property(e => e.StoryId).HasColumnName("story_id");
            entity.Property(e => e.ToUserId).HasColumnName("to_user_id");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");

            entity.HasOne(d => d.FromUser).WithMany(p => p.StotyInviteFromUsers)
                .HasForeignKey(d => d.FromUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__stoty_inv__from___3E1D39E1");

            entity.HasOne(d => d.Story).WithMany(p => p.StotyInvites)
                .HasForeignKey(d => d.StoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__stoty_inv__story__3D2915A8");

            entity.HasOne(d => d.ToUser).WithMany(p => p.StotyInviteToUsers)
                .HasForeignKey(d => d.ToUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__stoty_inv__to_us__40058253");
        });

        modelBuilder.Entity<Theme>(entity =>
        {
            entity.HasKey(e => e.ThemeId).HasName("PK__Theme__59291A6A0F746F52");

            entity.ToTable("Theme", "dbo");

            entity.Property(e => e.ThemeId).HasColumnName("Theme_id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
        });

        modelBuilder.Entity<Timesheet>(entity =>
        {
            entity.HasKey(e => e.TimesheetId).HasName("PK__timeshee__7BBF50680513EB45");

            entity.ToTable("timesheet", "dbo");

            entity.Property(e => e.TimesheetId).HasColumnName("timesheet_id");
            entity.Property(e => e.Action).HasColumnName("action");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DateVolunteer)
                .HasPrecision(0)
                .HasColumnName("date_volunteer");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("Mission_id");
            entity.Property(e => e.Notes)
                .HasColumnType("text")
                .HasColumnName("notes");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Time)
                .HasPrecision(0)
                .HasColumnName("time");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.Timesheets)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__timesheet__Missi__4D5F7D71");

            entity.HasOne(d => d.User).WithMany(p => p.Timesheets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__timesheet__User___4B7734FF");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__206A9DF8B932096C");

            entity.ToTable("Users", "dbo");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534D217AAAC").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("User_id");
            entity.Property(e => e.Availability)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Avatar)
                .HasMaxLength(2048)
                .IsUnicode(false);
            entity.Property(e => e.CityId).HasColumnName("City_id");
            entity.Property(e => e.CountryId).HasColumnName("Country_id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.Department)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Employee_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("First_name");
            entity.Property(e => e.LinkedInUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Linked_in_url");
            entity.Property(e => e.Manager)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber).HasColumnName("Phone_number");
            entity.Property(e => e.ProfileText)
                .HasColumnType("text")
                .HasColumnName("Profile_text");
            entity.Property(e => e.SecondName)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Second_name");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
            entity.Property(e => e.WhyIVolunteer)
                .HasColumnType("text")
                .HasColumnName("Why_i_volunteer");

            entity.HasOne(d => d.City).WithMany(p => p.Users)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__City_id__4CA06362");

            entity.HasOne(d => d.Country).WithMany(p => p.Users)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__Country_i__4E88ABD4");
        });

        modelBuilder.Entity<UserSkill>(entity =>
        {
            entity.HasKey(e => e.UserSkillId).HasName("PK__user_ski__FD3B576BA988C926");

            entity.ToTable("user_skill", "dbo");

            entity.Property(e => e.UserSkillId).HasColumnName("user_skill_id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.SkillId).HasColumnName("skill_id");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Skill).WithMany(p => p.UserSkills)
                .HasForeignKey(d => d.SkillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__user_skil__skill__55F4C372");

            entity.HasOne(d => d.User).WithMany(p => p.UserSkills)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__user_skil__User___540C7B00");
        });

        modelBuilder.Entity<Userpermission>(entity =>
        {
            entity.HasKey(e => e.UserpermissionId).HasName("PK__userperm__4AD7F7C44A2A3C5D");

            entity.ToTable("userpermission", "dbo");

            entity.Property(e => e.UserpermissionId).HasColumnName("userpermission_id");
            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.Seen)
                .HasDefaultValueSql("((1))")
                .HasColumnName("seen");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("((1))")
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Message).WithMany(p => p.Userpermissions)
                .HasForeignKey(d => d.MessageId)
                .HasConstraintName("FK__userpermi__messa__38B96646");

            entity.HasOne(d => d.User).WithMany(p => p.Userpermissions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__userpermi__user___35DCF99B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

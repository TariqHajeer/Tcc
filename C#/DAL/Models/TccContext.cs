using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DAL.Models
{
    public partial class TccContext : DbContext
    {
        public TccContext()
        {
        }
        
        public TccContext(DbContextOptions<TccContext> options)
            : base(options)
        {
        }   

        public virtual DbSet<Attatchments> Attatchments { get; set; }
        public virtual DbSet<ClosesetPersons> ClosesetPersons { get; set; }
        public virtual DbSet<Colleges> Colleges { get; set; }
        public virtual DbSet<Constraints> Constraints { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<CrossOf> CrossOf { get; set; }
        public virtual DbSet<Deattach> Deattach { get; set; }
        public virtual DbSet<Degree> Degree { get; set; }
        public virtual DbSet<DependenceSubject> DependenceSubject { get; set; }
        public virtual DbSet<EquivalentSubject> EquivalentSubject { get; set; }
        public virtual DbSet<ExamSemester> ExamSemester { get; set; }
        public virtual DbSet<ExamSystem> ExamSystem { get; set; }
        public virtual DbSet<Graduation> Graduation { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<GroupPrivilage> GroupPrivilage { get; set; }
        public virtual DbSet<Honesty> Honesty { get; set; }
        public virtual DbSet<Langaues> Langaues { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<MoreInformation> MoreInformation { get; set; }
        public virtual DbSet<Nationalies> Nationalies { get; set; }
        public virtual DbSet<Partners> Partners { get; set; }
        public virtual DbSet<PersonPhone> PersonPhone { get; set; }
        public virtual DbSet<PhoneType> PhoneType { get; set; }
        public virtual DbSet<Privilage> Privilage { get; set; }
        public virtual DbSet<Registrations> Registrations { get; set; }
        public virtual DbSet<Reparations> Reparations { get; set; }
        public virtual DbSet<Sanctions> Sanctions { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<SettingYearSystem> SettingYearSystem { get; set; }
        public virtual DbSet<Siblings> Siblings { get; set; }
        public virtual DbSet<SocialStates> SocialStates { get; set; }
        public virtual DbSet<Specializations> Specializations { get; set; }
        public virtual DbSet<StudentAttachment> StudentAttachment { get; set; }
        public virtual DbSet<StudentDegree> StudentDegree { get; set; }
        public virtual DbSet<StudentPhone> StudentPhone { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<StudentState> StudentState { get; set; }
        public virtual DbSet<StudentSubject> StudentSubject { get; set; }
        public virtual DbSet<StudyPlan> StudyPlan { get; set; }
        public virtual DbSet<StudySemester> StudySemester { get; set; }
        public virtual DbSet<StudyYear> StudyYear { get; set; }
        public virtual DbSet<Subjects> Subjects { get; set; }
        public virtual DbSet<SubjectType> SubjectType { get; set; }
        public virtual DbSet<Trasmentd> Trasmentd { get; set; }
        public virtual DbSet<TypeOfRegistar> TypeOfRegistar { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserGroup> UserGroup { get; set; }
        public virtual DbSet<Years> Years { get; set; }
        public virtual DbSet<YearSystem> YearSystem { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=Tcc;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attatchments>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ClosesetPersons>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Ssn)
                    .IsRequired()
                    .HasColumnName("SSN")
                    .HasMaxLength(50);

                entity.HasOne(d => d.SsnNavigation)
                    .WithMany(p => p.ClosesetPersons)
                    .HasForeignKey(d => d.Ssn)
                    .HasConstraintName("FK_ClosesetPersons_Students");
            });

            modelBuilder.Entity<Colleges>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.Colleges)
                    .HasForeignKey(d => d.ProvinceId)
                    .HasConstraintName("FK_Colleges_Counties");
            });

            modelBuilder.Entity<Constraints>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Honesty)
                    .WithMany(p => p.Constraints)
                    .HasForeignKey(d => d.HonestyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Constraints_Honesty");
            });

            modelBuilder.Entity<Countries>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.MainCountryNavigation)
                    .WithMany(p => p.InverseMainCountryNavigation)
                    .HasForeignKey(d => d.MainCountry)
                    .HasConstraintName("FK_Counties_Counties");
            });

            modelBuilder.Entity<CrossOf>(entity =>
            {
                entity.HasKey(e => e.Ssn);

                entity.Property(e => e.Ssn)
                    .HasColumnName("SSN")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.HasOne(d => d.SsnNavigation)
                    .WithOne(p => p.CrossOf)
                    .HasForeignKey<CrossOf>(d => d.Ssn)
                    .HasConstraintName("fk_name2");
            });

            modelBuilder.Entity<Deattach>(entity =>
            {
                entity.HasKey(e => e.Ssn);

                entity.Property(e => e.Ssn)
                    .HasColumnName("SSN")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.Decision).IsRequired();

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Reason).IsRequired();

                entity.HasOne(d => d.SsnNavigation)
                    .WithOne(p => p.Deattach)
                    .HasForeignKey<Deattach>(d => d.Ssn)
                    .HasConstraintName("fk_name3");
            });

            modelBuilder.Entity<Degree>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<DependenceSubject>(entity =>
            {
                entity.HasKey(e => new { e.SubjectId, e.DependsOnSubjectId });

                entity.Property(e => e.SubjectId).HasColumnName("subjectId");

                entity.Property(e => e.DependsOnSubjectId).HasColumnName("dependsOnSubjectId");

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.HasOne(d => d.DependsOnSubject)
                    .WithMany(p => p.DependenceSubjectDependsOnSubject)
                    .HasForeignKey(d => d.DependsOnSubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DependenceSubject_Subjects1");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.DependenceSubjectSubject)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DependenceSubject_Subjects");
            });

            modelBuilder.Entity<EquivalentSubject>(entity =>
            {
                entity.HasKey(e => new { e.FirstSubject, e.SecoundSubject });

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.HasOne(d => d.FirstSubjectNavigation)
                    .WithMany(p => p.EquivalentSubjectFirstSubjectNavigation)
                    .HasForeignKey(d => d.FirstSubject)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EquivalentSubject_Subjects");

                entity.HasOne(d => d.SecoundSubjectNavigation)
                    .WithMany(p => p.EquivalentSubjectSecoundSubjectNavigation)
                    .HasForeignKey(d => d.SecoundSubject)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EquivalentSubject_Subjects1");
            });

            modelBuilder.Entity<ExamSemester>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.HasOne(d => d.Year)
                    .WithMany(p => p.ExamSemester)
                    .HasForeignKey(d => d.YearId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamSemester_Years");
            });

            modelBuilder.Entity<ExamSystem>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Graduation>(entity =>
            {
                entity.HasKey(e => e.Ssn);

                entity.Property(e => e.Ssn)
                    .HasColumnName("SSN")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.ApprovalDate).HasColumnType("date");

                entity.Property(e => e.Cddate)
                    .HasColumnName("CDDate")
                    .HasColumnType("date");

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DesicionDate).HasColumnType("date");

                entity.Property(e => e.GeneralAppreciation).HasMaxLength(50);

                entity.Property(e => e.GeneralAverage).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.HasOne(d => d.SsnNavigation)
                    .WithOne(p => p.Graduation)
                    .HasForeignKey<Graduation>(d => d.Ssn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_name6");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(10);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<GroupPrivilage>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.PrivilageId });

                entity.ToTable("Group_Privilage");

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupPrivilage)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("fk_Privilage_id1");

                entity.HasOne(d => d.Privilage)
                    .WithMany(p => p.GroupPrivilage)
                    .HasForeignKey(d => d.PrivilageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Group_Privilage_Privilage");
            });

            modelBuilder.Entity<Honesty>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Honesty)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Honesty_Counties");
            });

            modelBuilder.Entity<Langaues>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.AfterAction).IsRequired();

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.EntityName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MoreInformation>(entity =>
            {
                entity.HasKey(e => e.Ssn);

                entity.Property(e => e.Ssn)
                    .HasColumnName("SSN")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FatherBirthDay).HasColumnType("date");

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.MotherBirthDay).HasColumnType("date");

                entity.Property(e => e.MotherFirstName).HasMaxLength(50);

                entity.Property(e => e.MotherLastName).HasMaxLength(50);

                entity.HasOne(d => d.SsnNavigation)
                    .WithOne(p => p.MoreInformation)
                    .HasForeignKey<MoreInformation>(d => d.Ssn)
                    .HasConstraintName("FK_MoreInformation_Students1");
            });

            modelBuilder.Entity<Nationalies>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Partners>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Ssn)
                    .IsRequired()
                    .HasColumnName("SSN")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Nationaliry)
                    .WithMany(p => p.Partners)
                    .HasForeignKey(d => d.NationaliryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Partners_Nationalies");

                entity.HasOne(d => d.SsnNavigation)
                    .WithMany(p => p.Partners)
                    .HasForeignKey(d => d.Ssn)
                    .HasConstraintName("FK_Partners_Students");
            });

            modelBuilder.Entity<PersonPhone>(entity =>
            {
                entity.HasKey(e => new { e.PersonId, e.PhoneTypeId });

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.PersonPhone)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PersonPhone_ClosesetPersons");

                entity.HasOne(d => d.PhoneType)
                    .WithMany(p => p.PersonPhone)
                    .HasForeignKey(d => d.PhoneTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PersonPhone_PhoneType");
            });

            modelBuilder.Entity<PhoneType>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Privilage>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Registrations>(entity =>
            {
                entity.Property(e => e.CardDate).HasColumnType("date");

                entity.Property(e => e.CardNumber).HasMaxLength(50);

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.SoldierDate).HasColumnType("date");

                entity.Property(e => e.Ssn)
                    .IsRequired()
                    .HasColumnName("SSN")
                    .HasMaxLength(50);

                entity.HasOne(d => d.FinalState)
                    .WithMany(p => p.RegistrationsFinalState)
                    .HasForeignKey(d => d.FinalStateId)
                    .HasConstraintName("FK_Registrations_StudentState1");

                entity.HasOne(d => d.SsnNavigation)
                    .WithMany(p => p.Registrations)
                    .HasForeignKey(d => d.Ssn)
                    .HasConstraintName("FK_Registrations_Students1");

                entity.HasOne(d => d.StudentState)
                    .WithMany(p => p.RegistrationsStudentState)
                    .HasForeignKey(d => d.StudentStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Registrations_StudentState");

                entity.HasOne(d => d.StudyYear)
                    .WithMany(p => p.Registrations)
                    .HasForeignKey(d => d.StudyYearId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Registrations_StudyYear");

                entity.HasOne(d => d.TypeOfRegistar)
                    .WithMany(p => p.Registrations)
                    .HasForeignKey(d => d.TypeOfRegistarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Registrations_TypeOfRegistar");

                entity.HasOne(d => d.Year)
                    .WithMany(p => p.Registrations)
                    .HasForeignKey(d => d.YearId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Registrations_Years");
            });

            modelBuilder.Entity<Reparations>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Reparation)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Ssn)
                    .IsRequired()
                    .HasColumnName("SSN")
                    .HasMaxLength(50);

                entity.HasOne(d => d.SsnNavigation)
                    .WithMany(p => p.Reparations)
                    .HasForeignKey(d => d.Ssn)
                    .HasConstraintName("FK_Reparations_Students1");
            });

            modelBuilder.Entity<Sanctions>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Sanction).IsRequired();

                entity.Property(e => e.Ssn)
                    .IsRequired()
                    .HasColumnName("SSN")
                    .HasMaxLength(50);

                entity.HasOne(d => d.SsnNavigation)
                    .WithMany(p => p.Sanctions)
                    .HasForeignKey(d => d.Ssn)
                    .HasConstraintName("FK_Sanctions_Students1");
            });

            modelBuilder.Entity<Settings>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<SettingYearSystem>(entity =>
            {
                entity.HasKey(e => new { e.YearSystem, e.SettingId });

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.HasOne(d => d.Setting)
                    .WithMany(p => p.SettingYearSystem)
                    .HasForeignKey(d => d.SettingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SettingYearStstem_Settings");

                entity.HasOne(d => d.YearSystemNavigation)
                    .WithMany(p => p.SettingYearSystem)
                    .HasForeignKey(d => d.YearSystem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SettingYearStstem_YearSystem");
            });

            modelBuilder.Entity<Siblings>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Ssn)
                    .IsRequired()
                    .HasColumnName("SSN")
                    .HasMaxLength(50);

                entity.Property(e => e.Work).HasColumnName("_Work");

                entity.HasOne(d => d.SocialStateNavigation)
                    .WithMany(p => p.Siblings)
                    .HasForeignKey(d => d.SocialState)
                    .HasConstraintName("FK_Siblings_SocialStates");

                entity.HasOne(d => d.SsnNavigation)
                    .WithMany(p => p.Siblings)
                    .HasForeignKey(d => d.Ssn)
                    .HasConstraintName("FK_Siblings_Students1");
            });

            modelBuilder.Entity<SocialStates>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Specializations>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<StudentAttachment>(entity =>
            {
                entity.Property(e => e.Attachemnt).IsRequired();

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Ssn)
                    .IsRequired()
                    .HasColumnName("SSN")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Attachment)
                    .WithMany(p => p.StudentAttachment)
                    .HasForeignKey(d => d.AttachmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentAttachment_Attatchments");

                entity.HasOne(d => d.SsnNavigation)
                    .WithMany(p => p.StudentAttachment)
                    .HasForeignKey(d => d.Ssn)
                    .HasConstraintName("FK_StudentAttachment_Students1");
            });

            modelBuilder.Entity<StudentDegree>(entity =>
            {
                entity.HasKey(e => e.Ssn);

                entity.Property(e => e.Ssn)
                    .HasColumnName("SSN")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.DateNavigation)
                    .WithMany(p => p.StudentDegree)
                    .HasForeignKey(d => d.Date)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentDegree_Years");

                entity.HasOne(d => d.DegreeNavigation)
                    .WithMany(p => p.StudentDegree)
                    .HasForeignKey(d => d.DegreeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentDegree_Degree");

                entity.HasOne(d => d.SourceNavigation)
                    .WithMany(p => p.StudentDegree)
                    .HasForeignKey(d => d.Source)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentDegree_Countries");

                entity.HasOne(d => d.SsnNavigation)
                    .WithOne(p => p.StudentDegree)
                    .HasForeignKey<StudentDegree>(d => d.Ssn)
                    .HasConstraintName("FK_StudentDegree_Students1");
            });

            modelBuilder.Entity<StudentPhone>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Ssn)
                    .IsRequired()
                    .HasColumnName("SSN")
                    .HasMaxLength(50);

                entity.HasOne(d => d.PhoneType)
                    .WithMany(p => p.StudentPhone)
                    .HasForeignKey(d => d.PhoneTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentPhones_PhoneType");

                entity.HasOne(d => d.SsnNavigation)
                    .WithMany(p => p.StudentPhone)
                    .HasForeignKey(d => d.Ssn)
                    .HasConstraintName("FK_StudentPhone_Students");
            });

            modelBuilder.Entity<Students>(entity =>
            {
                entity.HasKey(e => e.Ssn);

                entity.Property(e => e.Ssn)
                    .HasColumnName("SSN")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.CeasedFromTheCollage).HasColumnType("date");

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.EnrollmentDate).HasColumnType("date");

                entity.Property(e => e.FatherName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LanguageId).HasColumnName("languageId");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.SpecializationId)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.Constraint)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.ConstraintId)
                    .HasConstraintName("FK_Students_Constraints");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Students_Langaues");

                entity.HasOne(d => d.Nationality)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.NationalityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Students_Nationalies");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.ProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Students_Countries");

                entity.HasOne(d => d.Specialization)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.SpecializationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Students_Specializations");

                entity.HasOne(d => d.TransformedFrom)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.TransformedFromId)
                    .HasConstraintName("FK_Students_Colleges");
            });

            modelBuilder.Entity<StudentState>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<StudentSubject>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.HelpDegree).HasDefaultValueSql("((0))");

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.PracticalDegree).HasColumnName("practicalDegree");

                entity.Property(e => e.Ssn)
                    .IsRequired()
                    .HasColumnName("SSN")
                    .HasMaxLength(50);

                entity.HasOne(d => d.ExamSemester)
                    .WithMany(p => p.StudentSubject)
                    .HasForeignKey(d => d.ExamSemesterId)
                    .HasConstraintName("FK_StudentSubject_ExamSemester");

                entity.HasOne(d => d.SsnNavigation)
                    .WithMany(p => p.StudentSubject)
                    .HasForeignKey(d => d.Ssn)
                    .HasConstraintName("FK_StudentSubject_Students");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.StudentSubject)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentSubject_Subjects");
            });

            modelBuilder.Entity<StudyPlan>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.SpecializationId)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.Specialization)
                    .WithMany(p => p.StudyPlan)
                    .HasForeignKey(d => d.SpecializationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudyPlan_Specializations");

                entity.HasOne(d => d.Year)
                    .WithMany(p => p.StudyPlan)
                    .HasForeignKey(d => d.YearId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudyPlan_Years");
            });

            modelBuilder.Entity<StudySemester>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.HasOne(d => d.StudyYear)
                    .WithMany(p => p.StudySemester)
                    .HasForeignKey(d => d.StudyYearId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudySemester_StudyYear");

                entity.HasOne(d => d.Studyplan)
                    .WithMany(p => p.StudySemester)
                    .HasForeignKey(d => d.StudyplanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudySemester_StudyPlan");
            });

            modelBuilder.Entity<StudyYear>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Subjects>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .HasName("UniqueCode")
                    .IsUnique();

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SubjectCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.StudySemester)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.StudySemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Subjects_StudySemester");

                entity.HasOne(d => d.SubjectType)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.SubjectTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Subjects_SubjectType");
            });

            modelBuilder.Entity<SubjectType>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Trasmentd>(entity =>
            {
                entity.HasKey(e => e.Ssn);

                entity.Property(e => e.Ssn)
                    .HasColumnName("SSN")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.HasOne(d => d.Collage)
                    .WithMany(p => p.Trasmentd)
                    .HasForeignKey(d => d.CollageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trasmentd_Colleges");

                entity.HasOne(d => d.SsnNavigation)
                    .WithOne(p => p.Trasmentd)
                    .HasForeignKey<Trasmentd>(d => d.Ssn)
                    .HasConstraintName("\\");
            });

            modelBuilder.Entity<TypeOfRegistar>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.GroupId });

                entity.ToTable("User_Group");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.Property(e => e.GroupId).HasColumnName("Group_ID");

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.UserGroup)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Group_Group");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserGroup)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_user_id1");
            });

            modelBuilder.Entity<Years>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.HasOne(d => d.ExamSystemNavigation)
                    .WithMany(p => p.Years)
                    .HasForeignKey(d => d.ExamSystem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Years_ExamSystem");

                entity.HasOne(d => d.YearSystemNavigation)
                    .WithMany(p => p.Years)
                    .HasForeignKey(d => d.YearSystem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Years_YearSystem");
            });

            modelBuilder.Entity<YearSystem>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}

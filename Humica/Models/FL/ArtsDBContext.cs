namespace Humica.Models.FL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration;

    public partial class ArtsDBContext : DbContext
    {
        static ArtsDBContext()
        {
            Database.SetInitializer<ArtsDBContext>(null);
        }

        public ArtsDBContext() : base("ArtsDBConnectionString") { }

        public DbSet<Art> Arts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ArtMap());
        }
    }

    public partial class Art
    {
        public Nullable<System.DateTime> LastWriteTime { get; set; }
        public string Name { get; set; }
        public int ID { get; set; }
        public Nullable<int> ParentID { get; set; }
        public Nullable<bool> IsFolder { get; set; }
        public byte[] Data { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public byte[] SSMA_TimeStamp { get; set; }
    }

    public class ArtMap : EntityTypeConfiguration<Art>
    {
        public ArtMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(100);

            this.Property(t => t.SSMA_TimeStamp)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            // Table & Column Mappings
            this.ToTable("Arts");
            this.Property(t => t.LastWriteTime).HasColumnName("LastWriteTime");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ParentID).HasColumnName("ParentID");
            this.Property(t => t.IsFolder).HasColumnName("IsFolder");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.OptimisticLockField).HasColumnName("OptimisticLockField");
            this.Property(t => t.GCRecord).HasColumnName("GCRecord");
            this.Property(t => t.SSMA_TimeStamp).HasColumnName("SSMA_TimeStamp");
        }
    }

}
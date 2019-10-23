using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Server.Database.Models;

namespace Server.Database
{
    public partial class Projet3DbContext : DbContext
    {
        public Projet3DbContext()
        {
        }

        public Projet3DbContext(DbContextOptions<Projet3DbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Message> Message { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=projet3.mysql.database.azure.com;port=3306;database=projet3;user=projet3_user@projet3;password=admin_password3");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => new { e.SenderId, e.Timestamp });

                entity.ToTable("message", "projet3");

                entity.Property(e => e.SenderId)
                    .HasColumnName("sender_id")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.SenderUsername)
                    .HasColumnName("sender_username")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Channel)
                    .HasColumnName("channel")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("message")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}

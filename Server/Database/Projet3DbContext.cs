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

        public virtual DbSet<Channel> Channel { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<UserChannel> UserChannel { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=projet3.mysql.database.azure.com;port=3306;user=projet3_user@projet3;password=admin_password3;database=projet3");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Channel>(entity =>
            {
                entity.ToTable("channel", "projet3");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(36)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsLobby)
                    .HasColumnName("is_lobby")
                    .HasColumnType("tinyint(4)");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => new {e.SenderId, e.Timestamp});

                entity.ToTable("message", "projet3");

                entity.HasIndex(e => e.ChannelId)
                    .HasName("channel_id_message_idx");

                entity.Property(e => e.SenderId)
                    .HasColumnName("sender_id")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ChannelId)
                    .IsRequired()
                    .HasColumnName("channel_id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasColumnType("blob");

                entity.Property(e => e.SenderUsername)
                    .IsRequired()
                    .HasColumnName("sender_username")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.HasOne(d => d.ChannelNavigation)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.ChannelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("channel_id_message");
            });

            modelBuilder.Entity<UserChannel>(entity =>
            {
                entity.HasKey(e => new {e.UserId, e.ChannelId});

                entity.ToTable("user_channel", "projet3");

                entity.HasIndex(e => e.ChannelId)
                    .HasName("channel_id_user_idx");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.ChannelId)
                    .HasColumnName("channel_id")
                    .HasColumnType("varchar(36)");

                entity.HasOne(d => d.Channel)
                    .WithMany(p => p.UserChannel)
                    .HasForeignKey(d => d.ChannelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("channel_id_user");
            });
        }
    }
}
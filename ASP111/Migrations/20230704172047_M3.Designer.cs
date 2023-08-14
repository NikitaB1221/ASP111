﻿// <auto-generated />
using System;
using ASP111.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ASP111.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230704172047_M3")]
    partial class M3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("asp111")
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ASP111.Data.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DeleteDt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("longtext");

                    b.Property<Guid>("ReplyId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ThemeId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("UpdateDt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Comments", "asp111");
                });

            modelBuilder.Entity("ASP111.Data.Entities.Rate", b =>
                {
                    b.Property<Guid>("ItemId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Moment")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.HasKey("ItemId", "UserId");

                    b.ToTable("Rates", "asp111");
                });

            modelBuilder.Entity("ASP111.Data.Entities.Section", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreateDt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DeleteDt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdateDt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Sections", "asp111");
                });

            modelBuilder.Entity("ASP111.Data.Entities.Theme", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DeleteDt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("TopicId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("UpdateDt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Themes", "asp111");
                });

            modelBuilder.Entity("ASP111.Data.Entities.Topic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DeleteDt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("longtext");

                    b.Property<Guid>("SectionId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdateDt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Topics", "asp111");
                });

            modelBuilder.Entity("ASP111.Data.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Avatar")
                        .HasColumnType("longtext");

                    b.Property<string>("ConfirmCode")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("CreatedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DeletedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("LastUpdatedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users", "asp111");
                });

            modelBuilder.Entity("ASP111.Data.Entities.Visit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Moment")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("Visits", "asp111");
                });

            modelBuilder.Entity("ASP111.Data.Entities.Section", b =>
                {
                    b.HasOne("ASP111.Data.Entities.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("ASP111.Data.Entities.Topic", b =>
                {
                    b.HasOne("ASP111.Data.Entities.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });
#pragma warning restore 612, 618
        }
    }
}

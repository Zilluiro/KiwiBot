﻿// <auto-generated />
using KiwiBot.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KiwiBot.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20201113114855_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("KiwiBot.Data.Entities.Booru", b =>
                {
                    b.Property<int>("BooruId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<bool>("ApiCompatible")
                        .HasColumnType("bit");

                    b.Property<string>("ApiEndpoint")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BooruName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileUrlKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TagsKey")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BooruId");

                    b.ToTable("Boorus");
                });

            modelBuilder.Entity("KiwiBot.Data.Entities.Chat", b =>
                {
                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<int>("BooruId")
                        .HasColumnType("int");

                    b.Property<int>("ChatMode")
                        .HasColumnType("int");

                    b.HasKey("ChatId");

                    b.HasIndex("BooruId");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("KiwiBot.Data.Entities.Chat", b =>
                {
                    b.HasOne("KiwiBot.Data.Entities.Booru", "Booru")
                        .WithMany()
                        .HasForeignKey("BooruId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booru");
                });
#pragma warning restore 612, 618
        }
    }
}

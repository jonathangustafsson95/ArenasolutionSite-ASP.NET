﻿// <auto-generated />
using System;
using DataAccessLayer.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Data_Access_Layer.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200318111341_TournamentChanges")]
    partial class TournamentChanges
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CommonLibrary.Advert", b =>
                {
                    b.Property<int>("advertId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdvertName")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("BeginDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeadlineDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("Sponsoring")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<byte[]>("productImage")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("advertId");

                    b.HasIndex("UserId");

                    b.ToTable("Adverts");
                });

            modelBuilder.Entity("CommonLibrary.Knockout", b =>
                {
                    b.Property<int>("KnockoutId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("LeftNodeKnockoutId")
                        .HasColumnType("int");

                    b.Property<int?>("RightNodeKnockoutId")
                        .HasColumnType("int");

                    b.Property<int?>("player1TournamentId")
                        .HasColumnType("int");

                    b.Property<int?>("player1UserId")
                        .HasColumnType("int");

                    b.Property<int?>("player2TournamentId")
                        .HasColumnType("int");

                    b.Property<int?>("player2UserId")
                        .HasColumnType("int");

                    b.HasKey("KnockoutId");

                    b.HasIndex("LeftNodeKnockoutId");

                    b.HasIndex("RightNodeKnockoutId");

                    b.HasIndex("player1TournamentId", "player1UserId");

                    b.HasIndex("player2TournamentId", "player2UserId");

                    b.ToTable("Knockouts");
                });

            modelBuilder.Entity("CommonLibrary.League", b =>
                {
                    b.Property<string>("LeagueId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LeagueName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LeagueOwnerId")
                        .HasColumnType("int");

                    b.HasKey("LeagueId");

                    b.HasIndex("LeagueOwnerId");

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("CommonLibrary.LeagueMember", b =>
                {
                    b.Property<string>("LeagueId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("Applicant")
                        .HasColumnType("bit");

                    b.HasKey("LeagueId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("LeagueMembers");
                });

            modelBuilder.Entity("CommonLibrary.Tournament", b =>
                {
                    b.Property<int>("TournamentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CurrentPlayers")
                        .HasColumnType("int");

                    b.Property<int?>("CurrentRoundKnockoutId")
                        .HasColumnType("int");

                    b.Property<string>("LeagueId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("MaxPlayer")
                        .HasColumnType("int");

                    b.Property<string>("TournamentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TournamentStyle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TournamentId");

                    b.HasIndex("CurrentRoundKnockoutId");

                    b.HasIndex("LeagueId");

                    b.ToTable("Tournaments");
                });

            modelBuilder.Entity("CommonLibrary.TournamentPlayer", b =>
                {
                    b.Property<int>("TournamentId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("TournamentPlayerId")
                        .HasColumnType("int");

                    b.HasKey("TournamentId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("TournamentPlayers");
                });

            modelBuilder.Entity("CommonLibrary.TournamentStyle", b =>
                {
                    b.Property<int>("TournamentStyleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("TournamentStyles")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TournamentStyleId");

                    b.ToTable("TournamentStyles");
                });

            modelBuilder.Entity("CommonLibrary.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("CommonLibrary.Advertiser", b =>
                {
                    b.HasBaseType("CommonLibrary.User");

                    b.Property<float>("Balance")
                        .HasColumnType("real");

                    b.HasDiscriminator().HasValue("Advertiser");
                });

            modelBuilder.Entity("CommonLibrary.LeagueOwner", b =>
                {
                    b.HasBaseType("CommonLibrary.User");

                    b.HasDiscriminator().HasValue("LeagueOwner");
                });

            modelBuilder.Entity("CommonLibrary.Operator", b =>
                {
                    b.HasBaseType("CommonLibrary.User");

                    b.Property<bool>("Admin")
                        .HasColumnType("bit");

                    b.HasDiscriminator().HasValue("Operator");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            Password = "123",
                            UserName = "Jonte",
                            Admin = false
                        });
                });

            modelBuilder.Entity("CommonLibrary.Player", b =>
                {
                    b.HasBaseType("CommonLibrary.User");

                    b.Property<string>("ConnectionId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurrentGameType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("Player");
                });

            modelBuilder.Entity("CommonLibrary.Advert", b =>
                {
                    b.HasOne("CommonLibrary.Advertiser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CommonLibrary.Knockout", b =>
                {
                    b.HasOne("CommonLibrary.Knockout", "LeftNode")
                        .WithMany()
                        .HasForeignKey("LeftNodeKnockoutId");

                    b.HasOne("CommonLibrary.Knockout", "RightNode")
                        .WithMany()
                        .HasForeignKey("RightNodeKnockoutId");

                    b.HasOne("CommonLibrary.TournamentPlayer", "player1")
                        .WithMany()
                        .HasForeignKey("player1TournamentId", "player1UserId");

                    b.HasOne("CommonLibrary.TournamentPlayer", "player2")
                        .WithMany()
                        .HasForeignKey("player2TournamentId", "player2UserId");
                });

            modelBuilder.Entity("CommonLibrary.League", b =>
                {
                    b.HasOne("CommonLibrary.LeagueOwner", "LeagueOwner")
                        .WithMany("Leagues")
                        .HasForeignKey("LeagueOwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CommonLibrary.LeagueMember", b =>
                {
                    b.HasOne("CommonLibrary.League", "League")
                        .WithMany("LeagueMembers")
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CommonLibrary.Player", "Player")
                        .WithMany("LeagueMemberShips")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("CommonLibrary.Tournament", b =>
                {
                    b.HasOne("CommonLibrary.Knockout", "CurrentRound")
                        .WithMany()
                        .HasForeignKey("CurrentRoundKnockoutId");

                    b.HasOne("CommonLibrary.League", "League")
                        .WithMany()
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CommonLibrary.TournamentPlayer", b =>
                {
                    b.HasOne("CommonLibrary.Tournament", "Tournament")
                        .WithMany("TournamentPlayers")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CommonLibrary.Player", "Player")
                        .WithMany("TournamentMemberships")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

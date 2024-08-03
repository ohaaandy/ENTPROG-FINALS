using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBuddies.DataModel
{
    internal class AppDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Andy SQL
            //optionsBuilder.UseSqlServer(
            //    "server=APOL\\SQLEXPRESS;" +
            //    "database=RunBuddies;" +
            //    "integrated security=SSPI;" +
            //    "trustservercertificate=true");

            //Monty SQL

            //Yash SQL
            optionsBuilder.UseSqlServer("server=LAPTOP-A7QL1S73\\SQLEXPRESS;Database=ENTPROG_Finals;integrated security=sspi;trustservercertificate=true");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //codes for implementing entity relationships
            //user to club moderator m to o
            modelBuilder.Entity<ClubModerator>()
                .HasOne(p => p.User)
                .WithMany(p => p.ClubModerators)
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            //user to to club member
            modelBuilder.Entity<ClubMember>()
                .HasOne(p => p.User)
                .WithMany(p => p.ClubMembers)
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            //club moderator to club
            modelBuilder.Entity<Club>()
                .HasOne(p => p.ClubModerator)
                .WithMany(p => p.Clubs)
                .HasForeignKey(p => p.ClubModeratorID)
                .OnDelete(DeleteBehavior.Restrict);

            //club member to club
            modelBuilder.Entity<Club>()
                .HasOne(p => p.ClubMember)
                .WithMany(p => p.Clubs)
                .HasForeignKey(p => p.ClubMemberID)
                .OnDelete(DeleteBehavior.Restrict);

            //User to Event
            modelBuilder.Entity<Event>()
                .HasOne(p => p.User)
                .WithMany(p => p.Events)
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            //Club to Event
            modelBuilder.Entity<Event>()
                .HasOne(p => p.Club)
                .WithMany(p => p.Events)
                .HasForeignKey(p => p.ClubID)
                .OnDelete(DeleteBehavior.Restrict);

            //Event to Leaderboard o to o
            modelBuilder.Entity<Leaderboard>()
                .HasOne(p => p.Events)
                .WithOne(p => p.Leaderboards)
                .HasForeignKey<Leaderboard>(p => p.EventID)
                .OnDelete(DeleteBehavior.Restrict);

            //User to Buddy partner o to m
            modelBuilder.Entity<BuddyPartner>()
                .HasOne(p => p.User)
                .WithMany(p => p.BuddyPartners)
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            //Buddy partner to Buddy session
            modelBuilder.Entity<BuddySession>()
                .HasOne(p => p.BuddyPartner)
                .WithMany(p => p.BuddySessions)
                .HasForeignKey(p => p.BuddyID)
                .OnDelete(DeleteBehavior.Restrict);

            //Verification to Buddy session o to o
            modelBuilder.Entity<BuddySession>()
                .HasOne(p => p.Verification)
                .WithOne(p => p.BuddySessions)
                .HasForeignKey<BuddySession>(p => p.VerificationID)
                .OnDelete(DeleteBehavior.Restrict);
        }

        DbSet<User> Users { get; set; }
        DbSet<ClubModerator> ClubModerators { get; set; }
        DbSet<ClubMember> ClubMembers { get; set; }
        DbSet<Club> Clubs { get; set; }
        DbSet<Event> Events { get; set; }
        DbSet<BuddyPartner> BuddyPartners { get; set; }
        DbSet<Leaderboard> Leaderboards { get; set; }
        DbSet<Verification> Verifications { get; set; }
        DbSet<BuddySession> BuddySessions { get; set; }
    }
}
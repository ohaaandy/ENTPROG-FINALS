using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBuddies.DataModel
{
    public class AppDBContext : IdentityDbContext<User>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }

        public void SeedSampleData()
        {
            if (!Users.Any())
            {
                Users.AddRange(
                    new User
                    {
                        UserName = "john_doe",
                        PasswordHash = "password123", // In a real app, make sure to hash passwords
                        Email = "john@example.com",
                        FirstName = "John",
                        LastName = "Doe",
                        Birthday = new DateOnly(1990, 1, 1),
                        Gender = "Male",
                        PhoneNumber = "1234567890",
                        RunnerLevel = "Beginner",
                        Schedule = new DateOnly(2023, 1, 2), // This represents Monday
                        Location = "Manila",
                        Distance = 5
                    },
                    new User
                    {
                        UserName = "jane_smith",
                        PasswordHash = "password456",
                        Email = "jane@example.com",
                        FirstName = "Jane",
                        LastName = "Smith",
                        Birthday = new DateOnly(1992, 5, 15),
                        Gender = "Female",
                        PhoneNumber = "1234567890",
                        RunnerLevel = "Intermediate",
                        Schedule = new DateOnly(2023, 1, 4), // This represents Wednesday
                        Location = "Quezon City",
                        Distance = 10
                    },
                    new User
                    {
                        UserName = "mike_johnson",
                        PasswordHash = "password789",
                        Email = "mike@example.com",
                        FirstName = "Mike",
                        LastName = "Johnson",
                        Birthday = new DateOnly(1988, 9, 30),
                        Gender = "Male",
                        PhoneNumber = "1234567890",
                        RunnerLevel = "Advanced",
                        Schedule = new DateOnly(2023, 1, 7), // This represents Saturday
                        Location = "Makati",
                        Distance = 15
                    }
                );

                SaveChanges();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //Monty SQL
                optionsBuilder.UseSqlServer(
                    "Server = ANDREIPC\\SQLEXPRESS;" +
                    "Database = ENTPROG_FINALS;" +
                    "Integrated Security = SSPI;" +
                    "TrustServerCertificate = true");
            }
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

            modelBuilder.Entity<BuddyInvitation>()
        .HasOne(bi => bi.Sender)
        .WithMany(u => u.SentBuddyInvitations)
        .HasForeignKey(bi => bi.SenderID)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BuddyInvitation>()
                .HasOne(bi => bi.Receiver)
                .WithMany(u => u.ReceivedBuddyInvitations)
                .HasForeignKey(bi => bi.ReceiverID)
                .OnDelete(DeleteBehavior.Restrict);
        }

        //public DbSet<User> AspNetUsers { get; set; }
        public DbSet<ClubModerator> ClubModerators { get; set; }
        public DbSet<ClubMember> ClubMembers { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<BuddyPartner> BuddyPartners { get; set; }
        public DbSet<Leaderboard> Leaderboards { get; set; }
        public DbSet<Verification> Verifications { get; set; }
        public DbSet<BuddySession> BuddySessions { get; set; }
        public DbSet<BuddyInvitation> BuddyInvitations { get; set; }

    }
}
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> userManager;
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            this.userManager = userManager;
        }

        public async Task SeedSampleDataAsync()
        {
            if (!Users.Any())
            {
                var users = new List<User>
                {
                    new User
                    {
                        UserName = "john_doe",
                        Email = "john@example.com",
                        FirstName = "John",
                        LastName = "Doe",
                        Birthday = new DateOnly(1990, 1, 1),
                        Gender = "Male",
                        PhoneNumber = "1234567890",
                        RunnerLevel = "Beginner",
                        Schedule = new DateOnly(2023, 1, 2),
                        Location = "Manila",
                        Distance = 5
                    },
                    new User
                    {
                        UserName = "jane_smith",
                        Email = "jane@example.com",
                        FirstName = "Jane",
                        LastName = "Smith",
                        Birthday = new DateOnly(1992, 5, 15),
                        Gender = "Female",
                        PhoneNumber = "9876543210",
                        RunnerLevel = "Intermediate",
                        Schedule = new DateOnly(2023, 1, 4),
                        Location = "Quezon City",
                        Distance = 10
                    },
                    new User
                    {
                        UserName = "mike_johnson",
                        Email = "mike@example.com",
                        FirstName = "Mike",
                        LastName = "Johnson",
                        Birthday = new DateOnly(1988, 9, 30),
                        Gender = "Male",
                        PhoneNumber = "5555555555",
                        RunnerLevel = "Advanced",
                        Schedule = new DateOnly(2023, 1, 7),
                        Location = "Makati",
                        Distance = 15
                    }
                };

                foreach (var user in users)
                {
                    var result = await userManager.CreateAsync(user, "Password123!");
                    if (!result.Succeeded)
                    {
                        // Log the error or handle it appropriately
                        Console.WriteLine($"Failed to create user {user.UserName}: {string.Join(", ", result.Errors)}");
                    }
                }

                // Verify that users were actually created
                var createdUsers = await Users.ToListAsync();
                Console.WriteLine($"Created {createdUsers.Count} users.");
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //Monty SQL
                optionsBuilder.UseSqlServer(
                    "server=LAPTOP-A7QL1S73\\SQLEXPRESS;Database=ENTPROG_Finals;integrated security=sspi;trustservercertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Existing relationships
            modelBuilder.Entity<ClubModerator>()
                .HasOne(p => p.User)
                .WithMany(p => p.ClubModerators)
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Modified: Many-to-Many relationship between Club and ClubMember
            modelBuilder.Entity<Club>()
                .HasMany(c => c.ClubMembers)
                .WithMany(m => m.Clubs)
                .UsingEntity(j => j.ToTable("ClubMemberships"));

            modelBuilder.Entity<Club>()
                .HasOne(p => p.ClubModerator)
                .WithMany(p => p.Clubs)
                .HasForeignKey(p => p.ClubModeratorID)
                .OnDelete(DeleteBehavior.Restrict);

            // Remove this as it's now a many-to-many relationship
            // modelBuilder.Entity<Club>()
            //     .HasOne(p => p.ClubMember)
            //     .WithMany(p => p.Clubs)
            //     .HasForeignKey(p => p.ClubMemberID)
            //     .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
                .HasOne(p => p.User)
                .WithMany(p => p.Events)
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
                .HasOne(p => p.Club)
                .WithMany(p => p.Events)
                .HasForeignKey(p => p.ClubID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Leaderboard>()
                .HasOne(p => p.Events)
                .WithOne(p => p.Leaderboards)
                .HasForeignKey<Leaderboard>(p => p.EventID)
                .OnDelete(DeleteBehavior.Restrict);

            // BuddyPartner relationships
            modelBuilder.Entity<BuddyPartner>()
                .HasOne(bp => bp.User1)
                .WithMany()
                .HasForeignKey(bp => bp.User1ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BuddyPartner>()
                .HasOne(bp => bp.User2)
                .WithMany()
                .HasForeignKey(bp => bp.User2ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BuddySession>()
                .HasOne(p => p.BuddyPartner)
                .WithMany(p => p.BuddySessions)
                .HasForeignKey(p => p.BuddyID)
                .OnDelete(DeleteBehavior.Restrict);

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
using Gymbuddy.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gymbuddy
{
    public class DB : DbContext
    {

        public DB(DbContextOptions<DB> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
            SeedData(modelBuilder);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Post> Posts { get; set; }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Ajdin",
                    LastName = "Bulut",
                    UserName = "admin",
                    PasswordHash = "woHbW3yWpukgOyG2slgRbtGtjQuSsZCNP1k9/P9vvJI=",
                    PasswordSalt = "+6peAHb8PQPB4+VlYvcd3Q==",
                    Age = 22,
                    Email = "ajdinbulut@gmail.com",


                }
                );
            modelBuilder.Entity<Role>().HasData(new List<Role>{
                new Role
                {
                    Id = 1,
                    Name = "User"
                },
                new Role
                {
                    Id = 2,
                    Name = "Admin"
                }
            });
            modelBuilder.Entity<UserRole>().HasData(new List<UserRole>
            {
                new UserRole
                {
                    Id = 1,
                    UserId = 1,
                    RoleId = 1
                },
                new UserRole
                {
                    Id = 2,
                    UserId = 1,
                    RoleId = 2
                }
            });

        }
    }
}

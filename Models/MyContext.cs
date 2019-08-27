using Microsoft.EntityFrameworkCore;

namespace retake.Models
{
    public class MyContext: DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }

        public DbSet<UserReg> Users {get; set;}

        public DbSet<Hobby> Hobbies {get; set;}

        public DbSet<Favorite> Faves {get; set;}
    }
}
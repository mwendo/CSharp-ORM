using Microsoft.EntityFrameworkCore;

namespace BeltExam.Models
{
    public class MyContext : DbContext
    {
        public MyContext (DbContextOptions options) : base(options) { }

        public DbSet<User> Users {get;set;}
        public DbSet<Entertainment> Entertainments {get;set;}
        public DbSet<Participant> Participants {get;set;}
    }
}
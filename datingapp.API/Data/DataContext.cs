using datingapp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace datingapp.API.Data
{
    public class DataContext : DbContext
    {
         public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Value> Values { get; set; }

        public DbSet<User> User {get; set;}

        public DbSet<Photo> Photos { get; set; }

        public DbSet<Like> Likes {get; set;}
        public DbSet<Message> Message {get; set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Like>()
            .HasKey(k => new { k.LikerId, k.LikeeId});

            builder.Entity<Like>()
            .HasOne( u => u.Likee)
            .WithMany( u => u.Likers)
            .HasForeignKey( u => u.LikeeId)
            .OnDelete(DeleteBehavior.Restrict);

             builder.Entity<Like>()
            .HasOne( u => u.Liker)
            .WithMany( u => u.Likees)
            .HasForeignKey( u => u.LikerId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
            .HasOne(u => u.Sender)
            .WithMany(u => u.MessageSent)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
            .HasOne(u => u.Recipient)
            .WithMany(u => u.MessageReceived)
            .OnDelete(DeleteBehavior.Restrict);




        }

        
    }
}
using Microsoft.EntityFrameworkCore;
using ALEXforums.Data.Entity;

namespace ALEXforums.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Entity.User>    Users    { get; set; }

        public DbSet<Entity.Theme>   Themes   { get; set; }

        public DbSet<Entity.Message> Messages { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }
       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.HasDefaultSchema("Testforum");
             modelBuilder  // вказуємо композитний ключ для таблиці Message
                 .Entity<Entity.Message>()
                .HasKey(nameof(Entity.Message.ThemeId), nameof(Entity.Message.UserId));
            // Встановлюємо зв'язки між таблицями Theme, Message та User
            /*   modelBuilder.Entity<Theme>()
                   .HasOne(t => t.User)
                   .WithMany(u => u.Themes)
                   .HasForeignKey(t => t.UserId);

               modelBuilder.Entity<Message>()
                   .HasOne(m => m.User)
                   .WithMany(u => u.Messages)
                   .HasForeignKey(m => m.UserId);

               modelBuilder.Entity<Message>()
                   .HasOne(m => m.Theme)
                   .WithMany(t => t.Messages)
                   .HasForeignKey(m => m.ThemeId); */
        }
    }
}

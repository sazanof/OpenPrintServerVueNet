using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenPrintServerVueNet.Models;
using OpenPrintServerVueNet.Server.Models;

namespace OpenPrintServerVueNet.Server.Contexts
{

    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Job> Jobs { get; set; }

        public DbSet<Config> Config { get; set; }

        public DbSet<Printer> Printers { get; set; }

        public DbSet<PrinterPort> PrinterPorts { get; set; }

        public DbSet<Consumables> Consumables { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureCreated();   // создаем базу данных при первом обращении
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /* modelBuilder.Entity<Printer>()
                 .HasMany(c => c.Ports)
                 .WithOne(o => o.Printer)
                 .OnDelete(DeleteBehavior.Cascade);

             modelBuilder.Entity<Printer>()
                 .HasMany(c => c.Consumables)
                 .WithOne(o => o.Printer)
                 .OnDelete(DeleteBehavior.Cascade);*/

            modelBuilder.Entity<Job>()
            .HasOne(u => u.Printer)
            .WithMany(c => c.Jobs)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Consumables>()
            .HasOne(u => u.Printer)
            .WithMany(c => c.Consumables)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PrinterPort>()
            .HasOne(u => u.Printer)
            .WithMany(c => c.Ports)
            .OnDelete(DeleteBehavior.Cascade);
            /*
                        modelBuilder.Entity<Job>()
                            .HasOne(j => j.Printer);*/
        }

    }

}

﻿using Microsoft.AspNetCore.Identity;
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

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureCreated();   // создаем базу данных при первом обращении
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Printer>()
                .HasMany(c => c.Ports)
                .WithOne(o => o.Printer);
        }

    }

}

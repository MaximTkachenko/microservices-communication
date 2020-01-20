using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Tickets.Common
{
    public class TicketsDb : DbContext
    {
        public TicketsDb(DbContextOptions<TicketsDb> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

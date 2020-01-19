using System;
using Microsoft.EntityFrameworkCore;

namespace Tickets.Common
{
    public class TicketsDb : DbContext
    {
        public TicketsDb(DbContextOptions<TicketsDb> options) : base(options)
        { }
    }
}

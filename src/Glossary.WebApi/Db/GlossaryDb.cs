using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Glossary.WebApi.Db
{
    public class GlossaryDb : DbContext
    {
        public GlossaryDb(DbContextOptions<GlossaryDb> options) : base(options)
        { }
    }
}

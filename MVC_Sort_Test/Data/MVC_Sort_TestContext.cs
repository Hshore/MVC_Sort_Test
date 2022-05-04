#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVC_Sort_Test.Models;

namespace MVC_Sort_Test.Data
{
    public class MVC_Sort_TestContext : DbContext
    {
        public MVC_Sort_TestContext (DbContextOptions<MVC_Sort_TestContext> options)
            : base(options)
        {
        }

        public DbSet<MVC_Sort_Test.Models.SortEntry> SortEntry { get; set; }
    }
}

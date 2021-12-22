using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HintLib.Context
{
    public class HintContext : DbContext
    {
        public DbSet<Hint>? Hints { get; set; }
        public DbSet<HintProject>? HintProjects { get; set; }
        public DbSet<ProjectFilename>? ProjectFilenames { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // This is a 'home' project so the connection string doesn't need to be hidden.
            optionsBuilder.UseSqlServer(@"Data Source=localhost\sqlexpress;Initial Catalog=HintProjects;Integrated Security=True");
        }

        internal static IEnumerable<HintProject>? GetHintProjects()
        {
            using (var context = new HintContext())
            {
                if (context != null && context.HintProjects != null)
                    return context.HintProjects?
                        .AsNoTracking()
                        .Include("Hints")
                        .Include("ProjectFilenames")
                        .ToArray();
                else return null;
            }
        }
    }
}

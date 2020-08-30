using Lendee.Core.DataAccess.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Lendee.Core.DataAccess
{
    public class LendeeContext : DbContext
    {
        public LendeeContext(DbContextOptions<LendeeContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("lendee");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContractMap).Assembly);
        }
    }
}
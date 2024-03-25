using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WorkFlowLog.Data.Entities;

namespace WorkFlowLog.Data
{
    public class WorkFlowDbContext : DbContext   
    {
        DbSet<Order> Orders => Set<Order>();
        DbSet<Employee> Employees => Set<Employee>();

        public WorkFlowDbContext(DbContextOptions<WorkFlowDbContext> options ) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("WorkFlowLogDb");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using WorkFlowLog.DataAccess.Data.Entities;

namespace WorkFlowLog.DataAccess.Data
{
    public class WorkFlowDbContext : DbContext
    {
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Operation> Operations => Set<Operation>();

        public WorkFlowDbContext(DbContextOptions<WorkFlowDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-4SSQKEB\\SQLEXPRESS01;Initial Catalog=WorkFLowLogStorage;Integrated Security=True;Encrypt=False");
        }
    }
}

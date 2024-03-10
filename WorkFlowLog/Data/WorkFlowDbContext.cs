﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WorkFlowLog.Entities;

namespace WorkFlowLog.Data
{
    public class WorkFlowDbContext : DbContext   
    {
        DbSet<Order> Orders => Set<Order>();
        DbSet<Employee> Employees => Set<Employee>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("WorkFlowLogDb");
        }
    }
}
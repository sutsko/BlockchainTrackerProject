using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netCoreMVCCrud1.Models
{
    public class ProjectContext:DbContext
    {
        //In the following cinstructor we will add the options for the database context. This means the provider (SQL Server)
        // and the connection string, which can be found in appsettings.json.
        public ProjectContext(DbContextOptions<ProjectContext> options):base(options)
        {
              
        }

        // a DB set is a representation of a table in the database
        public DbSet<Project> Projects { get; set; } 
        public DbSet<Industry> Industries { get; set; } 
        public DbSet<Organization> Organizations { get; set; } 
        public DbSet<Sector> Sectors { get; set; } 
        public DbSet<Usecase> Usecases { get; set; } 
   
    }
}

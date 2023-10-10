using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using refactored.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactored.dal
{
    public class RefactoredDBContext : DbContext
    {

        public DbSet<Product> Product { get; set; }
        public DbSet<ProductOption> ProductOption { get; set; }

        IWebHostEnvironment _webHostEnvironment;
        public  RefactoredDBContext(IWebHostEnvironment webHostEnvironment)

        {
            _webHostEnvironment = webHostEnvironment;
        }


        public RefactoredDBContext(DbContextOptions<RefactoredDBContext> options, IWebHostEnvironment webHostEnvironment) : base(options)
        {
            _webHostEnvironment = webHostEnvironment;

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
 
            //string sqlConnection = "Server=(localdb)\\MSSQLLocalDB;Database={DataDirectory};Trusted_Connection=True";
            //string path = _webHostEnvironment.ContentRootPath + @"\App_Data\Database.mdf";
            //var connstr = sqlConnection.Replace("{DataDirectory}", path);

            //// Configure your SQL Server Express connection string here
            //optionsBuilder.UseSqlServer(connstr);
        }
    }
}

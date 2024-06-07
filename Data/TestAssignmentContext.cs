using Microsoft.EntityFrameworkCore;
using System;

namespace TestAssignmentGrcp.Data
{
    public class TestAssignmentContext : DbContext
    {
        private readonly ILogger<TestAssignmentContext> _logger;

        public string DbPath { get; }


        public TestAssignmentContext(IConfiguration configuration, ILogger<TestAssignmentContext> logger)
        {
            _logger = logger;
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var sectioncfg = configuration.GetSection("db");
            DbPath = Path.Join(path, sectioncfg.GetValue<string>("db_name"));

            _logger.LogInformation("{ServiceName} Path to DB: {Path}", nameof(TestAssignmentContext), Path.Join(path, configuration.GetValue<string>("db_name")));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Filename = {DbPath}");

        public DbSet<FiboNumber> FiboNumbers { get; set; }


    }
}

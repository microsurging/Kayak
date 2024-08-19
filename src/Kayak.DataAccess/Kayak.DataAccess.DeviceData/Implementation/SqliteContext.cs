using Kayak.Core.Common.Extensions;
using Kayak.DataAccess.DeviceData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.UserPermission.Implementation
{
    public class SqliteContext : DataContext
    {
        private readonly string? _connstring = "Data Source=KayakData.db;";
        public SqliteContext() { }

        public SqliteContext(string? connstring)
        {
            _connstring = connstring;
        }
       public SqliteContext(DbContextOptions<DataContext> options):base(options)
        {
            _connstring = null;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_connstring!=null)
            {
                optionsBuilder.UseSqlite(_connstring);
            }
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public override async Task InitializeAsync()
        {
            await Database.MigrateAsync();
        }
    }


}

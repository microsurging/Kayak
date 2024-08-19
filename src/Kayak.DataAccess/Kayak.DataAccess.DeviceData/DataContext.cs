using Kayak.DataAccess.DeviceData.Entities; 
using Kayak.DataAccess.UserPermission.Implementation;
using Microsoft.EntityFrameworkCore;
using Surging.Core.CPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData
{
    public class DataContext : DbContext, IDisposable
    {
        public DataContext()
        {

        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public static DataContext Instance()
        {
            return ServiceLocator.GetService<DataContext>(AppConfig.DeviceDataOptions.DatabaseType.ToString())??new DataContext();
        }

        //实体
        public DbSet<SysUser> SysUser { get; set; }

        public DbSet<Device> Device {  get; set; }

        public DbSet<RegistryCenter> RegistryCenter {  get; set; }

        public DbSet<SysOrganization> SysOrganization { get; set; }

        public DbSet<SysDictionary> SysDictionary { get; set; }

        public DbSet<OperateLog> OperateLog { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<ProductCategory> ProductCategory { get; set; }

        public DbSet<Protocol> Protocol { get; set; }

        public DbSet<NetworkPart> NetworkPart { get; set; }

        public virtual async Task InitializeAsync()
        {

              await Database.MigrateAsync();
        }
    }
}

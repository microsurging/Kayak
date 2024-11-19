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
    public class DataContext : DbContext
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

        public DbSet<DeviceEvent> DeviceEvent {  get; set; }

        public DbSet<BlackWhiteList> BlackWhiteList { get; set; }

        public DbSet<Module> Module { get; set; }
        public DbSet<ReportPropertyLog> ReportPropertyLog { get; set; }
        public DbSet<PropertyThreshold> PropertyThreshold { get; set; }

        public DbSet<EventParameter> EventParameter { get; set; }

        public  DbSet<FunctionParameter> FunctionParameter { get; set; }
        public DbSet<SysUser> SysUser { get; set; }

        public DbSet<NetworkLog> NetworkLog { get; set; } 

        public DbSet<DeviceGateway> DeviceGateway { get; set; }

        public DbSet<Device> Device {  get; set; }

        public DbSet<DeviceAccess> DeviceAccess { get; set; }

        public DbSet<SysDataType> SysDataType { get; set; }

        public DbSet<SysUnit> SysUnit { get; set; }

        public DbSet<DeviceConfig> DeviceConfig { get; set; }

        public DbSet<ReportProperty> ReportProperty { get; set; }
        public DbSet<FunctionConfigure> FunctionConfigure {  get; set; }    

        public DbSet<DeviceType> DeviceType {  get; set; }
        public DbSet<EventConfigure> EventConfigure { get; set; }
        public DbSet<PropertyConfigure> PropertyConfigure { get; set; }

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

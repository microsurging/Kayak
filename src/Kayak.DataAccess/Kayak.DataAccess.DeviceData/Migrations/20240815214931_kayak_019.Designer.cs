﻿// <auto-generated />
using System;
using Kayak.DataAccess.UserPermission.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Kayak.DataAccess.DeviceData.Migrations
{
    [DbContext(typeof(SqliteContext))]
    [Migration("20240815214931_kayak_019")]
    partial class kayak_019
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.32");

            modelBuilder.Entity("Kayak.DataAccess.DeviceData.Entities.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Creater")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Remark")
                        .HasColumnType("TEXT")
                        .HasColumnName("Remark");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER")
                        .HasColumnName("State");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Updater")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Component_Device");
                });

            modelBuilder.Entity("Kayak.DataAccess.DeviceData.Entities.NetworkPart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClusterMode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ComponentType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Creater")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("EnableSSL")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Host")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Port")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Remark")
                        .HasColumnType("TEXT")
                        .HasColumnName("Remark");

                    b.Property<string>("RuleScript")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Updater")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Component_NetworkPart");
                });

            modelBuilder.Entity("Kayak.DataAccess.DeviceData.Entities.OperateLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Arguments")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Creater")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Payload")
                        .HasColumnType("TEXT");

                    b.Property<string>("Remark")
                        .HasColumnType("TEXT")
                        .HasColumnName("Remark");

                    b.Property<string>("ReturnType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ReturnValue")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RoutePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long?>("RunTime")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ServiceId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Updater")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Sys_OperateLog");
                });

            modelBuilder.Entity("Kayak.DataAccess.DeviceData.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Creater")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DeviceType")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProductCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Protocol")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Remark")
                        .HasColumnType("TEXT")
                        .HasColumnName("Remark");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER")
                        .HasColumnName("State");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Updater")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Component_Product");
                });

            modelBuilder.Entity("Kayak.DataAccess.DeviceData.Entities.ProductCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CategoryId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Creater")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsChildren")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Remark")
                        .HasColumnType("TEXT")
                        .HasColumnName("Remark");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Updater")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Component_ProductCategory");
                });

            modelBuilder.Entity("Kayak.DataAccess.DeviceData.Entities.Protocol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ComponentType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Creater")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FileAddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProtocolCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProtocolName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Remark")
                        .HasColumnType("TEXT")
                        .HasColumnName("Remark");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Updater")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Component_Protocol");
                });

            modelBuilder.Entity("Kayak.DataAccess.DeviceData.Entities.SysDictionary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("Code");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Creater")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IsShow")
                        .HasColumnType("INTEGER")
                        .HasColumnName("IsShow");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("Name");

                    b.Property<string>("ParentCode")
                        .HasColumnType("varchar(50)")
                        .HasColumnName("ParentCode");

                    b.Property<string>("Remark")
                        .HasColumnType("TEXT")
                        .HasColumnName("Remark");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER")
                        .HasColumnName("State");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Updater")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Value")
                        .HasColumnType("INTEGER")
                        .HasColumnName("Value");

                    b.HasKey("Id");

                    b.ToTable("Sys_Dictionary");
                });

            modelBuilder.Entity("Kayak.DataAccess.DeviceData.Entities.SysOrganization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Contacter")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Creater")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LevelCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Remark")
                        .HasColumnType("TEXT")
                        .HasColumnName("Remark");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SysOrgType")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Updater")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Sys_Organization");
                });

            modelBuilder.Entity("Kayak.DataAccess.DeviceData.Entities.SysUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AlipayToken")
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("Avatar")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Creater")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("QQToken")
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("QRCode")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("RealName")
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("Remark")
                        .HasColumnType("TEXT")
                        .HasColumnName("Remark");

                    b.Property<int>("Sex")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Updater")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("WeChatToken")
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Sys_User");
                });
#pragma warning restore 612, 618
        }
    }
}

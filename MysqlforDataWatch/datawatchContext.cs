using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MysqlforDataWatch
{
    public partial class datawatchContext : DbContext
    {
        public string vehicleid { get; set; }
        public datawatchContext()
        {
        }

        public datawatchContext(DbContextOptions<datawatchContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Brakerecognition> Brakerecognitions { get; set; }
        public virtual DbSet<Bumprecognition> Bumprecognitions { get; set; }
        public virtual DbSet<Gpsrecord> Gpsrecords { get; set; }
        public virtual DbSet<SatictisAnalysisdataAcc> SatictisAnalysisdataAccs { get; set; }
        public virtual DbSet<SatictisAnalysisdataWft> SatictisAnalysisdataWfts { get; set; }
        public virtual DbSet<Pumapermileage> Pumapermileages { get; set; }
        public virtual DbSet<Speeddistribution> Speeddistributions { get; set; }
        public virtual DbSet<Streeringrecognition> Streeringrecognitions { get; set; }
        public virtual DbSet<SysAuthority> SysAuthorities { get; set; }
        public virtual DbSet<Throttlerecognition> Throttlerecognitions { get; set; }
        

        public virtual DbSet<t_vehiclemaster> t_vehiclemaster { get; set; }
        public virtual DbSet<t_vehiclemonitorpara> t_vehiclemonitorpara { get; set; }
        public virtual DbSet<t_vehicleimportpara> t_vehicleimportpara { get; set; }
        public virtual DbSet<t_vehiclecomputepara> t_vehiclecomputepara { get; set; }
        public virtual DbSet<EngineRpmDistribution_Time> EngineRpmDistribution_Time { get; set; }
        public virtual DbSet<EngineRpmDistribution_Distance> EngineRpmDistribution_Distance { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;user id=root;password=Mxz04122465;database=datawatch", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.20-mysql")).ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8");

            modelBuilder.Entity<Brakerecognition>(entity =>
            {
                entity.ToTable("brakerecognition");

                entity.Property(e => e.Id)
                    .HasMaxLength(64)
                    .HasColumnName("id");

                entity.Property(e => e.BrakeAcc).HasColumnType("double(64,2)");

                entity.Property(e => e.Datadate)
                    .HasColumnType("datetime")
                    .HasColumnName("datadate");

                entity.Property(e => e.Filename)
                    .HasMaxLength(64)
                    .HasColumnName("filename");

                entity.Property(e => e.VehicleId)
                    .HasMaxLength(64)
                    .HasColumnName("VehicleID");
            });

            modelBuilder.Entity<Bumprecognition>(entity =>
            {
                entity.ToTable("bumprecognition");

                entity.Property(e => e.Id)
                    .HasMaxLength(64)
                    .HasColumnName("id");

                entity.Property(e => e.BumpAcc).HasColumnType("double(64,2)");

                entity.Property(e => e.Datadate)
                    .HasColumnType("datetime")
                    .HasColumnName("datadate");

                entity.Property(e => e.Filename)
                    .HasMaxLength(64)
                    .HasColumnName("filename");

                entity.Property(e => e.VehicleId)
                    .HasMaxLength(64)
                    .HasColumnName("VehicleID");
            });

            modelBuilder.Entity<Gpsrecord>(entity =>
            {
                entity.HasKey(e => e.Key)
                    .HasName("PRIMARY");

                entity.ToTable($"gpsrecord_{vehicleid}");

                entity.Property(e => e.Key)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("key");

                entity.Property(e => e.Datadate)
                    .HasColumnType("datetime")
                    .HasColumnName("datadate");

                entity.Property(e => e.Filename)
                    .HasMaxLength(64)
                    .HasColumnName("filename");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("id");

                entity.Property(e => e.Lat).HasColumnType("double(64,10)");

                entity.Property(e => e.Lon).HasColumnType("double(64,10)");

                entity.Property(e => e.Speed).HasColumnType("double(64,0)");

                entity.Property(e => e.VehicleId)
                    .HasMaxLength(64)
                    .HasColumnName("VehicleID");
            });

            modelBuilder.Entity<SatictisAnalysisdataAcc>(entity =>
            {
                entity.ToTable("satictis_analysisdata_acc");

                entity.Property(e => e.Id)
                    .HasMaxLength(64)
                    .HasColumnName("id");

                entity.Property(e => e.Chantitle)
                    .HasMaxLength(64)
                    .HasColumnName("chantitle");

                entity.Property(e => e.Datadate)
                    .HasColumnType("datetime")
                    .HasColumnName("datadate");

                entity.Property(e => e.Filename)
                    .HasMaxLength(64)
                    .HasColumnName("filename");
                //entity.Property(e => e.fileindex)
                //  .HasColumnType("int(20) unsigned")
                //  .HasColumnName("fileindex");

                entity.Property(e => e.Max)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("max");

                entity.Property(e => e.Min)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("min");

                entity.Property(e => e.Range)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("range");

                entity.Property(e => e.Rms)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("rms");

                entity.Property(e => e.VehicleId)
                    .HasMaxLength(64)
                    .HasColumnName("VehicleID");
            });

            modelBuilder.Entity<SatictisAnalysisdataWft>(entity =>
            {
                entity.ToTable("satictis_analysisdata_wft");

                entity.Property(e => e.Id)
                    .HasMaxLength(64)
                    .HasColumnName("id");

                entity.Property(e => e.Chantitle)
                    .HasMaxLength(64)
                    .HasColumnName("chantitle");

                entity.Property(e => e.DamageK5)
                    .HasColumnType("double(30,10)")
                    .HasColumnName("damagek5");
                entity.Property(e => e.DamageK3)
                  .HasColumnType("double(30,10)")
                  .HasColumnName("damagek3");

                entity.Property(e => e.Datadate)
                    .HasColumnType("datetime")
                    .HasColumnName("datadate");

                entity.Property(e => e.Filename)
                    .HasMaxLength(64)
                    .HasColumnName("filename");

                entity.Property(e => e.Max)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("max");

                entity.Property(e => e.Min)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("min");

                entity.Property(e => e.Range)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("range");

                entity.Property(e => e.Rms)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("rms");

                entity.Property(e => e.VehicleId)
                    .HasMaxLength(64)
                    .HasColumnName("VehicleID");
            });

            modelBuilder.Entity<Pumapermileage>(entity =>
            {
                entity.ToTable("pumapermileage_analysisdata");

                entity.Property(e => e.id).IsRequired()
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.vehicle)
                    .HasMaxLength(64)
                    .HasColumnName("vehicle");

                entity.Property(e => e.filedate)
                    .HasColumnType("datetime")
                    .HasColumnName("filedate");

                entity.Property(e => e.duration)
                   .HasColumnType("double(20,2)")
                   .HasColumnName("duration");
                entity.Property(e => e.mileage)
                  .HasColumnType("double(20,2)")
                  .HasColumnName("mileage");
                entity.Property(e => e.averagespeed)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("averagespeed");

                entity.Property(e => e.maxthrottle)
                  .HasColumnType("double(20,5)")
                  .HasColumnName("maxthrottle");

                entity.Property(e => e.filename)
                    .HasMaxLength(64)
                    .HasColumnName("filename");

                entity.Property(e => e.maxbrake)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("maxbrake");

                entity.Property(e => e.maxangle)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("maxangle");

                entity.Property(e => e.minangle)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("minangle");

                entity.Property(e => e.wftfxlfk3)
                    .HasColumnType("double(30,10)")
                    .HasColumnName("wftfxlfk3");
                entity.Property(e => e.wftfxrfk3)
                  .HasColumnType("double(30,10)")
                  .HasColumnName("wftfxrfk3");
                entity.Property(e => e.wftfxlrk3)
                  .HasColumnType("double(30,10)")
                  .HasColumnName("wftfxlrk3");
                entity.Property(e => e.wftfxrrk3)
                  .HasColumnType("double(30,10)")
                  .HasColumnName("wftfxrrk3");
                entity.Property(e => e.wftfylfk3)
                    .HasColumnType("double(30,10)")
                    .HasColumnName("wftfylfk3");
                entity.Property(e => e.wftfyrfk3)
                  .HasColumnType("double(30,10)")
                  .HasColumnName("wftfyrfk3");
                entity.Property(e => e.wftfylrk3)
                  .HasColumnType("double(30,10)")
                  .HasColumnName("wftfylrk3");
                entity.Property(e => e.wftfyrrk3)
                  .HasColumnType("double(30,10)")
                  .HasColumnName("wftfyrrk3");
                entity.Property(e => e.wftfzlfk3)
                    .HasColumnType("double(30,10)")
                    .HasColumnName("wftfzlfk3");
                entity.Property(e => e.wftfzrfk3)
                  .HasColumnType("double(30,10)")
                  .HasColumnName("wftfzrfk3");
                entity.Property(e => e.wftfzlrk3)
                  .HasColumnType("double(30,10)")
                  .HasColumnName("wftfzlrk3");
                entity.Property(e => e.wftfzrrk3)
                  .HasColumnType("double(30,10)")
                  .HasColumnName("wftfzrrk3");

                entity.Property(e => e.wftfxlfk5)
                  .HasColumnType("double(30,10)")
                  .HasColumnName("wftfxlfk5");
                entity.Property(e => e.wftfxrfk5)
                  .HasColumnType("double(30,10)")
                  .HasColumnName("wftfxrfk5");
                entity.Property(e => e.wftfxlrk5)
                  .HasColumnType("double(30,10)")
                  .HasColumnName("wftfxlrk5");
                entity.Property(e => e.wftfxrrk5)
                  .HasColumnType("double(30,10)")
                  .HasColumnName("wftfxrrk5");
                entity.Property(e => e.wftfylfk5)
                    .HasColumnType("double(30,10)")
                    .HasColumnName("wftfylfk5");
                entity.Property(e => e.wftfyrfk5)
                  .HasColumnType("double(30,10)")
                  .HasColumnName("wftfyrfk5");
                entity.Property(e => e.wftfylrk5)
                  .HasColumnType("double(30,10)")
                  .HasColumnName("wftfylrk5");
                entity.Property(e => e.wftfyrrk5)
                  .HasColumnType("double(30,10)")
                  .HasColumnName("wftfyrrk5");
                entity.Property(e => e.wftfzlfk5)
                    .HasColumnType("double(30,10)")
                    .HasColumnName("wftfzlfk5");
                entity.Property(e => e.wftfzrfk5)
                  .HasColumnType("double(30,10)")
                  .HasColumnName("wftfzrfk5");
                entity.Property(e => e.wftfzlrk5)
                  .HasColumnType("double(30,10)")
                  .HasColumnName("wftfzlrk5");
                entity.Property(e => e.wftfzrrk5)
                  .HasColumnType("double(30,10)")
                  .HasColumnName("wftfzrrk5");
            });
            modelBuilder.Entity<Speeddistribution>(entity =>
            {
                entity.ToTable("speeddistribution");

                entity.Property(e => e.Id)
                    .HasMaxLength(64)
                    .HasColumnName("id");

                entity.Property(e => e.Above120).HasColumnType("double(20,5)");

                entity.Property(e => e.Datadate)
                    .HasColumnType("datetime")
                    .HasColumnName("datadate");

                entity.Property(e => e.VehicleId)
                    .HasMaxLength(64)
                    .HasColumnName("VehicleID");

                entity.Property(e => e._010)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("0-10");

                entity.Property(e => e._100110)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("100-110");

                entity.Property(e => e._1020)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("10-20");

                entity.Property(e => e._110120)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("110-120");

                entity.Property(e => e._2030)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("20-30");

                entity.Property(e => e._3040)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("30-40");

                entity.Property(e => e._4050)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("40-50");

                entity.Property(e => e._5060)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("50-60");

                entity.Property(e => e._6070)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("60-70");

                entity.Property(e => e._7080)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("70-80");

                entity.Property(e => e._8090)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("80-90");

                entity.Property(e => e._90100)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("90-100");
            });

            modelBuilder.Entity<EngineRpmDistribution_Time>(entity =>
            {
                entity.ToTable("enginerpmdistribution_time");

                entity.Property(e => e.Id)
                    .HasMaxLength(64)
                    .HasColumnName("id");

                entity.Property(e => e._Above6000).HasColumnType("int(20) unsigned").HasColumnName("Above6000"); ;

                entity.Property(e => e.Datadate)
                    .HasColumnType("datetime")
                    .HasColumnName("datadate");

                entity.Property(e => e.VehicleId)
                    .HasMaxLength(64)
                    .HasColumnName("VehicleID");

                entity.Property(e => e._01000)
                    .HasColumnType("int(20) unsigned")
                    .HasColumnName("0-1000");

                entity.Property(e => e._10001500)
                    .HasColumnType("int(20) unsigned")
                    .HasColumnName("1000-1500");

                entity.Property(e => e._15002000)
                    .HasColumnType("int(20) unsigned")
                    .HasColumnName("1500-2000");

                entity.Property(e => e._20002500)
                    .HasColumnType("int(20) unsigned")
                    .HasColumnName("2000-2500");

                entity.Property(e => e._25003000)
                    .HasColumnType("int(20) unsigned")
                    .HasColumnName("2500-3000");

                entity.Property(e => e._30004000)
                    .HasColumnType("int(20) unsigned")
                    .HasColumnName("3000-4000");

                entity.Property(e => e._40005000)
                    .HasColumnType("int(20) unsigned")
                    .HasColumnName("4000-5000");

                entity.Property(e => e._50006000)
                    .HasColumnType("int(20) unsigned")
                    .HasColumnName("5000-6000");
            });

            modelBuilder.Entity<EngineRpmDistribution_Distance>(entity =>
            {
                entity.ToTable("enginerpmdistribution_distance");

                entity.Property(e => e.Id)
                    .HasMaxLength(64)
                    .HasColumnName("id");

                entity.Property(e => e._Above6000).HasColumnType("double(20,5)");

                entity.Property(e => e.Datadate)
                    .HasColumnType("datetime")
                    .HasColumnName("datadate");

                entity.Property(e => e.VehicleId)
                    .HasMaxLength(64)
                    .HasColumnName("VehicleID");

                entity.Property(e => e._01000)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("0-1000");

                entity.Property(e => e._10001500)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("1000-1500");

                entity.Property(e => e._15002000)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("1500-2000");

                entity.Property(e => e._20002500)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("2000-2500");

                entity.Property(e => e._25003000)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("2500-3000");

                entity.Property(e => e._30004000)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("3000-4000");

                entity.Property(e => e._40005000)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("4000-5000");

                entity.Property(e => e._50006000)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("5000-6000");
                entity.Property(e => e._Above6000)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("Above6000");
            });

            modelBuilder.Entity<Streeringrecognition>(entity =>
            {
                entity.ToTable("streeringrecognition");

                entity.Property(e => e.Id)
                    .HasMaxLength(64)
                    .HasColumnName("id");

                entity.Property(e => e.AngularAcc).HasColumnType("double(64,2)");

                entity.Property(e => e.Datadate)
                    .HasColumnType("datetime")
                    .HasColumnName("datadate");

                entity.Property(e => e.Filename)
                    .HasMaxLength(64)
                    .HasColumnName("filename");

                entity.Property(e => e.Speed).HasColumnType("double(64,2)");

                entity.Property(e => e.SteeringAcc).HasColumnType("double(64,2)");

                entity.Property(e => e.SteeringDirection).HasColumnType("tinyint(2)");

                entity.Property(e => e.StrgWhlAng).HasColumnType("double(64,2)");

                entity.Property(e => e.VehicleId)
                    .HasMaxLength(64)
                    .HasColumnName("VehicleID");
            });

            modelBuilder.Entity<SysAuthority>(entity =>
            {
                entity.ToTable("sys_authority");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("ID");

                entity.Property(e => e.AuthorityKey).HasColumnType("int(10)");

                entity.Property(e => e.LoginName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Throttlerecognition>(entity =>
            {
                entity.ToTable("throttlerecognition");

                entity.Property(e => e.Id)
                    .HasMaxLength(64)
                    .HasColumnName("id");

                entity.Property(e => e.Accelerograph).HasColumnType("double(64,1)");

                entity.Property(e => e.Datadate)
                    .HasColumnType("datetime")
                    .HasColumnName("datadate");

                entity.Property(e => e.Filename)
                    .HasMaxLength(64)
                    .HasColumnName("filename");

                entity.Property(e => e.LastingTime).HasColumnType("double(64,2)");

                entity.Property(e => e.Reverse).HasColumnType("tinyint(2)");

                entity.Property(e => e.Speed).HasColumnType("double(64,1)");

                entity.Property(e => e.ThrottleAcc).HasColumnType("double(64,2)");

                entity.Property(e => e.VehicleId)
                    .HasMaxLength(64)
                    .HasColumnName("VehicleID");
            });

            modelBuilder.Entity<t_vehiclemaster>(entity =>
            {
                entity.ToTable("t_vehiclemaster");

                entity.Property(e => e.id)
                    .IsRequired()
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.vehicleid)
                  .IsRequired()
                  .HasMaxLength(64)
                  .HasColumnName("vehicleid");

                entity.Property(e => e.samplerate)
                   .IsRequired()
                   .HasColumnType("int(8) unsigned")
                   .HasColumnName("samplerate");

                entity.Property(e => e.numberpoints)
                 .IsRequired()
                 .HasColumnType("int(8) unsigned")
                 .HasColumnName("numberpoints");

                entity.Property(e => e.analysisaccess)
                    .HasColumnType("tinyint(1) unsigned")
                    .HasColumnName("analysisaccess")
                    ;

                entity.Property(e => e.importaccess)
                   .HasColumnType("tinyint(1) unsigned")
                   .HasColumnName("importaccess")
                   ;

                entity.Property(e => e.predictaccess)
                    .HasColumnType("tinyint(1) unsigned")
                    .HasColumnName("predictaccess")
                    ;

                entity.Property(e => e.displaygpspoints)
                   .IsRequired()
                   .HasColumnType("int(8) unsigned")
                   .HasColumnName("displaygpspoints");

                entity.Property(e => e.area).HasMaxLength(64);

                entity.Property(e => e.country).HasMaxLength(64);

                entity.Property(e => e.state).IsRequired().HasColumnType("tinyint(1) unsigned");

                entity.Property(e => e.remarks).HasMaxLength(255);

              

            });

            modelBuilder.Entity<t_vehiclemonitorpara>(entity =>
            {
                entity.ToTable("t_vehiclemonitorpara");

                entity.Property(e => e.id)
                    .IsRequired()
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.vehicleid)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("vehicleid");

                entity.Property(e => e.monitorcsvcollumnname)
                   .IsRequired()
                   .HasMaxLength(1024)
                   .HasColumnName("monitorcsvcollumnname");


                entity.Property(e => e.monitorinputpath)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("monitorinputpath");


                entity.Property(e => e.monitorreductiontimes)
                    .IsRequired()
                    .HasColumnType("tinyint(3) unsigned")
                    .HasColumnName("monitorreductiontimes")
                    ;

                entity.Property(e => e.echart1channelname)
                   .HasMaxLength(255)
                   .HasColumnName("echart1channelname");

                entity.Property(e => e.echart2channelname)
                   .HasMaxLength(255)
                   .HasColumnName("echart2channelname");

                entity.Property(e => e.echart3channelname)
                  .HasMaxLength(255)
                  .HasColumnName("echart3channelname");

                entity.Property(e => e.echart4channelname)
                  .HasMaxLength(255)
                  .HasColumnName("echart4channelname");

                entity.Property(e => e.echart5channelname)
                  .HasMaxLength(255)
                  .HasColumnName("echart5channelname");

                entity.Property(e => e.echart6channelname)
                  .HasMaxLength(255)
                  .HasColumnName("echart6channelname");

                entity.Property(e => e.echart1title)
                 .HasMaxLength(255)
                 .HasColumnName("echart1title");

                entity.Property(e => e.echart2title)
               .HasMaxLength(255)
               .HasColumnName("echart2title");

                entity.Property(e => e.echart3title)
               .HasMaxLength(255)
               .HasColumnName("echart3title");

                entity.Property(e => e.echart4title)
               .HasMaxLength(255)
               .HasColumnName("echart4title");

                entity.Property(e => e.echart5title)
               .HasMaxLength(255)
               .HasColumnName("echart5title");

                entity.Property(e => e.echart6title)
               .HasMaxLength(255)
               .HasColumnName("echart6title");

            });

            modelBuilder.Entity<t_vehicleimportpara>(entity =>
            {
                entity.ToTable("t_vehicleimportpara");

                entity.Property(e => e.id).IsRequired()
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.vehicleid)
                   .IsRequired()
                   .HasMaxLength(64)
                   .HasColumnName("vehicleid");

                entity.Property(e => e.importinputpath)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("importinputpath");

                entity.Property(e => e.importresultpath)
             .IsRequired()
             .HasMaxLength(255)
             .HasColumnName("importresultpath");

                entity.Property(e => e.importbrake)
                    .HasColumnType("tinyint(1) unsigned")
                    .HasColumnName("importbrake")
                   ;

                entity.Property(e => e.importimpact)
                    .HasColumnType("tinyint(1) unsigned")
                    .HasColumnName("importimpact")
                   ;


                entity.Property(e => e.importgps)
                    .HasColumnType("tinyint(1) unsigned")
                    .HasColumnName("importgps")
                    ;

                entity.Property(e => e.importaccreductiontimes).IsRequired()
                    .HasColumnType("tinyint(3) unsigned")
                    .HasColumnName("importaccreductiontimes");


                entity.Property(e => e.importwftreductiontimes).IsRequired()
                    .HasColumnType("tinyint(3) unsigned")
                    .HasColumnName("importwftreductiontimes");

                entity.Property(e => e.importgpsreductiontimes).IsRequired()
                .HasColumnType("tinyint(3) unsigned")
                .HasColumnName("importgpsreductiontimes");

                entity.Property(e => e.importspeed)
                    .HasColumnType("tinyint(1) unsigned")
                     .HasColumnName("importspeed")
                    ;
                entity.Property(e => e.importpuma)
                    .HasColumnType("tinyint(1) unsigned")
                     .HasColumnName("importpuma")
                    ;
                entity.Property(e => e.importstatistic)
                    .HasColumnType("tinyint(1) unsigned")
                     .HasColumnName("importstatistic")
                    ;

                entity.Property(e => e.importsteering)
                    .HasColumnType("tinyint(1) unsigned")
                    .HasColumnName("importsteering")
                    ;
                entity.Property(e => e.importthrottle)
                    .HasColumnType("tinyint(1) unsigned")
                    .HasColumnName("importthrottle")
                    ;
                entity.Property(e => e.importwft)
                    .HasColumnType("tinyint(1) unsigned")
                    .HasColumnName("importwft")
                    ;
                entity.Property(e => e.importengspd)
                   .HasColumnType("tinyint(1) unsigned")
                   .HasColumnName("importengspd")
                   ;
                entity.Property(e => e.speedcolumnname)
                  .HasMaxLength(64)
                  .HasColumnName("speedcolumnname");
                entity.Property(e => e.throttlecolumnname)
                   .HasMaxLength(64)
                   .HasColumnName("throttlecolumnname");
                entity.Property(e => e.brakecolumnname)
                   .HasMaxLength(64)
                   .HasColumnName("brakecolumnname");
                entity.Property(e => e.whlangcolumnname)
   .HasMaxLength(64)
   .HasColumnName("whlangcolumnname");
                entity.Property(e => e.whlanggrcolumnname)
   .HasMaxLength(64)
   .HasColumnName("whlanggrcolumnname");
                entity.Property(e => e.acczwhllf)
   .HasMaxLength(64)
   .HasColumnName("acczwhllf");
                entity.Property(e => e.acczwhlrf)
   .HasMaxLength(64)
   .HasColumnName("acczwhlrf");
                entity.Property(e => e.acczwhllr)
   .HasMaxLength(64)
   .HasColumnName("acczwhllr");
                entity.Property(e => e.accybody)
   .HasMaxLength(64)
   .HasColumnName("accybody");
                entity.Property(e => e.accxbody)
.HasMaxLength(64)
.HasColumnName("accxbody");
                entity.Property(e => e.enginespeed)
.HasMaxLength(64)
.HasColumnName("enginespeed");
            });

            modelBuilder.Entity<t_vehiclecomputepara>(entity =>
            {
                entity.ToTable("t_vehiclecomputepara");

                entity.Property(e => e.id).IsRequired()
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.acctimegap).IsRequired()
                    .HasColumnType("tinyint(3) unsigned")
                    .HasColumnName("acctimegap");

                entity.Property(e => e.accvaluegap).IsRequired()
                    .HasColumnType("tinyint(3) unsigned")
                    .HasColumnName("accvaluegap");

                entity.Property(e => e.bumpzerostandard).IsRequired()
                    .HasColumnType("tinyint(3) unsigned")
                    .HasColumnName("bumpzerostandard");

                entity.Property(e => e.brakelastingpoints).IsRequired()
                    .HasColumnType("tinyint(3) unsigned")
                    .HasColumnName("brakelastingpoints");

                entity.Property(e => e.brakezerostandard).IsRequired()
                    .HasColumnType("tinyint(3) unsigned")
                    .HasColumnName("brakezerostandard");

                entity.Property(e => e.bumpmaxspeed).IsRequired()
                    .HasColumnType("tinyint(3) unsigned")
                    .HasColumnName("bumpmaxspeed");

                entity.Property(e => e.bumptimegap).IsRequired()
                    .HasColumnType("tinyint(3) unsigned")
                    .HasColumnName("bumptimegap");

                entity.Property(e => e.steeringlastingpoints).IsRequired()
                    .HasColumnType("tinyint(3) unsigned")
                    .HasColumnName("steeringlastingpoints");

                entity.Property(e => e.steeringzerostandard).IsRequired()
                    .HasColumnType("tinyint(3) unsigned")
                    .HasColumnName("steeringzerostandard");

                entity.Property(e => e.throttlelastingpoints).IsRequired()
                    .HasColumnType("tinyint(3) unsigned")
                    .HasColumnName("throttlelastingpoints");

                entity.Property(e => e.throttlezerostandard).IsRequired()
                    .HasColumnType("tinyint(3) unsigned")
                    .HasColumnName("throttlezerostandard");

                entity.Property(e => e.vehicleid)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("vehicleid");

                entity.Property(e => e.wheelbaselower).IsRequired()
                    .HasColumnType("float(2,1) unsigned")
                    .HasColumnName("wheelbaselower");

                entity.Property(e => e.wheelbaseupper).IsRequired()
                    .HasColumnType("float(2,1) unsigned")
                    .HasColumnName("wheelbaseupper");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

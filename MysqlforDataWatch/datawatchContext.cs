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
        public virtual DbSet<Speeddistribution> Speeddistributions { get; set; }
        public virtual DbSet<Streeringrecognition> Streeringrecognitions { get; set; }
        public virtual DbSet<SysAuthority> SysAuthorities { get; set; }
        public virtual DbSet<Throttlerecognition> Throttlerecognitions { get; set; }
        

        public virtual DbSet<t_vehiclemaster> t_vehiclemaster { get; set; }
        public virtual DbSet<t_vehiclemonitorpara> t_vehiclemonitorpara { get; set; }
        public virtual DbSet<t_vehicleimportpara> t_vehicleimportpara { get; set; }
        public virtual DbSet<t_vehiclecomputepara> t_vehiclecomputepara { get; set; }
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

                entity.Property(e => e.Damage)
                    .HasColumnType("double(30,10)")
                    .HasColumnName("damage");

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

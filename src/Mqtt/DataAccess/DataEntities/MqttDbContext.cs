using DataAccess.DataEntities;

using Microsoft.EntityFrameworkCore;

namespace DataEntities;

public partial class MqttDbContext : DbContext
{
    public MqttDbContext()
    {
    }

    public MqttDbContext(DbContextOptions<MqttDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CameraBroker> CameraBrokers { get; set; }

    public virtual DbSet<CameraDailyBroker> CameraDailyBrokers { get; set; }

    public virtual DbSet<CanBroker> CanBrokers { get; set; }

    public virtual DbSet<CanDailyBroker> CanDailyBrokers { get; set; }

    public virtual DbSet<CanDailyTranslate> CanDailyTranslates { get; set; }

    public virtual DbSet<CanTranslate> CanTranslates { get; set; }

    public virtual DbSet<DailyVisitDetail> DailyVisitDetails { get; set; }
    public virtual DbSet<DailyVisit> DailyVisits { get; set; }
    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<FuelRate> FuelRates { get; set; }
    public virtual DbSet<Fuel> Fuels { get; set; }
    public virtual DbSet<FuelType> FuelTypes { get; set; }

    public virtual DbSet<GeneralBroker> GeneralBrokers { get; set; }

    public virtual DbSet<GeneralDailyBroker> GeneralDailyBrokers { get; set; }

    public virtual DbSet<GpsBroker> GpsBrokers { get; set; }

    public virtual DbSet<GpsDailyBroker> GpsDailyBrokers { get; set; }

    public virtual DbSet<J1939> J1939s { get; set; }

    public virtual DbSet<MonitoringDevice> MonitoringDevices { get; set; }

    public virtual DbSet<ObdBroker> ObdBrokers { get; set; }

    public virtual DbSet<ObdDailyBroker> ObdDailyBrokers { get; set; }

    public virtual DbSet<SignalBroker> SignalBrokers { get; set; }

    public virtual DbSet<SignalDailyBroker> SignalDailyBrokers { get; set; }

    public virtual DbSet<TemperatureBroker> TemperatureBrokers { get; set; }

    public virtual DbSet<TemperatureDailyBroker> TemperatureDailyBrokers { get; set; }

    public virtual DbSet<TpmsBroker> TpmsBrokers { get; set; }

    public virtual DbSet<TpmsDailyBroker> TpmsDailyBrokers { get; set; }

    public virtual DbSet<Translation> Translations { get; set; }

    public virtual DbSet<VehicleType> VehicleTypes { get; set; }

    public virtual DbSet<VoltageBroker> VoltageBrokers { get; set; }

    public virtual DbSet<VoltageDailyBroker> VoltageDailyBrokers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=62.106.95.74;Initial Catalog=MoshaveranCoreV1;MultipleActiveResultSets=True;User ID=sa;Password=@Moshaveran@;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<CameraBroker>(entity =>
        {
            _ = entity.ToTable("Camera_Brokers");

            _ = entity.HasIndex(e => e.CreatedBy, "IX_Camera_Brokers_CreatedBy");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_Camera_Brokers_LastModifiedBy");

            _ = entity.Property(e => e.Id).ValueGeneratedNever();
            _ = entity.Property(e => e.Imei).HasColumnName("IMEI");
        });

        _ = modelBuilder.Entity<CameraDailyBroker>(entity =>
        {
            _ = entity.ToTable("Camera_Daily_Brokers");

            _ = entity.HasIndex(e => e.CreatedBy, "IX_Camera_Daily_Brokers_CreatedBy");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_Camera_Daily_Brokers_LastModifiedBy");

            _ = entity.Property(e => e.Id).ValueGeneratedNever();
            _ = entity.Property(e => e.Imei).HasColumnName("IMEI");
        });

        _ = modelBuilder.Entity<CanBroker>(entity =>
        {
            _ = entity.ToTable("CAN_Brokers");

            _ = entity.HasIndex(e => e.CreatedBy, "IX_CAN_Brokers_CreatedBy");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_CAN_Brokers_LastModifiedBy");

            _ = entity.Property(e => e.Id).ValueGeneratedNever();
            _ = entity.Property(e => e.Imei).HasColumnName("IMEI");
            _ = entity.Property(e => e.IsDelete).HasDefaultValue(false);
            _ = entity.Property(e => e.Pgn).HasColumnName("PGN");
        });

        _ = modelBuilder.Entity<CanDailyBroker>(entity =>
        {
            _ = entity.ToTable("CAN_Daily_Brokers");

            _ = entity.HasIndex(e => e.CreatedBy, "IX_CAN_Daily_Brokers_CreatedBy");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_CAN_Daily_Brokers_LastModifiedBy");

            _ = entity.Property(e => e.Id).ValueGeneratedNever();
            _ = entity.Property(e => e.Imei).HasColumnName("IMEI");
            _ = entity.Property(e => e.Pgn).HasColumnName("PGN");
        });

        _ = modelBuilder.Entity<CanDailyTranslate>(entity =>
        {
            _ = entity.ToTable("CAN_Daily_Translates");

            _ = entity.HasIndex(e => e.CanBrokerId, "IX_CAN_Daily_Translates_CAN_BrokerId");

            _ = entity.HasIndex(e => e.CreatedBy, "IX_CAN_Daily_Translates_CreatedBy");

            _ = entity.HasIndex(e => e.J1939id1, "IX_CAN_Daily_Translates_J1939Id1");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_CAN_Daily_Translates_LastModifiedBy");

            _ = entity.Property(e => e.Id).ValueGeneratedNever();
            _ = entity.Property(e => e.CanBrokerId).HasColumnName("CAN_BrokerId");
            _ = entity.Property(e => e.Imei).HasColumnName("IMEI");
            _ = entity.Property(e => e.J1939id).HasColumnName("J1939Id");
            _ = entity.Property(e => e.J1939id1).HasColumnName("J1939Id1");

            _ = entity.HasOne(d => d.CanBroker).WithMany(p => p.CanDailyTranslates).HasForeignKey(d => d.CanBrokerId);
        });

        _ = modelBuilder.Entity<CanTranslate>(entity =>
        {
            _ = entity.HasKey(e => e.Id).IsClustered(false);

            _ = entity.ToTable("CAN_Translates");

            _ = entity.HasIndex(e => e.CreatedOn, "IX_CAN_Translates_CreatedOn");

            _ = entity.HasIndex(e => e.CanBrokerId, "IX_CAN_Translates_CAN_BrokerId");

            _ = entity.HasIndex(e => e.CreatedBy, "IX_CAN_Translates_CreatedBy");

            _ = entity.HasIndex(e => e.J1939id1, "IX_CAN_Translates_J1939Id1");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_CAN_Translates_LastModifiedBy");

            _ = entity.Property(e => e.Id).ValueGeneratedNever();
            _ = entity.Property(e => e.CanBrokerId).HasColumnName("CAN_BrokerId");
            _ = entity.Property(e => e.Imei).HasColumnName("IMEI");
            _ = entity.Property(e => e.IsDelete).HasDefaultValue(false);
            _ = entity.Property(e => e.J1939id).HasColumnName("J1939Id");
            _ = entity.Property(e => e.J1939id1).HasColumnName("J1939Id1");
        });

        _ = modelBuilder.Entity<DailyVisit>(entity =>
        {
            _ = entity.HasIndex(e => e.CreatedBy, "IX_DailyVisits_CreatedBy");

            _ = entity.HasIndex(e => e.CustomerAssignmentId, "IX_DailyVisits_CustomerAssignmentId");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_DailyVisits_LastModifiedBy");
        });

        _ = modelBuilder.Entity<DailyVisitDetail>(entity =>
        {
            _ = entity.HasIndex(e => e.CreatedBy, "IX_DailyVisitDetails_CreatedBy");

            _ = entity.HasIndex(e => e.DailyVisitId, "IX_DailyVisitDetails_DailyVisitId");

            _ = entity.HasIndex(e => e.GoodId, "IX_DailyVisitDetails_GoodId");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_DailyVisitDetails_LastModifiedBy");

            _ = entity.HasOne(d => d.DailyVisit).WithMany(p => p.DailyVisitDetails).HasForeignKey(d => d.DailyVisitId);
        });

        _ = modelBuilder.Entity<Device>(entity =>
        {
            _ = entity.HasIndex(e => e.CreatedBy, "IX_Devices_CreatedBy");

            _ = entity.HasIndex(e => e.Imei, "IX_Devices_Imei")
                .IsUnique()
                .HasFilter("([Imei] IS NOT NULL)");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_Devices_LastModifiedBy");

            _ = entity.HasIndex(e => e.ProductId, "IX_Devices_ProductId");
        });

        _ = modelBuilder.Entity<Fuel>(entity =>
        {
            _ = entity.HasIndex(e => e.CreatedBy, "IX_Fuels_CreatedBy");

            _ = entity.HasIndex(e => e.CustomerAssignmentId, "IX_Fuels_CustomerAssignmentId");

            _ = entity.HasIndex(e => e.FuelTypeId, "IX_Fuels_FuelTypeId");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_Fuels_LastModifiedBy");

            _ = entity.Property(e => e.CurrentKm).HasColumnName("CurrentKM");
            _ = entity.Property(e => e.FreeLiterAmount).HasColumnType("decimal(18, 2)");
            _ = entity.Property(e => e.FreeLiterRate).HasColumnType("decimal(18, 2)");
            _ = entity.Property(e => e.LiterAmount).HasColumnType("decimal(18, 2)");
            _ = entity.Property(e => e.LiterRate).HasColumnType("decimal(18, 2)");

            _ = entity.HasOne(d => d.FuelType).WithMany(p => p.Fuels).HasForeignKey(d => d.FuelTypeId);
        });

        _ = modelBuilder.Entity<FuelRate>(entity =>
        {
            _ = entity.HasIndex(e => e.CreatedBy, "IX_FuelRates_CreatedBy");

            _ = entity.HasIndex(e => e.FuelTypeId, "IX_FuelRates_FuelTypeId");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_FuelRates_LastModifiedBy");

            _ = entity.HasIndex(e => e.VehicleTypeId, "IX_FuelRates_VehicleTypeId");

            _ = entity.Property(e => e.FreeLiterRate).HasColumnType("decimal(18, 2)");
            _ = entity.Property(e => e.LiterRate).HasColumnType("decimal(18, 2)");

            _ = entity.HasOne(d => d.FuelType).WithMany(p => p.FuelRates).HasForeignKey(d => d.FuelTypeId);

            _ = entity.HasOne(d => d.VehicleType).WithMany(p => p.FuelRates).HasForeignKey(d => d.VehicleTypeId);
        });

        _ = modelBuilder.Entity<GeneralBroker>(entity =>
        {
            _ = entity.ToTable("General_Brokers");

            _ = entity.HasIndex(e => e.CreatedBy, "IX_General_Brokers_CreatedBy");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_General_Brokers_LastModifiedBy");

            _ = entity.Property(e => e.Id).ValueGeneratedNever();
            _ = entity.Property(e => e.DhtBoardHumidity).HasColumnName("DHT_Board_Humidity");
            _ = entity.Property(e => e.DhtBoardStatus).HasColumnName("DHT_Board_Status");
            _ = entity.Property(e => e.DhtBoardTemperature).HasColumnName("DHT_Board_Temperature");
            _ = entity.Property(e => e.Imei).HasColumnName("IMEI");
            _ = entity.Property(e => e.InternetRemainingUssd).HasColumnName("InternetRemainingUSSD");
            _ = entity.Property(e => e.SignalQuality).HasColumnName("Signal_Quality");
        });

        _ = modelBuilder.Entity<GeneralDailyBroker>(entity =>
        {
            _ = entity.ToTable("General_Daily_Brokers");

            _ = entity.HasIndex(e => e.CreatedBy, "IX_General_Daily_Brokers_CreatedBy");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_General_Daily_Brokers_LastModifiedBy");

            _ = entity.Property(e => e.Id).ValueGeneratedNever();
            _ = entity.Property(e => e.DhtBoardHumidity).HasColumnName("DHT_Board_Humidity");
            _ = entity.Property(e => e.DhtBoardStatus).HasColumnName("DHT_Board_Status");
            _ = entity.Property(e => e.DhtBoardTemperature).HasColumnName("DHT_Board_Temperature");
            _ = entity.Property(e => e.Imei).HasColumnName("IMEI");
            _ = entity.Property(e => e.InternetRemainingUssd).HasColumnName("InternetRemainingUSSD");
            _ = entity.Property(e => e.SignalQuality).HasColumnName("Signal_Quality");
        });

        _ = modelBuilder.Entity<GpsBroker>(entity =>
        {
            _ = entity.ToTable("GPS_Brokers");

            _ = entity.HasIndex(e => e.CreatedBy, "IX_GPS_Brokers_CreatedBy");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_GPS_Brokers_LastModifiedBy");

            _ = entity.Property(e => e.Id).ValueGeneratedNever();
            _ = entity.Property(e => e.GpsDateTime).HasColumnName("GPS_DateTime");
            _ = entity.Property(e => e.Imei).HasColumnName("IMEI");
        });

        _ = modelBuilder.Entity<GpsDailyBroker>(entity =>
        {
            _ = entity.ToTable("GPS_Daily_Brokers");

            _ = entity.HasIndex(e => e.CreatedBy, "IX_GPS_Daily_Brokers_CreatedBy");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_GPS_Daily_Brokers_LastModifiedBy");

            _ = entity.Property(e => e.Id).ValueGeneratedNever();
            _ = entity.Property(e => e.GpsDateTime).HasColumnName("GPS_DateTime");
            _ = entity.Property(e => e.Imei).HasColumnName("IMEI");
        });

        _ = modelBuilder.Entity<J1939>(entity =>
        {
            _ = entity.HasIndex(e => e.CreatedBy, "IX_J1939s_CreatedBy");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_J1939s_LastModifiedBy");

            _ = entity.Property(e => e.StatesKey).HasColumnName("States_key");
            _ = entity.Property(e => e.StatesValue).HasColumnName("States_value");
        });

        _ = modelBuilder.Entity<MonitoringDevice>(entity =>
        {
            _ = entity
                .HasNoKey()
                .ToView("MonitoringDevices");

            _ = entity.Property(e => e.CanBrokersLastUpdate).HasColumnName("CAN_Brokers_LastUpdate");
            _ = entity.Property(e => e.GeneralBrokersLastUpdate).HasColumnName("General_Brokers_LastUpdate");
            _ = entity.Property(e => e.GpsBrokersLastUpdate).HasColumnName("GPS_Brokers_LastUpdate");
            _ = entity.Property(e => e.Imei)
                .HasMaxLength(450)
                .HasColumnName("IMEI");
            _ = entity.Property(e => e.IsWeighting).HasColumnName("isWeighting");
            _ = entity.Property(e => e.ObdBrokersLastUpdate).HasColumnName("OBD_Brokers_LastUpdate");
            _ = entity.Property(e => e.Signal).HasColumnName("SIGNAL");
            _ = entity.Property(e => e.SignalBrokersLastUpdate).HasColumnName("Signal_Brokers_LastUpdate");
            _ = entity.Property(e => e.TemperatureBrokersLastUpdate).HasColumnName("Temperature_Brokers_LastUpdate");
            _ = entity.Property(e => e.Value).HasColumnType("decimal(18, 0)");
            _ = entity.Property(e => e.VoltageBrokersLastUpdate).HasColumnName("Voltage_Brokers_LastUpdate");
        });

        _ = modelBuilder.Entity<ObdBroker>(entity =>
        {
            _ = entity.ToTable("OBD_Brokers");

            _ = entity.HasIndex(e => e.CreatedBy, "IX_OBD_Brokers_CreatedBy");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_OBD_Brokers_LastModifiedBy");

            _ = entity.Property(e => e.Id).ValueGeneratedNever();
            _ = entity.Property(e => e.Imei).HasColumnName("IMEI");
        });

        _ = modelBuilder.Entity<ObdDailyBroker>(entity =>
        {
            _ = entity.ToTable("OBD_Daily_Brokers");

            _ = entity.HasIndex(e => e.CreatedBy, "IX_OBD_Daily_Brokers_CreatedBy");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_OBD_Daily_Brokers_LastModifiedBy");

            _ = entity.Property(e => e.Id).ValueGeneratedNever();
            _ = entity.Property(e => e.Imei).HasColumnName("IMEI");
        });

        _ = modelBuilder.Entity<SignalBroker>(entity =>
        {
            _ = entity.ToTable("Signal_Brokers");

            _ = entity.HasIndex(e => e.CreatedBy, "IX_Signal_Brokers_CreatedBy");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_Signal_Brokers_LastModifiedBy");

            _ = entity.Property(e => e.Id).ValueGeneratedNever();
            _ = entity.Property(e => e.Imei).HasColumnName("IMEI");
            _ = entity.Property(e => e.SignalQuality).HasColumnName("Signal_Quality");
        });

        _ = modelBuilder.Entity<SignalDailyBroker>(entity =>
        {
            _ = entity.ToTable("Signal_Daily_Brokers");

            _ = entity.HasIndex(e => e.CreatedBy, "IX_Signal_Daily_Brokers_CreatedBy");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_Signal_Daily_Brokers_LastModifiedBy");

            _ = entity.Property(e => e.Id).ValueGeneratedNever();
            _ = entity.Property(e => e.Imei).HasColumnName("IMEI");
            _ = entity.Property(e => e.SignalQuality).HasColumnName("Signal_Quality");
        });

        _ = modelBuilder.Entity<TemperatureBroker>(entity =>
        {
            _ = entity.ToTable("Temperature_Brokers");

            _ = entity.HasIndex(e => e.CreatedBy, "IX_Temperature_Brokers_CreatedBy");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_Temperature_Brokers_LastModifiedBy");

            _ = entity.Property(e => e.Id).ValueGeneratedNever();
            _ = entity.Property(e => e.Imei).HasColumnName("IMEI");
        });

        _ = modelBuilder.Entity<TemperatureDailyBroker>(entity =>
        {
            _ = entity.ToTable("Temperature_Daily_Brokers");

            _ = entity.HasIndex(e => e.CreatedBy, "IX_Temperature_Daily_Brokers_CreatedBy");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_Temperature_Daily_Brokers_LastModifiedBy");

            _ = entity.Property(e => e.Id).ValueGeneratedNever();
            _ = entity.Property(e => e.Imei).HasColumnName("IMEI");
        });

        _ = modelBuilder.Entity<TpmsBroker>(entity =>
        {
            _ = entity.ToTable("TPMS_Brokers");

            _ = entity.HasIndex(e => e.CreatedBy, "IX_TPMS_Brokers_CreatedBy");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_TPMS_Brokers_LastModifiedBy");

            _ = entity.Property(e => e.Id).ValueGeneratedNever();
            _ = entity.Property(e => e.Imei).HasColumnName("IMEI");
        });

        _ = modelBuilder.Entity<TpmsDailyBroker>(entity =>
        {
            _ = entity.ToTable("TPMS_Daily_Brokers");

            _ = entity.HasIndex(e => e.CreatedBy, "IX_TPMS_Daily_Brokers_CreatedBy");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_TPMS_Daily_Brokers_LastModifiedBy");

            _ = entity.Property(e => e.Id).ValueGeneratedNever();
            _ = entity.Property(e => e.Imei).HasColumnName("IMEI");
        });

        _ = modelBuilder.Entity<Translation>(entity =>
        {
            _ = entity.HasIndex(e => e.CreatedBy, "IX_Translations_CreatedBy");

            _ = entity.HasIndex(e => e.LanguageId, "IX_Translations_LanguageId");

            _ = entity.HasIndex(e => e.LanguageKeyId, "IX_Translations_LanguageKeyId");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_Translations_LastModifiedBy");

        });

        _ = modelBuilder.Entity<VehicleType>(entity =>
        {
            _ = entity.HasIndex(e => e.CreatedBy, "IX_VehicleTypes_CreatedBy");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_VehicleTypes_LastModifiedBy");

            _ = entity.HasIndex(e => e.TransportTypeId, "IX_VehicleTypes_TransportTypeId");

            _ = entity.HasIndex(e => e.UsageTypeId, "IX_VehicleTypes_UsageTypeId");

            _ = entity.Property(e => e.IsDriverBehaviors).HasColumnName("isDriverBehaviors");
            _ = entity.Property(e => e.IsPeriodicService).HasColumnName("isPeriodicService");
            _ = entity.Property(e => e.IsValidCan).HasColumnName("IsValidCAN");
            _ = entity.Property(e => e.IsValidObd).HasColumnName("IsValidOBD");
        });

        _ = modelBuilder.Entity<VoltageBroker>(entity =>
        {
            _ = entity.ToTable("Voltage_Brokers");

            _ = entity.HasIndex(e => e.CreatedBy, "IX_Voltage_Brokers_CreatedBy");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_Voltage_Brokers_LastModifiedBy");

            _ = entity.Property(e => e.Id).ValueGeneratedNever();
            _ = entity.Property(e => e.DhtBoardHumidity).HasColumnName("DHT_Board_Humidity");
            _ = entity.Property(e => e.DhtBoardStatus).HasColumnName("DHT_Board_Status");
            _ = entity.Property(e => e.DhtBoardTemperature).HasColumnName("DHT_Board_Temperature");
            _ = entity.Property(e => e.Imei).HasColumnName("IMEI");
        });

        _ = modelBuilder.Entity<VoltageDailyBroker>(entity =>
        {
            _ = entity.ToTable("Voltage_Daily_Brokers");

            _ = entity.HasIndex(e => e.CreatedBy, "IX_Voltage_Daily_Brokers_CreatedBy");

            _ = entity.HasIndex(e => e.LastModifiedBy, "IX_Voltage_Daily_Brokers_LastModifiedBy");

            _ = entity.Property(e => e.Id).ValueGeneratedNever();
            _ = entity.Property(e => e.DhtBoardHumidity).HasColumnName("DHT_Board_Humidity");
            _ = entity.Property(e => e.DhtBoardStatus).HasColumnName("DHT_Board_Status");
            _ = entity.Property(e => e.DhtBoardTemperature).HasColumnName("DHT_Board_Temperature");
            _ = entity.Property(e => e.Imei).HasColumnName("IMEI");
        });

        this.OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
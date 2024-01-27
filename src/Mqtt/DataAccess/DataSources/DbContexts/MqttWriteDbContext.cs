using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Moshaveran.Infrastructure.Helpers;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

namespace Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;

internal sealed class MqttWriteDbContext : MqttDbContext
{
    public MqttWriteDbContext()
    {
    }

    public MqttWriteDbContext(DbContextOptions<MqttDbContext> options) : base(options)
    {
    }

    public async Task<int> SaveCanBrokerAsync(EntityEntry<CanBroker> entry, CancellationToken token = default)
    {
        var result = entry.Entity;
        FormattableString statement = $@"
                DELETE dbo.CAN_Daily_Brokers WHERE IMEI='{result.Imei}'
                INSERT INTO dbo.CAN_Daily_Brokers(Id,IMEI,PGN,[Identifier],[Value],CreatedBy,CreatedOn,IsDelete, DeleteOn)
                VALUES(NEWID(),'{result.Imei}',{result.Pgn},'{result.Identifier}','{result.Value}',NULL,{result.CreatedOn.ToSqlFormat()},0,NULL)";
        return await this.ExecuteSql(statement, token);
    }

    public async Task<int> SaveGeneralBrokerAsync(EntityEntry<GeneralBroker> entry, CancellationToken token = default)
    {
        var result = entry.Entity;
        FormattableString statement = $@"
                DELETE dbo.General_Daily_Brokers WHERE IMEI='{result.Imei}'
                INSERT INTO dbo.General_Daily_Brokers
                (Id,Signal_Quality,DHT_Board_Status,DHT_Board_Temperature,IMEI,Version,SimCardNumber,InternetRemainingVolume,InternetRemainingTime,InternetRemainingUSSD,InternetTotalVolume,CreatedBy,CreatedOn,IsDelete,DeleteOn)
                VALUES(NEWID(),0,0,0,'{result.Imei}','{result.Version}','{result.SimCardNumber}','{result.InternetRemainingVolume}','{result.InternetRemainingTime}','{result.InternetRemainingUssd}','{result.InternetTotalVolume}',NULL,'{result.CreatedOn}',0,NULL)";
        return await this.ExecuteSql(statement, token);
    }

    public async Task<int> SaveGeneralPlusBrokerAsync(EntityEntry<GeneralBroker> entry, CancellationToken token = default)
    {
        var result = entry.Entity;
        FormattableString statement = $@"
                DELETE dbo.General_Daily_Brokers WHERE IMEI='{result.Imei}'
                INSERT INTO dbo.General_Daily_Brokers
                (Id,IMEI,Version,SimCardNumber,InternetRemainingVolume,InternetRemainingTime,InternetRemainingUSSD,InternetTotalVolume,CreatedBy,CreatedOn,IsDelete,DeleteOn)
                VALUES(NEWID(),'{result.Imei}','{result.Version}','{result.SimCardNumber}','{result.InternetRemainingVolume}','{result.InternetRemainingTime}','{result.InternetRemainingUssd}','{result.InternetTotalVolume}',NULL,'{result.CreatedOn}',0,null)";
        return await this.ExecuteSql(statement, token);
    }

    public async Task<int> SaveSignalBrokerAsync(EntityEntry<SignalBroker> entry, CancellationToken token = default)
    {
        var result = entry.Entity;
        FormattableString statement = $@"
                DELETE FROM Signal_Daily_Brokers WHERE (IMEI = '{result.Imei}')

                INSERT INTO Signal_Daily_Brokers (Id, IMEI, Signal_Quality, CreatedBy, CreatedOn, IsDelete, DeleteOn)
                VALUES (NEWID(), '{result.Imei}', {result.SignalQuality}, NULL, {result.CreatedOn.ToSqlFormat()}, 0, NULL);";
        return await this.ExecuteSql(statement, token);
    }
    public async Task<int> SaveVoltageBrokerAsync(EntityEntry<VoltageBroker> entry, CancellationToken token = default)
    {
        var result = entry.Entity;
        FormattableString statement = $@"
                DELETE dbo.Voltage_Daily_Brokers WHERE IMEI='{result.Imei}'
                INSERT INTO dbo.Voltage_Daily_Brokers(Id,DHT_Board_Status,IMEI,InputVoltage,BatteryVoltage,CreatedBy,CreatedOn,IsDelete,DeleteOn)
                VALUES(NEWID(),0,'{result.Imei}','{result.InputVoltage}','{result.BatteryVoltage}',NULL,{result.CreatedOn.ToSqlFormat()},0,NULL)";
        return await this.ExecuteSql(statement, token);
    }
    public async Task<int> SaveGpsBrokerAsync(EntityEntry<GpsBroker> entry, CancellationToken token = default)
    {
        var result = entry.Entity;
        FormattableString statement = $@"
                DELETE dbo.GPS_Daily_Brokers WHERE IMEI='{result.Imei}'
                INSERT INTO dbo.GPS_Daily_Brokers(Id,IMEI,GPS_DateTime,Latitude,Longitude,Speed,Altitude, Address,Angle,CreatedBy,CreatedOn,IsDelete,DeleteOn)
                VALUES(NEWID(),'{result.Imei}',{result.GpsDateTime.ToSqlFormat()},'{result.Latitude}','{result.Longitude}','{result.Speed}','{result.Altitude}',N'{result.Address}','{result.Angle}',NULL,{result.CreatedOn.ToSqlFormat()},0,null)";
        return await this.ExecuteSql(statement, token);
    }
    // Not working
    //x private Task<int> ExecuteSql(FormattableString statement, CancellationToken token) =>
    //x     this.Database.ExecuteSqlInterpolatedAsync(statement, token);

    private Task<int> ExecuteSql(FormattableString statement, CancellationToken token)
    {
        return this.Database.ExecuteSqlRawAsync(statement.ToString(), token);
    }
}
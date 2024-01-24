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

    public async Task<int> SaveCanBrokerAsync(EntityEntry<CanBroker> canBroker, CancellationToken token = default)
    {
        var result = canBroker.Entity;
        FormattableString statement = $@"
                DELETE dbo.CAN_Daily_Brokers WHERE IMEI='{result.Imei}'
                INSERT INTO dbo.CAN_Daily_Brokers(Id,IMEI,PGN,[Identifier],[Value],CreatedBy,CreatedOn,IsDelete, DeleteOn)
                VALUES(NEWID(),'{result.Imei}',{result.Pgn},'{result.Identifier}','{result.Value}',NULL,{result.CreatedOn.ToSqlFormat()},0,NULL)";
        return await ExecuteSql(statement, token);
    }

    public async Task<int> SaveGeneralBrokerAsync(EntityEntry<GeneralBroker> canBroker, CancellationToken token = default)
    {
        var result = canBroker.Entity;
        FormattableString statement = $@"
                DELETE dbo.General_Daily_Brokers WHERE IMEI='{result.Imei}'
                INSERT INTO dbo.General_Daily_Brokers
                (Id,Signal_Quality,DHT_Board_Status,DHT_Board_Temperature,IMEI,Version,SimCardNumber,InternetRemainingVolume,InternetRemainingTime,InternetRemainingUSSD,InternetTotalVolume,CreatedBy,CreatedOn,IsDelete,DeleteOn)
                VALUES(NEWID(),0,0,0,'{result.Imei}','{result.Version}','{result.SimCardNumber}','{result.InternetRemainingVolume}','{result.InternetRemainingTime}','{result.InternetRemainingUssd}','{result.InternetTotalVolume}',NULL,'{result.CreatedOn}',0,NULL)";
        return await ExecuteSql(statement, token);
    }

    private Task<int> ExecuteSql(FormattableString tsql, CancellationToken token) =>
        this.Database.ExecuteSqlInterpolatedAsync(tsql, token);
}
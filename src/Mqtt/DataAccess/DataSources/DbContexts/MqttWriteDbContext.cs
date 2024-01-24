using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Moshaveran.Infrastructure.Helpers;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

namespace Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;

public sealed class MqttWriteDbContext : MqttDbContext
{
    public MqttWriteDbContext()
    {
    }

    public MqttWriteDbContext(DbContextOptions<MqttDbContext> options) : base(options)
    {
    }

    public async Task<int> SaveCanBrokerAsync(EntityEntry<CanBroker> canBroker, CancellationToken cancellationToken = default)
    {
        var result = canBroker.Entity;
        //FormattableString statement = $@"
        var statement = $@"
                DELETE dbo.CAN_Daily_Brokers WHERE IMEI='{result.Imei}'
                INSERT INTO dbo.CAN_Daily_Brokers(Id,IMEI,PGN,[Identifier],[Value],CreatedBy,CreatedOn,IsDelete, DeleteOn)
                VALUES(NEWID(),'{result.Imei}',{result.Pgn},'{result.Identifier}','{result.Value}',NULL,{result.CreatedOn.ToSqlFormat()},0,NULL)";
        return await this.Database.ExecuteSqlRawAsync(statement, cancellationToken);
        //return await this.Database.ExecuteSqlInterpolatedAsync(statement, cancellationToken);
    }
}
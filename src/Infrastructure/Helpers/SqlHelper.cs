namespace Moshaveran.Infrastructure.Helpers;

public static class SqlHelper
{
    public static string ToSqlFormat(this DateTime date, string onNull = "null") =>
         (date == default || DBNull.Value.Equals(date)) ? onNull : $"'{date:yyyy-MM-dd HH:mm:ss}'";

    public static string ToSqlFormat(this DateTime? date, string onNull = "null") =>
         (date == null || DBNull.Value.Equals(date)) ? onNull : date.Value.ToSqlFormat();
}
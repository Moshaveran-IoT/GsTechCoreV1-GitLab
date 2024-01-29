namespace Moshaveran.Infrastructure.Helpers;

public static class SqlTypeHelper
{
    public static string ToSqlFormat(this DateTime date, string onNull = "NULL", bool isForInterpolation = true)
        => date == default || DBNull.Value.Equals(date)
            ? onNull
            : isForInterpolation ? $"{date:yyyy-MM-dd HH:mm:ss}" : $"'{date:yyyy-MM-dd HH:mm:ss}'";

    public static string ToSqlFormat(this DateTime? date, string onNull = "NULL", bool isForInterpolation = true)
        => (date == null || DBNull.Value.Equals(date)) ? onNull : date.Value.ToSqlFormat(isForInterpolation: isForInterpolation);
}
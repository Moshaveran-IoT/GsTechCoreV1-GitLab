using System.Diagnostics;

namespace Moshaveran.Library.Helpers;

[DebuggerStepThrough]
[StackTraceHidden]
public static class SqlTypeHelper
{
    public static string ToSqlFormat(this DateTime date, bool isForInterpolation = true)
        => date == default || DBNull.Value.Equals(date)
            ? GetDbNullForDateTime(isForInterpolation)
            : isForInterpolation ? $"{date:yyyy-MM-dd HH:mm:ss}" : $"'{date:yyyy-MM-dd HH:mm:ss}'";

    public static string ToSqlFormat(this DateTime? date, bool isForInterpolation = true)
        => date == null || DBNull.Value.Equals(date)
            ? GetDbNullForDateTime(isForInterpolation)
            : date.Value.ToSqlFormat(isForInterpolation: isForInterpolation);

    private static string GetDbNullForDateTime(bool isForInterpolation)
        => isForInterpolation ? string.Empty : "NULL";
}
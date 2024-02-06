using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Moshaveran.Infrastructure;

[DebuggerStepThrough]
[StackTraceHidden]
public static partial class StringHelper
{
    public static string HexToUnicode(string hex)
    {
        var bytes = hexToUnicodeRegex().Matches(hex).OfType<Match>().Select(m => Convert.ToByte(m.Groups["hex"].Value, 16)).ToArray();
        var chars = Encoding.BigEndianUnicode.GetChars(bytes);
        return string.Join("", chars);
    }

    public static bool TryParseJson<T>(string @this, out T? result)
    {
        try
        {
            result = JsonSerializer.Deserialize<T>(@this);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }

    [GeneratedRegex(@"(?<hex>[0-9A-F]{2})", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex hexToUnicodeRegex();
}
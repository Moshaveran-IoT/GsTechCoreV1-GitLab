using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

using Moshaveran.Library.Validations;

namespace Moshaveran.Library;

//[DebuggerStepThrough]
//[StackTraceHidden]
public static partial class StringHelper
{
    public static StringBuilder AppendAll(this StringBuilder sb, in IEnumerable<string?> lines)
    {
        Check.MustBeArgumentNotNull(sb);

        if (lines?.Any() == true)
        {
            foreach (var line in lines)
            {
                _ = sb.Append(line);
            }
        }

        return sb;
    }

    public static StringBuilder AppendAllLine(this StringBuilder sb, in IEnumerable<string?> lines)
    {
        Check.MustBeArgumentNotNull(sb);

        if (lines?.Any() == true)
        {
            foreach (var line in lines)
            {
                _ = sb.AppendLine(line);
            }
        }

        return sb;
    }

    public static string HexToUnicode(in string hex)
    {
        var bytes = hexToUnicodeRegex().Matches(hex).OfType<Match>().Select(m => Convert.ToByte(m.Groups["hex"].Value, 16)).ToArray();
        var chars = Encoding.BigEndianUnicode.GetChars(bytes);
        return string.Join("", chars);
    }

    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? s)
        => string.IsNullOrEmpty(s);

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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Moshaveran.Infrastructure.Helpers;

[DebuggerStepThrough]
[StackTraceHidden]
public static class JsonDocumentHelpers
{
    public static bool TryParse(string json, [NotNullWhen(true)] out JsonDocument? result)
    {
        try
        {
            result = JsonDocument.Parse(json);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }
}
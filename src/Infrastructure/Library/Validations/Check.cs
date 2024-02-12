using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Moshaveran.Library.Validations;

public static class Check
{
    public static TArg ArgumentNotNull<TArg>([AllowNull][NotNull] this TArg? arg, [CallerArgumentExpression(nameof(arg))] string argName = null!)
    {
        MustBeArgumentNotNull(arg, argName);
        return arg;
    }

    public static void MustBeArgumentNotNull([AllowNull][NotNull] object? arg, [CallerArgumentExpression(nameof(arg))] string argName = null!)
        => ArgumentNullException.ThrowIfNull(arg, argName);
}
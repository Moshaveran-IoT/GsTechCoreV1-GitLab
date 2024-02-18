using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using Moshaveran.Library.Exceptions;

namespace Moshaveran.Library.Validations;

public static class Check
{
    public static TArg ArgumentNotNull<TArg>([AllowNull][NotNull] this TArg? arg, [CallerArgumentExpression(nameof(arg))] string argName = null!)
    {
        MustBeArgumentNotNull(arg, argName);
        return arg;
    }

    public static void MustBe<TException>([DoesNotReturnIf(false)] bool ok)
        where TException : GsTechExceptionBase, new()
    {
        if (!ok)
        {
            throw new TException();
        }
    }

    public static void MustBeArgumentNotNull([AllowNull][NotNull] object? arg, [CallerArgumentExpression(nameof(arg))] string argName = null!)
        => ArgumentNullException.ThrowIfNull(arg, argName);
}
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using Moshaveran.Library.Exceptions;

namespace Moshaveran.Library.Validations;

[DebuggerStepThrough]
[StackTraceHidden]
public static class Check
{
    public static TArg ArgumentNotNull<TArg>([AllowNull][NotNull] this TArg? arg, [CallerArgumentExpression(nameof(arg))] string argName = null!)
    {
        MustBeArgumentNotNull(arg, argName);
        return arg;
    }

    public static void MustBe<TException>([DoesNotReturnIf(false)] bool ok)
        where TException : GsTechExceptionBase, new()
        => MustBe(ok, () => new TException());

    public static void MustBe([DoesNotReturnIf(false)] bool ok, Func<Exception> getException)
        => InnerMustBe(ok, getException);

    public static void MustBeArgumentNotNull([AllowNull][NotNull] object? arg, [CallerArgumentExpression(nameof(arg))] string argName = null!)
        => InnerMustBe(arg != null, () => new ArgumentNullException(argName));

    private static void InnerMustBe([DoesNotReturnIf(false)] bool ok, Func<Exception> getException)
    {
        if (!ok)
        {
            throw getException();
        }
    }
}
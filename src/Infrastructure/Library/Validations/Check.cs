using Moshaveran.Library.Exceptions;
using Moshaveran.Library.Results;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Moshaveran.Library.Validations;

[DebuggerStepThrough, StackTraceHidden]
public static class Check
{
    public static TArg ArgumentNotNull<TArg>([AllowNull][NotNull] this TArg? arg, [CallerArgumentExpression(nameof(arg))] string argName = null!)
    {
        MustBeArgumentNotNull(arg, argName);
        return arg;
    }

    public static IResult If(bool ok)
        => ok ? IResult.Succeed : IResult.Failed;

    public static IResult<TValue> If<TValue>(bool ok, in Func<TValue> isTrue, in Func<TValue> isFalse)
        => ok ? IResult.Success(isTrue.ArgumentNotNull()()) : IResult.Fail(isFalse.ArgumentNotNull()());

    public static void MustBe<TException>([DoesNotReturnIf(false)] bool ok)
                where TException : GsTechExceptionBase, new()
        => InnerMustBe(ok, () => new TException());

    public static void MustBe([DoesNotReturnIf(false)] bool ok, Func<Exception> getException)
        => InnerMustBe(ok, getException);

    public static void MustBeArgumentNotNull([AllowNull][NotNull] object? arg, [CallerArgumentExpression(nameof(arg))] string argName = null!)
        => InnerMustBe(arg != null, () => new ArgumentNullException(argName));

    private static void InnerMustBe([DoesNotReturnIf(false)] bool ok, Func<Exception> getException)
    {
        if (ok)
        {
            return;
        }
        throw getException();
    }
}
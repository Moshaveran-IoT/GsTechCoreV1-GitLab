using Moshaveran.Library.Results;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Moshaveran.Library.Internals.Results;

[DebuggerStepThrough, StackTraceHidden]
internal sealed record Result(bool IsSucceed, string? Message = null, IEnumerable<Exception>? Exceptions = null, IResult? InnerResult = null)
    : IResult
{
    public Result(IResult result)
        : this(result.IsSucceed, result.Message, result.Exceptions, result.InnerResult)
    {
    }

    private static IResult? _failed;
    private static IResult? _succeed;

    [NotNull]
    public static IResult Failed => _failed ??= IResult.Fail();

    [NotNull]
    public static IResult Succeed => _succeed ??= IResult.Success();
}
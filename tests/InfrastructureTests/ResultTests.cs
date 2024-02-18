using Moshaveran.Library;
using Moshaveran.Library.Results;

namespace InfrastructureTests;

[Trait("Category", nameof(Moshaveran.Library.Results))]
public sealed class ResultTests
{
    [Fact]
    public void Generic_FailedHealthTest()
    {
        IResult result = Result<int>.Failed;
        _ = result.Should().NotBeNull();
        _ = result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Generic_SucceedHealthTest()
    {
        IResult result = Result<int>.Succeed;
        _ = result.Should().NotBeNull();
        _ = result.IsSucceed.Should().BeTrue();
    }

    [Fact]
    public void Simple_FailedHealthTest()
    {
        IResult result = Result.Failed;
        _ = result.Should().NotBeNull();
        _ = result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Simple_SucceedHealthTest()
    {
        IResult result = Result.Succeed;
        _ = result.Should().NotBeNull();
        _ = result.IsSucceed.Should().BeTrue();
    }
}
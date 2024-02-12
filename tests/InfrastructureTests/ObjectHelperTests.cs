using Moshaveran.Infrastructure.Helpers;

namespace InfrastructureTests;

public sealed class ObjectHelperTests
{
    [Fact]
    public void Pretty()
    {
        var p = new
        {
            Name = "Ali",
            Age = 15
        };
        var pretty = ObjectHelper.Pretty(p);
    }
}
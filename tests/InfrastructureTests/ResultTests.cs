using Moshaveran.Library;
using Moshaveran.Library.Results;

namespace InfrastructureTests;

[Trait("Category", nameof(Moshaveran.Library.Results))]
public sealed class ResultTests
{
    [Fact]
    public void Failed_Generic_FailedHealthTest()
    {
        IResult result = Result<int>.Failed;
        _ = result.Should().NotBeNull();
        _ = result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Succeed_Generic_SucceedHealthTest()
    {
        IResult result = Result<int>.Succeed;
        _ = result.Should().NotBeNull();
        _ = result.IsSucceed.Should().BeTrue();
    }

    [Fact]
    public void Failed_FailedHealthTest()
    {
        IResult result = Result.Failed;
        _ = result.Should().NotBeNull();
        _ = result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Succeed_SucceedHealthTest()
    {
        IResult result = Result.Succeed;
        _ = result.Should().NotBeNull();
        _ = result.IsSucceed.Should().BeTrue();
    }


    [Fact]
    public void OnSucceed_DoesNotExecuteActionWhenResultIsNull()
    {
        // Arrange
        Result? result = null;
        bool actionExecuted = false;

        void Action(Result r)
        {
            actionExecuted = true;
        }

        // Act
        var returnedResult = result.OnSucceed(Action);

        // Assert
        actionExecuted.Should().BeFalse();
        returnedResult.Should().BeNull();
    }

    [Fact]
    public void OnSucceed_ExecutesActionWhenResultIsSuccessful()
    {
        // Arrange
        var result = Result.Succeed;
        bool actionExecuted = false;

        void Action(Result r)
        {
            actionExecuted = true;
        }

        // Act
        var returnedResult = result.OnSucceed(Action);

        // Assert
        actionExecuted.Should().BeTrue();
        returnedResult.Should().Be(Result.Succeed);
    }

    [Fact]
    public void OnSucceed_DoesNotExecuteActionWhenResultIsFailure()
    {
        // Arrange
        var result = Result.Failed;
        bool actionExecuted = false;

        void Action(Result r)
        {
            actionExecuted = true;
        }

        // Act
        var returnedResult = result.OnSucceed(Action);

        // Assert
        actionExecuted.Should().BeFalse();
        returnedResult.Should().Be(Result.Failed);
    }

    [Fact]
    public void OnFailure_DoesNotExecuteActionWhenResultIsNull()
    {
        // Arrange
        Result? result = null;
        bool actionExecuted = false;

        void Action(Result r)
        {
            actionExecuted = true;
        }

        // Act
        var returnedResult = result.OnFailure(Action);

        // Assert
        actionExecuted.Should().BeTrue();
        returnedResult.Should().BeNull();
    }

    [Fact]
    public void OnFailure_DoesNotExecuteActionWhenResultIsSuccessful()
    {
        // Arrange
        var result = Result.Succeed;
        bool actionExecuted = false;

        void Action(Result r)
        {
            actionExecuted = true;
        }

        // Act
        var returnedResult = result.OnFailure(Action);

        // Assert
        actionExecuted.Should().BeFalse();
        returnedResult.Should().Be(Result.Succeed);
    }

    [Fact]
    public void OnFailure_ExecutesActionWhenResultIsFailure()
    {
        // Arrange
        var result = Result.Failed;
        bool actionExecuted = false;

        void Action(Result r)
        {
            actionExecuted = true;
        }

        // Act
        var returnedResult = result.OnFailure(Action);

        // Assert
        actionExecuted.Should().BeTrue();
        returnedResult.Should().Be(Result.Failed);
    }

    [Fact]
    public async Task OnFailureAsync__ReturnsDefaultFuncResultWhenResultIsSuccessful()
    {
        // Arrange
        var resultTask = Task.FromResult(Result.Succeed);
        var defaultFuncResult = "Default Result";

        // Act
        var returnedResult = await resultTask.OnFailure(r => "Action Result", defaultFuncResult);

        // Assert
        returnedResult.Should().Be(defaultFuncResult);
    }

    [Fact]
    public async Task OnFailureAsync_ExecutesActionWhenResultIsFailure()
    {
        // Arrange
        var resultTask = Task.FromResult(Result.Failed);
        var defaultFuncResult = "Default Result";

        // Act
        var returnedResult = await resultTask.OnFailure(r => "Action Result", defaultFuncResult);

        // Assert
        returnedResult.Should().Be("Action Result");
    }

    [Fact]
    public async Task OnSucceedAsync_ExecutesActionWhenResultIsSuccessful()
    {
        // Arrange
        var resultTask = Task.FromResult(Result.Succeed);
        bool actionExecuted = false;

        void Action(Result r)
        {
            actionExecuted = true;
        }

        // Act
        var returnedResult = await resultTask.OnSucceed(Action);

        // Assert
        actionExecuted.Should().BeTrue();
        returnedResult.Should().Be(Result.Succeed);
    }

    [Fact]
    public async Task OnSucceedAsync_DoesNotExecuteActionWhenResultIsFailure()
    {
        // Arrange
        var resultTask = Task.FromResult(Result.Failed);
        bool actionExecuted = false;

        void Action(Result r)
        {
            actionExecuted = true;
        }

        // Act
        var returnedResult = await resultTask.OnSucceed(Action);

        // Assert
        actionExecuted.Should().BeFalse();
        returnedResult.Should().Be(Result.Failed);
    }

    [Fact]
    public async Task OnSucceedAsync_ThrowsArgumentNullExceptionWhenResultAsyncIsNull()
    {
        // Arrange
        Task<Result> resultTask = null;

        void Action(Result r)
        {
            // This action should not be executed.
        }

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => resultTask.OnSucceed(Action));
    }
}
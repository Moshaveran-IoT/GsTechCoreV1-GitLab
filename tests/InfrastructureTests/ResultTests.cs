using Moshaveran.Library;
using Moshaveran.Library.Results;

namespace InfrastructureTests;

[Trait("Category", nameof(Moshaveran.Library.Results))]
public sealed class ResultTests
{
    [Fact]
    public async Task Deconstruct_ShouldReturnResultAndNullValueOnNullResult()
    {
        // Arrange
        var nullResultTask = Task.FromResult<IResult<string?>>(null!);

        // Act
        _ = await Assert.ThrowsAsync<NullReferenceException>(() => nullResultTask.Deconstruct());
    }

    [Fact]
    public async Task Deconstruct_ShouldReturnResultAndValueOnFailure()
    {
        // Arrange
        var failureResult = Result.Fail<string?>(null);
        var taskResult = Task.FromResult(failureResult);

        // Act
        var (result, value) = await taskResult.Deconstruct();

        // Assert
        Assert.False(result.IsSucceed);
        Assert.Null(value);
    }

    [Fact]
    public async Task Deconstruct_ShouldReturnResultAndValueOnSuccess()
    {
        // Arrange
        var successResult = Result.Success<string?>("Success Value");
        var taskResult = Task.FromResult(successResult);

        // Act
        var (result, value) = await taskResult.Deconstruct();

        // Assert
        Assert.True(result.IsSucceed);
        Assert.Equal("Success Value", value);
    }

    [Fact]
    public void Failed_FailedHealthTest()
    {
        IResult result = Result.Failed;
        _ = result.Should().NotBeNull();
        _ = result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Failed_Generic_FailedHealthTest()
    {
        IResult result = Result<int>.Failed;
        _ = result.Should().NotBeNull();
        _ = result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void OnFailure_DoesNotExecuteActionWhenResultIsNull()
    {
        // Arrange
        Result? result = null;
        var actionExecuted = false;

        void Action(Result r) => actionExecuted = true;

        // Act
        var returnedResult = result.OnFailure(Action);

        // Assert
        _ = actionExecuted.Should().BeTrue();
        _ = returnedResult.Should().BeNull();
    }

    [Fact]
    public void OnFailure_DoesNotExecuteActionWhenResultIsSuccessful()
    {
        // Arrange
        var result = Result.Succeed;
        var actionExecuted = false;

        void Action(Result r) => actionExecuted = true;

        // Act
        var returnedResult = result.OnFailure(Action);

        // Assert
        _ = actionExecuted.Should().BeFalse();
        _ = returnedResult.Should().Be(Result.Succeed);
    }

    [Fact]
    public void OnFailure_ExecutesActionWhenResultIsFailure()
    {
        // Arrange
        var result = Result.Failed;
        var actionExecuted = false;

        void Action(Result r) => actionExecuted = true;

        // Act
        var returnedResult = result.OnFailure(Action);

        // Assert
        _ = actionExecuted.Should().BeTrue();
        _ = returnedResult.Should().Be(Result.Failed);
    }

    [Fact]
    public async Task OnFailureAsync_ExecutesActionWhenResultIsFailure()
    {
        // Arrange
        var resultTask = Task.FromResult(Result.Failed);
        const string defaultFuncResult = "Default Result";

        // Act
        var returnedResult = await resultTask.OnFailure(_ => "Action Result", defaultFuncResult);

        // Assert
        _ = returnedResult.Should().Be("Action Result");
    }

    [Fact]
    public void OnSucceed_DoesNotExecuteActionWhenResultIsFailure()
    {
        // Arrange
        var result = Result.Failed;
        var actionExecuted = false;

        void Action(Result r) => actionExecuted = true;

        // Act
        var returnedResult = result.OnSucceed(Action);

        // Assert
        _ = actionExecuted.Should().BeFalse();
        _ = returnedResult.Should().Be(Result.Failed);
    }

    [Fact]
    public void OnSucceed_DoesNotExecuteActionWhenResultIsNull()
    {
        // Arrange
        Result? result = null;
        var actionExecuted = false;

        void Action(Result r) => actionExecuted = true;


        // Assert
        _ = actionExecuted.Should().BeFalse();

        // Act
        var returnedResult = result.OnSucceed<Result>(Action);
        _ = returnedResult.Should().BeNull();
    }

    [Fact]
    public void OnSucceed_ExecutesActionWhenResultIsSuccessful()
    {
        // Arrange
        var result = Result.Succeed;
        var actionExecuted = false;

        void Action(Result r) => actionExecuted = true;

        // Act
        var returnedResult = result.OnSucceed(Action);

        // Assert
        _ = actionExecuted.Should().BeTrue();
        _ = returnedResult.Should().Be(Result.Succeed);
    }

    [Fact]
    public void OnSucceed_ShouldExecuteActionOnSuccess()
    {
        // Arrange
        var successResult = Result.Succeed;
        var action = new Func<IResult, int>(_ => 42);
        const int defaultFuncResult = 0;

        // Act
        var result = successResult.OnSucceed(action, defaultFuncResult);

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public void OnSucceed_ShouldReturnDefaultOnFailure()
    {
        // Arrange
        var failureResult = Result.Fail("Error message");
        var action = new Func<IResult, int>(_ => 42);
        const int defaultFuncResult = 0;

        // Act
        var result = failureResult.OnSucceed(action, defaultFuncResult);

        // Assert
        Assert.Equal(defaultFuncResult, result);
    }

    [Fact]
    public async Task OnSucceedAsync_DoesNotExecuteActionWhenResultIsFailure()
    {
        // Arrange
        var resultTask = Task.FromResult(Result.Failed);
        var actionExecuted = false;

        void Action(Result r) => actionExecuted = true;

        // Act
        var returnedResult = await resultTask.OnSucceed(Action);

        // Assert
        _ = actionExecuted.Should().BeFalse();
        _ = returnedResult.Should().Be(Result.Failed);
    }

    [Fact]
    public async Task OnSucceedAsync_ExecutesActionWhenResultIsSuccessful()
    {
        // Arrange
        var resultTask = Task.FromResult(Result.Succeed);
        var actionExecuted = false;

        void Action(Result r) => actionExecuted = true;

        // Act
        var returnedResult = await resultTask.OnSucceed(Action);

        // Assert
        _ = actionExecuted.Should().BeTrue();
        _ = returnedResult.Should().Be(Result.Succeed);
    }

    [Fact]
    public async Task OnSucceedAsync_Should_Return_Default_On_Failure()
    {
        // Arrange
        var failureResult = Result.Fail("Error message").ToAsync();
        var action = new Func<IResult, int>(_ => 42);
        const int defaultFuncResult = 0;

        // Act
        var result = await failureResult.OnSucceedAsync(action, defaultFuncResult);

        // Assert
        Assert.Equal(defaultFuncResult, result);
    }

    [Fact]
    public async Task OnSucceedAsync_Should_Throw_On_Null_Result()
    {
        // Arrange
        Task<IResult>? nullResultTask = null;
        var action = new Func<IResult, int>(_ => 42);
        const int defaultFuncResult = 0;

        // Act & Assert
        _ = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await nullResultTask!.OnSucceedAsync(action, defaultFuncResult));
    }

    [Fact]
    public async Task OnSucceedAsync_ShouldExecute_ActionOnSuccess()
    {
        // Arrange
        var successResult = Result.Succeed.ToAsync();
        var action = new Func<IResult, int>(_ => 42);
        const int defaultFuncResult = 0;

        // Act
        var result = await successResult.OnSucceedAsync(action, defaultFuncResult);

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public async Task OnSucceedAsync_ThrowsArgumentNullExceptionWhenResultAsyncIsNull()
    {
        // Arrange
        Task<Result>? resultTask = null;

        static void Action(Result r)
        {
            // This action should not be executed.
        }

        // Act & Assert
        _ = await Assert.ThrowsAsync<ArgumentNullException>(() => resultTask!.OnSucceed(Action));
    }

    [Fact]
    public void Process_ShouldExecuteOnFailureOnFailure()
    {
        // Arrange
        IResult failureResult = Result.Fail("Error message");
        var onSucceed = new Func<IResult, int>(_ => 42);
        var onFailure = new Func<IResult?, int>(_ => 0);

        // Act
        var result = failureResult.Process(onSucceed, onFailure);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Process_ShouldExecuteOnFailureOnNullResult()
    {
        // Arrange
        IResult? nullResult = null;
        var onSucceed = new Func<IResult, int>(_ => 42);
        var onFailure = new Func<IResult?, int>(_ => 0);

        // Act
        var result = nullResult.Process(onSucceed, onFailure);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Process_ShouldExecuteOnSucceedOnSuccess()
    {
        // Arrange
        IResult successResult = Result.Succeed;
        var onSucceed = new Func<IResult, int>(_ => 42);
        var onFailure = new Func<IResult?, int>(_ => 0);

        // Act
        var result = successResult.Process(onSucceed, onFailure);

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public async Task ProcessAsync_ShouldExecuteOnFailureOnFailure()
    {
        // Arrange
        var failureResult = Result.Fail("Error message").ToAsync();
        var onSucceed = new Func<IResult, int>(_ => 42);
        var onFailure = new Func<IResult?, int>(_ => 0);

        // Act
        var result = await failureResult.Process(onSucceed, onFailure);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task ProcessAsync_ShouldExecuteOnFailureOnNullResult()
    {
        // Arrange
        var nullResultTask = Task.FromResult<IResult?>(null);
        var onSucceed = new Func<IResult, int>(_ => 42);
        var onFailure = new Func<IResult?, int>(_ => 0);

        // Act
        var result = await nullResultTask.Process(onSucceed, onFailure);

        // Assert
        Assert.Equal(0, result);
    }
    [Fact]
    public async Task ProcessAsync_ShouldExecuteOnSucceedOnSuccess()
    {
        // Arrange
        var successResult = Result.Succeed.ToAsync();
        var onSucceed = new Func<IResult, int>(_ => 42);
        var onFailure = new Func<IResult?, int>(_ => 0);

        // Act
        var result = await successResult.Process(onSucceed, onFailure);

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public void Succeed_Generic_SucceedHealthTest()
    {
        IResult result = Result<int>.Succeed;
        _ = result.Should().NotBeNull();
        _ = result.IsSucceed.Should().BeTrue();
    }

    [Fact]
    public void Succeed_SucceedHealthTest()
    {
        IResult result = Result.Succeed;
        _ = result.Should().NotBeNull();
        _ = result.IsSucceed.Should().BeTrue();
    }

    [Fact]
    public void WithValue_ShouldCreateResultWithValueOnFailure()
    {
        // Arrange
        IResult failureResult = Result.Fail("Error message");
        const string value = "Failure Value";

        // Act
        var resultWithValue = failureResult.WithValue(value);

        // Assert
        Assert.False(resultWithValue.IsSucceed);
        Assert.Equal(value, resultWithValue.Value);
    }

    [Fact]
    public void WithValue_ShouldCreateResultWithValueOnSuccess()
    {
        // Arrange
        IResult successResult = Result.Succeed;
        const string value = "Success Value";

        // Act
        var resultWithValue = successResult.WithValue(value);

        // Assert
        Assert.True(resultWithValue.IsSucceed);
        Assert.Equal(value, resultWithValue.Value);
    }
}
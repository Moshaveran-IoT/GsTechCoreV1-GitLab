using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using Moshaveran.Library.Results;

namespace InfrastructureTests;

[Trait("Category", nameof(Moshaveran.Library.Results))]
public sealed class ResultTests
{
    [Fact]
    public void CreateFail_ReturnsFailResult()
    {
        // Arrange
        const bool succeed = false;

        // Act
        var result = IResult.Create(succeed);

        // Assert
        Assert.False(result.IsSucceed);
        Assert.True(result.IsFailure);
        Assert.Null(result.Message);
        Assert.Empty(result.Exceptions);
    }

    [Fact]
    public void CreateSucceed_ReturnsSucceedResult()
    {
        // Arrange
        const bool succeed = true;

        // Act
        var result = IResult.Create(succeed);

        // Assert
        Assert.True(result.IsSucceed);
        Assert.False(result.IsFailure);
        Assert.Null(result.Message);
        Assert.Empty(result.Exceptions);
    }

    [Fact]
    public void Deconstruct_WithResult_ReturnsCorrectValues()
    {
        // Arrange
        var result = IResult.Success<int?>(42);

        // Act
        var (deconstructedResult, deconstructedValue) = result;

        // Assert
        Assert.Equal(result, deconstructedResult);
        Assert.Equal(42, deconstructedValue);
    }

    [Fact]
    public void Fail_WithCustomClassValue_ReturnsFailResultWithCustomClassValue()
    {
        // Arrange
        var value = new CustomClass { Property = "Test" }; // مثالی از یک کلاس سفارشی

        // Act
        var result = IResult.Fail(value);

        // Assert
        Assert.False(result.IsSucceed);
        Assert.True(result.IsFailure);
        Assert.Null(result.Message);
        Assert.Empty(result.Exceptions);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void Fail_WithEmptyValue_ReturnsFailResultWithEmptyValue()
    {
        // Arrange
        var value = string.Empty; // مثالی از یک مقدار خالی

        // Act
        var result = IResult.Fail(value: value);

        // Assert
        Assert.False(result.IsSucceed);
        Assert.True(result.IsFailure);
        Assert.Null(result.Message);
        Assert.Empty(result.Exceptions);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void Fail_WithException_ReturnsFailResultWithException()
    {
        // Arrange
        var exception = new Exception("Test exception");

        // Act
        var result = IResult.Fail(exception);

        // Assert
        Assert.False(result.IsSucceed);
        Assert.True(result.IsFailure);
        Assert.Null(result.Message);
        _ = Assert.Single(result.Exceptions);
        Assert.Contains(exception, result.Exceptions);
    }

    [Fact]
    public void Fail_WithMessage_HasEmptyExceptions()
    {
        // Arrange
        const string message = "Test failure message";

        // Act
        var result = IResult.Fail(message);

        // Assert
        Assert.Empty(result.Exceptions);
    }

    [Fact]
    public void Fail_WithMessage_ReturnsFailResultWithMessage()
    {
        // Arrange
        const string message = "Test failure message";

        // Act
        var result = IResult.Fail(message);

        // Assert
        Assert.False(result.IsSucceed);
        Assert.True(result.IsFailure);
        Assert.Equal(message, result.Message);
    }

    [Fact]
    public void Fail_WithMessageAndNullValue_HasEmptyExceptions()
    {
        // Arrange
        const string message = "Test failure message";

        // Act
        var result = IResult.Fail<int?>(message);

        // Assert
        Assert.Empty(result.Exceptions);
    }

    [Fact]
    public void Fail_WithMessageAndNullValue_ReturnsFailResultWithMessage()
    {
        // Arrange
        const string message = "Test failure message";

        // Act
        var result = IResult.Fail<int?>(message);

        // Assert
        Assert.False(result.IsSucceed);
        Assert.True(result.IsFailure);
        Assert.Equal(message, result.Message);
    }

    [Fact]
    public void Fail_WithMessageAndNullValue_ReturnsFailResultWithNullValue()
    {
        // Arrange
        const string message = "Test failure message";

        // Act
        var result = IResult.Fail<int?>(message);

        // Assert
        Assert.Null(result.Value);
    }

    [Fact]
    public void Fail_WithNullableCustomClass_ReturnsFailResultWithNullValue()
    {
        // Arrange هیچ مقداری برای تعریف نیاز نیست زیرا متد خودش مقدار خالی را برمی‌گرداند

        // Act
        var result = IResult.Fail<CustomClass?>();

        // Assert
        Assert.False(result.IsSucceed);
        Assert.True(result.IsFailure);
        Assert.Null(result.Message);
        Assert.Empty(result.Exceptions);
        Assert.Null(result.Value);
    }

    [Fact]
    public void Fail_WithNullableValueType_ReturnsFailResultWithNullValue()
    {
        // Arrange هیچ مقداری برای تعریف نیاز نیست زیرا متد خودش مقدار خالی را برمی‌گرداند

        // Act
        var result = IResult.Fail<int?>();

        // Assert
        Assert.False(result.IsSucceed);
        Assert.True(result.IsFailure);
        Assert.Null(result.Message);
        Assert.Empty(result.Exceptions);
        Assert.Null(result.Value);
    }

    [Fact]
    public void Fail_WithNullValue_ReturnsFailResultWithNullValue()
    {
        // Arrange
        const string? value = null; // مثالی از یک مقدار null

        // Act
        var result = IResult.Fail<string>(value!);

        // Assert
        Assert.False(result.IsSucceed);
        Assert.True(result.IsFailure);
        Assert.Null(result.Message);
        Assert.Empty(result.Exceptions);
        Assert.Null(result.Value);
    }

    [Fact]
    public void Fail_WithoutValue_HasEmptyExceptions()
    {
        // Arrange هیچ مقداری برای تعریف نیاز نیست زیرا متد خودش نتیجه ناموفق بدون مقدار نهایی را برمی‌گرداند

        // Act
        var result = IResult.Fail();

        // Assert
        Assert.Empty(result.Exceptions);
    }

    [Fact]
    public void Fail_WithoutValue_HasNullMessage()
    {
        // Arrange هیچ مقداری برای تعریف نیاز نیست زیرا متد خودش نتیجه ناموفق بدون مقدار نهایی را برمی‌گرداند

        // Act
        var result = IResult.Fail();

        // Assert
        Assert.Null(result.Message);
    }

    [Fact]
    public void Fail_WithoutValue_ReturnsFailResultWithoutValue()
    {
        // Arrange هیچ مقداری برای تعریف نیاز نیست زیرا متد خودش نتیجه ناموفق بدون مقدار نهایی را برمی‌گرداند

        // Act
        var result = IResult.Fail();

        // Assert
        Assert.False(result.IsSucceed);
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Fail_WithValidValue_ReturnsFailResultWithValidValue()
    {
        // Arrange
        const string value = "Test value";
        var exception = new Exception("Test exception");

        // Act
        var result = IResult.Fail(value, exception);

        // Assert
        Assert.Equal(exception, result.Exceptions.First());
    }

    [Fact]
    public void Fail_WithValue_ReturnsFailResultWithValue()
    {
        // Arrange
        const string value = "Test value";

        // Act
        var result = IResult.Fail(value: value);

        // Assert
        Assert.False(result.IsSucceed);
        Assert.True(result.IsFailure);
        Assert.Null(result.Message);
        Assert.Empty(result.Exceptions);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void Fail_WithValueAndException_HasNullMessage()
    {
        // Arrange
        const string value = "Test value";
        var exception = new Exception("Test exception");

        // Act
        var result = IResult.Fail(value, exception);

        // Assert
        Assert.Null(result.Message);
    }

    [Fact]
    public void Fail_WithValueAndException_ReturnsFailResultWithException()
    {
        // Arrange هیچ مقداری برای تعریف نیاز نیست زیرا متد خودش نتیجه ناموفق بدون مقدار نهایی را برمی‌گرداند

        // Act
        var result = IResult.Fail();

        // Assert
        Assert.Null(result.Message);
    }

    [Fact]
    public void Fail_WithValueAndException_ReturnsFailResultWithValue()
    {
        // Arrange
        const string value = "Test value";
        var exception = new Exception("Test exception");

        // Act
        var result = IResult.Fail(value, exception);

        // Assert
        Assert.False(result.IsSucceed);
        Assert.True(result.IsFailure);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void Fail_WithValueAndMessage_HasEmptyExceptions()
    {
        // Arrange
        const string value = "Test value";
        const string message = "Test failure message";

        // Act
        var result = IResult.Fail(value, message);

        // Assert
        Assert.Empty(result.Exceptions);
    }

    [Fact]
    public void Fail_WithValueAndMessage_ReturnsFailResultWithMessage()
    {
        // Arrange
        const string value = "Test value";
        const string message = "Test failure message";

        // Act
        var result = IResult.Fail(value, message);

        // Assert
        Assert.Equal(message, result.Message);
    }

    [Fact]
    public void Fail_WithValueAndMessage_ReturnsFailResultWithValue()
    {
        // Arrange
        const string value = "Test value";
        const string message = "Test failure message";

        // Act
        var result = IResult.Fail(value, message);

        // Assert
        Assert.False(result.IsSucceed);
        Assert.True(result.IsFailure);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void Fail_WithValueAndMessage_ReturnsFailResultWithValueAndMessage()
    {
        // Arrange
        const string value = "Test value";
        const string message = "Test failure message";

        // Act
        var result = IResult.Fail(value, message);

        // Assert
        Assert.False(result.IsSucceed);
        Assert.True(result.IsFailure);
        Assert.Equal(message, result.Message);
        Assert.Empty(result.Exceptions);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void FailedProperty_HasEmptyExceptions()
    {
        // Arrange هیچ مقداری برای تعریف نیاز نیست زیرا متد خودش نتیجه ناموفق بدون مقدار نهایی را برمی‌گرداند

        // Act
        var result = IResult<int?>.Failed;

        // Assert
        Assert.Empty(result.Exceptions);
    }

    [Fact]
    public void FailedProperty_HasNullMessage()
    {
        // Arrange هیچ مقداری برای تعریف نیاز نیست زیرا متد خودش نتیجه ناموفق بدون مقدار نهایی را برمی‌گرداند

        // Act
        var result = IResult<int?>.Failed;

        // Assert
        Assert.Null(result.Message);
    }

    [Fact]
    public void FailedProperty_HasNullValue()
    {
        // Arrange هیچ مقداری برای تعریف نیاز نیست زیرا متد خودش نتیجه ناموفق بدون مقدار نهایی را برمی‌گرداند

        // Act
        var result = IResult<int?>.Failed;

        // Assert
        Assert.Null(result.Value);
    }

    [Fact]
    public void FailedProperty_ReturnsFailedResultWithNullValue()
    {
        // Arrange هیچ مقداری برای تعریف نیاز نیست زیرا متد خودش نتیجه ناموفق بدون مقدار نهایی را برمی‌گرداند

        // Act
        var result = IResult<int?>.Failed;

        // Assert
        Assert.False(result.IsSucceed);
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task GetValueAsync_WithSuccessResult_ReturnsValue()
    {
        // Arrange
        const string expectedValue = "Test value";
        var successResult = Task.FromResult(IResult.Success<string?>(expectedValue));

        // Act
        var actualValue = await successResult.GetValueAsync();

        // Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void IsSucceed_WhenFailure_ReturnsFalse()
    {
        // Arrange
        var failureResult = IResult.Fail<int?>(new Exception("Failure"));

        // Act
        var isSuccess = failureResult.IsSucceed();

        // Assert
        Assert.False(isSuccess);
    }

    [Fact]
    public void IsSucceed_WhenNull_ReturnsFalse()
    {
        // Arrange
        IResult? nullResult = null;

        // Act
        var isSuccess = nullResult.IsSucceed();

        // Assert
        Assert.False(isSuccess);
    }

    [Fact]
    public void IsSucceed_WhenSuccess_ReturnsTrue()
    {
        // Arrange
        var successResult = IResult.Success<int?>(42);

        // Act
        var isSuccess = successResult.IsSucceed();

        // Assert
        Assert.True(isSuccess);
    }

    [Fact]
    public void OnFailure_WhenFailure_ExecutesAction()
    {
        // Arrange
        var result = IResult.Fail<int?>(null, "Error");
        var actionExecuted = false;

        // Act
        _ = result.OnFailure(_ => actionExecuted = true);

        // Assert
        Assert.True(actionExecuted);
    }

    [Fact]
    public async Task OnFailureAsync_WhenFailure_ExecutesActionAndReturnsResult()
    {
        // Arrange
        var resultTask = Task.FromResult(IResult.Fail<int?>(null, "Error"));
        var actionExecuted = false;
        const string expectedResult = "Expected Result";

        // Act
        var actualResult = await resultTask!.OnFailure(
            _ =>
            {
                actionExecuted = true;
                return expectedResult;
            },
            defaultFuncResult: "Default Result"
        );

        // Assert
        Assert.True(actionExecuted);
        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void OnSucceed_WhenSuccess_ExecutesAction()
    {
        // Arrange
        var result = IResult.Success<int?>(42);
        var actionExecuted = false;

        // Act
        _ = result.OnSucceed(_ => actionExecuted = true);

        // Assert
        Assert.True(actionExecuted);
    }

    [Fact]
    public void OnSucceed_WhenSuccess_ExecutesActionAndReturnsResult()
    {
        // Arrange
        var result = IResult.Success<int?>(42);
        const string expectedResult = "Success";

        // Act
        var actualResult = result.OnSucceed(_ => expectedResult, "Failure");

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public async Task OnSucceedAsync_WhenSuccess_ExecutesAction()
    {
        // Arrange
        var resultTask = Task.FromResult(IResult.Success<int?>(42));
        var actionExecuted = false;

        // Act
        var result = await resultTask.OnSucceed(_ => actionExecuted = true);

        // Assert
        Assert.True(result.IsSucceed);
        Assert.True(actionExecuted);
    }

    [Fact]
    public async Task OnSucceedAsync_WhenSuccess_ExecutesActionAndReturnsResult()
    {
        // Arrange
        var resultTask = ValueTask.FromResult(IResult.Success<int?>(42));
        const string expectedResult = "Success";

        // Act
        var actualResult = await resultTask.OnSucceedAsync(_ => expectedResult, "Failure");

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void Process_WhenSuccess_ExecutesOnSucceed()
    {
        // Arrange
        var result = IResult.Success<int?>(42);
        var onSucceedExecuted = false;
        var onFailureExecuted = false;

        // Act
        _ = result.Process(
            onSucceed: _ =>
            {
                onSucceedExecuted = true;
                return 0;
            },
            onFailure: _ =>
            {
                onFailureExecuted = true;
                return 0;
            }
        );

        // Assert
        Assert.True(onSucceedExecuted);
        Assert.False(onFailureExecuted);
    }

    [Fact]
    public async Task ProcessAsync_WhenFailure_ExecutesOnFailure()
    {
        // Arrange
        var resultTask = Task.FromResult(IResult.Fail<int?>(new Exception("Failure")));
        const string expectedSuccessResult = "Success";
        const string expectedFailureResult = "Failure";

        // Act
        var actualResult = await resultTask!.Process(
            onSucceed: _ => expectedSuccessResult,
            onFailure: _ => expectedFailureResult
        );

        // Assert
        Assert.Equal(expectedFailureResult, actualResult);
    }

    [Fact]
    public async Task ProcessAsync_WhenSuccess_ExecutesOnSucceed()
    {
        // Arrange
        var resultTask = Task.FromResult(IResult.Success<int?>(42));
        const string expectedSuccessResult = "Success";
        const string expectedFailureResult = "Failure";

        // Act
        var actualResult = await resultTask!.Process(
            onSucceed: _ => expectedSuccessResult,
            onFailure: _ => expectedFailureResult
        );

        // Assert
        Assert.Equal(expectedSuccessResult, actualResult);
    }

    [Fact]
    public void SucceedProperty_HasEmptyExceptions()
    {
        // Arrange هیچ مقداری برای تعریف نیاز نیست زیرا متد خودش نتیجه موفقیت آمیز بدون مقدار نهایی
        // را برمی‌گرداند

        // Act
        var result = IResult<int?>.Succeed;

        // Assert
        Assert.Empty(result.Exceptions);
    }

    [Fact]
    public void SucceedProperty_HasNullMessage()
    {
        // Arrange هیچ مقداری برای تعریف نیاز نیست زیرا متد خودش نتیجه موفقیت آمیز بدون مقدار نهایی
        // را برمی‌گرداند

        // Act
        var result = IResult<int?>.Succeed;

        // Assert
        Assert.Null(result.Message);
    }

    [Fact]
    public void SucceedProperty_HasNullValue()
    {
        // Arrange هیچ مقداری برای تعریف نیاز نیست زیرا متد خودش نتیجه موفقیت آمیز بدون مقدار نهایی
        // را برمی‌گرداند

        // Act
        var result = IResult<int?>.Succeed;

        // Assert
        Assert.Null(result.Value);
    }

    [Fact]
    public void SucceedProperty_ReturnsSuccessResultWithNullValue()
    {
        // Arrange هیچ مقداری برای تعریف نیاز نیست زیرا متد خودش نتیجه موفقیت آمیز بدون مقدار نهایی
        // را برمی‌گرداند

        // Act
        var result = IResult<int?>.Succeed;

        // Assert
        Assert.True(result.IsSucceed);
        Assert.False(result.IsFailure);
    }

    [Fact]
    public void Success_WithMessage_ReturnsSuccessResultWithMessage()
    {
        // Arrange
        const string message = "Test success message";

        // Act
        var result = IResult.Success(message);

        // Assert
        Assert.True(result.IsSucceed);
        Assert.False(result.IsFailure);
        Assert.Equal(message, result.Message);
        Assert.Empty(result.Exceptions);
    }

    [Fact]
    public void Success_WithoutValue_HasEmptyExceptions()
    {
        // Arrange هیچ مقداری برای تعریف نیاز نیست زیرا متد خودش نتیجه موفقیت آمیز بدون مقدار نهایی
        // را برمی‌گرداند

        // Act
        var result = IResult.Success();

        // Assert
        Assert.Empty(result.Exceptions);
    }

    [Fact]
    public void Success_WithoutValue_HasNullMessage()
    {
        // Arrange هیچ مقداری برای تعریف نیاز نیست زیرا متد خودش نتیجه موفقیت آمیز بدون مقدار نهایی
        // را برمی‌گرداند

        // Act
        var result = IResult.Success();

        // Assert
        Assert.Null(result.Message);
    }

    [Fact]
    public void Success_WithoutValue_ReturnsSuccessResultWithoutValue()
    {
        // Arrange هیچ مقداری برای تعریف نیاز نیست زیرا متد خودش نتیجه موفقیت آمیز بدون مقدار نهایی
        // را برمی‌گرداند

        // Act
        var result = IResult.Success();

        // Assert
        Assert.True(result.IsSucceed);
        Assert.False(result.IsFailure);
    }

    [Fact]
    public void Success_WithValue_HasEmptyExceptions()
    {
        // Arrange
        const string value = "Test value";

        // Act
        var result = IResult.Success<string>(value);

        // Assert
        Assert.Empty(result.Exceptions);
    }

    [Fact]
    public void Success_WithValue_HasNullMessage()
    {
        // Arrange
        const string value = "Test value";

        // Act
        var result = IResult.Success<string>(value);

        // Assert
        Assert.Null(result.Message);
    }

    [Fact]
    public void Success_WithValue_ReturnsSuccessResultWithValue()
    {
        // Arrange
        const string value = "Test value";

        // Act
        var result = IResult.Success<string>(value);

        // Assert
        Assert.True(result.IsSucceed);
        Assert.False(result.IsFailure);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void Success_WithValueAndMessage_HasEmptyExceptions()
    {
        // Arrange
        const string value = "Test value";
        const string message = "Test success message";

        // Act
        var result = IResult.Success(value, message);

        // Assert
        Assert.Empty(result.Exceptions);
    }

    [Fact]
    public void Success_WithValueAndMessage_ReturnsSuccessResultWithMessage()
    {
        // Arrange
        const string value = "Test value";
        const string message = "Test success message";

        // Act
        var result = IResult.Success(value, message);

        // Assert
        Assert.Equal(message, result.Message);
    }

    [Fact]
    public void Success_WithValueAndMessage_ReturnsSuccessResultWithValue()
    {
        // Arrange
        const string value = "Test value";
        const string message = "Test success message";

        // Act
        var result = IResult.Success(value, message);

        // Assert
        Assert.True(result.IsSucceed);
        Assert.False(result.IsFailure);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void Success_WithValueAndMessage_ReturnsSuccessResultWithValueAndMessage()
    {
        // Arrange
        const string value = "Test value";
        const string message = "Test success message";

        // Act
        var result = IResult.Success(value, message);

        // Assert
        Assert.True(result.IsSucceed);
        Assert.False(result.IsFailure);
        Assert.Equal(message, result.Message);
        Assert.Empty(result.Exceptions);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void WithValue_WhenFailure_ReturnsResultWithValue()
    {
        // Arrange
        var originalResult = IResult.Fail<int?>(new Exception("Failure"));
        const string expectedValue = "AdditionalValue";

        // Act
        var resultWithValue = originalResult.WithValue(expectedValue);

        // Assert
        Assert.False(resultWithValue.IsSucceed);
        Assert.Equal(expectedValue, resultWithValue.Value);
    }

    [Fact]
    public void WithValue_WhenSuccess_ReturnsResultWithValue()
    {
        // Arrange
        var originalResult = IResult.Success<int?>(42);
        const string expectedValue = "AdditionalValue";

        // Act
        var resultWithValue = originalResult.WithValue(expectedValue);

        // Assert
        Assert.True(resultWithValue.IsSucceed);
        Assert.Equal(expectedValue, resultWithValue.Value);
    }

    public class CustomClass
    {
        public string Property { get; set; }
    }
}
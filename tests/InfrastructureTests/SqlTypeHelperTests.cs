namespace InfrastructureTests;

[Trait("Category", nameof(Moshaveran.Library.Helpers))]
[Trait("Category", nameof(SqlTypeHelper))]
public sealed class SqlTypeHelperTests
{
    [Fact]
    public void ToSqlFormat_ReturnsDbNullForDateTimeWhenDateIsDefault()
    {
        // Arrange
        var date = default(DateTime);

        // Act
        var result = date.ToSqlFormat();

        // Assert
        result.Should().Be(""); // Assuming GetDbNullForDateTime returns "NULL" for interpolation
    }

    [Fact]
    public void ToSqlFormat_ReturnsDbNullForDateTimeWhenDateIsDBNull()
    {
        // Arrange
        var date = default(DateTime);

        // Act
        var result = date.ToSqlFormat(false);

        // Assert
        result.Should().Be("NULL"); // Assuming GetDbNullForDateTime returns "" for non-interpolation
    }

    [Fact]
    public void ToSqlFormat_ReturnsDbNullForDateTimeWhenDateIsDBNullForInterpolation()
    {
        // Arrange
        var date = default(DateTime);

        // Act
        var result = date.ToSqlFormat();

        // Assert
        result.Should().Be(""); // Assuming GetDbNullForDateTime returns "NULL" for interpolation
    }

    [Fact]
    public void ToSqlFormat_ReturnsFormattedDateForInterpolation()
    {
        // Arrange
        var date = new DateTime(2023, 4, 1);

        // Act
        var result = date.ToSqlFormat();

        // Assert
        result.Should().Be("2023-04-01 00:00:00");
    }

    [Fact]
    public void ToSqlFormat_ReturnsFormattedDateWithQuotesForNonInterpolation()
    {
        // Arrange
        var date = new DateTime(2023, 4, 1);

        // Act
        var result = date.ToSqlFormat(false);

        // Assert
        result.Should().Be("'2023-04-01 00:00:00'");
    }

    [Fact]
    public void ToSqlFormat_Nullable_ReturnsDbNullForDateTimeWhenDateIsNull()
    {
        // Arrange
        DateTime? date = null;

        // Act
        var result = date.ToSqlFormat();

        // Assert
        result.Should().Be(""); // Assuming GetDbNullForDateTime returns "NULL" for interpolation
    }
    [Fact]
    public void ToSqlFormat_Nullable_NoneInterpolation_ReturnsDbNullForDateTimeWhenDateIsNull()
    {
        // Arrange
        DateTime? date = null;

        // Act
        var result = date.ToSqlFormat(false);

        // Assert
        result.Should().Be("NULL"); // Assuming GetDbNullForDateTime returns "NULL" for non-interpolation
    }

    [Fact]
    public void ToSqlFormat_Nullable_ReturnsDbNullForDateTimeWhenDateIsDBNull()
    {
        // Arrange
        DateTime? date = null;

        // Act
        var result = date.ToSqlFormat();

        // Assert
        result.Should().Be(""); // Assuming GetDbNullForDateTime returns "NULL" for interpolation
    }

    [Fact]
    public void ToSqlFormat_Nullable_NoneInterpolation_ReturnsDbNullForDateTimeWhenDateIsDBNull()
    {
        // Arrange
        DateTime? date = null;

        // Act
        var result = date.ToSqlFormat(false);

        // Assert
        result.Should().Be("NULL"); // Assuming GetDbNullForDateTime returns "NULL" for interpolation
    }
    
    [Fact]
    public void ToSqlFormat_Nullable_ReturnsFormattedDateForInterpolation()
    {
        // Arrange
        DateTime? date = new DateTime(2023, 4, 1);

        // Act
        var result = date.ToSqlFormat();

        // Assert
        result.Should().Be("2023-04-01 00:00:00");
    }

    [Fact]
    public void ToSqlFormat_Nullable_ReturnsFormattedDateWithQuotesForNonInterpolation()
    {
        // Arrange
        DateTime? date = new DateTime(2023, 4, 1);

        // Act
        var result = date.ToSqlFormat(false);

        // Assert
        result.Should().Be("'2023-04-01 00:00:00'");
    }
}

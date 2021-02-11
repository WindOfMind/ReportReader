using FluentAssertions;
using ReportReader.Domain.Columns;
using Xunit;

namespace ReportReader.Tests.Columns
{
    public class DateColumnUnitTests
    {
        [Fact]
        public void DateColumn_Parse_ValidValue_ShouldReturnItem()
        {
            // Arrange
            var sut = new DateColumn("DateColumn", false);

            // Act
            var result = sut.Parse("2014-01-01 12:30:00.000");

            // Assert
            result.IsSuccessful.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }

        [Theory]
        [InlineData("unknown date")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("NULL")]
        public void DateColumn_Parse_InvalidValue_ShouldReturnError(string date)
        {
            // Arrange
            var sut = new DateColumn("DateColumn", false);

            // Act
            var result = sut.Parse(date);

            // Assert
            result.IsSuccessful.Should().BeFalse();
            result.Error.Should().Be($"Failed to parse date {date} in the column DateColumn");
        }

        [Fact]
        public void NullableDateColumn_Parse_NullValue_ShouldReturnItem()
        {
            // Arrange
            var sut = new DateColumn("DateColumn", true);

            // Act
            var result = sut.Parse("NULL");

            // Assert
            result.IsSuccessful.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }
    }
}

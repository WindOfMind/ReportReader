using FluentAssertions;
using ReportReader.Domain.Columns;
using Xunit;

namespace ReportReader.Tests.Columns
{
    public class MoneyColumnUnitTests
    {
        [Fact]
        public void MoneyColumn_Parse_ValidValue_ShouldReturnItem()
        {
            // Arrange
            var sut = new MoneyColumn("MoneyColumn", false);

            // Act
            var result = sut.Parse("4880.199567");

            // Assert
            result.IsSuccessful.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }

        [Theory]
        [InlineData("unknown money")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("NULL")]
        public void MoneyColumn_Parse_InvalidValue_ShouldReturnError(string money)
        {
            // Arrange
            var sut = new MoneyColumn("MoneyColumn", false);

            // Act
            var result = sut.Parse(money);

            // Assert
            result.IsSuccessful.Should().BeFalse();
            result.Error.Should().Be($"Failed to parse money value {money} in the column MoneyColumn");
        }

        [Fact]
        public void NullableMoneyColumn_Parse_NullValue_ShouldReturnItem()
        {
            // Arrange
            var sut = new MoneyColumn("MoneyColumn", true);

            // Act
            var result = sut.Parse("NULL");

            // Assert
            result.IsSuccessful.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }
    }
}

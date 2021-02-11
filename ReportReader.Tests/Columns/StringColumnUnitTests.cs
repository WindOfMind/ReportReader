using FluentAssertions;
using ReportReader.Domain.Columns;
using Xunit;

namespace ReportReader.Tests.Columns
{
    public class StringColumnUnitTests
    {
        [Fact]
        public void StringColumn_Parse_ValidValue_ShouldReturnItem()
        {
            // Arrange
            var sut = new StringColumn("StringColumn", false);

            // Act
            var result = sut.Parse("Some text");

            // Assert
            result.IsSuccessful.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }

        [Theory]
        [InlineData("", "Value not found for column StringColumn")]
        [InlineData(null, "Value not found for column StringColumn")]
        [InlineData("NULL", "Value can not be NULL for column StringColumn")]
        public void StringColumn_Parse_InvalidValue_ShouldReturnError(string text, string error)
        {
            // Arrange
            var sut = new StringColumn("StringColumn", false);

            // Act
            var result = sut.Parse(text);

            // Assert
            result.IsSuccessful.Should().BeFalse();
            result.Error.Should().Be(error);
        }

        [Fact]
        public void NullableStringColumn_Parse_NullValue_ShouldReturnItem()
        {
            // Arrange
            var sut = new StringColumn("StringColumn", true);

            // Act
            var result = sut.Parse("NULL");

            // Assert
            result.IsSuccessful.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }
    }
}

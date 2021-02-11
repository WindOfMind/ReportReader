using System;
using System.Collections.Generic;
using FluentAssertions;
using ReportReader.Domain;
using ReportReader.Domain.Columns;
using Xunit;

namespace ReportReader.Tests
{
    public class HeaderUnitTests
    {
        [Fact]
        public void Header_FromText_ValidText_ShouldReturnHeader()
        {
            // Arrange
            var text = "Project	Description	Start date";
            var columns = new Dictionary<string, Column>
            {
                { "Project", new StringColumn("Project") },
                { "Description", new StringColumn("Description") },
                { "Start date", new DateColumn("Start date") }
            };

            // Act
            var sut = Header.FromString(text, columns, '\t');

            // Assert
            sut.IsSuccessful.Should().BeTrue();
            sut.Value.ToString().Should().Be("Project Description Start date");
        }

        [Fact]
        public void Header_FromText_InvalidText_ShouldReturnError()
        {
            // Arrange
            var text = "Description	Start date";
            var columns = new Dictionary<string, Column>
            {
                { "Project", new StringColumn("Project") },
                { "Description", new StringColumn("Description") },
                { "Start date", new DateColumn("Start date") }
            };

            // Act
            var sut = Header.FromString(text, columns, '\t');

            // Assert
            sut.IsSuccessful.Should().BeFalse();
            sut.Error.Should().Be("Column(s) Project not found");
        }

        [Fact]
        public void Header_FromText_NoText_ShouldReturnError()
        {
            // Arrange
            var text = "";
            var columns = new Dictionary<string, Column>
            {
                { "Project", new StringColumn("Project") },
                { "Description", new StringColumn("Description") },
                { "Start date", new DateColumn("Start date") }
            };

            // Act
            var sut = Header.FromString(text, columns, '\t');

            // Assert
            sut.IsSuccessful.Should().BeFalse();
            sut.Error.Should().Be("Column(s) Project, Description, Start date not found");
        }

        [Fact]
        public void Header_FromText_DifferentOrder_ShouldReturnHeader()
        {
            // Arrange
            var text = "Description	Start date	Project";
            var columns = new Dictionary<string, Column>
            {
                { "Project", new StringColumn("Project") },
                { "Description", new StringColumn("Description") },
                { "Start date", new DateColumn("Start date") }
            };

            // Act
            var sut = Header.FromString(text, columns, '\t');

            // Assert
            sut.IsSuccessful.Should().BeTrue();
            sut.Value.ToString().Should().Be("Description Start date Project");
        }

        [Fact]
        public void Header_FromText_Null_ShouldThrow()
        {
            // Arrange
            var columns = new Dictionary<string, Column>
            {
                { "Project", new StringColumn("Project") },
                { "Description", new StringColumn("Description") },
                { "Start date", new DateColumn("Start date") }
            };

            // Act
            Action action = () => Header.FromString(null, columns, '\t');

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Header_FromText_NoColumns_ShouldThrow()
        {
            // Arrange
            var text = "Description	Start date	Project";

            // Act
            Action action = () => Header.FromString(text, null, '\t');

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }
    }
}

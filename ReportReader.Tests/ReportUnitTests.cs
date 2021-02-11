using System;
using System.Collections.Generic;
using FluentAssertions;
using ReportReader.Domain;
using ReportReader.Domain.Common;
using Xunit;

namespace ReportReader.Tests
{
    public class ReportUnitTests
    {
        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[]
                {
                    // Valid
                    new [] { "Project	Description	Start date	Category	Responsible	Savings amount	Currency	Complexity",
                        "2	Harmonize Lactobacillus acidophilus sourcing	2014-01-01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Simple",
                        "6	Black and white logo paper	2012-06-01 00:00:00.000	Office supplies	Clark Kent	4880.199567	EUR	Simple" },

                    $"Project Description Start date Category Responsible Savings amount Currency Complexity{Environment.NewLine}" +
                    $"2 Harmonize Lactobacillus acidophilus sourcing 2014-01-01 00:00:00.000 Dairy Daisy Milks 11689.322459 EUR Simple{Environment.NewLine}" +
                    $"6 Black and white logo paper 2012-06-01 00:00:00.000 Office supplies Clark Kent 4880.199567 EUR Simple{Environment.NewLine}"
                },
                new object[]
                {
                    // With comments
                    new [] { "# This is comment",
                        "Project	Description	Start date	Category	Responsible	Savings amount	Currency	Complexity",
                        "# This is comment",
                        "2	Harmonize Lactobacillus acidophilus sourcing	2014-01-01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Simple",
                    },

                    $"Project Description Start date Category Responsible Savings amount Currency Complexity{Environment.NewLine}" +
                    $"2 Harmonize Lactobacillus acidophilus sourcing 2014-01-01 00:00:00.000 Dairy Daisy Milks 11689.322459 EUR Simple{Environment.NewLine}"
                },
                new object[]
                {
                    // With empty lines
                    new [] { " ", "Project	Description	Start date	Category	Responsible	Savings amount	Currency	Complexity", " ",
                        "2	Harmonize Lactobacillus acidophilus sourcing	2014-01-01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Simple" },

                    $"Project Description Start date Category Responsible Savings amount Currency Complexity{Environment.NewLine}" +
                    $"2 Harmonize Lactobacillus acidophilus sourcing 2014-01-01 00:00:00.000 Dairy Daisy Milks 11689.322459 EUR Simple{Environment.NewLine}"
                },
                new object[]
                {
                    // With empty body
                    new [] { "Project	Description	Start date	Category	Responsible	Savings amount	Currency	Complexity"},

                    $"Project Description Start date Category Responsible Savings amount Currency Complexity{Environment.NewLine}"
                }
            };

        [Theory]
        [MemberData(nameof(TestData))]
        public void ToString_ValidInput_ShouldReturnString(string[] text, string expected)
        {
            // Act
            Result<Report> reportResult = Report.FromText(text);

            // Assert
            reportResult.IsSuccessful.Should().BeTrue();
            reportResult.Value.ToString().Should().Be(expected);
        }

        [Fact]
        public void FromText_Null_ShouldThrow()
        {
            // Act
            Action action = () => Report.FromText(null);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void FilterByProjectId_ProjectId_ShouldReturnFiltered()
        {
            // Arrange
            var text = new[]
            {
                "Project	Description	Start date	Category	Responsible	Savings amount	Currency	Complexity",
                "2	Harmonize Lactobacillus acidophilus sourcing	2014-01-01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Simple",
                "2	Harmonize Lactobacillus acidophilus sourcing	2014-01-01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Simple",
                "3	Harmonize Lactobacillus acidophilus sourcing	2014-01-01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Simple",
                "6	Black and white logo paper	2012-06-01 00:00:00.000	Office supplies	Clark Kent	4880.199567	EUR	Simple",
                "6	Black and white logo paper	2012-06-01 00:00:00.000	Office supplies	Clark Kent	4880.199567	EUR	Simple"
            };

            string expected =
                $"Project Description Start date Category Responsible Savings amount Currency Complexity{Environment.NewLine}" +
                $"3 Harmonize Lactobacillus acidophilus sourcing 2014-01-01 00:00:00.000 Dairy Daisy Milks 11689.322459 EUR Simple{Environment.NewLine}";

            // Act
            Result<Report> reportResult = Report.FromText(text);
            var report = reportResult.Value;
            report.FilterByProject("3");

            // Assert
            report.ToString().Should().Be(expected);
        }

        [Fact]
        public void FilterByProjectId_EmptyProjectId_ShouldThrow()
        {
            // Arrange
            var text = new[]
            {
                "Project	Description	Start date	Category	Responsible	Savings amount	Currency	Complexity",
                "2	Harmonize Lactobacillus acidophilus sourcing	2014-01-01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Simple",
            };

            // Act
            Result<Report> reportResult = Report.FromText(text);
            var report = reportResult.Value;
            Action action = () => report.FilterByProject("");

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void SortByStartDateAsc_ShouldReturnSorted()
        {
            // Arrange
            var text = new[]
            {
                "Project	Description	Start date	Category	Responsible	Savings amount	Currency	Complexity",
                "2	Harmonize Lactobacillus acidophilus sourcing	2019-01-01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Simple",
                "2	Harmonize Lactobacillus acidophilus sourcing	2015-01-01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Simple",
                "3	Harmonize Lactobacillus acidophilus sourcing	2017-01-01 00:00:00.000	Dairy	Daisy Milks	11689.322459	EUR	Simple",
                "6	Black and white logo paper	2011-06-01 00:00:00.000	Office supplies	Clark Kent	4880.199567	EUR	Simple",
                "6	Black and white logo paper	2015-06-01 00:00:00.001	Office supplies	Clark Kent	4880.199567	EUR	Simple"
            };

            string expected =
                $"Project Description Start date Category Responsible Savings amount Currency Complexity{Environment.NewLine}" +
                $"6 Black and white logo paper 2011-06-01 00:00:00.000 Office supplies Clark Kent 4880.199567 EUR Simple{Environment.NewLine}" +
                $"2 Harmonize Lactobacillus acidophilus sourcing 2015-01-01 00:00:00.000 Dairy Daisy Milks 11689.322459 EUR Simple{Environment.NewLine}" +
                $"6 Black and white logo paper 2015-06-01 00:00:00.001 Office supplies Clark Kent 4880.199567 EUR Simple{Environment.NewLine}" +
                $"3 Harmonize Lactobacillus acidophilus sourcing 2017-01-01 00:00:00.000 Dairy Daisy Milks 11689.322459 EUR Simple{Environment.NewLine}" +
                $"2 Harmonize Lactobacillus acidophilus sourcing 2019-01-01 00:00:00.000 Dairy Daisy Milks 11689.322459 EUR Simple{Environment.NewLine}";

            // Act
            Result<Report> reportResult = Report.FromText(text);
            var report = reportResult.Value;
            report.SortByStartDateAsc();

            // Assert
            report.ToString().Should().Be(expected);
        }
    }
}

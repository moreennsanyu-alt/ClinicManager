using Xunit; // Changed from NUnit.Framework
using System;

// In xUnit, no [TestFixture] attribute is required on the class.
public class CalculatorTests
{
    // [Fact] replaces [Test] for a single test case
    [Fact]
    public void Add_GivenTwoNumbers_ReturnsSum()
    {
        // Arrange
        var calculator = new Calculator();
        int expectedSum = 5;

        // Act
        int actualSum = calculator.Add(2, 3);

        // Assert: xUnit uses Assert.Equal(expected, actual)
        Assert.Equal(expectedSum, actualSum); 
    }

    // [Theory] + [InlineData] replaces [TestCase]
    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(-4, 0, -4)]
    [InlineData(-5, -5, -10)]
    public void Add_GivenMultipleDataInputs_ReturnsCorrectSum(int x, int y, int expected)
    {
        // Arrange
        var calculator = new Calculator();

        // Act
        int actual = calculator.Add(x, y);

        // Assert
        Assert.Equal(expected, actual);
    }
}

public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }
}

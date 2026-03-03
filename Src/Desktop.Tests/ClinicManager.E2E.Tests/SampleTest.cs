using NUnit.Framework; // Changed from Xunit
using System;

// A public class to hold the tests. 
// xUnit creates a new instance of the test class for every test run.
public class CalculatorTests
{

// In NUnit, [TestFixture] is optional for modern versions, 
// but still widely used for clarity.
[TestFixture]
public class CalculatorTests
{
    // [Test] replaces [Fact]
    [Test]
    public void Add_GivenTwoNumbers_ReturnsSum()
    {
        // Arrange
        var calculator = new Calculator();
        int expectedSum = 5;

        // Act
        int actualSum = calculator.Add(2, 3);

        // Assert: NUnit prefers the constraint-based "Assert.That" syntax
        Assert.That(actualSum, Is.EqualTo(expectedSum)); 
    }

    // [TestCase] replaces the combination of [Theory] and [InlineData]
    [TestCase(1, 2, 3)]
    [TestCase(-4, 0, -4)]
    [TestCase(-5, -5, -10)]
    public void Add_GivenMultipleDataInputs_ReturnsCorrectSum(int x, int y, int expected)
    {
        var calculator = new Calculator();

        int actual = calculator.Add(x, y);

        Assert.That(actual, Is.EqualTo(expected));
    }
}

public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }
}

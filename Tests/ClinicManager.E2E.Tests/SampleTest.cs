using Xunit; // Required using statement
using System;

// A public class to hold the tests. 
// xUnit creates a new instance of the test class for every test run.
public class CalculatorTests
{
    // The [Fact] attribute marks a method as a test to be run by the test runner.
    [Fact]
    public void Add_GivenTwoNumbers_ReturnsSum()
    {
        // Arrange: Set up the test environment and objects.
        var calculator = new Calculator();
        int expectedSum = 5;

        // Act: Execute the code under test.
        int actualSum = calculator.Add(2, 3);

        // Assert: Verify that the result is what you expect.
        Assert.Equal(expectedSum, actualSum); 
    }

    // You can also use the [Theory] attribute for tests that have parameters 
    // and run multiple times with different data.
    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(-4, 0, -4)]
    [InlineData(-5, -5, -10)]
    public void Add_GivenMultipleDataInputs_ReturnsCorrectSum(int x, int y, int expected)
    {
        var calculator = new Calculator();

        int actual = calculator.Add(x, y);

        Assert.Equal(expected, actual);
    }
}

// A simple example class to be tested (System Under Test).
public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }
}

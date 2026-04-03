using Xunit;

namespace ClinicManager.E2E.Tests;

public class CalculatorTests
{
    // [Fact] is used for tests that are always true (no parameters)
    [Fact]
    public void Add_TwoNumbers_ReturnsSum()
    {
        // 1. Arrange: Set up your objects and data
        var calculator = new Calculator();
        int a = 5;
        int b = 10;

        // 2. Act: Execute the method being tested
        int result = calculator.Add(a, b);

        // 3. Assert: Verify the result is what you expect
        Assert.Equal(15, result);
    }

    // [Theory] is used for data-driven tests with multiple inputs
    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(-1, 1, 0)]
    [InlineData(0, 0, 0)]
    public void Add_MultipleValues_ReturnsExpectedSum(int a, int b, int expected)
    {
        var calculator = new Calculator();
        
        int result = calculator.Add(a, b);
        
        Assert.Equal(expected, result);
    }
}
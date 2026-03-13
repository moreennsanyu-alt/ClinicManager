using NUnit.Framework;

namespace Calculator.Tests
{
    // [TestFixture] is optional in NUnit 3+ unless using generic or parameterized tests
    public class CalculatorTests
    {
        private Calculator _sut; // System Under Test

        [SetUp]
        public void Setup()
        {
            // This method runs before each test method.
            _sut = new Calculator();
        }

        [Test]
        public void ShouldAddTwoNumbers()
        {
            // Arrange
            int expectedResult = 15;

            // Act
            int actualResult = _sut.Add(7, 8);

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(1, true)]  // Example of using TestCase attribute for data-driven tests
        [TestCase(0, false)]
        [TestCase(-1, false)]
        public void IsPositive_VariousInputs(int value, bool expected)
        {
            // Act
            bool actual = _sut.IsPositive(value);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TearDown]
        public void Teardown()
        {
            // This method runs after each test method to clean up resources.
            _sut = null;
        }

}
public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }

    public bool IsPositive(int number)
    {
        return number > 0;
    }

}
}

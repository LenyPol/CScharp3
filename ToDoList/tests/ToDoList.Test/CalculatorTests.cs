namespace ToDoList.Test;

public class CalculatorTests
{
    [Fact]
    public void Calculator_Add_SouldReturnCorrectResult()
    {
        var calculator = new Calculator();

        var result = calculator.Add(2, 3);

        Assert.Equal(5, result);
    }
    public void Calculator_Divide_ThrowDivisionByZeroException()
    {
        var calculator = new Calculator();

        Assert.Throws<DivideByZeroException>(() => calculator.Divide(6, 0));
    }
}

public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }
    public int Divide(int a, int b)
    {
        if (b == 0)
        {
            throw new DivideByZeroException("Cannot divide by zer.");
        }
        return a / b;
    }
}

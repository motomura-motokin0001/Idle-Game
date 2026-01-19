public class Test_Num 
{
        public string FormatNumber(double number)
    {
        if (number < 1.0) return number.ToString("F3");

        if (number < 1000000) return number.ToString("F2");
        return number.ToString("0.00e0");
    }
}

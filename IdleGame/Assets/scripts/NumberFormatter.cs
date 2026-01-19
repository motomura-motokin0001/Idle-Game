using System;
using System.Collections.Generic;

public enum NotationType
{
    Standard,      // 通常 (1,000,000)
    Scientific,    // 科学的 (1.00e6)
    Engineering,   // エンジニアリング (1.00E+6 ※指数は3の倍数)
    Alphabet       // アルファベット (1.00M)
}

public static class NumberFormatter
{
    // アルファベット表記用の単位リスト
    private static readonly string[] AlphabetUnits = 
    { 
        "", "k", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc" 
    };

    /// <summary>
    /// 数値を指定した形式の文字列に変換します
    /// </summary>
    public static string FormatNumber(this double number, NotationType type)
    {
        if (double.IsInfinity(number)) return "Infinity";
        if (double.IsNaN(number)) return "NaN";
        if (Math.Abs(number) < 0.001 && number != 0) return number.ToString("E2");
        if (Math.Abs(number) < 1.0) return number.ToString("F3");

        switch (type)
        {
            case NotationType.Standard:
                return number < 1000000 ? number.ToString("N2") : number.ToString("F0");

            case NotationType.Scientific:
                return number < 1000000 ? number.ToString("N2") : number.ToString("0.00e0");

            case NotationType.Engineering:
                return GetEngineeringNotation(number);

            case NotationType.Alphabet:
                return GetAlphabetNotation(number);

            default:
                return number.ToString();
        }
    }

    // エンジニアリング表記の実装 (指数を3の倍数にする)
    private static string GetEngineeringNotation(double number)
    {
        double absValue = Math.Abs(number);
        int exponent = (int)Math.Floor(Math.Log10(absValue) / 3) * 3;
        double mantissa = number / Math.Pow(10, exponent);

        // 1000を超えないように調整 (例: 1000E+0 -> 1.00E+3)
        if (Math.Abs(mantissa) >= 1000.0)
        {
            mantissa /= 1000.0;
            exponent += 3;
        }

        return string.Format("{0:F2}E{1:+0;-0;+0}", mantissa, exponent);
    }

    // アルファベット表記の実装 (k, M, B, T...)
    private static string GetAlphabetNotation(double number)
    {
        double absValue = Math.Abs(number);
        if (absValue < 1000) return number.ToString("F2");

        int unitIndex = (int)Math.Floor(Math.Log10(absValue) / 3);
        
        if (unitIndex < AlphabetUnits.Length)
        {
            double shortValue = number / Math.Pow(10, unitIndex * 3);
            return string.Format("{0:F2}{1}", shortValue, AlphabetUnits[unitIndex]);
        }
        else
        {
            // 対応する単位がないほど巨大な場合はエンジニアリング表記にフォールバック
            return GetEngineeringNotation(number);
        }
    }
}
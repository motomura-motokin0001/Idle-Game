using System;
using System.Globalization;

// 放置ゲームで一般的な非常に大きな数値を扱うための静的ヘルパークラス
public static class Notation
{
    // C#では、放置ゲームの巨大な数値を扱うためにdouble（約1.7e308まで）を使用します。
    // それを超える場合は、専用のBigIntegerクラスやDecimal構造体が必要になりますが、
    // まずはdoubleでフェーズIの目標(1e308)までを目指します。

    /// <summary>
    /// 数値を指数表記（例: 1.23e15）にフォーマットします。
    /// </summary>
    /// <param name="value">フォーマットする数値。</param>
    /// <param name="decimalPlaces">小数点以下の桁数。</param>
    /// <returns>指数表記の文字列。</returns>
    public static string FormatExponential(double value, int decimalPlaces = 2)
    {
        if (double.IsInfinity(value)) return "Infinity";
        if (value < 1000) return value.ToString($"F{decimalPlaces}", CultureInfo.InvariantCulture);

        // 指数表記に変換 (例: 1.23456E+15)
        string eNotation = value.ToString($"E{decimalPlaces}", CultureInfo.InvariantCulture);

        // Eをeに、+を削除して一般的な放置ゲーム表記にする (例: 1.23e15)
        return eNotation.Replace('E', 'e').Replace("+", "");
    }
}
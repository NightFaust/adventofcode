using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventofcode.Utils;

public static class Utilities
{
    /// <summary>
    /// Splits the input into columns, this is sometimes nice for maps drawing. 
    /// Automatically expands to a full rectangle iff needed based on max length and number of rows. 
    /// Empty cells are denoted as ' ' (Space character)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string[] SplitIntoColumns(this string input)
    {
        var rows = input.SplitByNewline(false, false);
        int numColumns = rows.Max(x=> x.Length);

        var res = new string[numColumns];
        for (var i = 0; i < numColumns; i++)
        {
            StringBuilder sb = new();
            foreach (var row in rows)
            {
                try
                {
                    sb.Append(row[i]);
                }
                catch (IndexOutOfRangeException)
                {
                    sb.Append(' ');
                }
            }
            res[i] = sb.ToString();
        }
        return res;
    }
    
    public static List<string> SplitByNewline(this string input, bool blankLines = false, bool shouldTrim = true)
    {
        return input
            .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
            .Where(s => blankLines || !string.IsNullOrWhiteSpace(s))
            .Select(s => shouldTrim ? s.Trim() : s)
            .ToList();
    }
}
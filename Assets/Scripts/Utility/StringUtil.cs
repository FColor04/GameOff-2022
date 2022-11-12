using System.Text.RegularExpressions;

public static class StringUtil
{
    public static string FieldNameToLabelText(string fieldName) => SplitCamelCase(CapitalizeFirst(fieldName));

    public static string CapitalizeFirst(string s)
    {
        return char.ToUpper(s[0]) + s.Substring(1);
    }

    public static string SplitCamelCase(string camelCase)
    {
        return Regex.Replace(
        Regex.Replace(
            camelCase,
            @"(\P{Ll})(\P{Ll}\p{Ll})",
            "$1 $2"
        ),
        @"(\p{Ll})(\P{Ll})",
        "$1 $2");
    }
}
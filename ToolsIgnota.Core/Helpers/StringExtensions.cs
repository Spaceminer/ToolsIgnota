namespace ToolsIgnota.Core.Helpers;

public static class StringExtensions
{
    public static string Initials(this string input)
    {
        var sanitized = input.Replace(new char[] { '\\', '\'', '-', '(', ')', '"' }, "");
        var words = sanitized.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var initials = words.Take(2).Select(x => x?[..1] ?? "");
        return string.Join("", initials.Select(x => x.ToUpper()));
    }

    public static string Replace(this string input, char[] oldValues, string newValue)
    {
        return string.Join(
            newValue,
            input.Split(oldValues, StringSplitOptions.RemoveEmptyEntries));
    }
}

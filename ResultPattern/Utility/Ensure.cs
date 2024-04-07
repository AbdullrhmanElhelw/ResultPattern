namespace ResultPattern.Utility;

public static class Ensure
{
    public static void NotEmpty(string value, string message, string argumentName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(message, argumentName);
    }

    public static void NotNull<T>(T value, string message, string argumentName)
    {
        if (value == null)
            throw new ArgumentNullException(message, argumentName);
    }
}
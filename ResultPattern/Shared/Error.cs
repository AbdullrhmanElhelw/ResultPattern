namespace ResultPattern.Shared;

public sealed record Error
    (string Message)
{
    public static readonly Error None = new(string.Empty);
    public static implicit operator Error(string message) => new(message);
}
namespace Chat.Dal.Exceptions;

public class CustomDbUpdateException : Exception{
    public CustomDbUpdateException() { }
    public CustomDbUpdateException(string message) : base(message) { }
    public CustomDbUpdateException(
        string message, DbUpdateException innerException)
        : base(message, innerException) { }
}
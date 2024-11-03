namespace Chat.Dal.Exceptions;

public class CustomRetryLimitExceededException: Exception {
    public CustomRetryLimitExceededException() { }
    public CustomRetryLimitExceededException(string message)
        : base(message) { }
    public CustomRetryLimitExceededException(
        string message, RetryLimitExceededException innerException)
        : base(message, innerException) { }
}
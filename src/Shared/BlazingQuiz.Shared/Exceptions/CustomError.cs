using BlazingQuiz.Shared.Exceptions.Messages;

namespace BlazingQuiz.Shared.Exceptions;
public sealed record CustomError(string Code, string? Message = null, List<string>? Messages = null)
{
    private static readonly string NotFoundCode = ErrorCodes.NotFound;
    private static readonly string ConflictCode = ErrorCodes.Conflict;
    private static readonly string ErrorOnValidationCode = ErrorCodes.ErrorOnValidation;
    private static readonly string InvalidCredentialCode = ErrorCodes.InvalidCredential;
    private static readonly string UnauthorizedCode = ErrorCodes.Unauthorized;

    public static readonly CustomError None = new(string.Empty, string.Empty, new List<string>());

    public static CustomError NotFound(string message)
    {
        return new CustomError(NotFoundCode, message);
    }

    public static CustomError Conflict(string message)
    {
        return new CustomError(ConflictCode, message);
    }

    public static CustomError InvalidCredential()
    {
        return new CustomError(InvalidCredentialCode, Message: ResourceErrorMessages.INVALID_CREDENTIALS);
    }

    public static CustomError ErrorOnValidation(List<string>? messages)
    {
        return new CustomError(ErrorOnValidationCode, Messages: messages);
    }

    public static CustomError Unauthorized()
    {
        return new CustomError(UnauthorizedCode);
    }
}

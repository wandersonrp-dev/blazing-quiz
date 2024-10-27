namespace BlazingQuiz.Shared.DTOs;
public record ResponseError
{
    public List<string> ErrorMessages { get; set; }

    public ResponseError(List<string> errorMessages)
    {
        ErrorMessages = errorMessages;
    }

    public ResponseError(string errorMessage)
    {
        ErrorMessages = [errorMessage];
    }
}

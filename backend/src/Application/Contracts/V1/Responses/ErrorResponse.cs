namespace Application.Contracts.V1.Responses;

public class ErrorResponse
{
    public List<ErrorModel> Errors { get; set; } = new();
}
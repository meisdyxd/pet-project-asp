namespace DirectoryService.Contracts.Errors;

public class Error(
    string message,
    string code,
    string? invalidField = null)
{
    public string Message { get; set; } = message;
    public string Code { get; set; } = code;
    public string? InvalidField { get; set; } = invalidField;
}
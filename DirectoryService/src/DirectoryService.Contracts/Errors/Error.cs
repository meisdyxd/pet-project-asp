namespace DirectoryService.Contracts;

public class Error(
    string message,
    string code)
{
    public string Message { get; set; } = message;
    public string Code { get; set; } = code;
}
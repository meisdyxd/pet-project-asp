using DirectoryService.Contracts;

namespace DirectoryService.Domain;

public class Envelope
{
    public object? Result { get; set; }
    public IEnumerable<Error>? Errors { get; set; }
    public DateTime TimeGenerated { get; set; }

    public static Envelope Success(object result)
    {
        var envelope = new Envelope()
        {
            Result = result,
            Errors = null,
            TimeGenerated = DateTime.UtcNow
        };
        
        return envelope;
    }
    
    public static Envelope Failure(IEnumerable<Error> errors)
    {
        var envelope = new Envelope()
        {
            Result = null,
            Errors = errors,
            TimeGenerated = DateTime.UtcNow
        };
        
        return envelope;
    }
}
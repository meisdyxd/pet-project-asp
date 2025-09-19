using System.Net;
using CSharpFunctionalExtensions;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Errors;

namespace DirectoryService.Domain.ValueObjects.Position;

public class Description: ValueObject
{
    public string? Value { get; }

    private Description(string? value)
    {
        Value = value;
    }

    public static Result<Description, ErrorList> Create(string? value)
    {
        var errors = new List<Error>();
        
        if (value is not null 
            && value.Length > Constants.PositionConstants.MAX_LENGTH_DESCRIPTION)
            errors.Add(Errors.InvalidValue.IncorrectLength(
                nameof(Description).ToLower(),
                maxLength: Constants.PositionConstants.MAX_LENGTH_DESCRIPTION));
        
        if (errors.Count != 0)
            return new ErrorList(errors, HttpStatusCode.BadRequest);
        
        return new Description(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

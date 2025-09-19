using System.Net;
using CSharpFunctionalExtensions;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Errors;

namespace DirectoryService.Domain.ValueObjects.Position;

public class Name : ValueObject
{
    public string Value { get; }

    private Name(string value)
    {
        Value = value;
    }

    public static Result<Name, ErrorList> Create(string value)
    {
        var errors = new List<Error>();
        
        if (string.IsNullOrWhiteSpace(value))
            errors.Add(Errors.InvalidValue.Empty(nameof(Name).ToLower()));

        if (value.Length
            is < Constants.PositionConstants.MIN_LENGTH_NAME
            or > Constants.PositionConstants.MAX_LENGTH_NAME)
            errors.Add(Errors.InvalidValue.IncorrectLength(
                nameof(Name).ToLower(),
                Constants.PositionConstants.MIN_LENGTH_NAME,
                Constants.PositionConstants.MAX_LENGTH_NAME));

        if (errors.Count != 0)
            return new ErrorList(errors, HttpStatusCode.BadRequest);
        
        return new Name(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

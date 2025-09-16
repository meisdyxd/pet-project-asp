using System.Net;
using CSharpFunctionalExtensions;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Errors;

namespace DirectoryService.Domain.ValueObjects.Department;

public class Identifier : ValueObject
{
    public string Value { get; }

    private Identifier(string value)
    {
        Value = value;
    }

    public static Result<Identifier, ErrorList> Create(string value)
    {
        var errors = new List<Error>();
        
        if (string.IsNullOrWhiteSpace(value))
            errors.Add(Errors.InvalidValue.Empty(nameof(Identifier).ToLower()));
        
        if (value.Length
            is < Constants.DepartmentConstants.MIN_LENGTH_IDENTIFIER
            or > Constants.DepartmentConstants.MAX_LENGTH_IDENTIFIER)
            errors.Add(Errors.InvalidValue.IncorrectLength(
                nameof(Identifier).ToLower(),
                Constants.DepartmentConstants.MIN_LENGTH_IDENTIFIER,
                Constants.DepartmentConstants.MAX_LENGTH_IDENTIFIER));
        
        if (errors.Count != 0)
            return new ErrorList(errors, HttpStatusCode.BadRequest);
        
        return new Identifier(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
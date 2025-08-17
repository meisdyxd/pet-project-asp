using CSharpFunctionalExtensions;
using DirectoryService.Contracts;

namespace DirectoryService.Domain.ValueObjects;

public record Name
{
    public string Value { get; } = null!;

    private Name(string value)
    {
        Value = value;
    }

    public static Result<Name, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.InvalidValue.Empty(nameof(Name).ToLower());
        
        if (value.Length 
            is < Constants.DepartmentConstants.MIN_LENGTH_NAME 
            or > Constants.DepartmentConstants.MAX_LENGTH_NAME) 
            return Errors.InvalidValue.IncorrectLength(
                nameof(Name).ToLower(), 
                Constants.DepartmentConstants.MIN_LENGTH_NAME, 
                Constants.DepartmentConstants.MAX_LENGTH_NAME);
        
        return new Name(value);
    }
}
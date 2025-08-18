using CSharpFunctionalExtensions;
using DirectoryService.Contracts;

namespace DirectoryService.Domain.ValueObjects.Department;

public class Identifier : ValueObject
{
    public string Value { get; } = null!;

    private Identifier(string value)
    {
        Value = value;
    }

    public static Result<Identifier, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.InvalidValue.Empty(nameof(Identifier).ToLower());

        if (value.Length
            is < Constants.DepartmentConstants.MIN_LENGTH_IDENTIFIER
            or > Constants.DepartmentConstants.MAX_LENGTH_IDENTIFIER)
            return Errors.InvalidValue.IncorrectLength(
                nameof(Identifier).ToLower(),
                Constants.DepartmentConstants.MIN_LENGTH_IDENTIFIER,
                Constants.DepartmentConstants.MAX_LENGTH_IDENTIFIER);

        return new Identifier(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
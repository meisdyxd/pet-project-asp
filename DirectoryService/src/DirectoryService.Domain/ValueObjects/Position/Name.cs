using CSharpFunctionalExtensions;
using DirectoryService.Contracts;

namespace DirectoryService.Domain.ValueObjects.Position;

public class Name : ValueObject
{
    public string Value { get; }

    private Name(string value)
    {
        Value = value;
    }

    public static Result<Name, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.InvalidValue.Empty(nameof(Name).ToLower());

        if (value.Length
            is < Constants.PositionConstants.MIN_LENGTH_NAME
            or > Constants.PositionConstants.MAX_LENGTH_NAME)
            return Errors.InvalidValue.IncorrectLength(
                nameof(Name).ToLower(),
                Constants.PositionConstants.MIN_LENGTH_NAME,
                Constants.PositionConstants.MAX_LENGTH_NAME);

        return new Name(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

using CSharpFunctionalExtensions;
using DirectoryService.Contracts;

namespace DirectoryService.Domain.ValueObjects.Position;

public class Description: ValueObject
{
    public string? Value { get; }

    private Description(string? value)
    {
        Value = value;
    }

    public static Result<Description, Error> Create(string? value)
    {
        if (value is not null 
            && value.Length > Constants.PositionConstants.MAX_LENGTH_DESCRIPTION)
            return Errors.InvalidValue.IncorrectLength(
                nameof(Description).ToLower(),
                maxLength: Constants.PositionConstants.MAX_LENGTH_DESCRIPTION);

        return new Description(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

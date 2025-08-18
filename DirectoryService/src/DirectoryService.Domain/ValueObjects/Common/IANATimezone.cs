using CSharpFunctionalExtensions;
using DirectoryService.Contracts;
using System.Text.RegularExpressions;

namespace DirectoryService.Domain.ValueObjects.Common;

public class IANATimezone : ValueObject
{
    public string Value { get; }

    private IANATimezone(string value)
    {
        Value = value;
    }

    public static Result<IANATimezone, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.InvalidValue.Empty(nameof(IANATimezone).ToLower());

        if (!Regex.IsMatch(value, Constants.CommonConstants.REGEX_IANA))
            return Errors.InvalidValue.Default(nameof(IANATimezone).ToLower());

        return new IANATimezone(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

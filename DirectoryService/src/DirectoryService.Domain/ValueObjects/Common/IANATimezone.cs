using System.Net;
using CSharpFunctionalExtensions;
using DirectoryService.Contracts;
using System.Text.RegularExpressions;
using DirectoryService.Contracts.Errors;

namespace DirectoryService.Domain.ValueObjects.Common;

public class IANATimezone : ValueObject
{
    public string Value { get; }

    private IANATimezone(string value)
    {
        Value = value;
    }

    public static Result<IANATimezone, ErrorList> Create(string value)
    {
        var errors = new List<Error>();
        
        if (string.IsNullOrWhiteSpace(value))
            errors.Add(Errors.InvalidValue.Empty(nameof(IANATimezone).ToLower()));

        if (!Regex.IsMatch(value, Constants.CommonConstants.REGEX_IANA))
            errors.Add(Errors.InvalidValue.Default(nameof(IANATimezone).ToLower()));
        
        if (errors.Count != 0)
            return new ErrorList(errors, HttpStatusCode.BadRequest);
        
        return new IANATimezone(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

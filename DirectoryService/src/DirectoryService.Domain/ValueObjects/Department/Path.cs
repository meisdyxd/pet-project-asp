using CSharpFunctionalExtensions;
using DirectoryService.Contracts;
using System.Text.RegularExpressions;
using DirectoryService.Contracts.Errors;

namespace DirectoryService.Domain.ValueObjects.Department;

public class Path : ValueObject
{
    public string Value { get; }

    private Path(string value)
    {
        Value = value;
    }

    public static Result<Path, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.InvalidValue.Empty(nameof(Path).ToLower());

        if (!Regex.IsMatch(value, Constants.DepartmentConstants.REGEX_PATH))
            return Errors.InvalidValue.Default(nameof(Path).ToLower());

        return new Path(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
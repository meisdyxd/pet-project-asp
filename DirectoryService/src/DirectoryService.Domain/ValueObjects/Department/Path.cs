using CSharpFunctionalExtensions;
using DirectoryService.Contracts;
using System.Text.RegularExpressions;
using DirectoryService.Contracts.Errors;

namespace DirectoryService.Domain.ValueObjects.Department;

public class Path : ValueObject
{
    private const char SEPARATOR = '.';
    public string Value { get; }

    private Path(string value)
    {
        Value = value;
    }

    public static Result<Path, Error> CreateParent(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            return Errors.InvalidValue.Empty(nameof(Path).ToLower());

        if (!Regex.IsMatch(identifier, Constants.DepartmentConstants.REGEX_PATH))
            return Errors.InvalidValue.Default(nameof(Path).ToLower());

        return new Path(identifier);
    }

    public Result<Path, Error> CreateChild(Identifier childIdentifier)
    {
        return new Path(Value + SEPARATOR + childIdentifier);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
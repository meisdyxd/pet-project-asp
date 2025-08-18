using CSharpFunctionalExtensions;
using DirectoryService.Contracts;

namespace DirectoryService.Domain.ValueObjects.Department;

public class Depth : ValueObject
{
    public short Value { get; }

    private Depth(short value)
    {
        Value = value;
    }

    public static Result<Depth, Error> Create(short value)
    {
        if (value < Constants.DepartmentConstants.MIN_DEPTH)
            return Errors.InvalidValue.Greater(nameof(Depth), Constants.DepartmentConstants.MIN_DEPTH);

        return new Depth(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

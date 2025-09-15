using System.Net;
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

    public static Result<Depth, ErrorList> Create(short value)
    {
        var errors = new List<Error>();
        
        if (value < Constants.DepartmentConstants.MIN_DEPTH)
            errors.Add(Errors.InvalidValue.Greater(nameof(Depth), Constants.DepartmentConstants.MIN_DEPTH));

        if (errors.Count != 0)
            return new ErrorList(errors, HttpStatusCode.BadRequest);
        
        return new Depth(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

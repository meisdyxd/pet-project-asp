using System.Text.Json.Serialization;

namespace DirectoryService.Contracts.Dtos.Departments;

public sealed record GetWithTopPositionsDto
{
    public GetWithTopPositionsDto(
        Guid id,
        string name,
        string identifier,
        DateTime createdAt,
        long amountPositions)
    {
        Id = id;
        Name = name;
        Identifier = identifier;
        CreatedAt = createdAt;
        AmountPositions = amountPositions;
    }
    
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Identifier { get; init; }
    public DateTime CreatedAt { get; init; }
    public long AmountPositions { get; init; }
}
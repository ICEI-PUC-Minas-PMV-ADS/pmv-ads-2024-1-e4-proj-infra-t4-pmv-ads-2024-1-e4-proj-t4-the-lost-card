namespace Domain.Entities;

public abstract class Entity
{
    protected virtual string ProtectedPartitionKey { get; set; } = default!;
    public string PartitionKey { get => ProtectedPartitionKey; set => ProtectedPartitionKey = value; }

    public Guid? Id { get; set; }
}

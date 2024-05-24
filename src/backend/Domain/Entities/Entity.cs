using Newtonsoft.Json;

namespace Domain.Entities;

public abstract class Entity
{
    protected virtual string ProtectedPartitionKey { get; set; } = default!;
    public string PartitionKey { get => ProtectedPartitionKey; set => ProtectedPartitionKey = value; }

    [JsonProperty("id")]
    public Guid? Id { get; set; }

    //[JsonProperty("id")]
    //public string JsonId { get => Id!.Value.ToString(); set => Id = Guid.Parse(value); }
}

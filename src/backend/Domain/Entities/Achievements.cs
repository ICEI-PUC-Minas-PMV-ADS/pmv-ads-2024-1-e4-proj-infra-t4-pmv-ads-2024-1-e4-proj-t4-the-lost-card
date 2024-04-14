namespace Domain.Entities;

public class Achievements : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Descriptions { get; set; } = string.Empty;
    public IEnumerable<Player> Players { get; set; } = Enumerable.Empty<Player>();
}

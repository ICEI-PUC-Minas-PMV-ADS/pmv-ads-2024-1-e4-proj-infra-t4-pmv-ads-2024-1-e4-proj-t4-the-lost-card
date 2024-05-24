namespace Domain.Entities;

public partial class Player
{
    public class AchievmentInfo
    {
        public long AchievmentKey { get; set; }
        public DateTime UnlockedAt { get; set; }
    }
}

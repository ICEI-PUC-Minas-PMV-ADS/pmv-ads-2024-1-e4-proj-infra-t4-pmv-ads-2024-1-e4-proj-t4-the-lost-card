namespace Domain.Entities;

public partial class Player
{
    public class AchievmentInfo
    {
        public int AchievmentKey { get; set; }
        public DateTime UnlockedAt { get; set; }
    }
}

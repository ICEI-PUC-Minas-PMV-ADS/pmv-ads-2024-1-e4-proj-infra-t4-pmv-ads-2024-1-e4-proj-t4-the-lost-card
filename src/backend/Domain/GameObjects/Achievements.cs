namespace Domain.GameObjects;

public class Achievements
{
    public static Dictionary<int, Achievment> AchievmentsDictionary { get; set; } = new()
    {
        {
            1, new Achievment
            {
                Name = "Join a room",
                Descriptions = "Join a game room for the first time"
            }
        },
        {
            2, new Achievment
            {
                Name = "Finish a game",
                Descriptions = "Reach the end of game, winning or losing"
            }
        },
    };


    public class Achievment
    {
        public string Name { get; set; } = string.Empty;
        public string Descriptions { get; set; } = string.Empty;
    }
}

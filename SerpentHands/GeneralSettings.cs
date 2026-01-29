namespace SerpentHands;

public class GeneralSettings
{
    public byte MinPlayers { get; set; } = 5;
    public byte MaxPlayers { get; set; } = 8;
    public byte SpawnChance { get; set; } = 40;
    public byte SpawnLimit { get; set; } = 3;
}

public class CassieAnnouncement
{
    public string Announcement { get; set; }
    public string Subtitle { get; set; }
}
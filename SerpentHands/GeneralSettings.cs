namespace SerpentHands;

public class GeneralSettings
{
    public byte MinPlayers { get; set; } = 5;
    public byte MaxPlayers { get; set; } = 8;
    public byte SpawnChance { get; set; } = 35;
    public byte SpawnLimit { get; set; } = 1;
}

public class CassieAnnouncement
{
    public string Announcement { get; set; }
    public string Subtitle { get; set; }
}
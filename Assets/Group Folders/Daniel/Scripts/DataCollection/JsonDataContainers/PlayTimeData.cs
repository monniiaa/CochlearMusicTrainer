using System;

[Serializable]
public class PlayTimeData : AbstractDataContainer
{
    public override string Path => "\\PlayTime\\play_time_data.json";

    public string minigame;
    public string time;
    public string timePlayed;

    public PlayTimeData(string minigame, DateTime time, TimeSpan timePlayed)
    {
        this.minigame = minigame;
        this.time = time.ToString("dd/MM/yyyy HH:mm:ss");
        this.timePlayed = timePlayed.ToString("h'h 'm'm 's's'");
    }
}

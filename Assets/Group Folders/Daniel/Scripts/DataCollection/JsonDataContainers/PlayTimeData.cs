using System;

[Serializable]
public class PlayTimeData : AbstractDataContainer
{
    public override string Path => "\\PlayTime\\play_time_data.json";

    public string minigame;
    public string startTime;
    public string endTime;

    public PlayTimeData(string minigame, string startTime, string endTime)
    {
        this.minigame = minigame;
        this.startTime = startTime;
        this.endTime = endTime;
    }
}

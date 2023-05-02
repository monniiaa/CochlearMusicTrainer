using System;

[Serializable]
public class ScoreData : AbstractDataContainer
{
    public override string Path => "\\ScoreData\\score_data.json";

    public string minigame;
    public string time;
    public int score;
    public int level;

    public ScoreData(string minigame, DateTime time, int score, int level)
    {
        this.minigame = minigame;
        this.time = time.ToString("dd/MM/yyyy HH:mm:ss");
        this.score = score;
        this.level = level;
    }
    
}

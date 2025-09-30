using System;

public class LeaderboardData : IComparable
{
    public float Score { get; private set; }

    public LeaderboardData(float score)
    {
        Score = score;
    }

    public int CompareTo(object obj)
    {
        LeaderboardData data = obj as LeaderboardData;
        if (data.Score > Score)
            return 1;
        else if (data.Score < Score)
            return -1;
        return 0;
    }
}
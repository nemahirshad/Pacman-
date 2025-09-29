using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class LeaderboardDisplay : MonoBehaviour
{
    public static LeaderboardDisplay instance;

    [Header("TMP Objects")]
    [SerializeField] TMP_Text scorePrefab; // Prefab to instantiate for each score
    //[SerializeField] TMP_Text rankPrefab;  // Prefab to instantiate for each rank
    [SerializeField] Transform scoresParent; // Parent object for score text
    //[SerializeField] Transform ranksParent;  // Parent object for rank text

    [Space]
    List<LeaderboardData> data = new();
    [Space]
    string databasePath;
    [SerializeField] float playerScore; // Keep for the last submitted score


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ReadFromCSV();
    }

    public void AddToLeaderboard(float score)
    {
        data.Add(new LeaderboardData(score));

        playerScore = score;

        Save();
        SortData();
    }

    void SortData()
    {
        data.Sort();
        if (data.Count > 5)
        {
            data.RemoveRange(5, data.Count - 5);
        }
        UpdateBoard();
    }

    void ClearBoard()
    {
        foreach (Transform child in scoresParent)
        {
            Destroy(child.gameObject);
        }
        /*foreach (Transform child in ranksParent)
        {
            Destroy(child.gameObject);
        }*/
    }

    void UpdateBoard()
    {
        ClearBoard();

        for (int i = 0; i < data.Count; i++)
        {
            /*TMP_Text rankText = Instantiate(rankPrefab, ranksParent);
            rankText.text = (i + 1).ToString();*/

            TMP_Text scoreText = Instantiate(scorePrefab, scoresParent);
            scoreText.text = data[i].Score.ToString();
        }
    }

    void ReadFromCSV()
    {
        databasePath = $"{Application.streamingAssetsPath}/database.csv";
        if (!File.Exists(databasePath))
        {
            Debug.LogWarning("CSV file not found. Creating a new one when a score is saved.");
            return;
        }

        string[] lines = File.ReadAllLines(databasePath);
        if (lines.Length == 0)
            return;

        foreach (string line in lines)
        {
            if (float.TryParse(line.Trim(), out float score))
            {
                LeaderboardData parsedData = new LeaderboardData(score);
                data.Add(parsedData);
            }
        }
        SortData();
    }

    public void Save()
    {
        databasePath = $"{Application.streamingAssetsPath}/database.csv";
        LeaderboardData newScoreData = new LeaderboardData(playerScore);

        List<LeaderboardData> combinedData = new List<LeaderboardData>(data);
        combinedData.Add(newScoreData);
        combinedData.Sort(); // Sort all scores

        List<string> linesToWrite = new List<string>();
        int limit = Mathf.Min(combinedData.Count, 5); // Take min of count or 5

        for (int i = 0; i < limit; i++)
        {
            linesToWrite.Add(combinedData[i].Score.ToString());
        }

        File.WriteAllLines(databasePath, linesToWrite);

    }
}
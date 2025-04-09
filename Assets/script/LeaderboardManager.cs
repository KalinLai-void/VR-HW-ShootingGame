using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ScoreRecord
{
    public string playerName;
    public int score;
}

[System.Serializable]
public class ScoreRecordList
{
    public List<ScoreRecord> records = new List<ScoreRecord>();
}

public class LeaderboardManager : MonoBehaviour
{
    private static string rankFilePath => Application.persistentDataPath + "/leaderboard.json";

    private void Awake()
    {
        InitializeLeaderboard();
       

        Debug.Log(" 實際排行榜檔案位置：" + Application.persistentDataPath + "/leaderboard.json");
       
    }

    /// <summary>
    /// 初始化排行榜：若無排行榜檔案則建立一個空的
    /// </summary>
    public static void InitializeLeaderboard()
    {
        if (!File.Exists(rankFilePath))
        {
            ScoreRecordList emptyList = new ScoreRecordList();
            string json = JsonUtility.ToJson(emptyList, true);
            File.WriteAllText(rankFilePath, json);
            Debug.Log("初始化空排行榜資料。");
        }
        else
        {
            Debug.Log("排行榜資料已存在，略過初始化。");
            string existingJson = File.ReadAllText(rankFilePath);
            Debug.Log("當前排行榜 JSON 內容：\n" + existingJson);
        }
    }

    public static void SaveScore(string playerName, int score)
    {
        ScoreRecordList scoreList = new ScoreRecordList();

        if (File.Exists(rankFilePath))
        {
            string json = File.ReadAllText(rankFilePath);
            try
            {
                scoreList = JsonUtility.FromJson<ScoreRecordList>(json) ?? new ScoreRecordList();
            }
            catch
            {
                Debug.LogWarning("排行榜資料損壞，自動重新建立。");
                scoreList = new ScoreRecordList();
            }
        }

        scoreList.records.Add(new ScoreRecord { playerName = playerName, score = score });
        scoreList.records.Sort((a, b) => b.score.CompareTo(a.score)); // 高分在前

        string newJson = JsonUtility.ToJson(scoreList, true);
        File.WriteAllText(rankFilePath, newJson);

        Debug.Log("排行榜更新成功！");
    }

    public static List<ScoreRecord> LoadTopScores(int count = 10)
    {
        if (!File.Exists(rankFilePath))
        {
            Debug.LogWarning("排行榜資料不存在！");
            return new List<ScoreRecord>();
        }

        string json = File.ReadAllText(rankFilePath);
        Debug.Log("載入排行榜 JSON：\n" + json); // ← 這行可幫助確認

        ScoreRecordList scoreList = JsonUtility.FromJson<ScoreRecordList>(json);

        if (scoreList == null || scoreList.records == null)
        {
            Debug.LogWarning("解析排行榜資料失敗！");
            return new List<ScoreRecord>();
        }

        Debug.Log($"成功解析排行榜，共 {scoreList.records.Count} 筆紀錄");

        return scoreList.records.GetRange(0, Mathf.Min(count, scoreList.records.Count));
    }

}


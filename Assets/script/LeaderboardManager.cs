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
       

        Debug.Log(" ��ڱƦ�]�ɮצ�m�G" + Application.persistentDataPath + "/leaderboard.json");
       
    }

    /// <summary>
    /// ��l�ƱƦ�]�G�Y�L�Ʀ�]�ɮ׫h�إߤ@�ӪŪ�
    /// </summary>
    public static void InitializeLeaderboard()
    {
        if (!File.Exists(rankFilePath))
        {
            ScoreRecordList emptyList = new ScoreRecordList();
            string json = JsonUtility.ToJson(emptyList, true);
            File.WriteAllText(rankFilePath, json);
            Debug.Log("��l�ƪűƦ�]��ơC");
        }
        else
        {
            Debug.Log("�Ʀ�]��Ƥw�s�b�A���L��l�ơC");
            string existingJson = File.ReadAllText(rankFilePath);
            Debug.Log("��e�Ʀ�] JSON ���e�G\n" + existingJson);
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
                Debug.LogWarning("�Ʀ�]��Ʒl�a�A�۰ʭ��s�إߡC");
                scoreList = new ScoreRecordList();
            }
        }

        scoreList.records.Add(new ScoreRecord { playerName = playerName, score = score });
        scoreList.records.Sort((a, b) => b.score.CompareTo(a.score)); // �����b�e

        string newJson = JsonUtility.ToJson(scoreList, true);
        File.WriteAllText(rankFilePath, newJson);

        Debug.Log("�Ʀ�]��s���\�I");
    }

    public static List<ScoreRecord> LoadTopScores(int count = 10)
    {
        if (!File.Exists(rankFilePath))
        {
            Debug.LogWarning("�Ʀ�]��Ƥ��s�b�I");
            return new List<ScoreRecord>();
        }

        string json = File.ReadAllText(rankFilePath);
        Debug.Log("���J�Ʀ�] JSON�G\n" + json); // �� �o��i���U�T�{

        ScoreRecordList scoreList = JsonUtility.FromJson<ScoreRecordList>(json);

        if (scoreList == null || scoreList.records == null)
        {
            Debug.LogWarning("�ѪR�Ʀ�]��ƥ��ѡI");
            return new List<ScoreRecord>();
        }

        Debug.Log($"���\�ѪR�Ʀ�]�A�@ {scoreList.records.Count} ������");

        return scoreList.records.GetRange(0, Mathf.Min(count, scoreList.records.Count));
    }

}


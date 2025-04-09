using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;



public class MenuUIManager : MonoBehaviour
{
    [Header("UI 元件")]
    public Button startGameButton;
    public Button exitButton;
    public Button ShowLeaderboardButton;
    public Button CloseLeaderboardButton;
    public Transform leaderboardContainer;
    public GameObject leaderboardItemPrefab;
    //public InputField nameInputField;
    public TMP_InputField nameInputField;
    public GameObject vrKeyboard;
  
    void Start()
    {
        startGameButton.onClick.AddListener(OnStartGame);
        exitButton.onClick.AddListener(OnExitGame);
        ShowLeaderboardButton.onClick.AddListener(OnShowLeaderboard);
        CloseLeaderboardButton.onClick.AddListener(CloseLeaderboard);

        // ✅ 當點擊名稱欄位時，自動彈出系統鍵盤（Meta Quest 支援）
        if (nameInputField != null)
        {
            nameInputField.onSelect.AddListener(OnNameInputFieldSelected);
        }
        else
        {
            Debug.Log("ℹ️ 本場景未設置 nameInputField，略過鍵盤註冊");
        }


    }

    void OnNameInputFieldSelected(string text)
    {
        Debug.Log("玩家點擊名稱輸入框，呼叫虛擬鍵盤");

        // ✅ 開啟系統虛擬鍵盤
        vrKeyboard.SetActive(true);
        //vrKeyboardvrKeyboard = TouchScreenKeyboard.Open(nameInputField.text, TouchScreenKeyboardType.Default);

        // 📝 可選：更新文字（如果需要及時同步）
        // StartCoroutine(UpdateInputFieldText());
    }

    void OnStartGame()
    {
        string inputName = nameInputField.text.Trim();

        if (string.IsNullOrEmpty(inputName))
        {
            inputName = "PLAYER1"; // ✅ 預設名稱
            Debug.Log("未輸入名稱，自動設定為：" + inputName);
        }

        GameManager.Instance.SetPlayerName(inputName);
        GameManager.Instance.StartGame();
    }

    void OnExitGame()
    {
        Application.Quit();
        Debug.Log("離開遊戲");
    }

    void OnShowLeaderboard()
    {
        ShowLeaderboard();
        leaderboardContainer.gameObject.SetActive(true);
        Debug.Log("開啟排行榜");
    }

    void CloseLeaderboard()
    {
        leaderboardContainer.gameObject.SetActive(false);
        Debug.Log("關閉排行榜");
    }

    void Update()
    {
        if (GameManager.Instance != null && nameInputField != null)
        {
            nameInputField.text = GameManager.Instance.textBox.text;
        }
    }
    void ShowLeaderboard()
    {
        List<ScoreRecord> topScores = LeaderboardManager.LoadTopScores(10);

        for (int i = 0; i < topScores.Count; i++)
        {
            Debug.Log($"[排行榜資料] 第 {i + 1} 名：{topScores[i].playerName} - {topScores[i].score}");
        }

        foreach (Transform child in leaderboardContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < topScores.Count; i++)
        {
            GameObject item = Instantiate(leaderboardItemPrefab, leaderboardContainer);
            item.gameObject.SetActive(true);

            Transform rankObj = item.transform.Find("Rank");
            Transform nameObj = item.transform.Find("Name");
            Transform pointObj = item.transform.Find("Point");

            if (rankObj == null || nameObj == null || pointObj == null)
            {
                Debug.LogWarning($"[Prefab結構錯誤] 缺少 Rank/Name/Point 子物件");
                continue;
            }

            Text rankText = rankObj.GetComponent<Text>();
            Text nameText = nameObj.GetComponent<Text>();
            Text pointText = pointObj.GetComponent<Text>();

            Debug.Log($"[排行榜生成] Rank:{i + 1}, Name:{topScores[i].playerName}, Score:{topScores[i].score}");

            if (rankText) rankText.text = (i + 1).ToString();
            if (nameText) nameText.text = topScores[i].playerName;
            if (pointText) pointText.text = topScores[i].score.ToString();
        }
    }
}



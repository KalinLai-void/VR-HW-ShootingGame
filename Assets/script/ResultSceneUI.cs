using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ResultSceneUI : MonoBehaviour
{
    [Header("UI 元件")]
    public Text finalScoreText;
    public InputField playerNameInput;
    public Button submitButton;
    public Transform leaderboardContainer;
    public GameObject leaderboardItemPrefab;
    public Button playAgainButton;
    public Button returnToMenuButton;

    private bool hasSubmitted = false;

    void Start()
    {
        finalScoreText.text = "Your Score: " + GameManager.Instance.finalScore;

        submitButton.onClick.AddListener(OnSubmitScore);
        playAgainButton.onClick.AddListener(GameManager.Instance.PlayAgain);
        returnToMenuButton.onClick.AddListener(GameManager.Instance.GoToMenu);

        // 先載入排行榜（不顯示新紀錄前）
        ShowLeaderboard();
    }

    void OnSubmitScore()
    {
        if (hasSubmitted || string.IsNullOrWhiteSpace(playerNameInput.text)) return;

        LeaderboardManager.SaveScore(playerNameInput.text, GameManager.Instance.finalScore);
        hasSubmitted = true;
        ShowLeaderboard();
    }

    void ShowLeaderboard()
    {
        foreach (Transform child in leaderboardContainer)
        {
            Destroy(child.gameObject);
        }

        List<ScoreRecord> topScores = LeaderboardManager.LoadTopScores(10);
        for (int i = 0; i < topScores.Count; i++)
        {
            GameObject item = Instantiate(leaderboardItemPrefab, leaderboardContainer);
            item.GetComponent<Text>().text = $"{i + 1}. {topScores[i].playerName} - {topScores[i].score}";
        }
    }
}

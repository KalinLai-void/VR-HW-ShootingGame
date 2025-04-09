using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIController : MonoBehaviour
{
    [Header("UI 元件")]
    public GameObject gameOverPanel;
    public Text finalScoreText;
    public InputField playerNameInput;
    public Button submitButton;
    public Transform leaderboardContainer;
    public GameObject leaderboardItemPrefab;
    public Button playAgainButton;
    public Button returnToMenuButton;

    private bool hasSubmitted = false;

    private void Start()
    {
        // 預設隱藏結算畫面
        gameOverPanel.SetActive(false);

        submitButton.onClick.AddListener(OnSubmitScore);
        playAgainButton.onClick.AddListener(OnPlayAgain);
        returnToMenuButton.onClick.AddListener(OnReturnToMenu);
    }

    public void ShowGameOverUI(int finalScore)
    {
        gameOverPanel.SetActive(true);
        hasSubmitted = false;

        finalScoreText.text = "Your Score: " + finalScore;
        playerNameInput.text = "";
        ClearLeaderboard();
        ShowLeaderboard(); // 顯示原有前10名
    }

    void OnSubmitScore()
    {
        if (hasSubmitted || string.IsNullOrWhiteSpace(playerNameInput.text)) return;

        LeaderboardManager.SaveScore(playerNameInput.text, GameManager.Instance.finalScore);
        hasSubmitted = true;
        ClearLeaderboard();
        ShowLeaderboard();
    }

    void ShowLeaderboard()
    {
        List<ScoreRecord> topScores = LeaderboardManager.LoadTopScores(10);
        foreach (ScoreRecord record in topScores)
        {
            GameObject item = Instantiate(leaderboardItemPrefab, leaderboardContainer);
            item.GetComponent<Text>().text = $"{record.playerName} - {record.score}";
        }
    }

    void ClearLeaderboard()
    {
        foreach (Transform child in leaderboardContainer)
        {
            Destroy(child.gameObject);
        }
    }

    void OnPlayAgain()
    {
        GameManager.Instance.PlayAgain();
    }

    void OnReturnToMenu()
    {
        GameManager.Instance.GoToMenu();
    }
}

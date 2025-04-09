using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ResultSceneUI : MonoBehaviour
{
    [Header("UI ����")]
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

        // �����J�Ʀ�]�]����ܷs�����e�^
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

using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public TextMeshProUGUI textBox;
    public TextMeshProUGUI printBox;

    public int finalScore;
    public int startScore = 0;
    public int startAmmo = 10;
    public float maxTime = 30f;

    public string playerName;

    public Action OnGameReset; // ✅ 供 GameScene 監聽「再玩一次」事件

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Instance = this;
        printBox.text = "";
        textBox.text = "";
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
        Debug.Log("✅ 設定玩家名稱為：" + playerName);
    }

    public void StartGame()
    {
        finalScore = 0;
        SceneManager.LoadScene("GameScene"); // 初次進入場景
        //PlayAgain();
    }

    public void EndGame(int score)
    {
        finalScore = score;
        Debug.Log($"✅ 遊戲結束，總分為 {score}");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayAgain()
    {
        // 重設初始值
       startScore = 0;
       startAmmo = 10;
       maxTime = 30f;
       finalScore = 0;
        

    }

    public void DeleteLetter()
    {
        if (textBox.text.Length != 0)
        {
            textBox.text = textBox.text.Remove(textBox.text.Length - 1, 1);
        }
    }

    public void AddLetter(string letter)
    {
        textBox.text = textBox.text + letter;
    }

    public void SubmitWord()
    {
        printBox.text = textBox.text;
        textBox.text = "";
        // Debug.Log("Text submitted successfully!");
    }

}
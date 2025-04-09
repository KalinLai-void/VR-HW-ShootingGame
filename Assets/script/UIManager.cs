
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("TMP UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI scoreText1;
    [Header("Time Fill UI")]
    public Image timerFillImage;

    private Coroutine countdownCoroutine;

    public GameObject Result;
    public void InitializeGameUI(int startingScore, int startingAmmo, float maxTime)
    {
        UpdateScore(startingScore);
        UpdateAmmo(startingAmmo, startingAmmo);
        UpdateTimerFill(maxTime, maxTime);
        HideCountdown();
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
            scoreText1.text = "Score: " + score;
    }

    public void UpdateAmmo(int current, int max)
    {
        if (ammoText != null)
            ammoText.text = $"Ammo: {current}/{max}";
        
    }

    public void UpdateTimerFill(float currentTime, float maxTime)
    {
        if (timerFillImage != null)
            timerFillImage.fillAmount = Mathf.Clamp01(currentTime / maxTime);
    }

    public void StartCountdown(float seconds, System.Action onComplete)
    {
        if (countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);

        countdownCoroutine = StartCoroutine(CountdownRoutine(seconds, onComplete));
        Result.SetActive(false);

    }

    private IEnumerator CountdownRoutine(float seconds, System.Action onComplete)
    {
        countdownText.gameObject.SetActive(true);

        for (int i = Mathf.CeilToInt(seconds); i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(0.5f);

        countdownText.gameObject.SetActive(false);
        onComplete?.Invoke();
    }

    public void HideCountdown()
    {
        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
    }
}


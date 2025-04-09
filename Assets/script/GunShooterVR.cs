using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GunShooterVR : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    [Header("射擊控制")]
    public SimpleShoot simpleShoot;
    private XRGrabInteractable grab;

    [Header("音效設定")]
    public AudioSource audioSource;
    public AudioClip shootClip;
    public AudioClip shootClipStart;
    public AudioClip shootClipEnd;

    [Header("遊戲設定")]
    public float roundTime = 30f;
    public int scorePerHit = 10;
    public float timeBonusFactor = 2f;

    private float timer;
    private int hitCount;
    private int totalScore;
    private bool isPlaying;
    private bool isHeld;

    private UIManager uiManager;
    private LeaderboardManager leaderboardManager;
    private InputAction triggerAction;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        if (grab != null)
        {
            grab.selectEntered.AddListener(OnPickedUp);
            grab.selectExited.AddListener(OnDropped);
        }

        // 初始化左右手 Trigger 綁定
        triggerAction = new InputAction("Trigger");
        triggerAction.AddBinding("<XRController>{LeftHand}/trigger");
        triggerAction.AddBinding("<XRController>{RightHand}/trigger");
    }

    void OnEnable()
    {
        triggerAction.Enable();
    }

    void OnDisable()
    {
        triggerAction.Disable();
    }

    void Start()
    {
        uiManager = GameObject.Find("UIManager")?.GetComponent<UIManager>();

        if (uiManager == null)
        {
            Debug.LogWarning("❗ 找不到 UIManager，請確認場景中有名稱為 UIManager 的物件！");
        }
        // ✅ 記住槍的原始位置與角度
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        StartNewRound();
    }

    void Update()
    {
        if (!isPlaying) return;

        // 倒數時間更新
        timer -= Time.deltaTime;
        uiManager?.UpdateTimerFill(timer, roundTime);

        if (timer <= 0)
        {
            Debug.Log("🕒 時間結束！");
            EndRound();
            return;
        }

        // 滑鼠或 VR 扳機觸發射擊
        //bool isMouseTrigger = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1);
        bool isVRTrigger = triggerAction != null && triggerAction.WasPressedThisFrame();
        //if (isHeld && (isMouseTrigger || isVRTrigger))
            if (isHeld && ( isVRTrigger))
        {
            TryShoot();
        }
    }

    void TryShoot()
    {
        if (simpleShoot.currentAmmo <= 0)
        {
            Debug.Log("❌ 沒有子彈了！");
            return;
        }

        simpleShoot.currentAmmo--;

        simpleShoot.Shoot();

        if (shootClip != null && audioSource != null)
            audioSource.PlayOneShot(shootClip);

        hitCount++; // ✅ 可選：統計射擊次數
        totalScore += 10; // ✅ 每次射擊固定加 10 分

        Debug.Log($"✅ 成功射擊！+10分，總分：{totalScore}，剩餘子彈：{simpleShoot.currentAmmo}");

        uiManager?.UpdateAmmo(simpleShoot.currentAmmo, simpleShoot.maxAmmo);
        uiManager?.UpdateScore(totalScore); // ✅ 更新 UI 分數

        // ✅ 子彈射完立即結束回合
        if (simpleShoot.currentAmmo == 0)
        {
            Debug.Log("🛑 子彈耗盡，提前結束回合！");
            EndRound();
            audioSource.PlayOneShot(shootClipEnd);
        }
    }


    public void RegisterHit()
    {
        if (!isPlaying) return;

        hitCount++;
        totalScore += scorePerHit;

        Debug.Log($"🎯 擊中目標！總擊中數：{hitCount}，總分：{totalScore}");

        uiManager?.UpdateScore(totalScore);
    }

    public void StartNewRound()
    {
        // 重設槍的位置與角度
        transform.SetPositionAndRotation(initialPosition, initialRotation);
        audioSource.PlayOneShot(shootClipStart);
        // 解除持有（讓它掉落）
        /*  if (grab != null && grab.isSelected)
          {
              grab.interactionManager.CancelInteractableSelection(grab);
          }*/
        timer = roundTime;
        hitCount = 0;
        totalScore = 0;
        isPlaying = false;

        simpleShoot.currentAmmo = simpleShoot.maxAmmo;

        uiManager?.InitializeGameUI(0, simpleShoot.maxAmmo, roundTime);

        uiManager?.StartCountdown(3f, () =>
        {
            isPlaying = true;
            Debug.Log("✅ 回合正式開始！");
        });

        Debug.Log("🔄 準備開始新回合（倒數中）");
    }
 
    private void EndRound()
    {
        isPlaying = false;

        float timeBonus = timer * timeBonusFactor;
        int finalScore = totalScore + Mathf.RoundToInt(timeBonus);
        uiManager.UpdateScore(finalScore);

        Debug.Log($"🛑 回合結束！擊中：{hitCount} 次，時間加分：{Mathf.RoundToInt(timeBonus)}，總分：{finalScore}");

        // ✅ 寫入排行榜
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EndGame(finalScore);

            string playerName = string.IsNullOrEmpty(GameManager.Instance.playerName)
                ? "PLAYER1"
                : GameManager.Instance.playerName;

            LeaderboardManager.SaveScore(playerName, finalScore);
            Debug.Log($" 已將 {playerName} 的成績 {finalScore} 存入排行榜！");
        }

        // ✅ 顯示結算 UI
        if (uiManager?.Result != null)
            uiManager.Result.SetActive(true);
    }
    private void OnPickedUp(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    private void OnDropped(SelectExitEventArgs args)
    {
        isHeld = false;
    }
}

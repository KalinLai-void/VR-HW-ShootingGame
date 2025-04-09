using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("目標設定")]
    public int scoreValue = 10;
    public GameObject hitEffectPrefab; // 可選：命中特效

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // 播放命中特效
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            }

            // 呼叫分數累計
            GunShooterVR shooter = FindObjectOfType<GunShooterVR>();
            if (shooter != null)
            {
                shooter.RegisterHit();
            }

            // 自毀或重設（視設計）
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("�ؼг]�w")]
    public int scoreValue = 10;
    public GameObject hitEffectPrefab; // �i��G�R���S��

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // ����R���S��
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            }

            // �I�s���Ʋ֭p
            GunShooterVR shooter = FindObjectOfType<GunShooterVR>();
            if (shooter != null)
            {
                shooter.RegisterHit();
            }

            // �۷��έ��]�]���]�p�^
            Destroy(gameObject);
        }
    }
}

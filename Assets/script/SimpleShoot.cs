using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShoot : MonoBehaviour
{
    [Header("子彈設定")]
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletSpeed = 20f;
    public float bulletLifetime = 3f;

    [Header("彈藥設定")]
    public int maxAmmo = 10;
    [HideInInspector] public int currentAmmo; // 給 GunShooterVR 控制

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    public void Shoot()
    {
        if (bulletPrefab == null || shootPoint == null)
        {
            Debug.LogWarning("❗ 缺少 bulletPrefab 或 shootPoint 設定！");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // ✅ 用 AddForce 推進子彈，而非直接 velocity
            rb.AddForce(shootPoint.forward * bulletSpeed, ForceMode.Impulse);
        }
        else
        {
            Debug.LogWarning("❗ 子彈物件上缺少 Rigidbody 元件！");
        }

        Destroy(bullet, bulletLifetime);
    }
}

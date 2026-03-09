using UnityEngine;

public class TurretEnemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Attack Settings")]
    [SerializeField] private float timeBetweenShots = 0.2f;
    [SerializeField] private int bulletsPerBurst = 3;
    [SerializeField] private float burstCooldown = 2f;

    private float timer;
    private int shotsFired;
    private bool isBursting;

    void Update()
    {
        if (player == null) return;

        LookAtPlayer();
        HandleShooting();
    }

    void LookAtPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.up = direction;
    }

    void HandleShooting()
    {
        timer -= Time.deltaTime;

        if (isBursting)
        {
            if (timer <= 0)
            {
                Shoot();

                shotsFired++;

                if (shotsFired >= bulletsPerBurst)
                {
                    isBursting = false;
                    timer = Random.Range(burstCooldown * 0.8f, burstCooldown *1.2f);
                    shotsFired = 0;
                }
                else
                {
                    timer = timeBetweenShots;
                }
            }
        }
        else
        {
            if (timer <= 0)
            {
                isBursting = true;
                timer = 0;
            }
        }
    }

    void Shoot()
    {
        GameObject bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = firePoint.position;

        Vector2 direction = (player.position - firePoint.position).normalized;
        bullet.GetComponent<EnemyBullet>().Initialize(direction);
    }
}

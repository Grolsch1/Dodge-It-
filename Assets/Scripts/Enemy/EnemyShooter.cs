using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform firePoint;

    [Header("Attack Settings")]
    [SerializeField] private float timeBetweenShots = 0.2f;
    [SerializeField] private int bulletsPerBurst = 3;
    [SerializeField] private float burstCooldown = 2f;
    [SerializeField] private float spreadAngle = 5f;

    private Enemy enemy;

    private float timer;
    private int shotsFired;
    private bool isBursting;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        if (enemy.Player == null) return;

        HandleShooting();
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
                    timer = Random.Range(burstCooldown * 0.8f, burstCooldown * 1.2f);
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
        EnemyBullet bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = firePoint.position;

        Vector2 dir = (enemy.Player.position - firePoint.position).normalized;

        // add spread
        float spread = Random.Range(-spreadAngle, spreadAngle);
        dir = Quaternion.Euler(0, 0, spread) * dir;

        bullet.Initialize(dir);
    }
}
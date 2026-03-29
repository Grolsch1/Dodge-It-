using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 50;

    private Queue<EnemyBullet> pool = new Queue<EnemyBullet>();

    void Awake()
    {
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);

            pool.Enqueue(obj.GetComponent<EnemyBullet>());
        }
    }

    public EnemyBullet GetBullet()
    {
        if (pool.Count == 0)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj.GetComponent<EnemyBullet>());
        }

        EnemyBullet bullet = pool.Dequeue();
        bullet.gameObject.SetActive(true);

        return bullet;
    }

    public void ReturnBullet(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(false);
        pool.Enqueue(bullet);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 40;

    private Dictionary<EnemyBullet, Queue<EnemyBullet>> pools = new Dictionary<EnemyBullet, Queue<EnemyBullet>>();

    void Awake()
    {
        Instance = this;
    }

    public EnemyBullet GetBullet(EnemyBullet prefab)
    {
        if (!pools.ContainsKey(prefab))
        {
            pools[prefab] = new Queue<EnemyBullet>();

            for (int i = 0; i < poolSize; i++)
            {
                EnemyBullet obj = Instantiate(prefab);
                obj.PrefabReference = prefab;
                obj.gameObject.SetActive(false);
                pools[prefab].Enqueue(obj);
            }
        }

        var pool = pools[prefab];

        if (pool.Count == 0)
        {
            EnemyBullet obj = Instantiate(prefab);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }

        EnemyBullet bullet = pool.Dequeue();
        bullet.gameObject.SetActive(true);

        return bullet;
    }

    public void ReturnBullet(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(false);

        if (bullet.PrefabReference != null && pools.ContainsKey(bullet.PrefabReference))
        {
            pools[bullet.PrefabReference].Enqueue(bullet);
        }
        else
        {
            Destroy(bullet.gameObject); // fallback safety
        }
    }
}

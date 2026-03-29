using UnityEngine;

public class EnemyLookAtPlayer : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        if (enemy.Player == null) return;

        Vector2 dir = (enemy.Player.position - transform.position).normalized;
        transform.up = dir;
    }
}
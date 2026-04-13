using UnityEngine;

public class HealPack : MonoBehaviour
{
    [SerializeField] private int healAmount = 25;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.Heal(healAmount);
            }

            HealPackManager.instance.OnHealPackCollected(this);
            gameObject.SetActive(false);
        }
    }
}
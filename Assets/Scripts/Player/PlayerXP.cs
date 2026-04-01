using UnityEngine;
using System;

public class PlayerXP : MonoBehaviour
{
    public static PlayerXP instance;

    [Header("XP")]
    public int currentXP = 0;
    public int level = 1;
    public int requiredXP = 100;

    [Header("Scaling")]
    public float xpMultiplier = 1.5f;

    public Action onLevelUp;
    [SerializeField] private PlayerHUD playerHUD;

    private void Awake()
    {
        instance = this;
    }

    public void AddXP(int amount)
    {
        currentXP += amount;

        while (currentXP >= requiredXP)
        {
            LevelUp();
        } 
        playerHUD.UpdateXP(currentXP, requiredXP, level);
    }

    void LevelUp()
    {
        level++;
        currentXP -= requiredXP;
        requiredXP = Mathf.RoundToInt(requiredXP * xpMultiplier);

        onLevelUp?.Invoke();

        //Debug.Log("Leveled Up! Current Level: " + level);

        playerHUD.UpdateXP(currentXP, requiredXP, level);
    }
}

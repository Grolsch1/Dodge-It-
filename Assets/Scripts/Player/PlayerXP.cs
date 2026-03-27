using UnityEngine;
using System;

public class PlayerXP : MonoBehaviour
{
    public static PlayerXP instance;

    [Header("XP")]
    public int currentXP = 0;
    public int currentLevel = 1;
    public int xpToNextLevel = 100;

    [Header("Scaling")]
    public float xpMultiplier = 1.5f;

    public Action onLevelUp;

    private void Awake()
    {
        instance = this;
    }

    public void AddXP(int amount)
    {
        currentXP += amount;

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }   
    }

    void LevelUp()
    {
        currentLevel++;
        currentXP -= xpToNextLevel;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * xpMultiplier);

        onLevelUp?.Invoke();

        Debug.Log("Leveled Up! Current Level: " + currentLevel);
    }
}

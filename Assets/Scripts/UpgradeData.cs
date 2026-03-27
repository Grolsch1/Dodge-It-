using UnityEngine;

public enum UpgradeType
{
    Health,
    Damage,
    Speed
}

[System.Serializable]
public class UpgradeOption
{
    public UpgradeType type;
    public string displayName;
}
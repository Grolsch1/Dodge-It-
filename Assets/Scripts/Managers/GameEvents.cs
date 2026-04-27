using System;
using UnityEngine;

public static class GameEvents
{
    public static Action OnGameStart;
    public static Action<bool> OnPause;
    public static Action OnPlayerDeath;
    public static Action<int> OnKillUpdated;
    public static Action<int> OnVictory;
    public static Func<bool> CanPauseCheck;
    public static Action OnGameReset;
    public static System.Action<int> OnWaveUpdated;
    public static System.Action<Sprite> OnShowCutscene;
    public static System.Action OnHideCutscene;
}
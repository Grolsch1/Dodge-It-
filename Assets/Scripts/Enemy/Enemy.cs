using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform Player { get; private set; }

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }
}
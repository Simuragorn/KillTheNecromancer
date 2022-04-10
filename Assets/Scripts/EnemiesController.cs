using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    public static EnemiesController Instance { private set; get; }
    public Dictionary<GameObject, Enemy> Enemies { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        Enemies = new Dictionary<GameObject, Enemy>();
    }
}

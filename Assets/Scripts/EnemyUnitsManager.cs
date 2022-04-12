using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitsManager : MonoBehaviour
{
    public static EnemyUnitsManager Instance { private set; get; }
    public Dictionary<GameObject, UnitController> Enemies { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        Enemies = new Dictionary<GameObject, UnitController>();
    }
}


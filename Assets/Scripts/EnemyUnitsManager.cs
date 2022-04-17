using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyUnitsManager : MonoBehaviour
{
    public static EnemyUnitsManager Instance { private set; get; }
    public Dictionary<GameObject, UnitController> Enemies { get; set; }
    public List<UnitsCamp> EnemiesCamps { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        Enemies = new Dictionary<GameObject, UnitController>();
        EnemiesCamps = new List<UnitsCamp>();
    }

    public void RemoveUnit(GameObject unit)
    {
        if (Enemies.TryGetValue(unit, out UnitController controller))
        {
            Enemies.Remove(unit);
            var camp = EnemiesCamps.FirstOrDefault(c => c.Units.Contains(controller));
            if (camp != null)
            {
                camp.Units.Remove(controller);
            }
        }
    }
}


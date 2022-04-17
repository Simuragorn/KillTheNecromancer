using Assets.Scripts.Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsCamp : MonoBehaviour
{
    [SerializeField] private int enemiesCount;
    [SerializeField] private Transform spawnCenter;
    [SerializeField] private UnitController unitPrefab;
    [SerializeField] private int minXSpawn;
    [SerializeField] private int maxXSpawn;
    [SerializeField] private int minYSpawn;
    [SerializeField] private int maxYSpawn;

    private bool isEnemy => !UnitEnumExtensions.IsPlayerUnit((UnitEnum)unitPrefab.Unit.Id);

    [SerializeField] public List<UnitController> Units { private set; get; }

    public void EnemyUnitSpotted(Vector2 targetPosition)
    {
        AllMoveTo(targetPosition, MoveTypeEnum.ToEnemy);
    }

    private void Start()
    {
        if (isEnemy)
        {
            EnemyUnitsManager.Instance.EnemiesCamps.Add(this);
        }

        SpawnUnits();
    }

    private void SpawnUnits()
    {
        Units = new List<UnitController>();
        for (int i = 0; i < enemiesCount; i++)
        {
            float randomX = Random.Range(minXSpawn, maxXSpawn);
            float randomY = Random.Range(minYSpawn, maxYSpawn);
            Vector2 spawnPos = new Vector2(spawnCenter.position.x + randomX, spawnCenter.position.y + randomY);
            var unit = Instantiate(unitPrefab, spawnPos, Quaternion.identity, spawnCenter);
            unit.Init(this);
            Units.Add(unit);
        }
    }
    private void AllMoveTo(Vector2 targetPosition, MoveTypeEnum moveType)
    {
        Units.ForEach(u =>
        {
            if (u.IsFreeWay)
            {
                u.MoveTo(targetPosition, moveType);
            }
        });
    }

    private void OnDestroy()
    {
        if (isEnemy)
        {
            if (EnemyUnitsManager.Instance.EnemiesCamps.Contains(this))
            {
                EnemyUnitsManager.Instance.EnemiesCamps.Remove(this);
            }
        }
    }
}

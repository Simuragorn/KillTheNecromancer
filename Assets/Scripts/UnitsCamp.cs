using Assets.Scripts.Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsCamp : MonoBehaviour
{
    [SerializeField] private int enemiesCount;
    [SerializeField] private Transform spawnCenter;
    [SerializeField] private BaseUnitController unitPrefab;
    [SerializeField] private int minXSpawn;
    [SerializeField] private int maxXSpawn;
    [SerializeField] private int minYSpawn;
    [SerializeField] private int maxYSpawn;
    [SerializeField] private int destroyReward;
    [SerializeField] private float allMoveToOrderDelayInSeconds;
    [SerializeField] private float campRadius;
    public float CampRadius => campRadius;
    public int DestroyReward => destroyReward;

    private bool isEnemy => !unitPrefab.IsPlayerUnit;
    private bool isAllMoveOrderBusy;

    public List<BaseUnitController> Units { private set; get; }

    public void EnemyUnitSpotted(Vector2 targetPosition)
    {
        if (!isAllMoveOrderBusy)
            AllMoveTo(targetPosition, MoveTypeEnum.ToEnemy);
    }

    public void RemoveUnit(BaseUnitController unit)
    {
        if (Units.Contains(unit))
        {
            Units.Remove(unit);
            if (Units.Count == 0)
            {
                Destroy(this);
            }
        }
    }

    private void Start()
    {
        if (isEnemy)
        {
            EnemyUnitsManager.Instance.AddCamp(this);
        }

        SpawnUnits();
    }

    private void SpawnUnits()
    {
        Units = new List<BaseUnitController>();
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
        StartCoroutine(StartAllMoveToOrderDelay());
        Units.ForEach(u =>
        {
            if (u.CurrentAction != UnitActionEnum.Attacking)
                u.MoveTo(targetPosition, moveType);
        });
    }

    private IEnumerator StartAllMoveToOrderDelay()
    {
        isAllMoveOrderBusy = true;
        yield return new WaitForSeconds(allMoveToOrderDelayInSeconds);
        isAllMoveOrderBusy = false;
    }

    private void OnDestroy()
    {
        if (isEnemy)
        {
            EnemyUnitsManager.Instance.RemoveCamp(this);
        }
    }
}

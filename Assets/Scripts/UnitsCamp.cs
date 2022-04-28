using Assets.Scripts.Constants;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsCamp : MonoBehaviour
{
    [SerializeField] private Transform spawnCenter;
    [SerializeField] private List<BaseUnitController> unitPrefabs;
    [SerializeField] private float campBalance;
    [SerializeField] private int minXSpawn;
    [SerializeField] private int maxXSpawn;
    [SerializeField] private int minYSpawn;
    [SerializeField] private int maxYSpawn;
    [SerializeField] private int destroyReward;
    [SerializeField] private float allMoveToOrderDelayInSeconds;
    [SerializeField] private float campRadius;
    public float CampRadius => campRadius;
    public int DestroyReward => destroyReward;

    private bool isEnemy;
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
        isEnemy = !unitPrefabs.First().IsPlayerUnit;
        if (isEnemy)
        {
            EnemyUnitsManager.Instance.AddCamp(this);
        }

        SpawnUnits();
    }

    private void SpawnUnits()
    {
        Units = new List<BaseUnitController>();
        List<UnitEnum> unitIds = unitPrefabs.Select(u => u.UnitId).ToList();
        List<Unit> unitsData = unitIds.Select(id => GameManager.Instance.GetUnitById(id)).ToList();
        float minUnitCost = unitsData.Min(u => u.Cost);

        while (campBalance >= minUnitCost)
        {
            SpawnUnit(unitsData);
        }
    }

    private void SpawnUnit(List<Unit> unitsData)
    {
        List<Unit> suitableUnitsData = unitsData.Where(u => u.Cost <= campBalance).ToList();
        if (!suitableUnitsData.Any())
            return;
        Unit unitData = suitableUnitsData[Random.Range(0, suitableUnitsData.Count)];
        BaseUnitController unitPrefab = unitPrefabs.First(u => u.UnitId == unitData.Id);
        float randomX = Random.Range(minXSpawn, maxXSpawn);
        float randomY = Random.Range(minYSpawn, maxYSpawn);
        Vector2 spawnPos = new Vector2(spawnCenter.position.x + randomX, spawnCenter.position.y + randomY);
        BaseUnitController unit = Instantiate(unitPrefab, spawnPos, Quaternion.identity, spawnCenter);
        unit.Init(this);
        campBalance -= unitData.Cost;
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

using Assets.Scripts.Constants;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsCamp : MonoBehaviour
{
    [SerializeField] private Transform spawnCenter;
    [SerializeField] private List<RandomUnit> randomUnits;
    [SerializeField] private List<RequiredUnit> requiredUnits;
    [SerializeField] private float campBalance;
    [SerializeField] private int minXSpawn;
    [SerializeField] private int maxXSpawn;
    [SerializeField] private int minYSpawn;
    [SerializeField] private int maxYSpawn;
    [SerializeField] private int destroyReward;
    [SerializeField] private float allMoveToOrderDelayInSeconds;
    [SerializeField] private float campRadius;

    private List<UnitEnum> randomizerList;
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
        isEnemy = !randomUnits.First().Unit.IsPlayerUnit;
        if (isEnemy)
        {
            EnemyUnitsManager.Instance.AddCamp(this);
        }

        SpawnUnits();
    }

    private void InitRandomizerList()
    {
        randomizerList = new List<UnitEnum>();
        randomUnits.ForEach(u =>
        {
            for (int i = 0; i < u.Chance; i++)
                randomizerList.Add(u.Unit.UnitId);
        });
    }
    private void SpawnUnits()
    {
        Units = new List<BaseUnitController>();
        InitRandomizerList();

        SpawnRequiredUnits();

        List<UnitEnum> unitIds = randomUnits.Select(u => u.Unit.UnitId).ToList();
        List<(Unit Unit, int Chance)> unitsData = unitIds.Select(id =>
        {
            Unit unit = GameManager.Instance.GetUnitById(id);
            int chance = randomUnits.First(u => u.Unit.UnitId == id).Chance;
            return (unit, chance);
        }).ToList();
        float minUnitCost = unitsData.Min(u => u.Unit.Cost);

        while (campBalance >= minUnitCost)
            SpawnRandomUnit(unitsData);
    }

    private void SpawnRequiredUnits()
    {
        if (requiredUnits == null)
            return;
        requiredUnits.ForEach(u =>
        {
            Unit unit = GameManager.Instance.GetUnitById(u.Unit);
            for (int i = 0; i < u.Quantity; ++i)
                SpawnUnit(unit);
        });
    }

    private void SpawnRandomUnit(List<(Unit Unit, int Chance)> unitsData)
    {
        List<(Unit Unit, int Chance)> suitableUnitsData = unitsData.Where(u => u.Unit.Cost <= campBalance).ToList();
        if (!suitableUnitsData.Any())
            return;

        UnitEnum unitId = GetRandomUnitId();
        Unit unitData = suitableUnitsData.First(u => u.Unit.Id == unitId).Unit;
        SpawnUnit(unitData);
    }

    private UnitEnum GetRandomUnitId()
    {
        int id = Random.Range(1, randomizerList.Count);
        return randomizerList[id];
    }
    private void SpawnUnit(Unit unitData)
    {
        BaseUnitController unitPrefab = randomUnits.First(u => u.Unit.UnitId == unitData.Id).Unit;
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

[System.Serializable]
public class RequiredUnit
{
    public UnitEnum Unit;
    public int Quantity;
}

[System.Serializable]
public class RandomUnit
{
    public BaseUnitController Unit;
    public int Chance;
}
using Assets.Scripts.Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitsManager : MonoBehaviour
{
    public static PlayerUnitsManager Instance { private set; get; }
    public Dictionary<GameObject, UnitController> PlayerUnits { private set; get; }
    [SerializeField] private UnitDB _unitDB;
    [SerializeField] private UnitController unitPrefab;
    [SerializeField] private int playerMoney;
    [SerializeField] GameObject unitsRoot;
    private Camera camera;
    private Unit unit;

    public Unit GetUnitById(UnitEnum unitId)
    {
        return _unitDB.GetUnitById((int)unitId);
    }

    public void RemoveUnit(GameObject unit)
    {
        if (PlayerUnits.ContainsKey(unit))
        {
            PlayerUnits.Remove(unit);
        }
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        camera = Camera.main;
        PlayerUnits = new Dictionary<GameObject, UnitController>();
        unit = GetUnitById(unitPrefab.UnitId);
    }


    private void Update()
    {
        CheckSpawnUnit();
    }

    private void CheckSpawnUnit()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0))
        {
            if (playerMoney > unit.Cost)
            {
                Vector2 spawnPosition = camera.ScreenToWorldPoint(Input.mousePosition);
                SpawnUnit(spawnPosition);

                playerMoney -= unit.Cost;
            }
        }
    }

    private void SpawnUnit(Vector2 spawnPosition)
    {
        var unit = Instantiate(unitPrefab, spawnPosition, Quaternion.identity, unitsRoot.transform);
        unit.Init(null);
    }
}

using Assets.Scripts.Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitsManager : MonoBehaviour
{
    public static PlayerUnitsManager Instance { private set; get; }
    [SerializeField] private UnitDB _unitDB;
    [SerializeField] private UnitController unit;
    [SerializeField] private int playerMoney;
    [SerializeField] GameObject unitsRoot;
    private Camera camera;

    public Unit GetUnitById(UnitEnum unitId)
    {
        return _unitDB.GetUnitById((int)unitId);
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
    }


    private void Update()
    {
        CheckSpawnUnit();
    }

    private void CheckSpawnUnit()
    {
        if (Input.GetKey(KeyCode.S) && Input.GetMouseButtonDown(0))
        {
            if (playerMoney > 5)
            {
                Vector2 spawnPosition = camera.ScreenToWorldPoint(Input.mousePosition);
                SpawnUnit(spawnPosition);
            }
        }
    }

    private void SpawnUnit(Vector2 spawnPosition)
    {
        Instantiate(unit, spawnPosition, Quaternion.identity, unitsRoot.transform);
    }
}

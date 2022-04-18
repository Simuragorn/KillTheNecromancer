using Assets.Scripts.Constants;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Text moneyText;

    public Unit GetUnitById(UnitEnum unitId)
    {
        return _unitDB.GetUnitById((int)unitId);
    }

    public void RemoveUnit(GameObject unitForRemove)
    {
        if (PlayerUnits.ContainsKey(unitForRemove))
        {
            PlayerUnits.Remove(unitForRemove);
        }
        if (!PlayerUnits.Any() && playerMoney < unit.Cost)
        {
            GameManager.Instance.Defeat();
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
        UpdateMoneyText();
    }


    private void Update()
    {
        CheckSpawnUnit();
    }

    private void CheckSpawnUnit()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0))
        {
            if (playerMoney >= unit.Cost)
            {
                Vector2 spawnPosition = camera.ScreenToWorldPoint(Input.mousePosition);
                SpawnUnit(spawnPosition);

                playerMoney -= unit.Cost;
                UpdateMoneyText();
            }
        }
    }

    private void UpdateMoneyText()
    {
        moneyText.text = $"{playerMoney} coins";
    }

    private void SpawnUnit(Vector2 spawnPosition)
    {
        var unit = Instantiate(unitPrefab, spawnPosition, Quaternion.identity, unitsRoot.transform);
        unit.Init(null);
    }
}
